<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="ItemCategory.aspx.cs" Inherits="WMS_NEW.MasterData.ItemCategory" %>

<%@ Register Src="~/MasterData/AscxControls/SubCategory.ascx" TagPrefix="ucControls" TagName="SubCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server">
        <ControlTemplate>
            <ucControls:PanelControlRow ID="PanelControlRow0" runat="server">
                <ucControls:InputTextBox ID="txtItemCategory" runat="server" DataFieldValue="item_category" IsKeyL="true" IsPrimary="true" abelText="Item Category" ResourceGroup="item" ResourceName="item_category" />
                <ucControls:InputTextBox ID="InputTextBox2" runat="server" DataFieldValue="description" LabelText="Description" ResourceGroup="category" ResourceName="description" />
                <ucControls:InputCheckBox ID="chkActive" runat="server" DataFieldValue="is_active" CheckBoxType="String" LabelText="Active" ResourceGroup="general" ResourceName="active" Checked="true" />
            </ucControls:PanelControlRow>

            <ucControls:PanelTab ID="PanelTab1" runat="server">
                <ControlTemplate>
                    <ucControls:PanelControlTab ID="PanelControlTab1" runat="server" PanelName="Sub Category" ResourceGroup="item" ResourceName="tab_item_SubCategory">
                        <ucControls:SubCategory runat="server" ID="ucSubCategory" />
                    </ucControls:PanelControlTab>
                </ControlTemplate>
            </ucControls:PanelTab>

        </ControlTemplate>
    </ucControls:PanelPopupEntity>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.ItemCategory" KeyField="KeyId" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="item_category" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="1,2">
    </ucControls:GridExt>

</asp:Content>
