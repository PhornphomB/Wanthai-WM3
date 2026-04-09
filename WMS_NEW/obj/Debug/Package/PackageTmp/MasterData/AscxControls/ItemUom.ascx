<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemUom.ascx.cs" Inherits="WMS_NEW.MasterData.AscxControls.ItemUom" %>

<ucControls:PanelPopupEntity ID="popupEntity1" runat="server" ColumnControlFix="3">
    <ControlTemplate>
        <ucControls:PanelControlRow ID="PanelControlRow0" runat="server">
            <ucControls:InputHidden ID="hidItemMasterId" runat="server" DataFieldValue="item_master_id" IsStaticValue="true" />
            <ucControls:InputTextBox ID="txtItemNumber" runat="server" DataFieldValue="item_number" IsStaticValue="true" Enabled="false" LabelText="Item ID" ResourceGroup="item" ResourceName="item_number" />
            <ucControls:InputTextBox ID="InputTextBox2" runat="server" DataFieldValue="uom" IsPrimary="true" IsKey="true" LabelText="UOM" ResourceGroup="item_uom" ResourceName="uom" />
            <ucControls:InputCheckBox ID="chkActive" runat="server" DataFieldValue="is_active" CheckBoxType="String" LabelText="Active" ResourceGroup="general" ResourceName="active" />
        </ucControls:PanelControlRow>

        <ucControls:PanelTab ID="PanelTab1" runat="server">
            <ControlTemplate>
                <ucControls:PanelControlTab ID="PanelControlTab1" runat="server" PanelName="General" ResourceGroup="item_uom" ResourceName="tab_general">
                    <ucControls:PanelControlRow ID="PanelControlRow1" runat="server">
                        <ucControls:InputTextNumber ID="txtConversion" runat="server" DataFieldValue="conversion_factor" IsPrimary="true" LabelText="Conversion Factor" ResourceGroup="item_uom" ResourceName="conversion_factor" />
                        <ucControls:InputTextInteger ID="InputTextNumber9" runat="server" DataFieldValue="sequence" IsPrimary="true" MaxLength="2" ResourceGroup="item_uom" ResourceName="sequence" />
                        <ucControls:InputDropDown ID="ddPickRule" runat="server" DataFieldValue="picking_class" IsPrimary="true" ComboType="String" ResourceGroup="item_uom" ResourceName="picking_class" />
                        <ucControls:InputDropDown ID="ddPutRule" runat="server" DataFieldValue="putaway_class" IsPrimary="true" ComboType="String" ResourceGroup="item_uom" ResourceName="putaway_class" />
                        <ucControls:InputTextBox ID="txtuom_prompt" runat="server" DataFieldValue="uom_prompt" IsPrimary="true" LabelText="UOM Prompt" ResourceGroup="item_uom" ResourceName="uom_prompt" />
                        <ucControls:InputTextBox ID="InputTextBox5" runat="server" DataFieldValue="gtin" ResourceGroup="item_uom" ResourceName="gtin" />
                        <ucControls:InputCheckBox ID="chkPrimaryUOM" runat="server" DataFieldValue="primary_uom" CheckBoxType="String" AutoPostBack="true" LabelText="Primary UOM" ResourceGroup="item_uom" ResourceName="primary_uom" />
                        <ucControls:InputCheckBox ID="chkPickingUOM" runat="server" DataFieldValue="picking_uom" CheckBoxType="String" LabelText="Picking UOM" ResourceGroup="item_uom" ResourceName="picking_uom" />
                        <ucControls:InputCheckBox ID="chkShippingUOM" runat="server" DataFieldValue="shipping_uom" CheckBoxType="String" LabelText="Shipping UOM" ResourceGroup="item_uom" ResourceName="shipping_uom"/>
                        <ucControls:InputCheckBox ID="chkIsPackSizeUOM" runat="server" DataFieldValue="is_pack_size_uom" CheckBoxType="String" LabelText="Pack Size" ResourceGroup="item_uom" ResourceName="is_pack_size_uom"/>
                        <ucControls:InputCheckBox ID="chkIsPalletUOM" runat="server" DataFieldValue="is_pallet_uom" CheckBoxType="String" LabelText="Pallet" ResourceGroup="item_uom" ResourceName="is_pallet_uom"/>
                        <ucControls:InputCheckBox ID="chkIsFullPalletUOM" runat="server" DataFieldValue="is_full_pallet_uom" CheckBoxType="String" LabelText="Full Pallet" ResourceGroup="item_uom" ResourceName="is_full_pallet_uom"/>
                    </ucControls:PanelControlRow>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="PanelControlTab2" runat="server" PanelName="Dimensions" ResourceGroup="item_uom" ResourceName="tab_dimension">
                    <ucControls:PanelControlRow ID="PanelControlRow2" runat="server">
                        <ucControls:InputTextNumber ID="InputTextNumber1" runat="server" DataFieldValue="length" LabelText="Length" ResourceGroup="item_uom" ResourceName="length" />
                        <ucControls:InputTextNumber ID="InputTextNumber2" runat="server" DataFieldValue="width" LabelText="Width" ResourceGroup="item_uom" ResourceName="width" />
                        <ucControls:InputTextNumber ID="InputTextNumber3" runat="server" DataFieldValue="height" LabelText="Height" ResourceGroup="item_uom" ResourceName="height" />
                        <ucControls:InputTextNumber ID="InputTextNumber4" runat="server" DataFieldValue="weight" LabelText="Net Weight" ResourceGroup="item_uom" ResourceName="weight" />
                        <ucControls:InputTextNumber ID="InputTextNumber5" runat="server" DataFieldValue="gross_weight" LabelText="Gross Weight" ResourceGroup="item_uom" ResourceName="gross_weight" />
                        <ucControls:InputTextNumber ID="InputTextNumber6" runat="server" DataFieldValue="tare_weight" LabelText="Tare Weight" ResourceGroup="item_uom" ResourceName="tare_weight" />
                    </ucControls:PanelControlRow>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="PanelControlTab3" runat="server" PanelName="TI HI" ResourceGroup="item_uom" ResourceName="tab_tihi">
                    <ucControls:PanelControlRow ID="PanelControlRow3" runat="server">
                        <ucControls:InputTextNumber ID="InputTextNumber7" runat="server" DataFieldValue="ti_hi_unit_per_layer" LabelText="Ti Hi Per Layer" ResourceGroup="item_uom" ResourceName="ti_hi_unit_per_layer" />
                        <ucControls:InputTextNumber ID="InputTextNumber8" runat="server" DataFieldValue="ti_hi_layers_per_uom" LabelText="Ti Hi Layers Per UOM" ResourceGroup="item_uom" ResourceName="ti_hi_layers_per_uom" />
                        <ucControls:InputCheckBox ID="chkDisplayTIHI" runat="server" DataFieldValue="display_ti_hi" CheckBoxType="String" LabelText="Display Ti Hi" ResourceGroup="item_uom" ResourceName="display_ti_hi" />
                    </ucControls:PanelControlRow>
                </ucControls:PanelControlTab>
            </ControlTemplate>
        </ucControls:PanelTab>

    </ControlTemplate>
