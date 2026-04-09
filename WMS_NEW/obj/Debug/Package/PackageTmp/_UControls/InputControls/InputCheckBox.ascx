<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InputCheckBox.ascx.cs" Inherits="_UControls.InputCheckBox" %>

<%@ Register Src="~/_UControls/InputControls/BaseInputControl.ascx" TagName="BaseControl" TagPrefix="userControl" %>

    <userControl:BaseControl ID="baseControl" runat="server">
        <ControlTemplate>
            <label class="switch switch-lg switch-text switch-success"">
                <asp:CheckBox ID="chkValue" runat="server" />
                <span id="labelSwitch" runat="server" class="switch-label" data-on="YES" data-off="NO"></span>
                <span id="labelHandle" runat="server" class="switch-handle"></span>
            </label>
        </ControlTemplate>
    </userControl:BaseControl>
