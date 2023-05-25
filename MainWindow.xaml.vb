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
Imports Syncfusion.Windows.Controls.Input


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
        CHK_Weapon_Red_Cat.ItemsSource = GlobalVariables.Types.Categories
        CHK_Weapon_Red_Use.ItemsSource = GlobalVariables.Types.Usages
        CHK_Weapon_Red_Val.ItemsSource = GlobalVariables.Types.Values
        CHK_Weapon_Red_Tag.ItemsSource = GlobalVariables.Types.Tags
        WeaponKits_SideArms.DataContext = GlobalVariables.SideArms


        'Dim WeaponKitsTypes As String() = New String() {"Red", "Purple", "Blue", "Green", "Yellow"}
        '' Create the drop-down menu
        'Dim dropDownMenu As New ContextMenu()
        '' Create menu items for each type
        'For Each type As String In WeaponKitsTypes
        '    Dim menuItem As New MenuItem()
        '    menuItem.Header = type
        '    dropDownMenu.Items.Add(menuItem)
        'Next

        'WeaponKit_ColorChoice.Content = dropDownMenu

        'Dim WeaponKitsTypes As String() = New String() {"Red", "Purple", "Blue", "Green", "Yellow"}
        'Dim WeaponKitsColorCodes As String() = New String() {"Red", "Purple", "Blue", "Green", "Yellow"}
        'For i As Integer = 0 To WeaponKitsTypes.Length - 1
        '    GlobalVariables.WeaponKits.Add(New GlobalVariables.WeaponKitsInfo(WeaponKitsTypes(i), False, WeaponKitsColorCodes(i)))
        'Next i


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

    Private Sub WeaponKit_ColorChoice_SelectionChanged(sender As Object, e As Windows.Controls.SelectionChangedEventArgs) Handles WeaponKit_ColorChoice.SelectionChanged
        Dim SelectedItems = e.AddedItems
        Dim SelectedText = SelectedItems.Item(0).Content
        Select Case SelectedText
            Case "Red Weapon Kit"
                SetGridBackgroundColor(G_Weapon_Red, 131, 14, 14, 30)
            Case "Purple Weapon Kit"
                SetGridBackgroundColor(G_Weapon_Red, 238, 51, 229, 20)
            Case "Blue Weapon Kit"
                SetGridBackgroundColor(G_Weapon_Red, 48, 67, 225, 20)
            Case "Green Weapon Kit"
                SetGridBackgroundColor(G_Weapon_Red, 80, 255, 71, 20)
            Case "Yellow Weapon Kit"
                SetGridBackgroundColor(G_Weapon_Red, 255, 243, 0, 30)
            Case Else
                SetGridBackgroundColor(G_Weapon_Red, 83, 83, 83, 20)
        End Select
        Dim xx = ""
    End Sub
    Public Sub SetGridBackgroundColor(grid As Grid, r As Byte, g As Byte, b As Byte, alpha As Byte)
        Dim color As Color = Color.FromArgb(alpha, r, g, b)
        Dim brush As New SolidColorBrush(color)
        grid.Background = brush
    End Sub

    Private Async Sub ButtonAdv_Click(sender As Object, e As RoutedEventArgs)
        Dim results As String() = Await FileSelectionHelper.SelectMultipleFilesAsync()
        Dim foundTypes As List(Of String)
        For Each resultPath As String In results
            foundTypes = New List(Of String)()
            foundTypes = FileSelectionHelper.GetUniqueClassnamesAndVariants(resultPath) '.Add(FileSelectionHelper.GetUniqueClassnamesAndVariants(resultPath))
            foundTypes = FileSelectionHelper.RemoveDuplicates(foundTypes)
            AddMissingTypes(foundTypes, GlobalVariables.SideArms)
        Next

        UpdateTextBoxWithStrings(WeaponKits_SideArms, GlobalVariables.SideArms)

    End Sub
    Sub AddMissingTypes(foundTypes As List(Of String), SideArms As ObservableCollection(Of String))
        For Each item In foundTypes
            If Not SideArms.Contains(item) Then
                SideArms.Add(item)
            End If
        Next
    End Sub
    Public Sub UpdateTextBoxWithStrings(textBox As SfTextBoxExt, strings As ObservableCollection(Of String))
        textBox.Clear() ' Clear the existing contents of the textbox

        For Each str As String In strings
            textBox.Text += str & Environment.NewLine ' Append each string followed by a new line character
        Next
    End Sub
End Class

