<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="CountReconcilExt.aspx.cs" Inherits="WMS_NEW.Transaction.Count.CountReconcilExt" %>

<%@ Register Src="~/Report/AscxControls/ucReportViewer.ascx" TagPrefix="ucControls" TagName="ReportViewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopup ID="popupCountDetail" runat="server" StyleSize="Large" StyleColor="Default" HeaderText="Count Detail">
        <DataTemplate>

            <ucControls:ReportViewer runat="server" ID="ReportViewer" />

            <ucControls:PanelControlRow ID="panelCountHeader" runat="server" CssClass="row col-sm-12 col-md-12 col-lg-12 ml-0 mr-0" >

                <ucControls:PanelControlRow ID="PanelControlRow2" runat="server" CssClass="row col-sm-12 col-md-12 col-lg-9">
                    <ucControls:InputHidden ID="hidCountMasterID" runat="server" />
                    <ucControls:InputTextBox ID="txtWarehouseID" runat="server" Enabled="false" LabelText="Warehouse ID" ResourceGroup="warehouse" ResourceName="wh_id" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
                    <ucControls:InputTextBox ID="txtCountID" runat="server" Enabled="false" LabelText="Count ID" ResourceGroup="count_plan_master" ResourceName="count_id" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
                    <ucControls:InputTextBox ID="txtCountType" runat="server" Enabled="false" LabelText="Count Type" ResourceGroup="count_plan_master" ResourceName="count_type" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
                    <ucControls:InputTextBox ID="txtCountStatus" runat="server" Enabled="false" LabelText="Count Status" ResourceGroup="count_plan_master" ResourceName="count_status" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />

                    <ucControls:InputTextBox ID="txtCreateBy" runat="server" Enabled="false" LabelText="Create By" ResourceGroup="general" ResourceName="create_by" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
                    <ucControls:InputTextDate ID="txtCreateDate" runat="server" TextMode="DateTime" Enabled="false" LabelText="Create Date" ResourceGroup="general" ResourceName="create_date" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
                    <ucControls:InputTextBox ID="txtDesc" runat="server" Enabled="false" LabelText="Description" ResourceGroup="count_plan_master" ResourceName="description" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />

                    <ucControls:InputTextBox ID="txtCloseBy" runat="server" Enabled="false" LabelText="Close By" ResourceGroup="general" ResourceName="close_by" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
                    <ucControls:InputTextDate ID="txtCloseDate" runat="server" TextMode="DateTime" Enabled="false" LabelText="Close Date" ResourceGroup="general" ResourceName="close_date" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
                    <ucControls:InputTextBox ID="txtCloseRemark" runat="server" IsMultiLine="true" LabelText="Close Remark" ResourceGroup="general" ResourceName="remark" BaseContentCss="col-sm-12 col-md-4 col-lg-3" />
                </ucControls:PanelControlRow>

                <ucControls:PanelControlRow ID="PanelControlRow1" runat="server" CssClass="row col-sm-12 col-md-12 col-lg-3">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <div class="card card-outline-danger">
                            <div class="card-block">
                                <div class="text-uppercase text-danger text-bold text-center mb-2"><span>Total Summary</span></div>
                                <div class="text-bold text-muted">
                                    <div class="col-sm-12 col-md-12 col-lg-12">
                                        <ucControls:LabelExt ID="LabelExt1" runat="server" InnerText="Total On hand QTY : " ResourceGroup="count_plan_master" ResourceName="quantity_total_onhand" />
                                        <ucControls:LabelExt ID="lblSumStock" runat="server" InnerText="0" />

                                    </div>
                                    <div class="col-sm-12 col-md-12 col-lg-12">
                                        <ucControls:LabelExt ID="LabelExt12" runat="server" InnerText="Total Count Qty : " ResourceGroup="count_plan_master" ResourceName="quantity_total_count" />
                                        <ucControls:LabelExt ID="lblSumCount" runat="server" InnerText="0" />
                                    </div>
                                    <div class="col-sm-12 col-md-12 col-lg-12">
                                        <ucControls:LabelExt ID="LabelExt13" runat="server" InnerText="Total Variance Qty : " ResourceGroup="count_plan_master" ResourceName="quantity_total_variance" />
                                        <ucControls:LabelExt ID="lblSumDiff" runat="server" InnerText="0" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ucControls:PanelControlRow>
            </ucControls:PanelControlRow>

            <ucControls:PanelTab ID="panelTabCountView" runat="server">
                <ControlTemplate>
                    <ucControls:PanelControlTab ID="panelCountDetail" runat="server" PanelName="Count Plan" ResourceGroup="count_plan_master" ResourceName="tab_count_plan">
                        <ucControls:GridExt ID="gridCountDetail" runat="server"
                            SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Count.CountPlanDetailExt" KeyField="KeyId" KeyType="String"
                            DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true">
                            <CustomSearchTemplate>
                                <ucControls:InputHidden ID="hiddetail_count_master_id" runat="server" DataFieldValue="_count_master_id" />
                            </CustomSearchTemplate>
                            <CustomColumnTemplate>
                                <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Zone" DataField="zone" AllowSort="true" ResourceGroup="zone" ResourceName="zone" />
                                <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Location" DataField="location" AllowSort="true" ResourceGroup="location" ResourceName="location" />
                                <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Item Category" DataField="cate_description" Width="200" AllowSort="true" ResourceGroup="category" ResourceName="description" />
                                <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" HeaderText="Description" DataField="description" Width="250" AllowSort="true" ResourceGroup="item" ResourceName="description" />
                                <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" HeaderText="Item Number" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                                <ucControls:GridColumnExt ID="GridColumnExt52" runat="server" HeaderText="Lot" DataField="lot" AllowSort="true" ResourceGroup="inventory" ResourceName="lot_number" />
                                <ucControls:GridColumnExt ID="GridColumnExt24" runat="server" HeaderText="Serial" DataField="serial_number" AllowSort="true" ResourceGroup="inventory" ResourceName="serial_number" />
                                <ucControls:GridColumnExt ID="GridColumnExt53" runat="server" HeaderText="Expiry Date" Width="150" FormatType="Date" DataField="expiry_date" AllowSort="true" ResourceGroup="inventory" ResourceName="expiry_date" />
                                <ucControls:GridColumnExt ID="GridColumnExt17" runat="server" HeaderText="On hand QTY." DataField="stock_qty" FormatType="Number" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity" />

                                <ucControls:GridColumnExt ID="GridColumnExt55" runat="server" HeaderText="Attribute1" DataField="attribute1" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute1" />
                                <ucControls:GridColumnExt ID="GridColumnExt56" runat="server" HeaderText="Attribute2" DataField="attribute2" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute2" />
                                <ucControls:GridColumnExt ID="GridColumnExt57" runat="server" HeaderText="Attribute3" DataField="attribute3" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute3" />
                                <ucControls:GridColumnExt ID="GridColumnExt58" runat="server" HeaderText="Attribute4" DataField="attribute4" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute4" />
                                <ucControls:GridColumnExt ID="GridColumnExt59" runat="server" HeaderText="Attribute5" DataField="attribute5" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute5" />

                            </CustomColumnTemplate>
                        </ucControls:GridExt>
                    </ucControls:PanelControlTab>
                    <ucControls:PanelControlTab ID="panelCountDesc" runat="server" PanelName="Count Description" ResourceGroup="count_plan_master" ResourceName="tab_count_description">
                        <ucControls:GridExt ID="gridCountDesc" runat="server"
                            SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Count.CountPlanReconcilExt" KeyField="KeyId" KeyType="String"
                            DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true">
                            <CustomCommandTemplate>
                                <asp:DropDownList ID="comboCountDescOpt" runat="server" CssClass="btn btn-sm btn-success" Style="margin-left: 2px; margin-right: 2px; padding-top: 0.40rem; padding-bottom: 0.38rem;" />
                            </CustomCommandTemplate>
                            <CustomSearchTemplate>
                                <ucControls:InputHidden ID="hiddesc_count_master_id" runat="server" DataFieldValue="_count_master_id" />
                                <ucControls:InputHidden ID="hiddesc_view_opt" runat="server" DataFieldValue="_view_opt" />
                            </CustomSearchTemplate>
                            <CustomColumnTemplate>
                                <ucControls:GridColumnExt ID="GridColumnExt27" runat="server" HeaderText="Zone" DataField="zone" AllowSort="true" ResourceGroup="zone" ResourceName="zone" />
                                <ucControls:GridColumnExt ID="GridColumnExt19" runat="server" HeaderText="Location" DataField="location" AllowSort="true" ResourceGroup="location" ResourceName="location" />
                                <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" HeaderText="Item Category" DataField="cate_description" Width="200" AllowSort="true" ResourceGroup="category" ResourceName="description" />
                                <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" HeaderText="Description" DataField="description" Width="250" AllowSort="true" ResourceGroup="item" ResourceName="description" />
                                <ucControls:GridColumnExt ID="GridColumnExt20" runat="server" HeaderText="Item Number" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                                <ucControls:GridColumnExt ID="GridColumnExt48" runat="server" HeaderText="Lot" DataField="lot" AllowSort="true" ResourceGroup="inventory" ResourceName="lot_number" />
                                <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" HeaderText="Serial" DataField="serial_number" AllowSort="true" ResourceGroup="inventory" ResourceName="serial_number" />
                                <ucControls:GridColumnExt ID="GridColumnExt49" runat="server" HeaderText="Expiry Date" DataField="expiry_date" Width="150" FormatType="Date" AllowSort="true" ResourceGroup="inventory" ResourceName="expiry_date" />
                                <ucControls:GridColumnExt ID="GridColumnExt30" runat="server" HeaderText="Count Qty" DataField="count_qty" AllowSort="true" FormatType="Number" ResourceGroup="inventory" ResourceName="quantity_count" />
                                <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="Status" DataField="inv_status" AllowSort="true" ResourceGroup="inventory" ResourceName="inv_status" />
                                <ucControls:GridColumnExt ID="GridColumnExt28" runat="server" HeaderText="LPN" DataField="lpn" AllowSort="true" ResourceGroup="inventory" ResourceName="lpn" />

                                <ucControls:GridColumnExt ID="GridColumnExt26" runat="server" HeaderText="Attribute1" DataField="attribute1" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute1" />
                                <ucControls:GridColumnExt ID="GridColumnExt29" runat="server" HeaderText="Attribute2" DataField="attribute2" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute2" />
                                <ucControls:GridColumnExt ID="GridColumnExt31" runat="server" HeaderText="Attribute3" DataField="attribute3" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute3" />
                                <ucControls:GridColumnExt ID="GridColumnExt32" runat="server" HeaderText="Attribute4" DataField="attribute4" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute4" />
                                <ucControls:GridColumnExt ID="GridColumnExt33" runat="server" HeaderText="Attribute5" DataField="attribute5" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute5" />

                                <%--<ucControls:GridColumnExt ID="GridColumnExt18" runat="server" HeaderText="Owner" DataField="owner_id" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="owner_code" />
                                <ucControls:GridColumnExt ID="GridColumnExt29" runat="server" HeaderText="Stock Qty" DataField="stock_qty" AllowSort="true" FormatType="Number" ResourceGroup="count_plan_master" ResourceName="StockQty" />
                                <ucControls:GridColumnExt ID="GridColumnExt31" runat="server" HeaderText="Diff Qty" DataField="diff_qty" AllowSort="true" FormatType="Number" ResourceGroup="count_plan_master" ResourceName="DiffQty" />
                                <ucControls:GridColumnExt ID="GridColumnExt32" runat="server" HeaderText="UOM" DataField="uom_prompt" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="uom" />
                                <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" HeaderText="Parent LPN" DataField="parent_lpn" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="parent_lpn" />
                                <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="Grade" DataField="grade" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="Grade" />
                                <ucControls:GridColumnExt ID="GridColumnExt26" runat="server" HeaderText="Price" DataField="price" AllowSort="true" FormatType="Number" ResourceGroup="count_plan_master" ResourceName="Price" />
                                <ucControls:GridColumnExt ID="GridColumnExt33" runat="server" HeaderText="Count By" DataField="create_by" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="CountBy" />
                                <ucControls:GridColumnExt ID="GridColumnExt34" runat="server" HeaderText="Count Date" DataField="create_date" AllowSort="true" FormatType="DateTime" ResourceGroup="count_plan_master" ResourceName="CountDate" />--%>
                            </CustomColumnTemplate>
                        </ucControls:GridExt>
                    </ucControls:PanelControlTab>
                    <ucControls:PanelControlTab ID="panelCountReconcil" runat="server" PanelName="Count Reconcile" ResourceGroup="count_plan_master" ResourceName="tab_count_reconcile">
                        <ucControls:GridExt ID="gridCountReconcil" runat="server"
                            SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Count.CountDetailAndReconcilExt" KeyField="KeyId" KeyType="String"
                            DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true">
                            <CustomCommandTemplate>
                                <ucControls:ButtonExt ID="btPrint" runat="server" Text="Report" CssClass="btn btn-sm" CausesValidation="false" OnClick="btPrint_Click" ResourceGroup="general" ResourceName="btn_report" />
                            </CustomCommandTemplate>
                            <CustomSearchTemplate>
                                <ucControls:InputHidden ID="hidrec_count_master_id" runat="server" DataFieldValue="_count_master_id" />
                            </CustomSearchTemplate>
                            <CustomColumnTemplate>
                                <ucControls:GridColumnExt ID="GridColumnExt40" runat="server" HeaderText="Zone" DataField="zone" AllowSort="true" ResourceGroup="zone" ResourceName="zone" />
                                <ucControls:GridColumnExt ID="GridColumnExt41" runat="server" HeaderText="Location" DataField="location" AllowSort="true" ResourceGroup="location" ResourceName="location" />
                                <ucControls:GridColumnExt ID="GridColumnExt18" runat="server" HeaderText="Item Category" DataField="cate_description" Width="200" AllowSort="true" ResourceGroup="category" ResourceName="description" />
                                <ucControls:GridColumnExt ID="GridColumnExt36" runat="server" HeaderText="Description" DataField="description" Width="250" AllowSort="true" ResourceGroup="item" ResourceName="description" />
                                <ucControls:GridColumnExt ID="GridColumnExt35" runat="server" HeaderText="Item Number" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                                <ucControls:GridColumnExt ID="GridColumnExt50" runat="server" HeaderText="Lot" DataField="lot" AllowSort="true" ResourceGroup="inventory" ResourceName="lot_number" />
                                <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Serial" DataField="serial_number" AllowSort="true" ResourceGroup="inventory" ResourceName="serial_number" />
                                <ucControls:GridColumnExt ID="GridColumnExt51" runat="server" HeaderText="Expiry Date" DataField="expiry_date" Width="150" FormatType="Date" AllowSort="true" ResourceGroup="inventory" ResourceName="expiry_date" />
                                <ucControls:GridColumnExt ID="GridColumnExt44" runat="server" HeaderText="On hand QTY." DataField="stock_qty" AllowSort="true" FormatType="Number" ResourceGroup="inventory" ResourceName="quantity" />
                                <ucControls:GridColumnExt ID="GridColumnExt45" runat="server" HeaderText="Count Qty" DataField="count_qty" AllowSort="true" FormatType="Number" ResourceGroup="inventory" ResourceName="quantity_count" />
                                <ucControls:GridColumnExt ID="GridColumnExt46" runat="server" HeaderText="Variance Qty" DataField="diff_qty" AllowSort="true" FormatType="Number" ResourceGroup="inventory" ResourceName="quantity_diff" />
                                <ucControls:GridColumnExt ID="GridColumnExt37" runat="server" HeaderText="Status" DataField="inv_status" AllowSort="true" ResourceGroup="inventory" ResourceName="inv_status" />
                                <ucControls:GridColumnExt ID="GridColumnExt43" runat="server" HeaderText="LPN" DataField="lpn" AllowSort="true" ResourceGroup="inventory" ResourceName="lpn" />

                                <ucControls:GridColumnExt ID="GridColumnExt34" runat="server" HeaderText="Attribute1" DataField="attribute1" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute1" />
                                <ucControls:GridColumnExt ID="GridColumnExt38" runat="server" HeaderText="Attribute2" DataField="attribute2" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute2" />
                                <ucControls:GridColumnExt ID="GridColumnExt39" runat="server" HeaderText="Attribute3" DataField="attribute3" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute3" />
                                <ucControls:GridColumnExt ID="GridColumnExt42" runat="server" HeaderText="Attribute4" DataField="attribute4" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute4" />
                                <ucControls:GridColumnExt ID="GridColumnExt47" runat="server" HeaderText="Attribute5" DataField="attribute5" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute5" />

                                <%--<ucControls:GridColumnExt ID="GridColumnExt24" runat="server" HeaderText="Owner" DataField="owner_code" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="owner_code" />
                                <ucControls:GridColumnExt ID="GridColumnExt47" runat="server" HeaderText="UOM" DataField="uom_prompt" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="uom" />
                                <ucControls:GridColumnExt ID="GridColumnExt42" runat="server" HeaderText="Parent LPN" DataField="parent_lpn" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="parent_lpn" />
                                <ucControls:GridColumnExt ID="GridColumnExt38" runat="server" HeaderText="Grade" DataField="grade" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="Grade" />
                                <ucControls:GridColumnExt ID="GridColumnExt39" runat="server" HeaderText="Price" DataField="price" AllowSort="true" FormatType="Number" ResourceGroup="count_plan_master" ResourceName="Price" />--%>
                            </CustomColumnTemplate>
                        </ucControls:GridExt>
                    </ucControls:PanelControlTab>
                </ControlTemplate>
            </ucControls:PanelTab>
        </DataTemplate>
        <CommandTemplate>
            <ucControls:ButtonExt ID="btClosePlan" runat="server" Text="Close Plan" CssClass="btn btn-sm btn-danger" OnClick="btClosePlan_Click" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" ResourceGroup="count_plan_master" ResourceName="btn_close_plan" />
            <ucControls:ButtonExt ID="btRefresh" runat="server" Text="Refresh" CssClass="btn btn-sm btn-info" CausesValidation="false" OnClick="btRefresh_Click" ResourceGroup="general" ResourceName="btn_refresh" />
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Count.CountPlanExt" KeyField="KeyId" KeyType="Guid"
        GridAllowRowClick="true" GridSortDefault="create_date desc">
        <CustomSearchTemplate>
            <ucControls:InputHidden ID="hidSessionUser" runat="server" DataFieldValue="_userID" />
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Warehouse ID" DataField="wh_id" AllowFilter="true" ShowFilterNow="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="warehouse" ResourceName="wh_id" />
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Count ID" DataField="count_id" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="count_id" />
            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Status" DataField="count_status" AllowFilter="true" ShowFilterNow="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="count_plan_master" ResourceName="count_status" />
            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Type" DataField="count_plan_type" AllowFilter="true" ShowFilterNow="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="count_plan_master" ResourceName="count_plan_type" />
            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Description" DataField="description" Width="250" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="description" />
            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Create By" DataField="create_by" AllowSort="true" ResourceGroup="general" ResourceName="create_by" />
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Create Date" DataField="create_date" Width="150" AllowFilter="true" ShowFilterNow="true" FormatType="DateTime" FilterFormatType="Date" AllowSort="true" ResourceGroup="general" ResourceName="create_date" />
            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Close By" DataField="close_by" AllowSort="true" ResourceGroup="general" ResourceName="close_by" />
            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Close Date" DataField="close_date" Width="150" FormatType="DateTime" FilterFormatType="Date" AllowSort="true" ResourceGroup="general" ResourceName="close_date" />
            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="Close Remark" DataField="close_remark" AllowSort="true" ResourceGroup="general" ResourceName="remark" />
            <%--<ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Generate Method" DataField="count_generate_method" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="GenerateMethod" />--%>
        </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
