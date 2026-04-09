<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInboundPrint.ascx.cs" Inherits="WMS_NEW.Transaction.Inbound.AscxControl.ucInboundPrint" %>

<ucControls:PanelPopup ID="popupPrint" runat="server" HeaderText="Print" CssClass="popup_medium">
    <DataTemplate>
        <asp:Panel ID="Panel1" runat="server">
            <div class="row">
                <div class="col-sm-4">
                    <ucControls:InputTextBox runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="txtWarehouse" Enabled="false" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="general" ResourceName="prefix" ID="txtPrefix" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="general" ResourceName="suffix" ID="txtSuffix" />
                    <ucControls:InputCheckBox runat="server" ResourceGroup="general" ResourceName="chk_lpn" ID="chkLPN" AutoPostBack="true" Checked="false" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="lpn" ID="txtLpn" DataFieldValue="LPN" />

                </div>
                <div class="col-sm-4">
                    <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="lot_number" ID="txtLotPrint" DataFieldValue="" />
                    <ucControls:InputTextInteger runat="server" ResourceGroup="inventory" ResourceName="quantity" ID="txtQTY" IsPrimary="true" />
                    <ucControls:InputTextInteger runat="server" ResourceGroup="general" ResourceName="number_copy" ID="txtNumberofCopy" IsPrimary="true" />
                    <ucControls:InputTextInteger runat="server" ResourceGroup="general" ResourceName="number_serial" ID="txtNumberofSerial" DataFieldValue="" Enabled="false" />
                </div>
                <div class="col-sm-4">
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
            <Triggers>
                <%--<asp:PostBackTrigger ControlID="btnConfrimPrint" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </CommandTemplate>
</ucControls:PanelPopup>
