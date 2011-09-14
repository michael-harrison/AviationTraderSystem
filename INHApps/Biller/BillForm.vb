Option Explicit On
Option Strict On
Imports ATLib
Imports System
Imports System.IO


'***************************************************************************************
'*
'* Biller
'*
'* AUDIT TRAIL
'* 
'* V1.000   05-SEP-2010 BA  Original
'*
'* Biller - ads records to billling file
'*
'*
'*
'*
'***************************************************************************************
Public Class Form1

    Private sys As ATSystem
    Private billFileName As String


    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        sys = New ATSystem
        sys.Retrieve()

        Me.Text = Me.Text & " " & sys.SWVersion & " connected to " & sys.Name


    End Sub

    Private Sub runProcess()
    
        Try
            '
            ' Open the billing file, for overwrite
            '

            msgBox.Text = "Run started"
            Application.DoEvents()
            Me.Show()


            Dim fs As New FileStream(billfilename, FileMode.Create)
            Dim sw As New StreamWriter(fs)
            sw.WriteLine(formatBillHdr)

            Dim ads As New Ads
            ads.RetrieveBillSet()
            For Each Ad As Ad In ads
                billAd(sw, Ad)
            Next
            '
            ' close the billing file
            '
            sw.Close()
            msgBox.Text = "Run completed, " & ads.Count & " ads processed"



        Catch ex As Exception
            msgBox.Text = "Run aborted because " & ex.Message
        End Try


    End Sub

    Private Sub billAd(ByVal sw As streamwriter, ByVal Ad As Ad)
        '
        ' note - this is called of the adinstance loop. If the customer books the same ad to have multiple instances
        ' then multiple bill records will be produced
        '
        If detailCheck.Checked Then
            For Each AI As AdInstance In Ad.Instances
                sw.WriteLine(formatInstance(AI))
            Next
        End If
        sw.WriteLine(formatAd(Ad))

    End Sub

    Private Function formatBillHdr() As String
        Dim valueArray() As String = { _
     "ACCT Alias", _
   "Email", _
   "Name", _
   "Company", _
   "Ad Number", _
   "Category", _
   "Classification", _
   "Product", _
   "Product Type", _
   "Edition", _
   "Price", _
   "Price Adj", _
   "Instance Price", _
   "Subtotal", _
   "Latest Listing Loading", _
   "GST", _
   "Total Price" _
 }
        Return String.Join(vbTab, valueArray)
    End Function


    Private Function formatInstance(ByVal AI As AdInstance) As String
        Dim valueArray() As String = { _
     AI.Ad.Usr.AcctAlias, _
   AI.Ad.Usr.EmailAddr, _
   AI.Ad.Usr.FullName, _
   AI.Ad.Usr.Company, _
   AI.AdNumber, _
   AI.Ad.CategoryName, _
   AI.Ad.ClassificationName, _
   AI.Product.Name, _
   AI.ProductType.ToString, _
   AI.Edition.Name, _
   CommonRoutines.Integer2Dollars(AI.Price), _
   CommonRoutines.Integer2Dollars(AI.PriceAdjust), _
  CommonRoutines.Integer2Dollars(AI.Price + AI.PriceAdjust), _
  "", _
  "", _
  "", _
  "" _
}
        Return String.Join(vbTab, valueArray)
    End Function

    Private Function formatAd(ByVal Ad As Ad) As String

        Dim subtotal As Integer = 0
        For Each AI As AdInstance In Ad.Instances
            subtotal += (AI.Price + AI.PriceAdjust)
        Next
        Dim latestListingLoading As Integer = 0
        If Ad.IsLatestListing Then latestListingLoading = sys.LatestListingLoading
        Dim GST As Integer = Convert.ToInt32((subtotal + latestListingLoading) * 0.1)
        Dim totalPrice As Integer = subtotal + latestListingLoading + GST

        Dim valueArray() As String = { _
    Ad.Usr.AcctAlias, _
   Ad.Usr.EmailAddr, _
   Ad.Usr.FullName, _
   Ad.Usr.Company, _
        Ad.Adnumber, _
           Ad.CategoryName, _
           Ad.ClassificationName, _
           "Ad total", _
           "", _
           "", _
           "", _
           "", _
           "", _
           CommonRoutines.Integer2Dollars(subtotal), _
           CommonRoutines.Integer2Dollars(latestListingLoading), _
          CommonRoutines.Integer2Dollars(GST), _
          CommonRoutines.Integer2Dollars(totalPrice) _
}
        Return String.Join(vbTab, valueArray)
    End Function

    Private Function getAdSubtotal(ByVal Ad As Ad) As Integer
        Dim subtotal As Integer = 0
        For Each AI As AdInstance In Ad.Instances
            subtotal += AI.Price
        Next
    End Function

    Private Function getLatestListing(ByVal Ad As Ad) As Integer
        If Ad.IsLatestListing Then
                       Return sys.LatestListingLoading
        Else
            Return 0
        End If
    End Function

    Private Sub resetStatus()
        '
        ' resets all billing flags
        '
        Dim ads As New Ads
        Dim adCount As Integer = ads.ClearBillSet
        msgBox.Text = "Done, " & adCount & " ads reset"

    End Sub
    Private Sub btnChooseFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseFile.Click
        '
        ' nominates an output file to write to
        '
        BillFileDialog.Filter = "Text files|*.TXT"

        If BillFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            billFileName = BillFileDialog.FileName
            Dim fileinfo As IO.FileInfo = My.Computer.FileSystem.GetFileInfo(billFileName)
            FilenameBox.Text = fileinfo.Name
        End If
    End Sub


    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBill.Click
        runProcess()
    End Sub


    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        resetStatus()
    End Sub
End Class
