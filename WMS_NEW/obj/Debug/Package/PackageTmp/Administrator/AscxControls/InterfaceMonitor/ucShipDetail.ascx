<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucShipDetail.ascx.cs" Inherits="WMS_NEW.Administrator.AscxControls.InterfaceMonitor.ucShipDetail" %>

<ucControls:PanelControlRow ID="panelHeader" runat="server" CssClass="row col-sm-12 col-md-12 col-lg-12 ml-0 mr-0 mb-2">
    <ucControls:InputTextBox ID="txtProcessingCode" runat="server" Enabled="false" LabelText="Processing Code" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtProcessingStatus" runat="server" Enabled="false" LabelText="Processing Status" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtWarehouse" runat="server" Enabled="false" LabelText="Warehouse" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtOwner" runat="server" DataFieldValue="" Enabled="false" LabelText="Owner" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtOrderNumber" runat="server" Enabled="false" LabelText="Order Number" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextBox ID="txtOrderStatus" runat="server" Enabled="false" LabelText="Order Status" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
   <ucControls:InputTextBox ID="txtCreateBy" runat="server" Enabled="false" LabelText="Create By" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
    <ucControls:InputTextDate ID="txtCreateDate" runat="server" Enabled="false" TextMode="Date" LabelText="Create Date" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
</ucControls:PanelControlRow>

<ucControls:GridExt ID="gridDetail" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Administrator.InterfaceMonitors.ShipDetail" KeyField="KeyID" KeyType="String"
    DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true">
    <CustomCommandTemplate>
    </CustomCommandTemplate>
    <CustomSearchTemplate>
        <ucControls:InputHidden ID="hidKeyID" runat="server" DataFieldValue="host_record_id" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>        
        <ucControls:GridColumnExt ID="GridColumnExt117" runat="server" HeaderText="Host Shipment ID" DataField="host_shipment_id" AllowSort="true" ResourceGroup="" ResourceName="" />        
        <ucControls:GridColumnExt ID="GridColumnExt119" runat="server" HeaderText="Processing Status" DataField="processing_status" AllowSort="true" ResourceGroup="" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Line Number" DataField="line_number" AllowSort="true" ResourceGroup="" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Item Number" DataField="item_number" AllowSort="true" ResourceGroup="" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Location" DataField="location" AllowSort="true" ResourceGroup="" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="LPN" DataField="lpn" AllowSort="true" ResourceGroup="" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Quantity" DataField="quantity" AllowSort="true" ResourceGroup="" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" ResourceGroup="" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Lot" DataField="lot_number" AllowSort="true" ResourceGroup="" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="MFG Date" DataField="mfg_date" FormatType="Date" AllowSort="true" ResourceGroup="" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Expiry Date" DataField="expiry_date" AllowSort="true" ResourceGroup="" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Attribute 1" DataField="attribute1" AllowSort="true" ResourceGroup="" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt123" runat="server" HeaderText="Status" DataField="status" AllowSort="true" ResourceGroup="" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Receipt Date" DataField="receipt_date" FormatType="Date" AllowSort="true" ResourceGroup="" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt140" runat="server" HeaderText="Create Date" DataField="create_date" FormatType="Date" AllowSort="true" ResourceGroup="" ResourceName="" />
        <ucControls:GridColumnExt ID="GridColumnExt141" runat="server" HeaderText="Create By" DataField="create_by" AllowSort="true" ResourceGroup="" ResourceName="" />
    </CustomColumnTemplate>
</ucControls:GridExt>
