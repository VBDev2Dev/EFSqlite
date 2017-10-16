Imports System.ComponentModel
Imports System.Data.Entity
Public Class Form1

    Dim db As ContactsContext


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db = New ContactsContext
        db.Contacts.Load
        ContactBindingSource.DataSource = db.Contacts.Local.ToBindingList


    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        DataGridView1.EndEdit()
        ContactBindingSource.EndEdit()
        db.SaveChanges()
    End Sub
End Class
