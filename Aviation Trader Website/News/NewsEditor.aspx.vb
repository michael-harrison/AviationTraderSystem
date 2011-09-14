Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* News Editor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = NewsItemID
'*
'***************************************************************************************

Partial Class NewsEditor
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private NewsItem As NewsItem


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.skin

        headerbar.Slot = Slot
        headerbar.Loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim NewsItems As New ATLib.NewsItems
        NewsItem = NewsItems.Retrieve(Loader.ObjectID)
        displayLeftMenu()
        displayButtonBar()


        If Not IsPostBack Then
            displayItem()
        End If

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B0.Text = "Delete"
        ButtonBar.B1.Text = "RETURN TO LIST"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.NextASPX = ATLib.Loader.ASPX.NewsListing
        leftmenu.Add("News Items", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.NewsEditor
        leftmenu.Add("News Item " & NewsItem.ID, Loader.Target, True)

    End Sub


    Private Sub displayItem()

        Namebox.Text = NewsItem.Name

        Dim EA As EnumAssistant

        EA = New EnumAssistant(New NewsItem.ProdnState)
        StatusDropDown.DataSource = EA
         StatusDropDown.DataBind()
        StatusDropDown.SelectedValue = Convert.ToString(NewsItem.ProdnStatus)

        IntroBox.Text = Server.HtmlDecode(NewsItem.Intro)
        BodyBox.Text = Server.HtmlDecode(NewsItem.Body)
        PicCaptionBox.Text = NewsItem.PicCaption
        '
        ' display the pic if an image has been uploaded
        '
        picPanel.Visible = NewsItem.HasImage
        pic.ImageUrl = NewsItem.ImageURL


    End Sub

    Private Sub updateItem()

        NewsItem.ProdnStatus = CType(StatusDropDown.SelectedValue, NewsItem.ProdnState)

        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength
        NewsItem.Name = IV.ValidateText(Namebox, NameError)

        IV.MinStringLength = 0         'allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.textCharLength
        NewsItem.Intro = IV.ValidateText(IntroBox, IntroError)
        NewsItem.Body = IV.ValidateText(BodyBox, BodyError)

        If deleteCheck.Checked Then
            NewsItem.HasImage = False
        Else
            NewsItem.PicCaption = IV.ValidateText(PicCaptionBox, PicCaptionError)
        End If

   
        If IV.ErrorCount = 0 Then
            NewsItem.Update()
            ButtonBar.Msg = Constants.Saved
            displayItem()
        End If
    End Sub

    Private Sub return2List()
        '
        ' go back to the NewsItem list
        '
        Loader.NextASPX = Loader.ASPX.NewsListing
        Response.Redirect(Loader.Target)

    End Sub

    Private Function mapStatus2Tab(ByVal Status As NewsItem.ProdnState) As Integer

        Select Case Status
            Case ATLib.NewsItem.ProdnState.Incomplete : Return 0
            Case ATLib.NewsItem.ProdnState.Latest : Return 1
            Case ATLib.NewsItem.ProdnState.Current : Return 2
            Case ATLib.NewsItem.ProdnState.Archived : Return 3
        End Select

    End Function

    Private Sub deleteItem()
        NewsItem.Deleted = True
        NewsItem.Update()
        return2List()

    End Sub

    Private Sub uploadPic()

        If Not FileUpload1.HasFile Then
            msgbox.Text = Constants.UploadSelect
        Else
            '
            ' get the file type from the browser determine image file type
            '
            Dim fileInfo As System.IO.FileInfo
            fileInfo = My.Computer.FileSystem.GetFileInfo(FileUpload1.FileName)
            Dim name As String = fileInfo.Name
            Dim xtn As String = fileInfo.Extension.ToLower
            Dim type As Image.ImageTypes = CommonRoutines.Xtn2Type(xtn)

            If type <> Image.ImageTypes.JPG Then
                msgbox.Text = Constants.UnsupportedFileType
            Else
                '
                ' file image into website with id of this news item
                ' Use the image class to generate resized image from stream
                '         
                Dim Image As New Image
                NewsItem.PhysicalApplicationPath = sys.PhysicalApplicationPath
                Image.GenerateSubsample(FileUpload1.FileContent, NewsItem.ImageFilename, 200)
                deleteCheck.Checked = False
                NewsItem.HasImage = True
            End If
        End If
    End Sub

    Protected Sub Upload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtUpload.Click

        uploadPic()
        updateItem()            'save any pending changes and redisplay
    End Sub




    Protected Sub ButtonBar_buttonBarEvnt(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent

        Select Case buttonNumber
            Case 0 : deleteItem()
            Case 1 : return2List()
            Case 2 : updateItem()
        End Select
    End Sub

End Class
