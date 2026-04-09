<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="InventoryAdjust.aspx.cs" Inherits="WMS_NEW.Export.InventoryAdjust" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <ucControls:GridExt ID="GridExt1" runat="server"
            SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Viewer.InventoryAdjust" KeyField="KeyID" KeyType="String" GridSortDefault="adjust_date_stock desc">
            <CustomColumnTemplate>
                <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Warehouse" DataField="wh_id" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="warehouse" ResourceName="wh_id" />
                <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Owner" DataField="owner_code" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="owner" ResourceName="owner_code" />
                <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Location" DataField="location" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownFilterType="LazySearch" DropDownDisplayDefault="--All--" ResourceGroup="location" ResourceName="location" />
                <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Adjust Date Show" DataField="adjust_date_export" AllowFilter="true" AllowSort="true" ShowFilterNow="true" FilterFormatType="Date" FormatType="Date" ResourceGroup="inventory" ResourceName="adjust_date_export" />
                <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Item Number" DataField="item_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="item" ResourceName="item_number" />
                <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Item Description" DataField="item_desc" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="item" ResourceName="description" />
                <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Quantity" DataField="quantity" FormatType="Number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="inventory" ResourceName="quantity" />
                <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="After Quantity" DataField="after_quantity" FormatType="Number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="inventory" ResourceName="after_quantity" />
                <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="UOM" DataField="quantity_uom" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                <ucControls:GridColumnExt ID="GridColumnExt89" runat="server" HeaderText="Batch Number" DataField="lot_number" AllowSort="true" ResourceGroup="inventory" ResourceName="lot_number" />
                <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Expiry Date" DataField="expiry_date" FormatType="Date" AllowSort="true" ResourceGroup="inventory" ResourceName="expiry_date" />
                <ucControls:GridColumnExt ID="GridColumnExt96" runat="server" HeaderText="Serial Number" DataField="serial_number" AllowSort="true" ResourceGroup="inventory" ResourceName="serial_number" />
                <ucControls:GridColumnExt ID="GridColumnExt17" runat="server" HeaderText="Reason" DataField="reason_desc" DataFieldFilter="reason_code" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="reason" ResourceName="description" />
                <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Adjust By" DataField="adjust_by" AllowSort="true" ResourceGroup="inventory" ResourceName="adjust_by" />
                <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Adjust Date" DataField="adjust_date_stock" AllowSort="true" FormatType="Date" ResourceGroup="inventory" ResourceName="adjust_date" />

            </CustomColumnTemplate>
        </ucControls:GridExt>
</asp:Content>
