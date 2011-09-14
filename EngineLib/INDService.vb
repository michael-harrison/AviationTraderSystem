Option Strict On
Option Explicit On
Imports System.ComponentModel
Imports System.Configuration
Imports System
Imports InDesign


'***************************************************************************************
'*
'* INDService
'*
'*
'*
'***************************************************************************************
Public Class INDService
    Private _INDApp As InDesign.Application
    Private _indDoc As InDesign.Document
    Private _watermarkFont As Font
    Private Const watermarkText As String = "Aviation Trader Proof"

    Public ReadOnly Property INDApp() As InDesign.Application
        Get
            Return _INDApp
        End Get
    End Property

    Public ReadOnly Property INDDoc() As InDesign.Document
        Get
            Return _indDoc
        End Get
     
    End Property

    Public Sub InitializeINDD()
        '
        ' can be called multiple times - check that its already initialised
        ' ID launch may not work if we are starting too quickly
        '
        If _INDApp Is Nothing Then
            _INDApp = CType(GetObject("", "InDesign.Application.CS4"), InDesign.Application)

            If Not _INDApp Is Nothing Then
                '
                ' First delete all load PDF export presets and then load PK2 presets
                '
                Dim p As PDFExportPreset
                For Each p In _INDApp.PDFExportPresets
                    p.Delete()
                Next
                '
                ' scan the fonts looking for Arial which is the watermark font
                '
                For Each font As Font In _INDApp.Fonts

                    If (font.FontFamily = "Arial") And (font.FontStyleName = "Bold") Then
                        _watermarkFont = font
                        Exit For
                    End If
                Next
                If _watermarkFont Is Nothing Then _watermarkFont = CType(_INDApp.Fonts(1), Font)

            End If
        End If
    End Sub

    Public Sub FinalizeINDD()
        If Not _INDApp Is Nothing Then INDApp.Quit(idSaveOptions.idNo)
    End Sub

    Private Declare Function ShowWindow Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nCmdShow As SHOW_WINDOW) As Boolean

    <Flags()> _
    Private Enum SHOW_WINDOW As Integer
        SW_HIDE = 0
        SW_SHOWNORMAL = 1
        SW_NORMAL = 1
        SW_SHOWMINIMIZED = 2
        SW_SHOWMAXIMIZED = 3
        SW_MAXIMIZE = 3
        SW_SHOWNOACTIVATE = 4
        SW_SHOW = 5
        SW_MINIMIZE = 6
        SW_SHOWMINNOACTIVE = 7
        SW_SHOWNA = 8
        SW_RESTORE = 9
        SW_SHOWDEFAULT = 10
        SW_FORCEMINIMIZE = 11
        SW_MAX = 11
    End Enum

    Public Sub minimizeInDesign()
        For Each p As Process In Process.GetProcessesByName("InDesign")
            ShowWindow(p.MainWindowHandle, SHOW_WINDOW.SW_MINIMIZE)
        Next
    End Sub

    Public Sub NewDoc(ByVal size As Double())
        '
        ' creates a new doc
        '
    
        Dim presets As InDesign.DocumentPresets = _INDApp.DocumentPresets
        Dim preset As InDesign.DocumentPreset = presets.Add()
        preset.ColumnCount = 1
        preset.ColumnGutter = 0
        preset.PageWidth = size(0) & "mm"
        preset.PageHeight = size(1) & "mm"
        preset.PagesPerDocument = 1
        preset.MasterTextFrame = False

        _indDoc = _INDApp.Documents.Add(True, preset)

        Dim viewPref As InDesign.ViewPreference = CType(_indDoc.ViewPreferences, InDesign.ViewPreference)
        viewPref.HorizontalMeasurementUnits = idMeasurementUnits.idMillimeters
        viewPref.VerticalMeasurementUnits = idMeasurementUnits.idMillimeters
        viewPref.RulerOrigin = InDesign.idRulerOrigin.idSpreadOrigin


    End Sub



    Public Sub OpenDoc(ByVal Filename As String)
        _indDoc = CType(_INDApp.Open(Filename), InDesign.Document)
        '
        ' set dimensions to mm
        '
        Dim viewPref As InDesign.ViewPreference = CType(INDDoc.ViewPreferences, InDesign.ViewPreference)
        viewPref.HorizontalMeasurementUnits = idMeasurementUnits.idMillimeters
        viewPref.VerticalMeasurementUnits = idMeasurementUnits.idMillimeters
        viewPref.RulerOrigin = InDesign.idRulerOrigin.idSpreadOrigin

    End Sub

    Public Sub AbortDoc()
        If Not _indDoc Is Nothing Then _indDoc.Close(idSaveOptions.idNo)
    End Sub

    Public Sub SaveDoc()
        If Not _indDoc Is Nothing Then _indDoc.Close(idSaveOptions.idYes)
    End Sub

    Public Function getDocSize() As Double()
        '
        ' returns the exact doc size in mm
        '
        Dim rtnval() As Double = {0, 0}
        If Not _indDoc Is Nothing Then
            rtnval(0) = Convert.ToDouble(_indDoc.DocumentPreferences.PageWidth)
            rtnval(1) = Convert.ToDouble(_indDoc.DocumentPreferences.PageHeight)
        End If
        Return rtnval

    End Function

    Public Sub PlacePDF(ByVal filename As String)
        '
  


    End Sub

    Public Function GetDisplaySize() As Double()
        Dim rtnval As Double() = {1, 1}
        Return rtnval
    End Function

    Public Function GetClassadSize() As Double()
        '
        ' after placement, adjusts the boxes
        '
         Dim page As InDesign.Page = CType(_indDoc.Pages(1), Page)
        Dim tf As TextFrame = CType(page.TextFrames.FirstItem, TextFrame)

        tf.Fit(idFitOptions.idFrameToContent)
        '
        ' resize page to text frame height (T,L,B,R)

        Dim textarr As System.Array = CType(tf.GeometricBounds, System.Array)
        Dim tftop As Double = Convert.ToDouble(textarr.GetValue(0))
        Dim tfleft As Double = Convert.ToDouble(textarr.GetValue(1))
        Dim tfbottom As Double = Convert.ToDouble(textarr.GetValue(2))
        Dim tfright As Double = Convert.ToDouble(textarr.GetValue(3))
        Dim tfheight As Double = tfbottom - tftop
        Dim tfwidth As Double = tfright - tfleft

        '
        ' reposition ad to top of page
        '
        _indDoc.DocumentPreferences.PageHeight = tfheight
        tf.GeometricBounds = textarr
        '
        ' determine classad text height in lines and return to caller
        '
        Dim adheight As Double = tfheight
        Dim rtnval(1) As Double
        rtnval(0) = tfwidth
        rtnval(1) = adheight

        Return rtnval

    End Function



    Public Sub ExportLoRes(ByVal Filename As String, ByVal width As Integer)
        '
        ' exports a loRes JPG image to the web site
        '
        '
        ' IND export wants resolution, System has phyisical pixel width 
        ' Set resolution 
        '
        Dim w As Double = Convert.ToDouble(_indDoc.DocumentPreferences.PageWidth) * 0.0393700787     'inches
        Dim resolution As Integer = Convert.ToInt32(width / w)
        '
        ' export jpg pics
        '
        Dim prefs As JPEGExportPreference = _INDApp.JPEGExportPreferences
        prefs.JPEGExportRange = InDesign.idExportRangeOrAllPages.idExportRange
        prefs.Resolution = resolution
        prefs.JPEGQuality = idJPEGOptionsQuality.idMaximum

        Dim INDPage As Page = CType(INDDoc.Pages(1), InDesign.Page)
        prefs.PageString = 1.ToString
        INDDoc.Export(InDesign.idExportFormat.idJPG, Filename, , , , True)


    End Sub

    Public Sub ExportPicBox(ByVal PicBox As InDesign.Rectangle, ByVal Filename As String, ByVal height As Integer)
        '
        ' exports a loRes JPG image of a pic box to the web site
        '
        '
        ' IND export wants resolution, System has image height 
        ' Set resolution 
        '
        Dim arr As System.Array = CType(PicBox.GeometricBounds, System.Array)
        Dim T As Double = Convert.ToDouble(arr.GetValue(0))
        Dim L As Double = Convert.ToDouble(arr.GetValue(1))
        Dim B As Double = Convert.ToDouble(arr.GetValue(2))
        Dim R As Double = Convert.ToDouble(arr.GetValue(3))

        Dim h As Double = Convert.ToDouble((B - T) * 0.0393700787)   'inches

        Dim rx As Double = height / h
        Dim resolution As Integer = Convert.ToInt32(height / h)
        '
        ' export jpg pic
        '
        Dim prefs As JPEGExportPreference = _INDApp.JPEGExportPreferences
        prefs.JPEGExportRange = InDesign.idExportRangeOrAllPages.idExportRange
        prefs.Resolution = resolution
        prefs.JPEGQuality = idJPEGOptionsQuality.idMaximum
        PicBox.Export(InDesign.idExportFormat.idJPG, Filename, , , , True)
    End Sub

    Public Sub ExportPicBox(ByVal PicBox As InDesign.Rectangle, ByVal Filename As String)
        '
        ' exports a loRes JPG image of a pic box to the supplied file at a resolution of 300 dpi
        '
       
        Dim resolution As Integer = 300
        '
        ' export jpg pic
        '
        Dim prefs As JPEGExportPreference = _INDApp.JPEGExportPreferences
        prefs.JPEGExportRange = InDesign.idExportRangeOrAllPages.idExportRange
        prefs.Resolution = resolution
        prefs.JPEGQuality = idJPEGOptionsQuality.idMaximum
        PicBox.Export(InDesign.idExportFormat.idJPG, Filename, , , , True)
    End Sub

    Public Sub ExportJPG(ByVal Filename As String)
        _INDApp.JPEGExportPreferences.JPEGExportRange = InDesign.idExportRangeOrAllPages.idExportRange
        _INDApp.JPEGExportPreferences.Resolution = 300
        _INDApp.JPEGExportPreferences.JPEGQuality = idJPEGOptionsQuality.idMaximum

        _INDApp.JPEGExportPreferences.PageString = 1.ToString
        ''           _INDDoc.Export(InDesign.idExportFormat.idJPG, filename, 1,    Template.ProdnFileTypes.JPG), , , , True)

    End Sub

    Public Sub ExportProofPDF(ByVal Filename As String)
        '
        ' exports a multipage low res pdf to the subsampled images in the web site
        ' add a security password to prevent removal of watermark
        '
        Dim prefs As PDFExportPreference = _INDApp.PDFExportPreferences
        prefs.PageRange = "all"
        prefs.ColorBitmapCompression = idBitmapCompression.idJPEG
        prefs.ColorBitmapSampling = idSampling.idSubsample
        prefs.UseSecurity = True
        prefs.ChangeSecurityPassword = "ATrader"
        prefs.BleedBottom = 5
        prefs.BleedTop = 5
        prefs.BleedInside = 5
        prefs.BleedOutside = 5

        INDDoc.Export(InDesign.idExportFormat.idPDFType, Filename, , , , True)

    End Sub

    Public Sub ExportProdnPDF(ByVal Filename As String)
        '
        ' exports a multipage hi res pdf to the subsampled images in the web site
        ' add a security password to prevent removal of watermark
        '

        Dim prefs As PDFExportPreference = _INDApp.PDFExportPreferences
        prefs.PageRange = "all"
        prefs.ColorBitmapCompression = idBitmapCompression.idJPEG
        prefs.ColorBitmapSampling = idSampling.idSubsample
        prefs.UseSecurity = False
        prefs.BleedBottom = 0
        prefs.BleedTop = 0
        prefs.BleedInside = 0
        prefs.BleedOutside = 0
        prefs.CropMarks = False
        prefs.ColorBars = False
        prefs.RegistrationMarks = False
        prefs.BleedMarks = False
        prefs.PageInformationMarks = False



        INDDoc.Export(InDesign.idExportFormat.idPDFType, Filename, , , , True)
    End Sub


    Public Sub ExportINDD(ByVal INDDoc As InDesign.Document)
        ''      INDDoc.Save(Doc.ProdnFileName(Sys.OrderFolder, 1, Template.ProdnFileTypes.INDD), , , True)         ' save the template as a new doc into the doc folder
    End Sub

    Public Sub ExportXML(ByVal INDDoc As InDesign.Document)
        ''       INDDoc.Export(InDesignServer.idExportFormat.idXML, Doc.ProdnFileName(Sys.OrderFolder, 1, Template.ProdnFileTypes.XML), , , , True)
    End Sub


    Public Sub AddWaterMark()
        '
        ' adds watermark to each page of the doc
        ' the size of the water mark in inches is a function of the document width and height
        '
        Dim w As Double = Convert.ToDouble(_indDoc.DocumentPreferences.PageWidth)
        Dim h As Double = Convert.ToDouble(_indDoc.DocumentPreferences.PageHeight)
        '
        ' position object in page centre, 90% of page width and height=9% of object width
        '
        Dim tfwidth As Double = w * 0.9
        Dim tfheight As Double = tfwidth * 0.15
        Dim tftop As Double = (h - tfheight) / 2
        Dim tfleft As Double = (w - tfwidth) / 2
        Dim tfright As Double = tfleft + tfwidth
        Dim tfbottom As Double = tftop + tfheight
        '
        ' 2.8 pts = 1 mm
        '
        Dim ptsize As Double = tfheight * 0.4 * 2.8
        Dim cornerradius As Double = tfwidth * 0.03
        Dim opacity As Double = 15
        Dim strokeweight As Double = 1.0
        '
        ' get a rotation matrix to line up the objectwith the page diagonal
        '
        Dim sine As Double = h / (Math.Sqrt(h * h + w * w))
        Dim rotateMatrix As InDesign.TransformationMatrix = _INDApp.TransformationMatrices.Add()
        rotateMatrix = rotateMatrix.RotateMatrix(, , sine)
        '
        ' add the object to each page
        '
        For Each p As Page In INDDoc.Pages

            Dim tf As TextFrame = p.TextFrames.Add()
            tf.Label = "watermark"          'so we can find it again to delete
            tf.StrokeColor = "Black"
            tf.StrokeWeight = strokeweight
            tf.CornerOption = idCornerOptions.idRoundedCorner
            tf.CornerRadius = cornerradius
            tf.TextFramePreferences.InsetSpacing = cornerradius * 1.01
            tf.TextFramePreferences.VerticalJustification = idVerticalJustification.idCenterAlign
            tf.TextFramePreferences.IgnoreWrap = True
            tf.TextWrapPreferences.TextWrapMode = idTextWrapModes.idNone

            tf.BringToFront()
            tf.TransparencySettings.BlendingSettings.Opacity = opacity

            tf.Contents = watermarkText

            Dim para As Paragraph = CType(tf.Paragraphs(1), Paragraph)
            para.AppliedFont = _watermarkFont
            para.PointSize = ptsize         'pt size is height of object
            para.Justification = idJustification.idCenterAlign
            '
            ' draw the object in centre of page and rotate to diagonal
            ' object co-ords are relative to ruler 0 pt.
            '
            Dim arr As System.Array = CType(p.Bounds, System.Array)
            Dim pagetop As Double = Convert.ToDouble(arr.GetValue(0))
            Dim pageleft As Double = Convert.ToDouble(arr.GetValue(1))

            Dim gb() As Double = {tftop + pagetop, tfleft + pageleft, tfbottom + pagetop, tfright + pageleft}
            '
            tf.GeometricBounds = gb

            tf.Transform(idCoordinateSpaces.idPasteboardCoordinates, idAnchorPoint.idCenterAnchor, rotateMatrix)
        Next

    End Sub

    Public Sub RemoveWaterMark()
        '
        ' removes the watermark from each page
        '
        For Each p As Page In INDDoc.Pages
            For Each tf As TextFrame In p.TextFrames
                If tf.Label = "watermark" Then
                    tf.Delete()
                    Exit For
                End If
            Next
        Next

    End Sub

End Class
