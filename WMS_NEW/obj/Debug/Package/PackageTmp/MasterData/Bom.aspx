<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Bom.aspx.cs" Inherits="WMS_NEW.MasterData.Bom" %>


<%@ Register Src="~/MasterData/AscxControls/BOMAgenDetail.ascx" TagPrefix="ucControls" TagName="BOMAgenDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" ColumnControlFix="3">
        <ControlTemplate>
            <ucControls:PanelControlRow runat="server">
                <ucControls:InputDropDown ID="ddlWarehouse" runat="server" DataFieldValue="wh_master_id" IsPrimary="true" IsKey="true" />
                <ucControls:InputDropDown ID="ddAgency" runat="server" DataFieldValue="owner_id" IsPrimary="true" IsKey="true" DisplayDefault="--Select--" AutoPostBack="true" />
                <ucControls:InputDropDownHD ID="ddBom" runat="server" DataFieldValue="wh_item_master_id" IsPrimary="true" IsKey="true" AutoPostBack="true" DisplayDefault="--Select--" />
                <ucControls:InputTextBox ID="txtBomRawCode" runat="server" DataFieldValue="bom_raw_code" IsPrimary="true" IsKey="true" />
                <ucControls:InputDropDown ID="ddUom" runat="server" DataFieldValue="item_uom_id" IsPrimary="true" />
                <ucControls:InputTextBox ID="txtBomRawUOM" runat="server" DataFieldValue="bom_raw_uom" IsPrimary="true" />
                <ucControls:InputTextBox ID="txtDesc" runat="server"  DataFieldValue="bom_desc" />
                <ucControls:InputCheckBox ID="chkBomType" runat="server" DataFieldValue="is_bom_item" LabelText="Choose By Item" CheckBoxType="String" AutoPostBack="true" IsKey="true" Visible="false" />
                <ucControls:InputCheckBox ID="chkActive" runat="server" DataFieldValue="is_active" CheckBoxType="String" ColumnSpan="1" LabelText="Active" ResourceGroup="General" ResourceName="Active" />
            </ucControls:PanelControlRow>
            <ucControls:PanelTab ID="PanelTab1" runat="server">
                <ControlTemplate>
                    <ucControls:PanelControlTab ID="PanelControlTab1" runat="server" PanelName="Bom Detail" ResourceGroup="item" ResourceName="tab_bom">
                        <ucControls:BOMAgenDetail runat="server" ID="BOMAgenDetail" />
                    </ucControls:PanelControlTab>
                </ControlTemplate>
            </ucControls:PanelTab>
        </ControlTemplate>
    </ucControls:PanelPopupEntity>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.BomByItem" KeyField="KeyID" KeyType="Integer"
        GridAllowRowEdit="true" GridAllowRowDelete="true">
      
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Warehouse" DataField="wh_id" AllowFilter="true" ShowFilterNow="true" AllowSort="true" Width="150" />
            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Agency" DataField="owner_code" AllowFilter="true" ShowFilterNow="true" AllowSort="true" Width="150" />
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Bom Code" DataField="bom_code" AllowFilter="true" ShowFilterNow="true" AllowSort="true" Width="150" />
            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Description" DataField="bom_desc" AllowFilter="true" ShowFilterNow="true" AllowSort="true" Width="200" />
            <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="UOM" DataField="uom" AllowFilter="true" AllowSort="true" Width="80" />
            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Quantity" DataField="qty" AllowFilter="true" AllowSort="true" Width="100" FormatType="Integer" />
            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="Bom By Item" DataField="is_bom_item" AllowFilter="true" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Active" DataField="is_active" AllowFilter="true" AllowSort="true" Width="100" UseFilterDropDown="true" ResourceGroup="General" ResourceName="Active" />
            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Create By" DataField="create_by" Width="150" ResourceGroup="General" ResourceName="Create By" />
            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Create Date" DataField="create_date" Width="150" FormatType="DateTime" ResourceGroup="General" ResourceName="Create date" />
        </CustomColumnTemplate>
    </ucControls:GridExt>

</asp:Content>
