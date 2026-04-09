<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="ReportIssue.aspx.cs" Inherits="WMS_NEW.Transaction.Inventory.ReportIssue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" />
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inventory.ReportIssue" KeyField="KeyId" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="create_date desc" GridAllowRowEdit="false" GridAllowRowDelete="false"
        AutoGenerateColumn="false" ShowAllSort="true" ShowAllFilter="true">
        <CustomColumnTemplate>
            <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="iColWarehouse" DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ShowFilterNow="true" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="report_issue" ResourceName="issue_record" HeaderText="Tran Type" DataField="tran_type" ShowFilterNow="true" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="location" ResourceName="location" ID="iColLocation" DataField="location" FilterFormatType="Text" DropDownFilterType="LazySearch" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ShowFilterNow="true" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="report_issue" ResourceName="issue_record" HeaderText="Issue Record" DataField="issue_record" ShowFilterNow="true" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lpn" DataField="lpn" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="report_issue" ResourceName="remark" DataField="remark" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="report_issue" ResourceName="create_by" DataField="create_by" AllowSort="true" AllowFilter="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="report_issue" ResourceName="create_date" DataField="create_date" AllowSort="true" AllowFilter="true" ShowFilterNow="true" FilterFormatType="Date" />
        </CustomColumnTemplate>

    </ucControls:GridExt>

</asp:Content>