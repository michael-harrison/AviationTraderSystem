Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Ad Product Selector
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Ad id
'*          
'*
'***************************************************************************************

Partial Class ProductSelector
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
  
    Protected Ad As Ad


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
            displayProducts()
        End If
   

    End Sub



    Private Sub displayProducts()

        '
        ' bind publications to outside repeater - only include classified or display for now
        '
        Dim publications As New List(Of Publication)

        For Each pub As Publication In sys.Publications
            If (pub.Type = Publication.Types.ATClassified) Or (pub.Type = Publication.Types.ATDisplay) Then
                publications.Add(pub)
            End If
        Next

        PublicationList.DataSource = publications
        PublicationList.DataBind()

        '
        ' bind products and open editions to two inside repeaters
        '

        Dim i As Integer = 0
        For Each r1 As RepeaterItem In PublicationList.Items
            Dim productlist As Repeater = CType(r1.FindControl("ProductList"), Repeater)
            Dim products As Products = publications(i).Products
  
            Dim editionlist As Repeater = CType(r1.FindControl("EditionList"), Repeater)
            Dim editions As Editions = publications(i).Editions(Edition.ProdnState.Open)
            '
            ' put the first edition always checked and show in different color
            '
            If editions.Count > 0 Then
                editions(0).CSSClass = "error"
                ''        editions(0).Checked = True - disable this
            End If

            '
            ' spin thru intances for the ad and set the check matrix
            '
            For Each AdInstance As AdInstance In Ad.Instances
                For Each Product As Product In products
                    If Product.ID = AdInstance.ProductID Then Product.Checked = True
                Next

                For Each edition As Edition In editions
                    '
                    ' put the first edition on and show in different color
                    '
                    If edition.ID = AdInstance.EditionID Then edition.Checked = True
                Next
            Next

            productlist.DataSource = products
            productlist.DataBind()

            editionlist.DataSource = editions
            editionlist.DataBind()
            i += 1
        Next

    End Sub

    Private Sub updateInstances()
        '
        ' iterate thru repeaters and update instances
        '
        Dim errorFlag As Boolean = False
        Dim selectedProductCount As Integer = 0
        Errormsg1.Text = ""
        ErrorMsg2.Text = ""

        Dim i As Integer = 0
        For Each r1 As RepeaterItem In PublicationList.Items
            Dim errorMsg As Label = CType(r1.FindControl("errorMsg"), Label)
            Dim productList As Repeater = CType(r1.FindControl("productList"), Repeater)
            Dim editionList As Repeater = CType(r1.FindControl("editionList"), Repeater)
            errorMsg.Text = ""

            '
            ' get the selected products into an array
            '
            Dim productIDs As New List(Of Integer)
            For Each r As RepeaterItem In productList.Items
                Dim productField As HiddenField = CType(r.FindControl("ProductID"), HiddenField)
                Dim productID As Integer = CommonRoutines.Hex2Int(productField.Value)
                Dim productCheck As CheckBox = CType(r.FindControl("ProductCheck"), CheckBox)
                If productCheck.Checked Then productIDs.Add(productID)
            Next
            selectedProductCount += productIDs.Count     'accumulate total selected products over all pubs
            '
            ' get the selected editions into an array
            '
            Dim editionIDs As New List(Of Integer)
            For Each r As RepeaterItem In editionList.Items
                Dim editionField As HiddenField = CType(r.FindControl("EditionID"), HiddenField)
                Dim editionID As Integer = CommonRoutines.Hex2Int(editionField.Value)
                Dim editionCheck As CheckBox = CType(r.FindControl("EditionCheck"), CheckBox)
                If editionCheck.Checked Then editionIDs.Add(editionID)
                Ad.RemoveInstances(ATSystem.SysConstants.nullValue, editionID)       'remove all product instances for all open editions
            Next
            '
            ' validation - must select only one poroduct and if so, at least one edition
            '
            If productIDs.Count > 1 Then
                errorMsg.Text = Constants.OneSelectedProduct
                errorFlag = True
            Else
                If (productIDs.Count = 1) And (editionIDs.Count = 0) Then
                    errorMsg.Text = Constants.NoSelectedEdition
                    errorFlag = True
                Else
                    For Each productID As Integer In productIDs
                        For Each editionID As Integer In editionIDs
                            Ad.AddInstance(productID, editionID)
                        Next
                    Next
                End If
            End If
        Next
        '
        ' go to next step if no errors
        '
        If Not errorFlag Then
            If selectedProductCount = 0 Then
                Errormsg1.Text = Constants.NoSelectedProduct
                ErrorMsg2.Text = Constants.NoSelectedProduct
            Else
                Loader.NextASPX = ATLib.Loader.ASPX.AdCategorySelector
                Response.Redirect(Loader.Target)
            End If
        End If

    End Sub



    Protected Sub BtncontentEditor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtncontentEditor.Click, btnContentEditor2.Click
        updateInstances()

    End Sub

   
End Class
