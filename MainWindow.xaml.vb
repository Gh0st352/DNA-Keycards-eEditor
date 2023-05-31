'--- DNA Keycards Economy Editor
'-- ------------------------------------------------------------------------------------------
'-- 
'-- This Is free software, And you are welcome to redistribute it with certain conditions.
'-- 
'-- --------------------- Creative Commons Attribution-NonCommercial-ShareAlike 3.0 IGO License ------------------------------------
'-
'--This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 3.0 IGO License.
'-To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/igo/
'
'or
'
'-send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.
'
'--
'-- This program Is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
'-- without even the implied warranty of MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.
'--
'-Any person or entity obtaining any Syncfusion code, licensed assemblies, Or dependencies As a result Of the Open Source Project must
'-obtain their own licensed copy Of the Licensed Product from Syncfusion.
'--
'-- You should have received a copy of the Creative Commons Attribution-NonCommercial-ShareAlike 3.0 IGO License in the downloaded program folder. 
'--
'-- Copyright (C) 2023 Gh0st - This program comes with ABSOLUTELY NO WARRANTY
'-- ------------------------------------------------------------------------------------------

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
Imports System.Reflection
Imports System.Text.RegularExpressions
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
                SetGridBackgroundColor(G_LootSettings, 131, 14, 14, 30)
            Case "Purple Kit"
                SetGridBackgroundColor(G_KitType, 238, 51, 229, 20)
                SetGridBackgroundColor(G_ClothesSettings, 238, 51, 229, 20)
                SetGridBackgroundColor(G_LootSettings, 238, 51, 229, 20)
            Case "Blue Kit"
                SetGridBackgroundColor(G_KitType, 48, 67, 225, 20)
                SetGridBackgroundColor(G_ClothesSettings, 48, 67, 225, 20)
                SetGridBackgroundColor(G_LootSettings, 48, 67, 225, 20)
            Case "Green Kit"
                SetGridBackgroundColor(G_KitType, 80, 255, 71, 20)
                SetGridBackgroundColor(G_ClothesSettings, 80, 255, 71, 20)
                SetGridBackgroundColor(G_LootSettings, 80, 255, 71, 20)
            Case "Yellow Kit"
                SetGridBackgroundColor(G_KitType, 255, 243, 0, 30)
                SetGridBackgroundColor(G_ClothesSettings, 255, 243, 0, 30)
                SetGridBackgroundColor(G_LootSettings, 255, 243, 0, 30)
            Case Else
                SetGridBackgroundColor(G_KitType, 83, 83, 83, 20)
                SetGridBackgroundColor(G_ClothesSettings, 83, 83, 83, 20)
                SetGridBackgroundColor(G_LootSettings, 83, 83, 83, 20)
        End Select
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
                Clothes_Vests_Import.Click, Clothes_Backpacks_Import.Click, Clothes_Facewear_Import.Click, Loot_proprietary_Import.Click, Loot_medical_Import.Click, Loot_food_Import.Click, Loot_drink_Import.Click, Loot_tools_Import.Click, Loot_material_Import.Click, Loot_misc_Import.Click, Loot_valuable_Import.Click
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

                    'Loot Market

                Case sender Is Loot_proprietary_Import
                    AddMissingTypes(foundTypes, GlobalVariables.LootMarket.proprietary)
                    UpdateTextBoxWithStrings(Loot_proprietary, GlobalVariables.LootMarket.proprietary)
                Case sender Is Loot_medical_Import
                    AddMissingTypes(foundTypes, GlobalVariables.LootMarket.medical)
                    UpdateTextBoxWithStrings(Loot_medical, GlobalVariables.LootMarket.medical)
                Case sender Is Loot_food_Import
                    AddMissingTypes(foundTypes, GlobalVariables.LootMarket.food)
                    UpdateTextBoxWithStrings(Loot_food, GlobalVariables.LootMarket.food)
                Case sender Is Loot_drink_Import
                    AddMissingTypes(foundTypes, GlobalVariables.LootMarket.drink)
                    UpdateTextBoxWithStrings(Loot_drink, GlobalVariables.LootMarket.drink)
                Case sender Is Loot_tools_Import
                    AddMissingTypes(foundTypes, GlobalVariables.LootMarket.tools)
                    UpdateTextBoxWithStrings(Loot_tools, GlobalVariables.LootMarket.tools)
                Case sender Is Loot_material_Import
                    AddMissingTypes(foundTypes, GlobalVariables.LootMarket.material)
                    UpdateTextBoxWithStrings(Loot_material, GlobalVariables.LootMarket.material)
                Case sender Is Loot_misc_Import
                    AddMissingTypes(foundTypes, GlobalVariables.LootMarket.misc)
                    UpdateTextBoxWithStrings(Loot_misc, GlobalVariables.LootMarket.misc)
                Case sender Is Loot_valuable_Import
                    AddMissingTypes(foundTypes, GlobalVariables.LootMarket.valuable)
                    UpdateTextBoxWithStrings(Loot_valuable, GlobalVariables.LootMarket.valuable)

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
    Private Sub ClearTB_Click(sender As Object, e As RoutedEventArgs)

        'Dim clickedButton As ButtonAdv = DirectCast(sender, ButtonAdv)

        Select Case True
            ' Sidearms
                Case sender Is Kits_ClearSidearms
                    GlobalVariables.SideArms.Clear()
                    Kits_SideArms.Clear()

            ' Restricted
                Case sender Is Kits_ClearRestricted
                    GlobalVariables.RestrictedTypes.Clear()
                    Kits_Restricted.Clear()

            ' Clothing Market
                Case sender Is Clothes_Helmets_Clear
                    GlobalVariables.ClothingMarket.Helmets.Clear()
                    Clothes_Helmets.Clear()
                Case sender Is Clothes_Shirts_Clear
                    GlobalVariables.ClothingMarket.Shirts.Clear()
                    Clothes_Shirts.Clear()
                Case sender Is Clothes_Vests_Clear
                    GlobalVariables.ClothingMarket.Vests.Clear()
                    Clothes_Vests.Clear()
                Case sender Is Clothes_Pants_Clear
                    GlobalVariables.ClothingMarket.Pants.Clear()
                    Clothes_Pants.Clear()
                Case sender Is Clothes_Shoes_Clear
                    GlobalVariables.ClothingMarket.Shoes.Clear()
                    Clothes_Shoes.Clear()
                Case sender Is Clothes_Backpacks_Clear
                    GlobalVariables.ClothingMarket.Backpacks.Clear()
                    Clothes_Backpacks.Clear()
                Case sender Is Clothes_Gloves_Clear
                    GlobalVariables.ClothingMarket.Gloves.Clear()
                    Clothes_Gloves.Clear()
                Case sender Is Clothes_Belts_Clear
                    GlobalVariables.ClothingMarket.Belts.Clear()
                    Clothes_Belts.Clear()
                Case sender Is Clothes_Facewear_Clear
                    GlobalVariables.ClothingMarket.Facewears.Clear()
                    Clothes_Facewear.Clear()
                Case sender Is Clothes_Eyewear_Clear
                    GlobalVariables.ClothingMarket.Eyewears.Clear()
                    Clothes_Eyewear.Clear()
                Case sender Is Clothes_Armbands_Clear
                    GlobalVariables.ClothingMarket.Armbands.Clear()
                    Clothes_Armbands.Clear()

            ' Loot Market
                Case sender Is Loot_proprietary_Clear
                    GlobalVariables.LootMarket.proprietary.Clear()
                    Loot_proprietary.Clear()
                Case sender Is Loot_medical_Clear
                    GlobalVariables.LootMarket.medical.Clear()
                    Loot_medical.Clear()
                Case sender Is Loot_food_Clear
                    GlobalVariables.LootMarket.food.Clear()
                    Loot_food.Clear()
                Case sender Is Loot_drink_Clear
                    GlobalVariables.LootMarket.drink.Clear()
                    Loot_drink.Clear()
                Case sender Is Loot_tools_Clear
                    GlobalVariables.LootMarket.tools.Clear()
                    Loot_tools.Clear()
                Case sender Is Loot_material_Clear
                    GlobalVariables.LootMarket.material.Clear()
                    Loot_material.Clear()
                Case sender Is Loot_misc_Clear
                    GlobalVariables.LootMarket.misc.Clear()
                    Loot_misc.Clear()
                Case sender Is Loot_valuable_Clear
                    GlobalVariables.LootMarket.valuable.Clear()
                    Loot_valuable.Clear()
            End Select

    End Sub

    'Private Sub ClearTB_Click(sender As Object, e As RoutedEventArgs)
    '    'GlobalVariables.SideArms.Clear()
    '    'Kits_SideArms.Clear()
    '    Select Case True

    '            'Sidearms
    '        Case sender Is Kits_ClearSidearms
    '            GlobalVariables.SideArms.Clear()
    '            Kits_SideArms.Clear()

    '                'Restricted
    '        Case sender Is Kits_ClearRestricted
    '            GlobalVariables.RestrictedTypes.Clear()
    '            Kits_Restricted.Clear()
    '                'Clothing Market

    '                'dna_Helm
    '        Case sender Is Clothes_Helmets_Import
    '            GlobalVariables.ClothingMarket.Helmets.Clear()
    '            Clothes_Helmets.Clear()
    '                'dna_Shirt
    '        Case sender Is Clothes_Shirts_Import
    '            GlobalVariables.ClothingMarket.Shirts.Clear()
    '            Clothes_Shirts.Clear()
    '                'dna_Vest
    '        Case sender Is Clothes_Vests_Import
    '            GlobalVariables.ClothingMarket.Vests.Clear()
    '            Clothes_Vests.Clear()
    '                'dna_Pants
    '        Case sender Is Clothes_Pants_Import
    '            GlobalVariables.ClothingMarket.Pants.Clear()
    '            Clothes_Pants.Clear()
    '                'dna_Shoes
    '        Case sender Is Clothes_Shoes_Import
    '            GlobalVariables.ClothingMarket.Shoes.Clear()
    '            Clothes_Shoes.Clear()
    '                'dna_Backpack
    '        Case sender Is Clothes_Backpacks_Import
    '            GlobalVariables.ClothingMarket.Backpacks.Clear()
    '            Clothes_Backpacks.Clear()
    '                'dna_Gloves
    '        Case sender Is Clothes_Gloves_Import
    '            GlobalVariables.ClothingMarket.Gloves.Clear()
    '            Clothes_Gloves.Clear()
    '                'dna_Belt
    '        Case sender Is Clothes_Belts_Import
    '            GlobalVariables.ClothingMarket.Belts.Clear()
    '            Clothes_Belts.Clear()
    '                'dna_Facewear
    '        Case sender Is Clothes_Facewear_Import
    '            GlobalVariables.ClothingMarket.Facewears.Clear()
    '            Clothes_Facewear.Clear()
    '                'dna_Eyewear
    '        Case sender Is Clothes_Eyewear_Import
    '            GlobalVariables.ClothingMarket.Eyewears.Clear()
    '            Clothes_Eyewear.Clear()
    '                'dna_Armband
    '        Case sender Is Clothes_Armbands_Import
    '            GlobalVariables.ClothingMarket.Armbands.Clear()
    '            Clothes_Armbands.Clear()

    '                'Loot Market

    '        Case sender Is Loot_proprietary_Import
    '            GlobalVariables.LootMarket.proprietary.Clear()
    '            Loot_proprietary.Clear()

    '        Case sender Is Loot_medical_Import
    '            GlobalVariables.LootMarket.medical.Clear()
    '            Loot_medical.Clear()

    '        Case sender Is Loot_food_Import
    '            GlobalVariables.LootMarket.food.Clear()
    '            Loot_food.Clear()

    '        Case sender Is Loot_drink_Import
    '            GlobalVariables.LootMarket.drink.Clear()
    '            Loot_drink.Clear()

    '        Case sender Is Loot_tools_Import
    '            GlobalVariables.LootMarket.tools.Clear()
    '            Loot_tools.Clear()

    '        Case sender Is Loot_material_Import
    '            GlobalVariables.LootMarket.material.Clear()
    '            Loot_material.Clear()

    '        Case sender Is Loot_misc_Import
    '            GlobalVariables.LootMarket.misc.Clear()
    '            Loot_misc.Clear()

    '        Case sender Is Loot_valuable_Import
    '            GlobalVariables.LootMarket.valuable.Clear()
    '            Loot_valuable.Clear()

    '    End Select
    'End Sub

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
                tClothesKit.dna_Tier = colorTier.ToLower()
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

    Private Async Sub LootKits_Generate_Click(sender As Object, e As RoutedEventArgs) _
        Handles LootKits_Generate_Copy.Click, LootKits_Generate.Click
        Dim colorTier As String

        If IsNothing(Kit_ColorChoice.SelectedItem) Then
            Windows.MessageBox.Show("Please select a color tier", "Alert", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information)
        Else
            colorTier = Kit_ColorChoice.SelectedItem.Content
            colorTier = colorTier.Replace(" Kit", "")

            Dim tproprietary As New ObservableCollection(Of String)()
            Dim tmedical As New ObservableCollection(Of String)()
            Dim tfood As New ObservableCollection(Of String)()
            Dim tdrink As New ObservableCollection(Of String)()
            Dim ttools As New ObservableCollection(Of String)()
            Dim tmaterial As New ObservableCollection(Of String)()
            Dim tmisc As New ObservableCollection(Of String)()
            Dim tvaluable As New ObservableCollection(Of String)()

            Dim collection As New ObservableCollection(Of ObservableCollection(Of String))()
            With collection
                .Add(tproprietary)
                .Add(tmedical)
                .Add(tfood)
                .Add(tdrink)
                .Add(ttools)
                .Add(tmaterial)
                .Add(tmisc)
                .Add(tvaluable)
            End With

            For Each type In GlobalVariables.Types.Types
                checkedAddArgs = Await determineIfAdd(type, False)
                If checkedAddArgs = True Then
                    'If GlobalVariables.ClothingMarket.Helmets.Contains(type.typename) Then
                    If _
                        GlobalVariables.LootMarket.proprietary.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tproprietary.Add(type.typename)
                    If _
                        GlobalVariables.LootMarket.medical.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tmedical.Add(type.typename)
                    If _
                        GlobalVariables.LootMarket.food.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tfood.Add(type.typename)
                    If _
                        GlobalVariables.LootMarket.drink.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tdrink.Add(type.typename)
                    If _
                        GlobalVariables.LootMarket.tools.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        ttools.Add(type.typename)
                    If _
                        GlobalVariables.LootMarket.material.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tmaterial.Add(type.typename)
                    If _
                        GlobalVariables.LootMarket.misc.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tmisc.Add(type.typename)
                    If _
                        GlobalVariables.LootMarket.valuable.Any(
                            Function(t_) String.Equals(t_, type.typename, StringComparison.OrdinalIgnoreCase)) Then _
                        tvaluable.Add(type.typename)
                End If
            Next

            Dim tempLootInfo As New GenerateConfigs.Loot.LootInfoCats With {
                    .proprietary = New Collection(Of GenerateConfigs.Loot.LootInfoType),
                    .medical = New Collection(Of GenerateConfigs.Loot.LootInfoType),
                    .food = New Collection(Of GenerateConfigs.Loot.LootInfoType),
                    .drink = New Collection(Of GenerateConfigs.Loot.LootInfoType),
                    .tools = New Collection(Of GenerateConfigs.Loot.LootInfoType),
                    .material = New Collection(Of GenerateConfigs.Loot.LootInfoType),
                    .misc = New Collection(Of GenerateConfigs.Loot.LootInfoType),
                    .valuable = New Collection(Of GenerateConfigs.Loot.LootInfoType)
                    }
            For Each tSlot As ObservableCollection(Of String) In collection
                If tSlot Is tproprietary Then
                    For Each tType In tSlot
                        Dim info As New GenerateConfigs.Loot.LootInfoType With {
                                .dna_Tier = colorTier.ToLower(),
                                .dna_Category = "proprietary",
                                .dna_Type = tType
                                }

                        tempLootInfo.proprietary.Add(info)
                    Next
                End If
                If tSlot Is tmedical Then
                    For Each tType In tSlot
                        Dim info As New GenerateConfigs.Loot.LootInfoType()
                        info.dna_Tier = colorTier.ToLower()
                        info.dna_Category = "medical"
                        info.dna_Type = tType
                        tempLootInfo.medical.Add(info)
                    Next
                End If
                If tSlot Is tfood Then
                    For Each tType In tSlot
                        Dim info As New GenerateConfigs.Loot.LootInfoType()
                        info.dna_Tier = colorTier.ToLower()
                        info.dna_Category = "food"
                        info.dna_Type = tType
                        tempLootInfo.food.Add(info)
                    Next
                End If
                If tSlot Is tdrink Then
                    For Each tType In tSlot
                        Dim info As New GenerateConfigs.Loot.LootInfoType()
                        info.dna_Tier = colorTier.ToLower()
                        info.dna_Category = "drink"
                        info.dna_Type = tType
                        tempLootInfo.drink.Add(info)
                    Next
                End If
                If tSlot Is ttools Then
                    For Each tType In tSlot
                        Dim info As New GenerateConfigs.Loot.LootInfoType()
                        info.dna_Tier = colorTier.ToLower()
                        info.dna_Category = "tools"
                        info.dna_Type = tType
                        tempLootInfo.tools.Add(info)
                    Next
                End If
                If tSlot Is tmaterial Then
                    For Each tType In tSlot
                        Dim info As New GenerateConfigs.Loot.LootInfoType()
                        info.dna_Tier = colorTier.ToLower()
                        info.dna_Category = "material"
                        info.dna_Type = tType
                        tempLootInfo.material.Add(info)
                    Next
                End If
                If tSlot Is tmisc Then
                    For Each tType In tSlot
                        Dim info As New GenerateConfigs.Loot.LootInfoType()
                        info.dna_Tier = colorTier.ToLower()
                        info.dna_Category = "misc"
                        info.dna_Type = tType
                        tempLootInfo.misc.Add(info)
                    Next
                End If
                If tSlot Is tvaluable Then
                    For Each tType In tSlot
                        Dim info As New GenerateConfigs.Loot.LootInfoType()
                        info.dna_Tier = colorTier.ToLower()
                        info.dna_Category = "valuable "
                        info.dna_Type = tType
                        tempLootInfo.valuable.Add(info)
                    Next
                End If
            Next

            Select Case colorTier
                Case "Red"
                    GenerateConfigs.Loot.RedLootKits = tempLootInfo
                Case "Purple"
                    GenerateConfigs.Loot.PurpleLootKits = tempLootInfo
                Case "Blue"
                    GenerateConfigs.Loot.BlueLootKits = tempLootInfo
                Case "Green"
                    GenerateConfigs.Loot.GreenLootKits = tempLootInfo
                Case "Yellow"
                    GenerateConfigs.Loot.YellowLootKits = tempLootInfo
            End Select

            UpdateGeneratedLootKits()

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


    Function GenerateTreeViewNodes_Loot(lootKits As Collection(Of GenerateConfigs.Loot.LootInfoType), contentText As String) As TreeViewNode
        Dim proprietaryNode As New TreeViewNode
        proprietaryNode.Content = contentText

        For Each lootSet As GenerateConfigs.Loot.LootInfoType In lootKits
            Dim lootSetNode As New TreeViewNode
            lootSetNode.Content = lootSet.dna_Type
            lootSetNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Tier : " + lootSet.dna_Tier})
            lootSetNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Category : " + lootSet.dna_Category})
            lootSetNode.ChildNodes.Add(New TreeViewNode() With {.Content = "dna_Type : " + lootSet.dna_Type})
            proprietaryNode.ChildNodes.Add(lootSetNode)
        Next
        Return proprietaryNode
    End Function




    Sub UpdateGeneratedLootKits()
        Tab_KitsGenerated.IsSelected = True
        Tab_LootKits.IsSelected = True

        'Red Update
        If GenerateConfigs.Loot.RedLootKits IsNot Nothing Then
            TV_LootKit_Generated_Red.Nodes.Clear()
            TV_LootKit_Generated_Red.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.RedLootKits.proprietary, "proprietary"))
            TV_LootKit_Generated_Red.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.RedLootKits.medical, "medical"))
            TV_LootKit_Generated_Red.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.RedLootKits.food, "food"))
            TV_LootKit_Generated_Red.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.RedLootKits.drink, "drink"))
            TV_LootKit_Generated_Red.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.RedLootKits.tools, "tools"))
            TV_LootKit_Generated_Red.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.RedLootKits.material, "material"))
            TV_LootKit_Generated_Red.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.RedLootKits.misc, "misc"))
            TV_LootKit_Generated_Red.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.RedLootKits.valuable, "valuable "))
        End If
        'Purple Update
        If GenerateConfigs.Loot.PurpleLootKits IsNot Nothing Then
            TV_LootKit_Generated_Purple.Nodes.Clear()
            TV_LootKit_Generated_Purple.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.PurpleLootKits.proprietary, "proprietary"))
            TV_LootKit_Generated_Purple.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.PurpleLootKits.medical, "medical"))
            TV_LootKit_Generated_Purple.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.PurpleLootKits.food, "food"))
            TV_LootKit_Generated_Purple.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.PurpleLootKits.drink, "drink"))
            TV_LootKit_Generated_Purple.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.PurpleLootKits.tools, "tools"))
            TV_LootKit_Generated_Purple.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.PurpleLootKits.material, "material"))
            TV_LootKit_Generated_Purple.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.PurpleLootKits.misc, "misc"))
            TV_LootKit_Generated_Purple.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.PurpleLootKits.valuable, "valuable "))
        End If
        'Blue Update
        If GenerateConfigs.Loot.BlueLootKits IsNot Nothing Then
            TV_LootKit_Generated_Blue.Nodes.Clear()
            TV_LootKit_Generated_Blue.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.BlueLootKits.proprietary, "proprietary"))
            TV_LootKit_Generated_Blue.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.BlueLootKits.medical, "medical"))
            TV_LootKit_Generated_Blue.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.BlueLootKits.food, "food"))
            TV_LootKit_Generated_Blue.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.BlueLootKits.drink, "drink"))
            TV_LootKit_Generated_Blue.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.BlueLootKits.tools, "tools"))
            TV_LootKit_Generated_Blue.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.BlueLootKits.material, "material"))
            TV_LootKit_Generated_Blue.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.BlueLootKits.misc, "misc"))
            TV_LootKit_Generated_Blue.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.BlueLootKits.valuable, "valuable "))
        End If
        'Green Update
        If GenerateConfigs.Loot.GreenLootKits IsNot Nothing Then
            TV_LootKit_Generated_Green.Nodes.Clear()
            TV_LootKit_Generated_Green.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.GreenLootKits.proprietary, "proprietary"))
            TV_LootKit_Generated_Green.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.GreenLootKits.medical, "medical"))
            TV_LootKit_Generated_Green.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.GreenLootKits.food, "food"))
            TV_LootKit_Generated_Green.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.GreenLootKits.drink, "drink"))
            TV_LootKit_Generated_Green.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.GreenLootKits.tools, "tools"))
            TV_LootKit_Generated_Green.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.GreenLootKits.material, "material"))
            TV_LootKit_Generated_Green.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.GreenLootKits.misc, "misc"))
            TV_LootKit_Generated_Green.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.GreenLootKits.valuable, "valuable "))
        End If
        'Yellow Update
        If GenerateConfigs.Loot.YellowLootKits IsNot Nothing Then
            TV_LootKit_Generated_Yellow.Nodes.Clear()
            TV_LootKit_Generated_Yellow.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.YellowLootKits.proprietary, "proprietary"))
            TV_LootKit_Generated_Yellow.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.YellowLootKits.medical, "medical"))
            TV_LootKit_Generated_Yellow.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.YellowLootKits.food, "food"))
            TV_LootKit_Generated_Yellow.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.YellowLootKits.drink, "drink"))
            TV_LootKit_Generated_Yellow.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.YellowLootKits.tools, "tools"))
            TV_LootKit_Generated_Yellow.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.YellowLootKits.material, "material"))
            TV_LootKit_Generated_Yellow.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.YellowLootKits.misc, "misc"))
            TV_LootKit_Generated_Yellow.Nodes.Add(GenerateTreeViewNodes_Loot(GenerateConfigs.Loot.YellowLootKits.valuable, "valuable "))
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
        ' TV_Strongrooms.ItemsSource = GenerateConfigs.System.DNAConfigMainSystem_Strongrooms
    End Sub

    Public Sub seedHandlers()
        AddHandler TV_WeaponKits_Generated_Red.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_Kits
        AddHandler TV_WeaponKits_Generated_Purple.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_Kits
        AddHandler TV_WeaponKits_Generated_Blue.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_Kits
        AddHandler TV_WeaponKits_Generated_Green.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_Kits
        AddHandler TV_WeaponKits_Generated_Yellow.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_Kits

        AddHandler TV_WeaponKits_Generated_Red.ItemDeleting, AddressOf Event_Deleting_Generated_Kits
        AddHandler TV_WeaponKits_Generated_Purple.ItemDeleting, AddressOf Event_Deleting_Generated_Kits
        AddHandler TV_WeaponKits_Generated_Blue.ItemDeleting, AddressOf Event_Deleting_Generated_Kits
        AddHandler TV_WeaponKits_Generated_Green.ItemDeleting, AddressOf Event_Deleting_Generated_Kits
        AddHandler TV_WeaponKits_Generated_Yellow.ItemDeleting, AddressOf Event_Deleting_Generated_Kits


        AddHandler TV_ClothingKit_Generated_Red.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_Kits
        AddHandler TV_ClothingKit_Generated_Purple.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_Kits
        AddHandler TV_ClothingKit_Generated_Blue.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_Kits
        AddHandler TV_ClothingKit_Generated_Green.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_Kits
        AddHandler TV_ClothingKit_Generated_Yellow.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_Kits

        AddHandler TV_ClothingKit_Generated_Red.ItemDeleting, AddressOf Event_Deleting_Generated_Kits
        AddHandler TV_ClothingKit_Generated_Purple.ItemDeleting, AddressOf Event_Deleting_Generated_Kits
        AddHandler TV_ClothingKit_Generated_Blue.ItemDeleting, AddressOf Event_Deleting_Generated_Kits
        AddHandler TV_ClothingKit_Generated_Green.ItemDeleting, AddressOf Event_Deleting_Generated_Kits
        AddHandler TV_ClothingKit_Generated_Yellow.ItemDeleting, AddressOf Event_Deleting_Generated_Kits

        AddHandler TV_LootKit_Generated_Red.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_LootKits
        AddHandler TV_LootKit_Generated_Purple.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_LootKits
        AddHandler TV_LootKit_Generated_Blue.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_LootKits
        AddHandler TV_LootKit_Generated_Green.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_LootKits
        AddHandler TV_LootKit_Generated_Yellow.ItemBeginEdit, AddressOf Event_BeginEdit_Generated_LootKits

        AddHandler TV_LootKit_Generated_Red.ItemDeleting, AddressOf Event_Deleting_Generated_LootKits
        AddHandler TV_LootKit_Generated_Purple.ItemDeleting, AddressOf Event_Deleting_Generated_LootKits
        AddHandler TV_LootKit_Generated_Blue.ItemDeleting, AddressOf Event_Deleting_Generated_LootKits
        AddHandler TV_LootKit_Generated_Green.ItemDeleting, AddressOf Event_Deleting_Generated_LootKits
        AddHandler TV_LootKit_Generated_Yellow.ItemDeleting, AddressOf Event_Deleting_Generated_LootKits

        AddHandler Kits_ClearSidearms.Click, AddressOf ClearTB_Click
        AddHandler Kits_ClearRestricted.Click, AddressOf ClearTB_Click

        AddHandler Clothes_Helmets_Clear.Click, AddressOf ClearTB_Click
        AddHandler Clothes_Shirts_Clear.Click, AddressOf ClearTB_Click
        AddHandler Clothes_Vests_Clear.Click, AddressOf ClearTB_Click
        AddHandler Clothes_Pants_Clear.Click, AddressOf ClearTB_Click
        AddHandler Clothes_Shoes_Clear.Click, AddressOf ClearTB_Click
        AddHandler Clothes_Backpacks_Clear.Click, AddressOf ClearTB_Click
        AddHandler Clothes_Gloves_Clear.Click, AddressOf ClearTB_Click
        AddHandler Clothes_Belts_Clear.Click, AddressOf ClearTB_Click
        AddHandler Clothes_Facewear_Clear.Click, AddressOf ClearTB_Click
        AddHandler Clothes_Eyewear_Clear.Click, AddressOf ClearTB_Click
        AddHandler Clothes_Armbands_Clear.Click, AddressOf ClearTB_Click

        AddHandler Loot_proprietary_Clear.Click, AddressOf ClearTB_Click
        AddHandler Loot_medical_Clear.Click, AddressOf ClearTB_Click
        AddHandler Loot_food_Clear.Click, AddressOf ClearTB_Click
        AddHandler Loot_drink_Clear.Click, AddressOf ClearTB_Click
        AddHandler Loot_tools_Clear.Click, AddressOf ClearTB_Click
        AddHandler Loot_material_Clear.Click, AddressOf ClearTB_Click
        AddHandler Loot_misc_Clear.Click, AddressOf ClearTB_Click
        AddHandler Loot_valuable_Clear.Click, AddressOf ClearTB_Click

        AddHandler MainExe.Drop, AddressOf MainExeDropFiles
        AttachTextChangedEventToAllTextBoxes(Tab_ClothesSettings)
        AttachTextChangedEventToAllTextBoxes(Tab_Kits)
        AttachTextChangedEventToAllTextBoxes(Tab_LootSettings)
    End Sub
    Async Sub Event_Deleting_Generated_Kits(sender As Object, e As EventArgs)
        Dim EventInfo As ItemDeletingEventArgs = e
        Dim tree_ As SfTreeView = sender
        Dim xx = ""
        For Each _node As TreeViewNode In EventInfo.Nodes
            If _node.HasChildNodes Then
                EventInfo.Cancel = False
            Else
                EventInfo.Cancel = True
            End If
        Next
    End Sub
    Async Sub Event_Deleting_Generated_LootKits(sender As Object, e As EventArgs)
        Dim EventInfo As ItemDeletingEventArgs = e
        Dim tree_ As SfTreeView = sender
        Dim xx = ""
        For Each _node As TreeViewNode In EventInfo.Nodes
            If _node.HasChildNodes Then
                If _node.ParentNode Is Nothing Then
                    EventInfo.Cancel = True
                Else
                    EventInfo.Cancel = False
                End If
            Else
                EventInfo.Cancel = True
            End If
        Next
    End Sub
    Async Sub Event_BeginEdit_Generated_LootKits(sender As Object, e As EventArgs)
        Dim Edited_ As SfTreeView = sender
        Dim eventInfo As TreeViewItemBeginEditEventArgs = e
        eventInfo.Cancel = True
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

    Async Function determineIfAdd(type As GlobalVariables.Types.TypeInfo, Optional FilterAttatchments As Boolean = True) As Task(Of Boolean)
        'CHECK IF TYPE CONTAINS REQUIRED FLAGS FOR ADD
        '        'if selectedcats is not nothing then
        '        'for every selectedcat in selectedcats
        '        ' if type.section contains selected cat then
        '        'flag to add
        '        'else continue
        Dim selectedArgs


        If FilterAttatchments Then
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
    Private Sub TreeView_DeleteMiddleNode(sender As Object, e As TreeViewItemBeginEditEventArgs)

    End Sub

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
    Async Function ExportLootKitsToJson(filepath As String) As Task


        Dim list As New List(Of GenerateConfigs.Loot.LootInfoType)()
        Dim tTypeColorTrees As New Collection(Of SfTreeView)
        tTypeColorTrees.Add(TV_LootKit_Generated_Red)
        tTypeColorTrees.Add(TV_LootKit_Generated_Purple)
        tTypeColorTrees.Add(TV_LootKit_Generated_Blue)
        tTypeColorTrees.Add(TV_LootKit_Generated_Green)
        tTypeColorTrees.Add(TV_LootKit_Generated_Yellow)

        For Each t_TypeColorTree As SfTreeView In tTypeColorTrees
            'Build based on Shown
            If t_TypeColorTree.Nodes.First().Content.ToString() <> "Please Generate a Kit" Then
                For Each Node As TreeViewNode In t_TypeColorTree.Nodes
                    Dim tLootInfo As GenerateConfigs.Loot.LootInfoType
                    For Each Node2 In Node.ChildNodes
                        Try
                            tLootInfo = New GenerateConfigs.Loot.LootInfoType() With {
                            .dna_Tier = GetNodeContent(Node2, "dna_Tier"),
                            .dna_Category = GetNodeContent(Node2, "dna_Category"),
                            .dna_Type = GetNodeContent(Node2, "dna_Type")
                            }
                            list.Add(tLootInfo)
                        Catch ex As Exception
                            ' Log or handle the exception as needed
                            Console.WriteLine("An error occurred while processing a node: " & ex.Message)
                        End Try
                    Next
                Next
            End If
        Next

        Dim json = JsonConvert.SerializeObject(New With {Key .m_DNAConfig_Loot = list}, Formatting.Indented)
        File.WriteAllText(filepath, json)
    End Function
    'Public Function SortByDnaOption(list As List(Of GenerateConfigs.System.MainSystemSettings)) As List(Of GenerateConfigs.System.MainSystemSettings)
    '    ' Use LINQ to sort the list by dna_Option_Bak property
    '    Dim sortedList = list.OrderBy(Function(item) item.dna_Option).ToList()
    '    Return sortedList
    'End Function
    'Public Function SortByPrefix(list As List(Of GenerateConfigs.System.MainSystemSettings)) As List(Of GenerateConfigs.System.MainSystemSettings)
    '    ' Use LINQ to sort the list by the numeric value in the prefix
    '    Dim sortedList = list.OrderBy(Function(item) Integer.Parse(item.dna_Option.Substring(1))).ToList()
    '    Return sortedList
    'End Function

    Public Function SortByPrefix_SystemConfigExport(list As List(Of GenerateConfigs.System.MainSystemSettingsExport)) As List(Of GenerateConfigs.System.MainSystemSettingsExport)
        ' Use LINQ to sort the list by the numeric value in the prefix
        Dim sortedList = list.OrderBy(Function(item) GetPrefixValue(item.dna_Option)).ToList()
        Return sortedList
    End Function

    Private Function GetPrefixValue(value As String) As Integer
        Dim match As Match = Regex.Match(value, "\((\d+)\)")
        If match.Success Then
            Dim prefix As String = match.Groups(1).Value
            Return Integer.Parse(prefix)
        End If
        Return 0 ' Default value if prefix is not found
    End Function
    Async Function ExportSystemConfigToJson(filepath As String) As Task

        Dim json_ As String = ""

        Dim trees As New Collection(Of SfTreeView)
        trees.Add(TV_Strongrooms)
        trees.Add(TV_crates)
        trees.Add(TV_separation)
        trees.Add(TV_lockout)
        trees.Add(TV_keycard)
        trees.Add(TV_other)

        'm_DNAConfig_Version
        Dim tlist_Version As New List(Of GenerateConfigs.System.DNAConfigVersionExport)()
        Dim tVersion As New GenerateConfigs.System.DNAConfigVersionExport()
        tVersion.dna_ConfigVersion = GenerateConfigs.System.DNAConfigVersion.dna_ConfigVersion
        tVersion.dna_WarningMessage = GenerateConfigs.System.DNAConfigVersion.dna_WarningMessage
        tlist_Version.Add(tVersion)

        'm_DNAConfig_Main_System
        Dim tlist_System As New List(Of GenerateConfigs.System.MainSystemSettingsExport)()
        Dim TotalList As New List(Of GenerateConfigs.System.MainSystemSettings)
        GenerateConfigs.System.DNAConfigMainSystem_other.ToList().ForEach(Sub(item) TotalList.Add(item))
        GenerateConfigs.System.DNAConfigMainSystem_Crates.ToList().ForEach(Sub(item) TotalList.Add(item))
        GenerateConfigs.System.DNAConfigMainSystem_Strongrooms.ToList().ForEach(Sub(item) TotalList.Add(item))
        GenerateConfigs.System.DNAConfigMainSystem_Card.ToList().ForEach(Sub(item) TotalList.Add(item))
        GenerateConfigs.System.DNAConfigMainSystem_Separate.ToList().ForEach(Sub(item) TotalList.Add(item))
        GenerateConfigs.System.DNAConfigMainSystem_lockout.ToList().ForEach(Sub(item) TotalList.Add(item))

        For Each tree As SfTreeView In trees
            'Build based on Shown
            If tree.Nodes.Count <> 0 Then
                If tree.Nodes.First().Content.ToString() <> "Please import a config" Then
                    tree.SelectedItem = True
                    For Each Node As TreeViewNode In tree.Nodes
                        tlist_System.Add(New GenerateConfigs.System.MainSystemSettingsExport() With {.dna_Setting = Node.ChildNodes.Last().Content.ToString().TrimStart(" "), .dna_Option = Node.ChildNodes.Item(Node.ChildNodes.Count - 2).Content.ToString().TrimStart(" ")})
                    Next
                End If
            End If
        Next
        tlist_System = SortByPrefix_SystemConfigExport(tlist_System)

        Dim treesLoc As New Collection(Of SfTreeView)
        treesLoc.Add(TV_Location_Red_Strongroom)
        treesLoc.Add(TV_Location_Red_Crate)
        treesLoc.Add(TV_Location_Purple_Strongroom)
        treesLoc.Add(TV_Location_Purple_Crate)
        treesLoc.Add(TV_Location_Blue_Strongroom)
        treesLoc.Add(TV_Location_Blue_Crate)
        treesLoc.Add(TV_Location_Green_Strongroom)
        treesLoc.Add(TV_Location_Green_Crate)
        treesLoc.Add(TV_Location_Yellow_Strongroom)
        treesLoc.Add(TV_Location_Yellow_Crate)

        'Locations
        Dim tlist_Locations_Crate_Yellow As New List(Of GenerateConfigs.System.SpawnablePositionalData)()
        Dim tlist_Locations_Crate_Red As New List(Of GenerateConfigs.System.SpawnablePositionalData)()
        Dim tlist_Locations_Crate_Purple As New List(Of GenerateConfigs.System.SpawnablePositionalData)()
        Dim tlist_Locations_Crate_Blue As New List(Of GenerateConfigs.System.SpawnablePositionalData)()
        Dim tlist_Locations_Crate_Green As New List(Of GenerateConfigs.System.SpawnablePositionalData)()

        Dim tlist_Locations_Strongroom_Red As New List(Of GenerateConfigs.System.SpawnablePositionalData)()
        Dim tlist_Locations_Strongroom_Purple As New List(Of GenerateConfigs.System.SpawnablePositionalData)()
        Dim tlist_Locations_Strongroom_Blue As New List(Of GenerateConfigs.System.SpawnablePositionalData)()
        Dim tlist_Locations_Strongroom_Green As New List(Of GenerateConfigs.System.SpawnablePositionalData)()
        Dim tlist_Locations_Strongroom_Yellow As New List(Of GenerateConfigs.System.SpawnablePositionalData)()

        For Each tree As SfTreeView In treesLoc
            'Build based on Shown
            If tree.Nodes.Count <> 0 Then
                If tree.Nodes.First().Content.ToString() <> "Please import a config" Then
                    tree.SelectedItem = True
                    For Each Node As TreeViewNode In tree.Nodes
                        Dim chLocation = Node.ChildNodes.First()
                        Dim chRotation = Node.ChildNodes.Last()
                        Dim LocX = chLocation.ChildNodes.First().Content
                        Dim LocY = chLocation.ChildNodes.Item(1).Content
                        Dim LocZ = chLocation.ChildNodes.Last().Content
                        Dim RotX = chRotation.ChildNodes.First().Content
                        Dim RotY = chRotation.ChildNodes.Item(1).Content
                        Dim RotZ = chRotation.ChildNodes.Last().Content
                        Dim tPosiData As New GenerateConfigs.System.SpawnablePositionalData(LocX + " " + LocY + " " + LocZ, RotX + " " + RotY + " " + RotZ)

                        Select Case True
                            Case tree Is TV_Location_Red_Strongroom
                                tlist_Locations_Strongroom_Red.Add(tPosiData)
                            Case tree Is TV_Location_Purple_Strongroom
                                tlist_Locations_Strongroom_Purple.Add(tPosiData)
                            Case tree Is TV_Location_Blue_Strongroom
                                tlist_Locations_Strongroom_Blue.Add(tPosiData)
                            Case tree Is TV_Location_Green_Strongroom
                                tlist_Locations_Strongroom_Green.Add(tPosiData)
                            Case tree Is TV_Location_Yellow_Strongroom
                                tlist_Locations_Strongroom_Yellow.Add(tPosiData)
                            Case tree Is TV_Location_Red_Crate
                                tlist_Locations_Crate_Red.Add(tPosiData)
                            Case tree Is TV_Location_Purple_Crate
                                tlist_Locations_Crate_Purple.Add(tPosiData)
                            Case tree Is TV_Location_Blue_Crate
                                tlist_Locations_Crate_Blue.Add(tPosiData)
                            Case tree Is TV_Location_Green_Crate
                                tlist_Locations_Crate_Green.Add(tPosiData)
                            Case tree Is TV_Location_Yellow_Crate
                                tlist_Locations_Crate_Yellow.Add(tPosiData)
                        End Select
                    Next
                End If
            End If
        Next

        json_ = JsonConvert.SerializeObject(New With {
                                               Key .m_DNAConfig_Version = tlist_Version,
                                               Key .m_DNAConfig_Main_System = tlist_System,
                                               Key .m_DNAYellow_Crate_Locations = tlist_Locations_Crate_Yellow,
                                               Key .m_DNAGreen_Crate_Locations = tlist_Locations_Crate_Green,
                                               Key .m_DNABlue_Crate_Locations = tlist_Locations_Crate_Blue,
                                               Key .m_DNAPurple_Crate_Locations = tlist_Locations_Crate_Purple,
                                               Key .m_DNARed_Crate_Locations = tlist_Locations_Crate_Red,
                                               Key .m_DNAYellow_Strongroom_Locations = tlist_Locations_Strongroom_Yellow,
                                               Key .m_DNAGreen_Strongroom_Locations = tlist_Locations_Strongroom_Green,
                                               Key .m_DNABlue_Strongroom_Locations = tlist_Locations_Strongroom_Blue,
                                               Key .m_DNAPurple_Strongroom_Locations = tlist_Locations_Strongroom_Purple,
                                               Key .m_DNARed_Strongroom_Locations = tlist_Locations_Strongroom_Red}, Formatting.Indented)
        File.WriteAllText(filepath, json_)
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
    Private Async Sub LootKits_Generated_Export_Click(sender As Object, e As RoutedEventArgs) _
    Handles LootKit_Generated_Export.Click

        Dim tsaveFileDialog As New Forms.SaveFileDialog()

        ' Get the directory path of the executable
        Dim executableDirectory = AppDomain.CurrentDomain.BaseDirectory
        ' Set initial directory and filename
        tsaveFileDialog.InitialDirectory = executableDirectory ' Set your desired initial directory
        tsaveFileDialog.FileName = "KeyCard_General_Config"
        tsaveFileDialog.DefaultExt = ".json"
        tsaveFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"

        Dim result As Nullable(Of Boolean) = tsaveFileDialog.ShowDialog()

        If result = True Then
            Await ExportLootKitsToJson(tsaveFileDialog.FileName)
            Return
        Else
            Windows.MessageBox.Show("Export Canceled.", "Alert", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information)
            Return
        End If
    End Sub
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

    Private Async Sub Kits_ImportSystemConfig_Click(sender As Object, e As RoutedEventArgs) Handles Kits_ImportSystemConfig.Click
        Dim resultPath As String = Await FileSelectionHelper.SelectSingleFileAsync()
        Await FileSelectionHelper.ImportSystemConfigJSON(resultPath)
        Await UpdateSystemConfigTab()
    End Sub
    Public Shared Function SeparateStrings(ByVal inputString As String) As (String, String)
        Dim pattern As String = "\([0-9]+\)\s(.+)"
        Dim regex As New Regex(pattern)
        Dim match As Match = regex.Match(inputString)

        Dim separatedA As String = ""
        Dim separatedB As String = ""

        If match.Success Then
            separatedA = match.Groups(0).Value.Trim()
            separatedB = match.Groups(1).Value.Trim()
        End If

        Return (separatedA, separatedB)
    End Function
    Public Function ExtractCFGSettingName(ByVal inputString As String) As String
        Dim pattern As String = "\)(.*?)\("
        Dim match As Match = Regex.Match(inputString, pattern)

        If match.Success Then
            Return match.Groups(1).Value.Trim()
        Else
            Return String.Empty
        End If
    End Function
    Public Function ExtractTextAfterFirstOccurrence(ByVal InputString As String) As String
        ' Find the index of the first occurrence of '('
        Dim firstIndex As Integer = InputString.IndexOf("(")
        ' If the first occurrence is found, search for the second occurrence
        If firstIndex <> -1 Then
            ' Starting from the position after the first occurrence, find the index of the second occurrence
            Dim secondIndex As Integer = InputString.IndexOf("(", firstIndex + 1)
            If secondIndex <> -1 Then
                Console.WriteLine("Index of the second occurrence of '(': " & secondIndex)
                Dim extractedText As String = InputString.Substring(secondIndex).TrimStart()
                Return extractedText
            Else
                'Console.WriteLine("Second occurrence of '(' not found.")
            End If
        Else
            'Console.WriteLine("First occurrence of '(' not found.")
        End If

    End Function
    Function RemoveSubstring(originalString As String, substringToRemove As String) As String
        Dim pattern As String = Regex.Escape(substringToRemove)
        Dim result As String = Regex.Replace(originalString, pattern, "", RegexOptions.IgnoreCase)
        Return result
    End Function

    Async Function UpdateSystemConfigTab() As Task
        Tab_SystemConfig.IsSelected = True

        If GenerateConfigs.System.DNAConfigMainSystem_Strongrooms IsNot Nothing Then
            TV_Strongrooms.Nodes.Clear()
            For Each setting_ As GenerateConfigs.System.MainSystemSettings In GenerateConfigs.System.DNAConfigMainSystem_Strongrooms

                Dim tHeader As New TreeViewNode With {.Content = RemoveSubstring(ExtractCFGSettingName(setting_.dna_Option), "Strongrooms ").Replace(" -", "")}
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = "TIP:" + ExtractTextAfterFirstOccurrence(setting_.HelpText)})
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = "                                                                                                                                                                              " + setting_.dna_Option_Bak})
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = setting_.dna_Setting})
                TV_Strongrooms.Nodes.Add(tHeader)
            Next
        End If

        If GenerateConfigs.System.DNAConfigMainSystem_Crates IsNot Nothing Then
            TV_crates.Nodes.Clear()
            For Each setting_ As GenerateConfigs.System.MainSystemSettings In GenerateConfigs.System.DNAConfigMainSystem_Crates

                Dim tHeader As New TreeViewNode With {.Content = RemoveSubstring(ExtractCFGSettingName(setting_.dna_Option), "crates ").Replace(" -", "")}
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = "TIP:" + ExtractTextAfterFirstOccurrence(setting_.HelpText)})
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = "                                                                                                                                                                              " + setting_.dna_Option_Bak})
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = setting_.dna_Setting})
                TV_crates.Nodes.Add(tHeader)
            Next
        End If

        If GenerateConfigs.System.DNAConfigMainSystem_lockout IsNot Nothing Then
            TV_lockout.Nodes.Clear()
            For Each setting_ As GenerateConfigs.System.MainSystemSettings In GenerateConfigs.System.DNAConfigMainSystem_lockout

                Dim tHeader As New TreeViewNode With {.Content = RemoveSubstring(ExtractCFGSettingName(setting_.dna_Option), "lockout ").Replace(" -", "")}
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = "TIP:" + ExtractTextAfterFirstOccurrence(setting_.HelpText)})
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = "                                                                                                                                                                              " + setting_.dna_Option_Bak})
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = setting_.dna_Setting})
                TV_lockout.Nodes.Add(tHeader)
            Next
        End If

        If GenerateConfigs.System.DNAConfigMainSystem_Card IsNot Nothing Then
            TV_keycard.Nodes.Clear()
            For Each setting_ As GenerateConfigs.System.MainSystemSettings In GenerateConfigs.System.DNAConfigMainSystem_Card

                Dim tHeader As New TreeViewNode With {.Content = RemoveSubstring(setting_.dna_Option, "card ").Replace(" -", "")}
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = "TIP: ~~~~"})
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = "                                                                                                                                                                              " + setting_.dna_Option_Bak})
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = setting_.dna_Setting})
                TV_keycard.Nodes.Add(tHeader)
            Next
        End If

        If GenerateConfigs.System.DNAConfigMainSystem_Separate IsNot Nothing Then
            TV_separation.Nodes.Clear()
            For Each setting_ As GenerateConfigs.System.MainSystemSettings In GenerateConfigs.System.DNAConfigMainSystem_Separate

                Dim tHeader As New TreeViewNode With {.Content = RemoveSubstring(ExtractCFGSettingName(setting_.dna_Option), "Separate ").Replace(" -", "")}
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = "TIP:" + ExtractTextAfterFirstOccurrence(setting_.HelpText)})
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = "                                                                                                                                                                              " + setting_.dna_Option_Bak})
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = setting_.dna_Setting})
                TV_separation.Nodes.Add(tHeader)
            Next
        End If
        If GenerateConfigs.System.DNAConfigMainSystem_other IsNot Nothing Then
            TV_other.Nodes.Clear()
            For Each setting_ As GenerateConfigs.System.MainSystemSettings In GenerateConfigs.System.DNAConfigMainSystem_other

                Dim tHeader As New TreeViewNode With {.Content = ExtractCFGSettingName(setting_.dna_Option)}
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = "TIP:" + ExtractTextAfterFirstOccurrence(setting_.HelpText)})
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = "                                                                                                                                                                              " + setting_.dna_Option_Bak})
                tHeader.ChildNodes.Add(New TreeViewNode() With {.Content = setting_.dna_Setting})
                TV_other.Nodes.Add(tHeader)
            Next
        End If


        ''''''''''LOCATION

        '''''''RED
        Tab_MainLoc_Red.IsSelected = True
        If GenerateConfigs.System.Locations.Strongroom.Red IsNot Nothing Then
            TV_Location_Red_Strongroom.Nodes.Clear()
            Dim tCount = 1
            For Each setting_ As GenerateConfigs.System.SpawnablePositionalData In GenerateConfigs.System.Locations.Strongroom.Red
                TV_Location_Red_Strongroom.Nodes.Add(Await LocationSpawnNodeBuilder(setting_, tCount))
                tCount += 1
            Next
        End If

        If GenerateConfigs.System.Locations.Crate.Red IsNot Nothing Then
            TV_Location_Red_Crate.Nodes.Clear()
            Dim tCount = 1
            For Each setting_ As GenerateConfigs.System.SpawnablePositionalData In GenerateConfigs.System.Locations.Crate.Red
                TV_Location_Red_Crate.Nodes.Add(Await LocationSpawnNodeBuilder(setting_, tCount))
                tCount += 1
            Next
        End If

        '''''''Purple
        Tab_MainLoc_Purple.IsSelected = True
        If GenerateConfigs.System.Locations.Strongroom.Purple IsNot Nothing Then
            TV_Location_Purple_Strongroom.Nodes.Clear()
            Dim tCount = 1
            For Each setting_ As GenerateConfigs.System.SpawnablePositionalData In GenerateConfigs.System.Locations.Strongroom.Purple
                TV_Location_Purple_Strongroom.Nodes.Add(Await LocationSpawnNodeBuilder(setting_, tCount))
                tCount += 1
            Next
        End If

        If GenerateConfigs.System.Locations.Crate.Purple IsNot Nothing Then
            TV_Location_Purple_Crate.Nodes.Clear()
            Dim tCount = 1
            For Each setting_ As GenerateConfigs.System.SpawnablePositionalData In GenerateConfigs.System.Locations.Crate.Purple
                TV_Location_Purple_Crate.Nodes.Add(Await LocationSpawnNodeBuilder(setting_, tCount))
                tCount += 1
            Next
        End If
        '''''''Blue
        Tab_MainLoc_Blue.IsSelected = True
        If GenerateConfigs.System.Locations.Strongroom.Blue IsNot Nothing Then
            TV_Location_Blue_Strongroom.Nodes.Clear()
            Dim tCount = 1
            For Each setting_ As GenerateConfigs.System.SpawnablePositionalData In GenerateConfigs.System.Locations.Strongroom.Blue
                TV_Location_Blue_Strongroom.Nodes.Add(Await LocationSpawnNodeBuilder(setting_, tCount))
                tCount += 1
            Next
        End If

        If GenerateConfigs.System.Locations.Crate.Blue IsNot Nothing Then
            TV_Location_Blue_Crate.Nodes.Clear()
            Dim tCount = 1
            For Each setting_ As GenerateConfigs.System.SpawnablePositionalData In GenerateConfigs.System.Locations.Crate.Blue
                TV_Location_Blue_Crate.Nodes.Add(Await LocationSpawnNodeBuilder(setting_, tCount))
                tCount += 1
            Next
        End If
        '''''''Green
        Tab_MainLoc_Green.IsSelected = True
        If GenerateConfigs.System.Locations.Strongroom.Green IsNot Nothing Then
            TV_Location_Green_Strongroom.Nodes.Clear()
            Dim tCount = 1
            For Each setting_ As GenerateConfigs.System.SpawnablePositionalData In GenerateConfigs.System.Locations.Strongroom.Green
                TV_Location_Green_Strongroom.Nodes.Add(Await LocationSpawnNodeBuilder(setting_, tCount))
                tCount += 1
            Next
        End If

        If GenerateConfigs.System.Locations.Crate.Green IsNot Nothing Then
            TV_Location_Green_Crate.Nodes.Clear()
            Dim tCount = 1
            For Each setting_ As GenerateConfigs.System.SpawnablePositionalData In GenerateConfigs.System.Locations.Crate.Green
                TV_Location_Green_Crate.Nodes.Add(Await LocationSpawnNodeBuilder(setting_, tCount))
                tCount += 1
            Next
        End If
        '''''''Yellow
        Tab_MainLoc_Yellow.IsSelected = True
        If GenerateConfigs.System.Locations.Strongroom.Yellow IsNot Nothing Then
            TV_Location_Yellow_Strongroom.Nodes.Clear()
            Dim tCount = 1
            For Each setting_ As GenerateConfigs.System.SpawnablePositionalData In GenerateConfigs.System.Locations.Strongroom.Yellow
                TV_Location_Yellow_Strongroom.Nodes.Add(Await LocationSpawnNodeBuilder(setting_, tCount))
                tCount += 1
            Next
        End If

        If GenerateConfigs.System.Locations.Crate.Yellow IsNot Nothing Then
            TV_Location_Yellow_Crate.Nodes.Clear()
            Dim tCount = 1
            For Each setting_ As GenerateConfigs.System.SpawnablePositionalData In GenerateConfigs.System.Locations.Crate.Yellow
                TV_Location_Yellow_Crate.Nodes.Add(Await LocationSpawnNodeBuilder(setting_, tCount))
                tCount += 1
            Next
        End If



        Return

    End Function

    Async Function LocationSpawnNodeBuilder(setting_ As GenerateConfigs.System.SpawnablePositionalData, tCount As Double) As Task(Of TreeViewNode)
        Dim tHeader As New TreeViewNode With {.Content = "StrongRoom: # " + tCount.ToString()}
        Dim tLocation As New TreeViewNode With {.Content = "Location: (X Y Z)"}
        Dim tRotation As New TreeViewNode With {.Content = "Rotation: (X Y Z)"}
        Dim tLocArr = setting_.dna_Location.Split(" ")
        Dim tRotArr = setting_.dna_Rotation.Split(" ")
        For Each tLoc In tLocArr
            tLocation.ChildNodes.Add(New TreeViewNode() With {.Content = tLoc})
        Next
        For Each tRot In tRotArr
            tRotation.ChildNodes.Add(New TreeViewNode() With {.Content = tRot})
        Next
        tHeader.ChildNodes.Add(tLocation)
        tHeader.ChildNodes.Add(tRotation)
        Return tHeader
    End Function

    Private Async Sub Kits_ImportSystemConfig_MAP_Click(sender As Object, e As RoutedEventArgs) Handles Kits_ImportSystemConfig_MAP.Click
        Dim whiteListTypes As New List(Of String)
        With whiteListTypes
            .Add("DNA_Crate_Red")
            .Add("DNA_Crate_Green")
            .Add("DNA_Crate_Blue")
            .Add("DNA_Crate_Purple")
            .Add("DNA_Crate_Yellow")
            .Add("DNA_Strongroom_Red")
            .Add("DNA_Strongroom_Purple")
            .Add("DNA_Strongroom_Blue")
            .Add("DNA_Strongroom_Green")
            .Add("DNA_Strongroom_Yellow")
        End With
        Dim results As String() = Await FileSelectionHelper.SelectMultipleFilesAsync()
        Dim foundTypes As List(Of String)
        For Each resultPath As String In results
            foundTypes = New List(Of String)()

            Dim entries = Await FileSelectionHelper.LinkerAddressMapParser.ParseLinkerAddressMapFile(resultPath)
            Await FileSelectionHelper.LinkerAddressMapParser.RemoveNodesByNames(entries, whiteListTypes)
            Await FileSelectionHelper.LinkerAddressMapParser.ParseToUsable(entries)
            Await UpdateSystemConfigTab()




        Next
    End Sub

    Private Async Sub Kits_ExportSystemConfig_Click(sender As Object, e As RoutedEventArgs) Handles Kits_ExportSystemConfig.Click
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
            Await ExportSystemConfigToJson(tsaveFileDialog.FileName)
            Return
        Else
            Windows.MessageBox.Show("Export Canceled.", "Alert", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information)
            Return
        End If
    End Sub
End Class

