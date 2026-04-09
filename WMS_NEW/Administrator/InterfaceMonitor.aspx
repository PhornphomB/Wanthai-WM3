<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="InterfaceMonitor.aspx.cs" Inherits="WMS_NEW.Administrator.InterfaceMonitor" %>

<%@ Register Src="~/Administrator/AscxControls/InterfaceMonitor/ucInboundDetail.ascx" TagPrefix="ucControls" TagName="ucInboundDetail" %>
<%@ Register Src="~/Administrator/AscxControls/InterfaceMonitor/ucReceiptDetail.ascx" TagPrefix="ucControls" TagName="ucReceiptDetail" %>
<%@ Register Src="~/Administrator/AscxControls/InterfaceMonitor/ucOutboundDetail.ascx" TagPrefix="ucControls" TagName="ucOutboundDetail" %>
<%@ Register Src="~/Administrator/AscxControls/InterfaceMonitor/ucShipDetail.ascx" TagPrefix="ucControls" TagName="ucShipDetail" %>
<%@ Register Src="~/Administrator/AscxControls/InterfaceMonitor/ucM3ReceiptDetail.ascx" TagPrefix="ucControls" TagName="ucM3ReceiptDetail" %>
<%@ Register Src="~/Administrator/AscxControls/InterfaceMonitor/ucShipNotPODetail.ascx" TagPrefix="ucControls" TagName="ucShipNotPODetail" %>
<%@ Register Src="~/Administrator/AscxControls/InterfaceMonitor/ucShipPODetail.ascx" TagPrefix="ucControls" TagName="ucShipPODetail" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:PanelPopup runat="server" ID="popInboundDetail" CssClass="popup_full" HeaderText="Inbound Detail">
        <DataTemplate>
            <ucControls:ucInboundDetail runat="server" ID="ucInboundDetail" />
        </DataTemplate>
        <CommandTemplate>
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:PanelPopup runat="server" ID="popOutboundDetail" CssClass="popup_full" HeaderText="Outbound Detail">
        <DataTemplate>
            <ucControls:ucOutboundDetail runat="server" ID="ucOutboundDetail" />
        </DataTemplate>
        <CommandTemplate>
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:PanelPopup runat="server" ID="popReceiptDetail" CssClass="popup_full" HeaderText="Receipt Detail">
        <DataTemplate>
            <ucControls:ucReceiptDetail runat="server" ID="ucReceiptDetail" />
        </DataTemplate>
        <CommandTemplate>
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:PanelPopup runat="server" ID="popShipDetail" CssClass="popup_full" HeaderText="Ship Detail">
        <DataTemplate>
            <ucControls:ucShipDetail runat="server" ID="ucShipDetail" />
        </DataTemplate>
        <CommandTemplate>
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:PanelPopup runat="server" ID="popM3ReceiptDetail" CssClass="popup_full" HeaderText="M3 Receipt Detail">
    <DataTemplate>
        <ucControls:ucM3ReceiptDetail runat="server" ID="ucM3ReceiptDetail" />
    </DataTemplate>
    <CommandTemplate>
    </CommandTemplate>
</ucControls:PanelPopup>

        <ucControls:PanelPopup runat="server" ID="popShipNotPODetail" CssClass="popup_full" HeaderText="M3 Receipt Detail">
    <DataTemplate>
        <ucControls:ucShipNotPODetail runat="server" ID="ucShipNotPODetail" />
    </DataTemplate>
    <CommandTemplate>
    </CommandTemplate>
</ucControls:PanelPopup>

        <ucControls:PanelPopup runat="server" ID="popShipPODetail" CssClass="popup_full" HeaderText="M3 Receipt Detail">
    <DataTemplate>
        <ucControls:ucShipPODetail runat="server" ID="ucShipPODetail" />
    </DataTemplate>
    <CommandTemplate>
    </CommandTemplate>
