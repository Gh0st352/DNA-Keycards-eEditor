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
Imports Newtonsoft.Json
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

    Private Sub Kit_ColorChoice_SelectionChanged(sender As Object, e As Windows.Controls.SelectionChangedEventArgs) _
        Handles Kit_ColorChoice.SelectionChanged
        Dim SelectedItems = e.AddedItems
        Dim SelectedText = SelectedItems.Item(0).Content
        Select Case SelectedText
            Case "Red Kit"
                SetGridBackgroundColor(G_KitType, 131, 14, 14, 30)
                SetGridBackgroundColor(G_ClothesSettings, 131, 14, 14, 30)
            Case "Purple Kit"
                SetGridBackgroundColor(G_KitType, 238, 51, 229, 20)
                SetGridBackgroundColor(G_ClothesSettings, 238, 51, 229, 20)
            Case "Blue Kit"
                SetGridBackgroundColor(G_KitType, 48, 67, 225, 20)
                SetGridBackgroundColor(G_ClothesSettings, 48, 67, 225, 20)
            Case "Green Kit"
                SetGridBackgroundColor(G_KitType, 80, 255, 71, 20)
                SetGridBackgroundColor(G_ClothesSettings, 80, 255, 71, 20)
            Case "Yellow Kit"
                SetGridBackgroundColor(G_KitType, 255, 243, 0, 30)
                SetGridBackgroundColor(G_ClothesSettings, 255, 243, 0, 30)
            Case Else
                SetGridBackgroundColor(G_KitType, 83, 83, 83, 20)
                SetGridBackgroundColor(G_ClothesSettings, 83, 83, 83, 20)
        End Select
        Dim xx = ""
    End Sub

    Public Sub SetGridBackgroundColor(grid As Grid, r As Byte, g As Byte, b As Byte, alpha As Byte)
        Dim color As Color = Color.FromArgb(alpha, r, g, b)
        Dim brush As New SolidColorBrush(color)
        grid.Background = brush
    End Sub

    Private Async Sub DragImportExpansionMarket(results As ObservableCollection(Of String), sender As SfTextBoxExt)
        Dim foundTypes As List(Of String)
        For Each resultPath As String In results
            foundTypes = New List(Of String)()
            foundTypes = FileSelectionHelper.GetUniqueClassnamesAndVariants(resultPath) _
            '.Add(FileSelectionHelper.GetUniqueClassnamesAndVariants(resultPath))
            foundTypes = FileSelectionHelper.RemoveDuplicates(foundTypes)


            Select Case True

                ''Sidearms
                'Case sender Is Kits_SideArms
                '    AddMissingTypes(foundTypes, GlobalVariables.SideArms)
                '    UpdateTextBoxWithStrings(Kits_SideArms, GlobalVariables.SideArms)

                ''Restricted
                'Case sender Is Kits_Restricted
                '    AddMissingTypes(foundTypes, GlobalVariables.RestrictedTypes)
                '    UpdateTextBoxWithStrings(Kits_Restricted, GlobalVariables.RestrictedTypes)

                'Clothing Market

                'dna_Helm
                Case sender Is Clothes_Helmets
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Helmets)
                    UpdateTextBoxWithStrings(Clothes_Helmets, GlobalVariables.ClothingMarket.Helmets)
                    'dna_Shirt
                Case sender Is Clothes_Shirts
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Shirts)
                    UpdateTextBoxWithStrings(Clothes_Shirts, GlobalVariables.ClothingMarket.Shirts)
                    'dna_Vest
                Case sender Is Clothes_Vests
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Vests)
                    UpdateTextBoxWithStrings(Clothes_Vests, GlobalVariables.ClothingMarket.Vests)
                    'dna_Pants
                Case sender Is Clothes_Pants
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Pants)
                    UpdateTextBoxWithStrings(Clothes_Pants, GlobalVariables.ClothingMarket.Pants)
                    'dna_Shoes
                Case sender Is Clothes_Shoes
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Shoes)
                    UpdateTextBoxWithStrings(Clothes_Shoes, GlobalVariables.ClothingMarket.Shoes)
                    'dna_Backpack
                Case sender Is Clothes_Backpacks
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Backpacks)
                    UpdateTextBoxWithStrings(Clothes_Backpacks, GlobalVariables.ClothingMarket.Backpacks)
                    'dna_Gloves
                Case sender Is Clothes_Gloves
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Gloves)
                    UpdateTextBoxWithStrings(Clothes_Gloves, GlobalVariables.ClothingMarket.Gloves)
                    'dna_Belt
                Case sender Is Clothes_Belts
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Belts)
                    UpdateTextBoxWithStrings(Clothes_Belts, GlobalVariables.ClothingMarket.Belts)
                    'dna_Facewear
                Case sender Is Clothes_Facewear
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Facewears)
                    UpdateTextBoxWithStrings(Clothes_Facewear, GlobalVariables.ClothingMarket.Facewears)
                    'dna_Eyewear
                Case sender Is Clothes_Eyewear
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Eyewears)
                    UpdateTextBoxWithStrings(Clothes_Eyewear, GlobalVariables.ClothingMarket.Eyewears)
                    'dna_Armband
                Case sender Is Clothes_Armbands
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Armbands)
                    UpdateTextBoxWithStrings(Clothes_Armbands, GlobalVariables.ClothingMarket.Armbands)
            End Select

        Next
    End Sub

    Private Async Sub ButtonImportExpansionMarketClick(sender As Object, e As RoutedEventArgs) _
        Handles Kits_ImportRestricted.Click, Clothes_Helmets_Import.Click, Clothes_Pants_Import.Click,
                Clothes_Gloves_Import.Click, Clothes_Eyewear_Import.Click, Clothes_Shirts_Import.Click,
                Clothes_Shoes_Import.Click, Clothes_Belts_Import.Click, Clothes_Armbands_Import.Click,
                Clothes_Vests_Import.Click, Clothes_Backpacks_Import.Click, Clothes_Facewear_Import.Click
        Dim results As String() = Await FileSelectionHelper.SelectMultipleFilesAsync()
        Dim foundTypes As List(Of String)
        For Each resultPath As String In results
            foundTypes = New List(Of String)()
            foundTypes = FileSelectionHelper.GetUniqueClassnamesAndVariants(resultPath) _
            '.Add(FileSelectionHelper.GetUniqueClassnamesAndVariants(resultPath))
            foundTypes = FileSelectionHelper.RemoveDuplicates(foundTypes)


            Select Case True

                'Sidearms
                Case sender Is Kits_ImportSidearm
                    AddMissingTypes(foundTypes, GlobalVariables.SideArms)
                    UpdateTextBoxWithStrings(Kits_SideArms, GlobalVariables.SideArms)

                    'Restricted
                Case sender Is Kits_ImportRestricted
                    AddMissingTypes(foundTypes, GlobalVariables.RestrictedTypes)
                    UpdateTextBoxWithStrings(Kits_Restricted, GlobalVariables.RestrictedTypes)

                    'Clothing Market

                    'dna_Helm
                Case sender Is Clothes_Helmets_Import
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Helmets)
                    UpdateTextBoxWithStrings(Clothes_Helmets, GlobalVariables.ClothingMarket.Helmets)
                    'dna_Shirt
                Case sender Is Clothes_Shirts_Import
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Shirts)
                    UpdateTextBoxWithStrings(Clothes_Shirts, GlobalVariables.ClothingMarket.Shirts)
                    'dna_Vest
                Case sender Is Clothes_Vests_Import
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Vests)
                    UpdateTextBoxWithStrings(Clothes_Vests, GlobalVariables.ClothingMarket.Vests)
                    'dna_Pants
                Case sender Is Clothes_Pants_Import
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Pants)
                    UpdateTextBoxWithStrings(Clothes_Pants, GlobalVariables.ClothingMarket.Pants)
                    'dna_Shoes
                Case sender Is Clothes_Shoes_Import
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Shoes)
                    UpdateTextBoxWithStrings(Clothes_Shoes, GlobalVariables.ClothingMarket.Shoes)
                    'dna_Backpack
                Case sender Is Clothes_Backpacks_Import
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Backpacks)
                    UpdateTextBoxWithStrings(Clothes_Backpacks, GlobalVariables.ClothingMarket.Backpacks)
                    'dna_Gloves
                Case sender Is Clothes_Gloves_Import
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Gloves)
                    UpdateTextBoxWithStrings(Clothes_Gloves, GlobalVariables.ClothingMarket.Gloves)
                    'dna_Belt
                Case sender Is Clothes_Belts_Import
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Belts)
                    UpdateTextBoxWithStrings(Clothes_Belts, GlobalVariables.ClothingMarket.Belts)
                    'dna_Facewear
                Case sender Is Clothes_Facewear_Import
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Facewears)
                    UpdateTextBoxWithStrings(Clothes_Facewear, GlobalVariables.ClothingMarket.Facewears)
                    'dna_Eyewear
                Case sender Is Clothes_Eyewear_Import
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Eyewears)
                    UpdateTextBoxWithStrings(Clothes_Eyewear, GlobalVariables.ClothingMarket.Eyewears)
                    'dna_Armband
                Case sender Is Clothes_Armbands_Import
                    AddMissingTypes(foundTypes, GlobalVariables.ClothingMarket.Armbands)
                    UpdateTextBoxWithStrings(Clothes_Armbands, GlobalVariables.ClothingMarket.Armbands)
            End Select

        Next
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

    Private Sub Kits_ClearSidearms_Click(sender As Object, e As RoutedEventArgs) _
        Handles Kits_ClearSidearms.Click
        GlobalVariables.SideArms.Clear()
        Kits_SideArms.Clear()
    End Sub

    Private Async Sub WeaponKits_Generate_Click(sender As Object, e As RoutedEventArgs) _
        Handles WeaponKits_Generate.Click
        Dim WeaponColorTier As String

        If IsNothing(Kit_ColorChoice.SelectedItem) Then
            Windows.MessageBox.Show("Please select a color tier", "Alert", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information)
        Else
            WeaponColorTier = Kit_ColorChoice.SelectedItem.Content
            WeaponColorTier = WeaponColorTier.Replace(" Kit", "")


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
                    GenerateConfigs.Weapons.PurpleWeaponKits = weaponList
                Case "Blue"
                    GenerateConfigs.Weapons.BlueWeaponKits = weaponList
                Case "Green"
                    GenerateConfigs.Weapons.GreenWeaponKits = weaponList
                Case "Yellow"
                    GenerateConfigs.Weapons.YellowWeaponKits = weaponList
            End Select
            UpdateGeneratedWeaponKits()
            'Await GenerateConfigs.Weapons.GenerateConfig(WeaponColorTier)
        End If
    End Sub

    Sub UpdateGeneratedWeaponKits()
        Tab_KitsGenerated.IsSelected = True

        'Red Update
        If GenerateConfigs.Weapons.RedWeaponKits IsNot Nothing Then
            TV_WeaponKits_Generated_Red.Nodes.Clear()
            For Each WeaponSet_ As GenerateConfigs.Weapons.WeaponInfo In GenerateConfigs.Weapons.RedWeaponKits
                TV_WeaponKits_Generated_Red.Nodes.Add(CreateNodeSetWeapon(WeaponSet_))
            Next
        End If
        If GenerateConfigs.Weapons.PurpleWeaponKits IsNot Nothing Then
            'Purple Update
            TV_WeaponKits_Generated_Purple.Nodes.Clear()
            For Each WeaponSet_ As GenerateConfigs.Weapons.WeaponInfo In GenerateConfigs.Weapons.PurpleWeaponKits
                TV_WeaponKits_Generated_Purple.Nodes.Add(CreateNodeSetWeapon(WeaponSet_))
            Next
        End If
        If GenerateConfigs.Weapons.BlueWeaponKits IsNot Nothing Then
            'Blue Update
            TV_WeaponKits_Generated_Blue.Nodes.Clear()
            For Each WeaponSet_ As GenerateConfigs.Weapons.WeaponInfo In GenerateConfigs.Weapons.BlueWeaponKits
                TV_WeaponKits_Generated_Blue.Nodes.Add(CreateNodeSetWeapon(WeaponSet_))
            Next
        End If
        If GenerateConfigs.Weapons.GreenWeaponKits IsNot Nothing Then
            'Green Update
            TV_WeaponKits_Generated_Green.Nodes.Clear()
            For Each WeaponSet_ As GenerateConfigs.Weapons.WeaponInfo In GenerateConfigs.Weapons.GreenWeaponKits
                TV_WeaponKits_Generated_Green.Nodes.Add(CreateNodeSetWeapon(WeaponSet_))
            Next
        End If
        If GenerateConfigs.Weapons.YellowWeaponKits IsNot Nothing Then
            'Yellow Update
            TV_WeaponKits_Generated_Yellow.Nodes.Clear()
            For Each WeaponSet_ As GenerateConfigs.Weapons.WeaponInfo In GenerateConfigs.Weapons.YellowWeaponKits
                TV_WeaponKits_Generated_Yellow.Nodes.Add(CreateNodeSetWeapon(WeaponSet_))
            Next
        End If


        Tab_Kits.IsSelected = True
    End Sub

    Function CreateNodeSetWeapon(WeaponSet_ As GenerateConfigs.Weapons.WeaponInfo) As TreeViewNode
        Dim tParentNode As New TreeViewNode

        tParentNode.Content = WeaponSet_.dna_TheChosenOne
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Tier : " + WeaponSet_.dna_Tier})
        tParentNode.ChildNodes.Add(
            New TreeViewNode() _
                                      With {.Content = "dna_WeaponCategory : " + WeaponSet_.dna_WeaponCategory})
        tParentNode.ChildNodes.Add(
            New TreeViewNode() _
                                      With {.Content = "dna_TheChosenOne : " + WeaponSet_.dna_TheChosenOne})
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Magazine : " + WeaponSet_.dna_Magazine})
        tParentNode.ChildNodes.Add(
            New TreeViewNode() _
                                      With {.Content = "dna_Ammunition : " + WeaponSet_.dna_Ammunition})
        tParentNode.ChildNodes.Add(
            New TreeViewNode() _
                                      With {.Content = "dna_OpticType : " + WeaponSet_.dna_OpticType})
        tParentNode.ChildNodes.Add(
            New TreeViewNode() _
                                      With {.Content = "dna_Suppressor : " + WeaponSet_.dna_Suppressor})
        tParentNode.ChildNodes.Add(
            New TreeViewNode() _
                                      With {.Content = "dna_UnderBarrel : " + WeaponSet_.dna_UnderBarrel})
        tParentNode.ChildNodes.Add(
            New TreeViewNode() _
                                      With {.Content = "dna_ButtStock : " + WeaponSet_.dna_ButtStock})
        tParentNode.ChildNodes.Add(
            New TreeViewNode() _
                                      With {.Content = "dna_HandGuard : " + WeaponSet_.dna_HandGuard})
        tParentNode.ChildNodes.Add(
            New TreeViewNode() _
                                      With {.Content = "dna_Wrap : " + WeaponSet_.dna_Wrap})

        Return tParentNode
    End Function

    Public Shared Sub seedWeaponKitSettings()
        Dim WeaponKitsTypes As String() = New String() {"Red", "Purple", "Blue", "Green", "Yellow"}
        For Each type As String In WeaponKitsTypes
            GlobalVariables.Types.WeaponKits.Add(New GlobalVariables.Types.WeaponKitSettings(type, Nothing, Nothing,
                                                                                             Nothing, Nothing, Nothing))
        Next
    End Sub

    Public Sub seedItemSources()
        G_ImportedTypes.ItemsSource = GlobalVariables.Types.TypeFiles
        G_Types.ItemsSource = GlobalVariables.Types.Types

        'Weapon Kits 
        CHK_Weapon_Red_Cat.ItemsSource = GlobalVariables.Types.Categories
        CHK_Weapon_Red_Use.ItemsSource = GlobalVariables.Types.Usages
        CHK_Weapon_Red_Val.ItemsSource = GlobalVariables.Types.Values
        CHK_Weapon_Red_Tag.ItemsSource = GlobalVariables.Types.Tags
        'WeaponKits_SideArms.DataContext = GlobalVariables.SideArms
    End Sub

    Public Sub seedHandlers()
        AddHandler TV_WeaponKits_Generated_Red.ItemBeginEdit, AddressOf Event_BeginEdit_TV_WeaponKits_Generated_Red _
        ' tabLocal.ModTree.ItemDropping, AddressOf Steam.TabOperations.modtree_Drop

        AddHandler MainExe.Drop, AddressOf MainExeDropFiles
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
           type.typename.ToLower.Contains("hndgrd") Or
           type.typename.ToLower.Contains("suppressor") Or
           type.typename.ToLower.Contains("bayonet") Or
           type.typename.ToLower.Contains("compensator") Or
           type.typename.ToLower.Contains("goggles") Or
           type.typename.ToLower.Contains("light") Then
            Return False
        End If

        If StringExistsInCollection(type.typename.ToLower, GlobalVariables.RestrictedTypes) Then
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

    Function StringExistsInCollection(ByVal searchString As String, ByVal collection As ObservableCollection(Of String)) _
        As Boolean
        Return collection.Any(Function(item) String.Equals(item, searchString, StringComparison.OrdinalIgnoreCase))
    End Function

    Public Class GenerateConfigs
        Public Class Weapons
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
        End Class
    End Class
    ' Function to find the index of the child TreeViewNode with content containing the specified string.
    Private Function FindChildIndex(parentNode As TreeViewNode, searchString As String) As Integer
        ' Check if the parent node exists and has children.
        If parentNode IsNot Nothing AndAlso parentNode.ChildNodes IsNot Nothing Then
            ' Iterate through each child node.
            For index As Integer = 0 To parentNode.ChildNodes.Count - 1
                Dim childNode As TreeViewNode = parentNode.ChildNodes(index)

                ' Check if the child node's content contains the specified string.
                If childNode.Content.ToString().Contains(searchString) Then
                    Return index ' Return the index if a match is found.
                End If

                ' If no match is found, recursively search within the child node and its descendants.
                Dim resultIndex As Integer = FindChildIndex(childNode, searchString)
                If resultIndex <> - 1 Then
                    Return resultIndex ' Return the index from the recursive call.
                End If
            Next
        End If

        ' If no match is found, return -1 to indicate failure.
        Return - 1
    End Function

    Async Function ExportWeaponKitsToJson(filepath As String) As Task
        Dim weaponList As New List(Of GenerateConfigs.Weapons.WeaponInfo)()
        Dim WeaponKitTVTypeArr As New Collection(Of SfTreeView)
        WeaponKitTVTypeArr.Add(TV_WeaponKits_Generated_Red)
        WeaponKitTVTypeArr.Add(TV_WeaponKits_Generated_Purple)
        WeaponKitTVTypeArr.Add(TV_WeaponKits_Generated_Blue)
        WeaponKitTVTypeArr.Add(TV_WeaponKits_Generated_Green)
        WeaponKitTVTypeArr.Add(TV_WeaponKits_Generated_Yellow)

        For Each t_TypeColorTree As SfTreeView In WeaponKitTVTypeArr
            'Build based on Shown
            If t_TypeColorTree.Nodes.First().Content.ToString() <> "Please Generate a Kit" Then
                For Each Node As TreeViewNode In t_TypeColorTree.Nodes
                    Dim tWeaponInfo As GenerateConfigs.Weapons.WeaponInfo
                    Try
                        tWeaponInfo = New GenerateConfigs.Weapons.WeaponInfo() With {
                            .dna_Tier = GetNodeContent(Node, "dna_Tier"),
                            .dna_WeaponCategory = GetNodeContent(Node, "dna_WeaponCategory"),
                            .dna_TheChosenOne = GetNodeContent(Node, "dna_TheChosenOne"),
                            .dna_Magazine = GetNodeContent(Node, "dna_Magazine"),
                            .dna_Ammunition = GetNodeContent(Node, "dna_Ammunition"),
                            .dna_OpticType = GetNodeContent(Node, "dna_OpticType"),
                            .dna_Suppressor = GetNodeContent(Node, "dna_Suppressor"),
                            .dna_UnderBarrel = GetNodeContent(Node, "dna_UnderBarrel"),
                            .dna_ButtStock = GetNodeContent(Node, "dna_ButtStock"),
                            .dna_HandGuard = GetNodeContent(Node, "dna_HandGuard"),
                            .dna_Wrap = GetNodeContent(Node, "dna_Wrap")
                            }
                        weaponList.Add(tWeaponInfo)
                    Catch ex As Exception
                        ' Log or handle the exception as needed
                        Console.WriteLine("An error occurred while processing a node: " & ex.Message)
                    End Try
                Next
            End If
        Next

        Dim json = JsonConvert.SerializeObject(New With {Key .m_DNAConfig_Weapons = weaponList}, Formatting.Indented)
        File.WriteAllText(filepath, json)
    End Function

    Private Function GetNodeContent(node As TreeViewNode, identifier As String) As String
        Dim childNode = FindChildNode(node, identifier)
        If childNode IsNot Nothing Then
            Return childNode.Content.ToString().Replace(" ", "").Replace(identifier, "").Replace(":", "")
        End If
        Return ""
    End Function

    Private Function FindChildNode(node As TreeViewNode, identifier As String) As TreeViewNode
        For Each childNode In node.ChildNodes
            If childNode.Content.ToString().Contains(identifier) Then
                Return childNode
            End If
        Next
        Return Nothing
    End Function

    Private Async Sub WeaponKits_Generated_Export_Click(sender As Object, e As RoutedEventArgs) _
        Handles WeaponKits_Generated_Export.Click

        Dim tsaveFileDialog As New Forms.SaveFileDialog()

        ' Get the directory path of the executable
        Dim executableDirectory = AppDomain.CurrentDomain.BaseDirectory
        ' Set initial directory and filename
        tsaveFileDialog.InitialDirectory = executableDirectory ' Set your desired initial directory
        tsaveFileDialog.FileName = "KeyCard_Weapons_Config"
        tsaveFileDialog.DefaultExt = ".json"
        tsaveFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"

        Dim result As Nullable(Of Boolean) = tsaveFileDialog.ShowDialog()

        If result = True Then
            Await ExportWeaponKitsToJson(tsaveFileDialog.FileName)
            Return
        Else
            Windows.MessageBox.Show("Export Canceled.", "Alert", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information)
            Return
        End If
    End Sub

    Private Async Sub Kits_ImportRestricted_Click(sender As Object, e As RoutedEventArgs)
        Dim results As String() = Await FileSelectionHelper.SelectMultipleFilesAsync()
        Dim foundTypes As List(Of String)
        For Each resultPath As String In results
            foundTypes = New List(Of String)()
            foundTypes = FileSelectionHelper.GetUniqueClassnamesAndVariants(resultPath) _
            '.Add(FileSelectionHelper.GetUniqueClassnamesAndVariants(resultPath))
            foundTypes = FileSelectionHelper.RemoveDuplicates(foundTypes)
            AddMissingTypes(foundTypes, GlobalVariables.RestrictedTypes)
        Next

        UpdateTextBoxWithStrings(WeaponKits_Restricted, GlobalVariables.RestrictedTypes)
    End Sub

    Private Sub Kits_ClearRestricted_Click(sender As Object, e As RoutedEventArgs) Handles Kits_ClearRestricted.Click
        GlobalVariables.RestrictedTypes.Clear()
        Kits_Restricted.Clear()
    End Sub

    Private Async Sub MainExeDropFiles(sender As Object, e As Windows.DragEventArgs)
        Dim filePaths As New ObservableCollection(Of String)()
        If e.Data.GetDataPresent(Windows.DataFormats.FileDrop) Then
            Dim files As String() = DirectCast(e.Data.GetData(Windows.DataFormats.FileDrop), String())
            For Each filePath As String In files
                filePaths.Add(filePath)
            Next
        End If

        If MainTabs.SelectedItem Is Tab_ClothesSettings Then

            For Each filePath As String In filePaths
                Dim fileExt As String = System.IO.Path.GetExtension(filePath) ' Get the file extension
                If fileExt <> ".json" Then Continue For
                Dim fileRoot As String = System.IO.Path.GetDirectoryName(filePath) ' Get the file root (directory)
                Dim fileName As String = System.IO.Path.GetFileNameWithoutExtension(filePath) _
                ' Get the file name without extension


                If fileName.ToLower.Contains("helmet") Then
                    DragImportExpansionMarket(filePaths, Clothes_Helmets)
                ElseIf fileName.ToLower.Contains("pant") Then
                    DragImportExpansionMarket(filePaths, Clothes_Pants)
                ElseIf fileName.ToLower.Contains("glove") Then
                    DragImportExpansionMarket(filePaths, Clothes_Gloves)
                ElseIf fileName.ToLower.Contains("eyewear") Then
                    DragImportExpansionMarket(filePaths, Clothes_Eyewear)
                ElseIf fileName.ToLower.Contains("shirt") Then
                    DragImportExpansionMarket(filePaths, Clothes_Shirts)
                ElseIf fileName.ToLower.Contains("shoe") Then
                    DragImportExpansionMarket(filePaths, Clothes_Shoes)
                ElseIf fileName.ToLower.Contains("belt") Then
                    DragImportExpansionMarket(filePaths, Clothes_Belts)
                ElseIf fileName.ToLower.Contains("armband") Then
                    DragImportExpansionMarket(filePaths, Clothes_Armbands)
                ElseIf fileName.ToLower.Contains("vest") Then
                    DragImportExpansionMarket(filePaths, Clothes_Vests)
                ElseIf fileName.ToLower.Contains("backpack") Then
                    DragImportExpansionMarket(filePaths, Clothes_Backpacks)
                ElseIf fileName.ToLower.Contains("face") Then
                    DragImportExpansionMarket(filePaths, Clothes_Facewear)
                Else
                    Continue For
                End If
            Next


            Dim xx2 = ""
        End If
        Dim xx = ""
    End Sub
End Class

