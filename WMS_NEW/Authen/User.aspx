<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="WMS_NEW.Authen.User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopupEntity ID="popup1" runat="server" ColumnControlFix="2">
        <ControlTemplate>
            <ucControls:PanelControlRow ID="pnData" runat="server">
                <ucControls:InputTextBox ID="txtUsername" runat="server" DataFieldValue="user_id" IsKey="true" IsPrimary="true" LabelText="Username" ResourceGroup="user" ResourceName="user_id" />
                <ucControls:InputDropDown ID="ddlUserGroup" runat="server" DataFieldValue="user_group_id" IsPrimary="true" ComboType="String" DisplayDefault="--Select--" LabelText="User Group" ResourceGroup="user_group" ResourceName="name" />
                <ucControls:InputTextBox ID="txtFirstname" runat="server" DataFieldValue="first_name" IsPrimary="true" LabelText="Firstname" ResourceGroup="user" ResourceName="first_name" />
                <ucControls:InputTextBox ID="txtLastname" runat="server" DataFieldValue="last_name" LabelText="Lastname" ResourceGroup="user" ResourceName="last_name" />
                <ucControls:InputDropDown ID="ddlLocal" runat="server" DataFieldValue="locale_id" IsPrimary="true" ComboType="String" DisplayDefault="--Select--" LabelText="Local" ResourceGroup="user" ResourceName="locale_id" />
                <ucControls:InputTextBox ID="txtEmail" runat="server" DataFieldValue="email_address" LabelText="Email" ResourceGroup="user" ResourceName="email_address" />
                <ucControls:InputCheckBox ID="chkActive" runat="server" DataFieldValue="is_active" CheckBoxType="String" ResourceGroup="general" ResourceName="is_active"/>
                <div class="col-sm-2 pt-3">
                    <ucControls:ButtonExt ID="btResetPass" runat="server" Text="Reset Password" CssClass="btn btn-sm btn-danger" CausesValidation="false" OnClick="btResetPass_Click" OnClientClick="if (!confirm('Confirm Reset Password  ?')) return false;" />
                </div>
            </ucControls:PanelControlRow>
        </ControlTemplate>
    </ucControls:PanelPopupEntity>


    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="SecurityM.Access" SourceClassName="SecurityM.Access.Master.User" KeyField="KeyID" KeyType="String"
        GridAllowRowEdit="true" GridAllowRowDelete="true">
        <CustomSearchTemplate>
            <ucControls:InputHidden ID="hidSessionIsAdmin" runat="server" DataFieldValue="_isAdmin" />
            <ucControls:InputHidden ID="hidSessionUserGroupID" runat="server" DataFieldValue="_groupID" />
            <ucControls:InputHidden ID="hidSessionAppID" runat="server" DataFieldValue="_appID" />
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Username" DataField="user_id" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="user" ResourceName="user_id" />
            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Firstname" DataField="first_name" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="user" ResourceName="first_name" />
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Lastname" DataField="last_name" ShowFilterNow="true" AllowFilter="true" AllowSort="true" ResourceGroup="user" ResourceName="last_name" />
            <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="User Group" DataField="user_group_name" DataFieldFilter="user_group_id" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="user_group" ResourceName="name" FilterFormatType="Text" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Locale" DataField="local_name" DataFieldFilter="locale_id" FilterFormatType="Text" AllowFilter="true" AllowSort="true" ResourceGroup="user" ResourceName="local_name" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Email" DataField="email_address" AllowFilter="true" AllowSort="true" ResourceGroup="user" ResourceName="email_address" />
            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Active" DataField="is_active" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="general" ResourceName="is_active" />
            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Create By" DataField="create_by" ResourceGroup="General" ResourceName="create_by" />
            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Create Date" DataField="create_date" FormatType="DateTime" ResourceGroup="General" ResourceName="create_date" />
        </CustomColumnTemplate>
    </ucControls:GridExt>

</asp:Content>
