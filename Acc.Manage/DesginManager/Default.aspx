<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Acctrue.Equipment.Print.DesignManager._Default" %>

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
                    <td style="background: url(images/search_04.gif) repeat-y left;">
                    </td>
                    <td class="widthFull">
                        <div style="text-align: left; padding-bottom: 10px;">
                            <img src="images/document-1-add.png" border="0" style="vertical-align: middle" /><a
                                href="#" onclick="window.open('DesignEditor.aspx');"> 添加模板</a>
                            <img src="images/tools.png" border="0" style="vertical-align: middle; padding-left:20px;" /><a href="#"
                                onclick="document.URL='ParmManager.aspx'"> 参数维护</a>
                            <img src="images/arrow_refresh_small.png" border="0" style="vertical-align: middle; padding-left:20px;" /><asp:LinkButton
                                ID="Button_Refresh" runat="server" Text="刷 新" OnClick="Button_Refresh_Click" />
                        </div>
                    </td>
                    <td style="background: url(images/search_06.gif) repeat-y right;">
                    </td>
                </tr>
                <tr>
                    <td style="background: url(images/right_04.gif) repeat-y left;">
                    </td>
                    <td class="bothFull">
                        <table cellspacing="0" cellpadding="0" class="tblListParent">
                            <tr>
                                <td>
                                    <img alt="" src="images/list_01.gif" />
                                </td>
                                <td style="background: url(images/list_02.gif) repeat-x top;">
                                    <div class="listTitle">
                                        打印模板列表</div>
                                </td>
                                <td>
                                    <img alt="" src="images/list_03.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td style="background: url(images/list_04.gif) repeat-y left;">
                                </td>
                                <td style="text-align: center; width: 100%; height: 100%; vertical-align: top;">
                                    <asp:DataGrid ID="TempList" class="tblList" runat="server" AutoGenerateColumns="False"
                                        PageSize="20" AllowPaging="True" AllowCustomPaging="True" OnItemCommand="TempList_ItemCommand"
                                        OnPageIndexChanged="TempList_PageIndexChanged">
                                        <HeaderStyle CssClass="head" />
                                        <Columns>
                                            <asp:BoundColumn HeaderText="模板ID" DataField="ID" />
                                            <asp:BoundColumn HeaderText="模板名称" DataField="Name" ItemStyle-CssClass="leftComm" />
                                            <asp:BoundColumn HeaderText="创建时间" DataField="CreateDate" />
                                            <asp:BoundColumn HeaderText="修改时间" DataField="LastMode" />
                                            <asp:TemplateColumn HeaderText="操作">
                                                <ItemTemplate>
                                                    <a href='DesignEditor.aspx?id=<%#Eval("ID")%>' target="_blank">修 改</a> <a href='TempHistory.aspx?id=<%#Eval("ID")%>'>
                                                        查看历史</a>
                                                    <asp:LinkButton ID="lbnDelete" runat="server" CommandName="DEL" CommandArgument='<%#Eval("ID")%>'
                                                        OnClientClick="return confirm('您确认要删除此模板吗？');">删除</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle Mode="NumericPages" NextPageText="下一页" PrevPageText="上一页" />
                                    </asp:DataGrid>
                                </td>
                                <td style="background: url(images/list_06.gif) repeat-y right;">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <img alt="" src="images/list_07.gif" />
                                </td>
                                <td style="background: url(images/list_08.gif) repeat-x bottom;">
                                </td>
                                <td>
                                    <img alt="" src="images/list_09.gif" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="background: url(images/right_06.gif) repeat-y right;">
                    </td>
                </tr>
                <tr>
                    <td>
                        <img alt="" src="images/right_07.gif" />
                    </td>
                    <td style="background: url(images/right_08.gif) repeat-x bottom;">
                    </td>
                    <td>
                        <img alt="" src="images/right_09.gif" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
