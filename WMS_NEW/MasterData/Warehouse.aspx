<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Warehouse.aspx.cs" Inherits="WMS_NEW.MasterData.Warehouse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" />

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.Warehouse" KeyField="KeyId" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="wh_id" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="1,2">
    </ucControls:GridExt>
</asp:Content>
