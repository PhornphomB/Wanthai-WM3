<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="InventoryChangeSerial.aspx.cs" Inherits="WMS_NEW.Transaction.Inventory.InventoryChangeSerial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopup ID="popupChange" runat="server" HeaderText="Change Serial">
        <DataTemplate>
            <div class="form-group row">
                <div class="col-sm-12">
                    <ucControls:InputTextBox runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="txtWarehouse" Enabled="false" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="item" ResourceName="item_number" ID="txtItem" Enabled="false" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="item" ResourceName="description" ID="txtDescription" Enabled="false" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="serial_number" ID="txtSerial" Enabled="false" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="serial_new" ID="txtSerialNew" IsPrimary="true" IsKey="true" />
                    <ucControls:InputDropDown runat="server" ResourceGroup="reason" ResourceName="reason_code" ID="ddlReasonCode" DataFieldValue="" DisplayDefault="--Select--" IsPrimary="true" IsKey="true" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="general" ResourceName="remark" ID="txtRemark" />

                </div>
            </div>
        </DataTemplate>
        <CommandTemplate>
            <ucControls:ButtonExt ID="btnConfirmChange" runat="server" Text="Confirm" CssClass="btn btn-info" OnClick="btnConfirmChange_Click"  ResourceGroup="general" ResourceName="btn_confirm" />
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryChangeSerial" KeyField="KeyId" KeyType="String"
        GridAllowRowEdit="false" GridAllowRowDelete="false" AutoSize="true" GridAllowSelectBox="false" DisableExport="true" NewVisible="false" GridSortDefault="create_date">
        <CustomCommandTemplate>
        </CustomCommandTemplate>
        <CustomSearchTemplate>
            <%--            <ucControls:InputHidden ID="hid_active_load" runat="server" DataFieldValue="_active_load" />--%>
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt runat="server" ResourceGroup="invnetory" ResourceName="change_serial" ID="GridColumnExt2" DataField="change_serial" CommandText="Change Serial" CommandName="change_serial" ControlType="CommandButton" IsConfirm="false"/>

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
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="expiry_date" ID="GridColumnExt1" DataField="exp_date" AllowSort="true" FormatType="Date" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="days_to_expire" ID="GridColumnExt133" DataField="days_to_expire" FormatType="Integer" AllowSort="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="quantity" ID="GridColumnExt11" DataField="quantity" FormatType="Number" AllowSort="false" AllowFilter="false" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="inv_status" ID="iColInvStatus" DataField="inv_status" DataFieldFilter="inventory_status_id" FilterFormatType="Guid" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_by" ID="GridColumnExt3" DataField="create_by" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_date" ID="GridColumnExt7" DataField="create_date" FormatType="DateTime" AllowSort="true" AllowFilter="true" />

        </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
