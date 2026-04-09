<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="LabelPrinted.aspx.cs" Inherits="WMS_NEW.Export.LabelPrinted" %>
<%@ Register Src="~/Report/AscxControls/ucReportViewer.ascx" TagPrefix="ucControls" TagName="ucReportViewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:ucReportViewer runat="server" ID="ucReportViewer" />
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Viewer.LabelPrinted" KeyField="print_label_id" KeyType="Guid">
        <CustomCommandTemplate>
            <ucControls:ButtonExt ID="btReport" runat="server" Text="Report" CausesValidation="false" CssClass="btn btn-sm" OnClick="btReport_Click" ResourceGroup="general" ResourceName="btn_report" />
        </CustomCommandTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="GridColumnExt00" runat="server" ResourceGroup="warehouse" ResourceName="wh_id" HeaderText="Warehouse" DataField="wh_id" DataFieldFilter="wh_master_id" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownAutoPostBack="true" FilterFormatType="Guid" />
            <ucControls:GridColumnExt ID="GridColumnExt01" runat="server" ResourceGroup="item" ResourceName="item_number" HeaderText="Item Number" DataField="item_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt02" runat="server" ResourceGroup="item" ResourceName="description" HeaderText="Item Description" DataField="item_description" AllowFilter="true" />
            <ucControls:GridColumnExt ID="GridColumnExt03" runat="server" ResourceGroup="inventory" HeaderText="MFG Date" ResourceName="mfg_date" DataField="mfg_date" FormatType="Date" AllowFilter="true" AllowSort="true" FilterFormatType="Date" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt04" runat="server" ResourceGroup="print_label" ResourceName="print_date" DataField="print_date" FormatType="Date" AllowFilter="true" AllowSort="true" FilterFormatType="Date" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt05" runat="server" ResourceGroup="inbound_master" ResourceName="inbound_order_number" HeaderText="Inbound Order No" DataField="inbound_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt06" runat="server" ResourceGroup="inbound_master" ResourceName="production_line" HeaderText="Production Line" DataField="production_line" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownFilterType="LazySearch" />
            <ucControls:GridColumnExt ID="GridColumnExt07" runat="server" ResourceGroup="inbound_master" ResourceName="order_type" HeaderText="Order Type" DataField="order_type" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt08" runat="server" ResourceGroup="inventory" ResourceName="attribute1" HeaderText="Attribute1" DataField="attribute1" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt09" runat="server" ResourceGroup="label_printed" ResourceName="pack_size_per_pallet" HeaderText="Production Qty" DataField="pack_size_per_pallet" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" ResourceGroup="label_printed" ResourceName="stock_qty" HeaderText="Stock In Qty" DataField="stock_qty" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" ResourceGroup="label_printed" ResourceName="remain_qty" HeaderText="Remain Qty" DataField="remain_qty" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" ResourceGroup="item_uom" HeaderText="Pack Size UOM" ResourceName="item_uom_id" DataField="pack_size_uom" AllowFilter="true" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" ResourceGroup="inventory" HeaderText="LPN" ResourceName="lpn" DataField="lpn" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" ResourceGroup="inbound_detail" ResourceName="default_item_status" DataField="default_item_status" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" ResourceGroup="inbound_master" ResourceName="ref_lpn" DataFieldValue="ref_lpn" AllowFilter="true" AllowSort="true"  />
            <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" ResourceGroup="label_printed" ResourceName="qr_code" DataFieldValue="qr_code" AllowFilter="true" AllowSort="true" Visible="false" />
       </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
