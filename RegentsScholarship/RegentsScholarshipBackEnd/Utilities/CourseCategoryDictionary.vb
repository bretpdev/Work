Public Class CourseCategoryDictionary
    Inherits Dictionary(Of String, CourseCategory)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal existingCategories As CourseCategoryDictionary)
        MyBase.New(existingCategories)
    End Sub

    Public Overloads Sub Add(ByVal newCategory As CourseCategory)
        MyBase.Add(newCategory.Category(), newCategory)
    End Sub
End Class
