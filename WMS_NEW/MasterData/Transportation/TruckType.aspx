<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="TruckType.aspx.cs" Inherits="WMS_NEW.MasterData.Transportation.TruckType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" />
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.Transportation.TruckType" KeyField="KeyId" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="truck_type" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="1">
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="iColTruckType" runat="server" DataField="truck_type" ResourceGroup="TruckType" ResourceName="truck_type" />

        </CustomColumnTemplate>

    </ucControls:GridExt>

</asp:Content>
