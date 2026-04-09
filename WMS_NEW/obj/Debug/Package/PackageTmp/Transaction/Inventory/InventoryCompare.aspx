<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="InventoryCompare.aspx.cs" Inherits="WMS_NEW.Transaction.Inventory.InventoryCompare" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:PanelPopup ID="popupItemOrderDetail" runat="server" CssClass="popup_full" HeaderText="Order Detail">
        <DataTemplate>
            <ucControls:GridExt ID="gridItemOrderDetail" runat="server"
                SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.InventoryItemOrderDetail"
                KeyField="KeyID" KeyType="String" AutoSize="true" DisableSearch="true" DisableExport="true" DisableFirstSearch="true">
                <CustomCommandTemplate>
                </CustomCommandTemplate>
                <CustomSearchTemplate>
                    <ucControls:InputHidden ID="hid_ord_wh_item_master_id" runat="server" DataFieldValue="_wh_item_master_id" />
                </CustomSearchTemplate>
                <CustomColumnTemplate>
                    <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Order No" DataField="outbound_order_number" Width="100" ResourceGroup="outbound_master" ResourceName="outbound_order_number" />
                    <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" HeaderText="Line Number" DataField="line_number" Width="80" ResourceGroup="outbound_detail" ResourceName="line_number" />
                    <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" HeaderText="Item Number" DataField="item_number" Width="100" ResourceGroup="item" ResourceName="item_number" />
                    <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Cust PO No" DataField="customer_purchase_order" Width="100" ResourceGroup="customer" ResourceName="customer_purchase_order" />
                    <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Customer Code" DataField="customer_code" Width="100" ResourceGroup="customer" ResourceName="customer_code" />
                    <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" HeaderText="Customer Name" DataField="customer_name" Width="120" ResourceGroup="customer" ResourceName="customer" />
                    <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" HeaderText="Owner Code" DataField="owner_code" Width="80" ResourceGroup="Owner" ResourceName="Code" />
                    <ucControls:GridColumnExt ID="GridColumnExt17" runat="server" HeaderText="Order Type" DataField="order_type" Width="80" ResourceGroup="outbound_master" ResourceName="order_type" />
                    <ucControls:GridColumnExt ID="GridColumnExt18" runat="server" HeaderText="Quantity" DataField="quantity_order" Width="80"  ResourceGroup="outbound_detail" ResourceName="quantity_order"  />
                    <ucControls:GridColumnExt ID="GridColumnExt19" runat="server" HeaderText="UOM" DataField="uom" Width="80" ResourceGroup="item_uom" ResourceName="uom" />
                    <ucControls:GridColumnExt ID="GridColumnExt20" runat="server" HeaderText="Ship To" DataField="ship_to" Width="120" ResourceGroup="outbound_master" ResourceName="item_number" />
                </CustomColumnTemplate>
            </ucControls:GridExt>
        </DataTemplate>
    </ucControls:PanelPopup>

    <ucControls:PanelPopup ID="popupItemInventDetail" runat="server" CssClass="popup_medium" HeaderText="Invertory Detail">
        <DataTemplate>
            <ucControls:GridExt ID="gridItemInventDetail" runat="server"
                SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.InventoryItemInventDetail"
                KeyField="KeyID" KeyType="String" AutoSize="true" DisableSearch="true" DisableExport="true" DisableFirstSearch="true">
                <CustomCommandTemplate>
                </CustomCommandTemplate>
                <CustomSearchTemplate>
                    <ucControls:InputHidden ID="hid_inv_wh_item_master_id" runat="server" DataFieldValue="_wh_item_master_id" />
                </CustomSearchTemplate>
                <CustomColumnTemplate>
                    <ucControls:GridColumnExt ID="GridColumnExt27" runat="server" HeaderText="Item Number" DataField="item_number" Width="100" ResourceGroup="item" ResourceName="item_number" />
                    <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="Description" DataField="description" Width="120" ResourceGroup="item" ResourceName="description"/>
                    <ucControls:GridColumnExt ID="GridColumnExt26" runat="server" HeaderText="Location" DataField="location" Width="100" ResourceGroup="location" ResourceName="location" />
                    <ucControls:GridColumnExt ID="GridColumnExt28" runat="server" HeaderText="LPN" DataField="lpn" Width="100" ResourceGroup="inbound" ResourceName="lpn"  />
                    <ucControls:GridColumnExt ID="GridColumnExt29" runat="server" HeaderText="Batch Number" DataField="lot_number" Width="100" />
                    <ucControls:GridColumnExt ID="GridColumnExt30" runat="server" HeaderText="Expiry Date" DataField="expiry_date" Width="100" ResourceGroup="item" ResourceName="expiry_date" />
                    <ucControls:GridColumnExt ID="GridColumnExt33" runat="server" HeaderText="Quantity" DataField="quantity" Width="60"  ResourceGroup="outbound_detail" ResourceName="quantity_order" />
                    <ucControls:GridColumnExt ID="GridColumnExt34" runat="server" HeaderText="UOM" DataField="uom" Width="80" ResourceGroup="item_uom" ResourceName="uom" />
                </CustomColumnTemplate>
            </ucControls:GridExt>
        </DataTemplate>
    </ucControls:PanelPopup>

    <div class="col-lg-12 col-md-12 col-sm-12 pt-3 background-base">
        <asp:UpdatePanel ID="updateFilter" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" CssClass="col-sm-12">
            <ContentTemplate>
                <ucControls:PanelControlRow ID="pnFilter" runat="server" CssClass="row">
                    <ucControls:InputDropDown ID="ddlWareHouse" runat="server" Filterable="true" DataFieldValue="wh_master_id" DisplayDefault="-- All --" LabelText="Warehouse ID" ResourceGroup="warehouse" ResourceName="wh_id" BaseContentCss="col-sm-2" />
                    <ucControls:InputDropDown ID="ddlOwner" runat="server" Filterable="true" DisplayDefault="-- All --" DataFieldValue="owner_id" AutoPostBack="true" LabelText="Owner" ResourceGroup="owner" ResourceName="owner_code" BaseContentCss="col-sm-2" />
                    <ucControls:InputDropDownHD ID="ddlItemCategory" runat="server" Filterable="true" DisplayDefault="-- All --" DataFieldValue="category_id" LabelText="Item Category" ResourceGroup="category" ResourceName="category" BaseContentCss="col-sm-2" />
                    <ucControls:InputDropDownHD ID="ddlItem" runat="server" Filterable="true" DataFieldValue="item_number" ComboType="String" LabelText="Item" ResourceGroup="item" ResourceName="item_number" BaseContentCss="col-sm-2" DisplayDefault="-- All --" />
                    <ucControls:InputTextDate ID="txtDelPlanDate" runat="server" DataFieldValue="delivery_date_plan" Filterable="true" TextMode="Date" LabelText="Delivery Date Plan" ResourceGroup="outbound_master" ResourceName="delivery_date_plan" />
                </ucControls:PanelControlRow>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="updateContent" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" CssClass="row col-sm-12" style="padding-bottom: 14px;">
            <ContentTemplate>
                <ucControls:ButtonExt ID="btToggle" runat="server" Text="Hide Filter" CssClass="btn btn-sm btn-info" CausesValidation="false" OnClick="btToggle_Click" />
                <ucControls:ButtonExt ID="btSearch" runat="server" Text="Search" CssClass="btn btn-sm btn-success" CausesValidation="false" OnClick="btSearch_Click" ResourceGroup="General" ResourceName="Search" />
                <ucControls:ButtonExt ID="btClear" runat="server" Text="Clear" CssClass="btn btn-sm btn-danger" CausesValidation="false" OnClick="btClear_Click" ResourceGroup="General" ResourceName="Clear" />
            </ContentTemplate>
        </asp:UpdatePanel>


        <div style="width: 50%; float: left;">
            <span class="label label-info" style="padding: 2px 10px;">Item Order</span>
            <ucControls:GridExt ID="gridItemOrder" runat="server"
                SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.InventoryItemOrder"
                KeyField="KeyID" KeyType="String" GridAllowRowClick="true" DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true">
                <CustomCommandTemplate>
                </CustomCommandTemplate>
                <CustomSearchTemplate>
                    <ucControls:InputHidden ID="hidSessionUserItOrder" runat="server" DataFieldValue="_userID" />
                </CustomSearchTemplate>
                <CustomColumnTemplate>
                    <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Item" DataField="item_number" Width="100" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                    <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Description" DataField="item_description" Width="200" AllowSort="true" ResourceGroup="item" ResourceName="description" />
                    <ucControls:GridColumnExt ID="GridColumnExt24" runat="server" HeaderText="Category" DataField="item_category" Width="100" AllowSort="true" ResourceGroup="Category" ResourceName="Category" />
                    <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Summary Order" DataField="quantity_order" Width="100" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="SumOrder" />
                    <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="UOM" DataField="uom" Width="60" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                    <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Owner" DataField="owner_code" Width="60" AllowSort="true" ResourceGroup="owner" ResourceName="owner_code" />
                </CustomColumnTemplate>
            </ucControls:GridExt>
        </div>
        <div style="width: 50%; float: left;">
            <span class="label label-info" style="padding: 2px 10px;">Item Inventory</span>
            <ucControls:GridExt ID="gridItemInvent" runat="server"
                SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.InventoryItemInvent"
                KeyField="KeyID" KeyType="String" GridAllowRowClick="true" DisableButtonSearch="true" AutoSize="true" DisableFirstSearch="true">
                <CustomCommandTemplate>
                </CustomCommandTemplate>
                <CustomSearchTemplate>
                    <ucControls:InputHidden ID="hidSessionUserItInvent" runat="server" DataFieldValue="_userID" />
                </CustomSearchTemplate>
                <CustomColumnTemplate>
                    <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Item" DataField="item_number" Width="100" AllowSort="true" ResourceGroup="Item" ResourceName="item_number" />
                    <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Description" DataField="item_description" Width="200" AllowSort="true" ResourceGroup="Item" ResourceName="description" />
                    <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" HeaderText="Category" DataField="item_category" Width="100" AllowSort="true" ResourceGroup="Category" ResourceName="Category" />
                    <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Summary Inv." DataField="sumInv" Width="100" AllowSort="true" ResourceGroup="inventory" ResourceName="Sum_Inventory" />
                    <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Diff Quantity" DataField="quantity_diff" Width="80" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="DiffQty" />
                    <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="UOM" DataField="uom" Width="60" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                    <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Owner" DataField="owner_code" Width="60" AllowSort="true" ResourceGroup="DisplayGenernal" ResourceName="Owner" />
                </CustomColumnTemplate>
            </ucControls:GridExt>
        </div>
        <div style="clear: both;"></div>

    </div>

</asp:Content>
