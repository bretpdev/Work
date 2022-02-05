Attribute VB_Name = "Csharp"
Sub ORCPAYPOST()
    Sp.Common.ScriptStarter "ORCPAYPOST.dll", "ORCPAYPOST.dll", "ORCPAYPOST.ORCCPaymentPosting", True
End Sub

Sub CONPMTPST()
    StartScript "CONPMTPST"
End Sub

Sub NSFREVENTR()
    StartScript "NSFREVENTR"
End Sub

Sub NSFREVPOST()
    StartScript "NSFREVPOST"
End Sub

Sub PRECONADJ()
    StartScript "PRECONADJ", "PRECONADJ.PRECONADJ_UHEAA"
End Sub

Sub TILPCROST()
    StartScript "TILPCROST"
End Sub



