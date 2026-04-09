<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInboundDetail.ascx.cs" Inherits="WMS_NEW.Administrator.AscxControls.InterfaceMonitor.ucInboundDetail" %>

<ucControls:PanelControlRow ID="panelHeader" runat="server" CssClass="row col-sm-12 col-md-12 col-lg-12 ml-0 mr-0 mb-2">
    <ucControls:InputTextBox ID="txtRecordType" runat="server" Enabled="false" LabelText="RecordType" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtProcessingCode" runat="server" Enabled="false" LabelText="Processing Code" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtProcessingStatus" runat="server" Enabled="false" LabelText="Processing Status" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtWarehouse" runat="server" Enabled="false" LabelText="Warehouse" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtOwner" runat="server" DataFieldValue="" Enabled="false" LabelText="Owner" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtOrderType" runat="server" Enabled="false" LabelText="Order Type" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtOrderNumber" runat="server" Enabled="false" LabelText="Order Number" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextDate ID="txtExpectDate" runat="server" Enabled="false" TextMode="Date" LabelText="Expect Date" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtOrderStatus" runat="server" Enabled="false" LabelText="Order Status" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtSupplier" runat="server" Enabled="false" LabelText="Supplier" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtCustomer" runat="server" Enabled="false" LabelText="Customer" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtCreateBy" runat="server" Enabled="false" LabelText="Create By" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextDate ID="txtCreateDate" runat="server" Enabled="false" TextMode="Date" LabelText="Create Date" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
</ucControls:PanelControlRow>

<ucControls:GridExt ID="gridInboundDetail" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Administrator.InterfaceMonitors.InboundDetail" KeyField="KeyID" KeyType="String"
    DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true">
    <CustomCommandTemplate>
    </CustomCommandTemplate>
    <CustomSearchTemplate>
        <ucControls:InputHidden ID="hidInboundID" runat="server" DataFieldValue="host_record_id" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Inbound Order ID" DataField="KeyID" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt117" runat="server" HeaderText="Record Type" DataField="record_type" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt118" runat="server" HeaderText="Processing Code" DataField="processing_code" AllowSort="true" ResourceGroup="Location" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt119" runat="server" HeaderText="Processing Status" DataField="processing_status" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt120" runat="server" HeaderText="Warehouse" DataField="wh_id" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt121" runat="server" HeaderText="Owner" DataField="owner_id" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt122" runat="server" HeaderText="Order Number" DataField="order_number" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Line Number" DataField="line_number" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Item Number" DataField="item_number" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Order Quantity" DataField="order_quantity" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Lot" DataField="lot_number" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="LPN" DataField="lpn" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt123" runat="server" HeaderText="Item Status" DataField="default_item_status" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt124" runat="server" HeaderText="Over Receipt Percentage" DataField="over_receipt_percentage" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt125" runat="server" HeaderText="Supplier Item" DataField="supplier_item_number" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt126" runat="server" HeaderText="Is Used" DataField="is_used" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt127" runat="server" HeaderText="Price" DataField="price" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt129" runat="server" HeaderText="UDF 1" DataField="user_def1" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt130" runat="server" HeaderText="UDF 2" DataField="user_def2" AllowSort="true" ResourceGroup="ItemMaster" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt131" runat="server" HeaderText="UDF 3" DataField="user_def3" AllowSort="true" ResourceGroup="ItemMaster" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt132" runat="server" HeaderText="UDF 4" DataField="user_def4" AllowSort="true" ResourceGroup="Category" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt133" runat="server" HeaderText="UDF 5" DataField="user_def5" AllowSort="true" ResourceGroup="ItemMaster" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt134" runat="server" HeaderText="UDF 6" DataField="user_def6" AllowSort="true" ResourceGroup="Inbound" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt135" runat="server" HeaderText="UDF 7" DataField="user_def7" AllowSort="true" ResourceGroup="ItemMaster" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt136" runat="server" HeaderText="UDF 8" DataField="user_def8" AllowSort="true" ResourceGroup="ItemMaster" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt137" runat="server" HeaderText="UDF 9" DataField="user_def9" AllowSort="true" ResourceGroup="ItemMaster" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt138" runat="server" HeaderText="UDF 10" DataField="user_def10" AllowSort="true" ResourceGroup="ItemMaster" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt139" runat="server" HeaderText="Insert Date" DataField="insert_date" FormatType="Date" AllowSort="true" ResourceGroup="ItemMaster" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt140" runat="server" HeaderText="Create Date" DataField="create_date" FormatType="Date" AllowSort="true" ResourceGroup="ItemMaster" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt141" runat="server" HeaderText="Create By" DataField="create_by" AllowSort="true" ResourceGroup="ItemMaster" ResourceName="" />
    </CustomColumnTemplate>
</ucControls:GridExt>
