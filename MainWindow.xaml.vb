Imports System
Imports System.Collections.Generic
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
End Class

