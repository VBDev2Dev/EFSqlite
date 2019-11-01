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
End Class
