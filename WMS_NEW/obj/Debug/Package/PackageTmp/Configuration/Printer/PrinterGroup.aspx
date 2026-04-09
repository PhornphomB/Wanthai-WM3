<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="PrinterGroup.aspx.cs" Inherits="WMS_NEW.Configuration.Printer.PrinterGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" />
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Configuration.Printer.PrinterGroup" KeyField="group_id" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="group_name" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="1,2" AutoGenColumnFields_Exclude="location_id">
        <CustomColumnTemplate>
            <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="iColWarehouse" DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />

        </CustomColumnTemplate>

    </ucControls:GridExt>
</asp:Content>
