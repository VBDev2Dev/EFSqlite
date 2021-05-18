Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Public Class Contact
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Property ContactID As Long
    <Required>
    Property Name As String
    <MaxLength(200)>
    <EmailAddress>
    <Required>
    Property EmailAddress As String
    <Required>
    Property Birthdate As Date = Now.Date.AddYears(-18)
    <NotMapped>
    ReadOnly Property Image As Image
        Get
            If If(ImageBytes?.Length, 0) = 0 Then Return Nothing
            Using mem As New IO.MemoryStream(ImageBytes)
                Using tmp = Image.FromStream(mem)
                    Return DirectCast(tmp.Clone(), Image)
                End Using
            End Using
        End Get
    End Property
    Property ImageBytes As Byte()

    Public Overrides Function ToString() As String
        Return $"ContactID: {ContactID}, Name: {Name}, EmailAddress: {EmailAddress}"
    End Function
End Class
Partial Class ContactsContext
    Property Contacts As DbSet(Of Contact)
End Class