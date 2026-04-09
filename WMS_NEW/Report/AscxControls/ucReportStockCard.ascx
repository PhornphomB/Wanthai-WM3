<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReportStockCard.ascx.cs" Inherits="WMS_NEW.Report.AscxControls.ucReportStockCard" %>
<%@ Register Src="~/Report/AscxControls/ucReportViewer.ascx" TagPrefix="ucControls" TagName="ucReportViewer" %>

<ucControls:ucReportViewer runat="server" ID="ucReportViewer" />
<ucControls:PanelPopup runat="server" ID="popReport" StyleSize="Small">
    <DataTemplate>
        <div style="height: 450px">
            <ucControls:InputDropDown runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="ddlWarehouse" IsPrimary="true" AutoPostBack="true" DisplayDefault="--Select--" />
            <ucControls:InputDropDownHD runat="server" ResourceGroup="item" ResourceName="item_number" ID="ddlWhItemMaster" IsPrimary="true" DisplayDefault="--Select--" />
            <ucControls:InputTextDate runat="server" ResourceGroup="report" ResourceName="start_date" ID="txtDateStart" IsPrimary="true" TextMode="Date" />
            <ucControls:InputTextDate runat="server" ResourceGroup="report" ResourceName="end_date" ID="txtDateEnd" IsPrimary="true" TextMode="Date" />
        </div>
    </DataTemplate>
    <CommandTemplate>
        <ucControls:ButtonExt runat="server" ResourceGroup="general" ResourceName="report" ID="btnReport" CssClass="btn btn-sm btn-primary" CausesValidation="true" OnClick="btnReport_Click" />
    </CommandTemplate>
</ucControls:PanelPopup>
