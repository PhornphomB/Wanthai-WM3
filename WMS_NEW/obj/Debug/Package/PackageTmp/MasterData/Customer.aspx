<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Customer.aspx.cs" Inherits="WMS_NEW.MasterData.Customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:EntityCustomContent ID="contentEntity" runat="server">
        <TemplateIControls>
            <%-- IF ENABLE CHANGE TabIndex="2" --%>
            <ucControls:InputCheckBox ID="InputCheckBox1" runat="server" DataFieldValue="allow_short_shipment" CheckBoxType="String"
                TabIndex="1" ColumnSpan="2" VisibleExt="false" />
            <ucControls:InputCheckBox ID="chkAllowLotsOrder" runat="server" DataFieldValue="allow_mixed_lots_in_order_shipment" CheckBoxType="String" AutoPostBack="true" LabelText="Mixed Lots in Order/Shipment"
                TabIndex="2" ColumnSpan="2" VisibleExt="true" />
            <ucControls:InputTextInteger ID="txtLotAllowOrder" runat="server" DataFieldValue="lot_allow_order" Readonly="true" LabelText="Lots"
                TabIndex="2" ColumnSpan="1" VisibleExt="true" />
            <ucControls:InputTextInteger ID="txtDayBetweenLot" runat="server" DataFieldValue="day_between_lot" Readonly="true" LabelText="Day Between Lot"
                TabIndex="2" ColumnSpan="1" VisibleExt="true" />
            <ucControls:InputCheckBox ID="chkAllowLotsShipping" runat="server" DataFieldValue="allow_mixed_lots_in_shipping_container" CheckBoxType="String" AutoPostBack="true" LabelText="Mixed Lots in Ship/Container"
                TabIndex="2" ColumnSpan="2" VisibleExt="false" />
            <ucControls:InputTextInteger ID="txtLotAllowShipping" runat="server" DataFieldValue="lot_allow_shipping" Readonly="true" LabelText="Lots"
                TabIndex="2" ColumnSpan="1" EndOfLRow="true" VisibleExt="false" />

            <ucControls:InputCheckBox ID="chkControlShelfLife" runat="server" DataFieldValue="control_shelf_life" CheckBoxType="String" AutoPostBack="true" LabelText="Control Shelf Life Validation"
                TabIndex="2" ColumnSpan="2" />
            <ucControls:InputCheckBox ID="chkCustomerShelfLife" runat="server" DataFieldValue="customer_shelf_life" CheckBoxType="String" AutoPostBack="true" LabelText="Customer Shelf Life Day Remainnig (days)"
                TabIndex="2" ColumnSpan="2" />
            <ucControls:InputCheckBox ID="chkItemShelfLife" runat="server" DataFieldValue="item_shelf_life" CheckBoxType="String" AutoPostBack="true" LabelText="Item Shelf Life Day Remainnig (days)"
                TabIndex="2" ColumnSpan="2" />
            <ucControls:InputTextInteger ID="txtCustomerShelfLife" runat="server" DataFieldValue="shelf_life_day_remaining" Readonly="true" LabelText="Days"
                TabIndex="2" ColumnSpan="2" EndOfLRow="true" />

            <ucControls:InputCheckBox ID="chkControlNoBackDate" runat="server" DataFieldValue="control_no_back_date" CheckBoxType="String" LabelText="Control No Back Date"
                TabIndex="2" ColumnSpan="2" />
            <ucControls:InputCheckBox ID="chkControlReturnedLotProcessing" runat="server" DataFieldValue="control_returned_lot_processing" CheckBoxType="String" AutoPostBack="true" LabelText="Control Return Lot Processing"
                TabIndex="2" ColumnSpan="2" />
            <ucControls:InputCheckBox ID="InputCheckBox4" runat="server" DataFieldValue="tihi_information" CheckBoxType="String" LabelText="TiHi Infomation (show on PDA"
                TabIndex="2" ColumnSpan="2" EndOfLRow="true" VisibleExt="false" />
        </TemplateIControls>
    </ucControls:EntityCustomContent>

    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" />

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.Customer" KeyField="KeyId" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="owner_code,customer_code" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="1,2,3">
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="iColOwner" runat="server" DataField="owner_code" DataFieldFilter="owner_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
        </CustomColumnTemplate>
    </ucControls:GridExt>

</asp:Content>
