Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* SelectorButton
'*
'*
'***************************************************************************************
Partial Class ATControls_PriceBlock
    Inherits System.Web.UI.UserControl

    Public Event PriceChange(ByVal PB As ATControls_PriceBlock)


    Private _adInstance As AdInstance
    Private _isReadonly As Boolean

    Public Property IsReadonly() As Boolean
        Get
            Return _isReadonly
        End Get
        Set(ByVal value As Boolean)
            _isReadonly = value
        End Set
    End Property

    Public Property AdInstance() As AdInstance
        Get
            '
            ' on a postback recover object from viewstate id
            '
            If _adInstance Is Nothing Then
                Dim instanceID As Integer = CommonRoutines.Hex2Int(ViewState.Item("AdInstanceID").ToString)
                Dim adinstances As New AdInstances
                _adInstance = adinstances.Retrieve(instanceID)
            End If
            Return _adInstance
        End Get
        Set(ByVal value As AdInstance)
            _adInstance = value
            '
            ' save instance ID in viewstate
            '
            ViewState.Add("AdInstanceID", AdInstance.hexID)
        End Set
    End Property

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        displayPB()
    End Sub
    Private Sub displayPB()


        displayPanel.Visible = False
        classadPanel.Visible = False


        Dim mysys As New ATSystem
        mysys.Retrieve()


        Select Case AdInstance.ProductType
            Case Product.Types.DisplayComposite, Product.Types.DisplayFinishedArt, Product.Types.DisplayStandAlone
                showDisplayPanel()

            Case Product.Types.DisplayModule        'nothing yet

            Case Product.Types.WebBasic, Product.Types.WebPremium, Product.Types.WebPDF, Product.Types.WebPDFText, Product.Types.WebFeaturedAd
  
            Case Product.Types.ClassadColorPic, Product.Types.ClassadMonoPic, Product.Types.ClassadTextOnly
                showClassadPanel(mysys.ClassadLineHeight)

            Case Else

        End Select

        '
        ' product name
        '
        productlabel.text = AdInstance.Product.Name
        '
        ' price summary
        '
        PriceAdjustBox.Text = CommonRoutines.Integer2Dollars(AdInstance.PriceAdjust)
        PriceAdjustLabel.Text = PriceAdjustBox.Text
        priceadjustedit.Visible = True
        priceadjustread.Visible = False
        If _isReadonly Then
            priceadjustedit.Visible = False
            priceadjustread.Visible = True
        End If

        priceLabel.Text = "$" & CommonRoutines.Integer2Dollars(AdInstance.Price)
        subtotalLabel.Text = "$" & CommonRoutines.Integer2Dollars(AdInstance.Subtotal)

    End Sub

    Private Sub showDisplayPanel()
        displayPanel.Visible = True


        Dim EA As EnumAssistant

        EA = New EnumAssistant(New RateTable.DisplayWidths)
        DisplayWidthDD.DataSource = EA
        DisplayWidthDD.DataBind()
        DisplayWidthDD.SelectedValue = Convert.ToString(AdInstance.Width)
        '
        ' suppress unallowed values from width dd
        '
        Dim min As Integer = RateTable.DisplayHeights.cm1
        Dim max As Integer = RateTable.DisplayHeights.cm38
        If AdInstance.Width = RateTable.DisplayWidths.Col2 Or AdInstance.Width = RateTable.DisplayWidths.Col3 Then
            min = RateTable.DisplayHeights.cm2
        End If

        EA = New EnumAssistant(New RateTable.DisplayHeights, min, max)
        DisplayHeightDD.DataSource = EA
        DisplayHeightDD.DataBind()
        DisplayHeightDD.SelectedValue = Convert.ToString(AdInstance.Height)

        EA = New EnumAssistant(New RateTable.DisplayColorTypes)
        displayColorDD.DataSource = EA
        displayColorDD.DataBind()
        displayColorDD.SelectedValue = Convert.ToString(AdInstance.Color)

        displaywidthlabel.text = DisplayWidthDD.SelectedItem.Text
        displayheightlabel.text = DisplayHeightDD.SelectedItem.Text
        DisplayColorLabel.Text = DisplayColorDD.SelectedItem.Text
        '
        ' show exact size for diagnostics
        '
        ComputedSizelabel.Text = CommonRoutines.Integer2mm(AdInstance.ExactHeight, 3) & " mm x " & CommonRoutines.Integer2mm(AdInstance.ExactWidth, 3) & " mm"

        '
        ' set control visibility
        '
        displaywidthlabel.visible = _isReadonly
        displayheightlabel.visible = _isReadonly
        displaycolorlabel.visible = _isReadonly
        DisplayHeightDD.Visible = Not _isReadonly
        DisplayWidthDD.Visible = Not _isReadonly
        displayColorDD.Visible = Not _isReadonly
        
    End Sub

    Private Sub showClassadPanel(ByVal classadLineHeight As Integer)
        classadPanel.Visible = True


        Dim EA As EnumAssistant


        EA = New EnumAssistant(New RateTable.ClassadHeights)
        ClassadLinesDD.DataSource = EA
        ClassadLinesDD.DataBind()
        ClassadLinesDD.SelectedValue = Convert.ToString(AdInstance.Height)
        Dim computedLines As Double = AdInstance.ExactHeight / classadLineHeight
        ComputedHeightlabel.Text = FormatNumber(computedLines, 3)
        ClassadLinesLabel.Text = ClassadLinesDD.SelectedValue

        ClassadLinesDD.Visible = Not _isReadonly
        ClassadLinesLabel.Visible = _isReadonly

    End Sub


    Public Sub Update()
        '
        ' updates just the prodn surcharge and price adjustment into the instance
        '
        Dim IV As New InputValidator
        AdInstance.PriceAdjust = IV.ValidateDollars(PriceAdjustBox, PriceAdjustError)
        AdInstance.Update()
    End Sub

    Private Sub recalcDisplayPrice()
        '
        ' recalcs the price if any of the width, height or color dropdowns changes
        ' get the current dropdown values
        '

        AdInstance.Color = CType(displayColorDD.SelectedValue, RateTable.DisplayColorTypes)
        AdInstance.Width = CType(DisplayWidthDD.SelectedValue, RateTable.DisplayWidths)
        AdInstance.Height = CType(DisplayHeightDD.SelectedValue, RateTable.DisplayHeights)
        '
        ' incompatible width and height - reset width
        '
        If AdInstance.Width = RateTable.DisplayWidths.Col2 Or AdInstance.Width = RateTable.DisplayWidths.Col3 Then
            If AdInstance.Height = RateTable.DisplayHeights.cm1 Then
                AdInstance.Height = RateTable.DisplayHeights.cm2
            End If
        End If

        Dim rt As New RateTable
        rt.LoadDisplayTable(Constants.DisplayRates)
        AdInstance.Price = rt.GetDisplayRate(AdInstance)
        AdInstance.Update()

    End Sub

    Private Sub recalcClassadprice()
        '
        ' reecalcs the classad price if the lines changes
        '
        AdInstance.Height = CType(ClassadLinesDD.SelectedValue, RateTable.ClassadHeights)
        Dim rt As New RateTable
        rt.LoadClassadTable(Constants.ClassadRates)
        AdInstance.Price = rt.GetClassadRate(AdInstance)
        AdInstance.Update()

    End Sub

    Private Sub adjustPrice()


        Dim iv As New InputValidator
        AdInstance.PriceAdjust = iv.ValidateDollars(PriceAdjustBox, PriceAdjustError)
        AdInstance.Update()

    End Sub



    Protected Sub displayColorDD_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles displayColorDD.SelectedIndexChanged
        recalcDisplayPrice()
        RaiseEvent PriceChange(Me)
    End Sub


    Protected Sub DisplayHeightDD_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DisplayHeightDD.SelectedIndexChanged
        recalcDisplayPrice()
        RaiseEvent PriceChange(Me)
    End Sub


    Protected Sub DisplayWidthDD_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DisplayWidthDD.SelectedIndexChanged
        recalcDisplayPrice()
        RaiseEvent PriceChange(Me)
    End Sub


    Protected Sub ClassadLinesDD_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClassadLinesDD.SelectedIndexChanged
        recalcClassadprice()
        RaiseEvent PriceChange(Me)
    End Sub


    Protected Sub PriceAdjustBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PriceAdjustBox.TextChanged
        adjustPrice()
        RaiseEvent PriceChange(Me)
    End Sub


End Class
