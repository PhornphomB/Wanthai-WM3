<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucM3ReceiptDetail.ascx.cs" Inherits="WMS_NEW.Administrator.AscxControls.InterfaceMonitor.ucM3ReceiptDetail" %>

<ucControls:GridExt ID="gridDetail" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Administrator.InterfaceMonitor.M3ReceiptDetail" KeyField="KeyID" KeyType="String"
    DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true">
    <CustomCommandTemplate>
    </CustomCommandTemplate>
    <CustomSearchTemplate>
        <ucControls:InputHidden ID="hid_wmsorn" runat="server" DataFieldValue="wmsorn" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt ID="GridColumnExt184" runat="server" HeaderText="WMSORN" DataField="KeyID" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="WMSORN" AllowFilter="true" />
        <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Warehouse" DataField="warehouse" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="warehouse" />
        <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Item Number" DataField="item_number" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="item_number" />
        <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Location" DataField="location" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="location" />
        <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Lot" DataField="lot_number" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="lot_number" />
        <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Allocate Qty" DataField="allocate_qty" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="allocate_qty" />
        <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Delivery Qty" DataField="delivery_qty" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="delivery_qty" />
        <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="User Id" DataField="user_id" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="user_id" />
        <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Report Date" DataField="report_date" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="report_date" />
        <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Report Time" DataField="report_time" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="report_time" />
        <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="Line WMS Create Date" DataField="line_wms_create_date" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="line_wms_create_date" />
        <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Line WMS Create Time" DataField="line_wms_create_time" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="line_wms_create_time" />
        <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Line M3 Get Status" DataField="line_m3_get_status" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="line_m3_get_status" />
        <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Line M3 Ref. No" DataField="line_m3_reference_no" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="line_m3_reference_no" />
        <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Line M3 Interface Date" DataField="line_m3_interface_date" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="line_m3_interface_date" />
        <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" HeaderText="Line M3 Interface Time" DataField="line_m3_interface_time" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="line_m3_interface_time" />
        <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" HeaderText="Line M3 Interface Status" DataField="line_m3_interface_status" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="line_m3_interface_status" />
    </CustomColumnTemplate>
</ucControls:GridExt>
