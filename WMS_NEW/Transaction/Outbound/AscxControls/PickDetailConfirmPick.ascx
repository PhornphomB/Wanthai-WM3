<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PickDetailConfirmPick.ascx.cs" Inherits="WMS_NEW.Transaction.Outbound.AscxControls.PickDetailConfirmPick" %>
<%@ Register Src="~/Transaction/Outbound/AscxControls/ucSerial.ascx" TagPrefix="ucControls" TagName="ucSerial" %>

<ucControls:PanelPopup ID="popupPickDetail" runat="server" StyleSize="Large" StyleColor="Secondary" HeaderText="Confirm Pick">
    <DataTemplate>
        <ucControls:ucSerial runat="server" ID="ucSerial" />

        <ucControls:PanelControlRow ID="PanelControlRow7" runat="server" CssClass="row col-sm-12">
            <ucControls:InputTextBox ID="txtWarehouseId" runat="server" Enabled="false" LabelText="Warehouse ID" BaseContentCss="col-sm-2" ResourceGroup="warehouse" ResourceName="wh_id" />
            <ucControls:InputTextBox ID="txtOrderNo" runat="server" Enabled="false" LabelText="Outbound Order No" BaseContentCss="col-sm-2" ResourceGroup="outbound_master" ResourceName="outbound_order_number" />
            <ucControls:InputTextBox ID="txtCustomer" runat="server" Enabled="false" LabelText="Customer" BaseContentCss="col-sm-3" ResourceGroup="customer" ResourceName="customer_code" />
            <ucControls:InputTextBox ID="txtStatus" runat="server" Enabled="false" LabelText="Status" BaseContentCss="col-sm-2" ResourceGroup="outbound_master" ResourceName="order_status" />
            <ucControls:InputDropDown ID="ddLocStage" runat="server" DisplayDefault="--Select--" IsPrimary="true" ValidateGroup="PopupCFPick" LabelText="Staging Location" BaseContentCss="col-sm-3" ResourceGroup="location" ResourceName="location_stg" />
        </ucControls:PanelControlRow>

        <ucControls:GridExt ID="gridPickDetail" runat="server"
            SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.OutboundPickDetail" KeyField="KeyId" KeyType="Guid"
            DisableExport="true" DisableSearch="true" AutoSize="true" DisableFirstSearch="true">
            <CustomSearchTemplate>
                <ucControls:InputHidden ID="hidPickRefOrderMasterId" runat="server" DataFieldValue="_outbound_order_master_id" />
                <ucControls:InputHidden ID="hidAllowPick" runat="server" DataFieldValue="_allow_pick" />
            </CustomSearchTemplate>
            <CustomColumnTemplate>
                <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" HeaderText="Location" DataField="location" Width="100" ResourceGroup="location" ResourceName="location" />
                <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Description" DataField="item_description" Width="200" ResourceGroup="item" ResourceName="description" />
                <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Item" DataField="item_number" Width="100" ResourceGroup="item" ResourceName="item_number" />
                <ucControls:GridColumnExt ID="GridColumnExt29" runat="server" HeaderText="Batch" DataField="lot_number" Width="120" ResourceGroup="inventory" ResourceName="lot_number" />
                <ucControls:GridColumnExt ID="GridColumnExt30" runat="server" HeaderText="Expiry Date" DataField="expiry_date" FormatType="Date" Width="100" ResourceGroup="inventory" ResourceName="expiry_date" />

                <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Serial" DataField="select_serial" Width="100" ResourceGroup="inventory" ResourceName="select_serial" IsConfirm="false" ControlType="CommandButton" CommandName="select_serial" CommandText="Select Serial" />
                <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Serial" DataField="serial_number" Width="120" ResourceGroup="inventory" ResourceName="serial_number" />
                <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" HeaderText="Plan Qty" DataField="quantity_plan" Width="80" ResourceGroup="outbound_detail" ResourceName="quantity_plan" />
                <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" HeaderText="Pick Qty" DataField="quantity_pick" Width="80" ResourceGroup="outbound_detail" ResourceName="quantity_pick" />
                <ucControls:GridColumnExt ID="GridColumnExt24" runat="server" HeaderText="Remain Qty" DataField="quantity_remain" Width="80" ResourceGroup="outbound_detail" ResourceName="quantity_remain" />
                <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="Confirm Qty" DataField="quantity_comfirm_pick_pc" Width="90" ControlType="Text" InputTextAutoPostBack="true" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="quantity_comfirm_pick_pc" />
                <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="UOM" DataField="uom" Width="60" ResourceGroup="item_uom" ResourceName="uom" />
                <ucControls:GridColumnExt ID="GridColumnExt27" runat="server" HeaderText="Parent LPN" DataField="parent_lpn" Width="120" ResourceGroup="inventory" ResourceName="parent_lpn" />
                <ucControls:GridColumnExt ID="GridColumnExt28" runat="server" HeaderText="LPN" DataField="lpn" Width="140" ResourceGroup="inventory" ResourceName="lpn" />

                <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Serial" DataField="is_serial" Width="100" ResourceGroup="general_ex" ResourceName="select_serial" Visible="false" />
                <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Serial" DataField="sn_control" Width="100" ResourceGroup="inventory" ResourceName="sn_control" Visible="false" />

            </CustomColumnTemplate>
        </ucControls:GridExt>



    </DataTemplate>
    <CommandTemplate>
        <ucControls:ButtonExt ID="btConfirm" runat="server" Text="Confirm" CssClass="btn btn-sm btn-primary"
            OnClick="btConfirm_Click" OnClientClick="if (!confirm('Confirm Pick')) return false;" ValidationGroup="PopupCFPick" ResourceGroup="general" ResourceName="btn_confirm" />
    </CommandTemplate>
</ucControls:PanelPopup>
