<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="OutboundReles.aspx.cs" Inherits="WMS_NEW.Transaction.Outbound.OutboundReles" %>

<%@ Register Src="~/Transaction/Outbound/AscxControls/PickListDetail.ascx" TagPrefix="ucControls" TagName="PickListDetail" %>
<%@ Register Src="~/Report/AscxControls/ucReportViewer.ascx" TagPrefix="ucControls" TagName="ReportViewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:PickListDetail runat="server" ID="PickListDetail1" />
    <ucControls:ReportViewer runat="server" ID="ReportViewer1" />

    <ucControls:PanelPopup ID="popupConfirmRelease" runat="server" StyleSize="Default" StyleColor="Danger">
        <DataTemplate>
            <customControls:GridViewExt ID="gvConfirmRelease" runat="server" HorizontalAlign="Left" ShowHeaderWhenEmpty="true" AllowPaging="false" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField runat="server" DataField="item_description" HeaderText="Item Description" ItemStyle-Width="200" />
                    <asp:BoundField runat="server" DataField="item_number" HeaderText="Item Number" ItemStyle-Width="100" />
                    <asp:BoundField runat="server" DataField="grade" HeaderText="Grade" ItemStyle-Width="80" />
                    <asp:BoundField runat="server" DataField="inventory_status" HeaderText="Status" ItemStyle-Width="80" />
                    <asp:BoundField runat="server" DataField="price" HeaderText="Price" ItemStyle-Width="60" />
                    <asp:BoundField runat="server" DataField="planqty" HeaderText="Plan Qty" ItemStyle-Width="60" />
                    <asp:BoundField runat="server" DataField="quantity" HeaderText="Quantity" ItemStyle-Width="60" />
                    <asp:BoundField runat="server" DataField="uom" HeaderText="UOM" ItemStyle-Width="60" />
                </Columns>
            </customControls:GridViewExt>
        </DataTemplate>
        <CommandTemplate>
            <ucControls:ButtonExt ID="btConfirmBackOrder" runat="server" Text="Confirm Back Order" CssClass="btn btn-sm btn-warning" OnClick="btConfirmBackOrder_Click" ResourceGroup="general" ResourceName="btn_confirm_back_order" />
            <ucControls:ButtonExt ID="btComfirm" runat="server" Text="Confirm Release" CssClass="btn btn-sm btn-danger" OnClick="btComfirm_Click" ResourceGroup="general" ResourceName="btn_confirm" />
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.OutboundRelease" KeyField="KeyId" KeyType="Guid" GridAllowRowClick="true" GridSortDefault="create_date DESC">
        <CustomCommandTemplate>
            <ucControls:ButtonExt ID="btPrint" runat="server" Text="Report" CssClass="btn btn-sm" CausesValidation="false" OnClick="btPrint_Click" ResourceGroup="general" ResourceName="btn_report" />
        </CustomCommandTemplate>
        <CustomSearchTemplate>
            <ucControls:InputHidden ID="hidSessionUser" runat="server" DataFieldValue="_userID" />
            <ucControls:InputHidden ID="hidIsFirstLoad" runat="server" DataFieldValue="_isFirstLoad" />
            <ucControls:InputHidden ID="hidPickType" runat="server" DataFieldValue="_userPickType" />

            <ucControls:InputDropDownHD ID="ddlCategory" runat="server" DataFieldValue="_cateID" DisplayDefault="--All--" LabelText="Item Category" ResourceGroup="category" ResourceName="category" />
            <ucControls:InputTextBox ID="txtItemNumber" runat="server" DataFieldValue="_itemNumber" DefaultFilter="Contains" FixFilter="true" LabelText="Item" ResourceGroup="item" ResourceName="item_number" />
            <ucControls:InputTextBox ID="txtLot" runat="server" DataFieldValue="_lot_number" DefaultFilter="Contains" FixFilter="true" LabelText="Lot" ResourceGroup="inventory" ResourceName="lot_number" />
            <ucControls:InputTextDate ID="dtpExpDate" runat="server" ResourceGroup="inventory" ResourceName="exp_date" DataFieldValue="_exp_date" DefaultFilter="Equal" />

        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" HeaderText="" DataField="release" ControlType="CommandButton" CommandText="Release" CommandName="RELEASE" ResourceGroup="inventory" ResourceName="Release" />
            <ucControls:GridColumnExt ID="GridColumnExt24" runat="server" HeaderText="" DataField="unrel" ControlType="CommandButton" CommandText="Unrelease" CommandName="UNREL" />
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Warehouse" DataField="wh_id" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="warehouse" ResourceName="wh_id" />
            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" FormatType="Guid" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="owner" ResourceName="Code" />
            <%--<ucControls:GridColumnExt ID="GridColumnExt29" runat="server" HeaderText="Province" DataField="province" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="GenernalAddress" ResourceName="Province" Visible="false" />--%>
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Order Type (Mvt)" DataField="order_type" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="outbound_master" ResourceName="order_type" />
            <ucControls:GridColumnExt ID="GridColumnExt28" runat="server" HeaderText="Customer PO" DataField="customer_purchase_order" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="customer_purchase_order" />
            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Order No" DataField="outbound_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="outbound_order_number" />
            <%--<ucControls:GridColumnExt ID="GridColumnExt22" runat="server" HeaderText="Load ID" DataField="load_id" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="LoadID" />--%>
            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Status" DataField="order_status" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="outbound_master" ResourceName="order_status" />
            <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="Customer Code" DataField="customer_code" AllowFilter="true" AllowSort="true" ShowFilterNow="false" ResourceGroup="customer" ResourceName="customer_code" />
            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Customer" DataField="customer_name" DataFieldFilter="customer_id" FormatType="Guid" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownFilterType="LazySearch" DropDownDisplayDefault="--All--" ResourceGroup="customer" ResourceName="customer_name" />
            <ucControls:GridColumnExt ID="GridColumnExt30" runat="server" HeaderText="Invoice No" DataField="customer_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="customer_order_number" />
            <ucControls:GridColumnExt ID="GridColumnExt19" runat="server" ResourceGroup="outbound_master" ResourceName="department" HeaderText="Department" DataField="department" AllowSort="true" AllowFilter="true" ShowFilterNow="false" />
            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Order Date" DataField="order_date" FormatType="Date" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="order_date" />
            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Delivery Date Plan" DataField="delivery_date_plan" FormatType="DateTime" AllowFilter="true" AllowSort="true" ShowFilterNow="true" FilterFormatType="Date" ResourceGroup="outbound_master" ResourceName="delivery_date_plan" />
            <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Ship Date Plan" DataField="ship_date_plan" FormatType="DateTime" AllowFilter="true" AllowSort="true" ShowFilterNow="true" FilterFormatType="Date" ResourceGroup="outbound_master" ResourceName="ship_date_plan" />
            <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Ship Date Actual" DataField="ship_date_actual" FormatType="DateTime" AllowFilter="true" AllowSort="true" ShowFilterNow="true" FilterFormatType="Date" ResourceGroup="outbound_master" ResourceName="ship_date_actual" />
            <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Total Qty Order" DataField="qty_order" AllowSort="true" ResourceGroup="outbound_detail" ResourceName="quantity_order_total" />
            <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Total Qty Pick" DataField="qty_pick" AllowSort="true" ResourceGroup="outbound_detail" ResourceName="quantity_pick_total" />
            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="Create By" DataField="create_by" ResourceGroup="general" ResourceName="create_by" />
            <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Create Date" DataField="create_date" FormatType="DateTime" ResourceGroup="general" ResourceName="create_date" />
            <ucControls:GridColumnExt ID="GridColumnExt26" runat="server" HeaderText="Update By" DataField="update_by" ResourceGroup="general" ResourceName="update_by" />
            <ucControls:GridColumnExt ID="GridColumnExt27" runat="server" HeaderText="Update Date" DataField="update_date" FormatType="DateTime" ResourceGroup="general" ResourceName="update_date" />
            <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" HeaderText="Priority" DataField="priority" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="priority" />

        </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
