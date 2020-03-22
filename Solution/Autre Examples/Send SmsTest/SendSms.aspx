<%@ Page language="c#" Codebehind="SendSms.aspx.cs" AutoEventWireup="false" Inherits="SmsTest.WebForm1" %>

<HTML>
  <HEAD>
		<title>Sending Text Messages with .NET</title>
</HEAD>
	<body>
		<h2>Sending Text Messages with .NET</h2>
		<form method="post" runat="server" ID="Form1">
			<table>
  <TR>
    <TD colSpan=2 height=22>&nbsp;
<asp:RadioButtonList id=rdoType runat="server" AutoPostBack="True" RepeatDirection="Horizontal" Width="304px">
<asp:ListItem Value="1" Selected="True">SMS to India</asp:ListItem>
<asp:ListItem Value="2">SMS to world</asp:ListItem>
</asp:RadioButtonList></TD></TR>
				<TR>
					<TD>Email Id</TD>
					<TD>
						<asp:TextBox id="txtEmailId" runat="server"></asp:TextBox>( ex 
						:&nbsp;test@test.com )</TD>
				</TR>
				<TR>
					<TD>Country Code</TD>
					<TD><asp:textbox id="txtCountryCode" runat="server" text=""></asp:textbox>&nbsp;(ex 
						: 91 for&nbsp;India</TD>
				</TR>
				<tr>
					<td>Mobile/cell Number</td>
					<td><asp:textbox id="txtMobileNo" runat="server" text=""></asp:textbox>&nbsp;( ex 
						:&nbsp;984xxxxxxxx&nbsp;)</td>
				</tr>
				<tr>
					<td>Message</td>
					<td><asp:textbox id="txtMessage" runat="server" rows="4" textmode="MultiLine"></asp:textbox></td>
				</tr>
				<tr>
					<td></td>
					<td align="right"><asp:Button id="Send" runat="server" Text="Send"></asp:Button></td>
				</tr>
			</table>
			<asp:Label id="lblMessage" runat="server" forecolor="Red" Visible="False"></asp:Label>
		</form>
	</body>
</HTML>
