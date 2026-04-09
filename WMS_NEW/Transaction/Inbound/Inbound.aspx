<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Inbound.aspx.cs" Inherits="WMS_NEW.Transaction.Inbound.Inbound" %>

<%@ Register Src="~/Transaction/Inbound/AscxControl/ucInboundDetail.ascx" TagPrefix="ucControls" TagName="ucInboundDetail" %>
<%@ Register Src="~/Transaction/Inbound/AscxControl/ucReceiptDetail.ascx" TagPrefix="ucControls" TagName="ucReceiptDetail" %>
<%@ Register Src="~/Transaction/Inbound/AscxControl/ucReceivePartial.ascx" TagPrefix="ucControls" TagName="ucReceivePartial" %>
<%@ Register Src="~/Report/AscxControls/ucReportViewer.ascx" TagPrefix="ucControls" TagName="ucReportViewer" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:ucReportViewer runat="server" ID="ucReportViewer" />

    <ucControls:PanelPopupEntity ID="popupEx" runat="server">
        <ControlTemplate>
            <div class="row">
                <ucControls:PanelControlRow ID="PanelControlRow0" runat="server" CssClass="row col-sm-9">
                    <ucControls:InputDropDown runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="ddlWarehouse" DataFieldValue="wh_master_id" IsPrimary="true" IsKey="true" UseDefaultDisplay="false" AutoPostBack="true" />
                    <ucControls:InputDropDown runat="server" ResourceGroup="owner" ResourceName="owner_id" ID="ddlOwner" DataFieldValue="owner_id" ControlSequence="1" AutoPostBack="true" IsPrimary="true" IsKey="true" UseDefaultDisplay="false" />
                    <ucControls:InputDropDown runat="server" ResourceGroup="inbound_master" ResourceName="order_type" ID="ddlOrderType" DataFieldValue="order_type" ComboType="String" IsPrimary="true" DisplayDefault="--Select--" IsKey="true" AutoPostBack="true" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="inbound_master" ResourceName="order_status" ID="txtOrderStatus" DataFieldValue="order_status" Enabled="false" />

                    <ucControls:InputCheckBox runat="server" ResourceGroup="inbound_master" ResourceName="generate_order" ID="chkGenOrderNo" LabelText="Gen. Order" CheckBoxType="Boolean" AutoPostBack="true" ColumnSpan="1" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="inbound_master" ResourceName="inbound_order_number" ID="txtInboundNo" DataFieldValue="inbound_order_number" IsPrimary="true" IsKey="true" ColumnSpan="2" MaxLength="10" />
                    <ucControls:InputTextDate runat="server" ResourceGroup="inbound_master" ResourceName="expected_delivery_date" ID="dtpExpectDate" DataFieldValue="expected_delivery_date" TextMode="Date" />


                    <ucControls:InputDropDownHD ID="ddlSupplier" runat="server" ResourceGroup="supplier" ResourceName="supplier_id" DataFieldValue="supplier_id" ControlGroup="Owner" ControlSequence="2" DisplayDefault="--Select--" IsPrimary="false" />
                    <ucControls:InputDropDownHD ID="ddlCustomer" runat="server" ResourceGroup="customer" ResourceName="customer_id" DataFieldValue="customer_id" ControlGroup="Owner" ControlSequence="2" DisplayDefault="--Select--" />

                    <ucControls:InputTextBox ID="txtCreateBy" runat="server" ResourceGroup="general" ResourceName="create_by" DataFieldValue="create_by" Enabled="false" />
                    <ucControls:InputTextDate ID="dtpDate" runat="server" ResourceGroup="general" ResourceName="create_date" DataFieldValue="create_date" Enabled="false" TextMode="DateTime" IsDateNow="true" />

                    <ucControls:InputDropDownHD ID="ddlMaketo" runat="server" ResourceGroup="inbound_master" ResourceName="make_to" DataFieldValue="make_to" ComboType="String" ControlSequence="2" DisplayDefault="--Select--" IsPrimary="true" AutoPostBack="true" />
                    <ucControls:InputDropDownHD ID="ddlProductionLine" runat="server" ResourceGroup="inbound_master" ResourceName="production_line" DataFieldValue="production_line" ComboType="String" ControlSequence="2" DisplayDefault="--Select--" />

                    <ucControls:InputTextBox ID="txtRefInboundOrderNumber" runat="server" ResourceGroup="inbound_master" ResourceName="ref_inbound_order_number" DataFieldValue="ref_inbound_order_number" IsPrimary="true" />
                    <ucControls:InputHidden ID="hidRefInboundOrderNumber" runat="server" />
                    <ucControls:InputTextBox ID="txtRefOutboundOrderNumber" runat="server" ResourceGroup="inbound_master" ResourceName="ref_outbound_order_number" DataFieldValue="ref_outbound_order_number" />

                    <div class="col-sm-1" style="padding-top: 20px">
                        <ucControls:ButtonExt ID="btnCopy" ResourceGroup="general" ResourceName="btn_copy" runat="server" Text="Copy" CausesValidation="false" CssClass="btn btn-sm btn-info" OnClick="btCopy_Click" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" />
                    </div>
                    <ucControls:InputCheckBox ID="chkAddProduction" runat="server" ResourceGroup="inbound_master" ResourceName="add_production" LabelText="Additional Production" DataFieldValue="add_production" CheckBoxType="String" AutoPostBack="false" ColumnSpan="2" />
                    <ucControls:InputDropDownHD ID="ddlRefLPN" runat="server" ResourceGroup="inbound_master" ResourceName="ref_lpn" DataFieldValue="ref_lpn" ComboType="Guid" ControlSequence="2" DisplayDefault="--Select--" />

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
                    <ucControls:PanelControlTab ID="PanelControlTab1" runat="server" PanelName="General" ResourceGroup="inbound_master" ResourceName="tab_general">
                        <ucControls:PanelControlRow ID="PanelControlRow2" runat="server">
                            <ucControls:InputTextBox ID="InputTextBox3" runat="server" ResourceGroup="inbound_master" ResourceName="description" DataFieldValue="description" IsMultiLine="true" />
                            <ucControls:InputTextBox ID="txtCloseBy" runat="server" ResourceGroup="inbound_master" ResourceName="close_by" DataFieldValue="close_by" Readonly="true" />
                            <ucControls:InputTextDate ID="txtCloseDate" runat="server" ResourceGroup="inbound_master" ResourceName="close_date" DataFieldValue="close_date" Readonly="true" />
                            <ucControls:InputTextBox ID="txtRemark" runat="server" ResourceGroup="inbound_master" ResourceName="close_remark" DataFieldValue="close_remark" Readonly="true" />
                        </ucControls:PanelControlRow>
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="PanelControlTab2" runat="server" PanelName="User Define" ResourceGroup="inbound_master" ResourceName="tab_user_define">
                        <ucControls:PanelControlRow runat="server" ID="PanelControlRow3">
                            <ucControls:InputTextBox runat="server" ResourceGroup="inbound_master" ResourceName="user_def1" ID="InputTextBox11" DataFieldValue="user_def1" />
                            <ucControls:InputTextBox runat="server" ResourceGroup="inbound_master" ResourceName="user_def4" ID="InputTextBox25" DataFieldValue="user_def4" />
                            <ucControls:InputTextNumber runat="server" ResourceGroup="inbound_master" ResourceName="user_def7" ID="InputTextBox20" DataFieldValue="user_def7" />
                            <ucControls:InputTextDate runat="server" ResourceGroup="inbound_master" ResourceName="user_def9" ID="txtUserDef9" DataFieldValue="user_def9" TextMode="Date" />
                            <ucControls:InputTextBox runat="server" ResourceGroup="inbound_master" ResourceName="user_def2" ID="InputTextBox18" DataFieldValue="user_def2" />
                            <ucControls:InputTextBox runat="server" ResourceGroup="inbound_master" ResourceName="user_def5" ID="InputTextBox26" DataFieldValue="user_def5" />
                            <ucControls:InputTextNumber runat="server" ResourceGroup="inbound_master" ResourceName="user_def8" ID="InputTextBox21" DataFieldValue="user_def8" />
                            <ucControls:InputTextDate runat="server" ResourceGroup="inbound_master" ResourceName="user_def10" ID="InputTextBox23" DataFieldValue="user_def10" TextMode="Date" />
                            <ucControls:InputTextBox runat="server" ResourceGroup="inbound_master" ResourceName="user_def3" ID="InputTextBox19" DataFieldValue="user_def3" />
                            <ucControls:InputTextBox runat="server" ResourceGroup="inbound_master" ResourceName="user_def6" ID="InputTextBox27" DataFieldValue="user_def6" />
                        </ucControls:PanelControlRow>
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="PanelControlTab4" runat="server" PanelName="Receive" ResourceGroup="inbound_master" ResourceName="tab_receive">
                        <ucControls:ucInboundDetail runat="server" ID="ucInboundDetail" />
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="PanelControlTab5" runat="server" PanelName="Partial Receive" ResourceGroup="inbound_master" ResourceName="tab_partial_receive">
                        <ucControls:ucReceivePartial runat="server" ID="ucReceivePartial" />
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="PanelControlTab6" runat="server" PanelName="Receipt Detail" ResourceGroup="inbound_master" ResourceName="tab_receipt_detail">
                        <ucControls:ucReceiptDetail runat="server" ID="ucReceiptDetail" />
                    </ucControls:PanelControlTab>
                </ControlTemplate>
            </ucControls:PanelTab>

            <ucControls:PanelPopup ID="popupOrderClose" runat="server" HeaderText="Close Order">
                <DataTemplate>
                    <ucControls:InputTextBox runat="server" ID="txtInboundOrderNoClose" ResourceGroup="inbound_master" ResourceName="inbound_order_number" Enabled="false" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="inbound_master" ResourceName="close_remark" ID="txtCloseRemark" />
                </DataTemplate>
                <CommandTemplate>
                    <span style="padding-left: 5px;">
                        <ucControls:ButtonExt ID="btComfirm" ResourceGroup="general" ResourceName="btn_confirm" runat="server" Text="Confirm Close Plan" CssClass="btn btn-danger" OnClick="btComfirmClose_Click" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" />
                    </span>
                </CommandTemplate>
            </ucControls:PanelPopup>

        </ControlTemplate>
        <CommandTemplate>
            <ucControls:ButtonExt ID="btSendLims" ResourceGroup="general" ResourceName="btn_send_lims" runat="server" Text="Send LIMS" CausesValidation="false" CssClass="btn btn-success" OnClick="btSendLims_Click" />
            <ucControls:ButtonExt ID="btCloseOrder" ResourceGroup="general" ResourceName="btn_close" runat="server" Text="Close Order" CausesValidation="false" CssClass="btn btn-danger" OnClick="btCloseOrder_Click" />
        </CommandTemplate>
    </ucControls:PanelPopupEntity>
    <ucControls:PanelPopup ID="popupConfirmCloseOrder" runat="server" StyleSize="Small" HeaderText="Close Order">
        <DataTemplate>
            <div class="row">
                <div class="col-sm-12">
                    <ucControls:InputHidden runat="server" ID="hidIsConfirm" />
                    <asp:Label ID="lblValidateCloseOrder" runat="server"></asp:Label>
                </div>
            </div>
        </DataTemplate>
        <CommandTemplate>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <ucControls:ButtonExt ID="btnSaveReject" runat="server" Text="YES" CssClass="btn btn-info" OnClick="btnComfirmCloseOrder_Click" ResourceGroup="general" ResourceName="btn_save" CausesValidation="false" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSaveReject" />
                </Triggers>
            </asp:UpdatePanel>
        </CommandTemplate>

    </ucControls:PanelPopup>

    <ucControls:PanelPopup ID="pnlImportFile" runat="server" HeaderText="" StyleSize="Small">
        <CommandTemplate>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <ucControls:ButtonExt ID="btUpload" ResourceGroup="general" ResourceName="btn_upload" runat="server" Text="Upload" CssClass="btn btn-success" CausesValidation="false" OnClick="btUpload_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btUpload" />
                </Triggers>
            </asp:UpdatePanel>
        </CommandTemplate>
        <DataTemplate>
            <asp:Panel ID="panel15" runat="server" Style="width: 100%; padding-left: 40px">
                <h2>IMPORT INBOUND EXCEL</h2>
                <div class="row  col-12">
                    <asp:FileUpload ID="fuExcel" CssClass="btn btn-info col-12" runat="server" AllowMultiple="false" accept="application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" />
                </div>
            </asp:Panel>
        </DataTemplate>
    </ucControls:PanelPopup>


    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inbound.InboundMaster" KeyField="KeyId" KeyType="Guid" VisibleExportTemplate="true"
        GridAllowRowEdit="true" GridAllowRowDelete="true" GridSortDefault="create_date desc,inbound_order_number">
        <CustomCommandTemplate>
            <ucControls:ButtonExt ID="btnImport" ResourceGroup="general" ResourceName="btnImport" runat="server" Text="IMPORT EXCEL" CssClass="btn btn-sm btn-warning" CausesValidation="false" OnClick="btnImport_Click" />
            <ucControls:ButtonExt ID="btReport" runat="server" Text="Report" CausesValidation="false" CssClass="btn btn-sm" OnClick="btReport_Click" ResourceGroup="general" ResourceName="btn_report" />
        </CustomCommandTemplate>
        <CustomSearchTemplate>
            <ucControls:InputDropDownHD ID="ddlCategory" runat="server" ResourceGroup="category" ResourceName="category_id" DataFieldValue="_category_id" ComboType="Guid" DisplayDefault="--All--" DefaultFilter="Equal" FixFilter="true" />
            <ucControls:InputTextBox ID="txtItemNumber" runat="server" ResourceGroup="item" ResourceName="item_number" DataFieldValue="_item_number" DefaultFilter="Contains" FixFilter="true" />
            <ucControls:InputTextDate ID="txtReceiveDate" runat="server" ResourceGroup="inventory" ResourceName="receive_date" DataFieldValue="_receive_date" DefaultFilter="Equal" FixFilter="true" TextMode="Date" />
            <ucControls:InputHidden ID="txtPageType" runat="server" DataFieldValue="_page_type" />
            <ucControls:InputDropDown ID="ddOrderType" runat="server" ResourceGroup="inbound_master" ResourceName="order_type" DataFieldValue="_order_type" ComboType="String" DisplayDefault="--All--" DefaultFilter="Equal" FixFilter="true" />
            <ucControls:InputTextDate ID="txtMfgDate" runat="server" ResourceGroup="inventory" ResourceName="mfg_date" DataFieldValue="_mfg_date" FixFilter="false" TextMode="Date" />
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="iColWarehouse" DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownAutoPostBack="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="attribute1" DataField="ref_inbound_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="false" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_master" ResourceName="order_type" ID="iColOrderType" DataField="order_type" FilterFormatType="Text" AllowFilter="false" AllowSort="false" ShowFilterNow="false" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_master" ResourceName="inbound_order_number" DataField="inbound_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_master" ResourceName="production_line" DataField="production_line" ID="iColProductionLine" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownFilterType="LazySearch" DefaultFilter="Contains" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="item_number" DataField="item_number" Width="400" AllowSort="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_master" ResourceName="order_status" ID="iColOrderStatus" DataField="order_status" FilterFormatType="Text" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="line_number" DataField="line_number" FormatType="Integer" AllowSort="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="total_qty" DataField="total_qty" FormatType="Number" AllowSort="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_master" ResourceName="expected_delivery_date" DataField="expected_delivery_date" FormatType="Date" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="customer" ResourceName="customer_code" ID="iColCustomer" DataField="customer_code" DataFieldFilter="customer_id" FilterFormatType="Guid" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownFilterType="LazySearch" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="customer" ResourceName="customer_name" DataField="customer_name" AllowFilter="false" AllowSort="true" ShowFilterNow="false" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="supplier" ResourceName="supplier_code" ID="iColSupplier" DataField="supplier_code" DataFieldFilter="supplier_id" FilterFormatType="Guid" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownFilterType="LazySearch" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="supplier" ResourceName="supplier_name" DataField="supplier_name" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="owner" ResourceName="owner_code" ID="iColOwner" DataField="owner_code" DataFieldFilter="owner_id" FilterFormatType="Guid" AllowFilter="true" AllowSort="false" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_by" DataField="create_by" AllowFilter="true" AllowSort="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_date" DataField="create_date" FormatType="DateTime" AllowFilter="true" AllowSort="true" ShowFilterNow="true" FilterFormatType="Date" />
        </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
