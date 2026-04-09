<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Location.aspx.cs" Inherits="WMS_NEW.MasterData.Location" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server">
    </ucControls:PanelPopupEntity>

     <ucControls:PanelPopup ID="pnlImportFile" runat="server" HeaderText="" StyleSize="Small">
        <CommandTemplate>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <ucControls:ButtonExt ID="btUpload" ResourceGroup="general" ResourceName="btn_upload" runat="server" Text="Upload" CssClass="btn btn-success" CausesValidation="false" OnClick="btUpload_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btUpload" />
                </Triggers>
            </asp:UpdatePanel>
        </CommandTemplate>
        <DataTemplate>
            <asp:Panel ID="panel15" runat="server" Style="width: 100%; padding-left: 40px">
                <h2>IMPORT Location EXCEL</h2>
                <div class="row  col-12">
                    <asp:FileUpload ID="fuExcel" CssClass="btn btn-info col-12" runat="server" AllowMultiple="false" accept="application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" />
                </div>
            </asp:Panel>
        </DataTemplate>
    </ucControls:PanelPopup>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.Location" KeyField="KeyId" KeyType="Guid"  VisibleExportTemplate="true"
        ColumnFreezeLength="0" GridSortDefault="location" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="1,2,3">
        <CustomCommandTemplate>
              <ucControls:ButtonExt ID="btnImport" ResourceGroup="general" ResourceName="btnImport" runat="server" Text="IMPORT EXCEL" CssClass="btn btn-sm btn-warning" CausesValidation="false" OnClick="btnImport_Click" />
        </CustomCommandTemplate>
        <customcolumntemplate>
            <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="iColWarehouse" DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="location" ResourceName="loc_type" ID="iColLocType"  DataField="loc_type_name" DataFieldFilter="loc_type"  FilterFormatType="Text" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
        </customcolumntemplate>

    </ucControls:GridExt>
</asp:Content>
