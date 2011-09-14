Imports System.Xml
Imports System.IO
Imports System.Data
Imports ATLib
'***************************************************************************************
'*
'* Slideshow XMLContent
'*
'* AUDIT TRAIL
'* 
'* V1.000   28-OCT-2008  BA  Original
'*
'* This page is a call-back from the Flash object in the slideshow. It formats up an xml
'* file which defines the slideshow content.
'*
'* Querystring = 
'*
'***************************************************************************************

Partial Class SlideShowXMLContent
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.ContentType = "text/xml"
        Response.ContentEncoding = Encoding.UTF8


        Response.Write(SlideShowXML)        'deliver content to flash object in browser

    End Sub


    Private Function SlideShowXML() As String
        Dim sys As New ATSystem
        sys.Retrieve()
        '
        ' get a random object for the motion effect
        '
        Dim r As New Random(System.DateTime.Now.Millisecond)
        '
        ' see www.maani.us for parameter descriptions
        '
        Dim sw As New StringWriter
        Dim w As XmlWriter = New XmlTextWriter(sw)

        w.WriteStartElement("slideshow")

        w.WriteStartElement("control")
        w.WriteStartAttribute("bar_visible")
        w.WriteValue("on")
        w.WriteStartAttribute("volume_visible")
        w.WriteValue("true")
        w.WriteStartAttribute("height")
        w.WriteValue("30")
        w.WriteEndElement()
        '
        ' output the music section using a canned mp3 file
        '
             w.WriteStartElement("music")
        w.WriteAttributeString("url", "../audioTracks/Uptown Blues.mp3")
        w.WriteAttributeString("stream", "true")
        w.WriteAttributeString("loop", "true")
        w.WriteEndElement()
        '
        ' display top 
        '
        slidePic(w, sys.FrontImageURL, 0)
        '
        ' use the current classified edition to find all ads with a pic
        '
        Dim found As Boolean = False
        Dim editionID As Integer = ATSystem.SysConstants.nullValue
        For Each pub As Publication In sys.Publications(Publication.Types.ATClassified)
            For Each Edition As Edition In pub.Editions
                If Edition.Visibility = ATLib.Edition.VisibleState.Active Then
                    found = True
                    editionID = Edition.ID
                    Exit For
                End If
            Next
            If found Then Exit For
        Next
        '
        ' loop thru dump set and output main pic from ads with pics
        '
        If found Then
            Dim AdInstances As New AdInstances
            AdInstances.RetrieveDumpSet(editionID, Ad.ProdnState.Approved)
            For Each AI As AdInstance In AdInstances
                    Dim mainImage As Image = AI.Ad.MainImage
                If Not mainImage Is Nothing Then
                    slidePic(w, mainImage.LoResURL, r.Next(1, 6))
                End If
            Next
        End If
        '
        ' write tail 
        '

        slidePic(w, "../Graphics/slideshowtail.jpg", 0)

        w.WriteEndElement()

        Return sw.ToString

    End Function

    Private Sub slidePic(ByVal w As XmlWriter, ByVal picURL As String, ByVal motionIndex As Integer)

        w.WriteStartElement("slide")
        w.WriteAttributeString("purge", "true")

        w.WriteStartElement("image")
        w.WriteAttributeString("url", picURL)
        w.WriteAttributeString("duration", "3")
        w.WriteEndElement()

        w.WriteStartElement("transition")
        w.WriteAttributeString("type", "dissolve")
        w.WriteAttributeString("duration", "2")
        w.WriteEndElement()

        Motion(w, motionIndex)

        w.WriteEndElement()
    End Sub

    Private Sub Motion(ByVal w As XmlWriter, ByVal i As Integer)
        '
        ' outputs random motion effects
        '

        If i > 0 Then
            w.WriteStartElement("motion")
            w.WriteAttributeString("duration", "3")

            Select Case i

                Case 1      'left and up
                    w.WriteAttributeString("start_zoom", "120")
                    w.WriteAttributeString("end_zoom", "120")
                    w.WriteAttributeString("start_xOffset", "20")
                    w.WriteAttributeString("end_xOffset", "-20")
                    w.WriteAttributeString("start_yOffset", "20")
                    w.WriteAttributeString("end_yOffset", "-20")

                Case 2      'left and down
                    w.WriteAttributeString("start_zoom", "120")
                    w.WriteAttributeString("end_zoom", "120")
                    w.WriteAttributeString("start_xOffset", "20")
                    w.WriteAttributeString("end_xOffset", "-20")
                    w.WriteAttributeString("start_yOffset", "-20")
                    w.WriteAttributeString("end_yOffset", "20")

                Case 3      'right
                    w.WriteAttributeString("start_zoom", "120")
                    w.WriteAttributeString("end_zoom", "120")
                    w.WriteAttributeString("start_xOffset", "-20")
                    w.WriteAttributeString("end_xOffset", "20")
                    w.WriteAttributeString("start_yOffset", "0")
                    w.WriteAttributeString("end_yOffset", "0")


                Case 4      'left
                    w.WriteAttributeString("start_zoom", "120")
                    w.WriteAttributeString("end_zoom", "120")
                    w.WriteAttributeString("start_xOffset", "20")
                    w.WriteAttributeString("end_xOffset", "-20")
                    w.WriteAttributeString("start_yOffset", "0")
                    w.WriteAttributeString("end_yOffset", "0")

                Case 5      'right and down
                    w.WriteAttributeString("start_zoom", "120")
                    w.WriteAttributeString("end_zoom", "120")
                    w.WriteAttributeString("start_xOffset", "-20")
                    w.WriteAttributeString("end_xOffset", "20")
                    w.WriteAttributeString("start_yOffset", "-20")
                    w.WriteAttributeString("end_yOffset", "20")

                Case 6      'right and up
                    w.WriteAttributeString("start_zoom", "120")
                    w.WriteAttributeString("end_zoom", "120")
                    w.WriteAttributeString("start_xOffset", "-20")
                    w.WriteAttributeString("end_xOffset", "20")
                    w.WriteAttributeString("start_yOffset", "20")
                    w.WriteAttributeString("end_yOffset", "-20")

                Case Else       'no motion
                    w.WriteAttributeString("start_zoom", "100")
                    w.WriteAttributeString("end_zoom", "100")
                    w.WriteAttributeString("start_xOffset", "0")
                    w.WriteAttributeString("end_xOffset", "0")
                    w.WriteAttributeString("start_yOffset", "0")
                    w.WriteAttributeString("end_yOffset", "0")

            End Select

            w.WriteEndElement()
        End If
    End Sub


End Class
