﻿Public Class DSCollegeAttendance
    Private _stateStudentId As String
    Public Property StateStudentId() As String
        Get
            Return _stateStudentId
        End Get
        Set(ByVal value As String)
            _stateStudentId = value
        End Set
    End Property

    Private _firstName As String
    Public Property FirstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property

    Private _lastName As String
    Public Property LastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property

    Private _collegeName As String
    Public Property CollegeName() As String
        Get
            Return _collegeName
        End Get
        Set(ByVal value As String)
            _collegeName = value
        End Set
    End Property

    Private _applicationYear As String
    Public Property ApplicationYear() As String
        Get
            Return _applicationYear
        End Get
        Set(ByVal value As String)
            _applicationYear = value
        End Set
    End Property
End Class
