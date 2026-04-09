<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInboundReceipt.ascx.cs" Inherits="WMS_NEW.Transaction.Inbound.AscxControl.ucInboundReceipt" %>

<ucControls:PanelPopup ID="popupReason" runat="server" HeaderText="Reason" CssClass="popup_medium">
    <DataTemplate>
    </DataTemplate>
    <CommandTemplate>
        <%--        <ucControls:ButtonExt ID="btSave" runat="server" Text="Save" CssClass="btn btn-info" OnClick="btSave_Click" OnClientClick="if (!confirm('Do you want to save data ?')) return false;" ResourceGroup="" ResourceName="" />--%>
    </CommandTemplate>
</ucControls:PanelPopup>

<ucControls:PanelPopupEntity ID="popReceipt" runat="server" HeaderText="Receipt" AutoClear="false">
    <ControlTemplate>
        <ucControls:PanelTab ID="PanelTab1" runat="server">
            <ControlTemplate>
                <ucControls:PanelControlTab ID="PanelControlTab1" runat="server" PanelName="General" ResourceGroup="tab" ResourceName="receipt">
                    <ucControls:PanelControlRow runat="server" ID="PanelControlRow0">
                        <ucControls:InputTextBox runat="server" ResourceGroup="inbound_master" ResourceName="inbound_order_number" ID="txtInboundOrder" Enabled="false" />

                        <div class="col-sm-3">
                            <div class="input-group" style="margin-left: 0px">
                                <ucControls:InputDropDown runat="server" ResourceGroup="receipt_header" ResourceName="receipt_number" ID="ddlReceiveNo" DataFieldValue="receipt_header_id" AutoPostBack="true" ColumnSpan="12" />

                                <span class="input-group-prepend">
                                    <ucControls:ButtonExt runat="server" ResourceGroup="receipt_header" ResourceName="create_receipt" ID="btGenerateOrder" Text="Create Receipt" CssClass="btn btn-info" CausesValidation="false" OnClick="btGenerateOrder_Click" />
                                </span>

                            </div>
                        </div>

                        <ucControls:InputDropDownHD runat="server" ResourceGroup="location" ResourceName="location" ID="ddlLocation" DataFieldValue="location_id" AutoPostBack="true" DisplayDefault="-- Please Select --" IsPrimary="true" />
                        <ucControls:InputDropDownHD runat="server" ResourceGroup="item" ResourceName="item_number" ID="ddlWhItem" DataFieldValue="wh_item_master_id" AutoPostBack="true" DisplayDefault="-- Please Select --" IsPrimary="true" />

                        <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="parent_lpn" ID="txtParentLPN" DataFieldValue="parent_lpn" Enabled="false" />
                        <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="lpn" ID="txtLPN" DataFieldValue="lpn" />

                        <ucControls:InputDropDown runat="server" ResourceGroup="inventory" ResourceName="inv_status_id" ID="ddlItemStatusReceive" DataFieldValue="inv_status_id" ComboType="Guid" AutoPostBack="true" DisplayDefault="-- Please Select --" />
                        <ucControls:InputDropDown runat="server" ResourceGroup="receipt_header" ResourceName="mode_receive" ID="ddlModeReceive" ComboType="String" AutoPostBack="true" />
                        <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="lot_number" ID="txtLot" DataFieldValue="lot_number" Enabled="false" />
                       
                        <ucControls:InputTextDate runat="server" ResourceGroup="inventory" ResourceName="mfg_date" ID="txtMfgDate" DataFieldValue="mfg_date" TextMode="Date" Enabled="false" AutoPostBack="true" />
                        <ucControls:InputTextDate runat="server" ResourceGroup="inventory" ResourceName="expiry_date" ID="txtExpDate" DataFieldValue="_expiry_date" TextMode="Date" Enabled="false" />
                        <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="serial_number" ID="txtSerial" DataFieldValue="serial_number" Enabled="false" />

                        <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="attribute1" ID="txtAttribute1" DataFieldValue="attribute1" Enabled="false" />
                        <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="attribute2" ID="txtAttribute2" DataFieldValue="attribute2" Enabled="false" />
                        <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="attribute3" ID="txtAttribute3" DataFieldValue="attribute3" Enabled="false" />
                        <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="attribute4" ID="txtAttribute4" DataFieldValue="attribute4" Enabled="false" />
                        <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="attribute5" ID="txtAttribute5" DataFieldValue="attribute5" Enabled="false" />

                        <ucControls:InputDropDown runat="server" ResourceGroup="item_uom" ResourceName="item_uom_id" ID="ddlUOM" DataFieldValue="item_uom_id" Enabled="false" />
                        <ucControls:InputTextNumber runat="server" ResourceGroup="inbound_detail" ResourceName="quantity_receive" ID="txtQty" DataFieldValue="quantity_received" Enabled="false" IsPrimary="true" />
                        <ucControls:InputDropDown runat="server" ResourceGroup="reason" ResourceName="reason_code" ID="ddlReason" IsPrimary="true" Visible="false" />

                    </ucControls:PanelControlRow>
                </ucControls:PanelControlTab>

            </ControlTemplate>
        </ucControls:PanelTab>
    </ControlTemplate>
</ucControls:PanelPopupEntity>





