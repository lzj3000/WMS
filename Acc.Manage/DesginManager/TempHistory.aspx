<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TempHistory.aspx.cs" Inherits="Acctrue.Equipment.Print.DesignManager.TempHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/right.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/right.js"></script>
    <script type="text/javascript" src="js/common.js"></script>
    <style type="text/css">
        .head td
        {
            height: 27px;
            font: 12px "宋体";
            font-weight: bold;
            color: #056ea4;
            text-align: center;
            border-left: 1px solid #cad3da;
            white-space: nowrap;
        }
        .leftComm
        {
            text-align: left !important;
            padding-left: 3px;
        }
    </style>
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
                                        打印模板备份列表</div>
                                </td>
                                <td><img alt="" src="images/list_03.gif" /></td>
                            </tr>
                            <tr>
                                <td style="background: url(images/list_04.gif) repeat-y left;">
                                </td>
                                <td style="text-align: center; width: 100%; height: 100%; vertical-align: top;">
                                    <div style="text-align: left; padding-bottom: 10px;">
                                        <asp:Label ID="Label_Name" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="Label_NoMSG" runat="server" Visible="false" Font-Size="X-Large" ForeColor="Red"
                                            Text="无法查找到此模板的备份！"></asp:Label>
                                    </div>
                                    <asp:DataGrid ID="TempList" class="tblList" runat="server" AutoGenerateColumns="False"
                                        AllowPaging="True" OnItemCommand="TempList_ItemCommand" OnPageIndexChanged="TempList_PageIndexChanged"
                                        AllowCustomPaging="True">
                                        <HeaderStyle CssClass="head" />
                                        <Columns>
                                            <asp:BoundColumn HeaderText="序号" DataField="SID" />
                                            <asp:BoundColumn HeaderText="模板版本号" DataField="Sno" />
                                            <asp:BoundColumn HeaderText="备注" DataField="Detail" ItemStyle-Width="320" ItemStyle-CssClass="leftComm">
                                                <ItemStyle CssClass="leftComm" Width="320px" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn HeaderText="创建时间" DataField="CreateDate" />
                                            <asp:TemplateColumn HeaderText="操作">
                                                <ItemTemplate>
                                                    <a href='DesignEditor.aspx?sid=<%#Eval("SID")%>' target="_blank">查 看</a>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="REC" CommandArgument='<%#Eval("SID")%>'
                                                        OnClientClick="return confirm('您确认要还原此模板吗？')">还原模板</asp:LinkButton>
                                                    <asp:LinkButton ID="lbnDelete" runat="server" CommandName="DEL" CommandArgument='<%#Eval("SID")%>'
                                                        OnClientClick="return confirm('您确认要删除此备份吗？')">删除备份</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle Mode="NumericPages" NextPageText="下一页" PrevPageText="上一页" />
                                    </asp:DataGrid>
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
