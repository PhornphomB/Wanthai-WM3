<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="InventoryReprint.aspx.cs" Inherits="WMS_NEW.Transaction.Mod.InventoryReprint" %>

<%@ Register Src="~/Transaction/Mod/AscxControls/ucInventoryPrint.ascx" TagPrefix="ucControls" TagName="ucInventoryPrint" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:ucInventoryPrint runat="server" id="ucInventoryPrint" />

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryViewer.InventoryReprint" KeyField="KeyId" KeyType="Guid"
        GridSortDefault="location asc,cate_description asc,item_number asc,lot_number asc" >
        <CustomSearchTemplate>
            <ucControls:InputHidden ID="hid_active_load" runat="server" DataFieldValue="_active_load" />
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="reprint" ID="GridColumnExt18" DataField="cmd_reprint" ControlType="CommandButton" CommandText="Reprint" CommandName="RE_PRINT" IsConfirm="false" />
            <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" HeaderText="Warehouse" DataField="wh_id" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="warehouse" ResourceName="warehouse" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt35" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" FilterFormatType="Guid" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="owner" ResourceName="owner_code" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Zone" DataField="zone" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="zone" ResourceName="zone" />
            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Location" DataField="location" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="location" ResourceName="location" />
            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Item" DataField="item_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="item" ResourceName="item_number" />
            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Item Description" DataField="description" Width="250" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="item" ResourceName="description" Visible="true" />
            <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Quantity" DataField="quantity" FormatType="Number" AllowFilter="true" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity" />
            <ucControls:GridColumnExt ID="GridColumnExt70" runat="server" HeaderText="Quantity Allowcate" DataField="quantity_allocated" FormatType="Number" AllowFilter="true" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity_allocated" />
            <ucControls:GridColumnExt ID="GridColumnExt124" runat="server" HeaderText="QTY Available" DataField="quantity_avalible" FormatType="Number" AllowFilter="true" AllowSort="true" ResourceGroup="inventory" ResourceName="quantity_avl" />
            <ucControls:GridColumnExt ID="GridColumnExt71" runat="server" HeaderText="UOM" DataField="uom" AllowSort="true" AllowFilter="true" ResourceGroup="item_uom" ResourceName="uom" />
            <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Inventory Status" DataField="inv_status" AllowFilter="true" AllowSort="true" ResourceGroup="inventory" ResourceName="inv_status" />
            <ucControls:GridColumnExt ID="GridColumnExt64" runat="server" HeaderText="LPN" DataField="lpn" AllowSort="true" AllowFilter="true" ResourceGroup="inventory" ResourceName="lpn" />
            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Batch Number" DataField="lot_number" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="inventory" ResourceName="lot_number" />
            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="MFG Date" DataField="mfg_date" AllowFilter="true" ShowFilterNow="false" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="mfg_date" />
            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="Expiry Date" DataField="expiry_date" AllowFilter="true" ShowFilterNow="true" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="expiry_date" />
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Attribute1" DataField="attribute1" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="inventory" ResourceName="attribute1" />
            <%--<ucControls:GridColumnExt ID="GridColumnExt74" runat="server" HeaderText="Serial Number" DataField="serial_number" AllowFilter="true" AllowSort="true" ResourceGroup="inventory" ResourceName="serial_number" />--%>
            <ucControls:GridColumnExt ID="GridColumnExt52" runat="server" HeaderText="Item Category" DataField="cate_description" AllowFilter="true" DataFieldFilter="category_id" AllowSort="true" ResourceGroup="category" ResourceName="description" />
            <ucControls:GridColumnExt ID="GridColumnExt73" runat="server" HeaderText="Receipt Date" DataField="receive_date" AllowFilter="true" ShowFilterNow="true" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="receipt_date" />
            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="QR Code" DataField="qr_format" AllowFilter="true" ShowFilterNow="false" AllowSort="true" ResourceGroup="inventory" ResourceName="qr_code" Visible="false" />

        </CustomColumnTemplate>
    </ucControls:GridExt>

</asp:Content>