</ucControls:PanelPopup>

    <div class="col-lg-12 col-md-12 col-sm-12 pt-3 background-base">

        <asp:UpdatePanel ID="updateFilter" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" CssClass="col-sm-12">
            <ContentTemplate>
                <ucControls:PanelControlRow ID="pnFilter" runat="server" CssClass="row">

                    <ucControls:InputHidden ID="hid_active_load" runat="server" DataFieldValue="_active_load" />

                    <ucControls:InputDropDown ID="ddlProcessingStatus" runat="server" Filterable="true" DisplayDefault="-- All --" DataFieldValue="processing_status" ComboType="String" LabelText="Processing Status" BaseContentCss="col-sm-2" ResourceGroup="interface_monitor" ResourceName="processing_status" />
                    <ucControls:InputDropDown ID="ddlWarehouse" runat="server" Filterable="true" DisplayDefault="-- All --" DataFieldValue="wh_id" ComboType="String" LabelText="Ware House ID" BaseContentCss="col-sm-2" ResourceGroup="warehouse" ResourceName="wh_id" />
                    <ucControls:InputDropDown ID="ddlOwner" runat="server" Filterable="true" DisplayDefault="-- All --" DataFieldValue="owner_id" ComboType="String" LabelText="Owner Code" BaseContentCss="col-sm-2" ResourceGroup="owner" ResourceName="owner_code" />

                    <ucControls:InputDropDown ID="ddlOrderType" runat="server" Filterable="true" DisplayDefault="-- All --" DataFieldValue="type" ComboType="String" LabelText="Order Type" BaseContentCss="col-sm-2" ResourceGroup="inbound_master" ResourceName="order_type" />
                    <ucControls:InputDropDown ID="ddlOrderStatus" runat="server" Filterable="true" DisplayDefault="-- All --" DataFieldValue="order_status" ComboType="String" LabelText="Status" BaseContentCss="col-sm-2" ResourceGroup="inbound_master" ResourceName="order_status" />
                    <ucControls:InputTextBox ID="txtOrder" runat="server" Filterable="true" DataFieldValue="order_number" LabelText="Order No" BaseContentCss="col-sm-2" ResourceGroup="inbound_master" ResourceName="inbound_order_number" />
                    <ucControls:InputTextDate ID="txtExpectDeliveryDate" runat="server" Filterable="true" TextMode="Date" DataFieldValue="expected_delivery_date" LabelText="Expect Date" BaseContentCss="col-sm-2" ResourceGroup="inbound_master" ResourceName="expected_delivery_date" />

                    <ucControls:InputDropDownHD ID="ddlSupplier" runat="server" Filterable="true" DisplayDefault="-- All --" DataFieldValue="supplier_code" ComboType="String" LabelText="Supplier Code" BaseContentCss="col-sm-2" ResourceGroup="supplier" ResourceName="supplier_code" />
                    <ucControls:InputDropDownHD ID="ddlCustomer" runat="server" Filterable="true" DisplayDefault="-- All --" DataFieldValue="customer_code" ComboType="String" LabelText="Customer Name" BaseContentCss="col-sm-2" ResourceGroup="customer" ResourceName="customer_name" />
                    <ucControls:InputTextDate ID="txtCreateDate" runat="server" TextMode="Date" Filterable="true" DataFieldValue="create_date" LabelText="Create Date" BaseContentCss="col-sm-2" ResourceGroup="interface_monitor" ResourceName="create_date" />
                    <ucControls:InputTextBox ID="txtCreateBy" runat="server" Filterable="true" DataFieldValue="create_by" LabelText="Create By" BaseContentCss="col-sm-2" ResourceGroup="interface_monitor" ResourceName="create_by" />
                </ucControls:PanelControlRow>
            </ContentTemplate>
        </asp:UpdatePanel>


        <asp:UpdatePanel ID="updateContent" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" CssClass="row col-sm-12" style="padding-bottom: 14px;">
            <ContentTemplate>
                <ucControls:ButtonExt ID="btToggle" runat="server" Text="Hide Filter" CssClass="btn btn-sm btn-info" CausesValidation="false" OnClick="btToggle_Click" ResourceGroup="General" ResourceName="Hide Filter" />
                <ucControls:ButtonExt ID="btSearch" runat="server" Text="Search" CssClass="btn btn-primary" CausesValidation="false" OnClick="btSearch_Click" ResourceGroup="General" ResourceName="Search" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <ucControls:PanelTab ID="PanelTab1" runat="server">
            <ControlTemplate>
                <ucControls:PanelControlTab ID="pnInbound" runat="server" PanelName="Inbound" ResourceGroup="interface_monitor" ResourceName="tab_inbound">
                    <ucControls:GridExt ID="GridExtInbound" runat="server"
                        SourceAssemblyName="WM.AccessTransaction" SourceClassName="WMS_NEW.Access.Administrator.InterfaceMonitor.Inbound" KeyField="KeyID" KeyType="String"
                        DisableButtonSearch="true" AutoSize="true" GridAllowRowClick="true" OnGridRefreshClick="btSearch_Click">
                        <CustomCommandTemplate>
                        </CustomCommandTemplate>
                        <CustomSearchTemplate>
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Inbound Order ID" DataField="KeyID" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="inbound_order_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Host Record" DataField="host_record_id" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="host_record_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Record Type" DataField="record_type" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="record_type" />
                            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Processing Code" DataField="processing_code" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="processing_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Processing Status" DataField="processing_status" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="processing_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Warehouse" DataField="wh_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Owner" DataField="owner_id" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Order Number" DataField="order_number" AllowSort="true" ResourceGroup="inbound_master" ResourceName="inbound_order_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Order Type" DataField="type" AllowSort="true" ResourceGroup="inbound_master" ResourceName="order_type" />
                            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="Supplier" DataField="supplier_code" AllowSort="true" ResourceGroup="supplier" ResourceName="supplier_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Customer" DataField="customer_code" AllowSort="true" ResourceGroup="customer" ResourceName="customer_name" />
                            <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Order Create Date" DataField="order_create_date" AllowSort="true" FormatType="Date" ResourceGroup="general" ResourceName="create_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Order Status" DataField="order_status" AllowSort="true" ResourceGroup="inbound_master" ResourceName="order_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Expected Delivery Date" DataField="expected_delivery_date" AllowSort="true" FormatType="Date" ResourceGroup="inbound_master" ResourceName="expected_delivery_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" HeaderText="UDF 1" DataField="user_def1" AllowSort="true" ResourceGroup="inbound_master" ResourceName="user_def1" />
                            <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" HeaderText="UDF 2" DataField="user_def2" AllowSort="true" ResourceGroup="inbound_master" ResourceName="user_def2" />
                            <ucControls:GridColumnExt ID="GridColumnExt17" runat="server" HeaderText="UDF 3" DataField="user_def3" AllowSort="true" ResourceGroup="inbound_master" ResourceName="user_def3" />
                            <ucControls:GridColumnExt ID="GridColumnExt18" runat="server" HeaderText="UDF 4" DataField="user_def4" AllowSort="true" ResourceGroup="inbound_master" ResourceName="user_def4" />
                            <ucControls:GridColumnExt ID="GridColumnExt19" runat="server" HeaderText="UDF 5" DataField="user_def5" AllowSort="true" ResourceGroup="inbound_master" ResourceName="user_def5" />
                            <ucControls:GridColumnExt ID="GridColumnExt20" runat="server" HeaderText="UDF 6" DataField="user_def6" AllowSort="true" ResourceGroup="inbound_master" ResourceName="user_def6" />
                            <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" HeaderText="UDF 7" DataField="user_def7" AllowSort="true" ResourceGroup="inbound_master" ResourceName="user_def7" />
                            <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" HeaderText="UDF 8" DataField="user_def8" AllowSort="true" ResourceGroup="inbound_master" ResourceName="user_def8" />
                            <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" HeaderText="UDF 9" DataField="user_def9" AllowSort="true" FormatType="Date" ResourceGroup="inbound_master" ResourceName="user_def9" />
                            <ucControls:GridColumnExt ID="GridColumnExt24" runat="server" HeaderText="UDF 10" DataField="user_def10" AllowSort="true" FormatType="Date" ResourceGroup="inbound_master" ResourceName="user_def10" />
                            <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="Insert Date" DataField="insert_date" AllowSort="true" FormatType="Date" ResourceGroup="interface_monitor" ResourceName="insert_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt26" runat="server" HeaderText="Create Date" DataField="create_date" AllowSort="true" FormatType="Date" ResourceGroup="interface_monitor" ResourceName="create_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt27" runat="server" HeaderText="Create By" DataField="create_by" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="create_by" />
                            <ucControls:GridColumnExt ID="GridColumnExt28" runat="server" HeaderText="Host Source" DataField="host_source" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="host_source" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnOutbound" runat="server" PanelName="Outbound" ResourceGroup="interface_monitor" ResourceName="tab_outbound">
                    <ucControls:GridExt ID="GridExtOutbound" runat="server"
                        SourceAssemblyName="WM.AccessTransaction" SourceClassName="WMS_NEW.Access.Administrator.InterfaceMonitor.Outbound" KeyField="KeyID" KeyType="String"
                        GridAllowRowClick="true" DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true" OnGridRefreshClick="btSearch_Click">
                        <CustomCommandTemplate>
                        </CustomCommandTemplate>
                        <CustomSearchTemplate>
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt29" runat="server" HeaderText="Outbound Order ID" DataField="outbound_order_id" AllowSort="true" ResourceGroup="outbound_master" ResourceName="outbound_order_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt30" runat="server" HeaderText="Host Record" DataField="KeyID" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="host_record_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt31" runat="server" HeaderText="Record Type" DataField="record_type" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="record_type" />
                            <ucControls:GridColumnExt ID="GridColumnExt32" runat="server" HeaderText="Processing Code" DataField="processing_code" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="processing_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt33" runat="server" HeaderText="Processing Status" DataField="processing_status" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="processing_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt34" runat="server" HeaderText="Warehouse" DataField="wh_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt35" runat="server" HeaderText="Owner" DataField="owner_id" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt36" runat="server" HeaderText="Order Number" DataField="order_number" AllowSort="true" ResourceGroup="outbound_master" ResourceName="outbound_order_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt37" runat="server" HeaderText="Order Type" DataField="type" AllowSort="true" ResourceGroup="outbound_master" ResourceName="order_type" />
                            <ucControls:GridColumnExt ID="GridColumnExt39" runat="server" HeaderText="Customer" DataField="customer_code" AllowSort="true" ResourceGroup="customer" ResourceName="customer_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt40" runat="server" HeaderText="Order Status" DataField="order_status" AllowSort="true" ResourceGroup="outbound_master" ResourceName="order_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt41" runat="server" HeaderText="Customer Order Number" DataField="customer_order_number" AllowSort="true" ResourceGroup="outbound_master" ResourceName="customer_order_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt43" runat="server" HeaderText="Customer Code" DataField="customer_code" AllowSort="true" ResourceGroup="customer" ResourceName="customer_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt44" runat="server" HeaderText="Customer Purchase Order" DataField="customer_purchase_order" AllowSort="true" ResourceGroup="outbound_master" ResourceName="customer_purchase_order" />
                            <ucControls:GridColumnExt ID="GridColumnExt45" runat="server" HeaderText="Order Create Date" DataField="order_create_date" AllowSort="true" FormatType="Date" ResourceGroup="outbound_master" ResourceName="order_create_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt46" runat="server" HeaderText="Order Ship Date" DataField="order_ship_date" AllowSort="true" FormatType="Date" ResourceGroup="outbound_master" ResourceName="order_ship_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt47" runat="server" HeaderText="Department" DataField="department" AllowSort="true" ResourceGroup="outbound_master" ResourceName="department" />
                            <ucControls:GridColumnExt ID="GridColumnExt48" runat="server" HeaderText="Load ID" DataField="load_id" AllowSort="true" ResourceGroup="outbound_master" ResourceName="load_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt49" runat="server" HeaderText="Load Sequence Number" DataField="load_sequence_number" AllowSort="true" ResourceGroup="outbound_master" ResourceName="load_sequence_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt50" runat="server" HeaderText="Tracking Number" DataField="tracking_number" AllowSort="true" ResourceGroup="outbound_master" ResourceName="tracking_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt51" runat="server" HeaderText="Rush Order" DataField="rush_order" AllowSort="true" ResourceGroup="outbound_master" ResourceName="rush_order" />
                            <ucControls:GridColumnExt ID="GridColumnExt52" runat="server" HeaderText="Back Order Flag" DataField="back_order_flag" AllowSort="true" ResourceGroup="outbound_master" ResourceName="back_order_flag" />
                            <ucControls:GridColumnExt ID="GridColumnExt53" runat="server" HeaderText="Ship To Code" DataField="ship_to_code" AllowSort="true" ResourceGroup="outbound_master" ResourceName="ship_to_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt54" runat="server" HeaderText="Ship To Name" DataField="ship_to_name" AllowSort="true" ResourceGroup="outbound_master" ResourceName="ship_to_name" />
                            <ucControls:GridColumnExt ID="GridColumnExt55" runat="server" HeaderText="Bill To Code" DataField="bill_to_code" AllowSort="true" ResourceGroup="outbound_master" ResourceName="bill_to_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt56" runat="server" HeaderText="Bill To Name" DataField="bill_to_name" AllowSort="true" ResourceGroup="outbound_master" ResourceName="bill_to_name" />
                            <ucControls:GridColumnExt ID="GridColumnExt38" runat="server" HeaderText="Request By" DataField="requested_by" AllowSort="true" ResourceGroup="outbound_master" ResourceName="requested_by" />
                            <ucControls:GridColumnExt ID="GridColumnExt58" runat="server" HeaderText="UDF 1" DataField="user_def1" AllowSort="true" ResourceGroup="outbound_master" ResourceName="user_def1" />
                            <ucControls:GridColumnExt ID="GridColumnExt59" runat="server" HeaderText="UDF 2" DataField="user_def2" AllowSort="true" ResourceGroup="outbound_master" ResourceName="user_def2" />
                            <ucControls:GridColumnExt ID="GridColumnExt60" runat="server" HeaderText="UDF 3" DataField="user_def3" AllowSort="true" ResourceGroup="outbound_master" ResourceName="user_def3" />
                            <ucControls:GridColumnExt ID="GridColumnExt61" runat="server" HeaderText="UDF 4" DataField="user_def4" AllowSort="true" ResourceGroup="outbound_master" ResourceName="user_def4" />
                            <ucControls:GridColumnExt ID="GridColumnExt62" runat="server" HeaderText="UDF 5" DataField="user_def5" AllowSort="true" ResourceGroup="outbound_master" ResourceName="user_def5" />
                            <ucControls:GridColumnExt ID="GridColumnExt63" runat="server" HeaderText="UDF 6" DataField="user_def6" AllowSort="true" ResourceGroup="outbound_master" ResourceName="user_def6" />
                            <ucControls:GridColumnExt ID="GridColumnExt64" runat="server" HeaderText="UDF 7" DataField="user_def7" AllowSort="true" ResourceGroup="outbound_master" ResourceName="user_def7" />
                            <ucControls:GridColumnExt ID="GridColumnExt65" runat="server" HeaderText="UDF 8" DataField="user_def8" AllowSort="true" ResourceGroup="outbound_master" ResourceName="user_def8" />
                            <ucControls:GridColumnExt ID="GridColumnExt66" runat="server" HeaderText="UDF 9" DataField="user_def9" AllowSort="true" FormatType="Date" ResourceGroup="outbound_master" ResourceName="user_def9" />
                            <ucControls:GridColumnExt ID="GridColumnExt67" runat="server" HeaderText="UDF 10" DataField="user_def10" AllowSort="true" FormatType="Date" ResourceGroup="outbound_master" ResourceName="user_def10" />
                            <ucControls:GridColumnExt ID="GridColumnExt68" runat="server" HeaderText="Insert Date" DataField="insert_date" AllowSort="true" FormatType="Date" ResourceGroup="interface_monitor" ResourceName="insert_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt69" runat="server" HeaderText="Create Date" DataField="create_date" AllowSort="true" FormatType="Date" ResourceGroup="interface_monitor" ResourceName="create_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt70" runat="server" HeaderText="Create By" DataField="create_by" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="create_by" />
                            <ucControls:GridColumnExt ID="GridColumnExt71" runat="server" HeaderText="Host Source" DataField="host_source" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="host_source" />
                            <ucControls:GridColumnExt ID="GridColumnExt72" runat="server" HeaderText="Delivery Date Plan" DataField="delivery_date_plan" AllowSort="true" FormatType="Date" ResourceGroup="outbound_master" ResourceName="delivery_date_plan" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnReceipt" runat="server" PanelName="Receipt" ResourceGroup="interface_monitor" ResourceName="tab_receipt">
                    <ucControls:GridExt ID="GridExtReceipt" runat="server"
                        SourceAssemblyName="WM.AccessTransaction" SourceClassName="WMS_NEW.Access.Administrator.InterfaceMonitor.Receipt" KeyField="KeyID" KeyType="String"
                        GridAllowRowClick="true" DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true" OnGridRefreshClick="btSearch_Click">
                        <CustomCommandTemplate>
                        </CustomCommandTemplate>
                        <CustomSearchTemplate>
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt73" runat="server" HeaderText="Processing Code" DataField="processing_code" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="processing_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt74" runat="server" HeaderText="Processing Status" DataField="processing_status" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="processing_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt75" runat="server" HeaderText="Warehouse" DataField="wh_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt76" runat="server" HeaderText="Owner" DataField="owner_id" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt77" runat="server" HeaderText="Order Type" DataField="order_type" AllowSort="true" ResourceGroup="inbound_master" ResourceName="order_type" />
                            <ucControls:GridColumnExt ID="GridColumnExt78" runat="server" HeaderText="Supplier" DataField="supplier_code" AllowSort="true" ResourceGroup="supplier" ResourceName="supplier_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt79" runat="server" HeaderText="Inbound Order Number" DataField="inbound_order_number" AllowSort="true" ResourceGroup="inbound_master" ResourceName="inbound_order_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt80" runat="server" HeaderText="Reference Number" DataField="reference_number" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="reference_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt81" runat="server" HeaderText="Order Status" DataField="order_status" AllowSort="true" ResourceGroup="inbound_master" ResourceName="order_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt82" runat="server" HeaderText="UDF 1" DataField="user_def1" AllowSort="true" ResourceGroup="receipt_header" ResourceName="user_def1" />
                            <ucControls:GridColumnExt ID="GridColumnExt83" runat="server" HeaderText="UDF 2" DataField="user_def2" AllowSort="true" ResourceGroup="receipt_header" ResourceName="user_def2" />
                            <ucControls:GridColumnExt ID="GridColumnExt84" runat="server" HeaderText="UDF 3" DataField="user_def3" AllowSort="true" ResourceGroup="receipt_header" ResourceName="user_def3" />
                            <ucControls:GridColumnExt ID="GridColumnExt85" runat="server" HeaderText="UDF 4" DataField="user_def4" AllowSort="true" ResourceGroup="receipt_header" ResourceName="user_def4" />
                            <ucControls:GridColumnExt ID="GridColumnExt86" runat="server" HeaderText="UDF 5" DataField="user_def5" AllowSort="true" ResourceGroup="receipt_header" ResourceName="user_def5" />
                            <ucControls:GridColumnExt ID="GridColumnExt87" runat="server" HeaderText="UDF 6" DataField="user_def6" AllowSort="true" ResourceGroup="receipt_header" ResourceName="user_def6" />
                            <ucControls:GridColumnExt ID="GridColumnExt88" runat="server" HeaderText="UDF 7" DataField="user_def7" AllowSort="true" ResourceGroup="receipt_header" ResourceName="user_def7" />
                            <ucControls:GridColumnExt ID="GridColumnExt89" runat="server" HeaderText="UDF 8" DataField="user_def8" AllowSort="true" ResourceGroup="receipt_header" ResourceName="user_def8" />
                            <ucControls:GridColumnExt ID="GridColumnExt90" runat="server" HeaderText="UDF 9" DataField="user_def9" AllowSort="true" FormatType="Date" ResourceGroup="receipt_header" ResourceName="user_def9" />
                            <ucControls:GridColumnExt ID="GridColumnExt91" runat="server" HeaderText="UDF 10" DataField="user_def10" AllowSort="true" FormatType="Date" ResourceGroup="receipt_header" ResourceName="user_def10" />
                            <ucControls:GridColumnExt ID="GridColumnExt92" runat="server" HeaderText="Create Date" DataField="create_date" AllowSort="true" FormatType="Date" ResourceGroup="general" ResourceName="create_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt93" runat="server" HeaderText="Create By" DataField="create_by" AllowSort="true" ResourceGroup="general" ResourceName="create_by" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnShip" runat="server" PanelName="Ship" ResourceGroup="interface_monitor" ResourceName="tab_ship">
                    <ucControls:GridExt ID="GridExtShip" runat="server"
                        SourceAssemblyName="WM.AccessTransaction" SourceClassName="WMS_NEW.Access.Administrator.InterfaceMonitor.Ship" KeyField="KeyID" KeyType="String"
                        GridAllowRowClick="true" DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true" OnGridRefreshClick="btSearch_Click">
                        <CustomCommandTemplate>
                        </CustomCommandTemplate>
                        <CustomSearchTemplate>
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt94" runat="server" HeaderText="Processing Code" DataField="processing_code" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="processing_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt95" runat="server" HeaderText="Processing Status" DataField="processing_status" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="processing_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt96" runat="server" HeaderText="Warehouse" DataField="wh_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt97" runat="server" HeaderText="Customer" DataField="customer_id" AllowSort="true" ResourceGroup="customer" ResourceName="customer_name" />
                            <ucControls:GridColumnExt ID="GridColumnExt98" runat="server" HeaderText="Owner" DataField="owner_id" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt99" runat="server" HeaderText="Order Number" DataField="order_number" AllowSort="true" ResourceGroup="outbound_master" ResourceName="outbound_order_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt100" runat="server" HeaderText="Order Status" DataField="order_status" AllowSort="true" ResourceGroup="outbound_master" ResourceName="order_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt101" runat="server" HeaderText="Order Type" DataField="order_type" AllowSort="true" ResourceGroup="outbound_master" ResourceName="order_type" />
                            <ucControls:GridColumnExt ID="GridColumnExt102" runat="server" HeaderText="Tracking Number" DataField="tracking_number" AllowSort="true" ResourceGroup="outbound_master" ResourceName="tracking_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt103" runat="server" HeaderText="Ship Date Actual" DataField="ship_date_actual" AllowSort="true" ResourceGroup="outbound_master" ResourceName="ship_date_actual" />
                            <ucControls:GridColumnExt ID="GridColumnExt104" runat="server" HeaderText="Delivery Date Plan" DataField="delivery_date_plan" AllowSort="true" FormatType="Date" ResourceGroup="outbound_master" ResourceName="delivery_date_plan" />
                            <ucControls:GridColumnExt ID="GridColumnExt105" runat="server" HeaderText="Container Number" DataField="container_number" AllowSort="true" ResourceGroup="ship_conf_header" ResourceName="container_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt106" runat="server" HeaderText="Seal Number" DataField="seal_number" AllowSort="true" ResourceGroup="ship_conf_header" ResourceName="seal_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt107" runat="server" HeaderText="Trailer Number" DataField="trailer_number" AllowSort="true" ResourceGroup="ship_conf_header" ResourceName="trailer_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt108" runat="server" HeaderText="Carrier" DataField="carrier_code" AllowSort="true" ResourceGroup="ship_conf_header" ResourceName="carrier_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt109" runat="server" HeaderText="Equipment Type" DataField="equipment_type" AllowSort="true" ResourceGroup="ship_conf_header" ResourceName="equipment_type" />
                            <ucControls:GridColumnExt ID="GridColumnExt110" runat="server" HeaderText="CDL License" DataField="cdl_license" AllowSort="true" ResourceGroup="ship_conf_header" ResourceName="cdl_license" />
                            <ucControls:GridColumnExt ID="GridColumnExt111" runat="server" HeaderText="Ship Code" DataField="ship_to_code" AllowSort="true" ResourceGroup="outbound_master" ResourceName="ship_to_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt112" runat="server" HeaderText="Ship Name" DataField="ship_to_name" AllowSort="true" ResourceGroup="outbound_master" ResourceName="ship_to_name" />
                            <ucControls:GridColumnExt ID="GridColumnExt113" runat="server" HeaderText="Bill Code" DataField="bill_to_code" AllowSort="true" ResourceGroup="outbound_master" ResourceName="bill_to_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt114" runat="server" HeaderText="Bill Name" DataField="bill_to_name" AllowSort="true" ResourceGroup="outbound_master" ResourceName="bill_to_name" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnItemAndUOM" runat="server" PanelName="ItemAndUOM" ResourceGroup="interface_monitor" ResourceName="tab_item_and_uom">
                    <ucControls:GridExt ID="GridExtItemAndUOM" runat="server"
                        SourceAssemblyName="WM.AccessTransaction" SourceClassName="WMS_NEW.Access.Administrator.InterfaceMonitor.Item" KeyField="KeyID" KeyType="String"
                        GridAllowRowClick="false" DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true" OnGridRefreshClick="btSearch_Click">
                        <CustomCommandTemplate>
                        </CustomCommandTemplate>
                        <CustomSearchTemplate>
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt134" runat="server" HeaderText="Host Record" DataField="KeyID" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="host_record_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt42" runat="server" HeaderText="Item Number" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt57" runat="server" HeaderText="Description" DataField="description" AllowSort="true" ResourceGroup="item" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt115" runat="server" HeaderText="Item Category" DataField="item_category" AllowSort="true" ResourceGroup="category" ResourceName="category" />
                            <ucControls:GridColumnExt ID="GridColumnExt116" runat="server" HeaderText="Lot Control" DataField="lot_control" AllowSort="true" ResourceGroup="item" ResourceName="lot_control" />
                            <ucControls:GridColumnExt ID="GridColumnExt117" runat="server" HeaderText="Expiry Date Control" DataField="expiry_date_control" AllowSort="true" ResourceGroup="item" ResourceName="expiry_date_control" />
                            <ucControls:GridColumnExt ID="GridColumnExt118" runat="server" HeaderText="Serial Number Control" DataField="sn_control" AllowSort="true" ResourceGroup="item" ResourceName="sn_control" />
                            <ucControls:GridColumnExt ID="GridColumnExt119" runat="server" HeaderText="Attribute1 Control" DataField="attribute1_control" AllowSort="true" ResourceGroup="item" ResourceName="attribute1_control" />
                            <ucControls:GridColumnExt ID="GridColumnExt120" runat="server" HeaderText="Attribute2 Control" DataField="attribute2_control" AllowSort="true" ResourceGroup="item" ResourceName="attribute2_control" />
                            <ucControls:GridColumnExt ID="GridColumnExt121" runat="server" HeaderText="Attribute3 Control" DataField="attribute3_control" AllowSort="true" ResourceGroup="item" ResourceName="attribute3_control" />
                            <ucControls:GridColumnExt ID="GridColumnExt122" runat="server" HeaderText="Attribute4 Control" DataField="attribute4_control" AllowSort="true" ResourceGroup="item" ResourceName="attribute4_control" />
                            <ucControls:GridColumnExt ID="GridColumnExt123" runat="server" HeaderText="Attribute5 Control" DataField="attribute5_control" AllowSort="true" ResourceGroup="item" ResourceName="attribute5_control" />
                            <ucControls:GridColumnExt ID="GridColumnExt124" runat="server" HeaderText="Item IS Active" DataField="item_is_active" AllowSort="true" ResourceGroup="item" ResourceName="Active" />
                            <ucControls:GridColumnExt ID="GridColumnExt125" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                            <ucControls:GridColumnExt ID="GridColumnExt126" runat="server" HeaderText="UOM IS Active" DataField="uom_is_active" AllowSort="true" ResourceGroup="item_uom" ResourceName="Active" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnCustomer" runat="server" PanelName="Customer" ResourceGroup="interface_monitor" ResourceName="tab_customer">
                    <ucControls:GridExt ID="GridExtCustomer" runat="server"
                        SourceAssemblyName="WM.AccessTransaction" SourceClassName="WMS_NEW.Access.Administrator.InterfaceMonitor.Customer" KeyField="KeyID" KeyType="String"
                        GridAllowRowClick="false" DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true" OnGridRefreshClick="btSearch_Click">
                        <CustomCommandTemplate>
                        </CustomCommandTemplate>
                        <CustomSearchTemplate>
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt127" runat="server" HeaderText="Host Record" DataField="KeyID" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="host_record_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt128" runat="server" HeaderText="Processing Status" DataField="processing_status" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="processing_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt129" runat="server" HeaderText="Customer Code" DataField="customer_code" AllowSort="true" ResourceGroup="customer" ResourceName="customer_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt130" runat="server" HeaderText="Name" DataField="name" AllowSort="true" ResourceGroup="customer" ResourceName="customer_name" />
                            <ucControls:GridColumnExt ID="GridColumnExt131" runat="server" HeaderText="Phone" DataField="phone" AllowSort="true" ResourceGroup="customer" ResourceName="phone" />
                            <ucControls:GridColumnExt ID="GridColumnExt132" runat="server" HeaderText="Fax" DataField="fax" AllowSort="true" ResourceGroup="customer" ResourceName="fax" />
                            <ucControls:GridColumnExt ID="GridColumnExt133" runat="server" HeaderText="Email" DataField="email" AllowSort="true" ResourceGroup="customer" ResourceName="email" />
                            <ucControls:GridColumnExt ID="GridColumnExt135" runat="server" HeaderText="Contact" DataField="contact" AllowSort="true" ResourceGroup="customer" ResourceName="contact" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnM3Receipt" runat="server" PanelName="M3 Receipt" ResourceGroup="interface_monitor" ResourceName="tab_m3_receipt">
                    <%--- v_host_m3_receipt_header
