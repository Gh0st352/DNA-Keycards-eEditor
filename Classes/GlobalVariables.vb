Imports System.Collections.ObjectModel

Namespace Classes
    Public Class GlobalVariables
        Public Shared SideArms As New ObservableCollection(Of String)()
        Public Class WeaponKitsInfo
            Public Sub New(label As String, selected As Boolean, color As String)
                Me.Label = label
                Me.Selected = selected
                Me.Color = color
            End Sub

            Public Property Label As String
            Public Property Selected As Boolean
            Public Property Color As String

        End Class
        Public Shared WeaponKits As New ObservableCollection(Of WeaponKitsInfo)()
        Public Class Types
            Public Class WeaponKitSettings
                Public Sub New(type As String, flags() As String, categories() As String, values() As String, usages() As String, tags() As String)
                    Me.Type = type
                    Me.Flags = flags
                    Me.Categories = categories
                    Me.Values = values
                    Me.Usages = usages
                    Me.Tags = tags
                End Sub

                Public Property Type As String

                Public Property Flags As String()
                Public Property Categories As String()
                Public Property Values As String()
                Public Property Usages As String()
                Public Property Tags As String()
            End Class
            Public Shared WeaponKits As New ObservableCollection(Of WeaponKitSettings)()
            Public Class CategoryInfo
                Private _Checked As Boolean
                Private _Name As String

                Public Property Checked As Boolean
                    Get
                        Return _Checked
                    End Get
                    Set
                        _Checked = Value
                    End Set
                End Property

                Public Property Name As String
                    Get
                        Return _Name
                    End Get
                    Set
                        _Name = Value
                    End Set
                End Property
            End Class
            Public Shared Categories As New ObservableCollection(Of CategoryInfo)()
            Public Class UsageInfo
                Private _Checked As Boolean
                Private _Name As String

                Public Property Checked As Boolean
                    Get
                        Return _Checked
                    End Get
                    Set
                        _Checked = Value
                    End Set
                End Property

                Public Property Name As String
                    Get
                        Return _Name
                    End Get
                    Set
                        _Name = Value
                    End Set
                End Property
            End Class
            Public Shared Usages As New ObservableCollection(Of UsageInfo)()
            Public Class TagInfo
                Private _Checked As Boolean
                Private _Name As String

                Public Property Checked As Boolean
                    Get
                        Return _Checked
                    End Get
                    Set
                        _Checked = Value
                    End Set
                End Property

                Public Property Name As String
                    Get
                        Return _Name
                    End Get
                    Set
                        _Name = Value
                    End Set
                End Property
            End Class
            Public Shared Tags As New ObservableCollection(Of TagInfo)()
            Public Class ValueInfo
                Private _Checked As Boolean
                Private _Name As String

                Public Property Checked As Boolean
                    Get
                        Return _Checked
                    End Get
                    Set
                        _Checked = Value
                    End Set
                End Property

                Public Property Name As String
                    Get
                        Return _Name
                    End Get
                    Set
                        _Name = Value
                    End Set
                End Property
            End Class
            Public Shared Values As New ObservableCollection(Of ValueInfo)()
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
                Private _typename As String
                Private _nominal As String
                Private _lifetime As String
                Private _restock As String
                Private _min As String
                Private _quantmin As String
                Private _quantmax As String
                Private _cost As String
                Private _flags As String
                Private _category As String
                Private _usage As String
                Private _tag As String
                Private _value As String

                Public Property typename As String
                    Get
                        Return _typename
                    End Get
                    Set
                        _typename = Value
                    End Set
                End Property

                Public Property nominal As String
                    Get
                        Return _nominal
                    End Get
                    Set
                        _nominal = Value
                    End Set
                End Property

                Public Property lifetime As String
                    Get
                        Return _lifetime
                    End Get
                    Set
                        _lifetime = Value
                    End Set
                End Property

                Public Property restock As String
                    Get
                        Return _restock
                    End Get
                    Set
                        _restock = Value
                    End Set
                End Property

                Public Property min As String
                    Get
                        Return _min
                    End Get
                    Set
                        _min = Value
                    End Set
                End Property

                Public Property quantmin As String
                    Get
                        Return _quantmin
                    End Get
                    Set
                        _quantmin = Value
                    End Set
                End Property

                Public Property quantmax As String
                    Get
                        Return _quantmax
                    End Get
                    Set
                        _quantmax = Value
                    End Set
                End Property

                Public Property cost As String
                    Get
                        Return _cost
                    End Get
                    Set
                        _cost = Value
                    End Set
                End Property

                Public Property flags As String
                    Get
                        Return _flags
                    End Get
                    Set
                        _flags = Value
                    End Set
                End Property

                Public Property category As String
                    Get
                        Return _category
                    End Get
                    Set
                        _category = Value
                    End Set
                End Property

                Public Property usage As String
                    Get
                        Return _usage
                    End Get
                    Set
                        _usage = Value
                    End Set
                End Property

                Public Property tag As String
                    Get
                        Return _tag
                    End Get
                    Set
                        _tag = Value
                    End Set
                End Property

                Public Property value As String
                    Get
                        Return _value
                    End Get
                    Set
                        _value = Value
                    End Set
                End Property
            End Class
            Public Shared Types As New ObservableCollection(Of TypeInfo)()
        End Class
    End Class
End Namespace