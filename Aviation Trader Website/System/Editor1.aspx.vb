Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Edit System Parameters 1
'*
'* ON ENTRY:
'*
'*  Loader: objectID = undefined
'*
'***************************************************************************************

Partial Class Editor1
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem


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

        displaytabBar()
        displayLeftMenu()
        displaybuttonbar()

        If Not IsPostBack Then
            displaySystem()
        End If

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor1
        leftmenu.Add("System Parameters", Loader.Target, True)

        Loader.NextASPX = ATLib.Loader.ASPX.Technotes
        leftmenu.Add("Technotes", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.FolderListing
        Loader.ObjectID = sys.FirstFolderID
        leftmenu.Add("Folders", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.PublicationListing
        leftmenu.Add("Publications", Loader.Target, False)

        Loader.NextASPX = ATLib.loader.aspx.categorylisting
        leftmenu.Add("Categories", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.RotatorListing
        leftmenu.Add("Rotator Ads", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.NewsListing
        leftmenu.Add("News", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.UserListing
        leftmenu.Add("Users", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofList
        leftmenu.Add("Proof Reader", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.UserImpersonate
        leftmenu.Add("Impersonate User...", Loader.Target, False)


    End Sub

    Private Sub displaytabBar()

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor1
        topnode = New MenuNode("A", "Website", Loader.Target, True)
        tabbar.Nodes.Add(topnode)

         Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor2
       topnode = New MenuNode("B", "Email", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

    Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor3
       topnode = New MenuNode("B", "Twitter", Loader.Target, False)
        tabbar.Nodes.Add(topnode)


        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor4
        topnode = New MenuNode("D", "Production", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

    End Sub

    Private Sub displaySystem()

        buildinfo.Text = sys.BuildInfo

        NameBox.Text = sys.Name
        InternalURLBox.Text = sys.InternalURL
        ExternalURLBox.Text = sys.ExternalURL
        LRImageHeightBox.Text = sys.LRImageHeight.ToString
        THBImageHeightBox.Text = sys.THBImageHeight.ToString
        PhysicalApplicationPathBox.Text = sys.PhysicalApplicationPath
        FrontPicCaptionBox.Text = sys.FrontPicCaption
        BackPicCaptionBox.Text = sys.BackPicCaption

        Image1.ImageURL = sys.FrontImageURL
        Image1.ImageType = sys.FrontPicType

        Image2.ImageURL = sys.BackImageURL
        Image2.ImageType = sys.BackPicType

        Image3.ImageUrl = sys.CoverImageURL
 
        Dim EA As EnumAssistant

        EA = New EnumAssistant(New RotatorAd.Types)
        FrontPicTypeDD.DataSource = EA
        FrontPicTypeDD.DataBind()
        FrontPicTypeDD.SelectedValue = Convert.ToString(sys.FrontPicType)

        EA = New EnumAssistant(New RotatorAd.Types)
        BackPicTypeDD.DataSource = EA
        BackPicTypeDD.DataBind()
        BackPicTypeDD.SelectedValue = Convert.ToString(sys.BackPicType)


    End Sub

    Private Sub updatesystem()

        buildinfo.Text = sys.BuildInfo

        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength

        sys.FrontPicType = CType(FrontPicTypeDD.SelectedValue, RotatorAd.Types)
        sys.BackPicType = CType(BackPicTypeDD.SelectedValue, RotatorAd.Types)

        sys.Name = IV.ValidateText(NameBox, NameError)
        sys.PhysicalApplicationPath = IV.ValidatePath(PhysicalApplicationPathBox, PhysicalApplicationPathError)
        sys.InternalURL = IV.ValidateURL(InternalURLBox, InternalURLError)
        sys.ExternalURL = IV.ValidateURL(ExternalURLBox, ExternalURLError)
        sys.FrontPicCaption = IV.ValidateText(FrontPicCaptionBox, FrontPicCaptionError)
        sys.BackPicCaption = IV.ValidateText(BackPicCaptionBox, BackPicCaptionError)

        IV.MinValue = 100
        IV.MaxValue = 500
        sys.LRImageHeight = IV.ValidateInteger(LRImageHeightBox, LRImageHeightError)
        sys.THBImageHeight = IV.ValidateInteger(THBImageHeightBox, THBImageHeightError)

        If IV.ErrorCount = 0 Then
            sys.Update()
            ButtonBar.Msg = Constants.Saved
            displaySystem()
        End If

    End Sub

    Private Sub uploadPic(ByVal UploadID As Integer, ByVal fileupload As WebControls.FileUpload, ByVal msgbox As WebControls.Label)


        If Not fileupload.HasFile Then
            msgbox.Text = Constants.UploadSelect
        Else
            '
            ' get the file type from the browser determine image file type
            '
            Dim fileInfo As System.IO.FileInfo
            fileInfo = My.Computer.FileSystem.GetFileInfo(fileupload.FileName)
            Dim name As String = fileInfo.Name
            Dim xtn As String = fileInfo.Extension.ToLower
            Dim type As Image.ImageTypes = CommonRoutines.Xtn2Type(xtn)

            '
            ' allow jpg and swf option for front and back pageds
            ' for front page  only accept jpg
            '
            Dim Image As New Image
            Dim filename As String = ""
            msgbox.Text = ""

            Select Case UploadID

                Case 1
                    Dim selectedType As RotatorAd.Types = CType(FrontPicTypeDD.SelectedValue, RotatorAd.Types)

                    Select Case type
                        Case ATLib.Image.ImageTypes.JPG
                            If selectedType <> RotatorAd.Types.Image Then
                                msgbox.Text = Constants.InconsistentImageType
                            Else

                                sys.FrontSequence += 1
                                sys.FrontPicType = RotatorAd.Types.Image
                                sys.Update()
                                Image.GenerateSubsample(fileupload.FileContent, sys.FrontImageFilename, 570)
                            End If

                        Case ATLib.Image.ImageTypes.SWF
                             If selectedType <> RotatorAd.Types.Flash Then
                                msgbox.Text = Constants.InconsistentImageType
                            Else
                                sys.FrontSequence += 1
                                sys.FrontPicType = RotatorAd.Types.Flash
                                sys.Update()
                                fileupload.SaveAs(sys.FrontImageFilename)
                            End If

                        Case Else : msgbox.Text = Constants.UnsupportedFileType

                    End Select

                Case 2
                    Dim selectedType As RotatorAd.Types = CType(BackPicTypeDD.SelectedValue, RotatorAd.Types)

                    Select Case type
                        Case ATLib.Image.ImageTypes.JPG
                             If selectedType <> RotatorAd.Types.Image Then
                                msgbox.Text = Constants.InconsistentImageType
                            Else
                                sys.BackSequence += 1
                                sys.BackPicType = RotatorAd.Types.Image
                                sys.Update()
                                Image.GenerateSubsample(fileupload.FileContent, sys.BackImageFilename, 570)
                            End If

                        Case ATLib.Image.ImageTypes.SWF
                            If selectedType <> RotatorAd.Types.Flash Then
                                msgbox.Text = Constants.InconsistentImageType
                            Else
                                sys.BackSequence += 1
                                sys.BackPicType = RotatorAd.Types.Flash
                                sys.Update()
                                fileupload.SaveAs(sys.BackImageFilename)
                            End If

                        Case Else : msgbox.Text = Constants.UnsupportedFileType
                    End Select

                Case 3
                    Select Case type
                        Case ATLib.Image.ImageTypes.JPG
                            sys.CoverSequence += 1
                            sys.Update()
                            Image.GenerateSubsample(fileupload.FileContent, sys.CoverImageFilename, 570)

                        Case Else : msgbox.Text = Constants.UnsupportedFileType
                    End Select
            End Select
        End If
        displaySystem()

    End Sub

    Protected Sub Upload1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtUpload1.Click
        uploadPic(1, FileUpload1, msgbox1)
    End Sub

    Protected Sub Upload2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtUpload2.Click
        uploadPic(2, FileUpload2, msgbox2)
    End Sub


    Protected Sub Upload3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtUpload3.Click
        uploadPic(3, FileUpload3, msgbox3)
    End Sub


    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : updatesystem()
        End Select
    End Sub

End Class
