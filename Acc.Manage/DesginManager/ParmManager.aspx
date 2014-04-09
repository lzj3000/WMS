<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParmManager.aspx.cs" Inherits="Acctrue.Equipment.Print.DesignManager.ParmManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/right.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/right.js"></script>
    <script type="text/javascript" src="js/common.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <table cellspacing="0" cellpadding="0" class="tblOuter">
                <tr>
                    <td style="background: url(images/search_04.gif) repeat-y left;"></td>
                    <td class="widthFull" style="text-align:left; padding:5px;">
                        <table cellpadding="0" border="0" style="height: 30px; width: 600px; margin: 0;">
                            <tr>
                                <td style="text-align: right; width: 70px;">
                                    属性：
                                </td>
                                <td style="text-align: left; width: 100px;">
                                    <asp:TextBox ID="TextBox_Pro" runat="server" Height="20" Width="100" MaxLength="32"></asp:TextBox>
                                </td>
                                <td style="text-align: right; width: 70px;">
                                    属性名称：
                                </td>
                                <td style="text-align: left; width: 100px;">
                                    <asp:TextBox ID="TextBox_ProName" runat="server" Height="20" Width="120" MaxLength="32"></asp:TextBox>
                                </td>
                                <td style="text-align: left; width: 120px;">
                                    <asp:DropDownList runat="server" ID="DropDownList_Pro" Width="120" AutoPostBack="true"
                                        OnSelectedIndexChanged="DropDownList_Pro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: left; width: 100px;">
                                    <asp:Button ID="Button_Add" runat="server" Text="增 加" Height="25" Width="80" OnClick="Button_Add_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
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
                                        打印参数列表</div>
                                </td>
                                <td><img alt="" src="images/list_03.gif" /></td>
                            </tr>
                            <tr>
                                <td style="background: url(images/list_04.gif) repeat-y left;"></td>
                                <td style="text-align: center; width: 100%; height: 100%; vertical-align: top;">
                                    <asp:GridView ID="TempList" class="tblList" runat="server" AutoGenerateColumns="False"
                                        OnRowCommand="TempList_RowCommand">
                                        <Columns>
                                            <asp:BoundField HeaderText="序 号" DataField="SID" />
                                            <asp:BoundField HeaderText="属 性" DataField="BindProperty" />
                                            <asp:BoundField HeaderText="属性名称" DataField="BindPropertyName" />
                                            <asp:BoundField HeaderText="排序号" DataField="OrderNo" />
                                            <asp:BoundField HeaderText="创建时间" DataField="CreateDate" />
                                            <asp:TemplateField HeaderText="操 作">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbnDelete" runat="server" CommandName="DEL" CommandArgument='<%#Eval("SID")%>'
                                                        OnClientClick="return confirm('您确认要删除此模板吗？');">删除</asp:LinkButton>
                                                    <asp:LinkButton ID="LinkButton_Up" runat="server" CommandName="UP" CommandArgument='<%#Eval("SID")%>'>向上</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <a href="Default.aspx">返 回</a>
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
                    <td style="background: url(images/right_06.gif) repeat-y right;"></td>
                </tr>
                <tr>
                    <td><img alt="" src="images/right_07.gif" /></td>
                    <td style="background: url(images/right_08.gif) repeat-x bottom;"></td>
                    <td><img alt="" src="images/right_09.gif" /></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
