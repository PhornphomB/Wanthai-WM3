<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucResourceDetail.ascx.cs" Inherits="WMS_NEW.Configuration.AscxControls.ucResourceDetail" %>


<ucControls:PanelPopupEntity ID="popupEntity1" runat="server" ColumnControlFix="2">
    <ControlTemplate>
        <ucControls:PanelControlRow ID="PanelControlRow0" runat="server">
            <ucControls:InputHidden ID="txtResourcemasterId" runat="server" DataFieldValue="resource_master_id" IsStaticValue="true" />

            <ucControls:InputTextBox ID="txtResourceGroup" runat="server" Enabled="false" ResourceGroup="ResourceMaster" ResourceName="resource_group" />
            <ucControls:InputTextBox ID="txtResourceName" runat="server" Enabled="false" ResourceGroup="ResourceMaster" ResourceName="resource_name" />

            <ucControls:InputDropDown runat="server" ResourceGroup="resource" ResourceName="locale" ID="ddlLocale" DataFieldValue="locale_id" ComboType="String" />
            <ucControls:InputTextBox ID="txtResourceValue" runat="server" DataFieldValue="value" IsPrimary="true" />

        </ucControls:PanelControlRow>
    </ControlTemplate>
</ucControls:PanelPopupEntity>

<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Configuration.ResourceDetail" KeyField="KeyId" KeyType="String"
    GridAllowRowEdit="true" GridAllowRowDelete="true" AutoGenerateColumn="true" ShowAllSort="true" DisableExport="true" DisableSearchAll="true" AutoSize="true" DisableFirstSearch="true" DisableSearch="true"
    GridSortDefault="locale" AutoGenColumnFields_Exclude="resource_master_id,locale_id">
    <CustomSearchTemplate>
        <ucControls:InputHidden ID="hid_resource_master_id" runat="server" DataFieldValue="resource_master_id" />
    </CustomSearchTemplate>
</ucControls:GridExt>
