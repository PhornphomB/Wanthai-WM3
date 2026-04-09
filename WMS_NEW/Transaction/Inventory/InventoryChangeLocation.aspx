<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="InventoryChangeLocation.aspx.cs" Inherits="WMS_NEW.Transaction.Inventory.InventoryChangeLocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopup ID="popupChange" runat="server" HeaderText="Change Location">
        <DataTemplate>
            <div class="form-group row">
                <div class="col-sm-6">
                    <ucControls:InputDropDown runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="ddlWarehouse" DisplayDefault="--Select--" IsPrimary="true" IsKey="true" AutoPostBack="true" />
                    <ucControls:InputDropDownHD runat="server" ResourceGroup="location" ResourceName="location" ID="ddlLocation" DisplayDefault="--Select--" IsPrimary="true" IsKey="true" AutoPostBack="true" />
                    <div class="input-group" style="margin-left: 0px">
                        <ucControls:InputCheckBox runat="server" ResourceGroup="general" ResourceName="stay" ID="chkParentLPN" AutoPostBack="true" />
                        <span class="input-group-prepend">
                            <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="parent_lpn" ID="txtParentLPN" Enabled="true" />
                        </span>
                    </div>
                    <div class="input-group" style="margin-left: 0px">
                        <ucControls:InputCheckBox runat="server" ResourceGroup="general" ResourceName="stay" ID="chkLPN" AutoPostBack="true" Checked="true" />
                        <span class="input-group-prepend">
                            <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="lpn" ID="txtLPN" Enabled="true" />
                        </span>
                    </div>
                    <div class="small text-muted">
                        <ucControls:LabelExt runat="server" ID="lblLpn" />
                    </div>

                </div>
                <div class="col-sm-6">
                    <ucControls:InputTextNumber runat="server" ResourceGroup="inventory" ResourceName="quantity" ID="txtQty" IsPrimary="true" />
                    <div class="small text-muted">
                        <ucControls:LabelExt runat="server" ID="lblQty" />
                    </div>
                    <ucControls:InputDropDown runat="server" ResourceGroup="reason" ResourceName="reason_code" ID="ddlReasonCode" DisplayDefault="--Select--" IsPrimary="true" IsKey="true" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="general" ResourceName="remark" ID="txtRemark" />


                </div>
            </div>

        </DataTemplate>
        <CommandTemplate>
            <ucControls:ButtonExt ID="btnConfirmChange" runat="server" Text="Confirm" CssClass="btn btn-info" OnClick="btnConfirmChange_Click" ResourceGroup="general" ResourceName="btn_confirm" />
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryChange" KeyField="KeyId" KeyType="Guid"
        GridAllowRowEdit="false" GridAllowRowDelete="false" AutoSize="true" GridAllowSelectBox="true" GridAllowShowSelectBoxAll="true" DisableExport="true" NewVisible="false" GridSortDefault="create_date">
        <CustomCommandTemplate>
            <ucControls:ButtonExt ID="btnChangeLocation" runat="server" CssClass="btn btn-sm btn-success" Text="Change Location" OnClick="btnChangeLocation_Click" ResourceGroup="inventory" ResourceName="btn_change_location" />
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
            <%--            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="serial_number" ID="GridColumnExt16" DataField="serial_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />--%>
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="mfg_date" ID="GridColumnExt10" DataField="mfg_date" AllowSort="true" FormatType="Date" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="expiry_date" ID="GridColumnExt2" DataField="exp_date" AllowSort="true" FormatType="Date" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="attribute1" ID="GridColumnExt4" DataField="attribute1" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="days_to_expire" ID="GridColumnExt133" DataField="days_to_expire" FormatType="Integer" AllowSort="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="quantity" ID="GridColumnExt11" DataField="quantity" FormatType="Number" AllowSort="false" AllowFilter="false" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="inv_status" ID="iColInvStatus" DataField="inv_status" DataFieldFilter="inventory_status_id" FilterFormatType="Guid" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_by" ID="GridColumnExt1" DataField="create_by" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_date" ID="GridColumnExt7" DataField="create_date" FormatType="DateTime" AllowSort="true" AllowFilter="true" />

        </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
