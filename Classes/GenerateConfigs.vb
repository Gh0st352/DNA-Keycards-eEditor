'--- DNA Keycards Economy Editor
'-- ------------------------------------------------------------------------------------------
'-- 
'-- This Is free software, And you are welcome to redistribute it with certain conditions.
'-- 
'-- --------------------- GNU General Public License v3.0 ------------------------------------
'-- 
'-- This program Is free software: you can redistribute it And/Or modify it under the terms 
'-- of the GNU General Public License as published by the Free Software Foundation, 
'-- Version 3 of the License.
'--
'-- This program Is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
'-- without even the implied warranty of MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.
'-- See the GNU General Public License for more details.
'--
'-- You should have received a copy of the GNU General Public License in the downloaded 
'-- program folder.  If Not, see https://www.gnu.org/licenses/.
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
End Class

