<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportCommon.aspx.cs" Inherits="Acc.Manage.Reports.Report_aspx.ReportCommon" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <body>
        <form id="frm_report" runat="server">
        <div id="div_body">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewCommon" runat="server" Height="100%" Width="100%"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(集合)" Style="margin-right: 14px"
                WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" SizeToReportContent="True"
                OnDrillthrough="ReportViewCommon_Drillthrough">
            </rsweb:ReportViewer>
        </div>
        </form>
    </body>
</body>
</html>
