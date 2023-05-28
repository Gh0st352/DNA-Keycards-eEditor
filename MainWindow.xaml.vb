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
    Private random As New Random()

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

    Private Async Function DragImportExpansionMarket(resultPath As String, sender As SfTextBoxExt) As Task
        Dim foundTypes As List(Of String)
        foundTypes = New List(Of String)()
        foundTypes = Await FileSelectionHelper.GetUniqueClassnamesAndVariants(resultPath) _
        '.Add(FileSelectionHelper.GetUniqueClassnamesAndVariants(resultPath))
        foundTypes = Await FileSelectionHelper.RemoveDuplicates(foundTypes)

        Select Case True
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
    End Function

    Private Async Sub ButtonImportExpansionMarketClick(sender As Object, e As RoutedEventArgs) _
        Handles Kits_ImportRestricted.Click, Clothes_Helmets_Import.Click, Clothes_Pants_Import.Click,
                Clothes_Gloves_Import.Click, Clothes_Eyewear_Import.Click, Clothes_Shirts_Import.Click,
                Clothes_Shoes_Import.Click, Clothes_Belts_Import.Click, Clothes_Armbands_Import.Click,
                Clothes_Vests_Import.Click, Clothes_Backpacks_Import.Click, Clothes_Facewear_Import.Click
        Dim results As String() = Await FileSelectionHelper.SelectMultipleFilesAsync()
        Dim foundTypes As List(Of String)
        For Each resultPath As String In results
            foundTypes = New List(Of String)()
            foundTypes = Await FileSelectionHelper.GetUniqueClassnamesAndVariants(resultPath) _
            '.Add(FileSelectionHelper.GetUniqueClassnamesAndVariants(resultPath))
            foundTypes = Await FileSelectionHelper.RemoveDuplicates(foundTypes)

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

    Sub AddMissingTypes(foundTypes As List(Of String), collect_ As ObservableCollection(Of String))
        Dim uniqueTypes As HashSet(Of String) = New HashSet(Of String)(collect_)

        For Each item In foundTypes
            If Not uniqueTypes.Contains(item) Then
                uniqueTypes.Add(item)
            End If
        Next

        collect_.Clear()

        For Each item In uniqueTypes
            collect_.Add(item)
        Next
    End Sub


    Public Sub UpdateTextBoxWithStrings(textBox As SfTextBoxExt, strings As ObservableCollection(Of String))
        textBox.Clear() ' Clear the existing contents of the textbox

        For Each str As String In strings
            textBox.Text += str & Environment.NewLine ' Append each string followed by a new line character
        Next
    End Sub

    Public Sub UpdateGlobalsWithTextBox(textBox As SfTextBoxExt, strings As ObservableCollection(Of String))
        strings.Clear() ' Clear the existing strings in the collection
        Dim lines() As String = textBox.Text.Split({Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
        For Each line As String In lines
            strings.Add(line) ' Add each line as a string to the collection
        Next
    End Sub

    Private Sub Kits_ClearSidearms_Click(sender As Object, e As RoutedEventArgs) _
        Handles Kits_ClearSidearms.Click
        GlobalVariables.SideArms.Clear()
        Kits_SideArms.Clear()
    End Sub

    Public Function GetRandomString(ByVal percentChance As Integer, ByVal obcoll As ObservableCollection(Of String)) _
        As String
        If obcoll.Count = 0 Then Return String.Empty
        If percentChance = 0 Then Return String.Empty
        If percentChance = 100 Then
            If obcoll.Count = 1 Then Return obcoll.First()
            Dim index As Integer = random.Next(0, obcoll.Count - 1) _
            ' Generate a random index within the range of the collection
            Return obcoll(index)
        End If

        Dim success As Boolean = SimulateIfStatement_PercentChance(percentChance)

        If success Then
            If obcoll.Count = 1 Then Return obcoll.First()
            Dim index As Integer = random.Next(0, obcoll.Count - 1) _
            ' Generate a random index within the range of the collection
            Return obcoll(index) ' Return the random string from the collection
        Else
            Return String.Empty ' Return an empty string if the roll is not successful
        End If
    End Function

    Public Function SimulateIfStatement_PercentChance(ByVal percentChance As Integer) As Boolean
        Dim roll As Integer = random.Next(1, 101) ' Generate a random number between 1 and 100

        Return roll <= percentChance
    End Function

    Private Async Sub ClothingKits_Generate_Click(sender As Object, e As RoutedEventArgs) _
        Handles ClothingKits_Generate.Click, ClothingKits_Generate_Copy.Click
        Dim colorTier As String

        If IsNothing(Kit_ColorChoice.SelectedItem) Then
            Windows.MessageBox.Show("Please select a color tier", "Alert", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information)
        Else
            colorTier = Kit_ColorChoice.SelectedItem.Content
            colorTier = colorTier.Replace(" Kit", "")

            Dim tHelmets As New ObservableCollection(Of String)()
            Dim tShirts As New ObservableCollection(Of String)()
            Dim tVests As New ObservableCollection(Of String)()
            Dim tPants As New ObservableCollection(Of String)()
            Dim tShoes As New ObservableCollection(Of String)()
            Dim tBackpacks As New ObservableCollection(Of String)()
            Dim tGloves As New ObservableCollection(Of String)()
            Dim tBelts As New ObservableCollection(Of String)()
            Dim tFacewears As New ObservableCollection(Of String)()
            Dim tEyewears As New ObservableCollection(Of String)()
            Dim tArmbands As New ObservableCollection(Of String)()
            Dim tNVG As New ObservableCollection(Of String)()
            tNVG.Add("NVGoggles")

            Dim tClothingParts As New ObservableCollection(Of ObservableCollection(Of String))()
            With tClothingParts
                .Add(tHelmets)
                .Add(tShirts)
                .Add(tVests)
                .Add(tPants)
                .Add(tBackpacks)
                .Add(tGloves)
                .Add(tBelts)
                .Add(tFacewears)
                .Add(tEyewears)
                .Add(tArmbands)
                .Add(tNVG)
                .Add(tShoes)
            End With
            Dim NumSetsToGen As Integer = ClothingKits_GenNum.Text


            Dim Helmet_Chance = TB_Helmet_Chance.Text
            Dim Shirt_Chance = TB_Shirt_Chance.Text
            Dim Vest_Chance = TB_Vest_Chance.Text
            Dim Pants_Chance = TB_Pants_Chance.Text
            Dim Shoes_Chance = TB_Shoes_Chance.Text
            Dim Backpack_Chance = TB_Backpack_Chance.Text
            Dim Gloves_Chance = TB_Gloves_Chance.Text
            Dim Belt_Chance = TB_Belt_Chance.Text
            Dim Facewear_Chance = TB_Facewear_Chance.Text
            Dim Eyewear_Chance = TB_Eyewear_Chance.Text
            Dim Armband_Chance = TB_Armband_Chance.Text
            Dim NVG_Chance = TB_NVG_Chance.Text


            For Each type In GlobalVariables.Types.Types
                checkedAddArgs = Await determineIfAdd(type)
                If checkedAddArgs = True Then
                    'If GlobalVariables.ClothingMarket.Helmets.Contains(type.typename) Then
                    If _
                        GlobalVariables.ClothingMarket.Helmets.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tHelmets.Add(type.typename)
                    If _
                        GlobalVariables.ClothingMarket.Shirts.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tShirts.Add(type.typename)
                    If _
                        GlobalVariables.ClothingMarket.Vests.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tVests.Add(type.typename)
                    If _
                        GlobalVariables.ClothingMarket.Pants.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tPants.Add(type.typename)
                    If _
                        GlobalVariables.ClothingMarket.Shoes.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tShoes.Add(type.typename)
                    If _
                        GlobalVariables.ClothingMarket.Backpacks.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tBackpacks.Add(type.typename)
                    If _
                        GlobalVariables.ClothingMarket.Gloves.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tGloves.Add(type.typename)
                    If _
                        GlobalVariables.ClothingMarket.Belts.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tBelts.Add(type.typename)
                    If _
                        GlobalVariables.ClothingMarket.Facewears.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tFacewears.Add(type.typename)
                    If _
                        GlobalVariables.ClothingMarket.Eyewears.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tEyewears.Add(type.typename)
                    If _
                        GlobalVariables.ClothingMarket.Armbands.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tArmbands.Add(type.typename)
                End If
            Next

            Dim clothesKits As New ObservableCollection(Of GenerateConfigs.Clothes.ClothesInfo)

            For i = 1 To NumSetsToGen
                Dim tClothesKit As New GenerateConfigs.Clothes.ClothesInfo()
                tClothesKit.dna_Tier = colorTier
                For Each tSlot As ObservableCollection(Of String) In tClothingParts
                    If tSlot Is tHelmets Then tClothesKit.dna_Helm = GetRandomString(Helmet_Chance, tSlot)
                    If tSlot Is tShirts Then tClothesKit.dna_Shirt = GetRandomString(Shirt_Chance, tSlot)
                    If tSlot Is tVests Then tClothesKit.dna_Vest = GetRandomString(Vest_Chance, tSlot)
                    If tSlot Is tPants Then tClothesKit.dna_Pants = GetRandomString(Pants_Chance, tSlot)
                    If tSlot Is tShoes Then tClothesKit.dna_Shoes = GetRandomString(Shoes_Chance, tSlot)
                    If tSlot Is tBackpacks Then tClothesKit.dna_Backpack = GetRandomString(Backpack_Chance, tSlot)
                    If tSlot Is tGloves Then tClothesKit.dna_Gloves = GetRandomString(Gloves_Chance, tSlot)
                    If tSlot Is tBelts Then tClothesKit.dna_Belt = GetRandomString(Belt_Chance, tSlot)
                    If tSlot Is tFacewears Then tClothesKit.dna_Facewear = GetRandomString(Facewear_Chance, tSlot)
                    If tSlot Is tEyewears Then tClothesKit.dna_Eyewear = GetRandomString(Eyewear_Chance, tSlot)
                    If tSlot Is tArmbands Then tClothesKit.dna_Armband = GetRandomString(Armband_Chance, tSlot)
                    If tSlot Is tNVG Then tClothesKit.dna_NVG = GetRandomString(NVG_Chance, tSlot)
                Next
                clothesKits.Add(tClothesKit)
            Next i


            Select Case colorTier
                Case "Red"
                    GenerateConfigs.Clothes.RedClothesKits = clothesKits
                Case "Purple"
                    GenerateConfigs.Clothes.PurpleClothesKits = clothesKits
                Case "Blue"
                    GenerateConfigs.Clothes.BlueClothesKits = clothesKits
                Case "Green"
                    GenerateConfigs.Clothes.GreenClothesKits = clothesKits
                Case "Yellow"
                    GenerateConfigs.Clothes.YellowClothesKits = clothesKits
            End Select

            UpdateGeneratedClothingKits()

        End If
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
        End If
    End Sub

    Sub UpdateGeneratedClothingKits()
        Tab_KitsGenerated.IsSelected = True
        Tab_ClothingKits.IsSelected = True

        'Red Update
        If GenerateConfigs.Clothes.RedClothesKits IsNot Nothing Then
            TV_ClothingKit_Generated_Red.Nodes.Clear()
            For Each Clotheset_ As GenerateConfigs.Clothes.ClothesInfo In GenerateConfigs.Clothes.RedClothesKits
                Dim tCount = TV_ClothingKit_Generated_Red.Nodes.Count
                TV_ClothingKit_Generated_Red.Nodes.Add(CreateNodeSetClothes(Clotheset_, tCount))
            Next
        End If
        If GenerateConfigs.Clothes.PurpleClothesKits IsNot Nothing Then
            'Purple Update
            TV_ClothingKit_Generated_Purple.Nodes.Clear()
            For Each Clotheset_ As GenerateConfigs.Clothes.ClothesInfo In GenerateConfigs.Clothes.PurpleClothesKits
                Dim tCount = TV_ClothingKit_Generated_Purple.Nodes.Count
                TV_ClothingKit_Generated_Purple.Nodes.Add(CreateNodeSetClothes(Clotheset_, tCount))
            Next
        End If
        If GenerateConfigs.Clothes.BlueClothesKits IsNot Nothing Then
            'Blue Update
            TV_ClothingKit_Generated_Blue.Nodes.Clear()
            For Each Clotheset_ As GenerateConfigs.Clothes.ClothesInfo In GenerateConfigs.Clothes.BlueClothesKits
                Dim tCount = TV_ClothingKit_Generated_Blue.Nodes.Count
                TV_ClothingKit_Generated_Blue.Nodes.Add(CreateNodeSetClothes(Clotheset_, tCount))
            Next
        End If
        If GenerateConfigs.Clothes.GreenClothesKits IsNot Nothing Then
            'Green Update
            TV_ClothingKit_Generated_Green.Nodes.Clear()
            For Each Clotheset_ As GenerateConfigs.Clothes.ClothesInfo In GenerateConfigs.Clothes.GreenClothesKits
                Dim tCount = TV_ClothingKit_Generated_Green.Nodes.Count
                TV_ClothingKit_Generated_Green.Nodes.Add(CreateNodeSetClothes(Clotheset_, tCount))
            Next
        End If
        If GenerateConfigs.Clothes.YellowClothesKits IsNot Nothing Then
            'Yellow Update
            TV_ClothingKit_Generated_Yellow.Nodes.Clear()
            For Each Clotheset_ As GenerateConfigs.Clothes.ClothesInfo In GenerateConfigs.Clothes.YellowClothesKits
                Dim tCount = TV_ClothingKit_Generated_Yellow.Nodes.Count
                TV_ClothingKit_Generated_Yellow.Nodes.Add(CreateNodeSetClothes(Clotheset_, tCount))
            Next
        End If

        Tab_Kits.IsSelected = True
    End Sub

    Sub UpdateGeneratedWeaponKits()
        Tab_KitsGenerated.IsSelected = True
        Tab_WeaponKits.IsSelected = True
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

    Function CreateNodeSetClothes(ClothesSet_ As GenerateConfigs.Clothes.ClothesInfo, tCount As Integer) As TreeViewNode
        Dim tParentNode As New TreeViewNode

        tParentNode.Content = ("Set: # " + (tCount + 1).ToString())
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Tier : " + ClothesSet_.dna_Tier})
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Helm : " + ClothesSet_.dna_Helm})
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Shirt : " + ClothesSet_.dna_Shirt})
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Vest : " + ClothesSet_.dna_Vest})
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Pants : " + ClothesSet_.dna_Pants})
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Shoes : " + ClothesSet_.dna_Shoes})
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Backpack : " + ClothesSet_.dna_Backpack})
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Gloves : " + ClothesSet_.dna_Gloves})
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Belt : " + ClothesSet_.dna_Belt})
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Facewear : " + ClothesSet_.dna_Facewear})
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Eyewear : " + ClothesSet_.dna_Eyewear})
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Armband : " + ClothesSet_.dna_Armband})
        tParentNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_NVG : " + ClothesSet_.dna_NVG})

        Return tParentNode
    End Function

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
        CHK_Kits_Cat.ItemsSource = GlobalVariables.Types.Categories
        CHK_Kits_Use.ItemsSource = GlobalVariables.Types.Usages
        CHK_Kits_Val.ItemsSource = GlobalVariables.Types.Values
        CHK_Kits_Tag.ItemsSource = GlobalVariables.Types.Tags


    End Sub

    Public Sub seedHandlers()
        AddHandler TV_WeaponKits_Generated_Red.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_Kits _
        ' tabLocal.ModTree.ItemDropping, AddressOf Steam.TabOperations.modtree_Drop
        AddHandler TV_ClothingKit_Generated_Red.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_Kits
        AddHandler MainExe.Drop, AddressOf MainExeDropFiles
        AttachTextChangedEventToAllTextBoxes(Tab_ClothesSettings)
        AttachTextChangedEventToAllTextBoxes(Tab_Kits)
    End Sub

    Async Sub Event_BeginEdit_Generated_Kits(sender As Object, e As EventArgs)
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
        selectedArgs = CHK_Kits_Flags.SelectedItems
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
        selectedArgs = CHK_Kits_Cat.SelectedItems
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
        selectedArgs = CHK_Kits_Val.SelectedItems
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
        selectedArgs = CHK_Kits_Use.SelectedItems
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
        selectedArgs = CHK_Kits_Tag.SelectedItems
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

    Async Function ExportClothingKitsToJson(filepath As String) As Task
        Dim clothingList As New List(Of GenerateConfigs.Clothes.ClothesInfo)()
        Dim ClothesKitTVTypeArr As New Collection(Of SfTreeView)
        ClothesKitTVTypeArr.Add(TV_ClothingKit_Generated_Red)
        ClothesKitTVTypeArr.Add(TV_ClothingKit_Generated_Purple)
        ClothesKitTVTypeArr.Add(TV_ClothingKit_Generated_Blue)
        ClothesKitTVTypeArr.Add(TV_ClothingKit_Generated_Green)
        ClothesKitTVTypeArr.Add(TV_ClothingKit_Generated_Yellow)

        For Each t_TypeColorTree As SfTreeView In ClothesKitTVTypeArr
            'Build based on Shown
            If t_TypeColorTree.Nodes.First().Content.ToString() <> "Please Generate a Kit" Then
                For Each Node As TreeViewNode In t_TypeColorTree.Nodes
                    Dim tClothesInfo As GenerateConfigs.Clothes.ClothesInfo
                    Try
                        tClothesInfo = New GenerateConfigs.Clothes.ClothesInfo() With {
                            .dna_Tier = GetNodeContent(Node, "dna_Tier"),
                            .dna_Helm = GetNodeContent(Node, "dna_Helm"),
                            .dna_Shirt = GetNodeContent(Node, "dna_Shirt"),
                            .dna_Vest = GetNodeContent(Node, "dna_Vest"),
                            .dna_Pants = GetNodeContent(Node, "dna_Pants"),
                            .dna_Shoes = GetNodeContent(Node, "dna_Shoes"),
                            .dna_Backpack = GetNodeContent(Node, "dna_Backpack"),
                            .dna_Gloves = GetNodeContent(Node, "dna_Gloves"),
                            .dna_Belt = GetNodeContent(Node, "dna_Belt"),
                            .dna_Facewear = GetNodeContent(Node, "dna_Facewear"),
                            .dna_Eyewear = GetNodeContent(Node, "dna_Eyewear"),
                            .dna_Armband = GetNodeContent(Node, "dna_Armband"),
                            .dna_NVG = GetNodeContent(Node, "dna_NVG")
                            }
                        clothingList.Add(tClothesInfo)
                    Catch ex As Exception
                        ' Log or handle the exception as needed
                        Console.WriteLine("An error occurred while processing a node: " & ex.Message)
                    End Try
                Next
            End If
        Next

        Dim json = JsonConvert.SerializeObject(New With {Key .m_DNAConfig_Clothing = clothingList}, Formatting.Indented)
        File.WriteAllText(filepath, json)
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

    Private Async Sub ClothingKits_Generated_Export_Click(sender As Object, e As RoutedEventArgs) _
        Handles ClothingKit_Generated_Export.Click

        Dim tsaveFileDialog As New Forms.SaveFileDialog()

        ' Get the directory path of the executable
        Dim executableDirectory = AppDomain.CurrentDomain.BaseDirectory
        ' Set initial directory and filename
        tsaveFileDialog.InitialDirectory = executableDirectory ' Set your desired initial directory
        tsaveFileDialog.FileName = "KeyCard_Clothing_Config"
        tsaveFileDialog.DefaultExt = ".json"
        tsaveFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"

        Dim result As Nullable(Of Boolean) = tsaveFileDialog.ShowDialog()

        If result = True Then
            Await ExportClothingKitsToJson(tsaveFileDialog.FileName)
            Return
        Else
            Windows.MessageBox.Show("Export Canceled.", "Alert", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information)
            Return
        End If
    End Sub

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
            foundTypes = Await FileSelectionHelper.GetUniqueClassnamesAndVariants(resultPath) _
            '.Add(FileSelectionHelper.GetUniqueClassnamesAndVariants(resultPath))
            foundTypes = Await FileSelectionHelper.RemoveDuplicates(foundTypes)
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
                Dim fileName As String = System.IO.Path.GetFileNameWithoutExtension(filePath)


                If fileName.ToLower.Contains("helmet") Then
                    Await DragImportExpansionMarket(filePath, Clothes_Helmets)
                    Continue For
                ElseIf fileName.ToLower.Contains("pant") Then
                    Await DragImportExpansionMarket(filePath, Clothes_Pants)
                    Continue For
                ElseIf fileName.ToLower.Contains("glove") Then
                    Await DragImportExpansionMarket(filePath, Clothes_Gloves)
                    Continue For
                ElseIf fileName.ToLower.Contains("eyewear") Then
                    Await DragImportExpansionMarket(filePath, Clothes_Eyewear)
                    Continue For
                ElseIf fileName.ToLower.Contains("shirt") Then
                    Await DragImportExpansionMarket(filePath, Clothes_Shirts)
                    Continue For
                ElseIf fileName.ToLower.Contains("shoe") Then
                    Await DragImportExpansionMarket(filePath, Clothes_Shoes)
                    Continue For
                ElseIf fileName.ToLower.Contains("belt") Then
                    Await DragImportExpansionMarket(filePath, Clothes_Belts)
                    Continue For
                ElseIf fileName.ToLower.Contains("armband") Then
                    Await DragImportExpansionMarket(filePath, Clothes_Armbands)
                    Continue For
                ElseIf fileName.ToLower.Contains("vest") Then
                    Await DragImportExpansionMarket(filePath, Clothes_Vests)
                    Continue For
                ElseIf fileName.ToLower.Contains("backpack") Then
                    Await DragImportExpansionMarket(filePath, Clothes_Backpacks)
                    Continue For
                ElseIf fileName.ToLower.Contains("face") Then
                    Await DragImportExpansionMarket(filePath, Clothes_Facewear)
                    Continue For
                Else
                    Continue For
                End If
            Next
        End If
    End Sub

    Private Sub AttachTextChangedEventHandlerToTextBoxes(container As DependencyObject)
        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(container) - 1
            Dim child As DependencyObject = VisualTreeHelper.GetChild(container, i)

            If TypeOf child Is SfTextBoxExt Then
                Dim textBox As SfTextBoxExt = DirectCast(child, SfTextBoxExt)
                AddHandler textBox.TextChanged, AddressOf SfTextBoxExt_TextChanged
                AddHandler textBox.LostFocus, AddressOf SfTextBoxExt_LostFocus
            Else
                AttachTextChangedEventHandlerToTextBoxes(child)
            End If
        Next
    End Sub

    Private Sub SfTextBoxExt_TextChanged(sender As Object, e As TextChangedEventArgs)
        ' Handle the text changed event here
        Dim textBox As SfTextBoxExt = DirectCast(sender, SfTextBoxExt)
        ' Store the updated text in a variable or perform any immediate actions
        ' This section will be reached as the user is typing or making changes

        ' Example: Store the updated text
        Dim newText As String = textBox.Text
        textBox.Tag = newText
    End Sub

    Private Sub SfTextBoxExt_LostFocus(sender As Object, e As RoutedEventArgs)
        ' Handle the lost focus event here
        Dim textBox As SfTextBoxExt = DirectCast(sender, SfTextBoxExt)
        ' Retrieve the stored updated text
        'Dim newText As String = CStr(textBox.Tag)

        ' Perform any desired actions with the updated text
        Select Case True

            'Sidearms
            Case textBox Is Kits_SideArms
                UpdateGlobalsWithTextBox(Kits_SideArms, GlobalVariables.SideArms)
                UpdateTextBoxWithStrings(Kits_SideArms, GlobalVariables.SideArms)

                'Restricted
            Case textBox Is Kits_Restricted
                UpdateGlobalsWithTextBox(Kits_Restricted, GlobalVariables.RestrictedTypes)
                UpdateTextBoxWithStrings(Kits_Restricted, GlobalVariables.RestrictedTypes)

                'Clothing Market

                'dna_Helm
            Case textBox Is Clothes_Helmets
                UpdateGlobalsWithTextBox(Clothes_Helmets, GlobalVariables.ClothingMarket.Helmets)
                UpdateTextBoxWithStrings(Clothes_Helmets, GlobalVariables.ClothingMarket.Helmets)
                'dna_Shirt
            Case textBox Is Clothes_Shirts
                UpdateGlobalsWithTextBox(Clothes_Shirts, GlobalVariables.ClothingMarket.Shirts)
                UpdateTextBoxWithStrings(Clothes_Shirts, GlobalVariables.ClothingMarket.Shirts)
                'dna_Vest
            Case textBox Is Clothes_Vests
                UpdateGlobalsWithTextBox(Clothes_Vests, GlobalVariables.ClothingMarket.Vests)
                UpdateTextBoxWithStrings(Clothes_Vests, GlobalVariables.ClothingMarket.Vests)
                'dna_Pants
            Case textBox Is Clothes_Pants
                UpdateGlobalsWithTextBox(Clothes_Pants, GlobalVariables.ClothingMarket.Pants)
                UpdateTextBoxWithStrings(Clothes_Pants, GlobalVariables.ClothingMarket.Pants)
                'dna_Shoes
            Case textBox Is Clothes_Shoes
                UpdateGlobalsWithTextBox(Clothes_Shoes, GlobalVariables.ClothingMarket.Shirts)
                UpdateTextBoxWithStrings(Clothes_Shoes, GlobalVariables.ClothingMarket.Shoes)
                'dna_Backpack
            Case textBox Is Clothes_Backpacks
                UpdateGlobalsWithTextBox(Clothes_Backpacks, GlobalVariables.ClothingMarket.Backpacks)
                UpdateTextBoxWithStrings(Clothes_Backpacks, GlobalVariables.ClothingMarket.Backpacks)
                'dna_Gloves
            Case textBox Is Clothes_Gloves
                UpdateGlobalsWithTextBox(Clothes_Gloves, GlobalVariables.ClothingMarket.Gloves)
                UpdateTextBoxWithStrings(Clothes_Gloves, GlobalVariables.ClothingMarket.Gloves)
                'dna_Belt
            Case textBox Is Clothes_Belts
                UpdateGlobalsWithTextBox(Clothes_Belts, GlobalVariables.ClothingMarket.Belts)
                UpdateTextBoxWithStrings(Clothes_Belts, GlobalVariables.ClothingMarket.Belts)
                'dna_Facewear
            Case textBox Is Clothes_Facewear
                UpdateGlobalsWithTextBox(Clothes_Facewear, GlobalVariables.ClothingMarket.Facewears)
                UpdateTextBoxWithStrings(Clothes_Facewear, GlobalVariables.ClothingMarket.Facewears)
                'dna_Eyewear
            Case textBox Is Clothes_Eyewear
                UpdateGlobalsWithTextBox(Clothes_Eyewear, GlobalVariables.ClothingMarket.Eyewears)
                UpdateTextBoxWithStrings(Clothes_Eyewear, GlobalVariables.ClothingMarket.Eyewears)
                'dna_Armband
            Case textBox Is Clothes_Armbands
                UpdateGlobalsWithTextBox(Clothes_Armbands, GlobalVariables.ClothingMarket.Armbands)
                UpdateTextBoxWithStrings(Clothes_Armbands, GlobalVariables.ClothingMarket.Armbands)
        End Select
    End Sub

    Private Sub AttachTextChangedEventToAllTextBoxes(tabItem As TabItemExt)
        AttachTextChangedEventHandlerToTextBoxes(tabItem.Content)
    End Sub
End Class

