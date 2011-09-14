Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls


<DefaultProperty("Text"), ToolboxData("<{0}:GroupRadioButton runat=server></{0}:GroupRadioButton>")> _
Public Class GroupRadioButton
    Inherits WebControls.RadioButton
    Implements IPostBackDataHandler

    Private _onclick As String

    Public ReadOnly Property Value() As String
        Get
            Dim rtnval As String = Attributes("value")
            If rtnval = "" Then
                rtnval = UniqueID
            Else
                rtnval = UniqueID & "_" & rtnval
            End If
            Return rtnval
        End Get
    End Property

    Public Property OnClick() As String
        Get
            Return _onclick
        End Get
        Set(ByVal value As String)
            _onclick = value

        End Set
    End Property

    Protected Overrides Sub Render(ByVal output As HtmlTextWriter)


        output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID)
        output.AddAttribute(HtmlTextWriterAttribute.Type, "radio")
        output.AddAttribute(HtmlTextWriterAttribute.Name, GroupName)
        output.AddAttribute(HtmlTextWriterAttribute.Value, Value)
        If Not _onclick Is Nothing Then
            output.AddAttribute(HtmlTextWriterAttribute.Onclick, _onclick)
        End If
        If Checked Then output.AddAttribute(HtmlTextWriterAttribute.Checked, "checked")
        If Not Enabled Then output.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled")

        ''     Dim onClick As String = Attributes("onclick")
        If AccessKey.Length > 0 Then
            output.AddAttribute(HtmlTextWriterAttribute.Accesskey, AccessKey)
        End If
        If TabIndex <> 0 Then
            output.AddAttribute(HtmlTextWriterAttribute.Tabindex, TabIndex.ToString(System.Globalization.NumberFormatInfo.InvariantInfo))
        End If
        output.RenderBeginTag(HtmlTextWriterTag.Input)
        output.RenderEndTag()
    End Sub

    Public Shadows Sub RaisePostDataChangedEvent() Implements IPostBackDataHandler.RaisePostDataChangedEvent

        OnCheckedChanged(EventArgs.Empty)
    End Sub

    Public Shadows Function LoadPostData(ByVal postDataKey As String, ByVal postCollection As system.Collections.Specialized.NameValueCollection) As Boolean _
    Implements IPostBackDataHandler.LoadPostData


        Dim result As Boolean = False
        Dim rtnval As String = postCollection(GroupName)
        If rtnval = Value Then
            If Not Checked Then
                Checked = True
                result = True
            End If
        Else
            If Checked Then
                Checked = False
            End If
        End If
        Return result
    End Function


End Class
