Imports System.Windows.Forms

Public Class StatusBar
    Private Const MF_BYPOSITION = &H400
    Private Const MF_REMOVE = &H1000
    Private Const MF_DISABLED = &H2

    Private Declare Function RemoveMenu Lib "user32" (ByVal hMenu As IntPtr, ByVal nPosition As Integer, ByVal wFlags As Long) As IntPtr
    Private Declare Function GetSystemMenu Lib "user32" (ByVal hWnd As IntPtr, ByVal bRevert As Boolean) As IntPtr
    Private Declare Function GetMenuItemCount Lib "user32" (ByVal hMenu As IntPtr) As Integer
    Private Declare Function DrawMenuBar Lib "user32" (ByVal hwnd As IntPtr) As Boolean

    Public Sub New(ByVal total As Double, ByVal title As String)
        InitializeComponent()
        DisableCloseButton(Me.Handle)

        Review.Text = title
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = total
        ProgressBar1.Step = 1

    End Sub


    Public Sub updateStatBar(ByVal student As String)
        ProgressBar1.PerformStep()
        Label2.Text = student

        Label1.Refresh()
        Label2.Refresh()
        ProgressBar1.Refresh()
        Review.Refresh()
        Me.Refresh()
        Application.DoEvents()
    End Sub

    Public Sub DisableCloseButton(ByVal hwnd As IntPtr)
        Dim hMenu As IntPtr
        Dim menuItemCount As Integer

        hMenu = GetSystemMenu(hwnd, False)
        menuItemCount = GetMenuItemCount(hMenu)
        Call RemoveMenu(hMenu, menuItemCount - 1, MF_DISABLED Or MF_BYPOSITION)
        Call RemoveMenu(hMenu, menuItemCount - 2, MF_DISABLED Or MF_BYPOSITION)
        Call DrawMenuBar(hwnd)
    End Sub
End Class