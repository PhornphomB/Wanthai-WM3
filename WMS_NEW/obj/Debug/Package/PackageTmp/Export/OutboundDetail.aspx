<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="OutboundDetail.aspx.cs" Inherits="WMS_NEW.Export.OutboundDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Viewer.OutboundDetail" KeyField="KeyID" KeyType="Guid">
        <CustomSearchTemplate>
            <ucControls:InputHidden ID="hidSessionUser" runat="server" DataFieldValue="_userID" />
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" ResourceGroup="warehouse" ResourceName="wh_id" HeaderText="Warehouse" DataField="wh_id" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" ResourceGroup="owner" ResourceName="owner_code" HeaderText="Owner" DataField="owner_code" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" ResourceGroup="outbound_master" ResourceName="outbound_order_number" HeaderText="Order No" DataField="outbound_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" ResourceGroup="outbound_master" ResourceName="order_type" HeaderText="Order Type (Mvt)" DataField="order_type" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" ResourceGroup="outbound_master" ResourceName="order_status" HeaderText="Order Status" DataField="order_status" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" ResourceGroup="customer" ResourceName="customer_code" HeaderText="Customer Code" DataField="customer_code" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" ResourceGroup="customer" ResourceName="customer_name" HeaderText="Customer Name" DataField="customer_name" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt19" runat="server" ResourceGroup="outbound_master" ResourceName="department" HeaderText="Department" DataField="department" AllowSort="true" AllowFilter="true" ShowFilterNow="false" />
            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" ResourceGroup="outbound_master" ResourceName="ship_date_plan" HeaderText="Ship Date" DataField="ship_date_actual" FormatType="DateTime" AllowFilter="true" AllowSort="true" ShowFilterNow="true" FilterFormatType="Date" />
            <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" ResourceGroup="item" ResourceName="item_number" HeaderText="Item Number" DataField="item_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" ResourceGroup="item" ResourceName="description" HeaderText="Item Desc" DataField="item_description" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" ResourceGroup="inventory" ResourceName="attribute1" HeaderText="Update By" DataField="attribute1" AllowSort="true" />

            <ucControls:GridColumnExt ID="GridColumnExt20" runat="server" ResourceGroup="inventory" ResourceName="lot_number" HeaderText="Lot" DataField="lot_number" AllowSort="true" AllowFilter="true"  />
            <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" ResourceGroup="inventory" ResourceName="mfg_date" HeaderText="MFG Date" DataField="mfg_date" AllowSort="true" AllowFilter="true" FormatType="Date" />
            <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" ResourceGroup="inventory" ResourceName="exp_date" HeaderText="Expiry Date" DataField="exp_date" AllowSort="true" AllowFilter="true" FormatType="Date" />
           
            <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" ResourceGroup="outbound_detail" ResourceName="alter_quantity_order" HeaderText="Alter Qty Order" DataField="alter_quantity_order" AllowSort="true" FormatType="Number" />
            <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" ResourceGroup="outbound_detail" ResourceName="alter_quantity_ship" HeaderText="Alter Qty Ship" DataField="alter_quantity_ship" AllowSort="true" FormatType="Number" />
            <ucControls:GridColumnExt ID="GridColumnExt24" runat="server" ResourceGroup="item_uom" ResourceName="uom" HeaderText="UOM" DataField="pack_size_uom" AllowSort="true" />

            <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" ResourceGroup="inventory" ResourceName="lpn" HeaderText="LPN" DataField="lpn" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" ResourceGroup="outbound_master" ResourceName="user_def1" HeaderText="User Defined Field 1" DataField="user_def1" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt17" runat="server" ResourceGroup="general" ResourceName="create_by" HeaderText="Create By" DataField="create_by" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" ResourceGroup="general" ResourceName="update_by" HeaderText="Update By" DataField="update_by" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt18" runat="server" ResourceGroup="outbound_master" ResourceName="load_id" HeaderText="Load Id" DataField="load_id" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
        
        </CustomColumnTemplate>
    </ucControls:GridExt>

</asp:Content>
