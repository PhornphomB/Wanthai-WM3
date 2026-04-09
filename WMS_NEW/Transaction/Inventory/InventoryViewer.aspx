<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="InventoryViewer.aspx.cs" Inherits="WMS_NEW.Transaction.Inventory.InventoryViewer" %>

<%@ Register Src="~/Report/AscxControls/ucReportViewer.ascx" TagPrefix="ucControls" TagName="ReportViewer" %>
<%@ Register Src="~/Report/AscxControls/ucReportStockCard.ascx" TagPrefix="ucControls" TagName="ucReportStockCard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:ReportViewer runat="server" ID="ReportViewer" />
    <ucControls:ucReportStockCard runat="server" ID="ucReportStockCard" />
    <div class="col-lg-12 col-md-12 col-sm-12 pt-3 background-base">

        <asp:UpdatePanel ID="updateFilter" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" CssClass="col-sm-12">
            <ContentTemplate>
                <ucControls:PanelControlRow ID="pnFilter" runat="server" CssClass="row">
                    <ucControls:InputHidden ID="hid_active_load" runat="server" DataFieldValue="_active_load" />
                    <ucControls:InputDropDown ID="ddlWareHouse" runat="server" Filterable="true" DataFieldValue="wh_master_id" DisplayDefault="-- All --" LabelText="Warehouse ID" ResourceGroup="warehouse" ResourceName="wh_id" BaseContentCss="col-sm-2" />
                    <ucControls:InputDropDown ID="ddlOwner" runat="server" Filterable="true" DisplayDefault="-- All --" DataFieldValue="owner_id" AutoPostBack="true" LabelText="Owner" ResourceGroup="owner" ResourceName="owner_code" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextBox ID="txtZone" runat="server" Filterable="true" DataFieldValue="zone" LabelText="Zone" ResourceGroup="zone" ResourceName="zone" BaseContentCss="col-sm-2" />
                    <%--                    <ucControls:InputTextBox ID="txtLocation" runat="server" Filterable="true" DataFieldValue="location" LabelText="Location" ResourceGroup="location" ResourceName="location" BaseContentCss="col-sm-2" />--%>
                    <ucControls:InputDropDownHD ID="ddlLocation" runat="server" Filterable="true" DataFieldValue="location" ComboType="String" LabelText="Location" ResourceGroup="location" ResourceName="location" BaseContentCss="col-sm-2" DisplayDefault="-- All --" />

                    <ucControls:InputDropDownHD ID="ddlItemCategory" runat="server" Filterable="true" DisplayDefault="-- All --" DataFieldValue="category_id" LabelText="Item Category" ResourceGroup="category" ResourceName="category" BaseContentCss="col-sm-2" />
                    <ucControls:InputDropDownHD ID="ddlItem" runat="server" Filterable="true" DataFieldValue="item_number" ComboType="String" LabelText="Item" ResourceGroup="item" ResourceName="item_number" BaseContentCss="col-sm-2" DisplayDefault="-- All --" DefaultFilter="Contains" Visible="false" />
                    <ucControls:InputTextBox ID="txtItem" runat="server" Filterable="true" DataFieldValue="item_number" LabelText="Item" ResourceGroup="item" ResourceName="item_number" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextBox ID="txtDescription" runat="server" Filterable="true" DataFieldValue="description" LabelText="Description" ResourceGroup="item" ResourceName="description" BaseContentCss="col-sm-2" />
                    <%--<ucControls:InputTextBox ID="txtParentLPN" runat="server" Filterable="true" DataFieldValue="parent_lpn" LabelText="Parent LPN" ResourceGroup="inventory" ResourceName="parent_lpn" BaseContentCss="col-sm-2" />--%>
                    <ucControls:InputTextBox ID="txtLPN" runat="server" Filterable="true" DataFieldValue="lpn" LabelText="LPN" ResourceGroup="inventory" ResourceName="lpn" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextBox ID="txtBatch" runat="server" Filterable="true" DataFieldValue="lot_number" LabelText="Batch" ResourceGroup="inventory" ResourceName="lot_number" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextDate ID="dtpMfgDate" runat="server" Filterable="true" TextMode="Date" DataFieldValue="mfg_date" LabelText="MFG Date" ResourceGroup="inventory" ResourceName="mfg_date" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextDate ID="dtpExpiryDate" runat="server" Filterable="true" TextMode="Date" DataFieldValue="expiry_date" LabelText="Expiry Date" ResourceGroup="inventory" ResourceName="expiry_date" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextBox ID="txtSerial" runat="server" Filterable="true" DataFieldValue="serial_number" LabelText="Serial Number" ResourceGroup="inventory" ResourceName="serial_number" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextDate ID="dtpReceiveDate" runat="server" Filterable="true" TextMode="Date" DataFieldValue="receive_date" LabelText="Receive Date" ResourceGroup="receipt_detail" ResourceName="receive_date" BaseContentCss="col-sm-2" />
                    <ucControls:InputDropDown ID="ddlInventoryStatus" runat="server" Filterable="true" ComboType="String" DataFieldValue="inv_status" LabelText="Inventory Status" ResourceGroup="inventory" ResourceName="inv_status" BaseContentCss="col-sm-2" AllowSearchMultiValue="true" UseDefaultDisplay="false" />
                    <ucControls:InputTextBox ID="txtDGCode" runat="server" Filterable="true" DataFieldValue="dg_code" LabelText="DG Code" ResourceGroup="item" ResourceName="dg_code" VisibleExt="false" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextNumber ID="txtQty" runat="server" Filterable="true" DataFieldValue="quantity" LabelText="Qty" ResourceGroup="inventory" ResourceName="quantity" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextNumber ID="txtQtyAllocate" runat="server" Filterable="true" DataFieldValue="quantity_allocated" LabelText="Qty Allocate" ResourceGroup="inventory" ResourceName="quantity_allocated" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextBox ID="txtItemGrade" runat="server" Filterable="true" DataFieldValue="grade" LabelText="Grade" ResourceGroup="item" ResourceName="grade" VisibleExt="false" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextBox ID="txtAttribute1" runat="server" Filterable="true" DataFieldValue="attribute1" LabelText="Attribute1" ResourceGroup="inventory" ResourceName="attribute1" BaseContentCss="col-sm-2" />
                    <%--<ucControls:InputTextBox ID="txtAttribute2" runat="server" Filterable="true" DataFieldValue="attribute2" LabelText="Attribute2" ResourceGroup="inventory" ResourceName="attribute2" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextBox ID="txtAttribute3" runat="server" Filterable="true" DataFieldValue="attribute3" LabelText="Attribute3" ResourceGroup="inventory" ResourceName="attribute3" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextBox ID="txtAttribute4" runat="server" Filterable="true" DataFieldValue="attribute4" LabelText="Attribute4" ResourceGroup="inventory" ResourceName="attribute4" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextBox ID="txtAttribute5" runat="server" Filterable="true" DataFieldValue="attribute5" LabelText="Attribute5" ResourceGroup="inventory" ResourceName="attribute5" BaseContentCss="col-sm-2" />--%>
                    <ucControls:InputTextNumber ID="txtAging" runat="server" Filterable="true" DataFieldValue="aging" LabelText="Qty" ResourceGroup="inventory" ResourceName="aging" BaseContentCss="col-sm-2" />
                    <ucControls:InputTextNumber ID="txtRemainAging" runat="server" Filterable="true" DataFieldValue="remain_aging" LabelText="Qty" ResourceGroup="inventory" ResourceName="remain_aging" BaseContentCss="col-sm-2" />
                </ucControls:PanelControlRow>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="updateContent" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" CssClass="row col-sm-12" style="padding-bottom: 14px;">
            <ContentTemplate>
                <ucControls:ButtonExt ID="btToggle" runat="server" Text="Hide Filter" CssClass="btn btn-sm btn-info" CausesValidation="false" OnClick="btToggle_Click" />
                <ucControls:ButtonExt ID="btSearch" runat="server" Text="Search" CssClass="btn btn-sm btn-success" CausesValidation="false" OnClick="btSearch_Click" ResourceGroup="General" ResourceName="Search" />
                <ucControls:ButtonExt ID="btClear" runat="server" Text="Clear" CssClass="btn btn-sm btn-danger" CausesValidation="false" OnClick="btClear_Click" ResourceGroup="General" ResourceName="Clear" />
                <span class="ml-1">
                    <ucControls:ButtonExt ID="btReport" runat="server" Text="Report" CssClass="btn btn-sm" CausesValidation="false" OnClick="btReport_Click" ResourceGroup="general" ResourceName="btn_report" />
                    <ucControls:ButtonExt ID="btReportStockCard" runat="server" Text="Report Stock Card" CssClass="btn btn-sm" CausesValidation="false" OnClick="btReportStockCard_Click" ResourceGroup="general" ResourceName="report_stock_card" />

                </span>
            </ContentTemplate>
        </asp:UpdatePanel>

        <ucControls:PanelTab ID="PanelTab1" runat="server">
            <ControlTemplate>

                <ucControls:PanelControlTab ID="pnItem" runat="server" PanelName="By Item" ResourceGroup="inventory" ResourceName="TabByItem">
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Panel runat="server" ID="pnRule" Style="display: flex">
                                <%--  <div class="badge badge-danger pr-1" style="width: 1rem; height: 1rem">
        <asp:Label ID="lblBeforeExp" runat="server"></asp:Label>
    </div>--%>
                                <div class="btn-danger pr-1" style="width: 1rem; height: 1rem"></div>
                                <div class="pr-1"></div>
                                <asp:Label ID="lblExp" runat="server"></asp:Label>
                                <div class="pr-3"></div>
                                <div class="btn-warning pr-1" style="width: 1rem; height: 1rem"></div>
                                <div class="pr-1"></div>
                                <asp:Label ID="lblBeforeExp" runat="server"></asp:Label>

                            </asp:Panel>
                        </div>
                        <div class="col-md-6">
                            <asp:UpdatePanel runat="server" ID="UpdateGridExtItem" UpdateMode="Conditional" Style="display: flex; float: right;">
                                <ContentTemplate>
                                    <asp:Label ID="GridExtItemQty" runat="server"></asp:Label>
                                    <div class="pr-3"></div>
                                    <asp:Label ID="GridExtItemAllocate" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <%--<asp:Panel runat="server" ID="Panel1" Style="display: flex">
                                <asp:Label ID="Label1" runat="server">Quantity: 1111</asp:Label>
                                <div class="pr-3"></div>
                                <asp:Label ID="Label2" runat="server">Allocate Qty: 1111</asp:Label>

                            </asp:Panel>--%>
                        </div>
                    </div>

                    <ucControls:GridExt ID="GridExtItem" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryViewer.InventoryItem" KeyField="KeyId" KeyType="Guid"
                        DisableButtonSearch="true" GridSortDefault="location asc,cate_description asc,item_number asc,lot_number asc" OnGridRefreshClick="btSearch_Click">
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" HeaderText="Warehouse" DataField="wh_id" DataFieldFilter="wh_master_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt35" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Zone" DataField="zone" AllowSort="true" ResourceGroup="zone" ResourceName="zone" />
                            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Location" DataField="location" AllowSort="true" ResourceGroup="location" ResourceName="location" />
                            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Item" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Item Description" DataField="description" Width="250" AllowSort="true" ResourceGroup="item" ResourceName="description" Visible="true" />
                            <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Quantity" DataField="quantity" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity" />
                            <ucControls:GridColumnExt ID="GridColumnExt70" runat="server" HeaderText="Quantity Allocate" DataField="quantity_allocated" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity_allocated" />
                            <ucControls:GridColumnExt ID="GridColumnExt100" runat="server" HeaderText="Quantity Avalible" DataField="quantity_avalible" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity_avalible" />
                            <ucControls:GridColumnExt ID="GridColumnExt71" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                            <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Inventory Status" DataField="inv_status" AllowSort="true" ResourceGroup="inventory" ResourceName="inv_status" />
                            <%--<ucControls:GridColumnExt ID="GridColumnExt65" runat="server" HeaderText="Parent LPN" DataField="parent_lpn" AllowSort="true" ResourceGroup="inventory" ResourceName="parent_lpn" />--%>
                            <ucControls:GridColumnExt ID="GridColumnExt64" runat="server" HeaderText="LPN" DataField="lpn" AllowSort="true" ResourceGroup="inventory" ResourceName="lpn" />
                            <ucControls:GridColumnExt ID="GridColumnExt99" runat="server" HeaderText="Attribute1" DataField="attribute1" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute1" />
                            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Batch Number" DataField="lot_number" AllowSort="true" ResourceGroup="inventory" ResourceName="lot_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt60" runat="server" HeaderText="MFG Date" DataField="mfg_date" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="mfg_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="Expiry Date" DataField="expiry_date" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="expiry_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt74" runat="server" HeaderText="Serial Number" DataField="serial_number" AllowSort="true" ResourceGroup="inventory" ResourceName="serial_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt52" runat="server" HeaderText="Item Category" DataField="cate_description" DataFieldFilter="category_id" AllowSort="true" ResourceGroup="category" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt73" runat="server" HeaderText="Receipt Date" DataField="receipt_date" AllowSort="true" FormatType="Date" ResourceGroup="receipt_detail" ResourceName="receive_date" />

                            <ucControls:GridColumnExt ID="GridColumnExt98" runat="server" DataField="isBeforeExp" Visible="false" />
                            <ucControls:GridColumnExt ID="GridColumnExt95" runat="server" DataField="isExp" Visible="false" />

                        </CustomColumnTemplate>

                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnAging" runat="server" PanelName="By Aging" ResourceGroup="inventory" ResourceName="TabByaging">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:UpdatePanel runat="server" ID="UpdateGridExtAging" UpdateMode="Conditional" Style="display: flex; float: right;">
                                <ContentTemplate>
                                    <asp:Label ID="GridExtAgingQty" runat="server"></asp:Label>
                                    <div class="pr-3"></div>
                                    <asp:Label ID="GridExtAgingAllocate" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <ucControls:GridExt ID="GridExtAging" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryViewer.InventoryAging" KeyField="KeyId" KeyType="Guid"
                        DisableButtonSearch="true" DisableFirstSearch="true" GridSortDefault="location asc,cate_description asc,item_number asc,lot_number asc" OnGridRefreshClick="btSearch_Click">
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" HeaderText="Warehouse" DataField="wh_id" DataFieldFilter="wh_master_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt24" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="Zone" DataField="zone" AllowSort="true" ResourceGroup="zone" ResourceName="zone" />
                            <ucControls:GridColumnExt ID="GridColumnExt26" runat="server" HeaderText="Location" DataField="location" AllowSort="true" ResourceGroup="location" ResourceName="location" />
                            <ucControls:GridColumnExt ID="GridColumnExt28" runat="server" HeaderText="Item" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt29" runat="server" HeaderText="Item Description" DataField="description" Width="250" AllowSort="true" ResourceGroup="item" ResourceName="description" Visible="true" />
                            <ucControls:GridColumnExt ID="GridColumnExt30" runat="server" HeaderText="Quantity" DataField="quantity" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity" />
                            <ucControls:GridColumnExt ID="GridColumnExt31" runat="server" HeaderText="Quantity Allocate" DataField="quantity_allocated" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity_allocated" />
                            <ucControls:GridColumnExt ID="GridColumnExt32" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                            <ucControls:GridColumnExt ID="GridColumnExt36" runat="server" HeaderText="Inventory Status" DataField="inv_status" AllowSort="true" ResourceGroup="inventory" ResourceName="inv_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt37" runat="server" HeaderText="Parent LPN" DataField="parent_lpn" AllowSort="true" ResourceGroup="inventory" ResourceName="parent_lpn" />
                            <ucControls:GridColumnExt ID="GridColumnExt38" runat="server" HeaderText="LPN" DataField="lpn" AllowSort="true" ResourceGroup="inventory" ResourceName="lpn" />
                            <ucControls:GridColumnExt ID="GridColumnExt66" runat="server" HeaderText="Batch Number" DataField="lot_number" AllowSort="true" ResourceGroup="inventory" ResourceName="lot_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt61" runat="server" HeaderText="MFG Date" DataField="mfg_date" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="mfg_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt67" runat="server" HeaderText="Expiry Date" DataField="expiry_date" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="expiry_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt75" runat="server" HeaderText="Serial Number" DataField="serial_number" AllowSort="true" ResourceGroup="inventory" ResourceName="serial_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt76" runat="server" HeaderText="Item Category" DataField="cate_description" DataFieldFilter="category_id" AllowSort="true" ResourceGroup="category" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt77" runat="server" HeaderText="Receipt Date" DataField="receipt_date" AllowSort="true" FormatType="Date" ResourceGroup="receipt_detail" ResourceName="receive_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt78" runat="server" HeaderText="Aging" DataField="aging" FormatType="Integer" AllowSort="true" ResourceGroup="inventory" ResourceName="aging" />
                            <ucControls:GridColumnExt ID="GridColumnExt79" runat="server" HeaderText="Remain Aging" DataField="remain_aging" FormatType="Integer" AllowSort="true" ResourceGroup="inventory" ResourceName="remain_aging" />
                            <%--<ucControls:GridColumnExt ID="GridColumnExt100" runat="server" HeaderText="Inbound Order Number" DataField="inbound_order_number" AllowSort="true" />--%>
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                <%--<ucControls:PanelControlTab ID="pnSerial" runat="server" PanelName="By Serial" ResourceGroup="inventory" ResourceName="TabBySerial">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:UpdatePanel runat="server" ID="UpdateGridExtSerial" UpdateMode="Conditional" Style="display: flex; float: right;">
                                <ContentTemplate>
                                    <asp:Label ID="GridExtSerialQty" runat="server"></asp:Label>
                                    <div class="pr-3"></div>
                                    <asp:Label ID="GridExtSerialAllocate" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <ucControls:GridExt ID="GridExtSerial" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryViewer.InventorySerial" KeyField="KeyId" KeyType="Guid"
                        GridAllowRowEdit="false" GridAllowRowDelete="false" GridAllowSelectBox="false" DisableButtonSearch="true" DisableFirstSearch="true"
                        GridSortDefault="location asc,cate_description asc,item_number asc,lot_number asc" OnGridRefreshClick="btSearch_Click">
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Warehouse" DataField="wh_id" DataFieldFilter="wh_master_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt27" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt40" runat="server" HeaderText="Zone" DataField="zone" AllowSort="true" ResourceGroup="zone" ResourceName="zone" />
                            <ucControls:GridColumnExt ID="GridColumnExt54" runat="server" HeaderText="Location" DataField="location" AllowSort="true" ResourceGroup="location" ResourceName="location" />
                            <ucControls:GridColumnExt ID="GridColumnExt55" runat="server" HeaderText="Item" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt56" runat="server" HeaderText="Item Description" DataField="description" Width="250" AllowSort="true" ResourceGroup="item" ResourceName="description" Visible="true" />
                            <ucControls:GridColumnExt ID="GridColumnExt57" runat="server" HeaderText="Quantity" DataField="quantity" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity" />
                            <ucControls:GridColumnExt ID="GridColumnExt68" runat="server" HeaderText="Quantity Allocate" DataField="quantity_allocated" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity_allocated" />
                            <ucControls:GridColumnExt ID="GridColumnExt69" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                            <ucControls:GridColumnExt ID="GridColumnExt72" runat="server" HeaderText="Inventory Status" DataField="inv_status" AllowSort="true" ResourceGroup="inventory" ResourceName="inv_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt80" runat="server" HeaderText="Parent LPN" DataField="parent_lpn" AllowSort="true" ResourceGroup="inventory" ResourceName="parent_lpn" />
                            <ucControls:GridColumnExt ID="GridColumnExt81" runat="server" HeaderText="LPN" DataField="lpn" AllowSort="true" ResourceGroup="inventory" ResourceName="lpn" />
                            <ucControls:GridColumnExt ID="GridColumnExt82" runat="server" HeaderText="Batch Number" DataField="lot_number" AllowSort="true" ResourceGroup="inventory" ResourceName="lot_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt90" runat="server" HeaderText="MFG Date" DataField="mfg_date" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="mfg_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt83" runat="server" HeaderText="Expiry Date" DataField="expiry_date" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="expiry_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt84" runat="server" HeaderText="Serial Number" DataField="serial_number" AllowSort="true" ResourceGroup="inventory" ResourceName="serial_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt85" runat="server" HeaderText="Item Category" DataField="cate_description" DataFieldFilter="category_id" AllowSort="true" ResourceGroup="category" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt86" runat="server" HeaderText="Receipt Date" DataField="receipt_date" AllowSort="true" FormatType="Date" ResourceGroup="receipt_detail" ResourceName="receive_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt96" runat="server" HeaderText="Aging" DataField="aging" FormatType="Integer" AllowSort="true" ResourceGroup="inventory" ResourceName="aging" />
                            <ucControls:GridColumnExt ID="GridColumnExt97" runat="server" HeaderText="Remain Aging" DataField="remain_aging" FormatType="Integer" AllowSort="true" ResourceGroup="inventory" ResourceName="remain_aging" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>

                </ucControls:PanelControlTab>--%>

                <ucControls:PanelControlTab ID="pnItemSummary" runat="server" PanelName="Item Summary" ResourceGroup="inventory" ResourceName="TabByItemSum">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:UpdatePanel runat="server" ID="UpdateGridExtItemSummary" UpdateMode="Conditional" Style="display: flex; float: right;">
                                <ContentTemplate>
                                    <asp:Label ID="GridExtItemSummaryQty" runat="server"></asp:Label>
                                    <div class="pr-3"></div>
                                    <asp:Label ID="GridExtItemSummaryAllocate" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <ucControls:GridExt ID="GridExtItemSummary" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryViewer.InventoryItemSummary" KeyField="KeyId" KeyType="String"
                        GridAllowRowEdit="false" GridAllowRowDelete="false" DisableButtonSearch="true" DisableFirstSearch="true" GridSortDefault="cate_description asc,item_number asc" OnGridRefreshClick="btSearch_Click">
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt58" runat="server" HeaderText="Warehouse" DataField="wh_id" DataFieldFilter="wh_master_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt59" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Item" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Item Description" DataField="description" Width="250" AllowSort="true" ResourceGroup="item" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Quantity" DataField="quantity" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity" />
                            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Quantity Allocate" DataField="quantity_allocated" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity_allocated" />
                            <ucControls:GridColumnExt ID="GridColumnExt87" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                            <ucControls:GridColumnExt ID="GridColumnExt88" runat="server" HeaderText="Inventory Status" DataField="inv_status" AllowSort="true" ResourceGroup="inventory" ResourceName="inv_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Item Category" DataField="cate_description" DataFieldFilter="category_id" AllowSort="true" ResourceGroup="category" ResourceName="description" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>

                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnBatchSummary" runat="server" PanelName="Batch Summary" ResourceGroup="inventory" ResourceName="TabByBatchSum">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:UpdatePanel runat="server" ID="UpdateGridExtBatchSummary" UpdateMode="Conditional" Style="display: flex; float: right;">
                                <ContentTemplate>
                                    <asp:Label ID="GridExtBatchSummaryQty" runat="server"></asp:Label>
                                    <div class="pr-3"></div>
                                    <asp:Label ID="GridExtBatchSummaryAllocate" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <ucControls:GridExt ID="GridExtBatchSummary" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryViewer.InventoryBatchSummary" KeyField="KeyId" KeyType="String"
                        GridAllowRowEdit="false" GridAllowRowDelete="false" DisableButtonSearch="true" DisableFirstSearch="true"
                        GridSortDefault="cate_description asc,item_number asc,lot_number asc" OnGridRefreshClick="btSearch_Click">
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" HeaderText="Warehouse" DataField="wh_id" DataFieldFilter="wh_master_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt17" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt18" runat="server" HeaderText="Item" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt19" runat="server" HeaderText="Item Description" DataField="description" Width="250" AllowSort="true" ResourceGroup="item" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt20" runat="server" HeaderText="Quantity" DataField="quantity" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity" />
                            <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" HeaderText="Quantity Allocate" DataField="quantity_allocated" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity_allocated" />
                            <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                            <ucControls:GridColumnExt ID="GridColumnExt41" runat="server" HeaderText="Inventory Status" DataField="inv_status" AllowSort="true" ResourceGroup="inventory" ResourceName="inv_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt43" runat="server" HeaderText="Batch Number" DataField="lot_number" AllowSort="true" ResourceGroup="inventory" ResourceName="lot_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt42" runat="server" HeaderText="Item Category" DataField="cate_description" DataFieldFilter="category_id" AllowSort="true" ResourceGroup="category" ResourceName="description" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>

                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnExpirySummary" runat="server" PanelName="Expiry Date Summary" ResourceGroup="inventory" ResourceName="TabByExpSum">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:UpdatePanel runat="server" ID="UpdateGridExtExpirySummary" UpdateMode="Conditional" Style="display: flex; float: right;">
                                <ContentTemplate>
                                    <asp:Label ID="GridExtExpirySummaryQty" runat="server"></asp:Label>
                                    <div class="pr-3"></div>
                                    <asp:Label ID="GridExtExpirySummaryAllocate" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <ucControls:GridExt ID="GridExtExpirySummary" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryViewer.InventoryExpirySummary" KeyField="KeyId" KeyType="String"
                        GridAllowRowEdit="false" GridAllowRowDelete="false" DisableButtonSearch="true" DisableFirstSearch="true" GridSortDefault="cate_description asc,item_number asc" OnGridRefreshClick="btSearch_Click">
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt33" runat="server" HeaderText="Warehouse" DataField="wh_id" DataFieldFilter="wh_master_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt34" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt39" runat="server" HeaderText="Item" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt44" runat="server" HeaderText="Item Description" DataField="description" Width="250" AllowSort="true" ResourceGroup="item" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt45" runat="server" HeaderText="Quantity" DataField="quantity" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity" />
                            <ucControls:GridColumnExt ID="GridColumnExt46" runat="server" HeaderText="Quantity Allocate" DataField="quantity_allocated" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity_allocated" />
                            <ucControls:GridColumnExt ID="GridColumnExt47" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                            <ucControls:GridColumnExt ID="GridColumnExt48" runat="server" HeaderText="Inventory Status" DataField="inv_status" AllowSort="true" ResourceGroup="inventory" ResourceName="inv_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt49" runat="server" HeaderText="Expiry Date" DataField="expiry_date" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="expiry_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt50" runat="server" HeaderText="Item Category" DataField="cate_description" DataFieldFilter="category_id" AllowSort="true" ResourceGroup="category" ResourceName="description" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnInventoryEmpty" runat="server" PanelName="Inventory Empty" ResourceGroup="inventory" ResourceName="TabEmpty">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:UpdatePanel runat="server" ID="UpdateGridInventoryEmpty" UpdateMode="Conditional" Style="display: flex; float: right;">
                                <ContentTemplate>
                                    <asp:Label ID="GridInventoryEmptyQty" runat="server"></asp:Label>
                                    <div class="pr-3"></div>
                                    <asp:Label ID="GridInventoryEmptyAllocate" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <ucControls:GridExt ID="GridInventoryEmpty" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryViewer.InventoryEmpty" KeyField="KeyId" KeyType="Guid"
                        DisableButtonSearch="true" DisableFirstSearch="true" OnGridRefreshClick="btSearch_Click">
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt101" runat="server" HeaderText="Warehouse" DataField="wh_id" DataFieldFilter="wh_master_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt102" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt104" runat="server" HeaderText="Description" DataField="description" Width="250" AllowSort="true" ResourceGroup="item" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt103" runat="server" HeaderText="Item" DataField="item_number" DataFieldFilter="item_master_id" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt105" runat="server" HeaderText="QTY" DataField="qty" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity" />
                            <ucControls:GridColumnExt ID="GridColumnExt107" runat="server" HeaderText="UOM" DataField="uom" DataFieldFilter="item_uom_id" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                            <ucControls:GridColumnExt ID="GridColumnExt106" runat="server" HeaderText="Category" DataField="item_category" DataFieldFilter="category_id" AllowSort="true" ResourceGroup="category" ResourceName="category" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnAttribute" runat="server" PanelName="Inventory Attribute" ResourceGroup="inventory" ResourceName="TabAttribute">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:UpdatePanel runat="server" ID="UpdateGridInventoryAttribute" UpdateMode="Conditional" Style="display: flex; float: right;">
                                <ContentTemplate>
                                    <asp:Label ID="GridInventoryAttributeQty" runat="server"></asp:Label>
                                    <div class="pr-3"></div>
                                    <asp:Label ID="GridInventoryAttributeAllocate" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <ucControls:GridExt ID="GridInventoryAttribute" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryViewer.InventoryAttribute" KeyField="KeyId" KeyType="Guid"
                        DisableButtonSearch="true" DisableFirstSearch="true" OnGridRefreshClick="btSearch_Click">
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt108" runat="server" HeaderText="Warehouse" DataField="wh_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt109" runat="server" HeaderText="Owner" DataField="owner_code" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt110" runat="server" HeaderText="Zone" DataField="zone" AllowSort="true" ResourceGroup="zone" ResourceName="zone" />
                            <ucControls:GridColumnExt ID="GridColumnExt111" runat="server" HeaderText="Location" DataField="location" AllowSort="true" ResourceGroup="location" ResourceName="location" />
                            <ucControls:GridColumnExt ID="GridColumnExt113" runat="server" HeaderText="Item Description" DataField="description" Width="250" AllowSort="true" ResourceGroup="item" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt112" runat="server" HeaderText="Item" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt114" runat="server" HeaderText="Quantity" DataField="quantity" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity" />
                            <ucControls:GridColumnExt ID="GridColumnExt115" runat="server" HeaderText="Quantity Allocate" DataField="quantity_allocated" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity_allocated" />
                            <ucControls:GridColumnExt ID="GridColumnExt116" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                            <ucControls:GridColumnExt ID="GridColumnExt117" runat="server" HeaderText="Inventory Status" DataField="inv_status" AllowSort="true" ResourceGroup="inventory" ResourceName="inv_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt118" runat="server" HeaderText="LPN" DataField="lpn" AllowSort="true" ResourceGroup="inventory" ResourceName="lpn" />
                            <ucControls:GridColumnExt ID="GridColumnExt119" runat="server" HeaderText="Batch Number" DataField="lot_number" AllowSort="true" ResourceGroup="inventory" ResourceName="lot_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt93" runat="server" HeaderText="MFG Date" DataField="mfg_date" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="mfg_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt120" runat="server" HeaderText="Expiry Date" DataField="expiry_date" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="expiry_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt121" runat="server" HeaderText="Serial Number" DataField="serial_number" AllowSort="true" ResourceGroup="inventory" ResourceName="serial_number" />

                            <ucControls:GridColumnExt ID="GridColumnExt128" runat="server" HeaderText="Attribute1" DataField="attribute1" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute1" />
                            <%--<ucControls:GridColumnExt ID="GridColumnExt129" runat="server" HeaderText="Attribute2" DataField="attribute2" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute2" />
                            <ucControls:GridColumnExt ID="GridColumnExt130" runat="server" HeaderText="Attribute3" DataField="attribute3" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute3" />
                            <ucControls:GridColumnExt ID="GridColumnExt131" runat="server" HeaderText="Attribute4" DataField="attribute4" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute4" />
                            <ucControls:GridColumnExt ID="GridColumnExt132" runat="server" HeaderText="Attribute5" DataField="attribute5" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute5" />--%>


                            <ucControls:GridColumnExt ID="GridColumnExt122" runat="server" HeaderText="Parent LPN" DataField="parent_lpn" AllowSort="true" ResourceGroup="inventory" ResourceName="parent_lpn" />
                            <ucControls:GridColumnExt ID="GridColumnExt123" runat="server" HeaderText="DG Code" DataField="dg_code" AllowSort="true" ResourceGroup="item" ResourceName="dg_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt125" runat="server" HeaderText="Item Category" DataField="item_category" AllowSort="true" ResourceGroup="category" ResourceName="category" />
                            <ucControls:GridColumnExt ID="GridColumnExt127" runat="server" HeaderText="Receipt Date" DataField="receipt_date" FormatType="Date" AllowSort="true" ResourceGroup="receipt_detail" ResourceName="receive_date" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnItemRole" runat="server" PanelName="By Item Low" ResourceGroup="Inventory" ResourceName="TabByItemLow">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:UpdatePanel runat="server" ID="UpdategridItemRole" UpdateMode="Conditional" Style="display: flex; float: right;">
                                <ContentTemplate>
                                    <asp:Label ID="gridItemRoleQty" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <ucControls:GridExt ID="gridItemRole" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryViewer.InventoryItemRole" KeyField="KeyId" KeyType="Guid"
                        DisableButtonSearch="true" DisableFirstSearch="true" GridSortDefault="item_number asc">
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt51" runat="server" HeaderText="Warehouse" DataField="wh_id" DataFieldFilter="wh_master_id" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="Warehouse" />
                            <ucControls:GridColumnExt ID="GridColumnExt53" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="Owner" />
                            <ucControls:GridColumnExt ID="GridColumnExt62" runat="server" HeaderText="Item" DataField="item_number" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="Item" />
                            <ucControls:GridColumnExt ID="GridColumnExt63" runat="server" HeaderText="Item Description" DataField="description" Width="250" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="Description" Visible="true" />
                            <ucControls:GridColumnExt ID="GridColumnExt89" runat="server" HeaderText="Quantity" DataField="quantity" FormatType="Number" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="Qty" />
                            <ucControls:GridColumnExt ID="GridColumnExt91" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="UOM" />
                            <ucControls:GridColumnExt ID="GridColumnExt92" runat="server" HeaderText="Inventory Status" DataField="inv_status" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="InventoryStatus" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>


                <%--// For 188--%>
                <ucControls:PanelControlTab ID="pnItemExpiry" runat="server" PanelName="Item Expire" ResourceGroup="inventory" ResourceName="TabByItemExpire">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:UpdatePanel runat="server" ID="UpdategridItemExpire" UpdateMode="Conditional" Style="display: flex; float: right;">
                                <ContentTemplate>
                                    <asp:Label ID="gridItemExpireQty" runat="server"></asp:Label>
                                    <div class="pr-3"></div>
                                    <asp:Label ID="gridItemExpireAllocate" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <ucControls:GridExt ID="gridItemExpire" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryViewer.InventoryItemExpire" KeyField="KeyId" KeyType="Guid"
                        GridAllowRowEdit="false" GridAllowRowDelete="false" GridAllowSelectBox="false" DisableButtonSearch="true" DisableFirstSearch="true"
                        GridSortDefault="location asc,cate_description asc,item_number asc,lot_number asc" OnGridRefreshClick="btSearch_Click">
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt124" runat="server" HeaderText="Warehouse" DataField="wh_id" DataFieldFilter="wh_master_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt126" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt133" runat="server" HeaderText="Zone" DataField="zone" AllowSort="true" ResourceGroup="zone" ResourceName="zone" />
                            <ucControls:GridColumnExt ID="GridColumnExt134" runat="server" HeaderText="Location" DataField="location" AllowSort="true" ResourceGroup="location" ResourceName="location" />
                            <ucControls:GridColumnExt ID="GridColumnExt135" runat="server" HeaderText="Item" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt136" runat="server" HeaderText="Item Description" DataField="description" AllowSort="true" ResourceGroup="item" ResourceName="description" Visible="true" />
                            <ucControls:GridColumnExt ID="GridColumnExt137" runat="server" HeaderText="Quantity" DataField="quantity" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity" />
                            <ucControls:GridColumnExt ID="GridColumnExt138" runat="server" HeaderText="Quantity Allocate" DataField="quantity_allocated" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity_allocated" />
                            <ucControls:GridColumnExt ID="GridColumnExt139" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                            <ucControls:GridColumnExt ID="GridColumnExt140" runat="server" HeaderText="Inventory Status" DataField="inv_status" AllowSort="true" ResourceGroup="inventory" ResourceName="inv_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt143" runat="server" HeaderText="Batch Number" DataField="lot_number" AllowSort="true" ResourceGroup="inventory" ResourceName="lot_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt94" runat="server" HeaderText="MFG Date" DataField="mfg_date" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="mfg_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt144" runat="server" HeaderText="Expiry Date" DataField="expiry_date" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="expiry_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt148" runat="server" HeaderText="Day to Expire" DataField="days_to_expire" AllowSort="true" FormatType="Integer" ResourceGroup="item" ResourceName="days_to_expire" />
                            <ucControls:GridColumnExt ID="GridColumnExt145" runat="server" HeaderText="Serial Number" DataField="serial_number" AllowSort="true" ResourceGroup="inventory" ResourceName="serial_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt146" runat="server" HeaderText="Item Category" DataField="cate_description" DataFieldFilter="category_id" AllowSort="true" ResourceGroup="category" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt147" runat="server" HeaderText="Receipt Date" DataField="receipt_date" AllowSort="true" FormatType="Date" ResourceGroup="receipt_detail" ResourceName="receive_date" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>
                <ucControls:PanelControlTab ID="PanelControlTab1" runat="server" PanelName="By Aging Mfg Date" ResourceGroup="inventory" ResourceName="TabByagingMfgDate">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional" Style="display: flex; float: right;">
                                <ContentTemplate>
                                    <asp:Label ID="GridExtAgingMdfDateQty" runat="server"></asp:Label>
                                    <div class="pr-3"></div>
                                    <asp:Label ID="GridExtAgingMdfDateAllocate" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <ucControls:GridExt ID="GridExtAgingMfgDate" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryViewer.InventoryAgingMfgDate" KeyField="KeyId" KeyType="Guid"
                        DisableButtonSearch="true" DisableFirstSearch="true" GridSortDefault="location asc,cate_description asc,item_number asc,lot_number asc" OnGridRefreshClick="btSearch_Click">
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Warehouse" DataField="wh_id" DataFieldFilter="wh_master_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt27" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt40" runat="server" HeaderText="Zone" DataField="zone" AllowSort="true" ResourceGroup="zone" ResourceName="zone" />
                            <ucControls:GridColumnExt ID="GridColumnExt54" runat="server" HeaderText="Location" DataField="location" AllowSort="true" ResourceGroup="location" ResourceName="location" />
                            <ucControls:GridColumnExt ID="GridColumnExt55" runat="server" HeaderText="Item" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt56" runat="server" HeaderText="Item Description" DataField="description" Width="250" AllowSort="true" ResourceGroup="item" ResourceName="description" Visible="true" />
                            <ucControls:GridColumnExt ID="GridColumnExt57" runat="server" HeaderText="Quantity" DataField="quantity" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity" />
                            <ucControls:GridColumnExt ID="GridColumnExt65" runat="server" HeaderText="Quantity Allocate" DataField="quantity_allocated" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity_allocated" />
                            <ucControls:GridColumnExt ID="GridColumnExt68" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                            <ucControls:GridColumnExt ID="GridColumnExt69" runat="server" HeaderText="Inventory Status" DataField="inv_status" AllowSort="true" ResourceGroup="inventory" ResourceName="inv_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt72" runat="server" HeaderText="Parent LPN" DataField="parent_lpn" AllowSort="true" ResourceGroup="inventory" ResourceName="parent_lpn" />
                            <ucControls:GridColumnExt ID="GridColumnExt80" runat="server" HeaderText="LPN" DataField="lpn" AllowSort="true" ResourceGroup="inventory" ResourceName="lpn" />
                            <ucControls:GridColumnExt ID="GridColumnExt81" runat="server" HeaderText="Batch Number" DataField="lot_number" AllowSort="true" ResourceGroup="inventory" ResourceName="lot_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt82" runat="server" HeaderText="MFG Date" DataField="mfg_date" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="mfg_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt83" runat="server" HeaderText="Expiry Date" DataField="expiry_date" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="expiry_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt84" runat="server" HeaderText="Serial Number" DataField="serial_number" AllowSort="true" ResourceGroup="inventory" ResourceName="serial_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt85" runat="server" HeaderText="Item Category" DataField="cate_description" DataFieldFilter="category_id" AllowSort="true" ResourceGroup="category" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt86" runat="server" HeaderText="Receipt Date" DataField="receipt_date" AllowSort="true" FormatType="Date" ResourceGroup="receipt_detail" ResourceName="receive_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt90" runat="server" HeaderText="Aging" DataField="aging" FormatType="Integer" AllowSort="true" ResourceGroup="inventory" ResourceName="aging" />
                            <ucControls:GridColumnExt ID="GridColumnExt96" runat="server" HeaderText="Remain Aging" DataField="remain_aging" FormatType="Integer" AllowSort="true" ResourceGroup="inventory" ResourceName="remain_aging" />
                            <%--<ucControls:GridColumnExt ID="GridColumnExt100" runat="server" HeaderText="Inbound Order Number" DataField="inbound_order_number" AllowSort="true" />--%>
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>
                <%--// For 188--%>
            </ControlTemplate>
        </ucControls:PanelTab>

    </div>

</asp:Content>
