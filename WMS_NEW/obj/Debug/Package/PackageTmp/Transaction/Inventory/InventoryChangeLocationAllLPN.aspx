<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="InventoryChangeLocationAllLPN.aspx.cs" Inherits="WMS_NEW.Transaction.Inventory.InventoryChangeLocationAllLPN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopup ID="popupChange" runat="server" HeaderText="Change Location">
        <DataTemplate>
            <div class="form-group">
                <div class="row">
                    <div class="col-sm-12">
                        <ucControls:InputDropDownHD runat="server" ResourceGroup="location" ResourceName="location" ID="ddlLocation" DisplayDefault="--Select--" IsPrimary="true" IsKey="true" AutoPostBack="true" />
                        <ucControls:InputDropDown runat="server" ResourceGroup="reason" ResourceName="reason_code" ID="ddlReasonCode" DisplayDefault="--Select--" IsPrimary="true" IsKey="true" />
                        <ucControls:InputTextBox runat="server" ResourceGroup="general" ResourceName="remark" ID="txtRemark" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <ucControls:GridExt ID="GridExt2" runat="server"
                            SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryChangeLocationAllLPNDetail" KeyField="KeyId" KeyType="Guid"
                            GridAllowRowEdit="false" GridAllowRowDelete="false" AutoSize="true" DisableFirstSearch="true" DisableSearch="true" DisableExport="true" NewVisible="false" GridSortDefault="create_date">
                            <CustomCommandTemplate>
                            </CustomCommandTemplate>
                            <CustomSearchTemplate>
                                <ucControls:InputHidden runat="server" ID="hidWh" DataFieldValue="_wh_master_id" />
                                <ucControls:InputHidden runat="server" ID="hidLPN" DataFieldValue="_lpn" />
                            </CustomSearchTemplate>
                            <CustomColumnTemplate>
                                <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="GridColumnExt3" DataField="wh_id" AllowSort="true" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="owner" ResourceName="owner_code" ID="GridColumnExt4" DataField="owner_code" AllowSort="true" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="zone" ResourceName="zone" ID="GridColumnExt5" DataField="zone" AllowSort="true" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="location" ResourceName="location" ID="GridColumnExt8" DataField="location" AllowSort="true" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="category" ResourceName="description" ID="GridColumnExt12" DataField="cate_description" AllowSort="true" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="item_number" ID="GridColumnExt15" DataField="item_number" AllowSort="true" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="description" ID="GridColumnExt14" DataField="description" AllowSort="true" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lpn" ID="GridColumnExt16" DataField="lpn" AllowSort="true" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lot_number" ID="GridColumnExt17" DataField="lot_number" AllowSort="true" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="mfg_date" ID="GridColumnExt18" DataField="mfg_date" AllowSort="true" FormatType="Date" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="expiry_date" ID="GridColumnExt19" DataField="exp_date" AllowSort="true" FormatType="Date" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="days_to_expire" ID="GridColumnExt20" DataField="days_to_expire" FormatType="Integer" AllowSort="true" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="quantity" ID="GridColumnExt21" DataField="quantity" FormatType="Number" AllowSort="false" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="inv_status" ID="GridColumnExt22" DataField="inv_status" AllowSort="true" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_by" ID="GridColumnExt23" DataField="create_by" AllowSort="true" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_date" ID="GridColumnExt24" DataField="create_date" FormatType="DateTime" AllowSort="true" />

                            </CustomColumnTemplate>
                        </ucControls:GridExt>
                    </div>
                </div>
            </div>

        </DataTemplate>
        <CommandTemplate>
            <ucControls:ButtonExt ID="btnConfirmChange" runat="server" Text="Confirm" CssClass="btn btn-info" OnClick="btnConfirmChange_Click" ResourceGroup="general" ResourceName="btn_confirm" />
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryChangeLocationAllLPN" KeyField="KeyId" KeyType="String"
        GridAllowRowEdit="false" GridAllowRowDelete="false" AutoSize="true" GridAllowSelectBox="true" DisableExport="true" NewVisible="false" GridSortDefault="wh_id,lpn">
        <CustomCommandTemplate>
            <ucControls:ButtonExt ID="btnChangeLocation" runat="server" CssClass="btn btn-sm btn-success" Text="Change Location" OnClick="btnChangeLocation_Click" ResourceGroup="inventory" ResourceName="btn_change_location" />
        </CustomCommandTemplate>
        <CustomSearchTemplate>
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="iColWarehouse" DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="owner" ResourceName="owner_code" ID="iColOwner" DataField="owner_code" DataFieldFilter="owner_id" FilterFormatType="Guid" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" />
            <%--            <ucControls:GridColumnExt runat="server" ResourceGroup="location" ResourceName="location" ID="iColLocation" DataField="location" DataFieldFilter="location_id" FilterFormatType="Guid" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" DropDownFilterType="LazySearch" />--%>
            <ucControls:GridColumnExt runat="server" ResourceGroup="location" ResourceName="location" ID="iColLocation" DataField="location" FilterFormatType="Text" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --" DropDownFilterType="LazySearch" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lpn" ID="GridColumnExt13" DataField="lpn" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />

        </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
