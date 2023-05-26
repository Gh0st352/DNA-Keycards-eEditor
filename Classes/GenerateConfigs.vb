Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Threading.Tasks
Imports Newtonsoft.Json

Namespace Classes
    Public Class GenerateConfigs
        Public Class Weapons

            Public Shared Async Function GenerateConfig(t_tier As String) As Task
                ExportTypesToJson(GlobalVariables.Types.Types, "KeyCard_Weapons_Config.json", t_tier)



            End Function


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
            Public Shared Function StringExistsInCollection(ByVal searchString As String, ByVal collection As ObservableCollection(Of String)) As Boolean
                Return collection.Any(Function(item) String.Equals(item, searchString, StringComparison.OrdinalIgnoreCase))
            End Function
            Public Shared Sub ExportTypesToJson(t_Types As ObservableCollection(Of GlobalVariables.Types.TypeInfo), fileName As String, t_tier As String)
                Dim weaponList As New List(Of WeaponInfo)()

                For Each type In t_Types
                    Dim weapon As New WeaponInfo()
                    weapon.dna_Tier = t_tier
                    weapon.dna_TheChosenOne = type.typename
                    Dim exists As Boolean = StringExistsInCollection(type.typename, GlobalVariables.SideArms)
                    If exists Then
                        weapon.dna_WeaponCategory = "side"
                    End If

                    'CHECK IF TYPE CONTAINS REQUIRED FLAGS FOR ADD
                    'if selectedcats is not nothing then
                    'for every selectedcat in selectedcats
                    ' if type.section contains selected cat then
                    'flag to add
                    'else continue









                    weaponList.Add(weapon)
                Next

                Dim json = JsonConvert.SerializeObject(New With {Key .m_DNAConfig_Weapons = weaponList}, Formatting.Indented)
                ' Get the directory path of the executable
                Dim executableDirectory = AppDomain.CurrentDomain.BaseDirectory

                ' Combine the directory path with the provided file name
                Dim filePath = Path.Combine(executableDirectory, fileName)

                ' Write the JSON to the file
                File.WriteAllText(filePath, json)
                'File.WriteAllText(filePath, json)
            End Sub
        End Class
    End Class
End Namespace
