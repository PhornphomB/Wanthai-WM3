<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReceiptDetail.ascx.cs" Inherits="WMS_NEW.Transaction.Inbound.AscxControl.ucReceiptDetail" %>

<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="WMS_NEW.Access.Transaction" SourceClassName="WMS_NEW.Access.Transaction.Inbound.Receipt.ReceiptDetail" KeyField="KeyId" KeyType="String"
    GridAllowRowEdit="false" GridAllowRowDelete="false" DisableExport="true" DisableSearch="true" AutoSize="true" DisableFirstSearch="true" GridSortDefault="line_number,lpn">
    <CustomCommandTemplate>
        <ucControls:ButtonExt ID="btRefresh" runat="server" Text="Refresh" CssClass="btn btn-sm btn-info" CausesValidation="false" OnClick="btRefresh_Click" ResourceGroup="General" ResourceName="Refresh" Visible="false" />
    </CustomCommandTemplate>
    <CustomSearchTemplate>
        <ucControls:InputHidden runat="server" ID="hdf_inbound_order_master_id" DataFieldValue="_inbound_order_master_id" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="line_number" DataField="line_number" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="category" ResourceName="description" DataField="category_description" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="description" DataField="item_description" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="item_number" DataField="item_number" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lot_number" DataField="lot_number" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="serial_number" DataField="serial_number" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="expiry_date" DataField="expiry_date" AllowSort="true" FormatType="Date" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="receive_date" DataField="receive_date" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="receipt_detail" ResourceName="location_received" DataField="location_received" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="print_label" ResourceName="pallet_seq" DataField="pallet_seq" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lpn" DataField="lpn" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_detail" ResourceName="quantity_order" DataField="quantity_order" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_detail" ResourceName="quantity_received" DataField="quantity_received" AllowSort="false" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item_uom" ResourceName="uom" DataField="uom" AllowSort="false" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_detail" ResourceName="over_receipt_allowed" DataField="over_receipt_allowed" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="attribute1" DataField="attribute1" AllowSort="true" />
        <%--<ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="attribute2" DataField="attribute2" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="attribute3" DataField="attribute3" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="attribute4" DataField="attribute4" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="attribute5" DataField="attribute5" AllowSort="true" />--%>
        <ucControls:GridColumnExt runat="server" ResourceGroup="receipt_detail" ResourceName="receive_by" DataField="receive_by" AllowSort="true" />
    </CustomColumnTemplate>
</ucControls:GridExt>
