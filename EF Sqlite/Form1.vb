Imports System.ComponentModel
Imports System.Data.Entity
Imports Serilog

Public Class Form1

    Dim db As ContactsContext


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db = New ContactsContext
        db.Database.Log = Sub(m) Log.Logger.Verbose(m)
        db.Contacts.Load
        ContactBindingSource.DataSource = db.Contacts.Local.ToBindingList


    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        DataGridView1.EndEdit()
        ContactBindingSource.EndEdit()
        db.SaveChanges()
    End Sub
    Dim rand As New Random
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click


        Dim tmps = Enumerable.Range(0, 3).Select(Function(n) New Contact With {.Birthdate = Now.AddSeconds(rand.Next(-599, 0)), .EmailAddress = "123@123.com", .Name = n.ToString}).ToList
        Dim EntriesOutput = Sub(tmpData As IEnumerable(Of Contact), Message As String)
                                Dim entries = tmpData.Select(Function(d) New With {.Data = d, .Entry = db.Entry(d)}).ToList
                                Log.Logger.Information(Message)

                                For Each entry In entries
                                    Log.Logger.Information("{Data} State: {State}", entry.Data, entry.Entry.State)
                                Next
                            End Sub
        db.Contacts.AddRange(tmps)
        EntriesOutput(tmps, "After Add")
        db.SaveChanges()
        EntriesOutput(tmps, "After Save")
        Dim id As Long = tmps(0).ContactID
        Dim toDelete = db.Contacts.Find(id)
        db.Contacts.Remove(toDelete)
        EntriesOutput(tmps, "After Remove")
        db.SaveChanges()
        EntriesOutput(tmps, "After Save")



    End Sub
    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        If e.Button = MouseButtons.Right AndAlso e.ColumnIndex = ImageColumn.Index AndAlso Not DataGridView1.Rows(e.RowIndex).IsNewRow Then
            Dim record As Contact = DataGridView1.Rows(e.RowIndex).DataBoundItem
            If record.Image IsNot Nothing Then
                Dim imgViewer As New frmImage(record.Image)
                imgViewer.ShowDialog()
            End If

        End If
    End Sub

    Function GetImage() As (Image As Byte(), Success As Boolean)
        Dim bytes As Byte()
        Dim ofd As New OpenFileDialog With {
         .Title = "Select Image",
         .Filter = "Image files|*.jpg;*.png;*.gif;*.bmp|All files (*.*)|*.*",
         .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
         }
        Dim dlg = ofd.ShowDialog

        If dlg = DialogResult.OK Then
            Try
                bytes = My.Computer.FileSystem.ReadAllBytes(ofd.FileName)
                Using mem As New IO.MemoryStream(bytes)
                    Using tmp As Image = Image.FromStream(mem) ' make sure it is a valid image
                        Return (bytes, True)
                    End Using
                End Using

            Catch ex As Exception
                MessageBox.Show($"Could not load as an image.{ex.Message}", "Error Loading Image", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        ElseIf dlg = DialogResult.Cancel Then
            If MessageBox.Show($"Clear Image?", "Unset Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then Return (Nothing, True)
        End If
        Return (Nothing, False)
    End Function

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.ColumnIndex = ImageColumn.Index AndAlso Not DataGridView1.Rows(e.RowIndex).IsNewRow Then
            Dim record As Contact = DataGridView1.Rows(e.RowIndex).DataBoundItem
            Dim img = GetImage()
            If img.Success Then record.ImageBytes = img.Image
        End If
    End Sub
End Class
