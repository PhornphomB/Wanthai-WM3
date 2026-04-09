<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="CancelReplenishment.aspx.cs" Inherits="WMS_NEW.Transaction.Inventory.CancelReplenishment" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" />
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.CancelReplenishment" KeyField="KeyId" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="create_date desc" GridAllowRowEdit="false" GridAllowRowDelete="false" GridAllowShowSelectBoxAll="true" GridAllowSelectBox="true"
        AutoGenerateColumn="false" ShowAllSort="true" ShowAllFilter="true">
        <CustomCommandTemplate>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <ucControls:ButtonExt runat="server" ResourceGroup="general" ResourceName="btn_cancel" ID="btnCancel" Text="Cancel" CssClass="btn btn-sm btn-danger" OnClick="btnCancel_Click" CausesValidation="false" Visible="true" OnClientClick="if (!confirm('Do you want to cancel replenishment ?')) return false;" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnCancel" />
                </Triggers>
            </asp:UpdatePanel>
        </CustomCommandTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="iColWarehouse" DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ShowFilterNow="true" AllowSort="true" AllowFilter="true" DropDownAutoPostBack="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="owner" ResourceName="owner_code" ID="iColOwner" DataField="owner_code" DataFieldFilter="owner_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ShowFilterNow="true" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="cancel_replenishment" ResourceName="source_zone"  DataField="source_zone" ShowFilterNow="true" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="cancel_replenishment" ResourceName="source_location" ID="iColLocation" DataField="source_location" DataFieldFilter="source_location_id" FilterFormatType="Guid" DropDownFilterType="LazySearch" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ShowFilterNow="true" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="cancel_replenishment" ResourceName="destination_location" ID="iColDesLocation" DataField="destination_location" DataFieldFilter="destination_location_id" FilterFormatType="Guid" DropDownFilterType="LazySearch" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ShowFilterNow="true" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lpn" DataField="lpn" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="item_number" ID="iColItem" DataField="item_number" FilterFormatType="Text" DropDownFilterType="LazySearch" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ShowFilterNow="true" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="description" DataField="item_description" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lot_number" DataField="lot_number" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="mfg_date" DataField="mfg_date" AllowSort="true" AllowFilter="true" FormatType="Date" FilterFormatType="Date" FormatString="dd/MM/yyyy" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="expiry_date" DataField="expiry_date" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="attribute1" DataField="attribute1" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="cancel_replenishment" ResourceName="quantity"  DataField="quantity" AllowSort="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item_uom" ResourceName="item_uom_id" DataField="uom" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_detail" ResourceName="create_date" DataField="create_date" AllowSort="true" AllowFilter="true" ShowFilterNow="true" FilterFormatType="Date" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="cancel_replenishment" ID="iColReplenish" ResourceName="is_replenish" DataField="is_replenish" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="cancel_replenishment" ResourceName="stg_location" DataField="stg_location" AllowSort="true" AllowFilter="true" />
            <%--<ucControls:GridColumnExt runat="server" ResourceGroup="report_issue" ResourceName="create_date" DataField="create_date" AllowSort="true" AllowFilter="true" ShowFilterNow="true" FilterFormatType="Date" />--%>
        </CustomColumnTemplate>

    </ucControls:GridExt>

</asp:Content>
