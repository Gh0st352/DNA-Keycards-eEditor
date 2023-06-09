﻿'--- DNA Keycards Economy Editor
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
Imports Syncfusion.UI.Xaml.Grid
Imports System.Xml
Imports Newtonsoft.Json
Imports Syncfusion.UI.Xaml.TreeGrid
Imports Syncfusion.Data
Imports Newtonsoft.Json.Linq


Namespace Classes
    Public Class FileSelectionHelper
        Public Shared Function FindMemberByDNAOptionBak(list As List(Of GenerateConfigs.System.MainSystemSettings), searchString As String) As GenerateConfigs.System.MainSystemSettings
            For Each member As GenerateConfigs.System.MainSystemSettings In list
                If StringContainsText(searchString.ToLower(), member.dna_Option_Bak.ToLower()) Then
                    Return member
                End If
            Next

            Return Nothing ' Return Nothing if no match is found
        End Function

        Public Shared Function StringContainsText(inputString As String, searchText As String) As Boolean
            Return inputString.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
        End Function
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
        Public Shared Async Function SelectSingleFileAsync() As Task(Of String)
            Dim openFileDialog As New OpenFileDialog()
            openFileDialog.Multiselect = False

            Dim fileNames As String = Await Application.Current.Dispatcher.InvokeAsync(Function()
                                                                                           If openFileDialog.ShowDialog() = True Then
                                                                                               Return openFileDialog.FileName
                                                                                           Else
                                                                                               Return ""
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
                        Dim exists As Boolean = IsStringExistsInCollection(GlobalVariables.Types.Categories,
                                                                           _Type.category)
                        If exists Then
                        Else
                            GlobalVariables.Types.Categories.Add(
                                New GlobalVariables.Types.CategoryInfo() _
                                                                    With {.Checked = False, .Name = _Type.category})
                        End If

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
                                UpdateCollectionsList(tempstr, GlobalVariables.Types.Usages, att_)
                            Case "tag"
                                _Type.tag = tempstr
                                UpdateCollectionsList(tempstr, GlobalVariables.Types.Tags, att_)
                            Case "value"
                                _Type.value = tempstr
                                UpdateCollectionsList(tempstr, GlobalVariables.Types.Values, att_)
                            Case Else
                        End Select
                    End If
                Next
                GlobalVariables.Types.Types.Add(_Type)
            Next
            RemoveDuplicatesFromTypes(GlobalVariables.Types.Types)
        End Function
        Shared Sub RemoveDuplicatesFromTypes(collection As ObservableCollection(Of GlobalVariables.Types.TypeInfo))
            Dim uniqueItems As New ObservableCollection(Of GlobalVariables.Types.TypeInfo)()

            For Each item In collection
                If Not uniqueItems.Any(Function(x) x.typename = item.typename AndAlso
                                                   x.nominal = item.nominal AndAlso
                                                   x.lifetime = item.lifetime AndAlso
                                                   x.restock = item.restock AndAlso
                                                   x.min = item.min AndAlso
                                                   x.quantmin = item.quantmin AndAlso
                                                   x.quantmax = item.quantmax AndAlso
                                                   x.cost = item.cost AndAlso
                                                   x.flags = item.flags AndAlso
                                                   x.category = item.category AndAlso
                                                   x.usage = item.usage AndAlso
                                                   x.tag = item.tag AndAlso
                                                   x.value = item.value) Then
                    uniqueItems.Add(item)
                End If
            Next

            ' Replace the original collection with the unique items
            collection.Clear()
            For Each item In uniqueItems
                collection.Add(item)
            Next
        End Sub
        Public Shared Function IsStringExistsInCollection(Of T)(collection As ObservableCollection(Of T),
                                                                 searchString As String) As Boolean
            For Each item As T In collection
                Dim properties = item.GetType().GetProperties()
                For Each prop In properties
                    If prop.PropertyType = GetType(String) AndAlso prop.GetValue(item)?.ToString() = searchString Then
                        Return True
                    End If
                Next
            Next
            Return False
        End Function

        Public Shared Function SeparateString(ByVal inputString As String) As String()
            Dim separator As String = ","
            Dim separatedArray As String() = inputString.Split({separator}, StringSplitOptions.RemoveEmptyEntries)
            Return separatedArray
        End Function

        Public Shared Sub UpdateCollectionsList(Of T)(searchString As String,
                                                       CollectionObject As ObservableCollection(Of T), type As String)
            Dim tCollectionObject As ObservableCollection(Of T) = CollectionObject
            Dim TempStringArr As String() = SeparateString(searchString)
            For Each _str In TempStringArr
                Dim exists As Boolean = IsStringExistsInCollection(Of T)(tCollectionObject, _str)
                If exists Then
                    Continue For
                Else
                    AddOptionToCollection(_str, type)
                End If
            Next
        End Sub

        Public Shared Sub AddOptionToCollection(str As String, type As String)
            Select Case type
                Case "usage"
                    GlobalVariables.Types.Usages.Add(
                        New GlobalVariables.Types.UsageInfo() _
                                                        With {.Checked = False, .Name = str})
                Case "tag"
                    GlobalVariables.Types.Tags.Add(
                        New GlobalVariables.Types.TagInfo() _
                                                      With {.Checked = False, .Name = str})
                Case "value"
                    GlobalVariables.Types.Values.Add(
                        New GlobalVariables.Types.ValueInfo() _
                                                        With {.Checked = False, .Name = str})
                Case Else
            End Select
        End Sub

        Public Shared Async Function GetUniqueClassnamesAndVariants(jsonFilePath As String) As Task(Of List(Of String))
            Dim classNamesAndVariants As New HashSet(Of String)()

            ' Read the JSON file
            Using reader As StreamReader = File.OpenText(jsonFilePath)
                Dim jsonText As String = Await reader.ReadToEndAsync()

                ' Deserialize the JSON data
                Dim jsonData As JObject = JsonConvert.DeserializeObject(Of JObject)(jsonText)

                ' Get the "Items" array
                Dim itemsArray As JArray = jsonData("Items")

                ' Iterate through each item
                For Each item As JObject In itemsArray
                    ' Get the "ClassName" value
                    Dim className As String = item("ClassName").ToString()

                    ' Add the "ClassName" value to the hash set if it's not already present
                    classNamesAndVariants.Add(className)

                    ' Get the "Variants" array
                    Dim variantsArray As JArray = item("Variants")

                    ' Iterate through each variant
                    For Each tvariant As JToken In variantsArray
                        ' Get the variant string
                        Dim variantString As String = tvariant.ToString()

                        ' Add the variant string to the hash set if it's not already present
                        classNamesAndVariants.Add(variantString)
                    Next
                Next
            End Using

            ' Return the list of unique "classname" and "variants" strings
            Return classNamesAndVariants.ToList()
        End Function
        Public Class LinkerAddressMapParser
            Public Shared Async Function ParseToUsable(entries As List(Of LinkerAddressMapEntry)) As Task
                Dim tLocRedCrate As ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData) = GenerateConfigs.System.Locations.Crate.Red
                Dim tLocPCrate As ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData) = GenerateConfigs.System.Locations.Crate.Purple
                Dim tLocBCrate As ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData) = GenerateConfigs.System.Locations.Crate.Blue
                Dim tLocGCrate As ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData) = GenerateConfigs.System.Locations.Crate.Green
                Dim tLocYCrate As ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData) = GenerateConfigs.System.Locations.Crate.Yellow

                Dim tLocR_SR As ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData) = GenerateConfigs.System.Locations.Strongroom.Red
                Dim tLocP_SR As ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData) = GenerateConfigs.System.Locations.Strongroom.Purple
                Dim tLocB_SR As ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData) = GenerateConfigs.System.Locations.Strongroom.Blue
                Dim tLocG_SR As ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData) = GenerateConfigs.System.Locations.Strongroom.Green
                Dim tLocY_SR As ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData) = GenerateConfigs.System.Locations.Strongroom.Yellow


                For Each entry_ In entries
                    Select Case True
                        Case StringContainsText(entry_.Name.ToLower(), "crate_red")
                            tLocRedCrate.Add(New GenerateConfigs.System.SpawnablePositionalData(entry_.Position.X.ToString() + " " + entry_.Position.Y.ToString() + " " + entry_.Position.Z.ToString(), entry_.Rotation.X.ToString() + " " + entry_.Rotation.Y.ToString() + " " + entry_.Rotation.Z.ToString()))
                        Case StringContainsText(entry_.Name.ToLower(), "crate_purple")
                            tLocPCrate.Add(New GenerateConfigs.System.SpawnablePositionalData(entry_.Position.X.ToString() + " " + entry_.Position.Y.ToString() + " " + entry_.Position.Z.ToString(), entry_.Rotation.X.ToString() + " " + entry_.Rotation.Y.ToString() + " " + entry_.Rotation.Z.ToString()))
                        Case StringContainsText(entry_.Name.ToLower(), "crate_blue")
                            tLocBCrate.Add(New GenerateConfigs.System.SpawnablePositionalData(entry_.Position.X.ToString() + " " + entry_.Position.Y.ToString() + " " + entry_.Position.Z.ToString(), entry_.Rotation.X.ToString() + " " + entry_.Rotation.Y.ToString() + " " + entry_.Rotation.Z.ToString()))
                        Case StringContainsText(entry_.Name.ToLower(), "crate_green")
                            tLocGCrate.Add(New GenerateConfigs.System.SpawnablePositionalData(entry_.Position.X.ToString() + " " + entry_.Position.Y.ToString() + " " + entry_.Position.Z.ToString(), entry_.Rotation.X.ToString() + " " + entry_.Rotation.Y.ToString() + " " + entry_.Rotation.Z.ToString()))
                        Case StringContainsText(entry_.Name.ToLower(), "crate_yellow")
                            tLocYCrate.Add(New GenerateConfigs.System.SpawnablePositionalData(entry_.Position.X.ToString() + " " + entry_.Position.Y.ToString() + " " + entry_.Position.Z.ToString(), entry_.Rotation.X.ToString() + " " + entry_.Rotation.Y.ToString() + " " + entry_.Rotation.Z.ToString()))


                        Case StringContainsText(entry_.Name.ToLower(), "strongroom_red")
                            tLocR_SR.Add(New GenerateConfigs.System.SpawnablePositionalData(entry_.Position.X.ToString() + " " + entry_.Position.Y.ToString() + " " + entry_.Position.Z.ToString(), entry_.Rotation.X.ToString() + " " + entry_.Rotation.Y.ToString() + " " + entry_.Rotation.Z.ToString()))
                        Case StringContainsText(entry_.Name.ToLower(), "strongroom_purple")
                            tLocP_SR.Add(New GenerateConfigs.System.SpawnablePositionalData(entry_.Position.X.ToString() + " " + entry_.Position.Y.ToString() + " " + entry_.Position.Z.ToString(), entry_.Rotation.X.ToString() + " " + entry_.Rotation.Y.ToString() + " " + entry_.Rotation.Z.ToString()))
                        Case StringContainsText(entry_.Name.ToLower(), "strongroom_blue")
                            tLocB_SR.Add(New GenerateConfigs.System.SpawnablePositionalData(entry_.Position.X.ToString() + " " + entry_.Position.Y.ToString() + " " + entry_.Position.Z.ToString(), entry_.Rotation.X.ToString() + " " + entry_.Rotation.Y.ToString() + " " + entry_.Rotation.Z.ToString()))
                        Case StringContainsText(entry_.Name.ToLower(), "strongroom_green")
                            tLocG_SR.Add(New GenerateConfigs.System.SpawnablePositionalData(entry_.Position.X.ToString() + " " + entry_.Position.Y.ToString() + " " + entry_.Position.Z.ToString(), entry_.Rotation.X.ToString() + " " + entry_.Rotation.Y.ToString() + " " + entry_.Rotation.Z.ToString()))
                        Case StringContainsText(entry_.Name.ToLower(), "strongroom_yellow")
                            tLocY_SR.Add(New GenerateConfigs.System.SpawnablePositionalData(entry_.Position.X.ToString() + " " + entry_.Position.Y.ToString() + " " + entry_.Position.Z.ToString(), entry_.Rotation.X.ToString() + " " + entry_.Rotation.Y.ToString() + " " + entry_.Rotation.Z.ToString()))

                    End Select
                Next

                GenerateConfigs.System.Locations.Crate.Red = tLocRedCrate
                GenerateConfigs.System.Locations.Crate.Purple = tLocPCrate
                GenerateConfigs.System.Locations.Crate.Blue = tLocBCrate
                GenerateConfigs.System.Locations.Crate.Green = tLocGCrate
                GenerateConfigs.System.Locations.Crate.Yellow = tLocYCrate

                GenerateConfigs.System.Locations.Strongroom.Red = tLocR_SR
                GenerateConfigs.System.Locations.Strongroom.Purple = tLocP_SR
                GenerateConfigs.System.Locations.Strongroom.Blue = tLocB_SR
                GenerateConfigs.System.Locations.Strongroom.Green = tLocG_SR
                GenerateConfigs.System.Locations.Strongroom.Yellow = tLocY_SR

            End Function
            Public Shared Async Function RemoveNodesByNames(entries As List(Of LinkerAddressMapEntry), allowedNames As List(Of String)) As Task
                entries.RemoveAll(Function(entry) Not allowedNames.Exists(Function(allowedName) entry.Name.IndexOf(allowedName, StringComparison.OrdinalIgnoreCase) >= 0))

            End Function

            Public Shared Async Function ParseLinkerAddressMapFile(filePath As String) As Task(Of List(Of LinkerAddressMapEntry))
                Dim entries As New List(Of LinkerAddressMapEntry)()

                Using reader As New StreamReader(filePath)
                    While Not reader.EndOfStream
                        Dim line As String = reader.ReadLine()
                        Dim parts As String() = line.Split("|"c)

                        If parts.Length = 3 Then
                            Dim entry As New LinkerAddressMapEntry()
                            entry.Name = parts(0).Trim()
                            entry.Position = ParseVector3(parts(1).Trim())
                            entry.Rotation = ParseVector3(parts(2).Trim())

                            entries.Add(entry)
                        End If
                    End While
                End Using
                Return entries
            End Function

            Private Shared Function ParseVector3(vectorString As String) As Vector3
                Dim values As String() = vectorString.Split(" "c)
                Dim vector As New Vector3()

                If values.Length = 3 Then
                    vector.X = Single.Parse(values(0))
                    vector.Y = Single.Parse(values(1))
                    vector.Z = Single.Parse(values(2))
                End If

                Return vector
            End Function
        End Class

        Public Class LinkerAddressMapEntry
            Public Property Name As String
            Public Property Position As Vector3
            Public Property Rotation As Vector3
        End Class

        Public Structure Vector3
            Public X As Single
            Public Y As Single
            Public Z As Single
        End Structure
        'Public Class MAP
        '    Public Shared Async Function ImportMAP(mapPath As String) As Task(Of List(Of String))
        '        Dim classNamesAndVariants As New HashSet(Of String)()

        '        ' Read the JSON file
        '        Using reader As StreamReader = File.OpenText(mapPath)



        '            'Dim jsonText As String = Await reader.ReadToEndAsync()

        '            '' Deserialize the JSON data
        '            'Dim jsonData As JObject = JsonConvert.DeserializeObject(Of JObject)(jsonText)

        '            '' Get the "Items" array
        '            'Dim itemsArray As JArray = jsonData("Items")

        '            '' Iterate through each item
        '            'For Each item As JObject In itemsArray
        '            '    ' Get the "ClassName" value
        '            '    Dim className As String = item("ClassName").ToString()

        '            '    ' Add the "ClassName" value to the hash set if it's not already present
        '            '    classNamesAndVariants.Add(className)

        '            '    ' Get the "Variants" array
        '            '    Dim variantsArray As JArray = item("Variants")

        '            '    ' Iterate through each variant
        '            '    For Each tvariant As JToken In variantsArray
        '            '        ' Get the variant string
        '            '        Dim variantString As String = tvariant.ToString()

        '            '        ' Add the variant string to the hash set if it's not already present
        '            '        classNamesAndVariants.Add(variantString)
        '            '    Next
        '            'Next
        '        End Using

        '        ' Return the list of unique "classname" and "variants" strings
        '        Return classNamesAndVariants.ToList()
        '    End Function
        'End Class
        Public Class JSON

            Public Shared Function GetNodeValue(json As JObject, path As String) As Object
                'Dim json As JObject = JObject.Parse(jsonString)
                'Dim nodeValue As Object = GetNodeValue(json, "m_DNAConfig_Version[0].dna_WarningMessage")

                Dim tokens As JToken = json.SelectToken(path)
                If tokens IsNot Nothing Then
                    Return tokens
                Else
                    Return Nothing
                End If
            End Function

            Public Shared Function GetNodeAttributes(json As JObject, path As String) As Dictionary(Of String, Object)
                'Dim json As JObject = JObject.Parse(jsonString)
                'Dim nodeAttributes As Dictionary(Of String, Object) = GetNodeAttributes(json, "m_DNAConfig_Main_System[0]")

                Dim attributes As New Dictionary(Of String, Object)()

                Dim tokens As JToken = json.SelectToken(path)
                If tokens IsNot Nothing AndAlso tokens.Type = JTokenType.Object Then
                    Dim obj As JObject = CType(tokens, JObject)
                    For Each prop As JProperty In obj.Properties()
                        attributes.Add(prop.Name, prop.Value)
                    Next
                End If

                Return attributes
            End Function

            Public Shared Function GetChildNodes(json As JObject, path As String) As List(Of JObject)
                'Dim json As JObject = JObject.Parse(jsonString)
                'Dim childNodes As List(Of JObject) = GetChildNodes(json, "arbitraryname1")

                Dim childNodes As New List(Of JObject)()

                Dim tokens As JToken = json.SelectToken(path)
                If tokens IsNot Nothing AndAlso tokens.Type = JTokenType.Array Then
                    Dim array As JArray = CType(tokens, JArray)
                    For Each item As JObject In array.Children(Of JObject)()
                        childNodes.Add(item)
                    Next
                End If

                Return childNodes
            End Function
            Public Shared Sub TraverseJSONNodes(json As JObject, Optional ByVal indent As Integer = 0)
                'Dim jsonString As String = "{...}" ' Your JSON string here
                'Dim json As JObject = JObject.Parse(jsonString)
                'TraverseJSONNodes(json)

                For Each prop As JProperty In json.Properties()
                    Dim nodePath As String = prop.Path
                    Dim nodeName As String = prop.Name
                    Dim nodeValue As Object = prop.Value

                    ' Indent the output for readability
                    Console.WriteLine($"{New String(" "c, indent)}Node Path: {nodePath}")
                    Console.WriteLine($"{New String(" "c, indent)}Node Name: {nodeName}")
                    Console.WriteLine($"{New String(" "c, indent)}Node Value: {nodeValue}")

                    ' Check if the node has child nodes
                    If prop.Value.Type = JTokenType.Array OrElse prop.Value.Type = JTokenType.Object Then
                        Dim childNodes As List(Of JObject) = GetChildNodes(json, nodePath)

                        ' Recursively traverse child nodes
                        For Each childNode As JObject In childNodes
                            TraverseJSONNodes(childNode, indent + 2)
                        Next
                    End If
                Next
            End Sub

            Public Shared Function ParseSystemConfigJSON(jsonString As String) As Dictionary(Of String, List(Of Dictionary(Of String, Object)))
                Dim result As New Dictionary(Of String, List(Of Dictionary(Of String, Object)))()

                ' Parse the JSON string
                Dim json As JObject = JObject.Parse(jsonString)

                ' Iterate over the properties in the JSON object
                For Each prop As KeyValuePair(Of String, JToken) In json
                    Dim dataList As New List(Of Dictionary(Of String, Object))()

                    ' Iterate over the array of objects for each property
                    For Each item As JObject In prop.Value
                        Dim dataItem As New Dictionary(Of String, Object)()

                        ' Iterate over the properties in each object
                        For Each subProp As KeyValuePair(Of String, JToken) In item
                            dataItem.Add(subProp.Key, subProp.Value.ToString())
                        Next

                        dataList.Add(dataItem)
                    Next

                    result.Add(prop.Key, dataList)
                Next

                Return result
            End Function
        End Class
        Public Shared Function RetrieveKeyValuePairs(propertyData As List(Of Dictionary(Of String, Object))) As List(Of KeyValuePair(Of String, Object))
            Dim keyValuePairs As New List(Of KeyValuePair(Of String, Object))()

            For Each item As Dictionary(Of String, Object) In propertyData
                For Each kvp As KeyValuePair(Of String, Object) In item
                    keyValuePairs.Add(kvp)
                Next
            Next

            Return keyValuePairs
        End Function
        Public Shared Async Function ImportSystemConfigJSON(jsonFilePath As String) As Task
            Dim classNamesAndVariants As New HashSet(Of String)()
            ' Read the JSON file
            Using reader As StreamReader = File.OpenText(jsonFilePath)
                Dim jsonText As String = Await reader.ReadToEndAsync()
                Dim xxx = JSON.ParseSystemConfigJSON(jsonText)
                'Header Cycle
                For Each tHeader In xxx
                    Dim OptionCount = tHeader.Value.Count

                    If tHeader.Key = "m_DNAConfig_Version" Then
                        For i = 0 To OptionCount - 1
                            'Options Cycle
                            For y = 0 To tHeader.Value.Item(i).Keys.Count - 1
                                If tHeader.Value.Item(i).Keys.ToList()(y) = "dna_WarningMessage" Then GenerateConfigs.System.DNAConfigVersion.dna_WarningMessage = tHeader.Value.Item(i).Values.ToList()(y)
                                If tHeader.Value.Item(i).Keys.ToList()(y) = "dna_ConfigVersion" Then GenerateConfigs.System.DNAConfigVersion.dna_ConfigVersion = tHeader.Value.Item(i).Values.ToList()(y)
                            Next y
                        Next i
                    End If


                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    If tHeader.Key = "m_DNAConfig_Main_System" Then
                        For i = 0 To OptionCount - 1
                            Dim OptText As String = tHeader.Value.Item(i).Values.ToList()(0)


                            Select Case True
                                Case StringContainsText(OptText, "crates")
                                    GenerateConfigs.System.DNAConfigMainSystem_Crates.Add(New GenerateConfigs.System.MainSystemSettings() With {.HelpText = tHeader.Value.Item(i).Values.ToList()(0), .dna_Option = tHeader.Value.Item(i).Values.ToList()(0), .dna_Option_Bak = tHeader.Value.Item(i).Values.ToList()(0), .dna_Setting = tHeader.Value.Item(i).Values.ToList()(1)})
                                Case StringContainsText(OptText, "strongrooms")
                                    GenerateConfigs.System.DNAConfigMainSystem_Strongrooms.Add(New GenerateConfigs.System.MainSystemSettings() With {.HelpText = tHeader.Value.Item(i).Values.ToList()(0), .dna_Option = tHeader.Value.Item(i).Values.ToList()(0), .dna_Option_Bak = tHeader.Value.Item(i).Values.ToList()(0), .dna_Setting = tHeader.Value.Item(i).Values.ToList()(1)})
                                Case StringContainsText(OptText, "card")
                                    GenerateConfigs.System.DNAConfigMainSystem_Card.Add(New GenerateConfigs.System.MainSystemSettings() With {.HelpText = tHeader.Value.Item(i).Values.ToList()(0), .dna_Option = tHeader.Value.Item(i).Values.ToList()(0), .dna_Option_Bak = tHeader.Value.Item(i).Values.ToList()(0), .dna_Setting = tHeader.Value.Item(i).Values.ToList()(1)})
                                Case StringContainsText(OptText, "separate")
                                    GenerateConfigs.System.DNAConfigMainSystem_Separate.Add(New GenerateConfigs.System.MainSystemSettings() With {.HelpText = tHeader.Value.Item(i).Values.ToList()(0), .dna_Option = tHeader.Value.Item(i).Values.ToList()(0), .dna_Option_Bak = tHeader.Value.Item(i).Values.ToList()(0), .dna_Setting = tHeader.Value.Item(i).Values.ToList()(1)})
                                Case StringContainsText(OptText, "lockout")
                                    GenerateConfigs.System.DNAConfigMainSystem_lockout.Add(New GenerateConfigs.System.MainSystemSettings() With {.HelpText = tHeader.Value.Item(i).Values.ToList()(0), .dna_Option = tHeader.Value.Item(i).Values.ToList()(0), .dna_Option_Bak = tHeader.Value.Item(i).Values.ToList()(0), .dna_Setting = tHeader.Value.Item(i).Values.ToList()(1)})
                                Case Else
                                    GenerateConfigs.System.DNAConfigMainSystem_other.Add(New GenerateConfigs.System.MainSystemSettings() With {.HelpText = tHeader.Value.Item(i).Values.ToList()(0), .dna_Option = tHeader.Value.Item(i).Values.ToList()(0), .dna_Option_Bak = tHeader.Value.Item(i).Values.ToList()(0), .dna_Setting = tHeader.Value.Item(i).Values.ToList()(1)})
                            End Select

                        Next i
                    End If

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    If tHeader.Key = "m_DNAYellow_Crate_Locations" Then
                        Dim t As New ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData)
                        For i = 0 To OptionCount - 1
                            t.Add(New GenerateConfigs.System.SpawnablePositionalData(tHeader.Value.Item(i).Values.ToList()(0), tHeader.Value.Item(i).Values.ToList()(1)))
                        Next i
                        GenerateConfigs.System.Locations.Crate.Yellow = t
                    End If
                    If tHeader.Key = "m_DNAGreen_Crate_Locations" Then
                        Dim t As New ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData)
                        For i = 0 To OptionCount - 1
                            t.Add(New GenerateConfigs.System.SpawnablePositionalData(tHeader.Value.Item(i).Values.ToList()(0), tHeader.Value.Item(i).Values.ToList()(1)))
                        Next i
                        GenerateConfigs.System.Locations.Crate.Green = t
                    End If
                    If tHeader.Key = "m_DNABlue_Crate_Locations" Then
                        Dim t As New ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData)
                        For i = 0 To OptionCount - 1
                            t.Add(New GenerateConfigs.System.SpawnablePositionalData(tHeader.Value.Item(i).Values.ToList()(0), tHeader.Value.Item(i).Values.ToList()(1)))
                        Next i
                        GenerateConfigs.System.Locations.Crate.Blue = t
                    End If
                    If tHeader.Key = "m_DNAPurple_Crate_Locations" Then
                        Dim t As New ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData)
                        For i = 0 To OptionCount - 1
                            t.Add(New GenerateConfigs.System.SpawnablePositionalData(tHeader.Value.Item(i).Values.ToList()(0), tHeader.Value.Item(i).Values.ToList()(1)))
                        Next i
                        GenerateConfigs.System.Locations.Crate.Purple = t
                    End If
                    If tHeader.Key = "m_DNARed_Crate_Locations" Then
                        Dim t As New ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData)
                        For i = 0 To OptionCount - 1
                            t.Add(New GenerateConfigs.System.SpawnablePositionalData(tHeader.Value.Item(i).Values.ToList()(0), tHeader.Value.Item(i).Values.ToList()(1)))
                        Next i
                        GenerateConfigs.System.Locations.Crate.Red = t
                    End If

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    If tHeader.Key = "m_DNAYellow_Strongroom_Locations" Then
                        Dim t As New ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData)
                        For i = 0 To OptionCount - 1
                            t.Add(New GenerateConfigs.System.SpawnablePositionalData(tHeader.Value.Item(i).Values.ToList()(0), tHeader.Value.Item(i).Values.ToList()(1)))
                        Next i
                        GenerateConfigs.System.Locations.Strongroom.Yellow = t
                    End If
                    If tHeader.Key = "m_DNAGreen_Strongroom_Locations" Then
                        Dim t As New ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData)
                        For i = 0 To OptionCount - 1
                            t.Add(New GenerateConfigs.System.SpawnablePositionalData(tHeader.Value.Item(i).Values.ToList()(0), tHeader.Value.Item(i).Values.ToList()(1)))
                        Next i
                        GenerateConfigs.System.Locations.Strongroom.Green = t
                    End If
                    If tHeader.Key = "m_DNABlue_Strongroom_Locations" Then
                        Dim t As New ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData)
                        For i = 0 To OptionCount - 1
                            t.Add(New GenerateConfigs.System.SpawnablePositionalData(tHeader.Value.Item(i).Values.ToList()(0), tHeader.Value.Item(i).Values.ToList()(1)))
                        Next i
                        GenerateConfigs.System.Locations.Strongroom.Blue = t
                    End If
                    If tHeader.Key = "m_DNAPurple_Strongroom_Locations" Then
                        Dim t As New ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData)
                        For i = 0 To OptionCount - 1
                            t.Add(New GenerateConfigs.System.SpawnablePositionalData(tHeader.Value.Item(i).Values.ToList()(0), tHeader.Value.Item(i).Values.ToList()(1)))
                        Next i
                        GenerateConfigs.System.Locations.Strongroom.Purple = t
                    End If
                    If tHeader.Key = "m_DNARed_Strongroom_Locations" Then
                        Dim t As New ObservableCollection(Of GenerateConfigs.System.SpawnablePositionalData)
                        For i = 0 To OptionCount - 1
                            t.Add(New GenerateConfigs.System.SpawnablePositionalData(tHeader.Value.Item(i).Values.ToList()(0), tHeader.Value.Item(i).Values.ToList()(1)))
                        Next i
                        GenerateConfigs.System.Locations.Strongroom.Red = t
                    End If

                Next
            End Using

            Return
        End Function

        Public Shared Async Function RemoveDuplicates(ByVal list As List(Of String)) As Task(Of List(Of String))
            Dim uniqueItems As New HashSet(Of String)(list)
            Return uniqueItems.ToList()
        End Function
    End Class
End Namespace