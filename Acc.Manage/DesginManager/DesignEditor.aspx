<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DesignEditor.aspx.cs" Inherits="Acctrue.Equipment.Print.DesignManager.DesignEditor" %>
<html>
<head runat="server">
    <title></title>
    <link href="css/right.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/right.js"></script>
    <script type="text/javascript" src="js/common.js"></script>
    <script src="../Scripts/jquery-1.3/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var doc = document.getElementById('designEditor');
            if (document.getElementById("TempValue") && document.getElementById("TempValue").value) {
                doc.LoadData(document.getElementById("TempValue").value);
                doc.LoadProperty(document.getElementById("TempProperty").value);
            }
            if (document.getElementById("TempValue").value.length <= 0) {
                doc.NewTemp();
            }
        });

        function OnSaveTemp() {
            var data = document.getElementById('designEditor').GetData();
            document.getElementById("TempValue").value = data;
        }

        function OpenFrom() {
            document.getElementById('designEditor').OpenFrom();

        }

        function SaveAs() {
            document.getElementById('designEditor').SaveAs();
        }
    </script>
    <style type="text/css">
        .tdtitle
        {
            width: 100px;
            text-align: right;
            font-weight: bold;
        }
        .actButton
        {
            width: 72px;
            height: 40px;
            margin-left: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" class="tblOuter" style=" width: 980px;">
        <tr>
            <td style="background: url(images/search_04.gif) repeat-y left;"></td>
            <td style="background: url(images/search_06.gif) repeat-y right;"></td>
        </tr>
        <tr>
            <td style="background: url(images/right_04.gif) repeat-y left;"></td>
            <td class="bothFull">
                <table cellspacing="0" cellpadding="0" class="tblListParent">
                    <tr>
                        <td><img alt="" src="images/list_01.gif" /></td>
                        <td style="background: url(images/list_02.gif) repeat-x top;">
                            <div class="listTitle">
                                打印模板编辑</div>
                        </td>
                        <td><img alt="" src="images/list_03.gif" /></td>
                    </tr>
                    <tr>
                        <td style="background: url(images/list_04.gif) repeat-y left;"></td>
                        <td style="text-align: center; width: 100%; height: 100%; vertical-align: top;">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                <ContentTemplate>
                                    <asp:HiddenField runat="server" ID="TempValue" />
                                    <asp:HiddenField runat="server" ID="TempProperty" />
                                    <table border="0" cellpadding="1" style="padding: 3px; height: 80px;">
                                        <tr>
                                            <td class="tdtitle">
                                                模板标识：
                                            </td>
                                            <td width="440">
                                                <asp:Label ID="Label_ID" runat="server"></asp:Label>
                                            </td>
                                            <td class="tdtitle">
                                                模板名称：
                                            </td>
                                            <td width="440">
                                                <asp:TextBox ID="TextBox_Name" runat="server" Height="25" Width="220" MaxLength="32"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdtitle">
                                                模板描述：
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox_Detail" runat="server" TextMode="MultiLine" Width="400"
                                                    Height="40" MaxLength="500"></asp:TextBox>
                                            </td>
                                            <td colspan="2" align="center" style="padding: 5px;">
                                                <asp:Button ID="Button_Save" CssClass="actButton" runat="server" Text="保存模板" OnClientClick="OnSaveTemp()"
                                                    OnClick="Button_Save_Click" />
                                                <asp:Button ID="Button1" CssClass="actButton" runat="server" Text="保存并备份" OnClientClick="OnSaveTemp()"
                                                    OnClick="Button1_Click" />
                                                <asp:Button ID="Button_Open" CssClass="actButton" runat="server" Text="导入模板" OnClientClick="OpenFrom();return false;" />
                                                <asp:Button ID="Button_OutPut" CssClass="actButton" runat="server" Text="导出模板" OnClientClick="SaveAs();return false;" />
                                                <input type="button" class="actButton" onclick="javascript:document.URL='ActiveX/Acctrue.Equipment.Print.ActiveXSetup.zip'" value="组件安装包" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div style="width: 100%; border:solid 1px #666666; padding:3px;">
                                <object id="designEditor" name="designEditor" classid="clsid:08C184B0-2694-4B89-840C-D9EE18CB9986">
                                </object>
                            </div>
                        </td>
                        <td style="background: url(images/list_06.gif) repeat-y right;"></td>
                    </tr>
                    <tr>
                        <td><img alt="" src="images/list_07.gif" /></td>
                        <td style="background: url(images/list_08.gif) repeat-x bottom;"></td>
                        <td><img alt="" src="images/list_09.gif" /></td>
                    </tr>
                </table>
            </td>
            <td style="background: url(images/right_06.gif) repeat-y right;">
            </td>
        </tr>
        <tr>
            <td><img alt="" src="images/right_07.gif" /></td>
            <td style="background: url(images/right_08.gif) repeat-x bottom;"></td>
            <td><img alt="" src="images/right_09.gif" /></td>
        </tr>
    </table>
    </form>
</body>
</html>
