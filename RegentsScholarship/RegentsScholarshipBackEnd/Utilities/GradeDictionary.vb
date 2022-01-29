Public Class GradeDictionary
    Inherits Dictionary(Of Integer, Grade)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal existingGrades As GradeDictionary)
        MyBase.New(existingGrades)
    End Sub

    Public Overloads Sub Add(ByVal newGrade As Grade)
        MyBase.Add(newGrade.Term(), newGrade)
    End Sub
End Class
