<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="UserGroup.aspx.cs" Inherits="WMS_NEW.Authen.UserGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopupEntity ID="popup1" runat="server" ColumnControlFix="1">
        <ControlTemplate>
            <ucControls:PanelControlRow ID="pnData" runat="server">
                <ucControls:InputHidden ID="hidApplicationID" runat="server" DataFieldValue="app_id" />
                <ucControls:InputTextBox ID="txtGroupName" runat="server" DataFieldValue="name" IsKey="true" IsPrimary="true" LabelText="Group Name" ResourceGroup="user_group" ResourceName="name" />
                <ucControls:InputTextBox ID="txtDescription" runat="server" DataFieldValue="description" IsPrimary="true" LabelText="Description" ResourceGroup="user_group" ResourceName="description" />
                <ucControls:InputCheckBox ID="chkActive" runat="server" DataFieldValue="is_active" CheckBoxType="String" ResourceGroup="general" ResourceName="is_active"/>
            </ucControls:PanelControlRow>
        </ControlTemplate>
    </ucControls:PanelPopupEntity>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="SecurityM.Access" SourceClassName="SecurityM.Access.Master.UserGroup" KeyField="KeyID" KeyType="String" GridAllowRowEdit="true" GridAllowRowDelete="true">
        <CustomSearchTemplate>
            <ucControls:InputHidden ID="hidSessionAppID" runat="server" DataFieldValue="_appID" />
            <ucControls:InputHidden ID="hidSessionIsAdmin" runat="server" DataFieldValue="_isAdmin" />
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Group Name" DataField="name" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="user_group" ResourceName="name" />
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Description" DataField="description" ShowFilterNow="true" AllowFilter="true" AllowSort="true" ResourceGroup="user_group" ResourceName="description" />
            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Active" DataField="is_active" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="general" ResourceName="is_active" />
            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Create By" DataField="create_by" ResourceGroup="general" ResourceName="create_by" />
            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Create Date" DataField="create_date" FormatType="DateTime" ResourceGroup="general" ResourceName="create_date" />
        </CustomColumnTemplate>
    </ucControls:GridExt>

</asp:Content>
