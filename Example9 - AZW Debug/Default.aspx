<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Example.Form1" %>

<!DOCTYPE html>

<html  xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background-color: #E0E0E0">
    <form id="form1" runat="server">
        <div>
            <h2>WinWrap&reg; Basic Example 9 - Azure Debug Web Site</h2>
            <table style="border-spacing: 10px;">
                <tr>
                    <td style="vertical-align: text-top">
                        <asp:Label ID="LabelScripts" runat="server" Text="Choose Script" Font-Bold="True"></asp:Label></td>
                    <td>
                        <asp:ListBox ID="ListBoxScripts" runat="server" Height="100px" Width="204px"></asp:ListBox></td>
                    <td style="vertical-align: text-top">
                        <asp:Button ID="ButtonRun" runat="server" OnClick="ButtonRun_Click" Text="Run" /><br />
                        <asp:Button ID="ButtonDebug" runat="server" OnClick="ButtonDebug_Click" Text="Debug" /><br />
                        <asp:Button ID="ButtonShow" runat="server" OnClick="ButtonShow_Click" Text="Show" /></td>
                </tr>
            </table>
            <pre style="border: 1px solid black; width: 600px; height: 80px; overflow: auto; background-color: #FFFF00;"><asp:Label ID="LabelLog" runat="server" Text=""></asp:Label></pre>
            <pre><asp:Label ID="LabelCode" runat="server"></asp:Label></pre>
            <p>
                <img border="0" src="http://www.winwrap.com/web/header2.asp<% {Response.Write("?zz=" + Request.QueryString["zz"]);} %>" /></p>
        </div>
    </form>
    <script type="text/javascript">
        window.onload = function () {
            if (document.getElementById('LabelLog').innerHTML.length == 0)
                document.getElementById('ButtonRun').click();
        }
    </script>
</body>
</html>
