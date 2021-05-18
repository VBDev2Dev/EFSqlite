Public Class frmImage
    Sub New()
        MyBase.New
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Sub New(Image As Image)
        MyClass.New
        pbImage.Image = Image
    End Sub

    Private Sub pbImage_Click(sender As Object, e As EventArgs) Handles pbImage.Click
        Dim szMode As Integer = pbImage.SizeMode + 1
        If szMode > PictureBoxSizeMode.Zoom Then szMode = 0
        pbImage.SizeMode = szMode
    End Sub
End Class