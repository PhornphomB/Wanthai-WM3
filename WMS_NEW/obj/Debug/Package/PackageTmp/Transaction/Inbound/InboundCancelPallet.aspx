<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="InboundCancelPallet.aspx.cs" Inherits="WMS_NEW.Transaction.Inbound.InboundCancelPallet" %>

<%@ Register Src="~/Transaction/Inbound/AscxControl/ucInboundCancelPalletDetail.ascx" TagPrefix="ucControls" TagName="ucInboundCancelPalletDetail" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" VisibleSave="false" >
        <ControlTemplate>
            <div class="row">
                <ucControls:PanelControlRow ID="PanelControlRow0" runat="server" CssClass="row col-sm-9">
                    <ucControls:InputDropDown runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="ddlWarehouse" DataFieldValue="wh_master_id" IsPrimary="true" IsKey="true" UseDefaultDisplay="false" />
                    <ucControls:InputDropDown runat="server" ResourceGroup="owner" ResourceName="owner_id" ID="ddlOwner" DataFieldValue="owner_id" ControlSequence="1" AutoPostBack="true" IsPrimary="true" IsKey="true" UseDefaultDisplay="false" />
                    <ucControls:InputTextBox ID="txtInboundOrderNumber" runat="server" ResourceGroup="inbound_master" ResourceName="inbound_order_number" DataFieldValue="inbound_order_number" Enabled="false" />
                    <ucControls:InputTextBox ID="txtRefInboundOrderNumber" runat="server" ResourceGroup="inbound_master" ResourceName="ref_inbound_order_number" DataFieldValue="ref_inbound_order_number" Enabled="false" />

                    <ucControls:InputDropDown runat="server" ResourceGroup="inbound_master" ResourceName="order_type" ID="ddlOrderType" DataFieldValue="order_type" ComboType="String" IsPrimary="true" DisplayDefault="--Select--" IsKey="true" AutoPostBack="true" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="inbound_master" ResourceName="order_status" ID="txtOrderStatus" DataFieldValue="order_status" Enabled="false" />
                    <ucControls:InputTextBox ID="txtCreateBy" runat="server" ResourceGroup="general" ResourceName="create_by" DataFieldValue="create_by" Enabled="false" />
                    <ucControls:InputTextDate ID="dtpDate" runat="server" ResourceGroup="general" ResourceName="create_date" DataFieldValue="create_date" Enabled="false" TextMode="DateTime" IsDateNow="true" />


                </ucControls:PanelControlRow>
                <ucControls:PanelControlRow ID="PanelControlRow1" runat="server" CssClass="row col-sm-3">
                    <div class="col-sm-12">
                        <div class="card card-outline-danger">
                            <div class="card-block">
                                <div class="text-uppercase text-danger text-bold text-center mb-2"><span><ucControls:LabelExt ID="LabelExt2" runat="server" InnerText="TOTAL SUMMARY" ResourceGroup="total_summary" ResourceName="total_summary" /></span></div>
                                <div class="text-bold text-muted">
                                    <asp:UpdatePanel runat="server" ID="update_summary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="col-sm-12">
                                                <ucControls:LabelExt ID="LabelExt12" runat="server" InnerText="Plan quantity : " ResourceGroup="total_summary" ResourceName="quantity_plan" />
                                                <ucControls:LabelExt ID="lblSumPlanQTY" runat="server" InnerText="0" />
                                            </div>
                                            <div class="col-sm-12">
                                                <ucControls:LabelExt ID="LabelExt13" runat="server" InnerText="Receive quantity : " ResourceGroup="total_summary" ResourceName="quantity_receive" />
                                                <ucControls:LabelExt ID="lblSumReceiveQTY" runat="server" InnerText="0" />
                                            </div>
                                            <div class="col-sm-12">
                                                <ucControls:LabelExt ID="LabelExt1" runat="server" InnerText="Printed quantity : " ResourceGroup="total_summary" ResourceName="printed_receive" />
                                                <ucControls:LabelExt ID="lblSumPrintQTY" runat="server" InnerText="0" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </ucControls:PanelControlRow>


            </div>
            <ucControls:PanelTab ID="PanelTab1" runat="server">
                <ControlTemplate>
                    <ucControls:PanelControlTab ID="PanelControlTab6" runat="server" PanelName="Cancel Pallet Detail" ResourceGroup="cancel_pallet" ResourceName="tab_cancel_pallet_detail">
                         <ucControls:ucInboundCancelPalletDetail runat="server" ID="ucInboundCancelPalletDetail" />
                    </ucControls:PanelControlTab>
                </ControlTemplate>
            </ucControls:PanelTab>

        </ControlTemplate>
    </ucControls:PanelPopupEntity>


    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inbound.InboundCancelPallet" KeyField="KeyId" KeyType="Guid" VisibleExportTemplate="true"
        GridAllowRowEdit="true" GridAllowRowDelete="false" GridSortDefault="create_date desc,inbound_order_number" NewVisible="false">

        <CustomSearchTemplate>
            <ucControls:InputDropDownHD ID="ddlCategory" runat="server" ResourceGroup="category" ResourceName="category_id" DataFieldValue="_category_id" ComboType="Guid" DisplayDefault="--All--" DefaultFilter="Equal" FixFilter="true" />
            <ucControls:InputTextBox ID="txtItemNumber" runat="server" ResourceGroup="item" ResourceName="item_number" DataFieldValue="_item_number" DefaultFilter="Contains" FixFilter="true" />
            <ucControls:InputHidden ID="txtPageType" runat="server" DataFieldValue="_page_type" />
            <ucControls:InputTextDate ID="txtMFGDate" runat="server" ResourceGroup="inventory" ResourceName="mfg_date" DataFieldValue="_mfg_date" DefaultFilter="Equal" FixFilter="true" TextMode="Date" />
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="iColWarehouse" DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownAutoPostBack="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_master" ResourceName="order_type" ID="iColOrderType" DataField="order_type" FilterFormatType="Text" AllowFilter="true" AllowSort="true" ShowFilterNow="false" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_master" ResourceName="inbound_order_number" DataField="inbound_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_master" ResourceName="ref_inbound_order_number" DataField="ref_inbound_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_master" ResourceName="production_line" ID="iColProductionLine" DataField="production_line" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownFilterType="LazySearch" DefaultFilter="Contains" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="item_number" DataField="item_number" Width="400" AllowSort="true" IsIncludeInExcel="false" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="item_number" DataField="item_number_excel" Visible="false" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_master" ResourceName="order_status" ID="iColOrderStatus" DataField="order_status" FilterFormatType="Text" AllowFilter="true" AllowSort="true" ShowFilterNow="false" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="total_qty" DataField="total_qty" FormatType="Number" AllowSort="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="customer" ResourceName="customer_code" ID="iColCustomer" DataField="customer_code" DataFieldFilter="customer_id" FilterFormatType="Guid" AllowFilter="true" AllowSort="true" ShowFilterNow="false" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownFilterType="LazySearch" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="customer" ResourceName="customer_name" DataField="customer_name" AllowFilter="false" AllowSort="true" ShowFilterNow="false" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_by" DataField="create_by" AllowFilter="true" AllowSort="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_date" DataField="create_date" FormatType="DateTime" AllowFilter="true" AllowSort="true" ShowFilterNow="false" FilterFormatType="Date" />
        </CustomColumnTemplate>
    </ucControls:GridExt>

</asp:Content>

