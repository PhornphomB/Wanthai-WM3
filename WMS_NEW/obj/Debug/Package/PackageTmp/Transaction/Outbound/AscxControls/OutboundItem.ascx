<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutboundItem.ascx.cs" Inherits="WMS_NEW.Transaction.Outbound.AscxControls.OutboundItem" %>

<ucControls:PanelPopupEntity ID="popupEntity1" runat="server" ColumnControlFix="3">
    <ControlTemplate>
        <ucControls:PanelTab ID="PanelTab1" runat="server">
            <ControlTemplate>

                <ucControls:PanelControlTab ID="pnItem" runat="server" PanelName="Item" ResourceGroup="outbound_detail" ResourceName="tab_item">
                    <ucControls:PanelControlRow ID="PanelControlRow1" runat="server">
                        <ucControls:InputHidden ID="hidMasterId" runat="server" DataFieldValue="outbound_order_master_id" IsStaticValue="true" />
                        <ucControls:InputTextBox ID="txtOrderNumberShow" runat="server" Enabled="false" LabelText="Outbound Order No" ResourceGroup="outbound_master" ResourceName="outbound_order_number" Visible="false" />
                        <ucControls:InputTextBox ID="txtLineNumber" runat="server" DataFieldValue="line_number" Enabled="false" LabelText="Line Number" ResourceGroup="outbound_detail" ResourceName="line_number" />

                        <ucControls:InputDropDownHD ID="ddlItemNo" runat="server" DataFieldValue="wh_item_master_id" IsPrimary="true" IsKey="true" AutoPostBack="true" DisplayDefault="--Select--" LabelText="Item No" ResourceGroup="item" ResourceName="item_number" ColumnSpan="3" />
                        <ucControls:InputDropDownHD ID="ddlItemBom" runat="server" ResourceGroup="item" ResourceName="item_bom" DataFieldValue="bom_id" IsPrimary="true" DisplayDefault="--Select--" ComboType="Integer" ColumnSpan="3" />
                        <ucControls:InputCheckBox ID="chkItemBom" runat="server" ResourceGroup="inbound_detail" ResourceName="create_by_bom" LabelText="Create By Bom" CheckBoxType="Boolean" AutoPostBack="true" ColumnSpan="1" />

                        <ucControls:InputTextBox ID="txtdgCode" runat="server" Enabled="false" LabelText="DG Code" ResourceGroup="item" ResourceName="dg_code" />
                        <ucControls:InputTextBox ID="txtItemCrossRef" runat="server" Enabled="false" LabelText="Item Cross Reference" ResourceGroup="item_crossref" ResourceName="alternate_item_number" />
                        <ucControls:InputDropDown ID="ddlDefaultItemStatus" runat="server" DataFieldValue="default_item_status" ComboType="String" IsPrimary="true" LabelText="Default Item Status" ResourceGroup="outbound_detail" ResourceName="default_item_status" AutoPostBack="true" />
                        <ucControls:InputTextNumber ID="txtPrice" runat="server" DataFieldValue="price" LabelText="Price" ResourceGroup="item" ResourceName="price" />
                        <ucControls:InputTextNumber ID="txtQty" runat="server" DataFieldValue="quantity_order" IsPrimary="true" LabelText="Order Qty" ResourceGroup="outbound_detail" ResourceName="quantity_order" />


                        <ucControls:InputTextNumber ID="txtRemainQty" runat="server" Enabled="false" LabelText="Remain Qty" ResourceGroup="outbound_detail" ResourceName="quantity_remain" />
                        <ucControls:InputDropDown ID="ddlUOM" runat="server" DataFieldValue="item_uom_id" IsPrimary="true" AutoPostBack="true" LabelText="UOM" ResourceGroup="item_uom" ResourceName="uom" />
                        <%-- <asp:Panel ID="panelUOM" runat="server" CssClass="row col-lg-12 col-md-12 col-sm-12">

                        </asp:Panel>--%>
                        <ucControls:InputTextBox ID="txtQtyUnit" runat="server" DataFieldValue="" LabelText="Qty Unit" ResourceGroup="outbound_detail" ResourceName="qty_unit" ColumnSpan="2"/>
                        <ucControls:InputTextBox ID="txtPackSizeUom" runat="server" DataFieldValue="" LabelText="Base Unit" ResourceGroup="outbound_detail" ResourceName="base_unit" ColumnSpan="2"/>
                        <ucControls:InputTextBox ID="txtLpn" runat="server" DataFieldValue="lpn" LabelText="LPN" ResourceGroup="inventory" ResourceName="lpn" />
                        <ucControls:InputTextBox ID="txtLot" runat="server" DataFieldValue="lot_number" LabelText="Batch" ResourceGroup="inventory" ResourceName="lot_number" />
                        <ucControls:InputTextBox ID="txtSerial" runat="server" DataFieldValue="serial_number" LabelText="Serial Number" ResourceGroup="inventory" ResourceName="serial_number" />
                        <%--<ucControls:InputTextDate ID="dtpExpiry" runat="server" DataFieldValue="expiry_date" TextMode="Date" LabelText="ExpireDate" ResourceGroup="inventory" ResourceName="expiry_date" />--%>
                        <ucControls:InputTextDate ID="dtpExpiry" runat="server" DataFieldValue="xx" TextMode="Date" LabelText="ExpireDate" ResourceGroup="inventory" ResourceName="expiry_date" />

                        <ucControls:InputTextBox ID="txtItemDesc" runat="server" DataFieldValue="item_description" IsMultiLine="true" Enabled="true" LabelText="Item Description" ResourceGroup="item" ResourceName="description" />
                        <ucControls:InputTextBox ID="InputTextBox4" runat="server" DataFieldValue="customer_item_code" LabelText="Customer Item Code" ResourceGroup="outbound_detail" ResourceName="customer_item_code" />
                        <ucControls:InputTextBox ID="InputTextBox3" runat="server" DataFieldValue="dangerous_good_flag" LabelText="Dangerous Goods Flag" ResourceGroup="outbound_detail" ResourceName="dangerous_good_flag" />

                        <ucControls:InputDropDownHD ID="ddlLocation" runat="server" ResourceGroup="location" ResourceName="location" DataFieldValue="location_id"  DisplayDefault="--Select--" ComboType="Guid" AutoPostBack="true" />
                        <ucControls:InputTextBox ID="txtAttribute1" runat="server" DataFieldValue="attribute1" LabelText="Attribute1" Enabled="false" ResourceGroup="inventory" ResourceName="attribute1" />
                        <ucControls:InputTextBox ID="txtAttribute2" runat="server" DataFieldValue="attribute2" LabelText="Attribute2" ResourceGroup="inventory" ResourceName="attribute2" />
                        <ucControls:InputTextBox ID="txtAttribute3" runat="server" DataFieldValue="attribute3" LabelText="Attribute3" ResourceGroup="inventory" ResourceName="attribute3" />
                        <ucControls:InputTextBox ID="txtAttribute4" runat="server" DataFieldValue="attribute4" LabelText="Attribute4" ResourceGroup="inventory" ResourceName="attribute4" />
                        <ucControls:InputTextBox ID="txtAttribute5" runat="server" DataFieldValue="attribute5" LabelText="Attribute5" ResourceGroup="inventory" ResourceName="attribute5" />
                    </ucControls:PanelControlRow>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnUserDefine" runat="server" PanelName="User Defined Data" ResourceGroup="outbound_detail" ResourceName="tab_user_define">
                    <ucControls:PanelControlRow ID="PanelControlRow4" runat="server">
                        <ucControls:InputTextBox ID="InputTextBox2" runat="server" DataFieldValue="user_def1" ResourceGroup="outbound_detail" ResourceName="user_def1" />
                        <ucControls:InputTextBox ID="InputTextBox18" runat="server" DataFieldValue="user_def2" ResourceGroup="outbound_detail" ResourceName="user_def2" />
                        <ucControls:InputTextBox ID="InputTextBox19" runat="server" DataFieldValue="user_def3" ResourceGroup="outbound_detail" ResourceName="user_def3" />
                        <ucControls:InputTextBox ID="InputTextBox25" runat="server" DataFieldValue="user_def4" ResourceGroup="outbound_detail" ResourceName="user_def4" />
                        <ucControls:InputTextBox ID="txt_UDF5" runat="server" DataFieldValue="user_def5" ResourceGroup="outbound_detail" ResourceName="user_def5" />
                        <ucControls:InputDropDown ID="ddl_UDF5" runat="server" DataFieldValue="user_def5" ComboType="String" ResourceGroup="outbound_detail" ResourceName="user_def5" />
                        <ucControls:InputTextBox ID="InputTextBox27" runat="server" DataFieldValue="user_def6" ResourceGroup="outbound_detail" ResourceName="user_def6" />
                        <ucControls:InputTextNumber ID="InputTextBox20" runat="server" DataFieldValue="user_def7" ResourceGroup="outbound_detail" ResourceName="user_def7" />
                        <ucControls:InputTextNumber ID="InputTextBox21" runat="server" DataFieldValue="user_def8" ResourceGroup="outbound_detail" ResourceName="user_def8" />
                        <ucControls:InputTextDate ID="InputTextBox22" runat="server" DataFieldValue="user_def9" TextMode="Date" ResourceGroup="outbound_detail" ResourceName="user_def9" />
                        <ucControls:InputTextDate ID="InputTextBox23" runat="server" DataFieldValue="user_def10" TextMode="Date" ResourceGroup="outbound_detail" ResourceName="user_def10" />
                    </ucControls:PanelControlRow>
                </ucControls:PanelControlTab>

            </ControlTemplate>
        </ucControls:PanelTab>
    </ControlTemplate>
