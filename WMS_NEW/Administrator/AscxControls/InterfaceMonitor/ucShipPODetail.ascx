<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucShipPODetail.ascx.cs" Inherits="WMS_NEW.Administrator.AscxControls.InterfaceMonitor.ucShipPODetail" %>


<ucControls:GridExt ID="gridDetail" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Administrator.InterfaceMonitor.ShipPODetail" KeyField="KeyID" KeyType="String"
    DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true">
    <CustomCommandTemplate>
    </CustomCommandTemplate>
    <CustomSearchTemplate>
        <ucControls:InputHidden ID="hid_wmsorn" runat="server" DataFieldValue="wmsorn" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Item Number" DataField="item_number" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="item_number" />
        <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Item Description" DataField="item_description" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="item_description" />
        <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Request Del Date" DataField="request_del_date" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="request_del_date" />
        <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Quantity" DataField="quantity" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="quantity" />
        <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="uom" />
        <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Sales Price" DataField="sales_price" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="sales_price" />
        <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Sales Price UOM" DataField="sales_price_uom" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="sales_price_uom" />
        <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="Line WMS Create Date" DataField="line_wms_create_date" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="line_wms_create_date" />
        <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Line WMS Create Time" DataField="line_wms_create_time" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="line_wms_create_time" />
        <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Line M3 Get Status" DataField="line_m3_get_status" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="line_m3_get_status" />
        <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Line M3 Ref. No" DataField="line_m3_reference_no" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="line_m3_reference_no" />
        <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Line M3 Interface Date" DataField="line_m3_interface_date" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="line_m3_interface_date" />
        <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" HeaderText="Line M3 Interface Time" DataField="line_m3_interface_time" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="line_m3_interface_time" />
        <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" HeaderText="Line M3 Interface Status" DataField="line_m3_interface_status" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="line_m3_interface_status" />
    </CustomColumnTemplate>
</ucControls:GridExt>
