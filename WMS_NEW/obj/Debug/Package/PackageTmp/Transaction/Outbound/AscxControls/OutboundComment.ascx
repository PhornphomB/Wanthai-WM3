<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutboundComment.ascx.cs" Inherits="WMS_NEW.Transaction.Outbound.AscxControls.OutboundComment" %>

<ucControls:PanelPopupEntity ID="popupEntity1" runat="server" ColumnControlFix="2">
    <ControlTemplate>
        <ucControls:PanelControlRow ID="PanelControlRow0" runat="server">
            <ucControls:InputHidden ID="hidMasterId" runat="server" DataFieldValue="outbound_order_master_id" IsStaticValue="true" />
            <ucControls:InputDropDown ID="ddlCommentType" runat="server" DataFieldValue="comment_type" IsPrimary="true" DisplayDefault="--Select--" ResourceGroup="outbound_comment" ResourceName="comment_type" />
            <ucControls:InputDropDown ID="ddlPosition" runat="server" DataFieldValue="comment_postion" IsPrimary="true" DisplayDefault="--Select--" ResourceGroup="outbound_comment" ResourceName="comment_postion"/>
            <ucControls:InputTextBox ID="txtComment" runat="server" DataFieldValue="comment" IsPrimary="true" IsMultiLine="true" ResourceGroup="outbound_comment" ResourceName="comment"/>
            <ucControls:InputTextInteger ID="txtSequence" runat="server" DataFieldValue="sequence" IsPrimary="true" ResourceGroup="outbound_comment" ResourceName="sequence"/>
        </ucControls:PanelControlRow>
    </ControlTemplate>
</ucControls:PanelPopupEntity>

<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.OutboundComment" KeyField="KeyId" KeyType="Guid"
    GridAllowRowEdit="true" GridAllowRowDelete="true" DisableExport="true" DisableSearchAll="true" AutoSize="true" DisableFirstSearch="true">
    <CustomSearchTemplate>
        <ucControls:InputHidden runat="server" ID="grid_outbound_order_master_id" DataFieldValue="_outbound_order_master_id" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Sequence" DataField="sequence" ResourceGroup="outbound_comment" ResourceName="sequence" />
        <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Comment Type" DataField="display_member" ResourceGroup="outbound_comment" ResourceName="comment_type" />
        <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Comment" DataField="comment" ResourceGroup="outbound_comment" ResourceName="comment" />
        <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Position" DataField="comment_postion" ResourceGroup="outbound_comment" ResourceName="comment_postion" />
        <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Create By" DataField="create_by" ResourceGroup="general" ResourceName="create_by" />
        <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Create Date" DataField="create_date" FormatType="DateTime" ResourceGroup="general" ResourceName="create_date" />
    </CustomColumnTemplate>
</ucControls:GridExt>
