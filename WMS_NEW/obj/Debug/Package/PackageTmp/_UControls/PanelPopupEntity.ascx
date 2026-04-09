<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PanelPopupEntity.ascx.cs" Inherits="_UControls.PanelPopupEntity" %>

<%@ Register Src="~/_UControls/PanelPopup.ascx" TagName="PanelPopup" TagPrefix="userControl" %>
<%@ Register Src="~/_UControls/ButtonExt.ascx" TagName="ButtonExt" TagPrefix="userControl" %>

<userControl:PanelPopup ID="PanelPopup1" runat="server">
    <DataTemplate>
        <div style="margin-top:5px;">
            <asp:PlaceHolder ID="PlaceHolderControl" runat="server"></asp:PlaceHolder>
        </div>
    </DataTemplate>
    <CommandTemplate>
        <div>
            <div class="pull-left">
                <asp:PlaceHolder ID="PlaceCustomCommand" runat="server"></asp:PlaceHolder>
            </div>
            <div class="pull-right">
                <span id="spanSave" runat="server">
                    <userControl:ButtonExt ID="btSave" runat="server" Text="Save" CssClass="btn btn-primary"
                        OnClick="btSave_Click" />
                </span>
                <span id="spanClear" runat="server">
                    <userControl:ButtonExt ID="btClear" runat="server" Text="Clear" CssClass="btn btn-warning"
                        OnClick="btClear_Click" CausesValidation="false" OnClientClick="if (!confirm('Are you sure ?')) return false;" />
                </span>
            </div>
        </div>
    </CommandTemplate>
</userControl:PanelPopup>
