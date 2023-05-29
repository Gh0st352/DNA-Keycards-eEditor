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

Imports System.Collections.ObjectModel

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

    Public Class Clothes
        Public Class ClothesInfo
            Public Property dna_Tier As String
            Public Property dna_Helm As String = ""
            Public Property dna_Shirt As String = ""
            Public Property dna_Vest As String = ""
            Public Property dna_Pants As String = ""
            Public Property dna_Shoes As String = ""
            Public Property dna_Backpack As String = ""
            Public Property dna_Gloves As String = ""
            Public Property dna_Belt As String = ""
            Public Property dna_Facewear As String = ""
            Public Property dna_Eyewear As String = ""
            Public Property dna_Armband As String = ""
            Public Property dna_NVG As String = ""
        End Class

        Public Shared RedClothesKits As ObservableCollection(Of ClothesInfo)
        Public Shared PurpleClothesKits As ObservableCollection(Of ClothesInfo)
        Public Shared BlueClothesKits As ObservableCollection(Of ClothesInfo)
        Public Shared GreenClothesKits As ObservableCollection(Of ClothesInfo)
        Public Shared YellowClothesKits As ObservableCollection(Of ClothesInfo)
    End Class
    Public Class Loot
        Public Class LootInfoType

            Public Property dna_Tier As String
            Public Property dna_Category As String = ""
            Public Property dna_Type As String = ""
        End Class

        Public Class LootInfoCats

            Public proprietary As Collection(Of LootInfoType)
            Public medical As Collection(Of LootInfoType)
            Public food As Collection(Of LootInfoType)
            Public drink As Collection(Of LootInfoType)
            Public tools As Collection(Of LootInfoType)
            Public material As Collection(Of LootInfoType)
            Public misc As Collection(Of LootInfoType)
            Public valuable As Collection(Of LootInfoType)


        End Class

        Public Shared RedLootKits As LootInfoCats
        Public Shared PurpleLootKits As LootInfoCats
        Public Shared BlueLootKits As LootInfoCats
        Public Shared GreenLootKits As LootInfoCats
        Public Shared YellowLootKits As LootInfoCats
    End Class
    Public Class System
        Public Class DNAConfigVersion
            Public Property dna_WarningMessage As String
            Public Property dna_ConfigVersion As Integer
        End Class

        Public Shared DNAConfigMainSystem As ObservableCollection(Of MainSystemSettings)

        Public Class MainSystemSettings
            Public Property dna_Option As String
            Public Property dna_Setting As Integer
        End Class


        Public Class Locations
            Public Class Crate

                Public Shared Red As ObservableCollection(Of SpawnablePositionalData)
                Public Shared Purple As ObservableCollection(Of SpawnablePositionalData)
                Public Shared Blue As ObservableCollection(Of SpawnablePositionalData)
                Public Shared Green As ObservableCollection(Of SpawnablePositionalData)
                Public Shared Yellow As ObservableCollection(Of SpawnablePositionalData)

            End Class
            Public Class Strongroom

                Public Shared Red As ObservableCollection(Of SpawnablePositionalData)
                Public Shared Purple As ObservableCollection(Of SpawnablePositionalData)
                Public Shared Blue As ObservableCollection(Of SpawnablePositionalData)
                Public Shared Green As ObservableCollection(Of SpawnablePositionalData)
                Public Shared Yellow As ObservableCollection(Of SpawnablePositionalData)

            End Class

        End Class

        Public Class SpawnablePositionalData
            Public Property dna_Location As String
            Public Property dna_Rotation As String
        End Class


        'Dim m_DNAYellow_Crate_Locations As ObservableCollection(Of SpawnablePositionalData)
        'Dim m_DNAGreen_Crate_Locations As ObservableCollection(Of SpawnablePositionalData)
        'Dim m_DNABlue_Crate_Locations As ObservableCollection(Of SpawnablePositionalData)
        'Dim m_DNAPurple_Crate_Locations As ObservableCollection(Of SpawnablePositionalData)
        'Dim m_DNARed_Crate_Locations As ObservableCollection(Of SpawnablePositionalData)
        'Dim m_DNAYellow_Strongroom_Locations As ObservableCollection(Of SpawnablePositionalData)
        'Dim m_DNAGreen_Strongroom_Locations As ObservableCollection(Of SpawnablePositionalData)
        'Dim m_DNABlue_Strongroom_Locations As ObservableCollection(Of SpawnablePositionalData)
        'Dim m_DNAPurple_Strongroom_Locations As ObservableCollection(Of SpawnablePositionalData)
        'Dim m_DNARed_Strongroom_Locations As ObservableCollection(Of SpawnablePositionalData)



    End Class
End Class