</ucControls:PanelPopupEntity>

<ucControls:PanelPopup ID="popupBomMaster" runat="server" HeaderText="Bom Master">
    <DataTemplate>
        <ucControls:PanelControlRow ID="PanelControlRow3" runat="server">
            <ucControls:InputTextBox ID="txtBomMsCode" runat="server" Readonly="true" Text="Bom Code" />
            <ucControls:InputTextNumber ID="txtBomMsQty" runat="server" NumberType="Double" IsPrimary="true" ValidateGroup="BomMaster" Text="Quantity" />
        </ucControls:PanelControlRow>

        <ucControls:PanelControlRow ID="panelBomDetail" runat="server">
            <fieldset>
                <legend>
                    <ucControls:LabelExt runat="server" Text="Items Set" />
                </legend>
                <ucControls:GridExt ID="gridBomDetail" runat="server"
                    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.Bom.BomDetail" KeyField="KeyId" KeyType="Guid" DisableFirstSearch="true"
                    AutoSize="true" GridAlign="Center" PageAlign="Right">
                    <CustomSearchTemplate>
                        <ucControls:InputHidden runat="server" ID="hdf_bom_outbound_order_detail_id" DataFieldValue="_outbound_order_detail_id" />
                    </CustomSearchTemplate>
                    <CustomColumnTemplate>
                        <ucControls:GridColumnExt ID="GridColumnExt26" runat="server" HeaderText="Item Number" DataField="item_number" />
                        <ucControls:GridColumnExt ID="GridColumnExt28" runat="server" HeaderText="UOM" DataField="uom" />
                        <ucControls:GridColumnExt ID="GridColumnExt29" runat="server" HeaderText="Bom Qty" DataField="bom_detail_quantity" />
                    </CustomColumnTemplate>
                </ucControls:GridExt>
            </fieldset>
        </ucControls:PanelControlRow>

    </DataTemplate>
    <CommandTemplate>
        <ucControls:ButtonExt ID="btBomMsQtyDelete" runat="server" Text="Delete Bom Set" CssClass="btn btn-danger" OnClick="btBomMsQtyDelete_Click" ValidateGroup="BomMaster" OnClientClick="if (!confirm('Do you want delete bom set ?')) return false;" />
        <ucControls:ButtonExt ID="btBomMsQtyUpdate" runat="server" Text="Update Qty" CssClass="btn btn-info" OnClick="btBomMsQtyUpdate_Click" ValidateGroup="BomMaster" OnClientClick="if (!confirm('Do you want update quantity ?')) return false;" />
    </CommandTemplate>
