Imports System.Collections.Generic

Public Class ReviewDictionary
    Inherits Dictionary(Of String, Review)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal existingReviews As ReviewDictionary)
        MyBase.New(existingReviews)
    End Sub

    Public Overloads Sub Add(ByVal newReview As Review)
        MyBase.Add(newReview.ReviewType(), newReview)
    End Sub
End Class
