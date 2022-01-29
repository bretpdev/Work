Public Class ClassDataControl
    'The Regents form needs to be notified when a radio button, grade level, or class weight changes,
    'so we watch for those events here and raise a public event.
    Public Event GradeLevelChanged(ByVal sender As ClassDataControl)
    Public Event RadioButtonChanged()
    Public Event ClassWeightChanged(ByVal weight As String)

    Private Sub radClassAcceptableYes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radClassAcceptableYes.CheckedChanged
        RaiseEvent RadioButtonChanged()
    End Sub

    Private Sub radClassAcceptableNo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radClassAcceptableNo.CheckedChanged
        RaiseEvent RadioButtonChanged()
    End Sub

    Private Sub radClassAcceptableInProgress_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radClassAcceptableInProgress.CheckedChanged
        RaiseEvent RadioButtonChanged()
    End Sub

    Private Sub txtGradeLevel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGradeLevel.TextChanged
        RaiseEvent GradeLevelChanged(Me)
    End Sub

    Private Sub cmbWeightDesignation_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWeightDesignation.TextChanged
        RaiseEvent ClassWeightChanged(cmbWeightDesignation.Text)
    End Sub
End Class
