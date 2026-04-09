<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemPickface.ascx.cs" Inherits="WMS_NEW.MasterData.AscxControls.ItemPickface" %>


<div class="col-lg-12 col-md-12 col-sm-12 pt-3 background-base">
    <%--    <asp:UpdatePanel ID="updateFilter" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" CssClass="col-sm-12">
            <ContentTemplate>
                <ucControls:PanelControlRow ID="pnFilter" runat="server" CssClass="row">
                    <ucControls:InputDropDown ID="ddlWareHouse" runat="server" Filterable="true" DataFieldValue="wh_master_id" DisplayDefault="-- All --" LabelText="Warehouse ID" ResourceGroup="warehouse" ResourceName="wh_id" BaseContentCss="col-sm-2" />
                    <ucControls:InputDropDown ID="ddlOwner" runat="server" Filterable="true" DisplayDefault="-- All --" DataFieldValue="owner_id" AutoPostBack="true" LabelText="Owner" ResourceGroup="owner" ResourceName="owner_code" BaseContentCss="col-sm-2" />
                    <ucControls:InputDropDownHD ID="ddlItemCategory" runat="server" Filterable="true" DisplayDefault="-- All --" DataFieldValue="category_id" LabelText="Item Category" ResourceGroup="category" ResourceName="category" BaseContentCss="col-sm-2" />
                    <ucControls:InputDropDownHD ID="ddlItem" runat="server" Filterable="true" DataFieldValue="item_number" ComboType="String" LabelText="Item" ResourceGroup="item" ResourceName="item_number" BaseContentCss="col-sm-2" DisplayDefault="-- All --" />
                    <ucControls:InputTextDate ID="txtDelPlanDate" runat="server" DataFieldValue="delivery_date_plan" Filterable="true" TextMode="Date" LabelText="Delivery Date Plan" ResourceGroup="outbound_master" ResourceName="delivery_date_plan" />
                </ucControls:PanelControlRow>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="updateContent" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" CssClass="row col-sm-12" style="padding-bottom: 14px;">
            <ContentTemplate>
                <ucControls:ButtonExt ID="btToggle" runat="server" Text="Hide Filter" CssClass="btn btn-sm btn-info" CausesValidation="false" OnClick="btToggle_Click" />
                <ucControls:ButtonExt ID="btSearch" runat="server" Text="Search" CssClass="btn btn-sm btn-success" CausesValidation="false" OnClick="btSearch_Click" ResourceGroup="General" ResourceName="Search" />
                <ucControls:ButtonExt ID="btClear" runat="server" Text="Clear" CssClass="btn btn-sm btn-danger" CausesValidation="false" OnClick="btClear_Click" ResourceGroup="General" ResourceName="Clear" />
            </ContentTemplate>
        </asp:UpdatePanel>--%>
    <div style="width: 49%; float: left;">
        <div class="card">
            <div style="padding: 20px 10px;">
                <span class="text-uppercase text-danger text-bold text-center mb-2" style="padding: 2px 10px;"><ucControls:LabelExt ID="lblDummy1" runat="server" DefaultText="Location Pickface" CssClass="text-uppercase text-danger text-bold text-center"  ResourceGroup="item" ResourceName="panel_location_pickface" /></span>
            </div>

            <ucControls:GridExt ID="gridLocationPickface" runat="server"
                SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.LocationPickface" GridAllowSelectBox="true"
                KeyField="KeyID" KeyType="Guid" DisableButtonSearch="true" AutoSize="true" DisableExport="true" DisableFirstSearch="true">
                <CustomCommandTemplate>
                    <ucControls:ButtonExt ID="btnAddLocation" runat="server" Text="ADD" CssClass="btn btn-sm btn-success" CausesValidation="false" ResourceGroup="General" ResourceName="ADD" OnClick="btnAddLocation_Click" />
                </CustomCommandTemplate>
                <CustomSearchTemplate>
                    <ucControls:InputHidden ID="hidSessionUserItOrder" runat="server" DataFieldValue="_userID" />
                    <ucControls:InputHidden ID="hidItemMasterLocPick" runat="server" DataFieldValue="_wh_item_master_id" />
                </CustomSearchTemplate>
                <CustomColumnTemplate>
                    <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="iColWarehouse" DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" UseFilterDropDown="false" DropDownDisplayDefault="--All--" />
                    <ucControls:GridColumnExt runat="server" ResourceGroup="location" ResourceName="loc_type" ID="iColLocType" DataField="loc_type_name" DataFieldFilter="loc_type" FilterFormatType="Text" UseFilterDropDown="false" DropDownDisplayDefault="--All--" />
                    <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="location" DataField="location" />
                </CustomColumnTemplate>
            </ucControls:GridExt>
        </div>
    </div>
    <div style="width: 49%; float: right;">
        <div class="card">
            <div style="padding: 20px 10px;">
                <span class="text-uppercase text-danger text-bold text-center mb-2" style="padding: 2px 10px;"><ucControls:LabelExt ID="LabelExt1" runat="server" DefaultText="Item Pickface" CssClass="text-uppercase text-danger text-bold text-center"  ResourceGroup="item" ResourceName="panel_item_pickface" /></span>
            </div>
            <ucControls:GridExt ID="gridLocationPickfaceItem" runat="server"
                SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.LocationPickfaceItem" GridAllowSelectBox="true"
                KeyField="KeyID" KeyType="String" DisableButtonSearch="true" DisableExport="true" AutoSize="true" DisableFirstSearch="true" OnGridRowTextChanged="gridLocationPickfaceItem_GridRowTextChanged">
                <CustomCommandTemplate>
                    <ucControls:ButtonExt ID="btnDeleteLocationItem" runat="server" Text="DELETE" CssClass="btn btn-sm btn-danger" CausesValidation="false" ResourceGroup="General" ResourceName="DELETE" OnClick="btnDeleteLocationItem_Click" />
                </CustomCommandTemplate>
                <CustomSearchTemplate>
                    <ucControls:InputHidden ID="hidSessionUserItInvent" runat="server" DataFieldValue="_userID" />
                    <ucControls:InputHidden ID="hidItemMasterLocPickItem" runat="server" DataFieldValue="_wh_item_master_id" />
                </CustomSearchTemplate>
                <CustomColumnTemplate>
                    <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="GridColumnExt2" DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" UseFilterDropDown="false" DropDownDisplayDefault="--All--" />
                    <%--    <ucControls:GridColumnExt runat="server" ResourceGroup="location" ResourceName="loc_type" ID="GridColumnExt3" DataField="loc_type_name" DataFieldFilter="loc_type" FilterFormatType="Text" UseFilterDropDown="false" DropDownDisplayDefault="--All--" />--%>
                    <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="location" DataField="location" />
                    <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="Max" DataField="max" Width="90" ControlType="Text" InputTextAutoPostBack="true" FormatType="Number" ResourceGroup="item" ResourceName="max" />
                    <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Min" DataField="min" Width="90" ControlType="Text" InputTextAutoPostBack="true" FormatType="Number" ResourceGroup="item" ResourceName="min" />
                </CustomColumnTemplate>
            </ucControls:GridExt>
        </div>
    </div>
    <div style="clear: both;"></div>

</div>
