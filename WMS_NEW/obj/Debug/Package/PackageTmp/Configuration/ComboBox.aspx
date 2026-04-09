<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="ComboBox.aspx.cs" Inherits="WMS_NEW.Configuration.ComboBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" />
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Configuration.ComboBox" KeyField="KeyId" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="group_name" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="1,2">
        <CustomColumnTemplate>
        </CustomColumnTemplate>

    </ucControls:GridExt>

</asp:Content>
