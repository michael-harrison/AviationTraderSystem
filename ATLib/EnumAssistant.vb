Option Strict On
Option Explicit On
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.ComponentModel



'***************************************************************************************
'*
'* Enumerator assistant
'*
'*
'*
'***************************************************************************************


''' <summary>
''' This is a helper class that provides a binding helper for enums and other programmed lists.
''' It gives the node content display and other enums something to bind with
''' It emulates the behaviour of the other ATLib classes for databinding. See the example in the overloaded constructor
''' New(x) for a demonstration of how the class is used.
''' </summary>
Public Class EnumAssistant : Inherits CollectionBase

    ''' <summary>
    ''' Instantiates the object.
    ''' </summary>
    ''' <remarks>It would be easier to use the overloaded instantiator, which specifies the enum type.</remarks>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Instantiates the object and primes it with an instantiation of a specific enum type.
    ''' </summary>
    ''' <param name="x">[Enum] type</param>
    ''' <example>
    ''' <code>
    ''' dim control as control
    ''' dim EA as New EnumAssistant(New ShippingAssistant.ShipMethods)
    ''' control.DataSource = EA
    ''' control.databind
    '''</code></example>
    Public Sub New(ByVal x As [Enum])
        Bind2Enum(x, ATSystem.SysConstants.nullValue, &H7FFFFFFF)
    End Sub

    ''' <summary>
    ''' Instantiates the object and primes it with an instantiation of a specific enum type.
    ''' This version allows a subsetted range of the total enum to be used.</summary>
    ''' <param name="x">[Enum] type</param>
    ''' <param name="min">Lower end of enum range that is used in databinding</param>
    ''' <param name="max">Upper end of enum range that is used in databinding</param>
    Public Sub New(ByVal x As [Enum], ByVal min As Integer, ByVal max As Integer)
        Bind2Enum(x, min, max)
    End Sub

    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG EnumAssistant(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...EnumAssistant.count-1</param>
    ''' <value>EnumItem object from EnumAssistant collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As EnumItem
        Get
            Return CType(List(index), EnumItem)
        End Get
        Set(ByVal value As EnumItem)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds an EnumItem object to the EnumAssistant collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">EnumItem object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As EnumItem) As Integer
        Return (List.Add(value))
    End Function
    Public Function IndexOf(ByVal value As EnumItem) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As EnumItem)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As EnumItem)
        List.Remove(value)
    End Sub
    Public Function Contains(ByVal value As EnumItem) As Boolean
        Return (List.Contains(value))
    End Function

    '''<summary>
    ''' Converts the enum into a bindable object.
    '''</summary> 
    ''' <param name="x">[Enum] type</param>
    Public Sub Bind2Enum(ByVal x As [Enum])
        Bind2Enum(x, ATSystem.SysConstants.nullValue, &H7FFFFFFF)
    End Sub


    '''<summary>
    ''' Converts the enum into a bindable object.
    '''</summary> 
    ''' <param name="x">[Enum] type</param>
    ''' <param name="min">Lower end of enum range that is used in databinding</param>
    ''' <param name="max">Upper end of enum range that is used in databinding</param>
    Public Sub Bind2Enum(ByVal x As [Enum], ByVal min As Integer, ByVal max As Integer)
        '
        ' converts an enum into a bindable object
        '
        ' If each enum field includes an optional <description attribute> then this is returned instead of
        ' the enum name. EG
        '

        '        Public Enum xxx
        '          <Description("first")>  a = 10
        '          <Description("second")> b = 20
        '          <Description("third")>  c = 30
        '                                 c1 = 35
        '          <Description("last")>   d = 40
        '        End Enum

        List.Clear()
        Dim enumitem As EnumItem
        Dim y As Type = x.GetType

        Dim names As String() = [Enum].GetNames(y)
        Dim values As Array = [Enum].GetValues(y)

        For i As Integer = 0 To names.Length - 1
            Dim tstvalue As Integer = Convert.ToInt32(values.GetValue(i))
            '
            ' only add to the list if value is bracketed by start and end
            ' this is used to produce subsets enum lists
            '
            If tstvalue >= min And tstvalue <= max Then
                enumitem = New EnumItem
                enumitem.Name = names(i)
                enumitem.Description = names(i)
                enumitem.Value = tstvalue
                '
                ' use reflection to see if any of the fields have a description
                ' if so use that instead of the standard name
                '
                Dim member As Reflection.MemberInfo() = y.GetMember(names(i))
                If member.Length > 0 Then
                    '
                    Dim descrAttributes As Object() = member(0).GetCustomAttributes(GetType(DescriptionAttribute), True)
                    If descrAttributes.Length > 0 Then
                        enumitem.Description = CType(descrAttributes(0), DescriptionAttribute).Description
                    End If
                End If

                List.Add(enumitem)

            End If

        Next
    End Sub

End Class

'***************************************************************************************
'*
'* Enumerator
'*
'* AUDIT TRAIL
'* 
'* This is little class that provides a binding helper for enums and other programmed lists.
' It gives the node content display and other enums something to bind with
'* It emulates the behaviour of the other ATLib classes for databinding.
'*
'* V1.000   16-JUL-2007  BA  Original
'*
'*
'***************************************************************************************


''' <summary>
''' The EnumItem are the bindable objects that are returned in the EnumAssistant class.
''' </summary>
Public Class EnumItem

    Private _navTarget As String
    Private _name As String
    Private _value As Integer
    Private _description As String

    ''' <summary>Object Name. This is the name of the enum. If the optional description attribute is 
    ''' used then this name is returned instead of the enum name
    '''</summary>
    ''' <returns>Enum value as integer</returns>
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value

        End Set
    End Property

    ''' <summary>Object Value. This is the numeric value of the enum
    '''</summary>
    ''' <returns>Enum value as integer</returns>
    Public Property Value() As Integer
        Get
            Return _value
        End Get
        Set(ByVal value As Integer)
            _value = value
        End Set
    End Property

    ''' <summary>Object Value. This is an alternative string value of the enum
    '''</summary>
    ''' <returns>Enum value as string</returns>
    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

End Class

