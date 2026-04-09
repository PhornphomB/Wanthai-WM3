<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InputTextDate.ascx.cs" Inherits="_UControls.InputTextDate" %>
<%@ Register Src="~/_UControls/InputControls/BaseInputControl.ascx" TagName="BaseControl" TagPrefix="userControl" %>

<userControl:BaseControl ID="baseControl" runat="server">
    <ControlTemplate>
        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control text-left datepicker"></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqValidate" runat="server" SetFocusOnError="true"></asp:RequiredFieldValidator>
        <asp:LinkButton ID="linkDateTrigger" CssClass="dp_change" runat="server" Text="Trigger" Style="display: none;" CausesValidation="false"></asp:LinkButton>

        <asp:Panel ID="panelValueTo" runat="server" Style="margin-top: -9px;">
            <asp:TextBox ID="txtValueTo" runat="server" CssClass="form-control  text-left"></asp:TextBox>
        </asp:Panel>
    </ControlTemplate>
</userControl:BaseControl>