- v_host_m3_receipt_line--%>
                    <ucControls:GridExt ID="GridExtM3Receipt" runat="server"
                        SourceAssemblyName="WM.AccessTransaction" SourceClassName="WMS_NEW.Access.Administrator.InterfaceMonitor.M3Receipt" KeyField="KeyID" KeyType="String"
                        GridAllowRowClick="true" DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true" OnGridRefreshClick="btSearch_Click">
                        <CustomCommandTemplate>
                        </CustomCommandTemplate>
                        <CustomSearchTemplate>
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt149" runat="server" HeaderText="Ship Date" DataField="ship_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="ship_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt150" runat="server" HeaderText="Order Type" DataField="order_type" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="order_type" />
                            <ucControls:GridColumnExt ID="GridColumnExt151" runat="server" HeaderText="WMS Create Date" DataField="wms_create_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="wms_create_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt152" runat="server" HeaderText="WMS Create Time" DataField="wms_create_time" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="wms_create_time" />
                            <ucControls:GridColumnExt ID="GridColumnExt153" runat="server" HeaderText="M3 Get Status" DataField="m3_get_status" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_get_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt154" runat="server" HeaderText="M3 Ref. No" DataField="m3_reference_no" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_reference_no" />
                            <ucControls:GridColumnExt ID="GridColumnExt155" runat="server" HeaderText="M3 Interface Date" DataField="m3_interface_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_interface_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt156" runat="server" HeaderText="M3 Interface Time" DataField="m3_interface_time" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_interface_time" />
                            <ucControls:GridColumnExt ID="GridColumnExt157" runat="server" HeaderText="M3 Interface Status" DataField="m3_interface_status" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_interface_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt158" runat="server" HeaderText="Sync Unsuccess No." DataField="Sync_UnsuccessNo" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="Sync_UnsuccessNo" />
                            <ucControls:GridColumnExt ID="GridColumnExt159" runat="server" HeaderText="Sync Flag" DataField="sync_flag" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="sync_flag" />
                            <ucControls:GridColumnExt ID="GridColumnExt160" runat="server" HeaderText="Sync Date" DataField="sync_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="sync_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt161" runat="server" HeaderText="Error Message" DataField="error_msg" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="error_msg" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>
                <ucControls:PanelControlTab ID="pnM3ShipNotPO" runat="server" PanelName="M3 Ship not PO" ResourceGroup="interface_monitor" ResourceName="tab_m3_ship_not_po">
                    <%--v_host_m3_ship_not_po_header
