<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="InventoryAdjustStock.aspx.cs" Inherits="WMS_NEW.Transaction.Inventory.InventoryAdjustStock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopup ID="popupAdjust" runat="server" HeaderText="Adjust Stock">
        <DataTemplate>
            <div class="row">
                <div class="col-sm-6">
                    <ucControls:InputDropDown runat="server" ID="ddlAdjustType" ResourceGroup="inventory" ResourceName="adjust_for" ComboType="String" IsPrimary="true" IsKey="true" DisplayDefault="-- Select --" AutoPostBack="true" />
                    <ucControls:InputDropDown runat="server" ID="ddlAdjustFunction" ResourceGroup="inventory" ResourceName="adjust_function" ComboType="String" IsPrimary="true" IsKey="true" DisplayDefault="-- Select --" />
                </div>
                <div class="col-sm-6">
                    <ucControls:InputDropDown runat="server" ResourceGroup="reason" ResourceName="reason_code" ID="ddlReasonCode" DataFieldValue="" DisplayDefault="--Select--" IsPrimary="true" IsKey="true" />

                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <ucControls:GridExt ID="GridExt2" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryAdjust" KeyField="KeyId" KeyType="String"
                        GridAllowRowEdit="false" GridAllowRowDelete="false" AutoSize="true" GridAllowSelectBox="false" DisableExport="true" NewVisible="false" DisableSearch="true" GridSortDefault="create_date">
                        <CustomCommandTemplate>
                        </CustomCommandTemplate>
                        <CustomSearchTemplate>
                            <ucControls:InputHidden runat="server" ID="hidListKey" DataFieldValue="_list_key" />
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="adjust_qty" ID="GridColumnExt31" DataField="adjust_qty" AllowSort="true" ControlType="Text" FormatType="Number" Width="150" FieldTextAlign="Right" InputTextAutoPostBack="true" />
                            <ucControls:GridColumnExt ID="GridColumnExt100" runat="server" HeaderText="Quantity Avalible" DataField="quantity_avalible" FormatType="Number" ResourceGroup="inventory" ResourceName="quantity_avalible" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="quantity" ID="GridColumnExt22" DataField="quantity" FormatType="Number" AllowSort="false" AllowFilter="false" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="GridColumnExt2" DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="owner" ResourceName="owner_code" ID="GridColumnExt3" DataField="owner_code" DataFieldFilter="owner_id" FilterFormatType="Guid" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="zone" ResourceName="zone" ID="GridColumnExt4" DataField="zone" FilterFormatType="Text" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownFilterType="LazySearch" DropDownDisplayDefault="-- All --" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="location" ResourceName="location" ID="GridColumnExt5" DataField="location" DataFieldFilter="location_id" FilterFormatType="Guid" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" DropDownFilterType="LazySearch" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="category" ResourceName="description" ID="GridColumnExt8" DataField="cate_description" DataFieldFilter="category_id" FilterFormatType="Guid" AllowSort="true" AllowFilter="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" DropDownFilterType="LazySearch" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="item_number" ID="GridColumnExt14" DataField="item_number" FormatType="Text" AllowSort="true" AllowFilter="true" ShowFilterNow="true" DropDownFilterType="LazySearch" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="description" ID="GridColumnExt12" DataField="description" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lpn" ID="GridColumnExt15" DataField="lpn" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lot_number" ID="GridColumnExt17" DataField="lot_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="serial_number" ID="GridColumnExt18" DataField="serial_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="mfg_date" ID="GridColumnExt19" DataField="mfg_date" AllowSort="true" FormatType="Date" AllowFilter="true" ShowFilterNow="true" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="expiry_date" ID="GridColumnExt20" DataField="exp_date" AllowSort="true" FormatType="Date" AllowFilter="true" ShowFilterNow="true" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="days_to_expire" ID="GridColumnExt21" DataField="days_to_expire" FormatType="Integer" AllowSort="true" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="inv_status" ID="GridColumnExt23" DataField="inv_status" DataFieldFilter="inventory_status_id" FilterFormatType="Guid" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_by" ID="GridColumnExt24" DataField="create_by" AllowSort="true" AllowFilter="true" />
                            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_date" ID="GridColumnExt26" DataField="create_date" FormatType="DateTime" AllowSort="true" AllowFilter="true" />
                            <ucControls:GridColumnExt runat="server" DataField="sn_control" Visible="false" />


                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </div>
            </div>

        </DataTemplate>
        <CommandTemplate>
            <ucControls:ButtonExt ID="btnConfirmAdjust" runat="server" Text="Confirm" CssClass="btn btn-info" OnClick="btnConfirmAdjust_Click" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" ResourceGroup="general" ResourceName="btn_confirm" />
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryAdjust" KeyField="KeyId" KeyType="String"
        GridAllowRowEdit="false" GridAllowRowDelete="false" AutoSize="true" GridAllowSelectBox="true" DisableExport="true" NewVisible="false" GridSortDefault="create_date">
        <CustomCommandTemplate>
            <ucControls:ButtonExt ID="btnAdjustStock" runat="server" CssClass="btn btn-sm btn-success" Text="Adjust Stock" OnClick="btnAdjustStock_Click" ResourceGroup="inventory" ResourceName="btn_adjust_in_out" />
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
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="description" ID="GridColumnExt6" DataField="description" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lpn" ID="GridColumnExt13" DataField="lpn" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lot_number" ID="GridColumnExt9" DataField="lot_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="serial_number" ID="GridColumnExt16" DataField="serial_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="mfg_date" ID="GridColumnExt10" DataField="mfg_date" AllowSort="true" FormatType="Date" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="expiry_date" ID="GridColumnExt1" DataField="exp_date" AllowSort="true" FormatType="Date" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="days_to_expire" ID="GridColumnExt133" DataField="days_to_expire" FormatType="Integer" AllowSort="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="quantity" ID="GridColumnExt11" DataField="quantity" FormatType="Number" AllowSort="false" AllowFilter="false" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="inv_status" ID="iColInvStatus" DataField="inv_status" DataFieldFilter="inventory_status_id" FilterFormatType="Guid" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_by" ID="GridColumnExt7" DataField="create_by" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_date" ID="GridColumnExt25" DataField="create_date" FormatType="DateTime" AllowSort="true" AllowFilter="true" />

        </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
