<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="InterfaceSAPHana.aspx.cs" Inherits="WMS_NEW.Administrator.InterfaceSAPHana" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server">
        <ControlTemplate>
            <div class="row">
                <ucControls:PanelControlRow ID="PanelControlRow0" runat="server">
                    <ucControls:InputDropDown runat="server" ResourceGroup="interface_hana" ResourceName="processing_status" ID="ddlProcessingStatus" DataFieldValue="processing_status" IsPrimary="true" ComboType="String" />
                    <ucControls:InputTextDate ID="txtPostingDate" runat="server" ResourceGroup="interface_hana" ResourceName="posting_date" DataFieldValue="posting_date" TextMode="Date" />
                    <ucControls:InputTextDate ID="txtDocumentDate" runat="server" ResourceGroup="interface_hana" ResourceName="document_date" DataFieldValue="document_date" TextMode="Date" />
                    <ucControls:InputTextBox ID="txtMovementType" runat="server" ResourceGroup="interface_hana" ResourceName="movement_type" DataFieldValue="movement_type" />
                    <ucControls:InputTextNumber ID="txtEntryQuantity" runat="server" ResourceGroup="interface_hana" ResourceName="entry_quantity" DataFieldValue="entry_quantity" TextMode="Decimal" />
                    <ucControls:InputTextBox ID="txtEntryUnit" runat="server" ResourceGroup="interface_hana" ResourceName="entry_unit" DataFieldValue="entry_unit" />
                    <ucControls:InputTextBox ID="txtStorageLocation" runat="server" ResourceGroup="interface_hana" ResourceName="storage_location" DataFieldValue="storage_location" />
                    <ucControls:InputTextBox ID="txtDestinationStorageLocation" runat="server" ResourceGroup="interface_hana" ResourceName="destination_storage_location" DataFieldValue="destination_storage_location" />
                </ucControls:PanelControlRow>
            </div>
        </ControlTemplate>
    </ucControls:PanelPopupEntity>


    <ucControls:PanelPopup ID="popupChangeStatus" runat="server" StyleSize="Small" HeaderText="Change Status">
        <DataTemplate>
            <ucControls:InputDropDown runat="server" ResourceGroup="interface_hana" ResourceName="processing_status" ID="ddlChangeStatus" DataFieldValue="processing_status" IsPrimary="true" ComboType="String" />
        </DataTemplate>
        <CommandTemplate>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <ucControls:ButtonExt ID="btnConfirm" runat="server" Text="Print" CssClass="btn btn-info" OnClick="btnConfirm_Click" ResourceGroup="general" ResourceName="btn_confirm" CausesValidation="false" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnConfirm" />
                </Triggers>
            </asp:UpdatePanel>
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Administrator.InterfaceSAPHana" KeyField="KeyID" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="create_date" GridAllowRowEdit="true" GridAllowRowDelete="false" GridAllowShowSelectBoxAll="true" GridAllowSelectBox="true"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="2,3,4,5,7,8,9,10,12,13,14,20,21,23,24,27,29,30,33,34" NewVisible="false">
        <CustomCommandTemplate>
            <ucControls:ButtonExt runat="server" ResourceGroup="interface_hana" ResourceName="btn_change_status" ID="btnChangeStatus" Text="Change Status" OnClick="btnChangeStatus_Click" CssClass="btn btn-sm btn-warning" CausesValidation="false" Visible="true" />
            <ucControls:ButtonExt runat="server" ResourceGroup="interface_hana" ResourceName="btn_export_sap" ID="btnExportSAP" Text="Export SAP" OnClick="btnExportSAP_Click"  CssClass="btn btn-sm btn-success" CausesValidation="false" Visible="true" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" />
        </CustomCommandTemplate>
    </ucControls:GridExt>

</asp:Content>
