<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PickDetailUnPick.ascx.cs" Inherits="WMS_NEW.Transaction.Outbound.AscxControls.PickDetailUnPick" %>

<ucControls:PanelPopup ID="popupPickDetail" runat="server" StyleSize="Large" StyleColor="Warning" HeaderText="Unpick Item">
    <DataTemplate>

        <ucControls:PanelControlRow ID="PanelControlRow7" runat="server" CssClass="row col-sm-12">
            <ucControls:InputTextBox ID="txtWarehouseId" runat="server" Enabled="false" LabelText="Warehouse ID" BaseContentCss="col-sm-3" ResourceGroup="warehouse" ResourceName="wh_id" />
            <ucControls:InputTextBox ID="txtOrderNo" runat="server" Enabled="false" LabelText="Outbound Order No" BaseContentCss="col-sm-3" ResourceGroup="outbound_master" ResourceName="outbound_order_number"/>
            <ucControls:InputTextBox ID="txtCustomer" runat="server" Enabled="false" LabelText="Customer" BaseContentCss="col-sm-3" ResourceGroup="customer" ResourceName="customer_code"/>
            <ucControls:InputTextBox ID="txtStatus" runat="server" Enabled="false" LabelText="Status" BaseContentCss="col-sm-3" ResourceGroup="outbound_master" ResourceName="order_status"/>
        </ucControls:PanelControlRow>

        <ucControls:GridExt ID="gridPickDetail" runat="server"
            SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.OutboundPickDetail" KeyField="KeyId" KeyType="Guid"
            DisableExport="true" DisableSearch="true" AutoSize="true" GridAllowSelectBox="true" DisableFirstSearch="true">
            <CustomCommandTemplate>
            </CustomCommandTemplate>
            <CustomSearchTemplate>
                <ucControls:InputHidden ID="hidPickRefOrderMasterId" runat="server" DataFieldValue="_outbound_order_master_id" />
                <ucControls:InputHidden ID="hidAllowUnpick" runat="server" DataFieldValue="_allow_unpick" />
            </CustomSearchTemplate>
            <CustomColumnTemplate>
                <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" HeaderText="Location" DataField="location" ResourceGroup="location" ResourceName="location" />
                <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Item" DataField="item_number" ResourceGroup="item" ResourceName="item_number" />
                <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Description" DataField="item_description" Width="250" ResourceGroup="item" ResourceName="description" />
                <ucControls:GridColumnExt ID="GridColumnExt18" runat="server" HeaderText="Item Category" DataField="category_description" Width="150" ResourceGroup="category" ResourceName="description" />
                <ucControls:GridColumnExt ID="GridColumnExt19" runat="server" HeaderText="Item Grade" DataField="grade" ResourceGroup="item" ResourceName="grade" />
                <ucControls:GridColumnExt ID="GridColumnExt20" runat="server" HeaderText="Price" DataField="price" ResourceGroup="item" ResourceName="price" />
                <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" HeaderText="Plan Qty" DataField="quantity_plan" ResourceGroup="outbound_detail" ResourceName="PlanQty" />
                <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" HeaderText="Pick Qty" DataField="quantity_pick" ResourceGroup="outbound_detail" ResourceName="PickQty" />
                <ucControls:GridColumnExt ID="GridColumnExt24" runat="server" HeaderText="Pack Qty" DataField="quantity_pack" ResourceGroup="outbound_detail" ResourceName="PackQty" />
                <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="Stage Qty" DataField="quantity_stage" ResourceGroup="outbound_detail" ResourceName="StageQty" />
                <ucControls:GridColumnExt ID="GridColumnExt26" runat="server" HeaderText="Load Qty" DataField="quantity_load" ResourceGroup="outbound_detail" ResourceName="LoadQty" />
                <ucControls:GridColumnExt ID="GridColumnExt27" runat="server" HeaderText="Parent LPN" DataField="parent_lpn" ResourceGroup="inventory" ResourceName="parent_lpn" />
                <ucControls:GridColumnExt ID="GridColumnExt28" runat="server" HeaderText="LPN" DataField="lpn" ResourceGroup="inventory" ResourceName="lpn" />
                <ucControls:GridColumnExt ID="GridColumnExt29" runat="server" HeaderText="Batch" DataField="lot_number" Width="120" ResourceGroup="inventory" ResourceName="lot_number" />
                <ucControls:GridColumnExt ID="GridColumnExt30" runat="server" HeaderText="Expiry Date" DataField="expiry_date" Width="100" ResourceGroup="inventory" ResourceName="expiry_date" />
                <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" DataField="serial_number" ResourceGroup="inventory" ResourceName="serial_number" />
             
                <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Is Staging" DataField="is_staging_location" Width="100" Visible="false" />

            </CustomColumnTemplate>
        </ucControls:GridExt>

    </DataTemplate>
    <CommandTemplate>
        <ucControls:ButtonExt ID="btConfirm" runat="server" Text="Confirm" CssClass="btn btn-sm btn-warning" CausesValidation="false" OnClick="btConfirm_Click"
            OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" ResourceGroup="general" ResourceName="btn_confirm"/>
    </CommandTemplate>
</ucControls:PanelPopup>
