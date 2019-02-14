<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PilotView.aspx.cs" Inherits="Husky_Air.PilotView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
            <Columns>
                <asp:BoundField DataField="Emailid" HeaderText="Emailid" SortExpression="Emailid" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
                <asp:BoundField DataField="Dob" HeaderText="Dob" SortExpression="Dob" />
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                <asp:BoundField DataField="Phoneno" HeaderText="Phoneno" SortExpression="Phoneno" />
                <asp:BoundField DataField="Hrs" HeaderText="Hrs" SortExpression="Hrs" />
                <asp:BoundField DataField="Certification" HeaderText="Certification" SortExpression="Certification" />
                <asp:BoundField DataField="Rating" HeaderText="Rating" SortExpression="Rating" />
                <asp:BoundField DataField="Passowrd" HeaderText="Passowrd" SortExpression="Passowrd" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HuskyAirConnectionString %>" SelectCommand="SELECT * FROM [Pilot]"></asp:SqlDataSource>
    </form>
</body>
</html>
