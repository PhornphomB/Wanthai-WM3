<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInboundPrint144.ascx.cs" Inherits="WMS_NEW.Transaction.Inbound.AscxControl.ucInboundPrint144" %>

<ucControls:PanelPopup ID="popupPrint" runat="server" HeaderText="Print" CssClass="popup_medium">
    <DataTemplate>
        <asp:Panel ID="Panel1" runat="server">
            <div class="row">
                <div class="col-6">
                    <ucControls:InputTextBox runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="txtWarehouse" Enabled="false" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="item_number" ID="txtItemNumber" Enabled="false" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="description" ID="txtItemDesc" IsMultiLine="true" Enabled="false" />
                    <ucControls:InputDropDown runat="server" ResourceGroup="inventory" ResourceName="uom" ID="ddlItemUom" IsPrimary="true" DisplayDefault="--Select--" AutoPostBack="true" ComboType="Double" />
                    <ucControls:InputTextNumber runat="server" ResourceGroup="inventory" ResourceName="pack_size" ID="txtPackSize" IsPrimary="true" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="lot_number" ID="txtLotPrint" />
                    <ucControls:InputTextDate runat="server" ResourceGroup="inventory" ResourceName="mfg_date" ID="txtMfgDate" TextMode="Date" AutoPostBack="true" />
                    <ucControls:InputTextDate runat="server" ResourceGroup="inventory" ResourceName="expiry_date" ID="txtExpDate" TextMode="Date" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="remark" ID="txtRemark" IsMultiLine="true" />
                </div>
                <div class="col-6">
                    <ucControls:InputTextBox runat="server" ResourceGroup="general" ResourceName="prefix" ID="txtPrefix" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="general" ResourceName="suffix" ID="txtSuffix" />
                    <ucControls:InputCheckBox runat="server" ResourceGroup="general" ResourceName="chk_lpn" ID="chkLPN" AutoPostBack="true" Checked="false" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="lpn" ID="txtLpn" DataFieldValue="LPN" />
                    <ucControls:InputTextNumber runat="server" ResourceGroup="inventory" ResourceName="quantity" ID="txtQTY" IsPrimary="true" NumberType="Double" />
                    <ucControls:InputTextInteger runat="server" ResourceGroup="general" ResourceName="number_copy" ID="txtNumberofCopy" IsPrimary="true" />
                    <ucControls:InputCheckBox runat="server" ID="chkFullAmt" LabelText="Full Amount" CheckBoxType="Boolean" />
                    <ucControls:InputDropDown runat="server" ResourceGroup="PrinterGroup" ResourceName="group_name" ID="ddlGroupPrint" IsPrimary="true" />
                    <ucControls:InputDropDown runat="server" ResourceGroup="Printer" ResourceName="printer_name" ID="ddlPrint" />
                </div>
            </div>
        </asp:Panel>
    </DataTemplate>
    <CommandTemplate>
        <asp:UpdatePanel runat="server" ID="updatePrint" UpdateMode="Conditional">
            <ContentTemplate>
                <ucControls:ButtonExt runat="server" ResourceGroup="general" ResourceName="btn_print" ID="btnConfrimPrint" Text="Print" CssClass="btn btn-sm btn-info" OnClick="btnConfrimPrint_Click" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </CommandTemplate>
</ucControls:PanelPopup>
