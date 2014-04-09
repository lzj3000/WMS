<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report_StockInfoGather.aspx.cs" Inherits="Acc.Manage.Views.Report_StockInfoGather" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #Table1
        {
            width: 36px;
        }
    </style>
</head>
<script language="javascript" type="text/javascript" src="My97DatePicker/WdatePicker.js"></script>
<body>
    <form id="form1" runat="server">
        <font size=2>
        查询条件<br />
        &nbsp;&nbsp;
        <asp:Label ID="Label1" runat="server">产品名称:</asp:Label>
        <asp:TextBox ID="Fname" runat="server"></asp:TextBox>
        <asp:Label ID="Label2" runat="server">产品编码:</asp:Label>
        <asp:TextBox ID="Code" runat="server"></asp:TextBox>
        <br/>
        &nbsp;&nbsp;
        <asp:Label ID="Label4" runat="server">存储仓库:</asp:Label>
        <asp:TextBox ID="WareHouse" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="查询" />
        <br/>
        </font>
    <asp:ScriptManager ID="ScriptManager1" runat=server></asp:ScriptManager>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="4000px" 
        Width="1056px" Font-Names="Verdana" Font-Size="8pt" 
        InteractiveDeviceInfos="(集合)" style="margin-right: 14px" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
        SizeToReportContent="True">
    </rsweb:ReportViewer>
    </form>
</body>
</html>
