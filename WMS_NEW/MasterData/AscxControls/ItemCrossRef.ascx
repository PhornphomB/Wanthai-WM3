<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemCrossRef.ascx.cs" Inherits="WMS_NEW.MasterData.AscxControls.ItemCrossRef" %>

<ucControls:PanelPopupEntity ID="popupEntity1" runat="server" ColumnControlFix="2">
    <ControlTemplate>
        <ucControls:PanelControlRow ID="PanelControlRow0" runat="server">
            <ucControls:InputHidden ID="hidItemMasterId" runat="server" DataFieldValue="item_master_id" IsStaticValue="true" />
            <ucControls:InputTextBox ID="txtItemNumber" runat="server" DataFieldValue="item_number" IsStaticValue="true" Enabled="false" LabelText="Item ID" ResourceGroup="item" ResourceName="item_number" />
            <ucControls:InputTextBox ID="txtItemNumAlt" runat="server" DataFieldValue="alternate_item_number" IsPrimary="true" LabelText="Item Cross Ref" ResourceGroup="item_crossref" ResourceName="alternate_item_number" />
            <ucControls:InputDropDownHD ID="ddlUOM" runat="server" DataFieldValue="item_uom_id" ComboType="Guid" IsPrimary="true" DisplayDefault="--Select--" LabelText="UOM" ResourceGroup="item_uom" ResourceName="uom" />
            <ucControls:InputCheckBox ID="chkActive" runat="server" DataFieldValue="is_active" CheckBoxType="String" LabelText="Active" ResourceGroup="general" ResourceName="active" />
        </ucControls:PanelControlRow>
    </ControlTemplate>
</ucControls:PanelPopupEntity>

<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.ItemCrossRef" KeyField="KeyId" KeyType="Guid"
    GridAllowRowEdit="true" GridAllowRowDelete="true" ShowAllSort="true" DisableExport="true" DisableSearchAll="true" AutoSize="true" DisableFirstSearch="true"
    GridSortDefault="alternate_item_number" AutoGenColumnFields_Exclude="item_master_id">
    <CustomSearchTemplate>
        <ucControls:InputHidden ID="gridItemMasterId" runat="server" DataFieldValue="item_master_id" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Cross Refference" DataField="alternate_item_number" Width="150" ResourceGroup="item_crossref" ResourceName="alternate_item_number" />
        <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="UOM" DataField="uom" ResourceGroup="item_uom" ResourceName="uom" />
        <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Active" DataField="is_active" ResourceGroup="general" ResourceName="active" />
    </CustomColumnTemplate>
</ucControls:GridExt>
