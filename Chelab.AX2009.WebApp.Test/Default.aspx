<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Chelab.AX2009.WebApp.Test._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Table</title>
    <link id="gridviewcss" href="Style/Grv/WhiteChromeGridView.css" rel="stylesheet"
        type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="grv_01" runat="server" AllowPaging="true" OnPageIndexChanging="grv_01_PageIndexChanging"
            CssClass="GridViewStyle" PageSize="20">
            <HeaderStyle CssClass="HeaderStyle" />
            <RowStyle CssClass="RowStyle" />
            <AlternatingRowStyle CssClass="AltRowStyle" />
            <PagerStyle CssClass="PagerStyle" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>
