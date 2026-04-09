<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InputDropDown.ascx.cs" Inherits="_UControls.InputDropDown" %>

<%@ Register Src="~/_UControls/InputControls/BaseInputControl.ascx" TagName="BaseControl" TagPrefix="userControl" %>

    <userControl:BaseControl ID="baseControl" runat="server">
        <ControlTemplate>
            <asp:DropDownList ID="comboValue" runat="server" CssClass="form-control">
            </asp:DropDownList>
            <asp:HiddenField ID="hidMultiValue" runat="server" />
            <asp:RequiredFieldValidator ID="reqValidate" runat="server" SetFocusOnError="true"></asp:RequiredFieldValidator>
            <asp:LinkButton ID="linkTrigger" runat="server" Text="Trigger" Style="display: none;"></asp:LinkButton>
        </ControlTemplate>
    </userControl:BaseControl>
