Option Strict On
Option Explicit On

'***************************************************************************************
'*
'* Constants - This class holds all string literals
'*
'*
'*
'*
'*
'**************************************************************************************

''' <summary>
''' The constants class holds all string literals. These literals are used throughout the
''' web app and the inhouse apps for error messages, proforma image names, icon names etc.
''' </summary>
Public Class Constants
    Public Const ClassadPicFudgefactor As Double = 0.714

    '
    ' Timer services TOD preset
    '
    Public Const FourPM As Date = #4:00:00 PM#
    Public Const TextSeparator As Char = Chr(&H5C)      '\
    ''Public Const TextSeparator As Char = Chr(&HA)      'lf

    Public Const GuestName As String = "guest@aviationtrader.com.au"
    Public Const GuestPassword As String = "guest"
    Public Const ProdnEmail As String = "admin@aviationtrader.com.au"
    ''Public Const ProdnEmail As String = "brian@wavefront.com.au"
    '
    ' the following are proforma images
    '
    Public Const ImageNotFound As String = "Graphics/ImageNotFound.png"
    Public Const InvalidPreviewImage As String = "Graphics/InvalidPreview.png"
    Public Const WebBasicPreviewImage As String = "Graphics/WebBasicPreview.png"
    Public Const WebPremiumPreviewImage As String = "Graphics/WebPremiumPreview.png"
    Public Const WebFeaturedAdPreviewImage As String = "Graphics/WebFeaturedAdPreview.png"
    Public Const WebPDFPreviewImage As String = "Graphics/WebPDFPreview.png"
    Public Const WebPDFTextPreviewImage As String = "Graphics/WebPDFTextPreview.png"

    Public Const InvalidPreviewPDF As String = "Graphics/InvalidPreview.pdf"
    Public Const subsampledImagesTHB As String = "SubsampledImages/ThumbNail"
    Public Const subsampledImagesLores As String = "SubsampledImages/Lores"
    Public Const subsampledImagesAdInstance As String = "SubsampledImages/AdInstance"
    Public Const subsampledImagesNewsPics As String = "SubsampledImages/NewsPics"
    Public Const subsampledImagesSitePics As String = "SubsampledImages/SitePics"
    Public Const DefaultPDF As String = "DefaultPDF.gif"
    Public Const DefaultEPS As String = "DefaultEPS.gif"
    Public Const DefaultImage As String = "DefaultImage.gif"
    Public Const InvalidFinishedArtPDF As String = "InvalidFinishedArt.pdf"
    Public Const InvalidFinishedArt As String = "InvalidFinishedArt.jpg"
    Public Const InconsistentImageType As String = "Inconsistent Image Type"


    Public Const PDFIcon As String = "pdf.gif"
    '
    ' SQL rate table names
    '

    Public Const DisplayRates As String = "DisplayRates"
    Public Const ClassadRates As String = "ClassadRates"
    '
    ' following are error messages
    '
    Public Const Saved As String = "Changes saved"
    Public Const Tweeted As String = "Ad successfully tweeted"
    Public Const OAuthSuccess As String = "OAuth keys successfully uppdated"
    Public Const UnknownUsr As String = "User name not found"
    Public Const InvalidPassword As String = "Invalid password"
    Public Const EmailSent As String = "An email has been sent to your inbox"
    Public Const SubsEmailSent As String = "Thank You - your request has been forwarded to our subscription team"
    Public Const ProdnEmailSent As String = "Thank you - your request has been forwarded to Aviation Trader production staff."
    Public Const DuplicateUsr As String = "Sorry - this email address is already registered"
    Public Const UploadSelect As String = "Please select an image to upload"
    Public Const UnsupportedFileType As String = "The image file type is not recognised by the system."
    Public Const UserApproved As String = "Thank you - your ad is cleared for publication"
    Public Const UserUnapproved As String = "Thank you. (Did you leave notes for Production Operators?)"
    Public Const UserPrompt As String = "Reminder - if you disapprove, please leave notes in the Send Instructions tab."
    Public Const Approved As String = "This ad has already been approved or unapproved"
    Public Const RerunOK As String = "Ad re-run successful"
    Public Const NoOpenEditions As String = "There are no currently open editions for this publication"
    Public Const DisplayAdTooWide As String = "Warning -Ad is wider than page - truncating to page width"
    Public Const DisplayAdTooHigh As String = "Warning - ad height is greater than column height - truncating to column height"
    Public Const DisplayAdTooHighAndWide As String = "Warning - ad exceeds page size - truncating to page size"
    Public Const NoCategoryDelete As String = "Cannot delete because the category is not empty - move classifications to another category first"
    Public Const NoClassificationDelete As String = "Cannot delete because the classification is not empty - move ads to another classification first"
    Public Const NoFolderDelete As String = "Cannot delete because the folder is not empty - move ads to another folder first"
    Public Const NoSpecGroupDelete As String = "Cannot delete because the spec group is not empty - move spec defintions to another spec group first"
    Public Const NoPublicationDelete As String = "Cannot delete because the publication is not empty - delete products and editions first"
    Public Const NoEditionDelete As String = "Cannot delete because the edition is not empty - move ads to another edition first"
    Public Const NoProductDelete As String = "Cannot delete because the product is not empty - move ads to another product first"
    Public Const NoProductSend As String = "Cannot send because the product is not empty - move ads to another product first"
    Public Const NoEditionSend As String = "Cannot send because the edition is not empty - move ads to another edition first"
    Public Const NoTextFromPDF As String = "Sorry - could not extract text"
    Public Const NoAd As String = "Ad not found"
    Public Const NoSelfDelete As String = "Cannot delete myself!"
    Public Const NoUserDelete As String = "Cannot delete because user has ads - delete ads or send to another user first"
    Public Const NoSearchKey As String = "Please enter something to search for"
    Public Const NoSelectedProduct As String = "Please select at least one product before continuing to step 2"
    Public Const NoProdnSelectedProduct As String = "Warning - no products selected"
    Public Const OneSelectedProduct As String = "You can only select one product for this publication"
    Public Const NoSelectedEdition As String = "You must select at least one edition for this publication"

End Class

