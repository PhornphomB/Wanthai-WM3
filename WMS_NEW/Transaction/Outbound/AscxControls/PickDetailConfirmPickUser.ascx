<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PickDetailConfirmPickUser.ascx.cs" Inherits="WMS_NEW.Transaction.Outbound.AscxControls.PickDetailConfirmPickUser" %>

<ucControls:PanelPopup runat="server" ID="popPickDetail" HeaderText="Confirm Pick" StyleSize="Default">
    <DataTemplate>
        <asp:Panel ID="pnData" runat="server">
            <asp:UpdatePanel runat="server" ID="updateControl" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-sm-6">
                            <ucControls:InputTextBox ResourceGroup="outbound_master" ResourceName="outbound_order_number" ID="txtOrderNo" runat="server" Readonly="true" IsKey="true" />
                            <ucControls:InputTextBox ResourceGroup="outbound_master" ResourceName="box_number" ID="txtBoxNumber" runat="server" ValidateGroup="PopupCFPick" IsKey="true" />
                            <ucControls:InputDropDown ResourceGroup="location" ResourceName="location" ID="ddlLocation" runat="server" DisplayDefault="--Select--" IsPrimary="true" ValidateGroup="PopupCFPick" AutoPostBack="true" />
                            <ucControls:InputDropDown ResourceGroup="location" ResourceName="location_stg" ID="ddlLocationStaging" runat="server" DisplayDefault="--Select--" IsPrimary="true" ValidateGroup="PopupCFPick" />
                            <ucControls:InputDropDown ResourceGroup="inventory" ResourceName="lpn" ID="ddlLpn" runat="server" DisplayDefault="--Select--" ValidateGroup="PopupCFPick" ComboType="String" AutoPostBack="true" />

                            <ucControls:InputDropDown ResourceGroup="item" ResourceName="item_number" ID="ddlItem" runat="server" DisplayDefault="--Select--" IsPrimary="true" ValidateGroup="PopupCFPick" AutoPostBack="true" />
                            <ucControls:InputDropDown ResourceGroup="item" ResourceName="item_status" ID="ddlItemStatus" runat="server" IsPrimary="true" ValidateGroup="PopupCFPick" AutoPostBack="true" ComboType="String" />
                            <ucControls:InputDropDown ResourceGroup="inventory" ResourceName="lot_number" ID="ddlLot" runat="server" DisplayDefault="--Select--" ValidateGroup="PopupCFPick" AutoPostBack="true" ComboType="String" />
                            <ucControls:InputDropDown ResourceGroup="inventory" ResourceName="expiry_date" ID="ddlExpiryDate" runat="server" DisplayDefault="--Select--" ValidateGroup="PopupCFPick" AutoPostBack="true" ComboType="String" />
                        </div>
                        <div class="col-sm-6">
                            <ucControls:InputDropDown ResourceGroup="inventory" ResourceName="serial_number" ID="ddlSerial" runat="server" DisplayDefault="--All--" ValidateGroup="PopupCFPick" ComboType="String" />
                            <ucControls:InputTextBox ResourceGroup="inventory" ResourceName="attribute1" ID="txtAttribute1" runat="server" />
                            <ucControls:InputTextBox ResourceGroup="inventory" ResourceName="attribute2" ID="txtAttribute2" runat="server" />
                            <ucControls:InputTextBox ResourceGroup="inventory" ResourceName="attribute3" ID="txtAttribute3" runat="server" />
                            <ucControls:InputTextBox ResourceGroup="inventory" ResourceName="attribute4" ID="txtAttribute4" runat="server" />
                            <ucControls:InputTextBox ResourceGroup="inventory" ResourceName="attribute5" ID="txtAttribute5" runat="server" />
                            <ucControls:InputDropDown ResourceGroup="item_uom" ResourceName="uom" ID="ddlUOM" runat="server" IsPrimary="true" ValidateGroup="PopupCFPick" />
                            <ucControls:InputTextNumber ResourceGroup="outbound_detail" ResourceName="quantity" ID="txtQty" NumberType="Double" runat="server" IsPrimary="true" />
                        </div>
                    </div>

                </ContentTemplate>

            </asp:UpdatePanel>
        </asp:Panel>

    </DataTemplate>
    <CommandTemplate>
        <ucControls:ButtonExt ID="btConfirm" runat="server" Text="Confirm" CssClass="btn btn-primary" OnClick="btConfirm_Click"
            OnClientClick="if (!confirm('Do you want to confirm pick ?')) return false;" ValidationGroup="PopupCFPick" ResourceGroup="general" ResourceName="btn_confirm" />
    </CommandTemplate>
</ucControls:PanelPopup>