- v_host_m3_ship_not_po_line--%>
                    <ucControls:GridExt ID="GridExtM3ShipNotPO" runat="server"
                        SourceAssemblyName="WM.AccessTransaction" SourceClassName="WMS_NEW.Access.Administrator.InterfaceMonitor.ShipNotPO" KeyField="KeyID" KeyType="String"
                        GridAllowRowClick="true" DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true" OnGridRefreshClick="btSearch_Click">
                        <CustomCommandTemplate>
                        </CustomCommandTemplate>
                        <CustomSearchTemplate>
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt183" runat="server" HeaderText="Host Record" DataField="KeyID" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="host_record_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt136" runat="server" HeaderText="Ship Date" DataField="ship_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="ship_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt137" runat="server" HeaderText="Order Type" DataField="order_type" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="order_type" />
                            <ucControls:GridColumnExt ID="GridColumnExt138" runat="server" HeaderText="WMS Create Date" DataField="wms_create_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="wms_create_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt139" runat="server" HeaderText="WMS Create Time" DataField="wms_create_time" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="wms_create_time" />
                            <ucControls:GridColumnExt ID="GridColumnExt140" runat="server" HeaderText="M3 Get Status" DataField="m3_get_status" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_get_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt141" runat="server" HeaderText="M3 Ref. No" DataField="m3_reference_no" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_reference_no" />
                            <ucControls:GridColumnExt ID="GridColumnExt142" runat="server" HeaderText="M3 Interface Date" DataField="m3_interface_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_interface_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt143" runat="server" HeaderText="M3 Interface Time" DataField="m3_interface_time" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_interface_time" />
                            <ucControls:GridColumnExt ID="GridColumnExt144" runat="server" HeaderText="M3 Interface Status" DataField="m3_interface_status" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_interface_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt145" runat="server" HeaderText="Sync Unsuccess No." DataField="Sync_UnsuccessNo" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="Sync_UnsuccessNo" />
                            <ucControls:GridColumnExt ID="GridColumnExt146" runat="server" HeaderText="Sync Flag" DataField="sync_flag" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="sync_flag" />
                            <ucControls:GridColumnExt ID="GridColumnExt147" runat="server" HeaderText="Sync Date" DataField="sync_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="sync_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt148" runat="server" HeaderText="Error Message" DataField="error_msg" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="error_msg" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>
                <ucControls:PanelControlTab ID="pnM3ShipPO" runat="server" PanelName="M3 Ship PO" ResourceGroup="interface_monitor" ResourceName="tab_m3_ship_po">
                    <%--- v_host_m3_ship_po_header
