Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports ATLib

<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WebServices
    Inherits System.Web.Services.WebService

    <WebMethod()> _
        Public Sub IncrRotatorClickCount(ByVal HexID As String)
        Dim rotatorads As New RotatorAds
        Dim rotatorad As RotatorAd = rotatorads.Retrieve(HexID)
        rotatorad.IncrClickCount()
    End Sub

    <WebMethod()> _
      Public Sub IncrAdClickCount(ByVal HexID As String)
        Dim ads As New Ads
        Dim ad As Ad = ads.Retrieve(HexID)
        ad.IncrClickCount()

    End Sub

    <WebMethod()> _
Public Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim adInstances As New AdInstances
        Dim firstWebPubID As Integer = CommonRoutines.Hex2Int(contextKey.Substring(0, 8))
        Dim visibility As Edition.VisibleState = CType(contextKey.Substring(8, 2), Edition.VisibleState)
        Dim objectType As ATSystem.ObjectTypes = CType(contextKey.Substring(10, 2), ATSystem.ObjectTypes)
        Dim objectID As Integer = CommonRoutines.Hex2Int(contextKey.Substring(12, 8))
        Return adInstances.GetKeywordList(firstWebPubID, objectType, objectID, Visibility, Ad.ProdnState.Approved, prefixText)
    End Function

    <WebMethod()> _
Public Function GetAliasList(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim usrs As New Usrs
        Return usrs.GetAliasList(prefixText)
    End Function

End Class
