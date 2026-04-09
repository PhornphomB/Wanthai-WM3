<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucResource.ascx.cs" Inherits="WMS_NEW.Authen.AscxControls.ucResource" %>

<ucControls:PanelPopupEntity ID="popup1" runat="server" ColumnControlFix="1">
    <ControlTemplate>
        <ucControls:PanelControlRow ID="pnItem" runat="server">
            <ucControls:InputHidden ID="hid_resource_ms_id" runat="server" DataFieldValue="resource_master_id" IsStaticValue="true" />
            <ucControls:InputDropDown ID="ddlLocal" runat="server" DataFieldValue="locale_id" ComboType="String" IsPrimary="true" DisplayDefault="--Select--" LabelText="Language" ResourceGroup="Inbound" ResourceName="" />
            <ucControls:InputTextBox ID="txtValue" runat="server" DataFieldValue="value" IsPrimary="true" LabelText="Value" ResourceGroup="Inbound" ResourceName="" />
        </ucControls:PanelControlRow>
    </ControlTemplate>
</ucControls:PanelPopupEntity>

<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="SecurityM.Access" SourceClassName="SecurityM.Access.Master.ResourceDetail" KeyField="KeyId" KeyType="String"
    GridAllowRowEdit="true" GridAllowRowDelete="true" DisableSearch="true" AutoSize="true">
    <CustomSearchTemplate>
        <ucControls:InputHidden ID="hid_grid_resource_ms_id" runat="server" DataFieldValue="_resource_master_id" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Language" DataField="name" AllowSort="true" ShowFilterNow="true" />
        <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Display" DataField="value" AllowSort="true" ShowFilterNow="true" />
        <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" HeaderText="Create By" DataField="create_by" AllowSort="true" />
        <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" HeaderText="Create Date" DataField="create_date" FormatType="DateTime" AllowSort="true" />
    </CustomColumnTemplate>
</ucControls:GridExt>
