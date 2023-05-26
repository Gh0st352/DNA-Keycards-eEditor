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
Imports System.Windows.Forms
Imports Syncfusion.UI.Xaml.Grid
Imports System.Xml
Imports Syncfusion.UI.Xaml.TreeGrid
Imports Syncfusion.Data
Imports Syncfusion.UI.Xaml.TreeView
Imports Syncfusion.UI.Xaml.TreeView.Engine
Imports Syncfusion.Windows.Controls.Input
Imports Syncfusion.Windows.Tools.Controls


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

        seedItemSources()
        seedWeaponKitSettings()
        seedHandlers()
        'HandleGridControlEvents(G_Weapon_Red)

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
        Dim sizeMode As Syncfusion.SfSkinManager.SizeMode = Syncfusion.SfSkinManager.SizeMode.[Default]
        [Enum].TryParse(CurrentSizeModeProperty, sizeMode)
        If sizeMode <> Syncfusion.SfSkinManager.SizeMode.[Default] Then
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

    Private Sub WeaponKits_ClearSidearms_Click(sender As Object, e As RoutedEventArgs) Handles WeaponKits_ClearSidearms.Click
        GlobalVariables.SideArms.Clear()
        WeaponKits_SideArms.Clear()
    End Sub

    Private Async Sub WeaponKits_Generate_Click(sender As Object, e As RoutedEventArgs) Handles WeaponKits_Generate.Click
        Dim WeaponColorTier As String

        If IsNothing(WeaponKit_ColorChoice.SelectedItem) Then
            Windows.MessageBox.Show("Please select a color tier", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            WeaponColorTier = WeaponKit_ColorChoice.SelectedItem.Content
            WeaponColorTier = WeaponColorTier.Replace(" Weapon Kit", "")



            Dim weaponList As New ObservableCollection(Of GenerateConfigs.Weapons.WeaponInfo)()

            For Each type In GlobalVariables.Types.Types


                ''''''''FLAGS

                checkedAddArgs = Await determineIfAdd(type)

                If checkedAddArgs = True Then
                    Dim weapon As New GenerateConfigs.Weapons.WeaponInfo()
                    weapon.dna_Tier = WeaponColorTier
                    weapon.dna_TheChosenOne = type.typename
                    Dim exists As Boolean = StringExistsInCollection(type.typename, GlobalVariables.SideArms)
                    If exists Then
                        weapon.dna_WeaponCategory = "side"
                    End If


                    weaponList.Add(weapon)
                End If
            Next

            Select Case WeaponColorTier
                Case "Red"
                    GenerateConfigs.Weapons.RedWeaponKits = weaponList
                Case "Purple"

                Case "Blue"

                Case "Green"

                Case "Yellow"

            End Select
            UpdateGeneratedWeaponKits()
            'Await GenerateConfigs.Weapons.GenerateConfig(WeaponColorTier)
        End If

    End Sub
    Sub UpdateGeneratedWeaponKits()
        Tab_WeaponsGenerated.IsSelected = True


        'Red Update
        TV_WeaponKits_Generated_Red.Nodes.Clear()
        For Each WeaponSet_ As GenerateConfigs.Weapons.WeaponInfo In GenerateConfigs.Weapons.RedWeaponKits
            Dim tParentNode As New TreeViewNode

            tParentNode.Content = WeaponSet_.dna_TheChosenOne
            tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Tier : " + WeaponSet_.dna_Tier})
            tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_WeaponCategory : " + WeaponSet_.dna_WeaponCategory})
            tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_TheChosenOne : " + WeaponSet_.dna_TheChosenOne})
            tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Magazine : " + WeaponSet_.dna_Magazine})
            tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Ammunition : " + WeaponSet_.dna_Ammunition})
            tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_OpticType : " + WeaponSet_.dna_OpticType})
            tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Suppressor : " + WeaponSet_.dna_Suppressor})
            tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_UnderBarrel : " + WeaponSet_.dna_UnderBarrel})
            tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_ButtStock : " + WeaponSet_.dna_ButtStock})
            tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_HandGuard : " + WeaponSet_.dna_HandGuard})


            TV_WeaponKits_Generated_Red.Nodes.Add(tParentNode)
        Next




        Tab_Weapons.IsSelected = True
    End Sub
    Public Shared Sub seedWeaponKitSettings()
        Dim WeaponKitsTypes As String() = New String() {"Red", "Purple", "Blue", "Green", "Yellow"}
        For Each type As String In WeaponKitsTypes
            GlobalVariables.Types.WeaponKits.Add(New GlobalVariables.Types.WeaponKitSettings(type, Nothing, Nothing, Nothing, Nothing, Nothing))
        Next
    End Sub
    Public Sub seedItemSources()
        G_ImportedTypes.ItemsSource = GlobalVariables.Types.TypeFiles
        G_Types.ItemsSource = GlobalVariables.Types.Types
        CHK_Weapon_Red_Cat.ItemsSource = GlobalVariables.Types.Categories
        CHK_Weapon_Red_Use.ItemsSource = GlobalVariables.Types.Usages
        CHK_Weapon_Red_Val.ItemsSource = GlobalVariables.Types.Values
        CHK_Weapon_Red_Tag.ItemsSource = GlobalVariables.Types.Tags
        WeaponKits_SideArms.DataContext = GlobalVariables.SideArms
    End Sub
    Public Sub seedHandlers()
        AddHandler TV_WeaponKits_Generated_Red.ItemBeginEdit, AddressOf Event_BeginEdit_TV_WeaponKits_Generated_Red ' tabLocal.ModTree.ItemDropping, AddressOf Steam.TabOperations.modtree_Drop
    End Sub

    Async Sub Event_BeginEdit_TV_WeaponKits_Generated_Red(sender As Object, e As EventArgs)
        Dim Edited_ As SfTreeView = sender
        Dim eventInfo As TreeViewItemBeginEditEventArgs = e

        If IsNothing(eventInfo.Node.ParentNode) Then
            eventInfo.Cancel = True
        Else
            eventInfo.Cancel = False
        End If

    End Sub
    Async Function determineIfAdd(type As GlobalVariables.Types.TypeInfo) As Task(Of Boolean)
        'CHECK IF TYPE CONTAINS REQUIRED FLAGS FOR ADD
        '        'if selectedcats is not nothing then
        '        'for every selectedcat in selectedcats
        '        ' if type.section contains selected cat then
        '        'flag to add
        '        'else continue
        Dim selectedArgs


        If type.typename.ToLower.Contains("ammo") Or
           type.typename.ToLower.Contains("mag") Or
           type.typename.ToLower.Contains("stock") Or
           type.typename.ToLower.Contains("optic") Or
           type.typename.ToLower.Contains("muzzle") Or
           type.typename.ToLower.Contains("bttstck") Or
           type.typename.ToLower.Contains("Hndgrd") Or
           type.typename.ToLower.Contains("Suppressor") Or
           type.typename.ToLower.Contains("bayonet") Or
           type.typename.ToLower.Contains("compensator") Or
           type.typename.ToLower.Contains("goggles") Or
           type.typename.ToLower.Contains("light") Then
            Return False
        End If
        Dim counter = 0
        Dim RequiredCount = 0
        'Flags
        selectedArgs = Nothing
        selectedArgs = CHK_Weapon_Red_Flags.SelectedItems
        counter = 0
        RequiredCount = selectedArgs.Count
        If RequiredCount <> 0 Then
            For Each _arg As CheckListBoxItem In selectedArgs
                Select Case _arg.Content
                    Case "Count in Cargo"
                        If type.flags.Contains("count_in_cargo=1") Then
                            counter += 1
                        End If
                    Case "Count in Hoarder"
                        If type.flags.Contains("count_in_hoarder=1") Then
                            counter += 1
                        End If
                    Case "Count in Map"
                        If type.flags.Contains("count_in_map=1") Then
                            counter += 1
                        End If
                    Case "Count in Player"
                        If type.flags.Contains("count_in_player=1") Then
                            counter += 1
                        End If
                    Case "Crafted"
                        If type.flags.Contains("crafted=1") Then
                            counter += 1
                        End If
                    Case "Dynamic Event Loot"
                        If type.flags.Contains("deloot=1") Then
                            counter += 1
                        End If
                End Select
            Next
            If counter <> RequiredCount Then Return False
        End If

        'Categories
        selectedArgs = Nothing
        selectedArgs = CHK_Weapon_Red_Cat.SelectedItems
        counter = 0
        RequiredCount = selectedArgs.Count
        If RequiredCount <> 0 Then
            If type.category IsNot Nothing Then
                For Each _arg As GlobalVariables.Types.CategoryInfo In selectedArgs
                    If type.category.Contains(_arg.Name) Then
                        counter += 1
                    End If
                Next
            End If
            If counter <> RequiredCount Then Return False
        End If
        'Values
        selectedArgs = Nothing
        selectedArgs = CHK_Weapon_Red_Val.SelectedItems
        counter = 0
        RequiredCount = selectedArgs.Count
        If RequiredCount <> 0 Then
            If type.value IsNot Nothing Then
                For Each _arg As GlobalVariables.Types.ValueInfo In selectedArgs
                    If type.value.Contains(_arg.Name) Then
                        counter += 1
                    End If
                Next
            End If
            If counter <> RequiredCount Then Return False
        End If
        'Usages
        selectedArgs = Nothing
        selectedArgs = CHK_Weapon_Red_Use.SelectedItems
        counter = 0
        RequiredCount = selectedArgs.Count
        If RequiredCount <> 0 Then
            If type.usage IsNot Nothing Then
                For Each _arg As GlobalVariables.Types.UsageInfo In selectedArgs
                    If type.usage.Contains(_arg.Name) Then
                        counter += 1
                    End If
                Next
            End If
            If counter <> RequiredCount Then Return False
        End If
        'Tags
        selectedArgs = Nothing
        selectedArgs = CHK_Weapon_Red_Tag.SelectedItems
        counter = 0
        RequiredCount = selectedArgs.Count
        If RequiredCount <> 0 Then
            If type.tag IsNot Nothing Then
                For Each _arg As GlobalVariables.Types.TagInfo In selectedArgs
                    If type.tag.Contains(_arg.Name) Then
                        counter += 1
                    End If
                Next
            End If
            If counter <> RequiredCount Then Return False
        End If

        Return True
    End Function

    Function StringExistsInCollection(ByVal searchString As String, ByVal collection As ObservableCollection(Of String)) As Boolean
        Return collection.Any(Function(item) String.Equals(item, searchString, StringComparison.OrdinalIgnoreCase))
    End Function
    Public Class GenerateConfigs
        Public Class Weapons

            'Async Function GenerateConfig(t_tier As String) As Task
            '    ExportTypesCollection(GlobalVariables.Types.Types, t_tier)



            'End Function


            Public Class WeaponInfo
                Public Property dna_Tier As String
                Public Property dna_WeaponCategory As String = "main"
                Public Property dna_TheChosenOne As String = ""
                Public Property dna_Magazine As String = "random"
                Public Property dna_Ammunition As String = "random"
                Public Property dna_OpticType As String = "random"
                Public Property dna_Suppressor As String = "random"
                Public Property dna_UnderBarrel As String = "random"
                Public Property dna_ButtStock As String = "random"
                Public Property dna_HandGuard As String = "random"
                Public Property dna_Wrap As String = "random"
            End Class
            Public Shared RedWeaponKits As ObservableCollection(Of WeaponInfo)
            Public Shared PurpleWeaponKits As ObservableCollection(Of WeaponInfo)
            Public Shared BlueWeaponKits As ObservableCollection(Of WeaponInfo)
            Public Shared GreenWeaponKits As ObservableCollection(Of WeaponInfo)
            Public Shared YellowWeaponKits As ObservableCollection(Of WeaponInfo)

            'Public Shared Function StringExistsInCollection(ByVal searchString As String, ByVal collection As ObservableCollection(Of String)) As Boolean
            '    Return collection.Any(Function(item) String.Equals(item, searchString, StringComparison.OrdinalIgnoreCase))
            'End Function

            'Async Sub ExportTypesCollection(t_Types As ObservableCollection(Of GlobalVariables.Types.TypeInfo), t_tier As String)
            '    Dim weaponList As New List(Of WeaponInfo)()

            '    For Each type In t_Types


            '        ''''''''FLAGS

            '        checkedAddArgs = Await determineIfAdd(type)



            '        'Dim weapon As New WeaponInfo()
            '        'weapon.dna_Tier = t_tier
            '        'weapon.dna_TheChosenOne = type.typename
            '        'Dim exists As Boolean = StringExistsInCollection(type.typename, GlobalVariables.SideArms)
            '        'If exists Then
            '        '    weapon.dna_WeaponCategory = "side"
            '        'End If

            '        'CHECK IF TYPE CONTAINS REQUIRED FLAGS FOR ADD
            '        'if selectedcats is not nothing then
            '        'for every selectedcat in selectedcats
            '        ' if type.section contains selected cat then
            '        'flag to add
            '        'else continue

            '        If checkedAddArgs = True Then
            '            weaponList.Add(weapon)
            '        End If
            '    Next

            '    'Dim json = JsonConvert.SerializeObject(New With {Key .m_DNAConfig_Weapons = weaponList}, Formatting.Indented)
            '    '' Get the directory path of the executable
            '    'Dim executableDirectory = AppDomain.CurrentDomain.BaseDirectory

            '    '' Combine the directory path with the provided file name
            '    'Dim filePath = Path.Combine(executableDirectory, fileName)

            '    '' Write the JSON to the file
            '    'File.WriteAllText(filePath, json)
            '    ''File.WriteAllText(filePath, json)
            'End Sub
            'Public Shared Sub ExportTypesToJson(t_Types As ObservableCollection(Of GlobalVariables.Types.TypeInfo), fileName As String, t_tier As String)
            '    Dim weaponList As New List(Of WeaponInfo)()

            '    For Each type In t_Types
            '        Dim weapon As New WeaponInfo()
            '        weapon.dna_Tier = t_tier
            '        weapon.dna_TheChosenOne = type.typename
            '        Dim exists As Boolean = StringExistsInCollection(type.typename, GlobalVariables.SideArms)
            '        If exists Then
            '            weapon.dna_WeaponCategory = "side"
            '        End If

            '        'CHECK IF TYPE CONTAINS REQUIRED FLAGS FOR ADD
            '        'if selectedcats is not nothing then
            '        'for every selectedcat in selectedcats
            '        ' if type.section contains selected cat then
            '        'flag to add
            '        'else continue

            '        If checkedAddArgs = True Then
            '            weaponList.Add(weapon)
            '        End If
            '    Next

            '    Dim json = JsonConvert.SerializeObject(New With {Key .m_DNAConfig_Weapons = weaponList}, Formatting.Indented)
            '    ' Get the directory path of the executable
            '    Dim executableDirectory = AppDomain.CurrentDomain.BaseDirectory

            '    ' Combine the directory path with the provided file name
            '    Dim filePath = Path.Combine(executableDirectory, fileName)

            '    ' Write the JSON to the file
            '    File.WriteAllText(filePath, json)
            '    'File.WriteAllText(filePath, json)
            'End Sub
            'Public Function GetWeaponKitByCategory(category As String) As GlobalVariables.Types.WeaponKitSettings
            '    Dim selectedKit = GlobalVariables.WeaponKits.FirstOrDefault(Function(kit) kit.Label.Contains(category))
            '    Return selectedKit
            'End Function

        End Class
    End Class

End Class

