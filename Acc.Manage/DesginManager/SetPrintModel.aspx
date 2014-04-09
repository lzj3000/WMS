<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetPrintModel.aspx.cs" Inherits="Acc.Manage.SetPrintModel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<script type="text/javascript" language="javascript">　　
function Ceshi()　　{
    alert("nihao");
   if (confirm("您确定要关闭本页吗？")){
    window.opener=null;
    window.open('','_self');
    window.close();
    }
    else{}
    }
}
</script>
<body>
    <form id="form1" runat="server">
    <div  style="background-color:rgb(255,251,255)">
        <br />
        <span style="font-size: 90;">打印模板设置</span><br />
        <br />
        <span style="font-size: 12; font-weight: bolder;">打印模板选择</span><asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button1" runat="server" Text="保存" onclick="Button1_Click" />
    </div>
    </form>
</body>
</html>
