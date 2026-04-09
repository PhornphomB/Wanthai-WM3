<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Supplier.aspx.cs" Inherits="WMS_NEW.MasterData.Supplier" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" />

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.Supplier" KeyField="KeyId" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="supplier_code" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="1,2,3">
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="iColOwner" runat="server" ResourceGroup="owner" ResourceName="owner_code" DataField="owner_code" DataFieldFilter="owner_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
        </CustomColumnTemplate>

    </ucControls:GridExt>
</asp:Content>
