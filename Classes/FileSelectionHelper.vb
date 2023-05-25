Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports Syncfusion.SfSkinManager
Imports Syncfusion.Windows.Shared
Imports System.Threading.Tasks
Imports Microsoft.Win32
Imports System.Windows.Threading
Imports DNA_Keycard_Editor.Classes
Imports System.IO
Imports Syncfusion.UI.Xaml.Grid
Imports System.Xml
Imports Syncfusion.UI.Xaml.TreeGrid
Imports Syncfusion.Data

Namespace Classes
    Public Class FileSelectionHelper
        Public Shared Async Function SelectMultipleFilesAsync() As Task(Of String())
            Dim openFileDialog As New OpenFileDialog()
            openFileDialog.Multiselect = True

            Dim fileNames As String() = Await Application.Current.Dispatcher.InvokeAsync(Function()
                                                                                             If openFileDialog.ShowDialog() = True Then
                                                                                                 Return openFileDialog.FileNames
                                                                                             Else
                                                                                                 Return New String() {}
                                                                                             End If
                                                                                         End Function)
            Return fileNames
        End Function

        Public Shared Async Function LoadDataFromXml(_path As String) As Task(Of Task())
            ' Path to your XML file
            Dim xmlFilePath As String = _path

            ' Load the XML file
            Dim xmlDoc As New XmlDocument()
            xmlDoc.Load(xmlFilePath)

            Dim typeNodes As XmlNodeList = xmlDoc.SelectNodes("/types/type")

            For Each typeNode As XmlNode In typeNodes
                Dim _Type As New GlobalVariables.Types.TypeInfo()
                _Type.typename = typeNode.Attributes("name").Value
                If typeNode.SelectSingleNode("nominal") IsNot Nothing Then
                    If typeNode.SelectSingleNode("nominal").InnerText IsNot Nothing Then
                        _Type.nominal = Integer.Parse(typeNode.SelectSingleNode("nominal").InnerText)
                    End If
                End If
                If typeNode.SelectSingleNode("lifetime") IsNot Nothing Then
                    If typeNode.SelectSingleNode("lifetime").InnerText IsNot Nothing Then
                        _Type.lifetime = Integer.Parse(typeNode.SelectSingleNode("lifetime").InnerText)
                    End If
                End If
                If typeNode.SelectSingleNode("restock") IsNot Nothing Then
                    If typeNode.SelectSingleNode("restock").InnerText IsNot Nothing Then
                        _Type.restock = Integer.Parse(typeNode.SelectSingleNode("restock").InnerText)
                    End If
                End If
                If typeNode.SelectSingleNode("min") IsNot Nothing Then
                    If typeNode.SelectSingleNode("min").InnerText IsNot Nothing Then
                        _Type.min = Integer.Parse(typeNode.SelectSingleNode("min").InnerText)
                    End If
                End If
                If typeNode.SelectSingleNode("quantmin") IsNot Nothing Then
                    If typeNode.SelectSingleNode("quantmin").InnerText IsNot Nothing Then
                        _Type.quantmin = Integer.Parse(typeNode.SelectSingleNode("quantmin").InnerText)
                    End If
                End If
                If typeNode.SelectSingleNode("quantmax") IsNot Nothing Then
                    If typeNode.SelectSingleNode("quantmax").InnerText IsNot Nothing Then
                        _Type.quantmax = Integer.Parse(typeNode.SelectSingleNode("quantmax").InnerText)
                    End If
                End If
                If typeNode.SelectSingleNode("cost") IsNot Nothing Then
                    If typeNode.SelectSingleNode("cost").InnerText IsNot Nothing Then
                        _Type.cost = Integer.Parse(typeNode.SelectSingleNode("cost").InnerText)
                    End If
                End If
                If typeNode.SelectSingleNode("category") IsNot Nothing Then
                    If typeNode.SelectSingleNode("category").Attributes("name").Value IsNot Nothing Then
                        _Type.category = typeNode.SelectSingleNode("category").Attributes("name").Value
                        Dim exists As Boolean = IsStringExistsInCollection(GlobalVariables.Types.Categories, _Type.category)
                        If exists Then
                        Else
                            GlobalVariables.Types.Categories.Add(New GlobalVariables.Types.CategoryInfo() With {.Checked = False, .Name = _Type.category})
                        End If

                    End If
                End If
                Dim tempstr As String = ""

                Dim flagsNode As XmlNode = typeNode.SelectSingleNode("flags")
                If flagsNode IsNot Nothing Then
                    For Each attribute As XmlAttribute In flagsNode.Attributes
                        tempstr = tempstr + attribute.Name + "=" + attribute.Value + ","
                    Next
                    _Type.flags = tempstr
                End If

                Dim attributeArray As List(Of String) = New List(Of String)
                With attributeArray
                    .Add("usage")
                    .Add("tag")
                    .Add("value")
                End With
                For Each att_ In attributeArray
                    Dim usageNodes As XmlNodeList = typeNode.SelectNodes(att_)
                    If usageNodes IsNot Nothing Then
                        tempstr = ""
                        For Each Node As XmlNode In usageNodes
                            If Node.Attributes("name") IsNot Nothing Then
                                If Node.Attributes("name").Value IsNot Nothing Then
                                    tempstr = tempstr + Node.Attributes("name").Value + ","
                                End If
                            End If
                        Next
                        Select Case att_
                            Case "usage"
                                _Type.usage = tempstr
                                UpdateCollectionsList(tempstr, GlobalVariables.Types.Usages, att_)
                            Case "tag"
                                _Type.tag = tempstr
                                UpdateCollectionsList(tempstr, GlobalVariables.Types.Tags, att_)
                            Case "value"
                                _Type.value = tempstr
                                UpdateCollectionsList(tempstr, GlobalVariables.Types.Values, att_)
                            Case Else
                        End Select
                    End If
                Next
                GlobalVariables.Types.Types.Add(_Type)
            Next
        End Function
        'Public Shared Function IsStringExistsInCollection(Of T)(collection As ObservableCollection(Of T), searchString As String) As Boolean
        '    For Each item As T In collection
        '        Dim properties = item.GetType().GetProperties()
        '        For Each prop In properties
        '            If prop.PropertyType = GetType(String) AndAlso prop.GetValue(item)?.ToString() = searchString Then
        '                Return True
        '            End If
        '        Next
        '    Next
        '    Return False
        'End Function
        Public Shared Function IsStringExistsInCollection(Of T)(collection As ObservableCollection(Of T), searchString As String) As Boolean
            For Each item As T In collection
                Dim properties = item.GetType().GetProperties()
                For Each prop In properties
                    If prop.PropertyType = GetType(String) AndAlso prop.GetValue(item)?.ToString() = searchString Then
                        Return True
                    End If
                Next
            Next
            Return False
        End Function
        Public Shared Function SeparateString(ByVal inputString As String) As String()
            Dim separator As String = ","
            Dim separatedArray As String() = inputString.Split({separator}, StringSplitOptions.RemoveEmptyEntries)
            Return separatedArray
        End Function
        Public Shared Sub UpdateCollectionsList(Of T)(searchString As String, CollectionObject As ObservableCollection(Of T), type As String)
            Dim tCollectionObject As ObservableCollection(Of T) = CollectionObject
            Dim TempStringArr As String() = SeparateString(searchString)
            For Each _str In TempStringArr
                Dim exists As Boolean = IsStringExistsInCollection(Of T)(tCollectionObject, _str)
                If exists Then
                    Continue For
                Else
                    AddOptionToCollection(_str, type)
                End If
            Next
        End Sub
        Public Shared Sub AddOptionToCollection(str As String, type As String)
            Select Case type
                Case "usage"
                    GlobalVariables.Types.Usages.Add(New GlobalVariables.Types.UsageInfo() With {.Checked = False, .Name = _str})
                Case "tag"
                    GlobalVariables.Types.Tags.Add(New GlobalVariables.Types.TagInfo() With {.Checked = False, .Name = _str})
                Case "value"
                    GlobalVariables.Types.Values.Add(New GlobalVariables.Types.ValueInfo() With {.Checked = False, .Name = _str})
                Case Else
            End Select
        End Sub
        'Public Shared Sub UpdateCollectionsList(searchString, CollectionObject, type)
        '    Dim tCollectionObject As ObservableCollection(Of T) = CollectionObject
        '    Dim TempStringArr As String() = SeparateString(searchString)
        '    For Each _str In TempStringArr
        '        Dim exists As Boolean = IsStringExistsInCollection(tCollectionObject, _str)
        '        If exists Then
        '            Continue For
        '        Else
        '            Select Case type
        '                Case "usage"
        '                    CollectionObject.Add(New GlobalVariables.Types.UsageInfo() With {.Checked = False, .Name = _str})
        '                Case "tag"
        '                    CollectionObject.Add(New GlobalVariables.Types.TagInfo() With {.Checked = False, .Name = _str})
        '                Case "value"
        '                    CollectionObject.Add(New GlobalVariables.Types.ValueInfo() With {.Checked = False, .Name = _str})
        '                Case Else
        '            End Select
        '        End If
        '    Next
        'End Sub
    End Class
End Namespace