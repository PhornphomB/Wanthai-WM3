<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InputDropDownHD.ascx.cs" Inherits="_UControls.InputDropDownHD" %>

<%@ Register Src="~/_UControls/InputControls/BaseInputControl.ascx" TagName="BaseControl" TagPrefix="userControl" %>
<%@ Register Src="~/_UControls/CallBackHandler.ascx" TagPrefix="userControls" TagName="CallBackHandler" %>


    <userControls:CallBackHandler runat="server" ID="CallBackHandler1" />

    <userControl:BaseControl ID="baseControl" runat="server">
        <ControlTemplate>

            <asp:TextBox ID="txtValue" CssClass="form-control" runat="server" Style="display: none;"  ></asp:TextBox>
            <asp:HiddenField ID="comboValue" runat="server" />
            <asp:HiddenField ID="hidDisplay" runat="server" />
            <asp:RequiredFieldValidator ID="reqValidate" runat="server" SetFocusOnError="true"></asp:RequiredFieldValidator>
            <asp:HiddenField ID="panelData" runat="server" />
            <asp:LinkButton ID="linkTrigger" runat="server" Text="Trigger" Style="display: none;"></asp:LinkButton>
        </ControlTemplate>
    </userControl:BaseControl>