- v_host_m3_ship_po_line--%>
                    <ucControls:GridExt ID="GridExtM3ShipPO" runat="server"
                        SourceAssemblyName="WM.AccessTransaction" SourceClassName="WMS_NEW.Access.Administrator.InterfaceMonitor.ShipPO" KeyField="KeyID" KeyType="String"
                        GridAllowRowClick="true" DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true" OnGridRefreshClick="btSearch_Click">
                        <CustomCommandTemplate>
                        </CustomCommandTemplate>
                        <CustomSearchTemplate>
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt175" runat="server" HeaderText="Customer Code" DataField="customer_code" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="customer_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt176" runat="server" HeaderText="Facility Code" DataField="facility_code" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="facility_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt177" runat="server" HeaderText="Warehouse" DataField="warehouse" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="warehouse" />
                            <ucControls:GridColumnExt ID="GridColumnExt178" runat="server" HeaderText="Order Type" DataField="order_type" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="order_type" />
                            <ucControls:GridColumnExt ID="GridColumnExt179" runat="server" HeaderText="Request Del Date" DataField="request_del_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="request_del_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt180" runat="server" HeaderText="Customer PO No." DataField="customer_po_no" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="customer_po_no" />
                            <ucControls:GridColumnExt ID="GridColumnExt181" runat="server" HeaderText="Customer PO Date" DataField="customer_po_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="customer_po_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt182" runat="server" HeaderText="Order Date" DataField="order_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="order_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt162" runat="server" HeaderText="Ship Date" DataField="ship_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="ship_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt163" runat="server" HeaderText="Ship Time" DataField="ship_time" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="ship_time" />
                            <ucControls:GridColumnExt ID="GridColumnExt164" runat="server" HeaderText="WMS Create Date" DataField="wms_create_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="wms_create_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt165" runat="server" HeaderText="WMS Create Time" DataField="wms_create_time" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="wms_create_time" />
                            <ucControls:GridColumnExt ID="GridColumnExt166" runat="server" HeaderText="M3 Get Status" DataField="m3_get_status" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_get_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt167" runat="server" HeaderText="M3 Ref. No" DataField="m3_reference_no" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_reference_no" />
                            <ucControls:GridColumnExt ID="GridColumnExt168" runat="server" HeaderText="M3 Interface Date" DataField="m3_interface_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_interface_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt169" runat="server" HeaderText="M3 Interface Time" DataField="m3_interface_time" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_interface_time" />
                            <ucControls:GridColumnExt ID="GridColumnExt170" runat="server" HeaderText="M3 Interface Status" DataField="m3_interface_status" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="m3_interface_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt171" runat="server" HeaderText="Sync Unsuccess No." DataField="Sync_UnsuccessNo" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="Sync_UnsuccessNo" />
                            <ucControls:GridColumnExt ID="GridColumnExt172" runat="server" HeaderText="Sync Flag" DataField="sync_flag" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="sync_flag" />
                            <ucControls:GridColumnExt ID="GridColumnExt173" runat="server" HeaderText="Sync Date" DataField="sync_date" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="sync_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt174" runat="server" HeaderText="Error Message" DataField="error_msg" AllowSort="true" ResourceGroup="interface_monitor" ResourceName="error_msg" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>
            </ControlTemplate>
        </ucControls:PanelTab>

    </div>

</asp:Content>
