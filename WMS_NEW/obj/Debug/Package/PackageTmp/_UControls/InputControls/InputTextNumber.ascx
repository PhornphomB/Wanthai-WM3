<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InputTextNumber.ascx.cs" Inherits="_UControls.InputTextNumber" %>

<%@ Register Src="~/_UControls/InputControls/BaseInputControl.ascx" TagName="BaseControl" TagPrefix="userControl" %>

    <userControl:BaseControl ID="baseControl" runat="server">
        <ControlTemplate>
            <asp:TextBox ID="txtValue" runat="server" CssClass="form-control text-number"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqValidate" runat="server" SetFocusOnError="true" Style="display: none;"></asp:RequiredFieldValidator>
        </ControlTemplate>
    </userControl:BaseControl>
