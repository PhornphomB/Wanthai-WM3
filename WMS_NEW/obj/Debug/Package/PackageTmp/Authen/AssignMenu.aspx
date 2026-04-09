<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="AssignMenu.aspx.cs" Inherits="WMS_NEW.Authen.AssignMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../_scripts/teeViewCheckBox.js" type="text/javascript"></script>
    <style type="text/css">
        .control-treeview a {
            font-size: 14px;
            color: #808080;
        }

        .panel-treeview {
            max-height: 350px;
            max-width: 100%;
            overflow: auto;
            margin-bottom: 5px;
            text-align: left;
        }

        .panel-msg {
            font-size: 22px;
            text-align: left;
            margin: 4rem 2rem;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="col-sm-12 background-base">

        <asp:UpdatePanel ID="updateContentSearch" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" class="row">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btSave" EventName="Click" />
            </Triggers>
            <ContentTemplate>

                <div class="col-sm-12 pt-2">
                    <fieldset>
                        <legend>
                            <%--<asp:Label ID="Label1" runat="server" Text="Search Option" CssClass="text-muted"  ResourceGroup="general" ResourceName="search_option"/>--%>
                            <ucControls:LabelExt ID="lblDummy1" runat="server" DefaultText="Search Option" CssClass="text-muted" ResourceGroup="general" ResourceName="search_option" />
                        </legend>
                    </fieldset>
                </div>


                <asp:Panel ID="pnCondition" runat="server" CssClass="row col-sm-12">
                    <ucControls:InputDropDown ID="ddGroupUser" runat="server" IsPrimary="true" DisplayDefault="--Select--" ComboType="String" LabelText="User Group" BaseContentCss="col-sm-3" ResourceGroup="user_group" ResourceName="name" />
                    <ucControls:InputDropDown ID="ddPlatform" runat="server" IsPrimary="true" DisplayDefault="--Select--" ComboType="String" LabelText="Platform" BaseContentCss="col-sm-3" ResourceGroup="menu" ResourceName="platform" />
                </asp:Panel>
                <asp:Panel ID="pnCommand" runat="server" CssClass="row col-sm-12 mt-3 ml-1">
                    <ucControls:ButtonExt ID="btSearch" runat="server" Text="Search" CssClass="btn btn-sm btn-success" CausesValidation="true" OnClick="btSearch_Click" ResourceGroup="General" ResourceName="Search" />
                    <asp:UpdatePanel ID="updateContentSave" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" class="ml-2">
                        <ContentTemplate>
                            <ucControls:ButtonExt ID="btSave" runat="server" Text="Assign Menu" CssClass="btn btn-sm  btn-info" CausesValidation="true"
                                OnClick="btSave_Click" OnClientClick="if (!confirm('Confirm assign menu ?')) return false;" ResourceGroup="general" ResourceName="save" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>

                <asp:Panel ID="Panel1" runat="server" CssClass="row col-sm-12 mt-4">
                    <div class="col-sm-12">
                        <fieldset>
                            <legend>
                                <%--<asp:Label ID="LabelExt0" runat="server" Text="Menu List" CssClass="text-muted" ResourceGroup="general" ResourceName="menu_list" />--%>
                                <ucControls:LabelExt ID="LabelExt1" runat="server" DefaultText="Menu List" CssClass="text-muted" ResourceGroup="general" ResourceName="menu_list" />

                            </legend>
                        </fieldset>
                    </div>
                    <asp:Panel ID="panelMsg" runat="server" CssClass="panel-msg text-danger">
                        <asp:Literal ID="labMsg" runat="server">
                            Select search option for find menu
                            <%--<ucControls:LabelExt ID="LabelExt2" runat="server" DefaultText="Select search option for find menu" ResourceGroup="general" ResourceName="select_search_info" />--%>
                        </asp:Literal>
                    </asp:Panel>

                    <div class="col-sm-12 pb-4">
                        <ucControls:PanelTab ID="PanelTab1" runat="server">
                            <ControlTemplate>
                            </ControlTemplate>
                        </ucControls:PanelTab>
                    </div>
                </asp:Panel>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
