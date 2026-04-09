<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PickListDetail.ascx.cs" Inherits="WMS_NEW.Transaction.Outbound.AscxControls.PickListDetail" %>

<%@ Register Src="~/Transaction/Outbound/AscxControls/PickDetailConfirmPick.ascx" TagPrefix="ucControls" TagName="PickDetailConfirmPick" %>
<%@ Register Src="~/Transaction/Outbound/AscxControls/PickDetailUnPick.ascx" TagPrefix="ucControls" TagName="PickDetailUnPick" %>
<%@ Register Src="~/Transaction/Outbound/AscxControls/PickDetailConfirmShip.ascx" TagPrefix="ucControls" TagName="PickDetailConfirmShip" %>
<%@ Register Src="~/Report/AscxControls/ucReportViewer.ascx" TagPrefix="ucControls" TagName="ReportViewer" %>
<%@ Register Src="~/Transaction/Outbound/AscxControls/PickDetailConfirmPickUser.ascx" TagPrefix="ucControls" TagName="PickDetailConfirmPickUser" %>


<ucControls:PanelPopup ID="popupPickDetail" runat="server" StyleSize="Large" HeaderText="Pick List Detail" ResourceGroup="outbound_master" ResourceName="TabPickListDetail">
    <DataTemplate>

        <ucControls:PickDetailConfirmPick runat="server" ID="PickDetailConfirmPick1" />
        <ucControls:PickDetailUnPick runat="server" ID="PickDetailUnPick1" />
        <ucControls:PickDetailConfirmShip runat="server" ID="PickDetailConfirmShip1" />
        <ucControls:ReportViewer runat="server" ID="ReportViewer" />
        <ucControls:PickDetailConfirmPickUser runat="server" id="PickDetailConfirmPickUser" />

        <ucControls:PanelPopup ID="popClose" runat="server" StyleSize="Small" StyleColor="Danger" HeaderText="Close Order" ResourceGroup="outbound_master" ResourceName="pop_cloase_order">
            <DataTemplate>
                <ucControls:PanelControlRow ID="PanelControlRow7" runat="server">
                    <ucControls:InputTextBox ID="txtOutboundOrderNoClose" runat="server" Enabled="false" LabelText="Outbound Order No" ResourceGroup="outbound_master" ResourceName="outbound_order_number"/>
                    <ucControls:InputTextDate ID="txtCloseDate" runat="server" TextMode="DateTime" IsPrimary="true" LabelText="Close Date" ValidateGroup="OUT_CLOSE_ORDER" ResourceGroup="outbound_master" ResourceName="close_date"/>
                    <ucControls:InputTextBox ID="txtCloseRemark" runat="server" LabelText="Close Remark" ResourceGroup="outbound_master" ResourceName="close_remark"/>
                </ucControls:PanelControlRow>
            </DataTemplate>
            <CommandTemplate>
                <ucControls:ButtonExt ID="btCloseConfirm" runat="server" Text="Confirm Close Plan" CssClass="btn btn-sm btn-danger" ValidateGroup="OUT_CLOSE_ORDER" OnClick="btCloseConfirm_Click" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" ResourceGroup="general" ResourceName="btn_close" />
            </CommandTemplate>
        </ucControls:PanelPopup>

        <ucControls:PanelControlRow ID="PanelControlRow1" runat="server" CssClass="row col-sm-12">
            <ucControls:InputTextBox ID="txtWarehouseId" DataFieldValue="WAREHOUSE_HIDE" runat="server" Enabled="false" LabelText="Warehouse ID" ResourceGroup="warehouse" ResourceName="wh_id" BaseContentCss="col-sm-3" />
            <ucControls:InputTextBox ID="txtOrderNo" runat="server" Enabled="false" LabelText="Outbound Order No" ResourceGroup="outbound_master" ResourceName="outbound_order_number" BaseContentCss="col-sm-3" />
            <ucControls:InputTextBox ID="txtCustomer" runat="server" Enabled="false" LabelText="Customer" ResourceGroup="customer" ResourceName="customer_code" BaseContentCss="col-sm-2" />
            <ucControls:InputTextBox ID="txtOwner" runat="server" Enabled="false" LabelText="Owner" ResourceGroup="owner" ResourceName="owner_code" BaseContentCss="col-sm-2" />
            <ucControls:InputTextBox ID="txtStatus" runat="server" Enabled="false" LabelText="Status" ResourceGroup="outbound_master" ResourceName="order_status" BaseContentCss="col-sm-2" />
        </ucControls:PanelControlRow>
        <ucControls:PanelControlRow ID="PanelControlRow3" runat="server" CssClass="row col-sm-12">
            <ucControls:InputTextNumber ID="txtQtyPlan" runat="server" Enabled="false" LabelText="Plan Quantity" ResourceGroup="outbound_detail" ResourceName="quantity_order" BaseContentCss="col-sm-3" />
            <ucControls:InputTextNumber ID="txtQtyPick" runat="server" Enabled="false" LabelText="Pick Quantity" ResourceGroup="outbound_detail" ResourceName="quantity_pick" BaseContentCss="col-sm-3" />
            <ucControls:InputTextNumber ID="txtQtyStaging" runat="server" Enabled="false" LabelText="Staging Quantity" ResourceGroup="outbound_detail" ResourceName="quantity_stage" BaseContentCss="col-sm-2" />
            <ucControls:InputTextNumber ID="txtQtyLoad" runat="server" Enabled="false" LabelText="Load Quantity" ResourceGroup="outbound_detail" ResourceName="quantity_load" BaseContentCss="col-sm-2" />
            <ucControls:InputTextNumber ID="txtQtyShip" runat="server" Enabled="false" LabelText="Ship Quantity" ResourceGroup="outbound_detail" ResourceName="quantity_ship" BaseContentCss="col-sm-2" />
        </ucControls:PanelControlRow>
        <ucControls:PanelControlRow ID="PanelControlRow2" runat="server" CssClass="row col-sm-12 mt-2 mb-3">
            <ucControls:ButtonExt ID="btConfirmPick" runat="server" Text="Pick" CssClass="btn btn-sm btn-primary" CausesValidation="false"
                OnClick="btConfirmPick_Click" ResourceGroup="general" ResourceName="btn_pick" />

            <ucControls:ButtonExt ID="btUnPick" runat="server" Text="Unpick" CssClass="btn btn-sm btn-warning" CausesValidation="false"
                OnClick="btUnPick_Click" ResourceGroup="general" ResourceName="btn_unpick" />

            <%--<ucControls:ButtonExt ID="btPartialShip" runat="server" Text="Partial Ship" CssClass="btn btn-sm btn-info" CausesValidation="false"
                OnClick="btPartialShip_Click" ResourceGroup="general" ResourceName="btn_partial_ship" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" />--%>

            <ucControls:ButtonExt ID="btConfirmShip" runat="server" Text="Ship" CssClass="btn btn-sm btn-success" CausesValidation="false"
                OnClick="btConfirmShip_Click" ResourceGroup="general" ResourceName="btn_ship" />

            <ucControls:ButtonExt ID="btCloseOrder" runat="server" Text="Close Order" CssClass="btn btn-sm btn-danger" CausesValidation="false"
                OnClick="btCloseOrder_Click" ResourceGroup="general" ResourceName="btn_close" />
        </ucControls:PanelControlRow>

        <ucControls:PanelTab ID="PanelTab1" runat="server">
            <ControlTemplate>

                <ucControls:PanelControlTab ID="pnDetail" runat="server" PanelName="Order Detail" ResourceGroup="outbound_master" ResourceName="tab_order_item_detail">
                    <ucControls:GridExt ID="gridDetail" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.OutboundDetail" KeyField="KeyId" KeyType="Guid"
                        AutoSize="true" DisableExport="false" DisableFirstSearch="true" GridSortDefault="line_number_int asc" DisableSearchAll="true">
                        <CustomSearchTemplate>
                            <ucControls:InputHidden ID="hidOrderMasterId" runat="server" DataFieldValue="_outbound_order_master_id" />
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Line Number" DataField="line_number" ResourceGroup="outbound_detail" ResourceName="line_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Item Category" DataField="category_description" Width="150" ResourceGroup="category" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Description" DataField="item_description" Width="250" ResourceGroup="item" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Item" DataField="item_number" ResourceGroup="item" ResourceName="item_number" />
                            <%--<ucControls:GridColumnExt ID="GridColumnExt15" runat="server" HeaderText="Item Grade" DataField="grade" ResourceGroup="item" ResourceName="ItemGrade" />
                            <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Price" DataField="price" ResourceGroup="item" ResourceName="Price" />--%>
                            <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Alter Order Qty" DataField="alter_quantity_order" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="alter_quantity_order" />
                            <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" HeaderText="Alter Pick Qty" DataField="alter_quantity_pick" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="alter_quantity_pick" />
                            <ucControls:GridColumnExt ID="GridColumnExt19" runat="server" HeaderText="Alter Load Qty" DataField="alter_quantity_load" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="alter_quantity_load" />
                            <ucControls:GridColumnExt ID="GridColumnExt20" runat="server" HeaderText="Alter Ship Qty" DataField="alter_quantity_ship" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="alter_quantity_ship" />
                            <ucControls:GridColumnExt ID="GridColumnExt27" runat="server" HeaderText="Alter UOM" DataField="alter_uom" ResourceGroup="outbound_detail" ResourceName="alter_uom" />
                            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Default Status" DataField="default_item_status" ResourceGroup="outbound_detail" ResourceName="default_item_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt52" runat="server" HeaderText="Batch" DataField="lot_number" ResourceGroup="inventory" ResourceName="lot_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt53" runat="server" HeaderText="Expiry Date" DataField="expiry_date" Width="150" FormatType="Date" ResourceGroup="inventory" ResourceName="expiry_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Order Qty" DataField="quantity_order" ResourceGroup="outbound_detail" ResourceName="quantity_order" FormatType="Number" />
                            <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Pick Qty" DataField="quantity_pick" ResourceGroup="outbound_detail" ResourceName="quantity_pick" FormatType="Number" />
                            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Load Qty" DataField="quantity_load" ResourceGroup="outbound_detail" ResourceName="quantity_load" FormatType="Number" />
                            <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Ship Qty" DataField="quantity_ship" ResourceGroup="outbound_detail" ResourceName="quantity_ship" FormatType="Number" />
                            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="UOM" DataField="uom" ResourceGroup="item_uom" ResourceName="uom" />
                            <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" HeaderText="Create By" DataField="create_by" ResourceGroup="general" ResourceName="create_by" />
                            <ucControls:GridColumnExt ID="GridColumnExt17" runat="server" HeaderText="Create Date" DataField="create_date" Width="150" FormatType="DateTime" ResourceGroup="general" ResourceName="create_date" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnPickDetail" runat="server" PanelName="Pick Detail" ResourceGroup="outbound_master" ResourceName="tab_pick_list_detail">
                    <ucControls:GridExt ID="gridPickDetail" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.OutboundPickDetail" KeyField="KeyId" KeyType="Guid"
                        AutoSize="true" DisableExport="false" DisableFirstSearch="true" GridSortDefault="pick_line_number_int asc" DisableSearchAll="true">
                        <CustomCommandTemplate>
                            <ucControls:ButtonExt ID="btReport" runat="server" Text="Report" CssClass="btn btn-sm" CausesValidation="false" OnClick="btReport_Click" ResourceGroup="general" ResourceName="btn_report" />
                        </CustomCommandTemplate>
                        <CustomSearchTemplate>
                            <ucControls:InputHidden ID="hidPickRefOrderMasterId" runat="server" DataFieldValue="_outbound_order_master_id" />
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Override Pick" DataField="is_override" ResourceGroup="outbound_pick_detail" ResourceName="is_override" />
                            <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" HeaderText="Location" DataField="location" ResourceGroup="location" ResourceName="location" />
                            <ucControls:GridColumnExt ID="GridColumnExt18" runat="server" HeaderText="Item Category" DataField="category_description" Width="150" ResourceGroup="category" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Description" DataField="item_description" Width="250" ResourceGroup="item" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Item" DataField="item_number" ResourceGroup="item" ResourceName="item_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt29" runat="server" HeaderText="Batch" DataField="lot_number" ResourceGroup="inventory" ResourceName="lot_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt30" runat="server" HeaderText="Expiry Date" DataField="expiry_date" FormatType="Date" ResourceGroup="inventory" ResourceName="expiry_date" />
                            <%--<ucControls:GridColumnExt ID="GridColumnExt19" runat="server" HeaderText="Item Grade" DataField="grade" ResourceGroup="item" ResourceName="ItemGrade" />
                            <ucControls:GridColumnExt ID="GridColumnExt20" runat="server" HeaderText="Price" DataField="price" ResourceGroup="item" ResourceName="Price" />--%>
                            <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" HeaderText="Plan Qty" DataField="quantity_plan" ResourceGroup="outbound_detail" ResourceName="quantity_plan" FormatType="Number" />
                            <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" HeaderText="Pick Qty" DataField="quantity_pick" ResourceGroup="outbound_detail" ResourceName="quantity_pick" FormatType="Number" />
                            <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="Stage Qty" DataField="quantity_stage" ResourceGroup="outbound_detail" ResourceName="quantity_stage" FormatType="Number" />
                            <ucControls:GridColumnExt ID="GridColumnExt24" runat="server" HeaderText="Pack Qty" DataField="quantity_pack" ResourceGroup="outbound_detail" ResourceName="quantity_pack" FormatType="Number" />
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
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnPickSerialDetail" runat="server" PanelName="Pick Serial Detail" ResourceGroup="outbound_master" ResourceName="tab_pick_serial_detail">
                    <ucControls:GridExt ID="gridPickSerialDetail" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.OutboundPickSerialDetail" KeyField="KeyId" KeyType="String"
                        AutoSize="true" DisableExport="false" DisableFirstSearch="true" DisableSearchAll="true">
                        <CustomSearchTemplate>
                            <ucControls:InputHidden ID="hidSrlPickRefOrderMasterId" runat="server" DataFieldValue="_outbound_order_master_id" />
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt32" runat="server" HeaderText="Location" DataField="location" ResourceGroup="location" ResourceName="location" />
                            <ucControls:GridColumnExt ID="GridColumnExt47" runat="server" HeaderText="Location Pick" DataField="pick_location" ResourceGroup="location" ResourceName="location_pick" />
                            <ucControls:GridColumnExt ID="GridColumnExt34" runat="server" HeaderText="Description" Width="250" DataField="description" ResourceGroup="item" ResourceName="description" />
                            <ucControls:GridColumnExt ID="GridColumnExt33" runat="server" HeaderText="Item" DataField="item_number" ResourceGroup="item" ResourceName="item_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt45" runat="server" HeaderText="Batch" DataField="lot_number" ResourceGroup="inventory" ResourceName="lot_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt31" runat="server" HeaderText="Serial" DataField="serial_number" ShowFilterNow="true" ResourceGroup="inventory" ResourceName="serial_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt46" runat="server" HeaderText="Expiry Date" DataField="expiry_date" FormatType="Date" ResourceGroup="inventory" ResourceName="expiry_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt44" runat="server" HeaderText="LPN" DataField="lpn" ResourceGroup="inventory" ResourceName="lpn" />
                            <ucControls:GridColumnExt ID="GridColumnExt49" runat="server" HeaderText="Pick To LPN" DataField="pick_to_lpn" ResourceGroup="inventory" ResourceName="pick_to_lpn" />
                            <%--<ucControls:GridColumnExt ID="GridColumnExt48" runat="server" HeaderText="Parent LPN" DataField="parent_lpn" ResourceGroup="inventory" ResourceName="parent_lpn" />
                            <ucControls:GridColumnExt ID="GridColumnExt50" runat="server" HeaderText="Pick To Parent LPN" DataField="pick_to_parent_lpn" ResourceGroup="inventory" ResourceName="parent_lpn" />--%>
                            <ucControls:GridColumnExt ID="GridColumnExt38" runat="server" HeaderText="Plan Qty" DataField="quantity_plan" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="quantity_plan"/>
                            <ucControls:GridColumnExt ID="GridColumnExt39" runat="server" HeaderText="Pick Qty" DataField="quantity_pick" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="quantity_pick"/>
                            <ucControls:GridColumnExt ID="GridColumnExt41" runat="server" HeaderText="Stage Qty" DataField="quantity_stage" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="quantity_stage"/>
                            <ucControls:GridColumnExt ID="GridColumnExt42" runat="server" HeaderText="Load Qty" DataField="quantity_load" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="quantity_load"/>
                            <ucControls:GridColumnExt ID="GridColumnExt43" runat="server" HeaderText="Ship Qty" DataField="quantity_ship" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="quantity_ship"/>
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

            </ControlTemplate>
        </ucControls:PanelTab>
    </DataTemplate>
</ucControls:PanelPopup>
