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
End Class
Partial Class ContactsContext
    Property Contacts As DbSet(Of Contact)
End Class