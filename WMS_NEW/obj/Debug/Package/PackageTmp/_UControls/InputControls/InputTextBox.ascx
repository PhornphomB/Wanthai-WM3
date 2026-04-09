<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InputTextBox.ascx.cs" Inherits="_UControls.InputTextBox" %>

<%@ Register Src="~/_UControls/InputControls/BaseInputControl.ascx" TagName="BaseControl" TagPrefix="userControl" %>

    <userControl:BaseControl ID="baseControl" runat="server">
        <ControlTemplate>
            <asp:TextBox ID="txtValue" runat="server" CssClass="form-control target"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqValidate" runat="server" SetFocusOnError="true" CssClass="isValidate">
            </asp:RequiredFieldValidator>

            <%--<asp:RegularExpressionValidator ID="reqValidateEmail" runat="server" ErrorMessage="format" SetFocusOnError="true" ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$">
            </asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="reqValidatePhone" runat="server" ErrorMessage="format" SetFocusOnError="true" ValidationExpression="^(?=.*\d)(?=.*\-).*$">
            </asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="reqValidateNumber" runat="server" ErrorMessage="require number" SetFocusOnError="true" ValidationExpression="^\d+$">
            </asp:RegularExpressionValidator>--%>
        </ControlTemplate>
    </userControl:BaseControl>
