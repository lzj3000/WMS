<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Print.aspx.cs" Inherits="Acc.Manage.DesginManager.Print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../res/scripts/JQuery/jquery-1.5.1.js" type="text/javascript"></script>
    <script src="../res/scripts/common.js" type="text/javascript"></script>
    <script src="../res/scripts/commonPaging.js" type="text/javascript"></script>
    <script>
        window.onload = function () {
            wmsCommon.print();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <img alt="" src="<%=barCodeImg %>" />
    </div>
    </form>
</body>
</html>
