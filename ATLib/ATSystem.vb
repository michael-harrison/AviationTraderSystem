Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.ComponentModel
Imports System.Net
Imports System.Xml
Imports System.Runtime.Remoting

'***************************************************************************************
'*
'* ATSystem
'*
'*
'*
'***************************************************************************************


''' <summary>The System object is the root node of the database. It manages the System table in the database. All other
''' objects are subordinate to the System. In the current implementation,
''' there is only one System record in the DB and therefore only one System object.
''' The UI does not currently support more than one System, but implementers should
''' note that the database structure and the object model does indeed support
''' multiple Systems if that is ever required.
''' </summary>
''' <version>V1.000 16-JUL-2007</version>
Public Class ATSystem
    Private Const _swbuild As Integer = 5827
    Private Const _buildDate As String = "June 23, 2010 13:58"

    Friend Enum ObjectState
        Modified = 1
        Deleted = 6
        Original = 3
        Initial = 4
    End Enum

    ''' <summary> This is the definition of bits in the status word for all objects. The low 16 bits are
    ''' common to all classes, and the high 16 bits can be class-specific and therefore
    ''' have overlapping values. The class that uses these high bits is identified in
    ''' the comment</summary>
    ''' <includesource>yes</includesource>
    Public Enum StatusBits
        '
        ' this is the status word for all objects. First 16 bits only reserved for common status
        '
        IsSpecActive = &H1               'spec
        IsPreviewValid = &H4             'ad instance
        IsCheckedOut = &H8               'image,adinstance
        IncludeInBar = &H20              'category - include in category bar
        HasImage = &H40                  'news item has image
        IsGSTExempt = &H80               'user - gst exempt
        IsWebEnabled = &H100               'image - web enabled
        IsSpooled = &H200                  'folder - is spooled
        IsSpoolerActive = &H400           'folder = spooler is active
        IsProdnFolder = &H800             'folder - is a production folder
        IsEmailTestMode = &H1000
        IsPDFHint = &H2000                 'ad - sets pdf hint for tweets, featured ads
        IsVisibleInWizard = &H4000        'set to true if the edition is visible in the wizard
    End Enum

    ''' <summary> Defines global system constants which are used to define SQL field lengths
    ''' In particular, nullValue is used to denote the NULL value of the identity value of the primary ke</summary>
    ''' <includesource>yes</includesource>
    Public Enum SysConstants
        nullValue = &H80000000              'smallest 32 bit negative number - use this for "not specified" or "dont care"
        charLength = 255                    'SQL character length for most text fields
        textCharLength = 8000               'SQL character length for text fields
    End Enum

    Public Enum EngineModes
        Server
        Client
        Local
    End Enum


    ''' <summary> Skins</summary>
    Public Enum Skins
        <Description("AT Skin")> ATSkin = 1
        <Description("Midnight")> midnight = 2
        <Description("Myriad")> myriad = 3
    End Enum



    ''' <summary> Country codes. Source = http://ftp.ics.uci.edu/pub/websoft/wwwstat/country-codes.txt</summary>
    Public Enum countrycodes
        <Description("Andorra")> AD
        <Description("United Arab Emirates")> AE
        <Description("Afghanistan")> AF
        <Description("Antigua and Barbuda")> AG
        <Description("Anguilla")> AI
        <Description("Albania")> AL
        <Description("Armenia")> AM
        <Description("Netherlands Antilles")> AN
        <Description("Angola")> AO
        <Description("Antarctica")> AQ
        <Description("Argentina")> AR
        <Description("American Samoa")> ASA
        <Description("Austria")> AT
        <Description("Australia")> AU
        <Description("Aruba")> AW
        <Description("Aland Islands")> AX
        <Description("Azerbaijan")> AZ
        <Description("Bosnia and Herzegovina")> BA
        <Description("Barbados")> BB
        <Description("Bangladesh")> BD
        <Description("Belgium")> BE
        <Description("Burkina Faso")> BF
        <Description("Bulgaria")> BG
        <Description("Bahrain")> BH
        <Description("Burundi")> BI
        <Description("Benin")> BJ
        <Description("Bermuda")> BM
        <Description("Brunei Darussalam")> BN
        <Description("Bolivia")> BO
        <Description("Brazil")> BR
        <Description("Bahamas")> BS
        <Description("Bhutan")> BT
        <Description("Bouvet Island")> BV
        <Description("Botswana")> BW
        <Description("Belarus")> BY
        <Description("Belize")> BZ
        <Description("Canada")> CA
        <Description("Cocos (Keeling) Islands")> CC
        <Description("Democratic Republic of the Congo")> CD
        <Description("Central African Republic")> CF
        <Description("Congo")> CG
        <Description("Switzerland")> CH
        <Description("Cote D'Ivoire (Ivory Coast)")> CI
        <Description("Cook Islands")> CK
        <Description("Chile")> CL
        <Description("Cameroon")> CM
        <Description("China")> CN
        <Description("Colombia")> CO
        <Description("Costa Rica")> CR
        <Description("Serbia and Montenegro")> CS
        <Description("Cuba")> CU
        <Description("Cape Verde")> CV
        <Description("Christmas Island")> CX
        <Description("Cyprus")> CY
        <Description("Czech Republic")> CZ
        <Description("Germany")> DE
        <Description("Djibouti")> DJ
        <Description("Denmark")> DK
        <Description("Dominica")> DM
        <Description("Dominican Republic")> DOM
        <Description("Algeria")> DZ
        <Description("Ecuador")> EC
        <Description("Estonia")> EE
        <Description("Egypt")> EG
        <Description("Western Sahara")> EH
        <Description("Eritrea")> ER
        <Description("Spain")> ES
        <Description("Ethiopia")> ET
        <Description("Finland")> FI
        <Description("Fiji")> FJ
        <Description("Falkland Islands (Malvinas)")> FK
        <Description("Federated States of Micronesia")> FM
        <Description("Faroe Islands")> FO
        <Description("France")> FR
        <Description("France, Metropolitan")> FX
        <Description("Gabon")> GA
        <Description("Great Britain (UK)")> GB
        <Description("Grenada")> GD
        <Description("Georgia")> GE
        <Description("French Guiana")> GF
        <Description("Ghana")> GH
        <Description("Gibraltar")> GI
        <Description("Greenland")> GL
        <Description("Gambia")> GM
        <Description("Guinea")> GN
        <Description("Guadeloupe")> GP
        <Description("Equatorial Guinea")> GQ
        <Description("Greece")> GR
        <Description("South Georgia and Sandwich Islands")> GS
        <Description("Guatemala")> GT
        <Description("Guam")> GU
        <Description("Guinea-Bissau")> GW
        <Description("Guyana")> GY
        <Description("Hong Kong")> HK
        <Description("Heard Island and McDonald Islands")> HM
        <Description("Honduras")> HN
        <Description("Croatia (Hrvatska)")> HR
        <Description("Haiti")> HT
        <Description("Hungary")> HU
        <Description("Indonesia")> ID
        <Description("Ireland")> IE
        <Description("Israel")> IL
        <Description("India")> IND
        <Description("British Indian Ocean Territory")> IO
        <Description("Iraq")> IQ
        <Description("Iran")> IR
        <Description("Iceland")> ISL
        <Description("Italy")> IT
        <Description("Jamaica")> JM
        <Description("Jordan")> JO
        <Description("Japan")> JP
        <Description("Kenya")> KE
        <Description("Kyrgyzstan")> KG
        <Description("Cambodia")> KH
        <Description("Kiribati")> KI
        <Description("Comoros")> KM
        <Description("Saint Kitts and Nevis")> KN
        <Description("Korea (North)")> KP
        <Description("Korea (South)")> KR
        <Description("Kuwait")> KW
        <Description("Cayman Islands")> KY
        <Description("Kazakhstan")> KZ
        <Description("Laos")> LA
        <Description("Lebanon")> LB
        <Description("Saint Lucia")> LC
        <Description("Liechtenstein")> LI
        <Description("Sri Lanka")> LK
        <Description("Liberia")> LR
        <Description("Lesotho")> LS
        <Description("Lithuania")> LT
        <Description("Luxembourg")> LU
        <Description("Latvia")> LV
        <Description("Libya")> LY
        <Description("Morocco")> MA
        <Description("Monaco")> MC
        <Description("Moldova")> MD
        <Description("Madagascar")> MG
        <Description("Marshall Islands")> MH
        <Description("Macedonia")> MK
        <Description("Mali")> ML
        <Description("Myanmar")> MM
        <Description("Mongolia")> MN
        <Description("Macao")> MO
        <Description("Northern Mariana Islands")> MP
        <Description("Martinique")> MQ
        <Description("Mauritania")> MR
        <Description("Montserrat")> MS
        <Description("Malta")> MT
        <Description("Mauritius")> MU
        <Description("Maldives")> MV
        <Description("Malawi")> MW
        <Description("Mexico")> MX
        <Description("Malaysia")> MYA
        <Description("Mozambique")> MZ
        <Description("Namibia")> NA
        <Description("New Caledonia")> NC
        <Description("Niger")> NE
        <Description("Norfolk Island")> NF
        <Description("Nigeria")> NG
        <Description("Nicaragua")> NI
        <Description("Netherlands")> NL
        <Description("Norway")> NO
        <Description("Nepal")> NP
        <Description("Nauru")> NR
        <Description("Niue")> NU
        <Description("New Zealand (Aotearoa)")> NZ
        <Description("Oman")> OM
        <Description("Panama")> PA
        <Description("Peru")> PE
        <Description("French Polynesia")> PF
        <Description("Papua New Guinea")> PG
        <Description("Philippines")> PH
        <Description("Pakistan")> PK
        <Description("Poland")> PL
        <Description("Saint Pierre and Miquelon")> PM
        <Description("Pitcairn")> PN
        <Description("Puerto Rico")> PR
        <Description("Palestinian Territory")> PS
        <Description("Portugal")> PT
        <Description("Palau")> PW
        <Description("Paraguay")> PY
        <Description("Qatar")> QA
        <Description("Reunion")> RE
        <Description("Romania")> RO
        <Description("Russian Federation")> RU
        <Description("Rwanda")> RW
        <Description("Saudi Arabia")> SA
        <Description("Solomon Islands")> SB
        <Description("Seychelles")> SC
        <Description("Sudan")> SD
        <Description("Sweden")> SE
        <Description("Singapore")> SG
        <Description("Saint Helena")> SH
        <Description("Slovenia")> SI
        <Description("Svalbard and Jan Mayen")> SJ
        <Description("Slovakia")> SK
        <Description("Sierra Leone")> SL
        <Description("San Marino")> SM
        <Description("Senegal")> SN
        <Description("Somalia")> SO
        <Description("Suriname")> SR
        <Description("Sao Tome and Principe")> ST
        <Description("USSR (former)")> SU
        <Description("El Salvador")> SV
        <Description("Syria")> SY
        <Description("Swaziland")> SZ
        <Description("Turks and Caicos Islands")> TC
        <Description("Chad")> TD
        <Description("French Southern Territories")> TF
        <Description("Togo")> TG
        <Description("Thailand")> TH
        <Description("Tajikistan")> TJ
        <Description("Tokelau")> TK
        <Description("Timor-Leste")> TL
        <Description("Turkmenistan")> TM
        <Description("Tunisia")> TN
        <Description("Tonga")> TON
        <Description("East Timor")> TP
        <Description("Turkey")> TR
        <Description("Trinidad and Tobago")> TT
        <Description("Tuvalu")> TV
        <Description("Taiwan")> TW
        <Description("Tanzania")> TZ
        <Description("Ukraine")> UA
        <Description("Uganda")> UG
        <Description("United Kingdom")> UK
        <Description("United States")> US
        <Description("Uruguay")> UY
        <Description("Uzbekistan")> UZ
        <Description("Vatican City State (Holy See)")> VA
        <Description("Saint Vincent and the Grenadines")> VC
        <Description("Venezuela")> VE
        <Description("Virgin Islands (British)")> VG
        <Description("Virgin Islands (US)")> VI
        <Description("Viet Nam")> VN
        <Description("Vanuatu")> VU
        <Description("Wallis and Futuna")> WF
        <Description("Samoa")> WS
        <Description("Yemen")> YE
        <Description("Mayotte")> YT
        <Description("South Africa")> ZA
        <Description("Zambia")> ZM
        <Description("Zaire (former)")> ZR
        <Description("Zimbabwe")> ZW
    End Enum

    ''' <summary>
    ''' Provides a numeric enumerator for all the classes in  In many cases, these values qualify the objectID parameter, to determine which 
    ''' parent table is appropriate. See Datafields and Contacts for an example of this usage.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum ObjectTypes
        Unspecified
        Slot
        System
        Technote
        Category
        Classification
        SpecGroup
        SpecDefinition
        Publication
        Edition
        Product
        Folder
        Usr
        Ad
        AdInstance
        Image
        NewsItem
        CategoryBrowse
        ClassificationBrowse
        AdBrowse
        ProdnStatus

    End Enum


    Private _connectionString As String
    Private _ObjectStatus As ATSystem.ObjectState
    Private _ID As Integer
    Private _name As String
    Private _createTime As Date
    Private _modifyTime As Date
    Private _PhysicalApplicationPath As String
    Private _InternalURL As String
    Private _ExternalURL As String
    Private _EditionCloseDays As Integer
    Private _LRImageHeight As Integer
    Private _THBImageHeight As Integer
    Private _frontPicType As RotatorAd.Types
    Private _backPicType As RotatorAd.Types
    Private _status As ATSystem.StatusBits
    Private _slotTimeout As Integer
    Private _slotPurge As Integer
    Private _CurrentAdSequence As Integer
    Private _CurrentMonth As Integer
    Private _FrontPicCaption As String
    Private _BackPicCaption As String
    Private _FrontSequence As Integer
    Private _BackSequence As Integer
    Private _CoverSequence As Integer
    Private _testEmailAddr As String
    '
    ' twitter resources
    '
    Private _twitConsumerKey As String
    Private _twitConsumerKeySecret As String
    Private _twitOAuthToken As String
    Private _twitOAuthTokenSecret As String
    Private _twitUserName As String
    '
    ' production file locations
    '
    Private _SourceImageOriginalFolder As String
    Private _SourceImageWorkingFolder As String
    Private _proformaImageFolder As String
    Private _displayAdFolder As String
    Private _classadFolder As String
    Private _prodnPDFFolder As String
    Private _classadTemplate As String
    Private _classadLineHeight As Integer
    Private _classadPicHeight As Integer
    '
    ' rate tables and loadings
    '
    Private _RateSpreadsheet As String
    Private _DisplaySheet As String
    Private _ClassifiedSheet As String
    Private _LatestListingLoading As Integer
    Private _LatestListingKillTime As DateTime
    '
    ' display page dimensions
    '
    Private _displayColumnCount As Integer
    Private _displayColumnWidth As Integer
    Private _displayColumnHeight As Integer
    Private _displayGutterWidth As Integer
    '
    ' SMTP EMail params
    '
    Private _SMTPHost As String
    Private _SMTPPort As Integer
    Private _SMTPUser As String
    Private _SMTPPassword As String
    Private _BCCEmailAddr As String
    '
    ' embedded objects and collections
    '

    Private _usrs As Usrs
    Private _folders As Folders
    Private _prodnFolders As Folders
    Private _slots As Slots
    Private _categories As Categories
    Private _classifications As Classifications
    Private _Publications As Publications
    Private _rotatorAds As RotatorAds
    Private _technotes As Technotes
    Private _newsItems As NewsItems
    Private _newsItemStatus As NewsItem.ProdnState
    Private _technoteStatus As Technote.State
    Private _rotatorCategory As RotatorAd.Categories
    Private _publicationType As Publication.Types
    Private _FirstWebPublication As Publication
    Private _firstFolderID As Integer

    Private _slotStatus As Slot.LoginStates
    '
    ' engine parameters
    '
    Private _engine As Engine
    Private _engineAddress As Integer
    Private _enginePort As Integer
    Private _engineName As String
    Private _jobTimeout As Integer
    Private _engineMode As EngineModes
    Private _refEngineService As ObjRef
    Private _TCPChannel As Channels.Tcp.TcpChannel


    ''' <summary>
    ''' Instantiates the object. When instantiated, the ID property will be set to ATSystem.SysConstants.nullValue and the Status word will be zero.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _ID = ATSystem.SysConstants.nullValue
        _firstFolderID = ATSystem.SysConstants.nullValue
        _status = 0
    End Sub

    ''' <summary>
    ''' Looks up the DNS to get the host name that this object is exectuing on.
    ''' </summary>
    ''' <value>First entry in the DNS that desribes this host.</value>
    Public ReadOnly Property HostIP() As String
        '
        ' returns the machine IP address as a string
        '
        Get
            Dim hostNm As String = Dns.GetHostName()
            Dim host As IPHostEntry = Dns.GetHostEntry(hostNm)
            Dim firstAddress As IPAddress = host.AddressList(0)

            Return firstAddress.ToString
        End Get
    End Property

    ''' <summary>
    ''' Returns the local name of the computer this object is exectuing on.
    ''' </summary>
    ''' <value>My.Computer.Name</value>
    Public ReadOnly Property MyName() As String
        Get
            '
            ' returns my machine name as a string
            '
            Return My.Computer.Name
        End Get
    End Property


    ''' <summary>
    ''' Returns the integer build value from the in-memory system object which identifies the build number.
    ''' </summary>
    Public ReadOnly Property SWBuild() As Integer
        Get
            Return _swbuild
        End Get
    End Property

    ''' <summary>
    ''' Returns the build number converted to a string and divided by 1000, eg V2.233
    ''' </summary>
    Public ReadOnly Property SWVersion() As String
        Get
            Return "V" & (_swbuild / 1000).ToString
        End Get
    End Property

    ''' <summary>
    ''' Returns the buld date and time as a string from the in-memory system object.
    ''' </summary>
    Public ReadOnly Property BuildDate() As String
        Get
            Return _buildDate
        End Get
    End Property

    ''' <summary>
    ''' Returns a human readable string whcih summaries the build information.
    ''' </summary>
    Public ReadOnly Property BuildInfo() As String
        Get
            Return "System Software Build " & _swbuild & ", Built " & _buildDate
        End Get
    End Property

    ''' <summary>Object ID, which is the identity value of the database Primary Key, represented as a 32 bit integer</summary>
    ''' <value>Object ID as an integer</value>
    Public ReadOnly Property ID() As Integer
        Get
            Return _ID
        End Get
    End Property

    ''' <summary>Object ID, which is the identity value of the database Primary Key, represented as an 8 character hex string</summary>
    ''' <value>Object ID as an 8 character hex string</value>
    Public ReadOnly Property hexID() As String
        Get
            Return _ID.ToString("X8")
        End Get
    End Property



    ''' <summary>Object Name. When this property is set, the name is truncated to 255
    ''' characters and sanitized to remove invalid characters. See
    ''' CommonRoutines.Sanitize. If scope-specific uniqueness of Name is required on
    ''' input, test the proposed name first by calling IsNameUnique</summary>
    ''' <returns>Object Name</returns>
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>Timestamp, read from dbTime, that the database object was created</summary>
    ''' <value>Long timestamp, in the current culture</value>
    Public ReadOnly Property CreateTime() As Date
        Get
            Return _createTime
        End Get
    End Property

    ''' <summary>Timestamp, read from dbTime, that the database object was last modified</summary>
    ''' <value>Long timestamp, in the current culture</value>
    Public ReadOnly Property ModifyTime() As Date
        Get
            Return _modifyTime
        End Get
    End Property

    ''' <summary>
    ''' The name of the mail server SMTP host. This is used by Timer Services and Billing Agent to forward order-processed emails.
    ''' </summary>
    ''' <value>Email server name eg mail.com</value>
    Public Property SMTPHost() As String
        Get
            Return _SMTPHost
        End Get
        Set(ByVal value As String)
            _SMTPHost = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' the SMTP port number, used in conjunction with the SMTPHost property for identifying which mail server to use for sending in emails
    ''' </summary>
    ''' <value>Normally set to Port 25.</value>
    Public Property SMTPPort() As Integer
        Get
            Return _SMTPPort
        End Get
        Set(ByVal value As Integer)
            _SMTPPort = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' The login name used by Timer Services and Billing Agent to login to the SMTP host for sending order processed emails.
    ''' </summary>
    Public Property SMTPUser() As String
        Get
            Return _SMTPUser
        End Get
        Set(ByVal value As String)
            _SMTPUser = value
            _ObjectStatus = ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' the login password used by Timer Services an Billing Agent to login to the SMTP host for sending order pocessed emails.
    ''' </summary>
    Public Property SMTPPassword() As String
        Get
            Return _SMTPPassword
        End Get
        Set(ByVal value As String)
            _SMTPPassword = value
            _ObjectStatus = ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' BCC email address for drop copies of all outgoing emails.
    ''' </summary>
    Public Property BCCEmailAddr() As String
        Get
            Return _BCCEmailAddr
        End Get
        Set(ByVal value As String)
            _BCCEmailAddr = value
            _ObjectStatus = ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Returns th ID of the first folder
    ''' </summary>
    Public ReadOnly Property FirstFolderID() As Integer
        Get
            If _firstFolderID = ATSystem.SysConstants.nullValue Then
                Dim folders As New Folders
                _firstFolderID = folders.GetFirstFolderID
            End If
            Return _firstFolderID
        End Get
    End Property

    ''' <summary>
    ''' Rates spreadsheet.
    ''' </summary>
    Public Property TestEmailAddr() As String
        Get
            Return _testEmailAddr
        End Get

        Set(ByVal value As String)
            _testEmailAddr = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' twitter oauth keys
    ''' </summary>
    Public Property TwitConsumerKey() As String
        Get
            Return _twitConsumerKey
        End Get

        Set(ByVal value As String)
            _twitConsumerKey = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property
    ''' <summary>
    ''' twitter oauth keys
    ''' </summary>
    Public Property TwitConsumerKeySecret() As String
        Get
            Return _twitConsumerKeySecret
        End Get

        Set(ByVal value As String)
            _twitConsumerKeySecret = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' twitter oauth keys
    ''' </summary>
    Public Property TwitOAuthToken() As String
        Get
            Return _twitOAuthToken
        End Get

        Set(ByVal value As String)
            _twitOAuthToken = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' twitter oauth keys
    ''' </summary>
    Public Property TwitOAuthTokenSecret() As String
        Get
            Return _twitOAuthTokenSecret
        End Get

        Set(ByVal value As String)
            _twitOAuthTokenSecret = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' twitter user name
    ''' </summary>
    Public Property TwitUserName() As String
        Get
            Return _twitUserName
        End Get

        Set(ByVal value As String)
            _twitUserName = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property





    ''' <summary>
    ''' Rates spreadsheet.
    ''' </summary>
    Public Property RateSpreadsheet() As String
        Get
            Return _RateSpreadsheet
        End Get

        Set(ByVal value As String)
            _RateSpreadsheet = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Rates spreadsheet.
    ''' </summary>
    Public Property DisplaySheet() As String
        Get
            Return _DisplaySheet
        End Get

        Set(ByVal value As String)
            _DisplaySheet = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Rates spreadsheet.
    ''' </summary>
    Public Property ClassifiedSheet() As String
        Get
            Return _ClassifiedSheet
        End Get

        Set(ByVal value As String)
            _ClassifiedSheet = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Classad line height in mm * 1000.
    ''' </summary>
    Public Property ClassadLineHeight() As Integer
        Get
            Return _classadLineHeight
        End Get

        Set(ByVal value As Integer)
            _classadLineHeight = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>
    ''' Classad line height in mm * 1000.
    ''' </summary>
    Public Property ClassadPicHeight() As Integer
        Get
            Return _classadPicHeight
        End Get

        Set(ByVal value As Integer)
            _classadPicHeight = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Latest Listing Loading in cents.
    ''' </summary>
    Public Property LatestListingLoading() As Integer
        Get
            Return _LatestListingLoading
        End Get

        Set(ByVal value As Integer)
            _LatestListingLoading = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Latest Listing Kill Time.
    ''' </summary>
    Public Property LatestListingKillTime() As DateTime
        Get
            Return _LatestListingkilltime
        End Get

        Set(ByVal value As DateTime)
            _LatestListingkilltime = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Display column count
    ''' </summary>
    Public Property DisplayColumnCount() As Integer
        Get
            Return _displayColumnCount
        End Get

        Set(ByVal value As Integer)
            _displayColumnCount = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Display column width in mm * 1000.
    ''' </summary>
    Public Property DisplayColumnWidth() As Integer
        Get
            Return _displayColumnWidth
        End Get

        Set(ByVal value As Integer)
            _displayColumnWidth = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Display column height in mm * 1000.
    ''' </summary>
    Public Property DisplayColumnHeight() As Integer
        Get
            Return _displayColumnHeight
        End Get

        Set(ByVal value As Integer)
            _displayColumnHeight = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Display gutter width in mm * 1000.
    ''' </summary>
    Public Property DisplayGutterWidth() As Integer
        Get
            Return _displayGutterWidth
        End Get

        Set(ByVal value As Integer)
            _displayGutterWidth = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public ReadOnly Property ClassificationCount() As Integer
        Get
            Dim Classifications As New Classifications
            Classifications.ConnectionString = _connectionString
            Return Classifications.GetClassificationCount(ATSystem.ObjectTypes.System, _ID)
        End Get
    End Property

    Public ReadOnly Property ProductCount() As Integer
        Get
            Dim Products As New Products
            Products.ConnectionString = _connectionString
            Return Products.GetProductCount(ATSystem.ObjectTypes.System, _ID)
        End Get
    End Property

    Public ReadOnly Property EditionCount() As Integer
        Get
            Dim Editions As New Editions
            Editions.ConnectionString = _connectionString
            Return Editions.GetEditionCount(ATSystem.ObjectTypes.Publication, _ID)
        End Get
    End Property


    Public ReadOnly Property AdCount() As Integer
        Get
            Dim Ads As New Ads
            Ads.ConnectionString = _connectionString
            Return Ads.GetAdCount(ATSystem.ObjectTypes.System, _ID)
        End Get
    End Property


    ''' <summary>
    ''' Engine name
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EngineName() As String
        Get
            Return _engineName
        End Get

        Set(ByVal value As String)
            _engineName = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Engine address
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EngineAddress() As Integer
        Get
            Return _engineAddress
        End Get

        Set(ByVal value As Integer)
            _engineAddress = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property
    ''' <summary>
    ''' Engine port
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EnginePort() As Integer
        Get
            Return _enginePort
        End Get

        Set(ByVal value As Integer)
            _enginePort = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property
    ''' <summary>
    ''' Job Timeout
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property JobTimeout() As Integer
        Get
            Return _jobTimeout
        End Get

        Set(ByVal value As Integer)
            _jobTimeout = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property



    ''' <summary>
    ''' Nanme of stand-alone classad template.
    ''' </summary>
    Public Property ClassadTemplate() As String
        Get
            Return _classadTemplate
        End Get

        Set(ByVal value As String)
            _classadTemplate = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Absolute physical path to the shared folder where images are stored on upload.
    ''' </summary>
    Public Property SourceImageOriginalFolder() As String
        Get
            Return _SourceImageOriginalFolder
        End Get

        Set(ByVal value As String)
            _SourceImageOriginalFolder = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Absolute physical path to the shared folder where images are stored on upload.
    ''' </summary>
    Public Property SourceImageWorkingFolder() As String
        Get
            Return _SourceImageWorkingFolder
        End Get

        Set(ByVal value As String)
            _SourceImageWorkingFolder = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Front Iamge  Sequence - used to generate unique preview filenames</summary>
    Public Property FrontSequence() As Integer
        Get
            Return _FrontSequence
        End Get
        Set(ByVal value As Integer)
            _FrontSequence = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public ReadOnly Property FrontImageFilename() As String
        Get
            Dim extn As String = "-front.jpg"
            If _frontPicType = RotatorAd.Types.Flash Then extn = "-front.swf"

            Dim filename As String = hexID & "-" & _FrontSequence & extn
            Return IO.Path.Combine(_PhysicalApplicationPath, Constants.subsampledImagesSitePics & "/" & filename)
        End Get
    End Property


    Public ReadOnly Property FrontImageURL() As String
        Get
            Dim extn As String = "-front.jpg"
            If _frontPicType = RotatorAd.Types.Flash Then extn = "-front.swf"

            Dim filename As String = hexID & "-" & _FrontSequence & extn
            Return GetApplicationPath() & Constants.subsampledImagesSitePics & "/" & filename
        End Get
    End Property

    ''' <summary>Back Iamge  Sequence - used to generate unique preview filenames</summary>
    Public Property BackSequence() As Integer
        Get
            Return _BackSequence
        End Get
        Set(ByVal value As Integer)
            _BackSequence = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    Public ReadOnly Property BackImageFilename() As String
        Get
            Dim extn As String = "-back.jpg"
            If _backPicType = RotatorAd.Types.Flash Then extn = "-back.swf"

            Dim filename As String = hexID & "-" & _BackSequence & extn
            Return IO.Path.Combine(_PhysicalApplicationPath, Constants.subsampledImagesSitePics & "/" & filename)
        End Get
    End Property


    Public ReadOnly Property BackImageURL() As String
        Get
            Dim extn As String = "-back.jpg"
            If _backPicType = RotatorAd.Types.Flash Then extn = "-back.swf"

            Dim filename As String = hexID & "-" & _BackSequence & extn
            Return GetApplicationPath() & Constants.subsampledImagesSitePics & "/" & filename
        End Get
    End Property

    ''' <summary>Cover Iamge  Sequence - used to generate unique preview filenames</summary>
    Public Property CoverSequence() As Integer
        Get
            Return _CoverSequence
        End Get
        Set(ByVal value As Integer)
            _CoverSequence = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    Public ReadOnly Property CoverImageFilename() As String
        Get
            Dim filename As String = hexID & "-" & _CoverSequence & "-cover.jpg"
            Return IO.Path.Combine(_PhysicalApplicationPath, Constants.subsampledImagesSitePics & "/" & filename)
        End Get
    End Property


    Public ReadOnly Property CoverImageURL() As String
        Get
            Dim filename As String = hexID & "-" & _CoverSequence & "-cover.jpg"
            Return GetApplicationPath() & Constants.subsampledImagesSitePics & "/" & filename
        End Get
    End Property

    ''' <summary>
    ''' Caption of front cover pic.
    ''' </summary>
    Public Property FrontPicCaption() As String
        Get
            Return _FrontPicCaption
        End Get

        Set(ByVal value As String)
            _FrontPicCaption = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' 
    ''' <summary>
    ''' Caption of back cover pic.
    ''' </summary>
    Public Property BackPicCaption() As String
        Get
            Return _BackPicCaption
        End Get

        Set(ByVal value As String)
            _BackPicCaption = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Absoulute physical path where the web server is currently executing. This is used by all objects that need to read or write files directly into subfolders of the web application, using Windows io services.
    ''' </summary>
    Public Property PhysicalApplicationPath() As String
        Get
            Return _PhysicalApplicationPath
        End Get
        Set(ByVal value As String)
            _PhysicalApplicationPath = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Web site URL in internal format, eg //192.168.... which is used by Data Wizard to provide a private inhouse access to teh web app.
    ''' </summary>
    Public Property InternalURL() As String
        Get
            Return _InternalURL
        End Get
        Set(ByVal value As String)
            _InternalURL = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Web site URL in external format, eg http://www.printfuse.com which is used by the order email generator to provide an absolute path to the web app for image retrieval from received emails.
    ''' </summary>
    Public Property ExternalURL() As String
        Get
            Return _ExternalURL
        End Get
        Set(ByVal value As String)
            _ExternalURL = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Absolute path which specifies the shared folder location where proxy images are held for inclusion into InDesign templates.
    ''' </summary>
    Public Property ProformaImageFolder() As String
        Get
            Return _proformaImageFolder
        End Get

        Set(ByVal value As String)
            _proformaImageFolder = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>
    ''' Absolute path which specifies the shared folder location where display ads are kept.
    ''' </summary>
    Public Property DisplayAdFolder() As String
        Get
            Return _displayAdFolder
        End Get

        Set(ByVal value As String)
            _displayAdFolder = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Absolute path which specifies the shared folder location where display ads are kept.
    ''' </summary>
    Public Property ClassAdFolder() As String
        Get
            Return _classadFolder
        End Get

        Set(ByVal value As String)
            _classadFolder = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Absolute path to the prodn pdf for display ads.
    ''' </summary>
    Public Property ProdnPDFFolder() As String
        Get
            Return _prodnPDFFolder
        End Get

        Set(ByVal value As String)
            _prodnPDFFolder = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>
    ''' Current ad number as it increases sequentially thru range 0000...9999. Reset at the beginning
    ''' of each month by timer services.
    ''' </summary>
    Public Property CurrentAdSequence() As Integer
        Get
            Return _CurrentAdSequence
        End Get
        Set(ByVal value As Integer)
            _CurrentAdSequence = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Defines whether user is gst exempt.
    ''' </summary>
    Public Property IsEmailTestMode() As Boolean
        Get
            Return Convert.ToBoolean(ATSystem.StatusBits.IsEmailTestMode And _status)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _status = _status Or ATSystem.StatusBits.IsEmailTestMode
            Else
                _status = _status And Not ATSystem.StatusBits.IsEmailTestMode
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property




    ''' <summary>
    ''' Number of days before edition close for timer services to generate emails.
    ''' </summary>
    Public Property EditionCloseDays() As Integer
        Get
            Return _EditionCloseDays
        End Get
        Set(ByVal value As Integer)
            _EditionCloseDays = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Subsampled width in pixels of images that are painted up in the image preview page on the browser.
    ''' </summary>
    Public Property LRImageHeight() As Integer
        Get
            Return _LRImageHeight
        End Get
        Set(ByVal value As Integer)
            _LRImageHeight = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Image width for list-mode display of multiple images or templates at the browser.
    ''' </summary>
    Public Property THBImageHeight() As Integer
        Get
            Return _THBImageHeight
        End Get
        Set(ByVal value As Integer)
            _THBImageHeight = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' front pic image type.
    ''' </summary>
    Public Property FrontPicType() As RotatorAd.Types
        Get
            Return _frontPicType
        End Get
        Set(ByVal value As RotatorAd.Types)
            _frontPicType = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' back pic image type.
    ''' </summary>
    Public Property BackPicType() As RotatorAd.Types
        Get
            Return _backPicType
        End Get
        Set(ByVal value As RotatorAd.Types)
            _backPicType = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Time in seconds that a slot can be idle (ie no user activity) before Timer Services times it out.
    ''' </summary>
    Public Property SlotTimeout() As Integer
        Get
            Return _slotTimeout
        End Get
        Set(ByVal value As Integer)
            _slotTimeout = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Time in days that logged-out or timed-out slots will be retained on the database before being purged by Timer Services.
    ''' </summary>
    Public Property SlotPurge() As Integer
        Get
            Return _slotPurge
        End Get
        Set(ByVal value As Integer)
            _slotPurge = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>Embedded collection of slot objects. Holds all slots within the
    ''' system.</summary>
    ''' <value>Collection of slots</value>
    ''' 
    Public ReadOnly Property Slots() As Slots
        Get
            Return Slots(Slot.LoginStates.Unspecified)
        End Get
    End Property

    ''' <summary>Embedded collection of slot objects. Holds all slots within the
    ''' system whose status matches the supplied parameter</summary>
    ''' <param name="status">slot status</param>
    ''' <returns>Returns collection of slots subordinate to the system whose status is as requested</returns>
    Public ReadOnly Property Slots(ByVal Status As Slot.LoginStates) As Slots
        '
        ' if there is no slots collection, or if there is, but the status has changed,
        ' then get a new collection
        '
        Get
            If (_slots Is Nothing) Or _
            (Status <> _slotStatus) Then
                _slotStatus = Slot.LoginStates.Active             'save the new status
                _slots = New Slots()                      'get a new collection
                _slots.ConnectionString = _connectionString
                _slots.retrieveSet(Status)
            End If
            Return _slots
        End Get
    End Property

    ''' <summary>Embedded collection of category objects.</summary>
    ''' <returns>Returns collection of categories subordinate to the system whose status is as requested</returns>
    Public ReadOnly Property Categories() As Categories
        '
        ' if there is no Categories collection, then get a new collection
        '
        Get
            If (_categories Is Nothing) Then
                _categories = New Categories()                      'get a new collection
                _categories.ConnectionString = _connectionString
                _categories.retrieveSet(_ID)
            End If
            Return _categories
        End Get
    End Property


    ''' <summary>Embedded collection of classification objects.</summary>
    ''' <returns>Returns the entire set of classifications across all categories, sorted by classification
    ''' sort order</returns>
    Public ReadOnly Property Classifications() As Classifications
        '
        ' if there is no Categories collection, then get a new collection
        '
        Get
            If (_classifications Is Nothing) Then
                _classifications = New Classifications()                      'get a new collection
                _classifications.ConnectionString = _connectionString
                _classifications.Retrieve()
            End If
            Return _classifications
        End Get
    End Property

    ''' <summary>Embedded collection of category objects.</summary>
    ''' <returns>Returns collection of categories subordinate to the system whose status is as requested</returns>
    Public ReadOnly Property Publications() As Publications
        Get
            Return Publications(Publication.Types.Unspecified)
        End Get
    End Property

    ''' <summary>Embedded collection of category objects.</summary>
    ''' <returns>Returns collection of categories subordinate to the system whose status is as requested</returns>
    '''<param name="Type">publication type</param>
    Public ReadOnly Property Publications(ByVal Type As Publication.Types) As Publications
        '
        ' if there is no Publications collection, or the type has changed then get a new collection
        '
        Get
            If (_Publications Is Nothing) Or (_publicationType <> Type) Then
                _publicationType = Type                                   'update type
                _Publications = New Publications()                      'get a new collection
                _Publications.ConnectionString = _connectionString
                _Publications.retrieveSet(_ID, Type)
            End If
            Return _Publications
        End Get
    End Property

    ''' <summary>Embedded collection of category objects.</summary>
    ''' <returns>Returns collection of categories subordinate to the system whose status is as requested</returns>
    Public ReadOnly Property RotatorAds(ByVal Category As RotatorAd.Categories) As RotatorAds
        '
        ' if there is no RotatorAds collection or if the category changes, then get a new collection
        '
        Get
            If (_rotatorAds Is Nothing) Or (_rotatorCategory <> Category) Then
                _rotatorAds = New RotatorAds()                      'get a new collection
                _rotatorCategory = Category
                _rotatorAds.ConnectionString = _connectionString
                _rotatorAds.retrieveSet(Category)
            End If
            Return _rotatorAds
        End Get
    End Property

    ''' <summary>Embedded collection of newsitem objects.</summary>
    ''' <returns>Returns collection of newsitems subordinate to the system whose status is as requested</returns>
    Public ReadOnly Property NewsItems(ByVal ProdnStatus As NewsItem.ProdnState) As NewsItems
        '
        ' if there is no technotes collection or if the status changes, then get a new collection
        '
        Get
            If (_newsItems Is Nothing) Or (_newsItemStatus <> ProdnStatus) Then
                _newsItems = New NewsItems()                      'get a new collection
                _newsItemStatus = ProdnStatus
                _newsItems.ConnectionString = _connectionString
                _newsItems.retrieveSet(ProdnStatus)
            End If
            Return _newsItems
        End Get
    End Property

    ''' <summary>Embedded collection of technote objects.</summary>
    ''' <returns>Returns collection of technotes subordinate to the system whose status is as requested</returns>
    Public ReadOnly Property Technotes(ByVal Status As Technote.State) As Technotes
        '
        ' if there is no technotes collection or if the status changes, then get a new collection
        '
        Get
            If (_technotes Is Nothing) Or (_technoteStatus <> Status) Then
                _technotes = New Technotes()                      'get a new collection
                _technoteStatus = Status
                _technotes.ConnectionString = _connectionString
                _technotes.retrieveSet(_ID, Status)
            End If
            Return _technotes
        End Get
    End Property

   

    ''' <summary>Embedded collection of folder objects.</summary>
    Public ReadOnly Property Folders() As Folders
        '
        ' if there is no Folders collection, then get a new collection
        '
        Get
            If (_folders Is Nothing) Then
                _folders = New Folders()                      'get a new collection
                _folders.ConnectionString = _connectionString
                _folders.RetrieveSet(_ID)
            End If
            Return _folders
        End Get
    End Property

    ''' <summary>Embedded collection of folder objects which are defined as prodn folders.</summary>
    Public ReadOnly Property ProdnFolders() As Folders
        '
        ' if there is no Folders collection, then get a new collection
        '
        Get
            If (_prodnFolders Is Nothing) Then
                _prodnFolders = New Folders()                      'get a new collection
                _prodnFolders.ConnectionString = _connectionString
                _prodnFolders.RetrieveProdnSet(_ID)
            End If
            Return _prodnFolders
        End Get
    End Property


    ''' <summary>Embedded collection of user objects.</summary>
    ''' <returns>Returns collection of categories subordinate to the system whose status is as requested</returns>
    Public ReadOnly Property Usrs() As Usrs
        '
        ' if there is no Usrs collection, then get a new collection
        '
        Get
            If (_usrs Is Nothing) Then
                _usrs = New Usrs()                      'get a new collection
                _usrs.ConnectionString = _connectionString
                _usrs.retrieveSet(_ID)
            End If
            Return _usrs
        End Get
    End Property
  
    Friend Property ObjectStatus() As ATSystem.ObjectState
        Get
            Return _ObjectStatus
        End Get
        Set(ByVal value As ATSystem.ObjectState)
            _ObjectStatus = value
        End Set
    End Property

    ''' <summary>This is a set of 32 boolean status flags, implemented as a 32 bit
    ''' integer. The bit values are defined in ATSystem.StatusBits. The status property can
    ''' be used to set/test/clear bits within the word. For operations on individual
    ''' bits, it may be more convenient to use the specifically named boolean properties
    ''' of this object.</summary>
    ''' <returns>32 status bits, defined in ATSystem.StatusBits, and implemented as an
    ''' integer</returns>
    ''' <example>
    ''' <code>dim currentStatus as ATSystem.StatusBits = obj.Status
    ''' currentStatus = CurrentStatus or (ATSystem.StatusBits.isStressed or ATSystem.StatusBits.hasCustomStressedIcon) 'sets these bits
    ''' currentStatus = CurrsntStatus and not ATSystem.StatusBits.isOpenForEdit 'clear this bit
    ''' obj.Status = currentStatus 'modify object in memory
    ''' obj.Update 'and update to db</code></example>
    Public Property Status() As ATSystem.StatusBits
        Get
            Return _status
        End Get

        Set(ByVal value As ATSystem.StatusBits)
            _status = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Boolean value which defines if the object is marked for deletion. The
    ''' object is neither removed from the parent collection nor deleted from the
    ''' database when the property is set to true. Rather, the object will be physically
    ''' deleted from the db and removed from the collection on the next call to
    ''' object.Update or objectCollection.Update</summary>
    ''' <returns>true if object is marked for deletion, false otherwise</returns>
    Public Property Deleted() As Boolean
        Get
            If _ObjectStatus = ATSystem.ObjectState.Deleted Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            _ObjectStatus = ATSystem.ObjectState.Modified
            If value = True Then _ObjectStatus = ATSystem.ObjectState.Deleted
        End Set
    End Property

    '''
    ''' <summary>Allows the caller to specify the connection string, if known. This
    ''' value is, in turn, plugged into all child objects within retrieved collections.
    ''' This property does not have to be set; if it is not set, then the class will
    ''' make a call on the Configuration Manager to find the connection string. Setting
    ''' the connection string in advance via this property will however lead to faster
    ''' performance.</summary>
    ''' <value>Connection string, as read by the Configuration Manager from the
    ''' app.config or web.config file</value>
    ''' 
    Public Property ConnectionString() As String
        Get
            Return _connectionString
        End Get
        Set(ByVal value As String)
            _connectionString = value
        End Set
    End Property

    Public Function GetFirstWebPublication() As Publication
        '
        ' returns the first publication which has a type Web.
        '
        If _FirstWebPublication Is Nothing Then
            Dim mypubs As New Publications
            mypubs.retrieveSet(_ID, Publication.Types.WebSite)
            If mypubs.Count > 0 Then
                _FirstWebPublication = mypubs(0)
            End If
        End If
        Return _FirstWebPublication
    End Function

    ''' <summary>
    ''' Returns a list of cat-cls objects suitable for binding in a dd
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCatClassList() As List(Of clsItem)
        Dim clsList As New List(Of clsItem)

        Dim ci As clsItem
        ci = New clsItem("All Categories", ATSystem.ObjectTypes.System, ATSystem.SysConstants.nullValue)
        clsList.Add(ci)
        For Each cat As Category In Categories
            ci = New clsItem(cat.Name, ATSystem.ObjectTypes.Category, cat.ID)
            clsList.Add(ci)
            For Each cls As Classification In cat.Classifications
                ci = New clsItem("- " & cls.Name, ATSystem.ObjectTypes.Classification, cls.ID)
                clsList.Add(ci)
            Next
        Next
        Return clsList
    End Function


    ''' <summary>
    ''' Returns the current timestamp as read from the DBMS on the SQL Server machine. This provides a 
    ''' common system-wide time reference, which is preferable to calling NOW which returns the time on the local machine on which the call is made.
    ''' </summary>
    ''' <returns>Current time of the DBMS.</returns>
    ''' <remarks></remarks>
    Public Function GetdbTime() As Date
        '
        ' returns the current time from the DB
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT getdate()"
        Dim rtnval As Date = Nothing
        Try
            Cmd.Connection.Open()
            rtnval = CType(Cmd.ExecuteScalar(), Date)
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return rtnval
    End Function

    ''' <summary>
    ''' Returns the next ad number in the form YYYYMMNNNN where NNNN is derived from the current ad sequence
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNextAdNumber() As String
        _CurrentAdSequence += 1          'bump ad sequence
        doUpdate()                          'and save
        Return GetdbTime.ToString("yyyy.MM.") & _CurrentAdSequence.ToString("D4")
    End Function

    ''' <summary>
    ''' called by TS. looks at the current month and if it differs from todays, date, resets the ad sequence
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub TouchMonth()
        Dim nowMonth As Integer = GetdbTime.Month
        If nowMonth <> _CurrentMonth Then
            _CurrentMonth = nowMonth
            _CurrentAdSequence = 0
            _ObjectStatus = ATSystem.ObjectState.Modified
            Update()
        End If
    End Sub

    ''' <summary>
    ''' Returns the EngineServices object. Only one instantiation of engineservices is allowed, and this
    ''' is done by the engine (mode = Modes.server). Clients (mode = Modes.client) which call this method
    ''' are granted simultaneous to the same remote object that is running within the engine.
    ''' </summary>
    ''' <param name="mode">Calling mode, to denote client or server (engine)</param>
    ''' <returns>Engine Services proxy object</returns>
    Public Function MapEngine(ByVal mode As EngineModes) As Engine

        _engineMode = mode        'save mode

        If _engine Is Nothing Then

            Select Case mode
                Case EngineModes.Server
                    _engine = New Engine
                    '
                    ' set timeouts from me
                    '
                    _engine.JobTimeout = _jobTimeout
                    _TCPChannel = New Channels.Tcp.TcpChannel(_enginePort)
                    Channels.ChannelServices.RegisterChannel(_TCPChannel, False)

                    _refEngineService = RemotingServices.Marshal(_engine, "Engine")

                    '
                    ' If starting as a server, update engine to show IP and name
                    '
                    _engineAddress = _engine.HostIP
                    _engineName = _engine.HostName
                    doUpdate()

                Case EngineModes.Client
                    Dim objectURL As String = "tcp://" & CommonRoutines.IPInt2String(_engineAddress) & ":" & _enginePort.ToString & "/Engine"
                    _engine = CType(Activator.GetObject(GetType(Engine), objectURL), Engine)
            End Select
        End If
        Return _engine

    End Function

    ''' <summary>
    ''' Called by both client and engine when EngineServices are no longer required. Releases remoting resources
    ''' and unregisters the TCP channel used for communicating with the engine.
    ''' </summary>
    Public Sub UnMapServices()
        Select Case _engineMode
            Case EngineModes.Server
                Channels.ChannelServices.UnregisterChannel(_TCPChannel)
                RemotingServices.Unmarshal(_refEngineService)
                RemotingServices.Disconnect(_engine)
                _refEngineService = Nothing
                _engine = Nothing

            Case EngineModes.Client
                _engine = Nothing

        End Select

    End Sub


    Private Function getConnection() As SqlConnection
        If _connectionString Is Nothing Then _connectionString = ConfigurationManager.ConnectionStrings("ATConnectionString").ConnectionString
        Return New SqlConnection(_connectionString)
    End Function


    ''' <summary>Retrieves the system objects - there is only one.</summary>
    Public Sub Retrieve()
        '
        ' Retrieves the single system record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.System"
        doRetrieveR(Cmd)
    End Sub

    Private Sub doRetrieveR(ByVal Cmd As SqlCommand)
        '
        ' Retrieves records passed in the command object
        '
        Dim dr As SqlDataReader = Nothing
        Cmd.Connection.Open()
        Try
            dr = Cmd.ExecuteReader()
            While dr.Read
                DR2Object(dr)
            End While
        Finally
            If Not dr Is Nothing Then
                dr.Close()
                Cmd.Connection.Dispose()
            End If
        End Try
    End Sub

    ''' <summary>Called to update the object back into the database.
    ''' <para></para>
    ''' <para>If the object has been modified since retrieval, then it is updated into
    ''' the db.</para>
    ''' <para>If the object has been marked for deletion (obj.deleted=true) then it is
    ''' deleted from the database.</para>
    ''' <para>If the object has been newly instantiated, and update has not been
    ''' previously called, then a new record is written to the database, and the ID of
    ''' the newly created record is returned </para></summary>
    ''' <remarks>If a newly substantiated object is to be written to the database,
    ''' ensure that all properties, especially parentID references have been correctly
    ''' plugged prior to calling Update</remarks>
    ''' <returns>Object ID, which is the identity value of the record's Primary
    ''' Key</returns>
    Public Function Update() As Integer

        If _ID = ATSystem.SysConstants.nullValue Then _ObjectStatus = ATSystem.ObjectState.Initial

        Select Case _ObjectStatus
            Case ATSystem.ObjectState.Modified
                _ID = doUpdate()
            Case ATSystem.ObjectState.Initial
                '
                ' cannot insert new system
                '
            Case ATSystem.ObjectState.Deleted
                '
                ' can never delete the system
                '
        End Select

        Return _ID
    End Function



    Private Function doUpdate() As Integer


        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.CommandText = "UPDATE dbo.System SET " & _
        "modifyTime=getdate()," & _
        "Name=@Name," & _
        "Status=@Status," & _
        "SlotTimeout=@SlotTimeout," & _
        "SlotPurge=@SlotPurge," & _
        "DisplayColumnCount=@DisplayColumnCount," & _
        "DisplayColumnWidth=@DisplayColumnWidth," & _
        "DisplayColumnHeight=@DisplayColumnHeight," & _
        "DisplayGutterWidth=@DisplayGutterWidth," & _
        "ClassadLineHeight=@ClassadLineHeight," & _
        "ClassadPicHeight=@ClassadPicHeight," & _
        "ClassadTemplate=@ClassadTemplate," & _
        "TestEmailAddr=@TestEmailAddr," & _
        "TwitConsumerKey=@TwitConsumerKey," & _
        "TwitConsumerKeySecret=@TwitConsumerKeysecret," & _
        "TwitOAuthToken=@TwitOAuthToken," & _
        "TwitOAuthTokenSecret=@TwitOAuthTokenSecret," & _
        "TwitUserName=@TwitUserName," & _
        "RateSpreadsheet=@RateSpreadsheet," & _
        "DisplaySheet=@DisplaySheet," & _
        "ClassifiedSheet=@ClassifiedSheet," & _
        "LatestListingLoading=@LatestListingLoading," & _
        "LatestListingKillTime=@LatestListingKillTime," & _
        "EngineName=@EngineName," & _
        "EngineAddress=@EngineAddress," & _
        "EnginePort=@EnginePort," & _
        "JobTimeout=@JobTimeout," & _
        "SMTPHost=@SMTPHost," & _
        "SMTPPort=@SMTPPort," & _
        "SMTPUser=@SMTPUser," & _
        "SMTPPassword=@SMTPPassword," & _
        "BCCEmailAddr=@BCCEmailAddr," & _
        "FrontPicType=@FrontPicType," & _
        "BackPicType=@BackPicType," & _
        "THBImageHeight=@THBImageHeight," & _
        "LRImageHeight=@LRImageHeight," & _
        "EditionCloseDays=@EditionCloseDays," & _
        "FrontSequence=@FrontSequence," & _
        "BackSequence=@BackSequence," & _
        "CoverSequence=@CoverSequence," & _
        "CurrentMonth=@CurrentMonth," & _
        "CurrentAdSequence=@CurrentAdSequence," & _
        "PhysicalApplicationPath=@PhysicalApplicationPath," & _
        "InternalURL=@InternalURL," & _
        "ExternalURL=@ExternalURL," & _
        "FrontPicCaption=@FrontPicCaption," & _
        "BackPicCaption=@BackPicCaption," & _
        "ProformaImageFolder=@ProformaImageFolder," & _
        "DisplayAdFolder=@DisplayAdFolder," & _
        "ClassAdFolder=@ClassAdFolder," & _
        "ProdnPDFFolder=@ProdnPDFFolder," & _
        "SourceImageOriginalFolder=@SourceImageOriginalFolder," & _
        "SourceImageWorkingFolder=@SourceImageWorkingFolder " & _
        "WHERE ID=@ID"

        addParams(Cmd)

        Try
            Cmd.Connection.Open()
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return _ID

    End Function


    Private Sub addParams(ByVal Cmd As SqlCommand)
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID
        Cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = _name
        Cmd.Parameters.Add("@DisplayColumnCount", SqlDbType.Int).Value = _displayColumnCount
        Cmd.Parameters.Add("@DisplayColumnWidth", SqlDbType.Int).Value = _displayColumnWidth
        Cmd.Parameters.Add("@DisplayColumnHeight", SqlDbType.Int).Value = _displayColumnHeight
        Cmd.Parameters.Add("@DisplayGutterWidth", SqlDbType.Int).Value = _displayGutterWidth
        Cmd.Parameters.Add("@TestEmailAddr", SqlDbType.VarChar).Value = _testEmailAddr
        Cmd.Parameters.Add("@TwitConsumerKey", SqlDbType.VarChar).Value = _twitConsumerKey
        Cmd.Parameters.Add("@TwitConsumerKeySecret", SqlDbType.VarChar).Value = _twitConsumerKeySecret
        Cmd.Parameters.Add("@TwitOAuthToken", SqlDbType.VarChar).Value = _twitOAuthToken
        Cmd.Parameters.Add("@TwitOAuthTokenSecret", SqlDbType.VarChar).Value = _twitOAuthTokenSecret
        Cmd.Parameters.Add("@TwitUserName", SqlDbType.VarChar).Value = _twitUserName
        Cmd.Parameters.Add("@RateSpreadsheet", SqlDbType.VarChar).Value = _RateSpreadsheet
        Cmd.Parameters.Add("@DisplaySheet", SqlDbType.VarChar).Value = _DisplaySheet
        Cmd.Parameters.Add("@ClassadTemplate", SqlDbType.VarChar).Value = _classadTemplate
        Cmd.Parameters.Add("@ClassifiedSheet", SqlDbType.VarChar).Value = _ClassifiedSheet
        Cmd.Parameters.Add("@ClassadLineHeight", SqlDbType.Int).Value = _classadLineHeight
        Cmd.Parameters.Add("@ClassadPicHeight", SqlDbType.Int).Value = _classadPicHeight
        Cmd.Parameters.Add("@LatestListingLoading", SqlDbType.Int).Value = _LatestListingLoading
        Cmd.Parameters.Add("@LatestListingKillTime", SqlDbType.DateTime).Value = _LatestListingkilltime
        Cmd.Parameters.Add("@EngineName", SqlDbType.VarChar).Value = _engineName
        Cmd.Parameters.Add("@EngineAddress", SqlDbType.Int).Value = _engineAddress
        Cmd.Parameters.Add("@EnginePort", SqlDbType.Int).Value = _enginePort
        Cmd.Parameters.Add("@JobTimeout", SqlDbType.Int).Value = _jobTimeout
        Cmd.Parameters.Add("@SMTPHost", SqlDbType.VarChar).Value = _SMTPHost
        Cmd.Parameters.Add("@SMTPPort", SqlDbType.Int).Value = _SMTPPort
        Cmd.Parameters.Add("@SMTPUser", SqlDbType.VarChar).Value = _SMTPUser
        Cmd.Parameters.Add("@SMTPPassword", SqlDbType.VarChar).Value = _SMTPPassword
        Cmd.Parameters.Add("@BCCEmailAddr", SqlDbType.VarChar).Value = _BCCEmailAddr
        Cmd.Parameters.Add("@FrontPicCaption", SqlDbType.VarChar).Value = _FrontPicCaption
        Cmd.Parameters.Add("@BackPicCaption", SqlDbType.VarChar).Value = _BackPicCaption
        Cmd.Parameters.Add("@Status", SqlDbType.Int).Value = _status
        Cmd.Parameters.Add("@SlotTimeout", SqlDbType.Int).Value = _slotTimeout
        Cmd.Parameters.Add("@SlotPurge", SqlDbType.Int).Value = _slotPurge
        Cmd.Parameters.Add("@CurrentMonth", SqlDbType.Int).Value = _CurrentMonth
        Cmd.Parameters.Add("@CurrentAdSequence", SqlDbType.Int).Value = _CurrentAdSequence
        Cmd.Parameters.Add("@THBImageHeight", SqlDbType.Int).Value = _THBImageHeight
        Cmd.Parameters.Add("@FrontPicType", SqlDbType.Int).Value = _frontPicType
        Cmd.Parameters.Add("@BackPicType", SqlDbType.Int).Value = _backPicType
        Cmd.Parameters.Add("@LRImageHeight", SqlDbType.Int).Value = _LRImageHeight
        Cmd.Parameters.Add("@EditionCloseDays", SqlDbType.Int).Value = _EditionCloseDays
        Cmd.Parameters.Add("@PhysicalApplicationPath", SqlDbType.VarChar).Value = _PhysicalApplicationPath
        Cmd.Parameters.Add("@InternalURL", SqlDbType.VarChar).Value = _InternalURL
        Cmd.Parameters.Add("@ExternalURL", SqlDbType.VarChar).Value = _ExternalURL
        Cmd.Parameters.Add("@ProformaImageFolder", SqlDbType.VarChar).Value = _proformaImageFolder
        Cmd.Parameters.Add("@DisplayAdFolder", SqlDbType.VarChar).Value = _displayAdFolder
        Cmd.Parameters.Add("@ClassAdFolder", SqlDbType.VarChar).Value = _classadFolder
        Cmd.Parameters.Add("@ProdnPDFFolder", SqlDbType.VarChar).Value = _prodnPDFFolder
        Cmd.Parameters.Add("@SourceImageOriginalFolder", SqlDbType.VarChar).Value = _SourceImageOriginalFolder
        Cmd.Parameters.Add("@SourceImageworkingFolder", SqlDbType.VarChar).Value = _SourceImageWorkingFolder
        Cmd.Parameters.Add("@FrontSequence", SqlDbType.Int).Value = _FrontSequence
        Cmd.Parameters.Add("@BackSequence", SqlDbType.Int).Value = BackSequence
        Cmd.Parameters.Add("@CoverSequence", SqlDbType.Int).Value = _CoverSequence

    End Sub


    Friend Sub DR2Object(ByVal dr As IDataRecord)


        _ObjectStatus = ATSystem.ObjectState.Original
        _ID = Convert.ToInt32(dr("ID"))
        _name = Convert.ToString(dr("Name"))
        _engineName = Convert.ToString(dr("EngineName"))
        _engineAddress = Convert.ToInt32(dr("EngineAddress"))
        _enginePort = Convert.ToInt32(dr("EnginePort"))
        _jobTimeout = Convert.ToInt32(dr("JobTimeout"))
        _displayColumnCount = Convert.ToInt32(dr("DisplayColumnCount"))
        _displayColumnWidth = Convert.ToInt32(dr("DisplayColumnWidth"))
        _displayColumnHeight = Convert.ToInt32(dr("DisplayColumnHeight"))
        _displayGutterWidth = Convert.ToInt32(dr("DisplayGutterWidth"))
        _classadTemplate = Convert.ToString(dr("ClassadTemplate"))
        _classadLineHeight = Convert.ToInt32(dr("ClassadLineHeight"))
        _classadPicHeight = Convert.ToInt32(dr("ClassadPicHeight"))
        _RateSpreadsheet = Convert.ToString(dr("RateSpreadsheet"))
        _DisplaySheet = Convert.ToString(dr("DisplaySheet"))
        _ClassifiedSheet = Convert.ToString(dr("ClassifiedSheet"))
        _testEmailAddr = Convert.ToString(dr("TestEmailAddr"))
        _twitConsumerKey = Convert.ToString(dr("TwitConsumerKey"))
        _twitConsumerKeySecret = Convert.ToString(dr("TwitConsumerKeySecret"))
        _twitOAuthToken = Convert.ToString(dr("TwitOAuthToken"))
        _twitOAuthTokenSecret = Convert.ToString(dr("TwitOAuthTokenSecret"))
        _twitUserName = Convert.ToString(dr("TwitUserName"))
        _LatestListingLoading = Convert.ToInt32(dr("LatestListingLoading"))
        _latestlistingkilltime = Convert.ToDateTime(dr("LatestListingKillTime"))
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))
        _SMTPHost = Convert.ToString(dr("SMTPHost"))
        _SMTPPort = Convert.ToInt32(dr("SMTPPort"))
        _SMTPUser = Convert.ToString(dr("SMTPUser"))
        _SMTPPassword = Convert.ToString(dr("SMTPPassword"))
        _BCCEmailAddr = Convert.ToString(dr("BCCEmailAddr"))
        _FrontSequence = Convert.ToInt32(dr("FrontSequence"))
        _BackSequence = Convert.ToInt32(dr("BackSequence"))
        _CoverSequence = Convert.ToInt32(dr("CoverSequence"))
        _FrontPicCaption = Convert.ToString(dr("FrontPicCaption"))
        _BackPicCaption = Convert.ToString(dr("BackPicCaption"))
        _status = CType(dr("Status"), ATSystem.StatusBits)
        _slotTimeout = Convert.ToInt32(dr("SlotTimeout"))
        _slotPurge = Convert.ToInt32(dr("SlotPurge"))
        _proformaImageFolder = Convert.ToString(dr("ProformaImageFolder"))
        _displayAdFolder = Convert.ToString(dr("DisplayAdFolder"))
        _classadFolder = Convert.ToString(dr("ClassAdFolder"))
        _prodnPDFFolder = Convert.ToString(dr("ProdnPDFFolder"))
        _SourceImageOriginalFolder = Convert.ToString(dr("SourceImageOriginalFolder"))
        _SourceImageWorkingFolder = Convert.ToString(dr("SourceImageWorkingFolder"))
        _PhysicalApplicationPath = Convert.ToString(dr("PhysicalApplicationPath"))
        _InternalURL = Convert.ToString(dr("InternalURL"))
        _ExternalURL = Convert.ToString(dr("ExternalURL"))
        _CurrentMonth = Convert.ToInt32(dr("CurrentMonth"))
        _CurrentAdSequence = Convert.ToInt32(dr("CurrentAdSequence"))
        _THBImageHeight = Convert.ToInt32(dr("THBImageHeight"))
        _LRImageHeight = Convert.ToInt32(dr("LRImageHeight"))
        _EditionCloseDays = Convert.ToInt32(dr("EditionCloseDays"))
        _frontPicType = CType(dr("FrontPicType"), RotatorAd.Types)
        _backPicType = CType(dr("BackPicType"), RotatorAd.Types)

    End Sub


    Public Function qt(ByVal value As String) As String
        Return Chr(&H22) & value & Chr(&H22)
    End Function


End Class

Public Class clsItem
    Private _Name As String
    Private _objectType As ATSystem.ObjectTypes
    Private _objectID As Integer

    Friend Sub New(ByVal name As String, ByVal objectType As ATSystem.ObjectTypes, ByVal objectID As Integer)
        _Name = name
        _objectType = objectType
        _objectID = objectID
    End Sub

    Public ReadOnly Property Value() As String
        Get
            Return CommonRoutines.Int2ShortHex(_objectType) & CommonRoutines.Int2Hex(_objectID)
        End Get
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return _Name
        End Get
    End Property

End Class


