Imports System
Imports System.Collections.Generic
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


''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits ChromelessWindow
    Private currentVisualStyle As String
    Private currentSizeMode As String

    ''' <summary>
    ''' Property for Visual Style
    '''</summary>
    Public Property CurrentVisualStyleProperty As String
        Get
            Return currentVisualStyle
        End Get

        Set(ByVal value As String)
            currentVisualStyle = value
            OnVisualStyleChanged()
        End Set
    End Property

    ''' <summary>
    ''' Property for Size Mode
    '''</summary>
    Public Property CurrentSizeModeProperty As String
        Get
            Return currentSizeMode
        End Get

        Set(ByVal value As String)
            currentSizeMode = value
            OnSizeModeChanged()
        End Set
    End Property

    Public Sub New()

        InitializeComponent()
        AddHandler Me.Loaded, AddressOf OnLoaded
        G_ImportedTypes.ItemsSource = GlobalVariables.Types.TypeFiles
        G_Types.ItemsSource = GlobalVariables.Types.Types



        'GlobalVariables.Types.Types.Add(New GlobalVariables.Types.TypeInfo() With {
        '                   .category = "cat1,cat2",
        '                   .cost = "cost",
        '                   .flags = "flag1,flag2",
        '                   .lifetime = "lifetime",
        '                   .min = "min",
        '                   .nominal = "nominal",
        '                   .quantmax = "quantmax",
        '                   .quantmin = "quantmin",
        '                   .restock = "restock",
        '                   .tag = "tag1,tag2",
        '                   .typename = "typename",
        '                   .usage = "usage1,usage2",
        '                   .value = "value1,value2"})


    End Sub

    ''' <summary>
    ''' Method for onload
    '''</summary>
    Private Sub OnLoaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
        CurrentVisualStyleProperty = "Windows11Dark"
        CurrentSizeModeProperty = "Default"
    End Sub

    ''' <summary>
    ''' Method for Visual Style
    '''</summary>
    Private Sub OnVisualStyleChanged()
        Dim visualStyle As VisualStyles = VisualStyles.[Default]
        [Enum].TryParse(CurrentVisualStyleProperty, visualStyle)
        If visualStyle <> VisualStyles.[Default] Then
            SfSkinManager.ApplyStylesOnApplication = True
            SfSkinManager.SetVisualStyle(Me, visualStyle)
            SfSkinManager.ApplyStylesOnApplication = False
        End If
    End Sub

    ''' <summary>
    ''' Method for Size Mode
    '''</summary>
    Private Sub OnSizeModeChanged()
        Dim sizeMode As SizeMode = SizeMode.[Default]
        [Enum].TryParse(CurrentSizeModeProperty, sizeMode)
        If sizeMode <> SizeMode.[Default] Then
            SfSkinManager.ApplyStylesOnApplication = True
            SfSkinManager.SetSizeMode(Me, sizeMode)
            SfSkinManager.ApplyStylesOnApplication = False
        End If
    End Sub

    Private Async Sub B_ImportTypes_Click(sender As Object, e As RoutedEventArgs) Handles B_ImportTypes.Click
        Dim results As String() = Await FileSelectionHelper.SelectMultipleFilesAsync()
        For Each resultPath As String In results
            Dim temp As New GlobalVariables.Types.File(resultPath, IO.Path.GetFileName(resultPath), "types")
            GlobalVariables.Types.TypeFiles.Add(temp)
            Dim results2 = Await FileSelectionHelper.LoadDataFromXml(resultPath)
        Next
    End Sub
End Class

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
                        Case "tag"
                            _Type.tag = tempstr
                        Case "value"
                            _Type.value = tempstr
                        Case Else
                    End Select
                End If
            Next
            GlobalVariables.Types.Types.Add(_Type)
        Next
    End Function
End Class