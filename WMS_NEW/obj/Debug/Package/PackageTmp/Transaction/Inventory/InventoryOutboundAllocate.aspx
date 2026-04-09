<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="InventoryOutboundAllocate.aspx.cs" Inherits="WMS_NEW.Transaction.Inventory.InventoryOutboundAllocate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:GridExt ID="GirdExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.InventoryOuntboundAllocate"
        KeyField="KeyID" KeyType="Guid">
        <CustomSearchTemplate>

        </CustomSearchTemplate>
        <CustomColumnTemplate>
          <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="warehouse" DataField="wh_id" DataFieldFilter="wh_master_id"  AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" FilterFormatType="Guid" AllowFilter="true"   ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="owner" DataField="owner_code"  DataFieldFilter="owner_id"  AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" FilterFormatType="Guid" AllowFilter="true"   ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownFilterType="LazySearch" />
            <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" HeaderText="zone" DataField="zone"   AllowSort="true" ResourceGroup="zone" ResourceName="zone" ShowFilterNow="true" AllowFilter="true"    />
            <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="location" DataField="location"  AllowSort="true" ResourceGroup="location" ResourceName="location" ShowFilterNow="true" FilterFormatType="Text" AllowFilter="true"  UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownFilterType="LazySearch" />
            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Lpn" DataField="lpn"   AllowSort="true" ResourceGroup="inbound" ResourceName="lpn" AllowFilter="true" />
            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="category" DataField="cate_description"   AllowSort="true" ResourceGroup="category" ResourceName="description" AllowFilter="true" />
            <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="item_number" DataField="item_number"   AllowSort="true" ResourceGroup="item" ResourceName="item_number" ShowFilterNow="true" FilterFormatType="Text" AllowFilter="true"    UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownFilterType="LazySearch" />
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="description" DataField="description"   AllowSort="true" ResourceGroup="item" ResourceName="description" ShowFilterNow="true" AllowFilter="true"    />
            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="quantity" DataField="quantity"   AllowSort="true" ResourceGroup="inventory" ResourceName="quantity" AllowFilter="true" />
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="uom" DataField="uom"   AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" AllowFilter="true" />
            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="lot_number" DataField="lot_number"  AllowSort="true" ResourceGroup="inventory" ResourceName="lot_number" AllowFilter="true" />
            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="expiry_date" DataField="expiry_date" FormatType="Date"  AllowSort="true" ResourceGroup="inventory" ResourceName="expiry_date" AllowFilter="true" />
            <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Inventory Status" DataField="inv_status"  AllowSort="true" ResourceGroup="inventory" ResourceName="inv_status" ShowFilterNow="true" AllowFilter="true"    />
            <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Receive Date" DataField="receive_date"  FormatType="Date" AllowSort="true" ResourceGroup="inbound" ResourceName="receive_date" AllowFilter="true" />
            <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Order Number Desc" DataField="order_number_desc"  AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="order_number_desc" AllowFilter="true" />
        </CustomColumnTemplate>
    </ucControls:GridExt>

</asp:Content>
