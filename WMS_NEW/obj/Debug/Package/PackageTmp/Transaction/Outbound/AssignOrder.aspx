<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="AssignOrder.aspx.cs" Inherits="WMS_NEW.Transaction.Outbound.AssignOrder" %>

<%@ Register Src="~/Transaction/Outbound/AscxControls/AssignOrderDetail.ascx" TagPrefix="ucControls" TagName="AssignOrderDetail" %>
<%@ Register Src="~/Transaction/Outbound/AscxControls/AssignOrderUncheck.ascx" TagPrefix="ucControls" TagName="AssignOrderUncheck" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopupEntity ID="popupAssignOrder" runat="server" VisibleSave="false">
        <ControlTemplate>
            <ucControls:PanelControlRow ID="PanelControlRow0" runat="server">
                <ucControls:InputTextBox ID="InputTextBox_wh_id" Enabled="false" runat="server" DataFieldValue="wh_id" LabelText="Warehouse" ResourceGroup="warehouse" ResourceName="wh_id" />
                <ucControls:InputTextBox ID="InputTextBox_outbound_order_number" Enabled="false"  runat="server" DataFieldValue="outbound_order_number" LabelText="Outbound Order No." ResourceGroup="outbound_master" ResourceName="outbound_order_number" />
                <ucControls:InputTextBox ID="InputTextBox_customer_code" Enabled="false"  runat="server" DataFieldValue="customer_code" LabelText="Customer Code" ResourceGroup="customer" ResourceName="customer_code" />
                <ucControls:InputTextBox ID="InputTextBox_owner_code" Enabled="false"  runat="server" DataFieldValue="owner_code" LabelText="Owner" ResourceGroup="owner" ResourceName="owner_code" />
                <ucControls:InputTextBox ID="InputTextBox_order_status" Enabled="false" runat="server" DataFieldValue="order_status" LabelText="Order Status" ResourceGroup="outbound_master" ResourceName="order_status" />
                <ucControls:InputTextBox ID="InputTextBox_sum_quantity_order" Enabled="false"  runat="server" DataFieldValue="sum_quantity_order" LabelText="Order Quantity" ResourceGroup="outbound_detail" ResourceName="quantity_order" />
                <ucControls:InputTextBox ID="InputTextBox_sum_quantity_pick" Enabled="false" runat="server" DataFieldValue="sum_quantity_pick" LabelText="Picked Quantity" ResourceGroup="outbound_detail" ResourceName="quantity_pick" />
                <ucControls:InputTextBox ID="InputTextBox_sum_quantity_stage" Enabled="false" runat="server" DataFieldValue="sum_quantity_stage" LabelText="Staged Quantity" ResourceGroup="outbound_detail" ResourceName="quantity_stage" />
                <ucControls:InputTextBox ID="InputTextBox_sum_quantity_load" Enabled="false" runat="server" DataFieldValue="sum_quantity_load" LabelText="Loaded Quantity" ResourceGroup="outbound_detail" ResourceName="quantity_load" />
                <ucControls:InputTextBox ID="InputTextBox_sum_quantity_ship" Enabled="false" runat="server" DataFieldValue="sum_quantity_ship" LabelText="Shiped Quantity" ResourceGroup="outbound_detail" ResourceName="quantity_ship" />
            </ucControls:PanelControlRow>   
            <ucControls:PanelTab ID="PanelTab1" runat="server">
                <ControlTemplate>
                    <ucControls:PanelControlTab ID="PanelControlTabAssignOrder" runat="server" PanelName="Assign Order" ResourceGroup="AssignOrder" ResourceName="tab_assign_order">
                        <ucControls:AssignOrderDetail runat="server" ID="AssignOrderDetail1" />
                    </ucControls:PanelControlTab>
                    <ucControls:PanelControlTab ID="PanelControlTabUncheck" runat="server" PanelName="Uncheck" ResourceGroup="AssignOrder" ResourceName="tab_uncheck">
                        <ucControls:AssignOrderUncheck runat="server" ID="AssignOrderUncheck1" />
                    </ucControls:PanelControlTab>
                </ControlTemplate>
            </ucControls:PanelTab>
        </ControlTemplate>
         <CommandTemplate>
            <ucControls:ButtonExt ID="btSaveAssginOrder" runat="server" Text="Save" CssClass="btn btn-sm btn-danger" OnClick="btSaveAssginOrder_Click" ResourceGroup="AssignOrder" ResourceName="btSaveAssginOrder" />
        </CommandTemplate>
    </ucControls:PanelPopupEntity>
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.AssignOrder" KeyField="KeyId" KeyType="Guid"
        ColumnFreezeLength="3" GridSortDefault="create_date desc" GridAllowRowEdit="true" GridAllowRowDelete="false" NewVisible="false"
        AutoGenerateColumn="false" ShowAllSort="true" ShowAllFilter="true">    
        <customcolumntemplate>           
            <ucControls:GridColumnExt runat="server" ID="iColWarehouse" ResourceGroup="warehouse" ResourceName="wh_id"  DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" UseFilterDropDown="true" AllowFilter="true" AllowSort="true" DropDownDisplayDefault="--All--" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ID="iColOwner" ResourceGroup="owner" ResourceName="Code" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" FilterFormatType="Guid" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ID="iColOrderType" ResourceGroup="outbound_master" ResourceName="order_type" DataField="order_type" DataFieldFilter="order_type" FilterFormatType="Text" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ID="GridColumnExt1" HeaderText="Customer Order No." DataField="customer_order_number" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="outbound_master" ResourceName="customer_order_number" />
            <ucControls:GridColumnExt runat="server" ID="GridColumnExt4" HeaderText="Outbound Order No." DataField="outbound_order_number" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="outbound_master" ResourceName="outbound_order_number" />
            <ucControls:GridColumnExt runat="server" ID="iColOrderStatus" ResourceGroup="outbound_master" ResourceName="order_status" DataField="order_status" DataFieldFilter="order_status" FilterFormatType="Text"  AllowFilter="true" AllowSort="true"  ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt runat="server" ID="GridColumnExt7" HeaderText="Customer Code" DataField="customer_code" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="customer" ResourceName="customer_code" />
        </customcolumntemplate>
    </ucControls:GridExt>
</asp:Content>

