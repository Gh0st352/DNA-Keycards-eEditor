Imports System.Collections.ObjectModel

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

            'Public Shared TypeFiles As New List(Of File)
            Public Shared TypeFiles As New ObservableCollection(Of File)()

            Public Class TypeInfo
                Public Property typename As String
                Public Property nominal As String
                Public Property lifetime As String
                Public Property restock As String
                Public Property min As String
                Public Property quantmin As String
                Public Property quantmax As String
                Public Property cost As String
                Public Property flags As String
                Public Property typename As String
                Public Property typename As String
                Public Property typename As String
                Public Property typename As String
                Public Property typename As String
                Public Property typename As String
                Public Property typename As String
                Public Property typename As String

            End Class
        End Class
    End Class
End Namespace