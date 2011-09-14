Option Explicit On
Option Strict On
Imports ATLib


Public Class Form1



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sys As New ATSystem

        sys.Retrieve()
        Dim formName As String = Me.Text & " " & sys.SWVersion
        Me.Text = formName
        Dim countOK As Integer = 0
        Dim countBad As Integer = 0

        Try
            Dim images As New Images
            images.Retrieve()

            For Each Image As Image In images

                Dim fn As String = Image.WorkingSourceFileName
                If Not IO.File.Exists(fn) Then
                    msg("Deleted " & Image.hexID)
                    Image.Deleted = True
                    Image.Update()
                    countBad += 1
                Else
                    countOK += 1
                End If
            Next

            msg("Deleted " & countBad & ", kept " & countOK)

            '
            ' go thru all ads and make sure the ones left have a main image
            '
            Dim ads As New Ads
            ads.Retrieve()
            For Each Ad As Ad In ads
                Dim foundMain As Boolean = False
                For Each Image As Image In Ad.Images
                    If Image.IsMainImage Then
                        foundMain = True
                        Exit For
                    End If
                Next

                If Not foundMain Then
                    If Ad.Images.Count > 0 Then
                        Ad.Images(0).IsMainImage = True
                        Ad.Images(0).Update()
                        msg("Set main image on for ad " & Ad.ID & ": " & Ad.KeyWords)

                    End If

                End If

            Next


            msg("Done")

        Catch ex As Exception
            msg(ex.Message)
        End Try


    End Sub

    Private Sub msg(ByVal s As String)
        ListBox1.Items.Add(s)
    End Sub
End Class
