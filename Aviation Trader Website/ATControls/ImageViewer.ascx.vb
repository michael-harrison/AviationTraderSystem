Option Strict On
Option Explicit On
Imports ATLib


'***************************************************************************************
'*
'* Headerbar
'*
'* AUDIT TRAIL
'* 
'* V1.000   02-AUG-2009  BA  Original
'*
'*
'***************************************************************************************

Partial Class ATControls_ImageViewer
    Inherits System.Web.UI.UserControl

    Private _ad As Ad

    Public Property Ad() As Ad
        Get
            Return _ad
        End Get
        Set(ByVal value As Ad)
            _ad = value
        End Set
    End Property





    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Carousel.DataSource = Ad.Images()
        Carousel.DataBind()
    End Sub




End Class
