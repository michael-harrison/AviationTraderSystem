<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TermsOfUse.aspx.vb" Inherits="TermsOfUse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Privacy Policy</title>
    <link href="../App_Themes/AT Skin/default.css" type="text/css" rel="stylesheet" />
    <link href="../App_Themes/AT Skin/news.css" type="text/css" rel="stylesheet" />
</head>
<body style="background: white; text-align: left;margin:30px">
    <form id="form1" runat="server">
    <div>
        <cc1:InsertFileText ID="InsertFileText" Filename="statictext/termsofuse.txt" runat="server" />
    </div>
    </form>
</body>
</html>
