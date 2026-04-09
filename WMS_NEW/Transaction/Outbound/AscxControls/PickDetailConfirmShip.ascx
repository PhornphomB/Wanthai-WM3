<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PickDetailConfirmShip.ascx.cs" Inherits="WMS_NEW.Transaction.Outbound.AscxControls.PickDetailConfirmShip" %>

<ucControls:PanelPopup ID="popupPickDetail" runat="server" StyleSize="Large" StyleColor="Success" HeaderText="Confirm Ship">
    <DataTemplate>

        <ucControls:PanelControlRow ID="PanelControlRow7" runat="server" CssClass="row col-sm-12">
            <ucControls:InputHidden ID="hidWarehouseMasterId" runat="server" />
            <ucControls:InputTextBox ID="txtWarehouseId" runat="server" Enabled="false" LabelText="Warehouse ID" BaseContentCss="col-sm-3" ResourceGroup="warehouse" ResourceName="wh_id" />
            <ucControls:InputTextBox ID="txtOrderNo" runat="server" Enabled="false" LabelText="Outbound Order No" BaseContentCss="col-sm-3" ResourceGroup="outbound_master" ResourceName="outbound_order_number"/>
            <ucControls:InputDropDown ID="ddCarrier" runat="server" DisplayDefault="--Select--" LabelText="Carrier" BaseContentCss="col-sm-3" ResourceGroup="carrier" ResourceName="carrier_code"/>
            <ucControls:InputDropDown ID="ddTruckType" runat="server" DisplayDefault="--Select--" LabelText="Truck Type" BaseContentCss="col-sm-3" ResourceGroup="outbound_master" ResourceName="truck_type_id"/>
            <ucControls:InputTextBox ID="txtDriLicense" runat="server" LabelText="Driver License" BaseContentCss="col-sm-3" ResourceGroup="outbound_master" ResourceName="driver_license"/>
            <ucControls:InputTextDate ID="txtPostDate" runat="server" TextMode="Date" LabelText="Posting Date" BaseContentCss="col-sm-3" ResourceGroup="inbound_master" ResourceName="posting_date"/>
            <ucControls:InputTextDate ID="txtShipDate" runat="server" TextMode="DateTime" IsPrimary="true" LabelText="Ship Date" BaseContentCss="col-sm-3" ResourceGroup="outbound_master" ResourceName="ship_date_actual"/>
            <ucControls:InputTextBox ID="txtContainerNo" runat="server" LabelText="Container No" BaseContentCss="col-sm-3" ResourceGroup="outbound_master" ResourceName="container_no"/>
            <ucControls:InputTextNumber ID="txtQtyPlan" runat="server" Enabled="false" LabelText="Plan Quantity" BaseContentCss="col-sm-3" ResourceGroup="outbound_detail" ResourceName="quantity_plan"/>
            <ucControls:InputTextNumber ID="txtQtyStaging" runat="server" Enabled="false" LabelText="Staging Quantity" BaseContentCss="col-sm-3" ResourceGroup="outbound_detail" ResourceName="quantity_stage"/>
            <ucControls:InputTextNumber ID="txtQtyShip" runat="server" Enabled="false" LabelText="Ship Quantity" BaseContentCss="col-sm-3" ResourceGroup="outbound_detail" ResourceName="quantity_ship"/>
        </ucControls:PanelControlRow>

        <ucControls:GridExt ID="gridPickDetail" runat="server"
            SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.OutboundShipDetail" KeyField="KeyId" KeyType="Guid"
            DisableExport="true" DisableSearch="true" AutoSize="true" DisableFirstSearch="true">
            <CustomCommandTemplate>
            </CustomCommandTemplate>
            <CustomSearchTemplate>
                <ucControls:InputHidden ID="hidPickRefOrderMasterId" runat="server" DataFieldValue="_outbound_order_master_id" />
            </CustomSearchTemplate>
            <CustomColumnTemplate>
                <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Line Number" DataField="line_number" ResourceGroup="outbound_detail" ResourceName="line_number"/>
                <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Item Description" DataField="item_description" Width="220" ResourceGroup="item" ResourceName="description"/>
                <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Item No" DataField="item_number" ResourceGroup="item" ResourceName="item_number"/>
                <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Order Qty" DataField="quantity_order" ResourceGroup="outbound_detail" ResourceName="quantity_order"/>
                <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Stage Qty" DataField="quantity_stage" ResourceGroup="outbound_detail" ResourceName="quantity_stage"/>
                <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Confirm Ship Qty" DataField="quantity_stage" ResourceGroup="outbound_detail" ResourceName="quantity_stage"/>
                <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Ship Qty" DataField="quantity_ship" ResourceGroup="outbound_detail" ResourceName="quantity_ship"/>
                <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="UOM" DataField="uom" ResourceGroup="item_uom" ResourceName="uom"/>
            </CustomColumnTemplate>
        </ucControls:GridExt>

    </DataTemplate>
    <CommandTemplate>
        <ucControls:ButtonExt ID="btConfirm" runat="server" Text="Confirm" CssClass="btn btn-sm btn-success" CausesValidation="true" OnClick="btConfirm_Click"
            OnClientClick="if (!confirm('Do you want to confirm?')) return false;"  ResourceGroup="general" ResourceName="btn_confirm" />
    </CommandTemplate>
</ucControls:PanelPopup>
