<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PanelTab.ascx.cs" Inherits="_UControls.PanelTab" %>

<asp:UpdatePanel ID="updateContent" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
        <ul id="navUcPage" runat="server" class="nav nav-tabs" role="tablist" style="margin-top: 5px; margin-bottom: 20px;"></ul>
        <asp:PlaceHolder ID="PlaceHolderControl" runat="server"></asp:PlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>
