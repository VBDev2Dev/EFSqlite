Imports System
Imports System.Data.Entity
Imports System.Linq
Imports SQLite.CodeFirst

Public Class ContactsContext
    Inherits DbContext

    ' Your context has been configured to use a 'ContactsContext' connection string from your application's 
    ' configuration file (App.config or Web.config). By default, this connection string targets the 
    ' 'EF_Sqlite.ContactsContext' database on your LocalDb instance. 
    ' 
    ' If you wish to target a different database and/or database provider, modify the 'ContactsContext' 
    ' connection string in the application configuration file.
    Public Sub New()
        MyBase.New("name=ContactsContext")
    End Sub
    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        MyBase.OnModelCreating(modelBuilder)
        Dim sqliteConnectionInitializer = New SqliteCreateDatabaseIfNotExists(Of ContactsContext)(modelBuilder)
        Database.SetInitializer(sqliteConnectionInitializer)

    End Sub
    ' Add a DbSet for each entity type that you want to include in your model. For more information 
    ' on configuring and using a Code First model, see http:'go.microsoft.com/fwlink/?LinkId=390109.
    ' Public Overridable Property MyEntities() As DbSet(Of MyEntity)

End Class

'Public Class MyEntity
'    Public Property Id() As Int32
'    Public Property Name() As String
'End Class
