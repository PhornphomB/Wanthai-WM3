<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInboundCancelPalletDetail.ascx.cs" Inherits="WMS_NEW.Transaction.Inbound.AscxControl.ucInboundCancelPalletDetail" %>

<%@ Register Src="~/Report/AscxControls/ucReportViewer.ascx" TagPrefix="ucControls" TagName="ucReportViewer" %>

<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inbound.InboundCancelPalletDetail" KeyField="KeyId" KeyType="Guid"
    GridAllowRowEdit="false" GridAllowRowDelete="false" ShowAllSort="true" DisableExport="true" AutoSize="true" DisableFirstSearch="true"
    NewVisible="false" GridSortDefault="line_number asc,row_print asc" ShowAllFilter="true">
    <CustomSearchTemplate>
        <ucControls:InputHidden ID="gridInboundOrderMasterId" runat="server" DataFieldValue="_inbound_order_master_id" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt runat="server" HeaderText="Action" ID="gcPrint" DataField="print_label_id" CommandName="CancelBtn" IsConfirm="true" ControlType="CommandButton" CommandText="Cancelled" AllowFilter="false" AllowSort="false" ShowFilterNow="false" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_master" HeaderText="Production Line" ResourceName="production_line" ID="iColProductionLine" DataField="production_line" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownFilterType="LazySearch" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="LPN" ResourceName="lpn" ID="iColLPN" DataField="lpn" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="Is Print" ID="iColIsPrint" ResourceName="is_print" DataField="is_print" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item" HeaderText="Item Number" ResourceName="item_number" ID="iColItemNumber" DataField="item_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item" HeaderText="Description" ResourceName="description" DataField="item_description" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="print_label" HeaderText="print_date" ResourceName="print_date" DataField="print_date" AllowFilter="true" AllowSort="true" FormatType="Time" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="Lot Number" ResourceName="lot_number" DataField="lot_number" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="MFG Date" ResourceName="mfg_date" DataField="mfg_date" FormatType="Date" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="Expiry Date" ResourceName="expiry_date" DataField="expiry_date" FormatType="Date" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="Attribute 1" ResourceName="attribute1" DataField="attribute1" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="print_label" HeaderText="Pack Size/Pallet" ResourceName="pack_qty_pallet" DataField="pack_size_per_pallet" FormatType="Number" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item_uom" HeaderText="Pack Size Conversion" ResourceName="pack_size_conversion_factor" DataField="pack_size_conversion_factor" FormatType="Number" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item_uom" HeaderText="Pack Size UOM" ResourceName="item_uom_id" DataField="pack_size_uom" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item_uom" HeaderText="Pallet Size Conversion" ResourceName="pallet_size_conversion_factor" DataField="pallet_size_conversion_factor" FormatType="Number" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item_uom" HeaderText="Pallet Size UOM" ResourceName="pallet_uom" DataField="pallet_size_uom" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="Is Cancel" ID="iColIsCancel" ResourceName="is_cancelled" DataField="is_cancelled" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />

    </CustomColumnTemplate>
</ucControls:GridExt>