</ucControls:PanelPopupEntity>

<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.ItemUom" KeyField="KeyId" KeyType="Guid"
    GridAllowRowEdit="true" GridAllowRowDelete="true" ShowAllSort="true" DisableExport="true" DisableSearchAll="true" AutoSize="true" DisableFirstSearch="true"
    GridSortDefault="sequence asc" AutoGenColumnFields_Exclude="item_master_id">
    <CustomSearchTemplate>
        <ucControls:InputHidden ID="gridItemMasterId" runat="server" DataFieldValue="item_master_id" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="UOM" DataField="uom" ResourceGroup="item_uom" ResourceName="uom" />
        <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="UOM Prompt" DataField="uom_prompt" ResourceGroup="item_uom" ResourceName="uom_prompt" />
        <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Conversion Factor" DataField="conversion_factor" ResourceGroup="item_uom" ResourceName="conversion_factor" />
        <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Sequence" DataField="sequence" ResourceGroup="item_uom" ResourceName="sequence" />
        <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="Primary UOM" DataField="primary_uom" ResourceGroup="item_uom" ResourceName="primary_uom" />
        <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Picking UOM" DataField="picking_uom" ResourceGroup="item_uom" ResourceName="picking_uom" />
        <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Shipping UOM" DataField="shipping_uom" ResourceGroup="item_uom" ResourceName="shipping_uom" />
        <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Pack Size" DataField="is_pack_size_uom" ResourceGroup="item_uom" ResourceName="is_pack_size_uom" />
        <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Pallet" DataField="is_pallet_uom" ResourceGroup="item_uom" ResourceName="is_pallet_uom" />
        <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Full Pallet" DataField="is_full_pallet_uom" ResourceGroup="item_uom" ResourceName="is_full_pallet_uom" />
        <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Active" DataField="is_active" ResourceGroup="general" ResourceName="active" />
    </CustomColumnTemplate>
</ucControls:GridExt>
