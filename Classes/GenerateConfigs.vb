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
                Public Property dna_Magazine As String = ""
                Public Property dna_Ammunition As String = ""
                Public Property dna_OpticType As String = ""
                Public Property dna_Suppressor As String = ""
                Public Property dna_UnderBarrel As String = ""
                Public Property dna_ButtStock As String = ""
                Public Property dna_HandGuard As String = ""
                Public Property dna_Wrap As String = ""
            End Class
            Public Shared Sub ExportTypesToJson(t_Types As ObservableCollection(Of GlobalVariables.Types.TypeInfo), fileName As String, t_tier As String)
                Dim weaponList As New List(Of WeaponInfo)()

                For Each type In t_Types
                    Dim weapon As New WeaponInfo()


                    weapon.dna_Tier = t_tier
                    weapon.dna_TheChosenOne = type.typename
                    ' Set other properties as needed

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
