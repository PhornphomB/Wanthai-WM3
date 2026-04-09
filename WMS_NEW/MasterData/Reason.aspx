<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Reason.aspx.cs" Inherits="WMS_NEW.MasterData.Reason" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" />
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.Reason" KeyField="KeyId" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="reason_code" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="1,2,4">
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="iColReasonType" runat="server" ResourceGroup="reason" ResourceName="reason_type" DataField="reason_type" FilterFormatType="Text" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
        </CustomColumnTemplate>

    </ucControls:GridExt>

</asp:Content>
