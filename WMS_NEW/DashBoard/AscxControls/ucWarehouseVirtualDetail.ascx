<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucWarehouseVirtualDetail.ascx.cs" Inherits="WMS_NEW.DashBoard.AscxControls.ucWarehouseVirtualDetail" %>

<div class="row">
    <div class="col-sm-6">
        <ucControls:InputTextBox runat="server" ResourceGroup="location" ResourceName="location" ID="txtLocation" Enabled="false" />
        <ucControls:InputTextNumber runat="server" ResourceGroup="location" ResourceName="capacity_qty" ID="txtCapacityQty" Enabled="false" />
    </div>
    <div class="col-sm-6">
        <ucControls:InputTextNumber runat="server" ResourceGroup="location" ResourceName="current_qty" ID="txtCurrentQty" Enabled="false" />
        <ucControls:InputTextNumber runat="server" ResourceGroup="location" ResourceName="usage_qty" ID="txtUsageQty" Enabled="false" />
    </div>
</div>

<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.DashBoard.WarehouseVirtualDetail" KeyField="KeyId" KeyType="String"
     DisableSearch="true" DisableFirstSearch="true" AutoSize="true" DisableExport="true">
    <CustomCommandTemplate>
    </CustomCommandTemplate>
    <CustomSearchTemplate>
        <ucControls:InputHidden runat="server" ID="hidLocationId" DataFieldValue="_location_id" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="item_number" ID="GridColumnExt1" DataField="item_number" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="description" ID="GridColumnExt2" DataField="description" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lot_number" ID="GridColumnExt3" DataField="lot_number" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="expiry_date" ID="GridColumnExt4" DataField="expiry_date" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="serial_number" ID="GridColumnExt5" DataField="serial_number" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="quantity" ID="GridColumnExt6" DataField="quantity" FormatType="Number" />
    </CustomColumnTemplate>
</ucControls:GridExt>
