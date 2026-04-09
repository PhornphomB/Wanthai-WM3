<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLoading.ascx.cs" Inherits="MK.GUIWeb._UControls.ucLoading" %>

<asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <ProgressTemplate>
        <div class="content-spinner">
        </div>
        <div class="spinner">
            <div class="rect1"></div>
            <div class="rect2"></div>
            <div class="rect3"></div>
            <div class="rect4"></div>
            <div class="rect5"></div>
        </div>

    </ProgressTemplate>
</asp:UpdateProgress>
