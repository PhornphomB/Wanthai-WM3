<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Outbound.aspx.cs" Inherits="WMS_NEW.Transaction.Outbound.Outbound" %>

<%@ Register Src="~/Transaction/Outbound/AscxControls/OutboundItem.ascx" TagPrefix="ucControls" TagName="OutboundItem" %>
<%@ Register Src="~/Report/AscxControls/ucReportViewer.ascx" TagPrefix="ucControls" TagName="ReportViewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:PanelPopup ID="popupReportPack" runat="server" HeaderText="Report Packing List">
        <DataTemplate>

            <ucControls:PanelControlRow runat="server" ID="PanelControlRowRpt">
                <ucControls:InputTextDate ID="txtPackShipDate" runat="server" IsPrimary="true" TextMode="Date" ValidateGroup="ReportPackConf" LabelText="Ship Date" ResourceGroup="outbound_master" ResourceName="ship_date" />
            </ucControls:PanelControlRow>

            <ucControls:ReportViewer runat="server" ID="reportReportPack" />

        </DataTemplate>
        <CommandTemplate>
            <ucControls:ButtonExt ID="btReportPackConf" runat="server" Text="View Report" CssClass="btn btn-sm btn-primary" OnClick="btReportPackConf_Click" CausesValidation="true" ValidationGroup="ReportPackConf" ResourceGroup="outbound_master" ResourceName="btn_report_pack" />
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:PanelPopupEntity ID="popupOutbound" runat="server">
        <ControlTemplate>
            <ucControls:PanelControlRow ID="PanelControlRow0" runat="server">
                <ucControls:InputDropDown ID="ddlWarehouse" runat="server" DataFieldValue="wh_master_id" IsPrimary="true" AutoPostBack="true" LabelText="Warehouse ID" ControlGroup="WH_GRB" ControlSequence="1" ResourceGroup="warehouse" ResourceName="wh_id" />
                <ucControls:InputDropDown ID="ddlOwner" runat="server" DataFieldValue="owner_id" IsPrimary="true" AutoPostBack="true" IsKey="true" LabelText="Owner Code" ControlGroup="OWN_GRB" ControlSequence="1" ResourceGroup="owner" ResourceName="owner_code" />
                <ucControls:InputDropDown ID="ddlOrderType" runat="server" DataFieldValue="order_type" IsPrimary="true" ComboType="String" DisplayDefault="--Select--" AutoPostBack="true" IsKey="true" Text="Order Type (Mvt.)" ResourceGroup="outbound_master" ResourceName="order_type" />
                <ucControls:InputTextBox ID="txtOrderStatus" runat="server" DataFieldValue="order_status" Enabled="false" LabelText="Order Status" ResourceGroup="outbound_master" ResourceName="order_status" />
                <ucControls:InputHidden ID="hidOutboundOrderMasterId" DataFieldValue="KeyId" runat="server" />
                <asp:UpdatePanel ID="updateGenOrderNo" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" class="row col-sm-2 col-md-2 col-lg-1">
                    <ContentTemplate>
                        <ucControls:InputCheckBox ID="chkGenOrderNo" runat="server" AutoPostBack="true" CheckBoxType="Boolean" LabelText="Gen.Order" ResourceGroup="outbound_master" ResourceName="generate_order" BaseContentCss="col-sm-12" />
                    </ContentTemplate>
                </asp:UpdatePanel>

                <ucControls:InputTextBox ID="txtOutboundNo" runat="server" DataFieldValue="outbound_order_number" IsPrimary="true" IsKey="true" LabelText="Outbound Order No" ResourceGroup="outbound_master" ResourceName="outbound_order_number" BaseContentCss="col-lg-2 col-md-4 col-sm-6" />
                <ucControls:InputTextDate ID="dtpDate" runat="server" DataFieldValue="order_date" Enabled="false" TextMode="Date" LabelText="Order Date" ResourceGroup="outbound_master" ResourceName="order_date" />
                <ucControls:InputTextBox ID="txtCustomerPerchaseOrder" runat="server" DataFieldValue="customer_purchase_order" LabelText="Cust PO No." ResourceGroup="outbound_master" ResourceName="customer_purchase_order" />
                <ucControls:InputTextInteger ID="txtPriority" runat="server" DataFieldValue="priority" LabelText="Priority" ResourceGroup="outbound_master" ResourceName="priority" />
            </ucControls:PanelControlRow>

            <ucControls:PanelTab ID="PanelTab1" runat="server">
                <ControlTemplate>
                    <ucControls:PanelControlTab ID="pnCustomer" runat="server" PanelName="Customer" ResourceGroup="outbound_master" ResourceName="tab_customer">
                        <ucControls:PanelControlRow ID="PanelControlRow1" runat="server">
                            <asp:UpdatePanel ID="updateCustomer" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" class="row col-sm-12">
                                <ContentTemplate>
                                    <ucControls:InputHidden ID="hidCustomerCode" runat="server" DataFieldValue="customer_code" DataFieldTempValue="customer_code" />
                                    <ucControls:InputDropDownHD ID="ddlCustomer" runat="server" DataFieldValue="customer_id" DataFieldTempValue="customer_id" IsPrimary="true" ControlGroup="OWN_GRB" ControlSequence="2" DisplayDefault="--Select--" AutoPostBack="true" LabelText="Customer Code" ResourceGroup="customer" ResourceName="customer_code" />
                                    <ucControls:InputTextBox ID="InputTextBox1" runat="server" DataFieldValue="customer_name" DataFieldTempValue="customer_name" Text="Customer Name" ResourceGroup="customer" ResourceName="customer_name" />
                                    <ucControls:InputTextBox ID="InputTextBox2" runat="server" DataFieldValue="cus_description" DataFieldTempValue="description" LabelText="Description" ResourceGroup="customer" ResourceName="description" />
                                    <ucControls:InputTextBox ID="InputTextBox3" runat="server" DataFieldValue="cus_addr_line_1" DataFieldTempValue="addr_line_1" LabelText="Address 1" ResourceGroup="customer" ResourceName="addr_line_1" />
                                    <ucControls:InputTextBox ID="InputTextBox4" runat="server" DataFieldValue="cus_addr_line_2" DataFieldTempValue="addr_line_2" LabelText="Address 2" ResourceGroup="customer" ResourceName="addr_line_2" />
                                    <ucControls:InputTextBox ID="InputTextBox5" runat="server" DataFieldValue="cus_addr_line_3" DataFieldTempValue="addr_line_3" LabelText="Address 3" ResourceGroup="customer" ResourceName="addr_line_3" />
                                    <ucControls:InputTextBox ID="InputTextBox6" runat="server" DataFieldValue="cus_city" DataFieldTempValue="city" LabelText="City" ResourceGroup="customer" ResourceName="City" />
                                    <ucControls:InputTextBox ID="InputTextBox7" runat="server" DataFieldValue="cus_province" DataFieldTempValue="province" LabelText="Province" ResourceGroup="customer" ResourceName="city" />
                                    <ucControls:InputTextBox ID="InputTextBox15" runat="server" DataFieldValue="cus_postal_code" DataFieldTempValue="postal_code" LabelText="Postal Code" ResourceGroup="customer" ResourceName="postal_code" />
                                    <ucControls:InputTextBox ID="InputTextBox8" runat="server" DataFieldValue="cus_country_code" DataFieldTempValue="country_code" LabelText="Country Code" ResourceGroup="customer" ResourceName="country_code" />
                                    <ucControls:InputTextBox ID="InputTextBox9" runat="server" DataFieldValue="cus_country_name" DataFieldTempValue="country_name" LabelText="Country" ResourceGroup="customer" ResourceName="country_name" />
                                    <ucControls:InputTextBox ID="InputTextBox10" runat="server" DataFieldValue="cus_phone" DataFieldTempValue="phone" LabelText="Phone Number" ResourceGroup="customer" ResourceName="phone" />
                                    <ucControls:InputTextBox ID="InputTextBox11" runat="server" DataFieldValue="cus_fax" DataFieldTempValue="fax" LabelText="Fax Number" ResourceGroup="customer" ResourceName="fax" />
                                    <ucControls:InputTextBox ID="InputTextBox12" runat="server" DataFieldValue="cus_email" DataFieldTempValue="email" LabelText="Email" ResourceGroup="customer" ResourceName="email" />
                                    <ucControls:InputTextBox ID="InputTextBox13" runat="server" DataFieldValue="cus_contact" DataFieldTempValue="contact" LabelText="Contact" ResourceGroup="customer" ResourceName="contact" />
                                    <div class="col-lg-3 col-md-6 col-sm-12">
                                        <div>
                                            <ucControls:LabelExt runat="server" ID="lblText" ResourceGroup="outbound_master" ResourceName="copy_customer" />
                                        </div>
                                        <div>
                                            <ucControls:ButtonExt ID="btCopyShip" runat="server" Text="Copy to Ship" CssClass="btn btn-sm btn-info" CausesValidation="false" OnClick="btCopyShip_Click" ResourceGroup="general" ResourceName="btn_copy_ship" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ucControls:PanelControlRow>
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="pnShipTo" runat="server" PanelName="Ship To" ResourceGroup="outbound_master" ResourceName="tab_ship">
                        <ucControls:PanelControlRow ID="PanelControlRow2" runat="server">
                            <asp:UpdatePanel ID="updateShipTo" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" class="row col-sm-12">
                                <ContentTemplate>
                                    <ucControls:InputTextBox ID="txtShipToCodeName" runat="server" LabelText="Ship to Code" ResourceGroup="outbound_master" ResourceName="ship_to_code" />
                                    <ucControls:InputHidden ID="hidShipToCode" runat="server" DataFieldValue="ship_to_code" DataFieldTempValue="customer_code" />
                                    <ucControls:InputTextBox ID="InputTextBox14" runat="server" DataFieldValue="ship_to_name" DataFieldTempValue="customer_name" Text="Ship to Name" ResourceGroup="outbound_master" ResourceName="ship_to_name" />
                                    <ucControls:InputTextBox ID="InputTextBox16" runat="server" DataFieldValue="ship_to_description" DataFieldTempValue="cus_description" IsMultiLine="true" LabelText="Description" ResourceGroup="outbound_master" ResourceName="ship_to_description" />
                                    <ucControls:InputTextBox ID="InputTextBox17" runat="server" DataFieldValue="ship_to_addr_line_1" DataFieldTempValue="cus_addr_line_1" Text="Address 1" ResourceGroup="outbound_master" ResourceName="ship_to_addr_line_1" />
                                    <ucControls:InputTextBox ID="InputTextBox18" runat="server" DataFieldValue="ship_to_addr_line_2" DataFieldTempValue="cus_addr_line_2" LabelText="Address 2" ResourceGroup="outbound_master" ResourceName="ship_to_addr_line_2" />
                                    <ucControls:InputTextBox ID="InputTextBox19" runat="server" DataFieldValue="ship_to_addr_line_3" DataFieldTempValue="cus_addr_line_3" LabelText="Address 3" ResourceGroup="outbound_master" ResourceName="ship_to_addr_line_3" />
                                    <ucControls:InputTextBox ID="InputTextBox20" runat="server" DataFieldValue="ship_to_city" DataFieldTempValue="cus_city" LabelText="City" ResourceGroup="outbound_master" ResourceName="ship_to_city" />
                                    <ucControls:InputTextBox ID="InputTextBox21" runat="server" DataFieldValue="ship_to_province" DataFieldTempValue="cus_province" LabelText="Province" ResourceGroup="customer" ResourceName="province" />
                                    <ucControls:InputTextBox ID="InputTextBox22" runat="server" DataFieldValue="ship_to_postal_code" DataFieldTempValue="cus_postal_code" LabelText="Postal Code" ResourceGroup="outbound_master" ResourceName="ship_to_postal_code" />
                                    <ucControls:InputTextBox ID="InputTextBox23" runat="server" DataFieldValue="ship_to_country_code" DataFieldTempValue="cus_country_code" Text="Country Code" ResourceGroup="outbound_master" ResourceName="ship_to_country_code" />
                                    <ucControls:InputTextBox ID="InputTextBox24" runat="server" DataFieldValue="ship_to_country_name" DataFieldTempValue="cus_country_name" LabelText="Country" ResourceGroup="outbound_master" ResourceName="ship_to_country_name" />
                                    <ucControls:InputTextBox ID="InputTextBox25" runat="server" DataFieldValue="ship_to_phone" DataFieldTempValue="cus_phone" LabelText="Phone Number" ResourceGroup="outbound_master" ResourceName="ship_to_phone" />
                                    <ucControls:InputTextBox ID="InputTextBox26" runat="server" DataFieldValue="ship_to_fax" DataFieldTempValue="cus_fax" LabelText="Fax Number" ResourceGroup="outbound_master" ResourceName="ship_to_fax" />
                                    <ucControls:InputTextBox ID="InputTextBox27" runat="server" DataFieldValue="ship_to_email" DataFieldTempValue="cus_email" LabelText="Email" ResourceGroup="outbound_master" ResourceName="ship_to_email" />
                                    <ucControls:InputTextBox ID="InputTextBox28" runat="server" DataFieldValue="ship_to_contact" DataFieldTempValue="cus_contact" LabelText="Contact" ResourceGroup="outbound_master" ResourceName="ship_to_contact" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ucControls:PanelControlRow>
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="panelCarrier" runat="server" PanelName="Carrier" ResourceGroup="outbound_master" ResourceName="tab_carrier">
                        <ucControls:PanelControlRow ID="PanelControlRow3" runat="server">
                            <ucControls:InputDropDownHD ID="ddCarrier" runat="server" DataFieldValue="carrier_id" DisplayDefault="--Select--" Text="Carrier" ResourceGroup="carrier" ResourceName="carrier_code" />
                            <ucControls:InputDropDown ID="ddTruckType" runat="server" DataFieldValue="truck_type_id" DisplayDefault="--Select--" Text="Truck Type" ResourceGroup="outbound_master" ResourceName="truck_type_id" />
                            <ucControls:InputTextBox ID="InputTextBox31" runat="server" DataFieldValue="truck_no" Text="Truck No" ResourceGroup="outbound_master" ResourceName="truck_no" />
                            <ucControls:InputTextBox ID="InputTextBox32" runat="server" DataFieldValue="driver_name" Text="Driver Name" ResourceGroup="outbound_master" ResourceName="driver_name" />
                            <ucControls:InputTextBox ID="InputTextBox29" runat="server" DataFieldValue="driver_license" Text="Driver License" ResourceGroup="outbound_master" ResourceName="driver_license" />
                            <ucControls:InputTextBox ID="InputTextBox33" runat="server" DataFieldValue="container_no" LabelText="Container No" ResourceGroup="outbound_master" ResourceName="container_no" />
                            <ucControls:InputTextBox ID="InputTextBox37" runat="server" DataFieldValue="seal_no" Text="Seal No" ResourceGroup="outbound_master" ResourceName="seal_no" />
                            <ucControls:InputTextBox ID="txtLoadId" runat="server" DataFieldValue="load_id" Text="Load ID" ResourceGroup="outbound_master" ResourceName="load_id" />
                            <ucControls:InputDropDownHD ID="ddDockDoor" runat="server" DataFieldValue="dock_door_id" DisplayDefault="--Select--" LabelText="Dock Door" ControlGroup="WH_GRB" ControlSequence="2" ResourceGroup="outbound_master" ResourceName="dock_door_id" />
                            <ucControls:InputTextInteger ID="InputTextBox39" runat="server" DataFieldValue="load_sequence_number" Text="Load Seq No" ResourceGroup="outbound_master" ResourceName="load_sequence_number" />
                            <ucControls:InputTextBox ID="InputTextBox40" runat="server" DataFieldValue="tracking_number" Text="Tracking Number" ResourceGroup="outbound_master" ResourceName="tracking_number" />
                        </ucControls:PanelControlRow>
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="panelDate" runat="server" PanelName="Date" ResourceGroup="outbound_master" ResourceName="tab_date">
                        <ucControls:PanelControlRow ID="PanelControlRow4" runat="server">
                            <ucControls:InputTextDate ID="InputTextDate1" runat="server" DataFieldValue="loading_date_plan" TextMode="DateTime" Text="Planed Loading Date" ResourceGroup="outbound_master" ResourceName="loading_date_plan" />
                            <ucControls:InputTextDate ID="InputTextDate2" runat="server" DataFieldValue="delivery_date_plan" TextMode="DateTime" LabelText="Planed Delivery Date" ResourceGroup="outbound_master" ResourceName="delivery_date_plan" />
                            <ucControls:InputTextDate ID="InputTextDate3" runat="server" DataFieldValue="ship_date_plan" TextMode="DateTime" IsPrimary="true" Text="Planed Ship Date" ResourceGroup="outbound_master" ResourceName="ship_date_plan" />
                            <ucControls:InputTextDate ID="InputTextDate4" runat="server" DataFieldValue="loading_date_actual" TextMode="DateTime" Text="Actual Loading Date" ResourceGroup="outbound_master" ResourceName="loading_date_actual" />
                            <ucControls:InputTextDate ID="InputTextDate6" runat="server" DataFieldValue="ship_date_actual" TextMode="DateTime" LabelText="Actual Shipping Date" ResourceGroup="outbound_master" ResourceName="ship_date_actual" />
                            <ucControls:InputTextDate ID="InputTextDate5" runat="server" DataFieldValue="delivery_date_actual" TextMode="DateTime" LabelText="Actual Delivery Date" ResourceGroup="outbound_master" ResourceName="delivery_date_actual" />
                            <ucControls:InputTextDate ID="InputTextDate9" runat="server" DataFieldValue="close_date" TextMode="DateTime" LabelText="Close Date" ResourceGroup="outbound_master" ResourceName="close_date" />
                        </ucControls:PanelControlRow>
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="panelInfo" runat="server" PanelName="Information" ResourceGroup="outbound_master" ResourceName="tab_information">
                        <ucControls:PanelControlRow ID="PanelControlRow5" runat="server">
                            <ucControls:InputTextBox ID="InputTextBox30" runat="server" DataFieldValue="customer_order_number" LabelText="Customer Order No" ResourceGroup="outbound_master" ResourceName="customer_order_number" />
                            <ucControls:InputTextBox ID="InputTextBox35" runat="server" DataFieldValue="rush_order" Text="Rush Order" ResourceGroup="outbound_master" ResourceName="rush_order" />
                            <ucControls:InputDropDown ID="ddBackFlag" runat="server" DataFieldValue="back_order_flag" ComboType="String" DisplayDefault="--Select--" LabelText="Back Order Flag" ResourceGroup="outbound_master" ResourceName="back_order_flag" />
                            <ucControls:InputDropDownHD ID="ddTransportBy" runat="server" DataFieldValue="transport_by_code" DisplayDefault="--Select--" LabelText="Transport By" ComboType="String" ResourceGroup="outbound_master" ResourceName="transport_by_code" />
                            <ucControls:InputDropDown ID="ddWarehouseByRule" runat="server" ComboType="Guid" DisplayDefault="--Select--" ValidateMessage="Please select destination warehouse" IsPrimary="true" LabelText="Department" ResourceGroup="outbound_master" ResourceName="department_wh" />
                            <ucControls:InputTextBox ID="txtWarehouseByRule" runat="server" DataFieldValue="department" LabelText="Department" ResourceGroup="outbound_master" ResourceName="department" />
                            </ContentTemplate>

                            <ucControls:InputTextBox ID="InputTextBox41" runat="server" DataFieldValue="description" IsMultiLine="true" LabelText="Description" ResourceGroup="outbound_master" ResourceName="description" />
                            <ucControls:InputTextBox ID="txtCancelBy" runat="server" DataFieldShowValue="cancel_by" LabelText="Cancel By" ResourceGroup="outbound_master" ResourceName="cancel_by" />
                            <ucControls:InputTextDate ID="txtCancelDate" runat="server" DataFieldShowValue="cancel_date" TextMode="Date" Text="Cancel Date" ResourceGroup="outbound_master" ResourceName="cancel_date" />
                            <ucControls:InputTextBox ID="InputTextBox44" runat="server" DataFieldShowValue="cancel_remark" IsMultiLine="true" Text="Cancel Remark" ResourceGroup="outbound_master" ResourceName="cancel_remark" />
                        </ucControls:PanelControlRow>
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="panelUserDefine" runat="server" PanelName="User Defined Address" ResourceGroup="outbound_master" ResourceName="tab_user_define">
                        <ucControls:PanelControlRow ID="PanelControlRow6" runat="server">
                            <ucControls:InputTextBox ID="txtUdf1" runat="server" DataFieldValue="user_def1" Text="User Defined Field 1" ResourceGroup="outbound_master" ResourceName="user_def1" />
                            <ucControls:InputTextBox ID="txtUdf2" runat="server" DataFieldValue="user_def2" Text="User Defined Field 2" ResourceGroup="outbound_master" ResourceName="user_def2" />
                            <ucControls:InputTextBox ID="InputTextBox47" runat="server" DataFieldValue="user_def3" LabelText="User Defined Field 3" ResourceGroup="outbound_master" ResourceName="user_def3" />
                            <ucControls:InputTextBox ID="InputTextBox48" runat="server" DataFieldValue="user_def4" LabelText="User Defined Field 4" ResourceGroup="outbound_master" ResourceName="user_def4" />
                            <ucControls:InputTextBox ID="InputTextBox49" runat="server" DataFieldValue="user_def5" LabelText="User Defined Field 5" ResourceGroup="outbound_master" ResourceName="user_def5" />
                            <ucControls:InputTextBox ID="InputTextBox50" runat="server" DataFieldValue="user_def6" Text="User Defined Field 6" ResourceGroup="outbound_master" ResourceName="user_def6" />
                            <ucControls:InputTextNumber ID="InputTextNumber1" runat="server" DataFieldValue="user_def7" NumberType="Double" LabelText="User Defined Field 7" ResourceGroup="outbound_master" ResourceName="user_def7" />
                            <ucControls:InputTextNumber ID="InputTextNumber2" runat="server" DataFieldValue="user_def8" NumberType="Double" LabelText="User Defined Field 8" ResourceGroup="outbound_master" ResourceName="user_def8" />
                            <ucControls:InputTextDate ID="InputTextDate7" runat="server" DataFieldValue="user_def9" TextMode="Date" ResourceGroup="outbound_master" ResourceName="user_def9" LabelText="User Defined Field 9" />
                            <ucControls:InputTextDate ID="InputTextDate8" runat="server" DataFieldValue="user_def10" TextMode="Date" ResourceGroup="outbound_master" ResourceName="user_def10" LabelText="User Defined Field 10" />
                        </ucControls:PanelControlRow>
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="panelDetail" runat="server" PanelName="Item Detail" ResourceGroup="outbound_master" ResourceName="tab_detail">
                        <ucControls:OutboundItem runat="server" ID="OutboundItem1" />
                    </ucControls:PanelControlTab>

                    <%--<ucControls:PanelControlTab ID="panelComment" runat="server" PanelName="Comment">
                    </ucControls:PanelControlTab>--%>
                </ControlTemplate>
            </ucControls:PanelTab>

            <ucControls:PanelPopup ID="popupCancelOrder" runat="server" HeaderText="Confirm Cancel Order" StyleSize="Small" StyleColor="Danger">
                <DataTemplate>
                    <ucControls:PanelControlRow ID="PanelControlRow7" runat="server">
                        <ucControls:InputTextBox ID="txtOutboundCancelOrder" runat="server" Enabled="false" Text="Outbound Order No" ResourceGroup="outbound_master" ResourceName="outbound_order_number" />
                        <ucControls:InputTextBox ID="txtCancelRemark" runat="server" IsMultiLine="true" Text="Close Remark" ResourceGroup="outbound_master" ResourceName="close_remark" />
                    </ucControls:PanelControlRow>
                </DataTemplate>
                <CommandTemplate>
                    <span>
                        <ucControls:ButtonExt ID="btComfirm" runat="server" Text="Confirm" CssClass="btn btn-sm btn-danger" OnClick="btCancelComfirm_Click" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" ResourceGroup="general" ResourceName="btn_confirm" />
                    </span>
                </CommandTemplate>
            </ucControls:PanelPopup>

        </ControlTemplate>
        <CommandTemplate>
            <ucControls:ButtonExt ID="btCancelOrder" runat="server" Text="Cancel Order" CssClass="btn btn-sm btn-danger" OnClick="btCancelOrder_Click" ResourceGroup="general" ResourceName="btn_cancel_order" />
        </CommandTemplate>
    </ucControls:PanelPopupEntity>


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
                <h2>IMPORT OUTBOUND EXCEL</h2>
                <div class="row  col-12">
                    <asp:FileUpload ID="fuExcel" CssClass="btn btn-info col-12" runat="server" AllowMultiple="false" accept="application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"  />         
                </div>
            </asp:Panel>
        </DataTemplate>
    </ucControls:PanelPopup>


    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.Outbound" KeyField="KeyId" KeyType="Guid" VisibleExportTemplate="true"
        GridAllowRowEdit="true" GridAllowRowDelete="true" GridSortDefault="create_date desc,outbound_order_number">
        <CustomCommandTemplate>
               <ucControls:ButtonExt ID="btnImport" ResourceGroup="general" ResourceName="btnImport" runat="server" Text="IMPORT EXCEL" CssClass="btn btn-sm btn-warning" CausesValidation="false" OnClick="btnImport_Click" />
            <ucControls:ButtonExt ID="btReportPacking" runat="server" Text="Report Packing" CssClass="btn btn-sm" CausesValidation="false" OnClick="btReportPacking_Click" Visible="false" />
        </CustomCommandTemplate>
        <CustomSearchTemplate>
            <ucControls:InputHidden ID="hidSessionUser" runat="server" DataFieldValue="_userID" />
            <ucControls:InputHidden ID="hidIsFirstLoad" runat="server" DataFieldValue="_isFirstLoad" />

            <ucControls:InputDropDownHD ID="ddlCategory" runat="server" DataFieldValue="_cateID" DisplayDefault="--All--" LabelText="Item Category" ResourceGroup="category" ResourceName="description" />
            <ucControls:InputTextBox ID="txtItemNumber" runat="server" DataFieldValue="_itemNumber" DefaultFilter="Contains" LabelText="Item" ResourceGroup="item" ResourceName="item_number" />
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Warehouse" DataField="wh_id" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="warehouse" ResourceName="wh_id" />
            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" FormatType="Guid" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="owner" ResourceName="owner_code" />
            <%--<ucControls:GridColumnExt ID="GridColumnExt19" runat="server" HeaderText="Province" DataField="province" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="customer" ResourceName="province" />--%>
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Order Type (Mvt)" DataField="order_type" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="outbound_master" ResourceName="order_type" />
            <ucControls:GridColumnExt ID="GridColumnExt18" runat="server" HeaderText="Customer PO" DataField="customer_purchase_order" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="customer_purchase_order" />
            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Order No" DataField="outbound_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="outbound_order_number" />
            <%--<ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Load ID" DataField="load_id" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="load_id" />--%>
            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Status" DataField="order_status" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="outbound_master" ResourceName="order_status" />
            <ucControls:GridColumnExt ID="GridColumnExt17" runat="server" HeaderText="Total QTY" DataField="total_quantity" FormatType="Number" AllowFilter="true" AllowSort="true" ShowFilterNow="false" ResourceGroup="general" ResourceName="total_qty" />
            <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" HeaderText="Customer Code" DataField="customer_code" AllowFilter="true" AllowSort="true" ShowFilterNow="false" ResourceGroup="customer" ResourceName="customer_code" />
            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Customer Name" DataField="customer_name" DataFieldFilter="customer_id" FormatType="Guid" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownFilterType="LazySearch" DropDownDisplayDefault="--All--" ResourceGroup="customer" ResourceName="customer_name" />
            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Order Date" DataField="order_date" FormatType="DateTime" AllowFilter="true" AllowSort="true" ShowFilterNow="true" FilterFormatType="Date" ResourceGroup="outbound_master" ResourceName="order_date" />
            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Delivery Date Plan" DataField="delivery_date_plan" FormatType="DateTime" AllowFilter="true" AllowSort="true" ShowFilterNow="true" FilterFormatType="Date" ResourceGroup="outbound_master" ResourceName="delivery_date_plan" />
            <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Ship Date Plan" DataField="ship_date_plan" FormatType="DateTime" AllowFilter="true" AllowSort="true" ShowFilterNow="true" FilterFormatType="Date" ResourceGroup="outbound_master" ResourceName="ship_date_plan" />
            <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Ship Date Actual" DataField="ship_date_actual" FormatType="DateTime" AllowFilter="true" AllowSort="true" ShowFilterNow="true" FilterFormatType="Date" ResourceGroup="outbound_master" ResourceName="ship_date_actual" />
            <ucControls:GridColumnExt ID="GridColumnExt20" runat="server" HeaderText="Invoice No" DataField="customer_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="customer_order_number" />
            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="Create By" DataField="create_by" ResourceGroup="general" ResourceName="create_by" />
            <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Create Date" DataField="create_date" FormatType="DateTime" ResourceGroup="general" ResourceName="create_date" />
            <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Priority" DataField="priority" ResourceGroup="outbound_master" ResourceName="priority" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />

        </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
