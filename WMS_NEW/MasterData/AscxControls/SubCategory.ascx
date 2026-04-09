<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubCategory.ascx.cs" Inherits="WMS_NEW.MasterData.AscxControls.SubCategory" %>

<ucControls:PanelPopupEntity ID="popupEntity1" runat="server">
    <ControlTemplate>
        <ucControls:PanelControlRow ID="PanelControlRow0" runat="server">
            <ucControls:InputHidden ID="hidCategoryId" runat="server" DataFieldValue="category_id" IsStaticValue="true" />
            <ucControls:InputTextBox ID="txtSubCategory" runat="server" DataFieldValue="sub_category" IsPrimary="true" LabelText="Sub Category" ResourceGroup="subcategory" ResourceName="sub_category" />
            <ucControls:InputTextBox ID="txtDescription" runat="server" DataFieldValue="description" LabelText="Description" ResourceGroup="item_uom" ResourceName="description" />
            <ucControls:InputTextBox ID="txtInfStorage" runat="server" DataFieldValue="inf_storage" LabelText="Inf Storage" ResourceGroup="subcategory" ResourceName="inf_storage" />
            <ucControls:InputCheckBox ID="chkActive" runat="server" DataFieldValue="is_active" CheckBoxType="String" LabelText="Active" ResourceGroup="general" ResourceName="active" Checked="true" />
        </ucControls:PanelControlRow>
    </ControlTemplate>

</ucControls:PanelPopupEntity>


<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.SubCategory" KeyField="KeyId" KeyType="Guid"
    ColumnFreezeLength="0" GridAllowRowEdit="true" GridAllowRowDelete="true">
    <CustomSearchTemplate>
        <ucControls:InputHidden ID="hidCategory" runat="server" DataFieldValue="category_id" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Sub Category" DataField="sub_category" ResourceGroup="sub_category" ResourceName="sub_category" />
        <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Description" DataField="description" ResourceGroup="description" ResourceName="uom_prompt" />
        <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Inf Storage" DataField="inf_storage" ResourceGroup="subcategory" ResourceName="inf_storage" />
        <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Active" DataField="is_active" ResourceGroup="general" ResourceName="active" />
    </CustomColumnTemplate>
</ucControls:GridExt>
