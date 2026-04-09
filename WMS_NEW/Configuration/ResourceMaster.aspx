<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="ResourceMaster.aspx.cs" Inherits="WMS_NEW.Configuration.ResourceMaster" %>

<%@ Register Src="~/Configuration/AscxControls/ucResourceDetail.ascx" TagPrefix="ucControls" TagName="ucResourceDetail" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server">
        <ControlTemplate>
            <ucControls:PanelControlRow ID="pnData" runat="server">
                <ucControls:InputHidden ID="hidApplicationID" runat="server" DataFieldValue="app_id" />

                <ucControls:InputTextBox ID="InputTextBox3" runat="server" ResourceGroup="ResourceMaster" ResourceName="resource_group" DataFieldValue="resource_group" IsPrimary="true" />
                <ucControls:InputTextBox ID="InputTextBox1" runat="server" ResourceGroup="ResourceMaster" ResourceName="resource_name" DataFieldValue="resource_name" IsPrimary="true" />
                <ucControls:InputTextBox ID="InputTextBox2" runat="server" ResourceGroup="ResourceMaster" ResourceName="description" DataFieldValue="description" />
                <ucControls:InputTextBox ID="InputTextBox5" runat="server" ResourceGroup="ResourceMaster" ResourceName="default_value" DataFieldValue="default_value" />


            </ucControls:PanelControlRow>

            <ucControls:PanelTab ID="PanelTab1" runat="server">
                <ControlTemplate>
                    <ucControls:PanelControlTab ID="PanelControlTab1" runat="server" PanelName="Resource" ResourceGroup="ResourceMaster" ResourceName="tab_resource">
                        <ucControls:ucResourceDetail runat="server" ID="ucResourceDetail" />
                    </ucControls:PanelControlTab>
                </ControlTemplate>
            </ucControls:PanelTab>
        </ControlTemplate>
        <CommandTemplate>
        </CommandTemplate>
    </ucControls:PanelPopupEntity>
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Configuration.ResourceMaster" KeyField="KeyId" KeyType="String"
        ColumnFreezeLength="0" GridSortDefault="resource_group,resource_name" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="1,2,3" AutoGenColumnFields_Exclude="app_id">
        <CustomColumnTemplate>
        </CustomColumnTemplate>
        <CustomSearchTemplate>
            <ucControls:InputTextBox runat="server" ID="txtValue" ResourceGroup="ResourceMaster" ResourceName="value" DataFieldValue="_value" DefaultFilter="Contains" />
        </CustomSearchTemplate>

    </ucControls:GridExt>

</asp:Content>
