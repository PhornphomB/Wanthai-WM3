<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="CarRegister.aspx.cs" Inherits="WMS_NEW.Transaction.Outbound.CarRegister" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopupEntity ID="popupCarRegister" runat="server">
        <ControlTemplate>
            <ucControls:PanelControlRow ID="PanelControlRow0" runat="server">
                <ucControls:InputDropDown ID="ddlWarehouse" runat="server" ComboType="Guid" AutoPostBack="true" DataFieldValue="wh_master_id" IsPrimary="true" LabelText="Warehouse ID" ControlGroup="WH_GRB"  ResourceGroup="warehouse" ResourceName="wh_id" />
                <ucControls:InputDropDown ID="ddlOwner" runat="server" ComboType="Guid" DataFieldValue="owner_id" IsPrimary="true" LabelText="Owner Code" ControlGroup="OWN_GRB"  ResourceGroup="owner" ResourceName="owner_code" />
                <ucControls:InputDropDown ID="ddTruckType" runat="server" ComboType="String" DataFieldValue="truck_type" IsPrimary="true" LabelText="Truck Type"  ResourceGroup="CarRegister" ResourceName="truck_type" />
                <ucControls:InputDropDown ID="ddHeadTail" runat="server" ComboType="String" DataFieldValue="head_tail" IsPrimary="true" LabelText="Head/Tail"  ResourceGroup="CarRegister" ResourceName="head_tail" />
                <ucControls:InputTextBox ID="txtLicense" runat="server" DataFieldValue="license" IsPrimary="true" LabelText="License" ResourceGroup="CarRegister" ResourceName="license" />
                <ucControls:InputTextDate ID="dtpRegisterDate" runat="server" AutoPostBack="false" DataFieldValue="register_date" Enabled="true" TextMode="DateTime" LabelText="Register Date/Time" ResourceGroup="CarRegister" ResourceName="register_date" />
            </ucControls:PanelControlRow>   
            <ucControls:PanelTab ID="PanelTab1" runat="server">
                <ControlTemplate>
                    <ucControls:PanelControlTab ID="PanelControlTab1" runat="server" PanelName="Dock Door" ResourceGroup="CarRegister" ResourceName="tab_dock_door">
                        <ucControls:PanelControlRow ID="PanelControlRow1" runat="server">
                            <ucControls:InputTextDate ID="dtpDockDoorDate" runat="server" AutoPostBack="true" DataFieldValue="dock_door_date" Enabled="true" TextMode="DateTime" LabelText="Dock Door Date/Time" ResourceGroup="CarRegister" ResourceName="dock_door_date" />
                            <ucControls:InputDropDown ID="ddlDoor" ComboType="Guid" AutoPostBack="true" runat="server" DataFieldValue="location_id" LabelText="Door" ControlGroup="CarRegister" ResourceGroup="CarRegister" ResourceName="door" />
                            <%--<ucControls:InputDropDownHD ID="ddlDoorLocation" runat="server" ResourceGroup="CarRegister" ResourceName="door" DataFieldValue="location_id" AutoPostBack="true"  DisplayDefault="--Select--" ColumnSpan="2" />--%>
                        </ucControls:PanelControlRow>
                    </ucControls:PanelControlTab>
                </ControlTemplate>
            </ucControls:PanelTab>
        </ControlTemplate>
    </ucControls:PanelPopupEntity>
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.CarRegister" KeyField="KeyId" KeyType="Guid"
        ColumnFreezeLength="3" GridSortDefault="register_date" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="false" ShowAllSort="true" ShowAllFilter="true">    
        <customcolumntemplate>
            <ucControls:GridColumnExt ID="GridColumnBtnOpen" runat="server" HeaderText="Close" DataField="truck_manifest_id" ControlType="CommandButton" CommandText="OPEN" CommandName="OPEN" ResourceGroup="CarRegister" ResourceName="Open" />
            <ucControls:GridColumnExt runat="server" ID="iColWarehouse" ResourceGroup="warehouse" ResourceName="wh_id"  DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" UseFilterDropDown="true" AllowFilter="true" AllowSort="true" DropDownDisplayDefault="--All--" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ID="iColOwner" ResourceGroup="owner" ResourceName="Code" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" FilterFormatType="Guid" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ID="GridColumnExt6" HeaderText="Door" DataField="door" AllowFilter="true" AllowSort="true" ResourceGroup="CarRegister" ResourceName="door" />
            <ucControls:GridColumnExt runat="server" ID="iColTruckType" ResourceGroup="CarRegister" ResourceName="truck_type" DataField="truck_type" DataFieldFilter="truck_type" FilterFormatType="Text" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt runat="server" ID="GridColumnExt1" HeaderText="License" DataField="license" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="CarRegister" ResourceName="license" />
            <ucControls:GridColumnExt runat="server" ID="iColHeadTail" ResourceGroup="CarRegister" ResourceName="head_tail" DataField="head_tail" DataFieldFilter="head_tail" FilterFormatType="Text"  AllowFilter="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt runat="server" ID="iColLoadStatus" ResourceGroup="CarRegister" ResourceName="load_status" DataField="load_status" DataFieldFilter="load_status" FilterFormatType="Text"  AllowFilter="true" AllowSort="true"  ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt runat="server" ID="GridColumnExt2" HeaderText="Register Date/Time" DataField="register_date" FormatType="DateTime" AllowFilter="true" AllowSort="true" FilterFormatType="Date" ResourceGroup="CarRegister" ResourceName="register_date" />
            <ucControls:GridColumnExt runat="server" ID="GridColumnExt3" HeaderText="Dock Door Date/Time" DataField="dock_door_date" FormatType="DateTime" AllowFilter="true" AllowSort="true" FilterFormatType="Date" ResourceGroup="CarRegister" ResourceName="dock_door_date" />
            <ucControls:GridColumnExt runat="server" ID="GridColumnExt5" HeaderText="Close Date/Time" DataField="close_date" FormatType="DateTime" AllowFilter="true" AllowSort="true" FilterFormatType="Date" ResourceGroup="CarRegister" ResourceName="close_date" />
        </customcolumntemplate>
    </ucControls:GridExt>
</asp:Content>
