<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="OutboundMaster.aspx.cs" Inherits="WMS_NEW.Export.OutboundMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Viewer.OutboundMaster" KeyField="KeyID" KeyType="Guid">
        <CustomSearchTemplate>
            <ucControls:InputHidden ID="hidSessionUser" runat="server" DataFieldValue="_userID" />
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" ResourceGroup="warehouse" ResourceName="wh_id" HeaderText="Warehouse" DataField="wh_id" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" ResourceGroup="owner" ResourceName="owner_code" HeaderText="Owner" DataField="owner_code" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" ResourceGroup="outbound_master" ResourceName="outbound_order_number" HeaderText="Order No" DataField="outbound_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" ResourceGroup="outbound_master" ResourceName="order_type" HeaderText="Order Type (Mvt)" DataField="order_type" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" ResourceGroup="outbound_master" ResourceName="order_status" HeaderText="Order Status" DataField="order_status" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" ResourceGroup="customer" ResourceName="code" HeaderText="Customer Code" DataField="customer_code" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" ResourceGroup="customer" ResourceName="name" HeaderText="Customer Name" DataField="customer_name" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt19" runat="server" ResourceGroup="outbound_master" ResourceName="department" HeaderText="Department" DataField="department" AllowSort="true" AllowFilter="true" ShowFilterNow="false" />

            <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" ResourceGroup="outbound_master" ResourceName="ship_to_code" HeaderText="Ship to Code" DataField="ship_to_code" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" ResourceGroup="outbound_master" ResourceName="ship_name" HeaderText="Ship to Name" DataField="ship_to_name" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" ResourceGroup="outbound_master" ResourceName="ship_address" HeaderText="Ship to Addr" DataField="ship_to_addr_line_1" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" ResourceGroup="outbound_master" ResourceName="ship_date_plan" HeaderText="Ship Date" DataField="ship_date_actual" FormatType="DateTime" AllowFilter="true" AllowSort="true" ShowFilterNow="true" FilterFormatType="Date" />
            <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" ResourceGroup="outbound_detail" ResourceName="quantity_order_total" HeaderText="Total Qty Order" DataField="qty_order" AllowSort="true" FormatType="Number" />
            <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" ResourceGroup="outbound_detail" ResourceName="quantity_ship_total" HeaderText="Total Qty Ship" DataField="qty_ship" AllowSort="true" FormatType="Number" />
        </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
