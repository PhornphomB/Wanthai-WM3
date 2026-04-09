<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InputTextInteger.ascx.cs" Inherits="_UControls.InputTextInteger" %>

<%@ Register Src="~/_UControls/InputControls/BaseInputControl.ascx" TagName="BaseControl" TagPrefix="userControl" %>


    <userControl:BaseControl ID="baseControl" runat="server">
        <ControlTemplate>
            <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqValidate" runat="server" SetFocusOnError="true"></asp:RequiredFieldValidator>
        </ControlTemplate>
    </userControl:BaseControl>
