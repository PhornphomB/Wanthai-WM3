<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Truck.aspx.cs" Inherits="WMS_NEW.MasterData.Transportation.Truck" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" />
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.Transportation.Truck" KeyField="KeyId" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="truck_name" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="1,2,3">
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="iColTruckType" runat="server" DataField="truck_type" DataFieldFilter="truck_type_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="iColCarrier" runat="server" DataField="carrier_code" DataFieldFilter="carrier_id" ResourceGroup="carrier" ResourceName="carrier_code" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" DataField="carrier_name" ResourceGroup="carrier" ResourceName="carrier_name" />
        </CustomColumnTemplate>

    </ucControls:GridExt>
</asp:Content>
