<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HospitalView.aspx.cs" Inherits="Husky_Air.HospitalView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="ccn" DataSourceID="SqlDataSource1">
            <Columns>
                <asp:BoundField DataField="ccn" HeaderText="ccn" ReadOnly="True" SortExpression="ccn" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                <asp:BoundField DataField="Phoneno" HeaderText="Phoneno" SortExpression="Phoneno" />
                <asp:BoundField DataField="Emailid" HeaderText="Emailid" SortExpression="Emailid" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HuskyAirConnectionString %>" SelectCommand="SELECT * FROM [Hospital]"></asp:SqlDataSource>
    </form>
</body>
</html>
