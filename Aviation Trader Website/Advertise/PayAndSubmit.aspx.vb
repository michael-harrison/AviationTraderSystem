Option Strict On
Option Explicit On
Imports ATLib
Imports System.ComponentModel

'***************************************************************************************
'*
'* Ad Submit
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Ad number
'*          
'*
'***************************************************************************************

Partial Class PayAndSubmit
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private tabID As String
    Protected TotalPrice As Integer

    Private Ad As Ad
    Private paymentType As Ad.PaymentTypes

    Private Enum MonthOptions
        <Description("01")> Jan = 1
        <Description("02")> Feb = 2
        <Description("03")> Mar = 3
        <Description("04")> Apr = 4
        <Description("05")> May = 5
        <Description("06")> Jun = 6
        <Description("07")> Jul = 7
        <Description("08")> Aug = 8
        <Description("09")> Sep = 9
        <Description("10")> Oct = 10
        <Description("11")> Nov = 11
        <Description("12")> Dec = 12
    End Enum


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.skin

        headerbar.Slot = Slot
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
        ' if the user has not logged in, put up the login screen
        ' otherwise retrieve the ad
        '
        If Slot.LoginLevel = Usr.LoginLevels.Guest Then
            Loader.NextASPX = ATLib.Loader.ASPX.Login
            Response.Redirect(Loader.Target)
        Else
            Dim ads As New Ads
            Ad = ads.Retrieve(Loader.ObjectID)
        End If

        If Not IsPostBack Then
            displayAd()
            paymentMethodDD.Attributes.Add("onchange", "paymentChange(this);")
            ErrorMsg.Text = "&nbsp;"
        Else
            paymentType = CType(paymentMethodDD.SelectedValue, Ad.PaymentTypes)
        End If

    End Sub



    Private Sub displayAd()

        Dim EA As EnumAssistant

        TotalPrice = 1000

        Dim min As Integer = 0
        Dim max As Integer = Ad.PaymentTypes.VISA
        '
        ' include pay by terms if applicable
        '
        ''    If CustomerType = Dealer.CustomerTypes.WSD Then
        max = Ad.PaymentTypes.Terms
        ''    End If


        EA = New EnumAssistant(New Ad.PaymentTypes, min, max)
        paymentMethodDD.DataSource = EA
        paymentMethodDD.DataBind()

        EA = New EnumAssistant(New MonthOptions)
        CCExpiryMonthDD.DataSource = EA
        CCExpiryMonthDD.SelectedValue = Now.Month.ToString
        CCExpiryMonthDD.DataBind()
        '
        ' year - run from current year for 12 years
        '
        EA = New EnumAssistant
        Dim Year As Integer = Convert.ToInt32(Now.Year)
        For i As Integer = 0 To 11
            Dim EI As New EnumItem
            EI.Name = Year.ToString
            EI.Value = Year
            EA.Add(EI)
            Year += 1
        Next

        CCExpiryYearDD.DataSource = EA
        CCExpiryYearDD.DataBind()



    End Sub

    Private Sub validatePayment()

       
        Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
        Ad.Update()



        Loader.NextASPX = ATLib.Loader.ASPX.AdSubmitConfirmation
        Response.Redirect(Loader.Target)
    End Sub


    Protected Sub BtnReview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReview.Click
        Loader.NextASPX = ATLib.Loader.ASPX.AdReview
        Response.Redirect(Loader.Target)
    End Sub

    Protected Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubmit.Click
        validatepayment()

     
    End Sub
End Class
