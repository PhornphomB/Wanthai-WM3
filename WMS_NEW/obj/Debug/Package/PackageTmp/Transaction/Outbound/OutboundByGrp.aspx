<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="OutboundByGrp.aspx.cs" Inherits="WMS_NEW.Transaction.Outbound.OutboundByGrp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:PanelPopup ID="popupConfirmRelease" runat="server" StyleSize="Default" StyleColor="Danger">
        <DataTemplate>
            <customControls:GridViewExt ID="gvConfirmRelease" runat="server" HorizontalAlign="Left" ShowHeaderWhenEmpty="true" AllowPaging="false" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField runat="server" DataField="item_description" HeaderText="Item Description" ItemStyle-Width="200" />
                    <asp:BoundField runat="server" DataField="item_number" HeaderText="Item Number" ItemStyle-Width="100" />
                    <asp:BoundField runat="server" DataField="grade" HeaderText="Grade" ItemStyle-Width="80" />
                    <asp:BoundField runat="server" DataField="inventory_status" HeaderText="Status" ItemStyle-Width="80" />
                    <asp:BoundField runat="server" DataField="price" HeaderText="Price" ItemStyle-Width="60" />
                    <asp:BoundField runat="server" DataField="planqty" HeaderText="Plan Qty" ItemStyle-Width="60" />
                    <asp:BoundField runat="server" DataField="quantity" HeaderText="Quantity" ItemStyle-Width="60" />
                    <asp:BoundField runat="server" DataField="uom" HeaderText="UOM" ItemStyle-Width="60" />
                </Columns>
            </customControls:GridViewExt>
        </DataTemplate>
        <CommandTemplate>
            <ucControls:ButtonExt ID="btComfirm" runat="server" Text="Confirm Release" CssClass="btn btn-sm btn-danger" OnClick="btComfirm_Click" />
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:PanelPopup ID="popupLoadData" runat="server" StyleSize="Default" HeaderText="Order Load">
        <DataTemplate>

            <ucControls:PanelControlRow ID="PanelControlRow1" runat="server" CssClass="row mb-2">
                <ucControls:InputDropDown ID="ddlWarehouse" runat="server" IsPrimary="true" ComboType="String" ValidateGroup="_LoadInput" LabelText="Warehouse ID" BaseContentCss="col-lg-4" />
                <ucControls:InputDropDown ID="ddlOwner" runat="server" IsPrimary="true" ComboType="String" ValidateGroup="_LoadInput" LabelText="Owner ID" BaseContentCss="col-lg-4" />

                <asp:UpdatePanel ID="updateGenOrderNo" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" class="row col-lg-1">
                    <ContentTemplate>
                        <ucControls:InputCheckBox ID="chkGenOrderNo" runat="server" AutoPostBack="true" CheckBoxType="Boolean" LabelText="Gen.Load" BaseContentCss="col-sm-12" />
                    </ContentTemplate>
                </asp:UpdatePanel>

                <ucControls:InputTextBox ID="txtLoadId" runat="server" IsPrimary="true" ValidateGroup="_LoadInput" LabelText="Load ID" BaseContentCss="col-lg-3" />


                <asp:Panel ID="panelAddOrder" runat="server" CssClass="row col-lg-12 col-md-12 col-sm-12">
                    <ucControls:InputTextBox ID="txtStatus" runat="server" LabelText="Load Status" BaseContentCss="col-lg-4" Enabled="false" />
                    <div class="col-lg-4">
                        <div class="input-group">
                            <ucControls:InputDropDownHD runat="server" ID="ddlOrder" IsPrimary="true" LabelText="Order Number" ComboType="Guid" ValidateGroup="_ADD_ORDER" DisplayDefault="--Choose Order--" BaseContentCss="col-lg-10" />
                            <span class="input-group-prepend">
                                <ucControls:ButtonExt runat="server" ID="btAddOrder" Text="Add Order" CssClass="btn btn-success" ValidationGroup="_ADD_ORDER" CausesValidation="true" OnClick="btAddOrder_Click" />
                            </span>
                        </div>
                    </div>

                </asp:Panel>
                <ucControls:InputTextInteger ID="txtPriority" runat="server" LabelText="Priority" BaseContentCss="col-lg-4" IsPrimary="true" />
                <ucControls:InputTextDate ID="txtShipDate" runat="server" TextMode="DateTime" LabelText="Ship Date" BaseContentCss="col-sm-3" ResourceGroup="outbound_master" ResourceName="ship_date_actual" />
            </ucControls:PanelControlRow>

            <ucControls:PanelControlRow ID="panelOrderMaster" runat="server">
                <ucControls:GridExt ID="gridOutboundMaster" runat="server"
                    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.OutboundLoad" KeyField="KeyId" KeyType="Guid"
                    AutoSize="true" DisableFirstSearch="true" DisableSearchAll="true" GridAllowRowClick="true" IncludeValueFields="outbound_order_number" GridAllowRowDelete="true">
                    <CustomCommandTemplate>
                        <%--<ucControls:ButtonExt ID="btnConfirmShipPart" runat="server" Text="Confirm Partial Ship" ResourceGroup="general" ResourceName="btn_partial_ship" CssClass="btn btn-sm btn-success" CausesValidation="false" OnClick="btnConfirmShipPart_Click" OnClientClick="if (!confirm('Confirm Partial Ship ?')) return false;" />--%>
                        <ucControls:ButtonExt ID="btnConfirmShip" runat="server" Text="Confirm Ship" ResourceGroup="general" ResourceName="btn_ship" CssClass="btn btn-sm btn-info" CausesValidation="false" OnClick="btnConfirmShip_Click" OnClientClick="if (!confirm('Confirm Ship ?')) return false;" />
                    </CustomCommandTemplate>
                    <CustomSearchTemplate>
                        <ucControls:InputHidden ID="hidWarehouseID" runat="server" DataFieldValue="_wh_id" />
                        <ucControls:InputHidden ID="hidOwnerID" runat="server" DataFieldValue="_owner_code" />
                        <ucControls:InputHidden ID="hidLoadID" runat="server" DataFieldValue="_loadId" />
                    </CustomSearchTemplate>
                    <CustomColumnTemplate>
                        <%--<ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Warehouse" DataField="wh_id" />
                        <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Owner" DataField="owner_code" />
                        <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Load ID" DataField="load_id" />--%>
                        <%--<ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="" DataField="unrel" ControlType="CommandButton" CommandText="Unrelease" CommandName="UNREL" />--%>
                        <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Order No" DataField="outbound_order_number" AllowSort="true" />
                        <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Order Type (Mvt)" DataField="order_type" AllowSort="true" />
                        <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="Status" DataField="order_status" AllowSort="true" />
                        <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Customer" DataField="customer_name" AllowSort="true" />
                        <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Order Date" DataField="order_date" FormatType="Date" AllowSort="true" />
                    </CustomColumnTemplate>
                </ucControls:GridExt>
            </ucControls:PanelControlRow>

            <ucControls:PanelPopup ID="popupPickDetail" runat="server" HeaderText="Pick Detail">
                <DataTemplate>

                    <ucControls:PanelControlRow ID="PanelControlRow3" runat="server" CssClass="row mb-2">
                        <ucControls:InputTextBox ID="txtWarehouse" runat="server" Enabled="false" LabelText="Warehouse ID" BaseContentCss="col-lg-4 col-md-4" />
                        <ucControls:InputTextBox ID="txtOwner" runat="server" Enabled="false" LabelText="Owner ID" BaseContentCss="col-lg-4 col-md-4" />
                        <ucControls:InputTextBox ID="txtOrderNo" runat="server" Enabled="false" LabelText="Outbound Order No" BaseContentCss="col-lg-4 col-md-4" />
                    </ucControls:PanelControlRow>

                    <ucControls:PanelControlRow ID="PanelControlRow2" runat="server">
                        <ucControls:GridExt ID="gridPickDetail" runat="server"
                            SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.OutboundPickDetail" KeyField="KeyId" KeyType="Guid"
                            AutoSize="true" DisableFirstSearch="true" GridSortDefault="pick_line_number_int asc" DisableSearchAll="true">
                            <CustomSearchTemplate>
                                <ucControls:InputHidden ID="hidPickRefOrderMasterId" runat="server" DataFieldValue="_outbound_order_master_id" />
                            </CustomSearchTemplate>
                            <CustomColumnTemplate>
                                <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Override Pick" DataField="is_override" ResourceGroup="outbound_pick_detail" ResourceName="is_override" />
                                <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" HeaderText="Location" DataField="location" ResourceGroup="location" ResourceName="location" />
                                <ucControls:GridColumnExt ID="GridColumnExt18" runat="server" HeaderText="Item Category" DataField="category_description" Width="150" ResourceGroup="category" ResourceName="description" />
                                <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Description" DataField="item_description" Width="250" ResourceGroup="item" ResourceName="description" />
                                <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Item" DataField="item_number" ResourceGroup="item" ResourceName="item_number" />
                                <ucControls:GridColumnExt ID="GridColumnExt29" runat="server" HeaderText="Batch" DataField="lot_number" ResourceGroup="inventory" ResourceName="lot_number" />
                                <ucControls:GridColumnExt ID="GridColumnExt30" runat="server" HeaderText="Expiry Date" DataField="expiry_date" FormatType="Date" ResourceGroup="inventory" ResourceName="expiry_date" />
                                <%--<ucControls:GridColumnExt ID="GridColumnExt19" runat="server" HeaderText="Item Grade" DataField="grade" ResourceGroup="item" ResourceName="ItemGrade" />
                            <ucControls:GridColumnExt ID="GridColumnExt20" runat="server" HeaderText="Price" DataField="price" ResourceGroup="item" ResourceName="Price" />--%>
                                <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Plan Qty" DataField="quantity_plan" ResourceGroup="outbound_detail" ResourceName="quantity_plan" FormatType="Number" />
                                <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Pick Qty" DataField="quantity_pick" ResourceGroup="outbound_detail" ResourceName="quantity_pick" FormatType="Number" />
                                <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" HeaderText="Pack Qty" DataField="quantity_pack" ResourceGroup="outbound_detail" ResourceName="quantity_pack" FormatType="Number" />
                                <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="Stage Qty" DataField="quantity_stage" ResourceGroup="outbound_detail" ResourceName="quantity_stage" FormatType="Number" />
                                <ucControls:GridColumnExt ID="GridColumnExt26" runat="server" HeaderText="Load Qty" DataField="quantity_load" ResourceGroup="outbound_detail" ResourceName="quantity_load" FormatType="Number" />
                                <%--<ucControls:GridColumnExt ID="GridColumnExt27" runat="server" HeaderText="Parent LPN" DataField="parent_lpn" ResourceGroup="inventory" ResourceName="parent_lpn" />--%>
                                <ucControls:GridColumnExt ID="GridColumnExt28" runat="server" HeaderText="LPN" DataField="lpn" ResourceGroup="inventory" ResourceName="lpn" />
                                <ucControls:GridColumnExt ID="GridColumnExt35" runat="server" HeaderText="Attribute1" DataField="attribute1" Width="100" ResourceGroup="inventory" ResourceName="attribute1" />
                                <ucControls:GridColumnExt ID="GridColumnExt36" runat="server" HeaderText="Attribute2" DataField="attribute2" Width="100" ResourceGroup="inventory" ResourceName="attribute2" />
                                <ucControls:GridColumnExt ID="GridColumnExt37" runat="server" HeaderText="Attribute3" DataField="attribute3" Width="100" ResourceGroup="inventory" ResourceName="attribute3" />
                                <ucControls:GridColumnExt ID="GridColumnExt40" runat="server" HeaderText="Attribute4" DataField="attribute4" Width="100" ResourceGroup="inventory" ResourceName="attribute4" />
                                <ucControls:GridColumnExt ID="GridColumnExt51" runat="server" HeaderText="Attribute5" DataField="attribute5" Width="100" ResourceGroup="inventory" ResourceName="attribute5" />
                            </CustomColumnTemplate>
                        </ucControls:GridExt>
                    </ucControls:PanelControlRow>

                </DataTemplate>
            </ucControls:PanelPopup>

        </DataTemplate>
        <CommandTemplate>
            <ucControls:ButtonExt ID="btSave" runat="server" Text="Create" CssClass="btn btn-sm btn-primary" OnClick="btSave_Click" ValidationGroup="_LoadInput" CausesValidation="true" />
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.OutboundRelByLoad" KeyField="KeyId" KeyType="String"
        ColumnFreezeLength="0" GridSortDefault="create_date DESC" GridAllowRowEdit="true">
        <CustomCommandTemplate>
            <ucControls:ButtonExt ID="btNewLoad" runat="server" Text="New Load" CssClass="btn btn-sm btn-success" OnClick="btNewLoad_Click" />
        </CustomCommandTemplate>
        <CustomSearchTemplate>
            <ucControls:InputHidden ID="hidSessionUser" runat="server" DataFieldValue="_userID" />
            <ucControls:InputHidden ID="hidIsFirstLoad" runat="server" DataFieldValue="_isFirstLoad" />
            <ucControls:InputHidden ID="hidPickType" runat="server" DataFieldValue="_userPickType" />

            <ucControls:InputTextBox ID="txtIOrderNumber" runat="server" DataFieldValue="_orderNumber" DefaultFilter="Contains" LabelText="Order Number" />
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" HeaderText="" DataField="release" ControlType="CommandButton" CommandText="Release" CommandName="RELEASE" ResourceGroup="DisplayGenernal" ResourceName="Release" />
            <ucControls:GridColumnExt ID="GridColumnExt24" runat="server" HeaderText="" DataField="unrel" ControlType="CommandButton" CommandText="Unrelease" CommandName="UNREL" />
            <%--<ucControls:GridColumnExt ID="GridColumnExt24" runat="server" HeaderText="" DataField="unrel" ControlType="CommandButton" CommandText="Unrelease" CommandName="UNREL" />--%>
            <%--<ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="" DataField="ship" ControlType="CommandButton" CommandText="Ship" CommandName="SHIP" />
                <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="" DataField="cancel" ControlType="CommandButton" CommandText="Cancel" CommandName="CANCEL" />--%>
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Warehouse" DataField="wh_id" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" FormatType="Guid" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" HeaderText="Load ID" DataField="load_id" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Load Status" DataField="load_status" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <%--<ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Dock Door" DataField="location_dock" AllowSort="true" />--%>
            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Order Qty" DataField="order_qty" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt31" runat="server" HeaderText="Create Date" DataField="create_date" FormatType="DateTime" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" HeaderText="Priority" DataField="priority" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="priority" />

        </CustomColumnTemplate>
    </ucControls:GridExt>

</asp:Content>
