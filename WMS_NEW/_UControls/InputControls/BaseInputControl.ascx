<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BaseInputControl.ascx.cs" Inherits="_UControls.BaseInputControl" %>

<%@ Register Src="~/_UControls/InputControls/FilterStip.ascx" TagName="FilterStip" TagPrefix="userControl" %>

<asp:UpdatePanel ID="updateBaseContent" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
    <ContentTemplate>
        <asp:Panel ID="panelBaseContent" runat="server">
            <asp:UpdatePanel ID="updateContent" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>

                    <asp:Panel ID="panelLabel" runat="server">
                        <ucControls:LabelExt runat="server" ID="lblText" />
                    </asp:Panel>

                    <asp:Panel ID="panelInput" runat="server" CssClass="input-container">
                        <asp:Panel ID="panelFilter" runat="server">
                            <userControl:FilterStip ID="filterStrip" runat="server" />
                        </asp:Panel>
                        <asp:Panel ID="panelControl" runat="server" Style="width: 100%;">
                            <asp:PlaceHolder ID="placeHolderControl" runat="server"></asp:PlaceHolder>
                        </asp:Panel>
                    </asp:Panel>

                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

