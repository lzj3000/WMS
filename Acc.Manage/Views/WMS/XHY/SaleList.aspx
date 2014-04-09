<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaleList.aspx.cs" Inherits="Acc.Manage.SaleList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
       
        .style1
        {
            width: 360px;
        }
        .style3
        {
            width: 180px;
        }
        .style4
        {
            width: 181px;
        }
               
        .style6
        {
            height: 49px;
        }
       
        .style7
        {
            width: 150px;
        }
       
    </style>
</head>
<body style="height: 612px;  color:Black; font-size:14px;">

    <form id="form1" runat="server">
<div style="left:auto auto auto 20px;">
        <div style="height: auto; border: 0px solid black; width:auto;">
            <table style="height: 160px; width: 700px; line-height: 1px; font-size:14px;">
                <tr>
                    <td colspan="5" align="center" class="style11">
                        <div onclick="window.print();">
                            <span style="font-family: Arial; color:Black; font-size: 14pt; font-weight: 700;
                                font-style: normal; text-decoration: none; text-decoration: underline">武汉新华扬生物股份有限公司销售出库单 </span>
                        </div>
                    </td>
                </tr>
                <tr style="text-align: left;">
                    <td style="text-align: left;">
                        客户类型:<%=khlx %>
                    </td>
                    <td style="text-align: left;" >
                        发货仓库:<%=warehousename%>
                    </td>
 <td style="text-align: left;" >
                        编号:<%=code %>
                    </td>

                </tr>
                <tr style="text-align: left;">
                    <td class="style1">
                        购货单位:<%=customername%>
                    </td>
                    <td class="style7">
                        购货单位联系人:<%=lxrname%>
                    </td>
                    <td class="style4" colspan="2" >
                        移动电话:
                    </td>
                </tr>
                <tr style="text-align: left;">
                    <td colspan="2">
                        购货单位地址:<%=lxraddress%>
                    </td>
 <td class="style3">
                        联系电话:<%=lxrphone%>
                    </td>
                </tr>
<tr style="text-align: left;">
<td><br/></td>
</tr>
                <tr style="text-align: left;">
                    <td class="style1">
                        终端客户:<%=zdkh%>
                    </td>
                    <td class="style7">
                        终端联系人:<%=zdlxr%>
                    </td>
                    <td class="style4" colspan="2" >
                        终端手机:<%=zdphone%>
                    </td>
               
                </tr>

                <tr style="text-align: left;">
                    <td colspan="2">
                        终端客户地址:<%=zdaddress%>
                    </td>
 <td class="style7">
                        终端电话:<%=zdtel%>
                    </td>
                </tr>
                <tr style="text-align: left;">
                    <td class="style1">
                        运输方式:<%=logisticsname%>
                    </td>
                    <td class="style7">
                        运费:<%=yf %>
                    </td>
                    <td class="style3">
                        交货方式:<%=jhfs%>
                    </td>
                   
                 
                </tr>
                
            </table>

            <table style="width: 650px; text-align: left;">
                <tr>
                    <td>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="650px"
                            
                            BorderStyle="Solid" BorderWidth="1px" CellPadding="4" CellSpacing="2" 
                            ForeColor="Black" >
                            <Columns>
                                <asp:BoundField DataField="MCODE" HeaderText="产品代码">
                                 <ItemStyle Width="100px" />
                                  </asp:BoundField>
                                <asp:BoundField DataField="FNAME" HeaderText="产品名称" >
                                <ItemStyle Width="495px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FMODEL" HeaderText="规格型号" >
                                 <ItemStyle Width="115px" />
                                  </asp:BoundField>
                                <asp:BoundField DataField="bz" HeaderText="包装" >
                                <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="单位" DataField="dw" >
                                 <ItemStyle Width="50px" />
                                  </asp:BoundField>
                                <asp:BoundField DataField="num" HeaderText="数量" >
                                  <ItemStyle Width="70px" />
                                  </asp:BoundField>
                                <asp:BoundField DataField="jian" HeaderText="件数" >
                                 <ItemStyle Width="70px" />
                                  </asp:BoundField>
                            </Columns>
                           
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <table style="height: 60px; width: 655px; line-height: 13px;">
                <tr style="text-align: left;">
                    <td style="border: 1px solid black" class="style6">
                        摘要:<%=remark%>
                    </td>
                </tr>
                <tr style="text-align: right;">
                    <td class="style9">
                      
                        日期:<%=creationdate%>&nbsp;&nbsp;&nbsp;
                     编号:<%=code %>&nbsp;&nbsp;&nbsp; 业务单位:<%=ywdw%>
                    </td>
                </tr>
            </table>
        </div>
        <%-- <div style=" margin:300px auto auto auto;">
       <input type="button"   " style="width: 0px" />
    </div>--%>
</div>
    </form>
</body>
</html>
