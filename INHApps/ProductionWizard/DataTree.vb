Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Data Tree
'*
'* AUDIT TRAIL
'* 
'* V1.000   01-Dec-2009  BA  Original
'*
'* This is a viewer for an ad instance
'*
'*
'*
'*
'***************************************************************************************

Public Class DataTree
    Public Event NodeSelect(ByVal node As TreeNode)

    Private _selectedNode As TreeNode
    Private sys As ATSystem
    Private _aliasID As Integer
    Private _pubID As Integer
    Private _ednID As Integer

    Public ReadOnly Property SelectedNode() As TreeNode
        Get
            Return _selectedNode
        End Get
    End Property

    Public ReadOnly Property AliasID() As Integer
        Get
            Return _aliasID
        End Get
    End Property

    Public Sub Clear()
        TreeView.Nodes.Clear()
    End Sub

    Public Sub Render()
        '
        ' get a new parent object
        '
        sys = New ATSystem
        sys.Retrieve()
        '
        ' load the alias and pub DDs
        '
        '
        ' display the current alias publication and edn drop downs
        '
        Dim aliasList As New List(Of Usr)
        aliasList.Add(New Usr)
        aliasList(0).AcctAlias = "All Advertisers"
        For Each Usr As Usr In sys.Usrs
            aliasList.Add(Usr)
        Next

        AliasDD.DisplayMember = "AcctAlias"
        AliasDD.ValueMember = "ID"                  'fires change event to load list
        AliasDD.SelectedItem = aliasList(0)
        AliasDD.DataSource = aliasList
      

        PubDD.DisplayMember = "Name"
        PubDD.ValueMember = "ID"
        PubDD.SelectedItem = sys.Publications(0)
        PubDD.DataSource = sys.Publications

        TreeView.BeginUpdate()
        TreeView.Nodes.Clear()
        Dim root As New TreeNode
        root.Tag = Int2ShortHex(ATSystem.ObjectTypes.System) & sys.hexID
        root.Text = sys.Name
        TreeView.Nodes.Add(root)
        root.Nodes.Add("dummy")
        '    root.Expand()
        TreeView.EndUpdate()
    End Sub

    Private Sub displayFolders(ByVal p As TreeNode, ByVal sysID As Integer)

        TreeView.BeginUpdate()
        p.Nodes.Clear()       'remove dummy


        For Each Folder As Folder In sys.ProdnFolders
            Dim n As New TreeNode
            n.Tag = Int2ShortHex(ATSystem.ObjectTypes.Folder) & sys.hexID & Folder.hexID
            n.Text = Folder.Name
            p.Nodes.Add(n)
            n.Nodes.Add("dummy")
        Next
        TreeView.EndUpdate()
    End Sub

    Private Sub displayUsrs(ByVal p As TreeNode, ByVal SysID As Integer, ByVal FolderID As Integer)

        TreeView.BeginUpdate()
        p.Nodes.Clear()       'remove dummy
        Dim usrs As New Usrs
        '
        ' if alias filtering is on, just get the single user, else get the lot
        '
        If _aliasID = ATSystem.SysConstants.nullValue Then
            usrs.retrieveSet(SysID)
        Else
            usrs.Retrieve(_aliasID)
        End If

        Dim AdInstances As New AdInstances
        For Each usr As Usr In usrs
            '
            ' only add the user if he has ads in the current folder and 
            ' at least one of those ads has an instance in the current edition
            '
            Dim ads As New Ads

            ads.RetrieveSet(ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue, usr.ID, FolderID, Ad.ProdnState.Unspecified, Ad.SortOrders.AdNumber)
            If ads.Count > 0 Then       'user has ads in folder
                Dim includeUsr As Boolean = False
                For Each Ad As Ad In ads
                    AdInstances.RetrieveSet(ATSystem.SysConstants.nullValue, Ad.ID, ATSystem.SysConstants.nullValue, _ednID, ATSystem.SysConstants.nullValue)
                    If AdInstances.Count > 0 Then
                        includeUsr = True
                        Exit For
                    End If
                Next

                If includeUsr Then
                    Dim n As New TreeNode
                    n.Tag = Int2ShortHex(ATSystem.ObjectTypes.Usr) & Int2Hex(FolderID) & usr.hexID
                    n.Text = usr.AcctAlias
                    p.Nodes.Add(n)
                    n.Nodes.Add("dummy")
                End If
            End If
        Next
        TreeView.EndUpdate()
    End Sub


    Private Sub displayProdnStatus(ByVal p As TreeNode, ByVal FolderID As Integer, ByVal UsrID As Integer)
        Dim Usrs As New Usrs
        Dim Usr As Usr = Usrs.Retrieve(UsrID)
        TreeView.BeginUpdate()
        p.Nodes.Clear()       'remove dummy
        
        Dim EA As New EnumAssistant(New Ad.ProdnState, Ad.ProdnState.Submitted, Ad.ProdnState.Approved)
        For Each ei As EnumItem In EA
            Dim n As New TreeNode
            n.Text = ei.Name
            n.Tag = Int2ShortHex(ATSystem.ObjectTypes.ProdnStatus) & Int2Hex(FolderID) & Int2Hex(UsrID) & Int2Hex(ei.Value)
            p.Nodes.Add(n)
            n.Nodes.Add("dummy")
        Next
        TreeView.EndUpdate()

    End Sub

    Private Sub displayAds(ByVal p As TreeNode, ByVal FolderID As Integer, ByVal usrID As Integer, ByVal prodnStatus As Ad.ProdnState)

        TreeView.BeginUpdate()
        p.Nodes.Clear()       'remove dummy
        '
        ' get all ads in this folder for this user.
        '

        Dim ads As New Ads
        ads.RetrieveSet(ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue, usrID, FolderID, prodnStatus, Ad.SortOrders.AdNumber)

        Dim adinstances As New AdInstances
        For Each ad As Ad In ads
            '
            ' dont display the ad unless it has instances in the current edition
            ' If there is only one instance, click straight to instance viewer. Otherwise display producttypes
            '
            adinstances.RetrieveSet(ATSystem.SysConstants.nullValue, ad.ID, ATSystem.SysConstants.nullValue, _ednID, ATSystem.SysConstants.nullValue)
          
            If adinstances.Count = 1 Then
                Dim adInstance As AdInstance = adinstances(0)
                Dim n As New TreeNode
                n.Tag = Int2ShortHex(ATSystem.ObjectTypes.AdInstance) & adInstance.hexID
                n.Text = ad.Adnumber
                p.Nodes.Add(n)
            ElseIf adinstances.Count > 1 Then
                Dim n As New TreeNode
                n.Tag = Int2ShortHex(ATSystem.ObjectTypes.Ad) & ad.hexID
                n.Text = ad.Adnumber
                p.Nodes.Add(n)
                n.Nodes.Add("dummy")
            End If
        Next
        TreeView.EndUpdate()
    End Sub



    Private Sub displayInstances(ByVal p As TreeNode, ByVal AdID As Integer)
        Dim Ads As New Ads
        Dim Ad As Ad = Ads.Retrieve(AdID)
        TreeView.BeginUpdate()
        p.Nodes.Clear()       'remove dummy
        '
        ' filter the instances by edition and only include display and classad types
        '
        Dim instanceList As New List(Of AdInstance)


        For Each ai As AdInstance In Ad.Instances
            If ai.EditionID = _ednID Then
                Select Case ai.ProductType
                    Case Product.Types.ClassadColorPic, _
                    Product.Types.ClassadMonoPic, _
                    Product.Types.ClassadTextOnly, _
                        Product.Types.DisplayComposite, _
                        Product.Types.DisplayFinishedArt, _
                        Product.Types.DisplayModule, _
                        Product.Types.DisplayStandAlone

                        Dim m As New TreeNode
                        m.Tag = Int2ShortHex(ATSystem.ObjectTypes.AdInstance) & ai.hexID
                        m.Text = ai.ProductType.ToString
                        p.Nodes.Add(m)


                End Select
            End If


        Next


        TreeView.EndUpdate()

    End Sub




    Private Sub TView_AfterCollapse(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView.AfterCollapse
        '
        ' remove subordinate objects from list and add back a dummy
        '
        e.Node.Nodes.Clear()
        e.Node.Nodes.Add("dummy")

    End Sub

    Private Sub TView_AfterExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView.AfterExpand
        Dim hexTag As String = e.Node.Tag.ToString
        Dim nodeType As ATSystem.ObjectTypes = CType(Hex2Int(hexTag.Substring(0, 2)), ATSystem.ObjectTypes)
 
        Select Case nodeType

            Case ATSystem.ObjectTypes.System
                Dim SysID As Integer = Hex2Int(hexTag.Substring(2, 8))
                displayFolders(e.Node, SysID)

            Case ATSystem.ObjectTypes.Folder
                Dim SysID As Integer = Hex2Int(hexTag.Substring(2, 8))
                Dim FolderID As Integer = Hex2Int(hexTag.Substring(10, 8))
                displayUsrs(e.Node, SysID, FolderID)

            Case ATSystem.ObjectTypes.Usr
                Dim FolderID As Integer = Hex2Int(hexTag.Substring(2, 8))
                Dim usrID As Integer = Hex2Int(hexTag.Substring(10, 8))
                displayProdnStatus(e.Node, FolderID, usrID)

            Case ATSystem.ObjectTypes.ProdnStatus
                Dim FolderID As Integer = Hex2Int(hexTag.Substring(2, 8))
                Dim usrID As Integer = Hex2Int(hexTag.Substring(10, 8))
                Dim prodnStatus As Ad.ProdnState = CType(hexTag.Substring(18, 8), Ad.ProdnState)
                displayAds(e.Node, FolderID, usrID, prodnStatus)


            Case ATSystem.ObjectTypes.Ad
                Dim adID As Integer = Hex2Int(hexTag.Substring(2, 8))
                displayInstances(e.Node, adID)

            




        End Select
    End Sub


    Private Sub TreeView_BeforeSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles TreeView.BeforeSelect
        If Not TreeView.SelectedNode Is Nothing Then
            TreeView.SelectedNode.BackColor = Color.White
            TreeView.SelectedNode.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TreeView_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView.AfterSelect
        '
        ' will only be fired if a new node is selected - not if the same node is clicked again
        '
        _selectedNode = e.Node
        _selectedNode.BackColor = Color.DarkBlue
        _selectedNode.ForeColor = Color.White
        RaiseEvent NodeSelect(_selectedNode)
    End Sub

    Private Sub AliasDD_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AliasDD.SelectedValueChanged
        _aliasID = Convert.ToInt32(AliasDD.SelectedValue)
    End Sub


    Private Sub PubDD_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PubDD.SelectedValueChanged
        If PubDD.SelectedValue IsNot Nothing Then
            _pubID = Convert.ToInt32(PubDD.SelectedValue)
            '
            ' load the edition DD from the selected pub - only include editions which have the isVisibleInWizard bit set
            '
            Dim publications As New Publications
            Dim publication As Publication = publications.Retrieve(_pubID)

            EdnDD.DisplayMember = "Name"
            EdnDD.ValueMember = "ID"

            Dim ednList As New List(Of Edition)
            For Each edn As Edition In publication.Editions
                If edn.IsVisibleInWizard Then ednList.Add(edn)
            Next
            If ednList.Count = 0 Then
                TreeView.Visible = False
                EdnDD.Visible = False
            Else
                TreeView.Visible = True
                EdnDD.Visible = True
                EdnDD.SelectedItem = ednList(0)
                EdnDD.DataSource = ednList
            End If
        End If
    End Sub


    Private Sub EdnDD_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles EdnDD.SelectedValueChanged
        _ednID = Convert.ToInt32(EdnDD.SelectedValue)
    End Sub
End Class
