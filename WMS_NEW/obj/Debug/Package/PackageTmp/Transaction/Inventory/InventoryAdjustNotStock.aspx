<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="InventoryAdjustNotStock.aspx.cs" Inherits="WMS_NEW.Transaction.Inventory.InventoryAdjustNotStock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopup ID="popupAdjust" runat="server" HeaderText="Adjust Not Stock">
        <DataTemplate>
            <asp:Panel runat="server" ID="panelData">
                <div class="row">
                    <div class="col-sm-3">
                        <ucControls:InputDropDown ID="ddlAdjustFunction" runat="server" ResourceGroup="inventory" ResourceName="adjust_function" ComboType="String" AutoPostBack="true" IsPrimary="true" />
                        <ucControls:InputDropDown ID="ddlWarehouse" runat="server" ResourceGroup="warehouse" ResourceName="wh_id" IsPrimary="true" DisplayDefault="-- Select --" AutoPostBack="true" />
                        <ucControls:InputDropDown ID="ddlOwner" runat="server" ResourceGroup="owner" ResourceName="owner_code" IsPrimary="true" DisplayDefault="-- Select --" AutoPostBack="true" />
                        <ucControls:InputDropDownHD ID="ddlLocation" runat="server" ResourceGroup="location" ResourceName="location" DisplayDefault="-- Select --" IsPrimary="true" IsKey="true" AutoPostBack="true" />
                        <ucControls:InputTextBox ID="txtParentLPN" runat="server" ResourceGroup="inventory" ResourceName="parent_lpn" Enabled="false" />
                        <ucControls:InputTextBox ID="txtLPN" runat="server" ResourceGroup="inventory" ResourceName="lpn" Enabled="false" IsPrimary="true" IsKey="true" />
                    </div>
                    <div class="col-sm-3">
                        <ucControls:InputDropDownHD ID="ddlWhItem" runat="server" ResourceGroup="item" ResourceName="item_number" IsPrimary="true" IsKey="true" DisplayDefault="-- Select --" AutoPostBack="true" />
                        <ucControls:InputTextBox ID="txtItemDesc" runat="server" ResourceGroup="item" ResourceName="description" Enabled="false" />
                        <ucControls:InputTextBox ID="txtItemCrossRef" runat="server" ResourceGroup="item_crossref" ResourceName="alternate_item_number" Enabled="false" />
                        <ucControls:InputTextBox ID="txtDgCode" runat="server" ResourceGroup="item" ResourceName="dg_code" Enabled="false" />

                        <ucControls:InputTextDate ID="txtMfgDate" runat="server" ResourceGroup="inventory" ResourceName="mfg_date" TextMode="Date" AutoPostBack="true" />
                        <ucControls:InputTextDate ID="txtExpDate" runat="server" ResourceGroup="inventory" ResourceName="expiry_date" TextMode="Date" />
                    </div>
                    <div class="col-sm-3">
                        <ucControls:InputTextBox ID="txtLot" runat="server" ResourceGroup="inventory" ResourceName="lot_number" />
                        <ucControls:InputTextBox ID="txtSerial" runat="server" ResourceGroup="inventory" ResourceName="serial_number" IsPrimary="true" />
                        <ucControls:InputTextNumber ID="txtOrderQTY" runat="server" ResourceGroup="inventory" ResourceName="quantity" IsPrimary="true" IsKey="true" />
                        <ucControls:InputDropDown ID="ddlUOM" runat="server" ResourceGroup="item_uom" ResourceName="uom" IsPrimary="true" IsKey="true" />
                        <ucControls:InputDropDown ID="ddlItemStatus" runat="server" ResourceGroup="inventory" ResourceName="inv_status" IsPrimary="true" IsKey="true" DisplayDefault="--Select--" />
                        <ucControls:InputTextDate ID="txtReceiveDate" runat="server" ResourceGroup="inventory" ResourceName="receive_date" TextMode="Date" IsPrimary="true" />
                    </div>
                    <div class="col-sm-3">
                        <ucControls:InputDropDown runat="server" ID="ddlReasonCode" DisplayDefault="--Select--" ResourceGroup="reason" ResourceName="reason_code" IsPrimary="true" IsKey="true" />
                        <ucControls:InputTextBox ID="txtAttribute1" runat="server" ResourceGroup="inventory" ResourceName="attribute1" />
                        <ucControls:InputTextBox ID="txtAttribute2" runat="server" ResourceGroup="inventory" ResourceName="attribute2" />
                        <ucControls:InputTextBox ID="txtAttribute3" runat="server" ResourceGroup="inventory" ResourceName="attribute3" />
                        <ucControls:InputTextBox ID="txtAttribute4" runat="server" ResourceGroup="inventory" ResourceName="attribute4" />
                        <ucControls:InputTextBox ID="txtAttribute5" runat="server" ResourceGroup="inventory" ResourceName="attribute5" />
                    </div>
                </div>
            </asp:Panel>
        </DataTemplate>
        <CommandTemplate>
            <ucControls:ButtonExt ID="btnConfirmAdjust" runat="server" Text="Confirm" CssClass="btn btn-info" OnClick="btnConfirmAdjust_Click" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" ResourceGroup="General" ResourceName="Save" />
        </CommandTemplate>

    </ucControls:PanelPopup>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryAdjust" KeyField="KeyId" KeyType="String"
        GridAllowRowEdit="false" GridAllowRowDelete="false" AutoSize="true" GridAllowSelectBox="false" DisableExport="true" NewVisible="false" GridSortDefault="create_date">
        <CustomCommandTemplate>
            <ucControls:ButtonExt ID="btnAdjustIn" runat="server" CssClass="btn btn-sm btn-success" Text="Adjust In" OnClick="btnAdjustIn_Click" ResourceGroup="inventory" ResourceName="btn_adjust_in" />
        </CustomCommandTemplate>
        <CustomSearchTemplate>
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="iColWarehouse" DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="owner" ResourceName="owner_code" ID="iColOwner" DataField="owner_code" DataFieldFilter="owner_id" FilterFormatType="Guid" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="zone" ResourceName="zone" ID="iColZone" DataField="zone" FilterFormatType="Text" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownFilterType="LazySearch" DropDownDisplayDefault="-- All --" />
            <%--            <ucControls:GridColumnExt runat="server" ResourceGroup="location" ResourceName="location" ID="iColLocation" DataField="location" DataFieldFilter="location_id" FilterFormatType="Guid" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" DropDownFilterType="LazySearch" />--%>
            <ucControls:GridColumnExt runat="server" ResourceGroup="location" ResourceName="location" ID="iColLocation" DataField="location" FilterFormatType="Text" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" DropDownFilterType="LazySearch" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="category" ResourceName="description" ID="iColCategory" DataField="cate_description" DataFieldFilter="category_id" FilterFormatType="Guid" AllowSort="true" AllowFilter="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" DropDownFilterType="LazySearch" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="item_number" ID="iColItem" DataField="item_number" FormatType="Text" AllowSort="true" AllowFilter="true" ShowFilterNow="true" DropDownFilterType="LazySearch" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="description" ID="GridColumnExt6" DataField="description" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lpn" ID="GridColumnExt13" DataField="lpn" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lot_number" ID="GridColumnExt9" DataField="lot_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="serial_number" ID="GridColumnExt16" DataField="serial_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="mfg_date" ID="GridColumnExt10" DataField="mfg_date" AllowSort="true" FormatType="Date" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="expiry_date" ID="GridColumnExt2" DataField="exp_date" AllowSort="true" FormatType="Date" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="days_to_expire" ID="GridColumnExt133" DataField="days_to_expire" FormatType="Integer" AllowSort="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="quantity" ID="GridColumnExt11" DataField="quantity" FormatType="Number" AllowSort="false" AllowFilter="false" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="inv_status" ID="iColInvStatus" DataField="inv_status" DataFieldFilter="inventory_status_id" FilterFormatType="Guid" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_by" ID="GridColumnExt1" DataField="create_by" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_date" ID="GridColumnExt7" DataField="create_date" FormatType="DateTime" AllowSort="true" AllowFilter="true" />
        </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
