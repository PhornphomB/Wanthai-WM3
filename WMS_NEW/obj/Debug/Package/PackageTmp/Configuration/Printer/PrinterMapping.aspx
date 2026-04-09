<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="PrinterMapping.aspx.cs" Inherits="WMS_NEW.Configuration.Printer.PrinterMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" />
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Configuration.Printer.PrinterMapping" KeyField="group_printer_id" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="group_name" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="1,2">
        <customcolumntemplate>
                        <ucControls:GridColumnExt runat="server" ResourceGroup="Printer" ResourceName="printer_name" ID="iColPrint" DataField="printer_name" DataFieldFilter="printer_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
                        <ucControls:GridColumnExt runat="server" ResourceGroup="PrinterGroup" ResourceName="group_name" ID="iColGroupPrint" DataField="group_name" DataFieldFilter="group_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />

        </customcolumntemplate>

    </ucControls:GridExt>
</asp:Content>
