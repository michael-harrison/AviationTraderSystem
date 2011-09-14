Option Strict On
Option Explicit On
Imports atlib
Imports EngineLib



Public Class Form1


    Private Sub extract()

 
        Try

     
            Dim pdfassistant As New EngineLib.PDFAssistant

            TextBox1.Text = pdfassistant.ExtractTextFromPDF(TextBox2.Text)

        Catch ex As Exception
            TextBox1.Text = ex.Message
        End Try


    End Sub

    Private Sub getUsrs()
        Dim sys As New ATSystem
        sys.Retrieve()
        Dim usrs As New Usrs

        usrs.retrieveSet(sys.ID)

        ListBox1.Items.Clear()
        ListBox1.Items.Add(usrs.Count & " Users")

       

    End Sub

    Private Sub addusrs()

        Dim sys As New ATSystem
        sys.Retrieve()

        For i As Integer = 10001 To 100000
            Dim usr As New Usr

            usr.EditionVisibility = Edition.VisibleState.Active



            usr.EmailAddr = "fred" & i & "@abc.com"
            usr.Password = "aaaaaa"
            usr.FName = "Fred" & i
            usr.LName = "Bloggs" & i
            usr.Phone = "phone"
            usr.Addr1 = "3 smith street"
            usr.Suburb = "durban"
            usr.Postcode = "0000"
            usr.State = "NSW"
            usr.Discount = 0



            Dim EA As New EnumAssistant(New ATSystem.Skins, ATSystem.Skins.ATSkin, ATSystem.Skins.ATSkin)
            usr.Skin = EA(0).Description


            usr.WebSite = ""
            usr.Company = ""
            usr.ACN = ""
            usr.Addr2 = ""


            usr.Countrycode = "AU"
            usr.LoginLevel = ATLib.Usr.LoginLevels.Subscriber
            '
            ' remaining fields not required at this point
            '
            usr.AHPhone = ""
            usr.Fax = ""
            usr.AcctAlias = ""
            usr.Mobile = ""

            usr.SystemID = sys.ID
            usr.UAM = 0
            usr.IdentVisible = 0      'nothing visible to end users yet

            usr.Update()        'create new usr
        Next

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click


        ''  extract()
        getusrs()




    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        addusrs()
    End Sub
End Class
