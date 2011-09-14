Option Strict On
Option Explicit On
Imports System.ComponentModel

'***************************************************************************************
'*
'* Loader
'*
'*
'*
'***************************************************************************************

''' <summary>
''' The loader class is used to propate state information between server and client,
''' and serves as the primary dispatch mechanism for driving the user to the correcf
''' ASPX page and tab, and with the correct object context. 
''' <para></para>
''' <para>The loader packs a structure of 32 bit integer values into a set of nine 8
''' byte hex strings, and then transfers that data between browser and server as the
''' querystring.  The loader data structure is: </para>
''' <para></para>
''' <para></para>
''' <code>Name		Symbol		Type			Hex bytes
''' NextASPX		(X)		Loader.ASPX		XXXXXXXX
''' Referrer		(R)		Loader.ASPX		RRRRRRRR
''' SlotID		(S)		Integer			SSSSSSSS
''' SelectedTab	(N)		integer			NNNNNNNN
''' ObjectType	(T)		Integer			TTTTTTTT
''' ObjectID		(O)		Integer			OOOOOOOO
''' Param1		(P)		Integer			PPPPPPPP</code>
''' <para></para>
''' <para></para>
''' <para>In addition, the NextASPX value maps to an aspx page name, to yield a URL
''' for each target frame as follows:</para>
''' <para></para>
''' <para>pagename.aspx?XXXXXXXXRRRRRRRRSSSSSSSSFFFFFFFFLLLLLLLLNNNNNNNNTTTTTTTTOOOOOOOOPPPPPPPP</para>
''' <para></para>
''' <para>There are four frames in the system - three inline frames and a popup used
''' for documentation and other transient information. The final loader URL packs
''' each of the frame-specific URLS into a combined sting which becomes a
''' client-side javascript call to the top level parent page which then requests the
''' server to load each of the four frames with the specified content encoded in the
''' frame-specific query string, as follows:</para>
''' <para></para>
''' <para>javascript:parent.load(menu,tree,content)</para>
''' <para></para>
''' <para>Any or all four frames can be transmitted from server to broswer client by
''' this single call to parent.load.</para>
''' <para></para>
''' <para>The loader embeds these four frames as loader.Menu, loader.BFrame,
''' loader.CFrame and loader.Popup. Most of the activity takes place in the BFrame
''' (typically used as the left hand icon menu) and the CFrame (typically used as
''' the main content panel.) To facilitate this, when the query string is supplied
''' into the loader on an incoming request from the browser, the loader, uses a
''' synonym name of &quot;this&quot; for either the BFrame or CFrame. Hence
''' Loader.this refers to a request for the current incoming frame data, which might
''' be either the BFrame or CFrame.</para>
''' <para></para>
''' <para>The loader therefore acts as a translator between the querystring form and
''' the individual integer fields. Most of the properties of the loader therefore
''' provide a simple mechanism for supplying information into the loader and
''' extracting information from it.</para>
''' </summary>
Public Class Loader


    ''' <summary>
    ''' This is a numeric list of ASPX pages which are translated by the Loader into a string page description, which is ultimately incorporated into a URL.
    ''' These values are incorporated into the Loader QueryString as the NextASPX and Referrer fields.</summary>
    ''' <includesource>yes</includesource>
    Public Enum ASPX
        '
        ' Defines all the aspx pages that can be loaded.
        '
        Null                                'Null page, never accessed
        GlobalAsax                          'modified page to trap errors on large uploaded files
        HomeGuest                           'home page - guest
        HomeRegistered                      'home page - registered users
        HomeINH                             'home page - admin users
        Login                               'login panel
        Register                            'user registration panel
        Logout                              'login panel
        ForgotPW
        SystemEditor1                       'system editor 1
        SystemEditor2                       'system editor 2
        SystemEditor3                       'system editor 3
        SystemEditor4                       'system editor 4
        SkinTest
        Slideshow
        Technotes
        TechnoteEditor
        UserListing
        UserEditor1
        UserEditor2
        UserEditor3
        RotatorListing
        RotatorEditor                       'rotator ad editor
        ProofApprovalRQ                     'proof approval request
        ForgotPWEmail                       'forgot password email
        EditionCloseReminderEmail
        AdConfirmationEmail                 'ad confirmation email
        SubsRequestEmail
        RegistrationWelcomeEmail            'registration welcome new users
        ProdnNoteEmail                      'prodn note email
        FolderListing
        FolderEditor
        PublicationListing
        PublicationEditor                   'product editor
        ProductListing
        ProductEditor                       'product editor
        Editionlisting
        EditionEditor                       'Edition editor
        CategoryListing
        CategoryEditor
        ClassificationListing
        ClassificationEditor
        SpecGroupListing
        SpecGroupEditor
        SpecDefinitionListing
        SpecDefinitionEditor
        NewsListing
        NewsEditor
        NewsReader
        AllStories
        BrowseAllClassifications
        BrowseList                          'browse listings by category
        SearchList                          'browse listings by search key
        TextReader
        ImageReader
        VideoReader
        SpecReader
        SellerReader
        NoBrowseAds                         'no browse ad page
        NoSearchAds                         'no search ad page
        UserImpersonate                     'Impersonate another user
        SubsManager                         'subscription Manager
        AdManager                           'ad manager
        MyAds                               'display current or archived ads
        MMAPriceReader                         'displays readonly version of prices
        MMAContactProdn                      'contact prodn for change
        CopyAd                              'mma copy ad function
        RepeatAd                            'mma repeat ad function
        SubmitAd                            'mma submitad function
        NewAd                               'new ad creator
        AdCategorySelector                  'page 1 of ad process
        AdTextEditor                        'page 2 of ad process
        AdImageUploader                     'page 2 of ad process
        AdPDFUploader
        AdSpecEditor                        'page 2 of ad process
        AdProdnNote                        'page 2 of ad process
        AdProductSelector                   'page 3 of ad process
        AdReview                            'page 4 of ad process
        AdPayAndSubmit                      'page 5 of ad process
        AdCancelConfirmation                'page 6 of ad process
        AdSaveConfirmation                'page 6 of ad process
        AdSubmitConfirmation                'page 6 of ad process
        MMAPrices
        MMAPreview
        MMAProofApproval                       'Proof approval for customers
        MMAProdnNote
        AboutHome
        AboutHistory
        AboutTeam
        AboutPub
        Testimonials
        Deadlines
        AboutOpps
        AdHome
        AdOptions
        AdRates
        AdSpecs
        SubsHome
        ContactHome
        Weboffer
        FAQ
        ProofList                           'Proof read listing display
        ProofTextEditor                     'Proof reader edit function
        ProofImageUploader                  'Proof reader edit function
        ProofSpecEditor                     'Proof reader edit function
        ProofSellerInfo                     'Proof reader edit function
        ProofProdnNoteEditor                'Proof reader edit function
        ProofCategoryEditor                 'Proof reader category editor
        ProofProductEditor                  'Proof reader Product editor
        ProofPreview                        'Proof reader preview
        ProofPriceEditor                    'Proof reader Price editor


    End Enum

    Private pages() As String = { _
      "", _
      "Global.asax", _
      "HomeGuest.aspx", _
      "HomeRegistered.aspx", _
      "HomeINH.aspx", _
      "System/Login.aspx", _
      "System/Register.aspx", _
      "System/Logout.aspx", _
      "System/ForgotPW.aspx", _
      "System/Editor1.aspx", _
      "System/Editor2.aspx", _
      "System/Editor3.aspx", _
      "System/Editor4.aspx", _
      "System/SkinTest.aspx", _
      "BrowseAndSearch/Slideshow.aspx", _
      "System/Technotes.aspx", _
      "System/TechnoteEditor.aspx", _
      "System/UserListing.aspx", _
      "System/UserEditor1.aspx", _
      "System/UserEditor2.aspx", _
      "System/UserEditor3.aspx", _
      "RotatorAds/RotatorListing.aspx", _
      "RotatorAds/RotatorEditor.aspx", _
      "Email/ProofApprovalRQ.aspx", _
      "Email/ForgotPW.aspx", _
      "Email/EditionCloseReminder.aspx", _
      "Email/AdConfirmation.aspx", _
      "Email/SubsRequest.aspx", _
      "Email/RegistrationWelcome.aspx", _
      "Email/ProdnNote.aspx", _
      "Folder/FolderListing.aspx", _
      "Folder/FolderEditor.aspx", _
      "Publication/PublicationListing.aspx", _
      "Publication/PublicationEditor.aspx", _
      "Publication/ProductListing.aspx", _
      "Publication/ProductEditor.aspx", _
      "Publication/EditionListing.aspx", _
      "Publication/EditionEditor.aspx", _
      "Category/CategoryListing.aspx", _
      "Category/CategoryEditor.aspx", _
      "Category/ClassificationListing.aspx", _
      "Category/ClassificationEditor.aspx", _
      "Category/SpecGroupListing.aspx", _
      "Category/SpecGroupEditor.aspx", _
      "Category/SpecDefinitionListing.aspx", _
      "Category/SpecDefinitionEditor.aspx", _
      "News/NewsListing.aspx", _
      "News/NewsEditor.aspx", _
      "News/Reader.aspx", _
      "News/AllStories.aspx", _
      "BrowseAndSearch/BrowseAllClassifications.aspx", _
      "BrowseAndSearch/BrowseList.aspx", _
      "BrowseAndSearch/SearchList.aspx", _
      "BrowseAndSearch/Text.aspx", _
      "BrowseAndSearch/Images.aspx", _
      "BrowseAndSearch/Video.aspx", _
      "BrowseAndSearch/Specs.aspx", _
      "BrowseAndSearch/Seller.aspx", _
      "BrowseAndSearch/NoBrowseAds.aspx", _
      "BrowseAndSearch/NoSearchAds.aspx", _
      "Advertise/Impersonate.aspx", _
      "Subs/SubsManager.aspx", _
      "MMA/AdManager.aspx", _
      "MMA/MyAds.aspx", _
      "MMA/PriceReader.aspx", _
      "MMA/ContactProdn.aspx", _
      "MMA/CopyAd.aspx", _
      "MMA/RepeatAd.aspx", _
      "MMA/SubmitAd.aspx", _
      "Advertise/NewAd.aspx", _
      "Advertise/CategorySelector.aspx", _
      "Advertise/TextEditor.aspx", _
      "Advertise/ImageUploader.aspx", _
      "Advertise/PDFUploader.aspx", _
      "Advertise/SpecEditor.aspx", _
      "Advertise/ProdnNote.aspx", _
      "Advertise/ProductSelector.aspx", _
      "Advertise/Review.aspx", _
      "Advertise/PayAndSubmit.aspx", _
      "Advertise/CancelConfirmation.aspx", _
      "Advertise/SaveConfirmation.aspx", _
      "Advertise/SubmitConfirmation.aspx", _
      "MMA/PriceReader.aspx", _
      "MMA/Preview.aspx", _
      "MMA/ProofApproval.aspx", _
      "MMA/ProdnNote.aspx", _
      "Static/AboutHome.aspx", _
      "Static/AboutHistory.aspx", _
      "Static/AboutTeam.aspx", _
      "Static/AboutPub.aspx", _
      "Static/Testimonials.aspx", _
      "Static/Deadlines.aspx", _
      "Static/AboutOpps.aspx", _
      "Static/AdHome.aspx", _
      "Static/AdOptions.aspx", _
      "Static/AdRates.aspx", _
      "Static/AdSpecs.aspx", _
      "Static/SubsHome.aspx", _
      "Static/ContactHome.aspx", _
      "Static/WebOffer.aspx", _
      "Static/FAQ.aspx", _
      "Production/ProofList.aspx", _
      "Production/TextEditor.aspx", _
      "Production/ImageUploader.aspx", _
      "Production/SpecEditor.aspx", _
      "Production/SellerInfo.aspx", _
      "Production/ProdnNoteEditor.aspx", _
      "Production/CategoryEditor.aspx", _
      "Production/ProductEditor.aspx", _
      "Production/Preview.aspx", _
      "Production/PriceEditor.aspx" _
       }


    ''' <summary>
    ''' Defines a bitmap of buttons in the top menu. Passed to client side javascript and used to control which buttons are in an up, down or disabled state.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum menuMask
        BNull = 0
        '
        ' first 16 bits are the down states of up to 16 buttons
        '
        B1 = &H1
        B2 = &H2
        B3 = &H4
        B4 = &H8
        B5 = &H10
        B6 = &H20
        B7 = &H40
        B8 = &H80
        '
        ' second 16 bits are the enabled states of up to 16 buttons
        '
        B1Enabled = &H10000
        B2Enabled = &H20000
        B3Enabled = &H40000
        B4Enabled = &H80000
        B5Enabled = &H100000
        B6Enabled = &H200000
        B7Enabled = &H400000
        B8Enabled = &H800000
        '
        ' synonyms for common states
        '
        BAllEnabled = B1Enabled Or B2Enabled Or B3Enabled Or B8Enabled
        Home = B1 Or BAllEnabled
        Cart = B2 Or BAllEnabled
        Orders = B3 Or BAllEnabled
        Logout = B8 Or BAllEnabled
        DataWizard = B1 Or B1Enabled
    End Enum

    Public Const DataLength As Integer = 8 * 7
    Private _queryString As String
    Private _slotID As Integer
    Private _thisASPX As Loader.ASPX
    Private _nextASPX As Loader.ASPX
    Private _referrer As Loader.ASPX
    Private _selectedTab As Integer
    Private _objectType As ATSystem.ObjectTypes
    Private _objectID As Integer
    Private _param1 As Integer
    Private _nextASPXName As String
    Private _applicationPath As String
    Private _encryptionOn As Boolean = False

    ''' <summary>
    ''' Instantiates the object and sets up the 4 embedded frames.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _applicationPath = ""
    End Sub

    ''' <summary>
    ''' Instantiates the loader and provides it with the incoming query string.
    ''' </summary>
    ''' <param name="QS">72 character encoded querystring.</param>
    ''' <example>
    ''' dim Loader as New Loader(Request.QueryStrings(0))
    '''</example>
    Public Sub New(ByVal QS As String)
        _applicationPath = ""
        Dim dQS As String = decrypt(QS)
        '
        ' it may be a short qs
        '
        If dQS.Length = 8 Then
            _objectID = Hex2Int(dQS.Substring(0, 8))
        Else
            _nextASPX = ASPX.Null
            _thisASPX = CType(Hex2Int(dQS.Substring(0, 2)), Loader.ASPX)
            _referrer = CType(Hex2Int(dQS.Substring(2, 2)), Loader.ASPX)
            _slotID = Hex2Int(dQS.Substring(4, 8))
            _objectType = CType(Hex2Int(dQS.Substring(12, 2)), ATSystem.ObjectTypes)
            _objectID = Hex2Int(dQS.Substring(14, 8))
            _param1 = Hex2Int(dQS.Substring(22, 8))
            _selectedTab = Hex2Int(dQS.Substring(30, 2))
        End If
    End Sub


    ''' <summary>
    ''' Sets or gets the applicatinj path. Normally there's no need to ever set it since
    ''' the call to getappliationpath will do this. Email and inhapps however do need to 
    ''' explcitly set it to either the internal or extenal address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ApplicationPath() As String
        Get
            If _applicationPath.Length = 0 Then
                _applicationPath = GetApplicationPath()
            End If
            Return _applicationPath
        End Get
        Set(ByVal value As String)
            _applicationPath = value
            If Not (_applicationPath.EndsWith("/")) Then _applicationPath &= "/"

        End Set
    End Property


    ''' <summary>
    ''' Returns the Loader.ASPX id of the referring page - ie the page that caused the current
    ''' page to be displayed.
    ''' </summary>
    ''' <value></value>
    Public Property Referrer() As Loader.ASPX
        Get
            Return _referrer
        End Get
        Set(ByVal value As Loader.ASPX)
            _referrer = value
        End Set
    End Property

    ''' <summary>
    ''' Returns the Loader.ASPX id of the referring page - ie the page that caused the current
    ''' page to be displayed.
    ''' </summary>
    ''' <value></value>
    Public Property ThisASPX() As Loader.ASPX
        Get
            Return _thisASPX
        End Get
        Set(ByVal value As Loader.ASPX)
            _thisASPX = value
        End Set
    End Property


    ''' <summary>
    ''' Defines the currently selected tab. On set, this specifies which tab is to be highlighted
    ''' when the page is displayed. On get, provides server side code with the id of the tab that was clicked.
    ''' </summary>
    ''' <value></value>
    Public Property SelectedTab() As Integer
        Get
            Return _selectedTab
        End Get
        Set(ByVal value As Integer)
            _selectedTab = value
        End Set
    End Property

    ''' <summary>
    ''' Object ID of the current obect, as specified by the ObjectType property.
    ''' </summary>
    Public Property ObjectID() As Integer
        Get
            Return _objectID
        End Get
        Set(ByVal value As Integer)
            _objectID = value
        End Set
    End Property

    ''' <summary>
    ''' ObjectType, from one of ATSystem.ObjectTypes, which qualifies teh 
    '''  ObjectID property.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ObjectType() As ATSystem.ObjectTypes
        Get
            Return _objectType
        End Get
        Set(ByVal value As ATSystem.ObjectTypes)
            _objectType = value
        End Set
    End Property

    ''' <summary>
    '''Optional parameter, used typically to provide a second objectID, when two are required. 
    ''' </summary>
    ''' <value></value>
    Public Property Param1() As Integer
        Get
            Return _param1
        End Get
        Set(ByVal value As Integer)
            _param1 = value
        End Set
    End Property


    ''' <summary>
    ''' The current page ASPX, returned as a string value representing the real ASPX page name
    ''' </summary>
    Public ReadOnly Property ASPXName() As String
        Get
            Return pages(_thisASPX)
        End Get
    End Property


    ''' <summary>
    ''' The ASPX field is the ASPX page to load on the next incoming browser request. If its not set by the caller,
    '''  or its set to Loader.ASPX.Null,  then it means that the page in this frame should not be replaced on the next delivery from 
    ''' server to client
    ''' </summary>
    Public Property NextASPX() As Loader.ASPX
        Get
            Return _nextASPX
        End Get
        Set(ByVal value As Loader.ASPX)
            _nextASPX = value
            _nextASPXName = pages(_nextASPX)
        End Set
    End Property

    ''' <summary>
    ''' The current next page ASPX, returned as a string value representing the real ASPX page name
    ''' </summary>
    Public Property NextASPXName() As String
        Get
            Return _nextASPXName
        End Get
        Set(ByVal value As String)
            _nextASPXName = value
        End Set
    End Property


    ''' <summary>
    ''' returns the URL for this frame frame
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property Target() As String
        Get
            If NextASPXName = "" Then
                Return ""
            Else
                Return ApplicationPath() & _nextASPXName & "?" & encrypt(QueryString)
            End If
        End Get
    End Property

    ''' <summary>
    ''' returns the short URL consisting of just the objectid
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property ShortTarget() As String
        Get
            If NextASPXName = "" Then
                Return ""
            Else
                Return ApplicationPath() & _nextASPXName & "?" & encrypt(CommonRoutines.Int2Hex(_objectID))
            End If
        End Get
    End Property

    ''' <summary>
    ''' returns a parameterless URL with no QS
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property ParamLessTarget() As String
        Get
            If NextASPXName = "" Then
                Return ""
            Else
                Return ApplicationPath() & _nextASPXName
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns the querystring
    ''' </summary>
    Public ReadOnly Property QueryString() As String
        Get
            '
            ' returns the querystring for the specific frame
            '
            Return Int2ShortHex(_nextASPX) & _
            Int2ShortHex(_thisASPX) & _
            Int2Hex(_slotID) & _
            Int2ShortHex(_objectType) & _
            Int2Hex(_objectID) & _
            Int2Hex(_param1) & _
            Int2ShortHex(_selectedTab)
        End Get
    End Property


    ''' <summary>
    ''' SlotID of the current slot object for this session
    ''' </summary>
    Public Property SlotID() As Integer
        Get
            Return _slotID
        End Get

        Set(ByVal value As Integer)
            _slotID = value
        End Set
    End Property

    ''' <summary>
    ''' Returns a copy of the curent loader
    ''' this is used for calls that will destroy the loader contents
    ''' eg passing loader as a parameter really only passes the pointer
    ''' so this is a mechanism for passing a disposable copy
    ''' </summary>
    ''' <returns>Copy of loader as new object</returns>
    ''' <example>
    ''' dim myLoader As Loader = yourLoader.Copy
    ''' </example>
    Public Function Copy() As Loader
        '
        '
        Dim loaderCopy As New Loader()
        '
        ' copy the loader level contents
        '
        loaderCopy.SlotID = _slotID
        loaderCopy.ThisASPX = _thisASPX
        loaderCopy.NextASPX = _nextASPX
        loaderCopy.Referrer = _referrer
        loaderCopy.SelectedTab = _selectedTab
        loaderCopy.ObjectType = _objectType
        loaderCopy.ObjectID = _objectID
        loaderCopy.Param1 = _param1

        Return loaderCopy

    End Function

    Private Function encrypt(ByVal s As String) As String
        '
        ' encrypts a string
        '
        If _encryptionOn Then
            Dim sym As New Encryption.Symmetric(Encryption.Symmetric.Provider.Rijndael)
            Dim key As New Encryption.Data("LoaderData")
            Dim encryptedData As Encryption.Data
            encryptedData = sym.Encrypt(New Encryption.Data(s), key)
            Return encryptedData.ToHex
        Else
            Return s
        End If
    End Function

    Private Function decrypt(ByVal s As String) As String
        If _encryptionOn Then
            Dim sym As New Encryption.Symmetric(Encryption.Symmetric.Provider.Rijndael)
            Dim key As New Encryption.Data("LoaderData")
            Dim encryptedData As New Encryption.Data
            encryptedData.Hex = s
            Dim decryptedData As Encryption.Data
            decryptedData = sym.Decrypt(encryptedData, key)
            Return decryptedData.ToString
        Else
            Return s
        End If
    End Function

End Class




