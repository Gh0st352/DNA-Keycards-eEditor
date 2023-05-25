Namespace Classes
    Public Class GlobalVariables
        Public Class Types
            Public Class File
                Public Sub New(path As String, name As String, kind As String)
                    Me.Path = path
                    Me.Name = name
                    Me.Kind = kind
                End Sub

                Public Property Path As String
                Public Property Name As String
                Public Property Kind As String
            End Class

            Public Shared TypeFiles As New List(Of File)

        End Class
    End Class
End Namespace