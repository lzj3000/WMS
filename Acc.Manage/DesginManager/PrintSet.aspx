<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintSet.aspx.cs" Inherits="Acc.Manage.DesginManager.PrintSet" %>
<%@ Register Assembly="Acctrue.Equipment.WebControls" TagPrefix="ccl" Namespace="Acctrue.Equipment.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>打印设置</title>
    <link href="../res/styles/css/common.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function print() {
            try {
                document.getElementById("printAX").click();         
            } catch (e) { }
        }

        function setPrinter() {
            document.getElementById("printset").click();
        }
        function window.onload() {
            var sFun = getQueryValue("funName");
            var sPar = getQueryValue("parameter");
            document.getElementById("txtfunName").value = sFun;
            document.getElementById("txtparameter").value = sPar;
        }


        //得到查询字符串键值对。
        getQueryString = function () {
            var obj = window.dialogArguments;
            var queryString = new Array();
            var reg = /([^=&]*)=([^&]*)/g;
            var m;
            while ((m = reg.exec(obj)) != null) {
                queryString[m[1]] = unescape(m[2]);
            }
            return queryString;
        }

        //得到查询字符串中对应键的值
        getQueryValue = function (key) {
            try {
                return getQueryString()[key];
            }
            catch (e) {
                return "";
            }
        }
    </script>
</head>
<body style="padding-bottom: 5px; padding-left: 5px; padding-right: 5px; padding-top: 5px;
    background-color: White;">
    <form id="form1" runat="server">
    <table class="DisplayArea" style="font-size: 12px; color: Black; width: 100%; height: 100%;"
        cellpadding="0" cellspacing="0" align="center">
        <tr>
            <td class="headmiddle" style="height: 26px">
                <span id="LableTitle2">打印设置</span>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: text-top;">
                <!--edit content start-->
                <table class="singleEdit" cellpadding="0" cellspacing="0" style="background-color:rgb(255,251,255)">
                    <tr>
                        <td align="left" style="height: 30px; font-size: 12; font-weight: bolder;">
                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
                            <span style="font-size: 12; font-weight: bolder;">打印模版选择 </span>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="DropDownList1" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="font-size: 12; font-weight: bolder;">打印机设置 </span>
                        </td>
                        <td style="height: 30px" align="left">
                            <input class="imgBtn" onmouseover="this.className='btn_yellow'" onmouseout="this.className='imgBtn'"
                                onclick="setPrinter();" type="button" value="设置" style="padding-right: 9px; padding-left: 9px" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td>
                            <asp:TextBox ID="txtfunName" runat="server" Width="1px"></asp:TextBox>
                        </td>
                        <td style="height: 30px" align="left">
                            <asp:TextBox ID="txtparameter" runat="server" Width="1px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td colspan="2">
                            <asp:UpdatePanel ID="UP" runat="server">
                                <ContentTemplate>
                                    <ccl:PrintActiveX ID="printAX" Text="打印" runat="server" OnClick="btn_PP_Click"></ccl:PrintActiveX>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                   <tr style="display: none">
                        <td style="height: 30px" align="left" colspan="2">
                            <ccl:PrintSetActiveX ID="printset" Text="设置打印机" runat="server" ForPrintControlName="printAX" />
                        </td>
                    </tr>
        <tr>
            <td align="center" colspan="2" style="height: 40px;">
                <input class="imgBtn" onmouseover="this.className='btn_yellow'" onmouseout="this.className='imgBtn'"
                    onclick="print()" type="button" value="打印" style="padding-right: 20px; padding-left: 20px" />
            </td>
        </tr>
        </table>
        <!-- edit content end-->
        </td> </tr>
    </table>
    </form>
</body>
</html>
