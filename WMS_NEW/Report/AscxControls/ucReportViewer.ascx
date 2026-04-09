<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReportViewer.ascx.cs" Inherits="WMS_NEW.Report.AscxControls.ucReportViewer" %>

<ucControls:PanelPopup runat="server" ID="popReport" HeaderText="Report List" StyleSize="Small">
    <DataTemplate>
        <ucControls:GridExt ID="GridExt1" runat="server"
            SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Report.ReportViewer" KeyField="KeyId" KeyType="Guid"
            GridAllowRowEdit="false" GridAllowRowDelete="false" GridAllowRowClick="true" AutoSize="true" DisableExport="true" NewVisible="false" DisableSearchAll="true" GridSortDefault="report_seq asc">
            <CustomCommandTemplate>
            </CustomCommandTemplate>
            <CustomSearchTemplate>
                <ucControls:InputHidden runat="server" ID="hid_form_name" DataFieldValue="form_name" />
            </CustomSearchTemplate>
            <CustomColumnTemplate>
                <ucControls:GridColumnExt runat="server" ResourceGroup="report" ResourceName="report_name" ID="iColReport" DataField="report_name" HeaderText="Report Name" />
            </CustomColumnTemplate>
        </ucControls:GridExt>
    </DataTemplate>
</ucControls:PanelPopup>

<ucControls:PanelPopup ID="popReportViewer" runat="server" HeaderText="Report Viewer" StyleSize="Large">
    <DataTemplate>
        <iframe runat="server" id="ifrmReport" style="width: 100%; height: 1400px; border-width:0px;" border="0"></iframe>
    </DataTemplate>
</ucControls:PanelPopup>


