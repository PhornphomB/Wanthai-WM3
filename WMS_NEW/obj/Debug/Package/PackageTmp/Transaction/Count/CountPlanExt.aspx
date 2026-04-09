<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="CountPlanExt.aspx.cs" Inherits="WMS_NEW.Transaction.Count.CountPlanExt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="updateContent1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false" class="row col-sm-12 ml-0 mr-0 pl-0 pr-0">
        <ContentTemplate>
            <asp:UpdatePanel ID="updateCreateCount" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" class="col-sm-4">
                <ContentTemplate>
                    <div class="card card-accent-success">
                        <div class="card-header">
                            <strong>Create Count Plan</strong>
                        </div>
                        <div class="card-block row">
                            <ucControls:InputDropDown ID="ddCountType" runat="server" IsPrimary="true" ComboType="String" LabelText="Count Type" ResourceGroup="count_plan_master" ResourceName="count_plan_type" BaseContentCss="col-sm-12 col-md-12 col-lg-12" />
                            <ucControls:InputCheckBox ID="chkGenOrderNo" runat="server" AutoPostBack="true" CheckBoxType="Boolean" LabelText="Auto Generate Count ID" ResourceGroup="count_plan_master" ResourceName="generate_order" BaseContentCss="col-sm-12 col-md-12 col-lg-12" />
                            <ucControls:InputTextBox ID="txtCountID" runat="server" DataFieldValue="" IsPrimary="true" IsKey="true" LabelText="Count ID" ResourceGroup="count_plan_master" ResourceName="count_id" BaseContentCss="col-sm-12 col-md-12 col-lg-12" />
                            <ucControls:InputDropDown ID="ddWarehouseId" runat="server" IsPrimary="true" LabelText="Warehouse ID" AutoPostBack="true" ResourceGroup="warehouse" ResourceName="wh_id" BaseContentCss="col-sm-12 col-md-12 col-lg-12" />
                            <ucControls:InputDropDown ID="ddlOwner" runat="server" IsPrimary="true" LabelText="Owner" AutoPostBack="true" DisplayDefault="--Select--" ResourceGroup="owner" ResourceName="Code" BaseContentCss="col-sm-12 col-md-12 col-lg-12" />
                            <ucControls:InputDropDownHD ID="ddlCustomer" runat="server" IsPrimary="true" LabelText="Customer" AutoPostBack="true" DisplayDefault="--Select--" ResourceGroup="customer" ResourceName="customer_code" BaseContentCss="col-sm-12 col-md-12 col-lg-12" />
                            <ucControls:InputTextBox ID="txtDescription" runat="server" IsMultiLine="true" LabelText="Description" ResourceGroup="count_plan_master" ResourceName="description" BaseContentCss="col-sm-12 col-md-12 col-lg-12" />
                            <div class="row col-sm-12 col-md-12 col-lg-12 align-content-center ml-0 mt-2">
                                <ucControls:ButtonExt ID="btSave" runat="server" Text="Create Plan" CssClass="btn btn-block btn-success"
                                    OnClientClick="if (!confirm('Do you want to create plan ?')) return false;" OnClick="btSave_Click" ResourceGroup="count_plan_master" ResourceName="CreatePlan" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="updateCycleCount" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" class="col-sm-8">
                <ContentTemplate>
                    <div class="card card-accent-info">
                        <div class="card-header">
                            <strong>Items Plan for Cycle Count</strong>
                        </div>
                        <div class="card-block row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <div class="row col-sm-12 col-md-12 col-lg-12">
                                    <ucControls:InputDropDownHD ID="ddLocFrom" runat="server" DataFieldValue="location" ComboType="String" Filterable="true" DefaultFilter="Equal" FixFilter="true" DisplayDefault="--All--" LabelText="Location From" ResourceGroup="count_plan_master" ResourceName="From" BaseContentCss="col-sm-12 col-md-6 col-lg-6" />
                                    <ucControls:InputDropDownHD ID="ddLocTo" runat="server" DataFieldValue="location" ComboType="String" Filterable="true" DefaultFilter="Equal" FixFilter="true" DisplayDefault="--Select--" LabelText="To" ResourceGroup="count_plan_master" ResourceName="To" BaseContentCss="col-sm-12 col-md-6 col-lg-6" />
                                </div>
                                <ucControls:InputDropDownHD ID="ddItemCat" runat="server" DisplayDefault="--All--" DataFieldValue="category_id" Filterable="true" LabelText="Item Category" ResourceGroup="category" ResourceName="description" BaseContentCss="col-sm-12 col-md-12 col-lg-12" />
                                <ucControls:InputDropDownHD ID="ddItemNumber" runat="server" DisplayDefault="--All--" DataFieldValue="item_number" ComboType="String" Filterable="true" LabelText="Item Number" ResourceGroup="count_plan_master" ResourceName="ItemNumber" BaseContentCss="col-sm-12 col-md-12 col-lg-12" />
                                <div class="row col-sm-12 col-md-12 col-lg-12 align-content-end ml-0 mt-3 mb-3">
                                    <ucControls:ButtonExt ID="btAdd" runat="server" Text="Add Filtered Items" CssClass="btn btn-sm btn-info col-sm-5 col-md-4 col-lg-3" CausesValidation="false" OnClick="btAdd_Click" ResourceGroup="count_plan_master" ResourceName="AddItem" />
                                    <ucControls:ButtonExt ID="btnRemoveItemAll" runat="server" Text="Clear All Items" CssClass="btn btn-sm btn-danger col-sm-5 col-md-4 col-lg-3" CausesValidation="false" OnClick="btnRemoveItemAll_Click" OnClientClick="if (!confirm('Confirm Clear All ?')) return false;" />
                                </div>
                            </div>
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <ucControls:GridExt ID="gridCyclePlan" runat="server"
                                    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Count.CycleCountTempExt" KeyField="KeyId" KeyType="Guid"
                                    GridAllowRowDelete="true" DisableExport="true" DisableButtonSearch="true">
                                    <CustomSearchTemplate>
                                        <ucControls:InputHidden ID="hidUserID" runat="server" DataFieldValue="_userID" />
                                    </CustomSearchTemplate>
                                    <CustomColumnTemplate>
                                        <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Location" DataField="location" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="location" />
                                        <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Description" DataField="description" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="description" />
                                        <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Item Number" DataField="item_number" AllowSort="true" ResourceGroup="count_plan_master" ResourceName="ItemNumber" />
                                        <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Parent LPN" DataField="parent_lpn" AllowSort="true" ResourceGroup="inventory" ResourceName="parent_lpn" />
                                        <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="LPN" DataField="lpn" AllowSort="true" ResourceGroup="inventory" ResourceName="lpn" />

                                        <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Lot No." DataField="lot" AllowSort="true" ResourceGroup="inventory" ResourceName="Batch" />
                                        <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Serial No." DataField="serial_number" AllowSort="true" ResourceGroup="inventory" ResourceName="serial_number" />
                                        <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Expiry Date" DataField="expiry_date" FormatType="Date" AllowSort="true" ResourceGroup="inventory" ResourceName="ExpireDate" />
                                        <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="QTY" DataField="stock_qty" AllowSort="true" ResourceGroup="Count" ResourceName="QTY" />
                                    </CustomColumnTemplate>
                                </ucControls:GridExt>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