</ucControls:PanelPopup>


<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.OutboundDetail" KeyField="KeyId" KeyType="Guid"
    GridAllowRowEdit="true" GridAllowRowDelete="true" DisableExport="false" DisableSearch="true" AutoSize="true" DisableFirstSearch="true" GridSortDefault="line_number_int asc">
    <CustomCommandTemplate>
        <ucControls:ButtonExt ID="btRefresh" runat="server" LabelText="Refresh" CssClass="btn btn-sm btn-info" CausesValidation="false" OnClick="btRefresh_Click" ResourceGroup="general" ResourceName="btn_refresh" Visible="false" />
        <%--<ucControls:ButtonExt ID="btPrintAll" runat="server" LabelText="Print All" CssClass="btn btn-warning" CausesValidation="false" OnClick="btPrintAll_Click" Visible="false" />--%>
    </CustomCommandTemplate>
    <CustomSearchTemplate>
        <ucControls:InputHidden ID="hidOrderMasterId" runat="server" DataFieldValue="_outbound_order_master_id" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" HeaderText="" DataField="cmd_print" ControlType="CommandButton" CommandText="Print" CommandName="PRINT" IsConfirm="false" Visible="false" />
        <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Line Number" DataField="line_number" ResourceGroup="outbound_detail" ResourceName="line_number" FormatType="Integer" />
        <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Item Category" DataField="category_description" ResourceGroup="category" ResourceName="description" />
        <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Item Description" DataField="item_description" ResourceGroup="item" ResourceName="description" />
        <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Item No" DataField="item_number" ResourceGroup="item" ResourceName="item_number" />
        <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" HeaderText="Attribute1" DataField="attribute1" ResourceGroup="inventory" ResourceName="attribute1" />
        <ucControls:GridColumnExt ID="GridColumnExt27" runat="server" HeaderText="Bom Master" DataField="EditBOM" ControlType="CommandButton" CommandText="Bom Master" CommandName="EDITBOM" IsConfirm="false" ResourceGroup="item" ResourceName="bom" />
        <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Batch" DataField="lot_number" ResourceGroup="inventory" ResourceName="lot_number" />
        <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="LPN" DataField="lpn" ResourceGroup="inventory" ResourceName="lpn" />
        <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Expiry Date" DataField="expiry_date" FormatType="Date" ResourceGroup="inventory" ResourceName="expiry_date" />
        <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" HeaderText="Item Grade" DataField="grade" ResourceGroup="item" ResourceName="grade" />
        <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Price" DataField="price" ResourceGroup="item" ResourceName="price" />
        <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Alter Order Qty" DataField="alter_quantity_order" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="alter_quantity_order" />
        <ucControls:GridColumnExt ID="GridColumnExt24" runat="server" HeaderText="Alter Pick Qty" DataField="alter_quantity_pick" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="alter_quantity_pick" />
        <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="Alter Load Qty" DataField="alter_quantity_load" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="alter_quantity_load" />
        <ucControls:GridColumnExt ID="GridColumnExt30" runat="server" HeaderText="Alter Ship Qty" DataField="alter_quantity_ship" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="alter_quantity_ship" />
        <ucControls:GridColumnExt ID="GridColumnExt31" runat="server" HeaderText="Alter UOM" DataField="alter_uom" ResourceGroup="outbound_detail" ResourceName="alter_uom" />

        <ucControls:GridColumnExt ID="GridColumnExt18" runat="server" HeaderText="Attribute2" DataField="attribute2" ResourceGroup="inventory" ResourceName="attribute2" />
        <ucControls:GridColumnExt ID="GridColumnExt19" runat="server" HeaderText="Attribute3" DataField="attribute3" ResourceGroup="inventory" ResourceName="attribute3" />
        <ucControls:GridColumnExt ID="GridColumnExt20" runat="server" HeaderText="Attribute4" DataField="attribute4" ResourceGroup="inventory" ResourceName="attribute4" />
        <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" HeaderText="Attribute5" DataField="attribute5" ResourceGroup="inventory" ResourceName="attribute5" />

        <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Order Qty" DataField="quantity_order" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="quantity_order" />
        <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Pick Qty" DataField="quantity_pick" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="quantity_pick" />
        <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Load Qty" DataField="quantity_load" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="quantity_load" />
        <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Ship Qty" DataField="quantity_ship" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="quantity_ship" />
        <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="UOM" DataField="uom" ResourceGroup="item_uom" ResourceName="uom" />
        <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" HeaderText="Create By" DataField="create_by" ResourceGroup="general" ResourceName="create_by" />
        <ucControls:GridColumnExt ID="GridColumnExt17" runat="server" HeaderText="Create Date" DataField="create_date" FormatType="DateTime" ResourceGroup="general" ResourceName="create_date" />
    </CustomColumnTemplate>
</ucControls:GridExt>
