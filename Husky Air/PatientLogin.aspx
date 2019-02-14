<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PatientLogin.aspx.cs" Inherits="Husky_Air.PatientLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 656px;
        }
        .auto-style3 {
            width: 656px;
            text-align: right;
        }
    </style>
</head>
<body>

    <center><h1>Patient Login</h1></center>
    <form id="form1" runat="server">
        <table class="auto-style1">
            <tr>
                <td class="auto-style3">Emailid</td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">password</td>
                <td>
                    <asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">&nbsp;</td>
                <td>
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
                </td>
            </tr>
        </table>
        <div>
        </div>
    </form>
     <center><a href="Patient.aspx">Click here to Register</a></center>
   
</body>
</html>
