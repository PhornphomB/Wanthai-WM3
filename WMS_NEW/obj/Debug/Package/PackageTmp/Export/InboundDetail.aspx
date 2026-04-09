<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="InboundDetail.aspx.cs" Inherits="WMS_NEW.Export.InboundDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12">
            <asp:UpdatePanel runat="server" ID="UpdateAltherQty" UpdateMode="Conditional" Style="display: flex; float: right;">
                <ContentTemplate>
                    <asp:Label ID="AltherQty" runat="server"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Viewer.InboundDetail" KeyField="sys_id" KeyType="String" GridAllowRowEdit="true">
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="GridColumnExt00" runat="server" ResourceGroup="warehouse" ResourceName="wh_id" HeaderText="Warehouse" DataField="wh_id" DataFieldFilter="wh_master_id" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownAutoPostBack="true" FilterFormatType="Guid" />
            <ucControls:GridColumnExt ID="GridColumnExt03" runat="server" ResourceGroup="inbound_master" ResourceName="order_type" HeaderText="Order Type" DataField="order_type" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt02" runat="server" ResourceGroup="inbound_master" ResourceName="inbound_order_number" HeaderText="Inbound Order No" DataField="inbound_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt27" runat="server" ResourceGroup="print_label" ResourceName="print_date" DataField="print_date" FormatType="DateTime" AllowFilter="true" AllowSort="true" ShowFilterNow="true" FilterFormatType="Date" />
            <ucControls:GridColumnExt ID="GridColumnExt20" runat="server" ResourceGroup="inbound_master" ResourceName="production_line" HeaderText="Production Line" DataField="production_line" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownFilterType="LazySearch" DefaultFilter="Contains" />
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" ResourceGroup="item" ResourceName="item_number" HeaderText="Item Number" DataField="item_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt05" runat="server" ResourceGroup="item" ResourceName="description" HeaderText="Item Description" DataField="description" AllowFilter="true" />
            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" ResourceGroup="inventory" ResourceName="lot_number" HeaderText="Lot/Batch" DataField="lot_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" ResourceGroup="inventory" ResourceName="mfg_date" DataField="mfg_date" FormatType="Date" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" ResourceGroup="inventory" ResourceName="expiry_date" DataField="expiry_date" FormatType="Date" AllowSort="true" AllowFilter="true"  />
            <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" ResourceGroup="inventory" ResourceName="attribute1" HeaderText="Attribute1" DataField="attribute1" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" ResourceGroup="inbound_detail" ResourceName="alter_quantity_receive" HeaderText="Alther Quantity Receive" DataField="alter_quantity_receive" AllowSort="true" AllowFilter="true" FormatType="Number" />
            <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" ResourceGroup="item_uom" ResourceName="item_uom_id" HeaderText="Unit" DataField="pack_size_uom" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt ID="GridColumnExt07" runat="server" ResourceGroup="inventory" ResourceName="lpn" HeaderText="LPN" DataField="lpn" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" ResourceGroup="inventory" ResourceName="remark" HeaderText="Remark" DataField="remark" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt24" runat="server" ResourceGroup="customer" ResourceName="customer_code" HeaderText="Customer Code" DataField="customer_code" DataFieldFilter="customer_id" FilterFormatType="Guid" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownFilterType="LazySearch" />
            <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" ResourceGroup="customer" ResourceName="customer_name" HeaderText="Customer Name" DataField="customer_name" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt26" runat="server" ResourceGroup="general" ResourceName="create_by" HeaderText="Attribute1" DataField="create_by" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" ResourceGroup="general" ResourceName="create_date" DataField="create_date" FormatType="DateTime" AllowFilter="true" AllowSort="true" ShowFilterNow="true" FilterFormatType="Date" />
        </CustomColumnTemplate>
    </ucControls:GridExt>
    
<ucControls:PanelPopup ID="popupChange" runat="server" StyleSize="Small" HeaderText="Save Remark">
    <DataTemplate>
        <div class="row pt-2">
            <ucControls:InputHidden ID="hidReceiptDetailId" runat="server" />
            <ucControls:InputTextBox ID="txtLPN" runat="server" LabelText="LPN" ResourceGroup="inbound_master" ResourceName="lpn" BaseContentCss="col-sm-6" Enabled="false" />
            <ucControls:InputTextBox ID="txtRemark" runat="server" LabelText="Remark" ResourceGroup="inbound_master" ResourceName="remark" BaseContentCss="col-sm-6" Enabled="true" />
        </div>
    </DataTemplate>
    <CommandTemplate>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <ucControls:ButtonExt ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success"  OnClick="btnSave_Click" ResourceGroup="general" ResourceName="btn_save" CausesValidation="false" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave" />
            </Triggers>
        </asp:UpdatePanel>
    </CommandTemplate>

</ucControls:PanelPopup>
</asp:Content>
