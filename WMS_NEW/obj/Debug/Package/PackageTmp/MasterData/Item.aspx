<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Item.aspx.cs" Inherits="WMS_NEW.MasterData.Item" %>

<%@ Register Src="~/MasterData/AscxControls/ItemUom.ascx" TagPrefix="ucControls" TagName="ItemUom" %>
<%@ Register Src="~/MasterData/AscxControls/ItemCrossRef.ascx" TagPrefix="ucControls" TagName="ItemCrossRef" %>
<%--<%@ Register Src="~/MasterData/AscxControls/ItemPickface.ascx" TagPrefix="ucControls" TagName="ItemPickface" %>--%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:PanelPopupEntity ID="popupItem" runat="server">
        <ControlTemplate>
            <ucControls:PanelControlRow ID="PanelControlRow0" runat="server">
                <ucControls:InputDropDown ID="ddOwner" runat="server" DataFieldValue="owner_id" ComboType="Guid" IsPrimary="true" IsKey="true" LabelText="Owner" ResourceGroup="owner" ResourceName="Code" DisplayDefault="-- Select --" />
                <ucControls:InputTextBox ID="txtItemNumber" runat="server" DataFieldValue="item_number" IsPrimary="true" IsKey="true" LabelText="Item ID" ResourceGroup="item" ResourceName="item_number" />
                <ucControls:InputTextBox ID="InputTextBox2" runat="server" DataFieldValue="description" ColumnSpan="5" LabelText="Description" ResourceGroup="item" ResourceName="description" />
                <ucControls:InputCheckBox ID="chkActive" runat="server" DataFieldValue="is_active" CheckBoxType="String" ColumnSpan="1" LabelText="Active" ResourceGroup="General" ResourceName="Active" />
            </ucControls:PanelControlRow>

            <ucControls:PanelTab ID="PanelTab1" runat="server">
                <ControlTemplate>
                    <ucControls:PanelControlTab ID="PanelControlTab1" runat="server" PanelName="General" ResourceGroup="item" ResourceName="tab_general">
                        <ucControls:PanelControlRow ID="PanelControlRow1" runat="server">
                            <ucControls:InputDropDown ID="ddlRFID" runat="server" DataFieldValue="inventory_type" ComboType="String" LabelText="Inventory Type" ResourceGroup="item" ResourceName="inventory_type" UseDefaultDisplay="false" />
                            <ucControls:InputTextBox ID="InputTextBox4" runat="server" DataFieldValue="model" LabelText="Model" ResourceGroup="item" ResourceName="model" />
                            <ucControls:InputTextBox ID="InputTextBox5" runat="server" DataFieldValue="style" LabelText="Style" ResourceGroup="item" ResourceName="style" />
                            <ucControls:InputTextBox ID="InputTextBox6" runat="server" DataFieldValue="color" LabelText="Color" ResourceGroup="item" ResourceName="color" />
                            <ucControls:InputTextBox ID="InputTextBox7" runat="server" DataFieldValue="size" LabelText="Size" ResourceGroup="item" ResourceName="size" />
                            <ucControls:InputDropDown ID="ddlPrecut" runat="server" DataFieldValue="coo" ComboType="String" LabelText="Country of origin" ResourceGroup="item" ResourceName="coo" UseDefaultDisplay="false" />
                            <ucControls:InputTextNumber ID="InputTextBox9" runat="server" DataFieldValue="cost" LabelText="Cost / Price" ResourceGroup="item" ResourceName="cost" />
                            <ucControls:InputTextNumber ID="InputTextBox10" runat="server" DataFieldValue="price" Visible="false" LabelText="Price" ResourceGroup="item" ResourceName="price" />
                            <ucControls:InputDropDown ID="ddlBarcode" runat="server" DataFieldValue="grade" ComboType="String" LabelText="Grade" ResourceGroup="item" ResourceName="grade" UseDefaultDisplay="false" />
                            <ucControls:InputTextBox ID="txtDgCode" runat="server" DataFieldValue="dg_code" LabelText="DG Code" ResourceGroup="item" ResourceName="dg_code" />
                            <%--<ucControls:InputTextBox ID="InputTextBox12" runat="server" DataFieldValue="grade" TabIndex="1" />--%>
                        </ucControls:PanelControlRow>
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="PanelControlTab2" runat="server" PanelName="Control" ResourceGroup="item" ResourceName="tab_control">
                        <ucControls:PanelControlRow ID="PanelControlRow2" runat="server">
                            <ucControls:InputDropDown ID="ddBatch" runat="server" DataFieldValue="lot_control" IsPrimary="true" ComboType="String" LabelText="Batch Control" ResourceGroup="item" ResourceName="BatchControl" UseDefaultDisplay="false" />
                            <ucControls:InputDropDown ID="ddExpiryDate" runat="server" DataFieldValue="expiry_date_control" IsPrimary="true" ComboType="String" LabelText="Expiry Date Control" ResourceGroup="item" ResourceName="ExpireDateControl" UseDefaultDisplay="false" />
                            <ucControls:InputDropDown ID="ddSerial" runat="server" DataFieldValue="sn_control" IsPrimary="true" ComboType="String" LabelText="Serial Control" ResourceGroup="item" ResourceName="SerialControl" UseDefaultDisplay="false" />
                            <ucControls:InputDropDown ID="ddAttribute1" runat="server" DataFieldValue="attribute1_control" IsPrimary="true" ComboType="String" LabelText="Attribute1 Control" ResourceGroup="item" ResourceName="Attribute1" UseDefaultDisplay="false" />
                            <ucControls:InputDropDown ID="ddAttribute2" runat="server" DataFieldValue="attribute2_control" IsPrimary="true" ComboType="String" LabelText="Attribute2 Control" ResourceGroup="item" ResourceName="Attribute2" UseDefaultDisplay="false" />
                            <ucControls:InputDropDown ID="ddAttribute3" runat="server" DataFieldValue="attribute3_control" IsPrimary="true" ComboType="String" LabelText="Attribute3 Control" ResourceGroup="item" ResourceName="Attribute3" UseDefaultDisplay="false" />
                            <ucControls:InputDropDown ID="ddAttribute4" runat="server" DataFieldValue="attribute4_control" IsPrimary="true" ComboType="String" LabelText="Attribute4 Control" ResourceGroup="item" ResourceName="Attribute4" UseDefaultDisplay="false" />
                            <ucControls:InputDropDown ID="ddAttribute5" runat="server" DataFieldValue="attribute5_control" IsPrimary="true" ComboType="String" LabelText="Attribute5 Control" ResourceGroup="item" ResourceName="Attribute5" UseDefaultDisplay="false" />
                            <ucControls:InputDropDown ID="ddKit" runat="server" DataFieldValue="is_kit" IsPrimary="true" ComboType="String" LabelText="Kit" ResourceGroup="item" ResourceName="Kit" UseDefaultDisplay="false" />
                            <ucControls:InputDropDown ID="ddIsBom" runat="server" DataFieldValue="is_bom" IsPrimary="true" ComboType="String" UseDefaultDisplay="false" />
                            <ucControls:InputTextInteger ID="InputTextBox13" runat="server" DataFieldValue="days_to_expire" LabelText="Day to Expire" ResourceGroup="item" ResourceName="DateToExpire" IsPrimary="true"  />
                            <ucControls:InputTextInteger ID="InputTextInteger1" runat="server" DataFieldValue="fifo_window" LabelText="Day to FIFO" ResourceGroup="item" ResourceName="DateToFIFO" />
                            <ucControls:InputTextNumber ID="InputDropDown4" runat="server" DataFieldValue="wh_reorder_point" LabelText="Warehouse Reorder Point" ResourceGroup="item" ResourceName="ItemReorderPoint" />
                            <ucControls:InputTextNumber ID="InputTextNumber1" runat="server" DataFieldValue="wh_reorder_qty" LabelText="Warehouse Reorder Quantity" ResourceGroup="item" ResourceName="ItemReorderQuantity" />
                            <ucControls:InputTextNumber ID="InputTextNumber2" runat="server" DataFieldValue="min_qty" LabelText="Min Qty" ResourceGroup="item" ResourceName="MinQuantity" />
                            <ucControls:InputTextNumber ID="InputTextNumber3" runat="server" DataFieldValue="max_qty" LabelText="Max Qty" ResourceGroup="item" ResourceName="MaxQuantity" />
                            <ucControls:InputDropDownHD ID="ddCategory" runat="server" DataFieldValue="category_id" ComboType="Guid" IsPrimary="true" DisplayDefault="--Select--" ControlGroup="Category" ControlSequence="1" AutoPostBack="true" LabelText="Item Category" ResourceGroup="category" ResourceName="category" />
                            <ucControls:InputDropDownHD ID="ddSubCategory" runat="server" DataFieldValue="sub_category_id" ComboType="Guid" DisplayDefault="--Select--" ControlGroup="Category" ControlSequence="2" LabelText="Sub Category" ResourceGroup="sub_category" ResourceName="sub_category" IsPrimary="true" />
                        </ucControls:PanelControlRow>
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="PanelControlTab3" runat="server" PanelName="User Define" ResourceGroup="item" ResourceName="tab_user_define">
                        <ucControls:PanelControlRow ID="PanelControlRow3" runat="server">
                            <ucControls:InputTextBox ID="InputTextBox14" runat="server" DataFieldValue="user_def1" LabelText="User Defined Field 1" ResourceGroup="item" ResourceName="UDF1" />
                            <ucControls:InputTextBox ID="InputTextBox18" runat="server" DataFieldValue="user_def2" LabelText="User Defined Field 2" ResourceGroup="item" ResourceName="UDF2" />
                            <ucControls:InputTextBox ID="InputTextBox19" runat="server" DataFieldValue="user_def3" LabelText="User Defined Field 3" ResourceGroup="item" ResourceName="UDF3" />
                            <ucControls:InputTextBox ID="InputTextBox25" runat="server" DataFieldValue="user_def4" LabelText="User Defined Field 4" ResourceGroup="item" ResourceName="UDF4" />
                            <ucControls:InputTextBox ID="InputTextBox26" runat="server" DataFieldValue="user_def5" LabelText="User Defined Field 5" ResourceGroup="item" ResourceName="UDF5" />
                            <ucControls:InputTextBox ID="InputTextBox27" runat="server" DataFieldValue="user_def6" LabelText="User Defined Field 6" ResourceGroup="item" ResourceName="UDF6" />
                            <ucControls:InputTextNumber ID="InputTextBox20" runat="server" DataFieldValue="user_def7" LabelText="User Defined Field 7" ResourceGroup="item" ResourceName="UDF7" />
                            <ucControls:InputTextNumber ID="InputTextBox21" runat="server" DataFieldValue="user_def8" LabelText="User Defined Field 8" ResourceGroup="item" ResourceName="UDF8" />
                            <ucControls:InputTextDate ID="InputTextBox22" runat="server" DataFieldValue="user_def9" TextMode="Date" LabelText="User Defined Field 9" ResourceGroup="item" ResourceName="UDF9" />
                            <ucControls:InputTextDate ID="InputTextBox23" runat="server" DataFieldValue="user_def10" TextMode="Date" LabelText="User Defined Field 10" ResourceGroup="item" ResourceName="UDF10" />
                        </ucControls:PanelControlRow>
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="PanelControlTab4" runat="server" PanelName="Item Unit of Measure" ResourceGroup="item" ResourceName="tab_uom">
                        <ucControls:ItemUom runat="server" ID="ItemUom1" />
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="PanelControlTab5" runat="server" PanelName="Item Cross Reference" ResourceGroup="item" ResourceName="tab_item_cross_reference">
                        <ucControls:ItemCrossRef runat="server" ID="ItemCrossRef1" />
                    </ucControls:PanelControlTab>

                    <ucControls:PanelControlTab ID="PanelControlTab6" runat="server" PanelName="Image" ResourceGroup="item" ResourceName="tab_image">
                        <ucControls:UploadEx runat="server" ID="uploadFileEx1" FuncName="Image" FileTypes="Image" ExtensionName="jpg, jpeg, png" />
                    </ucControls:PanelControlTab>

                    <%--<ucControls:PanelControlTab ID="PanelControlTab7" runat="server" PanelName="Item Pickface" ResourceGroup="item" ResourceName="tab_item_pickface">
                        <ucControls:ItemPickface runat="server" ID="ucItemPickface" />
                    </ucControls:PanelControlTab>--%>

                </ControlTemplate>
            </ucControls:PanelTab>
        </ControlTemplate>
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
                <h2>IMPORT ITEM EXCEL</h2>
                <div class="row  col-12">
                    <asp:FileUpload ID="fuExcel" CssClass="btn btn-info col-12" runat="server" AllowMultiple="false" accept="application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" />
                </div>
            </asp:Panel>
        </DataTemplate>
    </ucControls:PanelPopup>

    <ucControls:GridExt ID="GridExt1" runat="server" VisibleExportTemplate="true"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.Item" KeyField="KeyId" KeyType="Guid"
        GridAllowRowEdit="true" GridAllowRowDelete="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="1,2,3" AutoGenColumnFields_Exclude="category_id">
        <CustomCommandTemplate>
            <ucControls:ButtonExt ID="btnImport" ResourceGroup="general" ResourceName="btnImport" runat="server" Text="IMPORT EXCEL" CssClass="btn btn-sm btn-warning" CausesValidation="false" OnClick="btnImport_Click" />
        </CustomCommandTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="iColOwner" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" FilterFormatType="Guid" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="owner" ResourceName="Code" />
            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Category" DataField="category_description" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="category" ResourceName="description" />
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Item ID" DataField="item_number" AllowFilter="true" ShowFilterNow="true" AllowSort="true"
                ResourceGroup="item" ResourceName="item_number" />
            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Description" DataField="description" AllowFilter="true" ShowFilterNow="true" AllowSort="true"
                ResourceGroup="item" ResourceName="description" />


            <ucControls:GridColumnExt ID="iColLot" runat="server" ResourceGroup="item" ResourceName="lot_control" DataField="lot_control" ShowFilterNow="true" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="iColExp" runat="server" ResourceGroup="item" ResourceName="expiry_date_control" DataField="expiry_date_control" ShowFilterNow="true" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="iColSerial" runat="server" ResourceGroup="item" ResourceName="sn_control" DataField="sn_control" ShowFilterNow="true" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />


            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Active" DataField="is_active" AllowFilter="true" AllowSort="true"
                UseFilterDropDown="true" ResourceGroup="General" ResourceName="Active" />
            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Create By" DataField="create_by" AllowFilter="true"
                ResourceGroup="General" ResourceName="create_by" />
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Create Date" DataField="create_date" FormatType="DateTime"
                ResourceGroup="General" ResourceName="create_date" />
        </CustomColumnTemplate>
    </ucControls:GridExt>

</asp:Content>
