<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridViewExt.ascx.cs" Inherits="_UControls.GridViewExt" %>

<%@ Register Src="~/_UControls/PagerGridViewExt.ascx" TagName="PagerGridExt" TagPrefix="userControl" %>
<%@ Register Src="~/_UControls/ButtonExt.ascx" TagName="ButtonExt" TagPrefix="userControl" %>

<asp:UpdatePanel ID="updateGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <Triggers>
        <asp:PostBackTrigger ControlID="btExportExcel" />
         <asp:PostBackTrigger ControlID="btTemplate" />
    </Triggers>
    <ContentTemplate>
        <div class="animated fadeIn grid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="card">

                        <div class="card-header card-header-gridview">

                            <div class="col-lg-12 row" style="margin-left: 0px; margin-right: 0px; padding-left: 0px; padding-right: 0px;">
                                <div class="col-lg-7 col-md-6 col-sm-12" style="display: flex; margin-left: 0px; margin-right: 0px; padding-left: 0px; padding-right: 0px;">

                                    <asp:Panel runat="server" ID="panelSearch">
                                        <asp:UpdatePanel ID="updateContentSearch" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                            <ContentTemplate>
                                                <%--<asp:UpdatePanel ID="updateBtnOptField" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                                    style="float: left;">
                                                    <ContentTemplate>
                                                        <userControl:ButtonExt ID="btOptField" runat="server" CssClass="btn btn-sm btn-danger"
                                                            CausesValidation="false" Text="|||" OnClick="btOptField_Click" Visible="false" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>--%>
                                                <div style="float: left;">
                                                    <userControl:ButtonExt ID="btSearch" runat="server" CssClass="btn btn-sm btn-info" CausesValidation="false" OnClick="btSearch_Click" />
                                                </div>
                                                <% if (!Request.Browser.IsMobileDevice)
                                                    { %>
                                                <div style="float: left; margin-left: -1px; margin-right: 2px;">
                                                    <asp:DropDownList ID="comboLimit" runat="server" CssClass="btn btn-sm btn-primary" Style="margin-left: 0px; margin-right: 3px; padding-top: 0.40rem; padding-bottom: 0.38rem;" />
                                                </div>
                                                <% } %>
                                                <div style="clear: both;"></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                    <asp:UpdatePanel ID="updateContentCommand" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                        <ContentTemplate>
                                            <span class="float-left">
                                                <userControl:ButtonExt ID="btNew" runat="server" Text="New" CssClass="btn btn-sm btn-success" CausesValidation="false" OnClick="btNew_Click" ResourceGroup="General" ResourceName="New" />
                                            </span>
                                            <span class="float-left">
                                                <asp:PlaceHolder ID="placeCustomCommand" runat="server"></asp:PlaceHolder>
                                            </span>
                                            <%--<span style="margin-left: 5px;"></span>
                                                <span style="margin-left: 5px;"></span>--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>

                                <div class="col-lg-5 col-md-6 col-sm-12 float-right" style="margin-left: 0px; margin-right: 0px; padding-left: 0px; padding-right: 0px;">
                                    <% if (!Request.Browser.IsMobileDevice)
                                        { %>
                                    <asp:Panel runat="server" ID="panelExport" class="float-right">
                                        <div style="padding-bottom: 5px">
                                            <%--<div style="float: left;">--%>
                                             <userControl:ButtonExt ID="btTemplate" runat="server" Text="Template" CssClass="btn btn-sm btn-danger"
                                                OnClick="btTemplate_Click"  />
                                                <%--<asp:PlaceHolder ID="placeCustomCommandLeft" runat="server"></asp:PlaceHolder>--%>
                                            <%--</div>--%>
                                            <userControl:ButtonExt ID="btExportExcel" runat="server" Text="to Excel" CssClass="btn btn-sm btn-primary"
                                                OnClick="linkToExcel_Click" OnClientClick="if (!confirm('Confirm Export ?')) return false;" />
                                        </div>
                                    </asp:Panel>
                                    <% } %>
                                    <asp:UpdatePanel ID="updateContentPage" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" class="float-right" style="margin-top: -0.3rem; margin-bottom: -0.1rem;">
                                        <ContentTemplate>
                                            <asp:Panel ID="panelContentPage" runat="server" CssClass="col-lg-12 col-md-12 col-sm-12" Style="display: flex; margin-left: 0px; margin-right: 0px; padding-left: 0px; padding-right: 10px;">
                                                <userControl:PagerGridExt ID="PagerGridExt1" runat="server" PagedControlID="gvView" />
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                        </div>
                        <div class="card-block">

                            <asp:UpdatePanel ID="updateContentFilter" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <div class="row">
                                        <asp:Panel ID="panelContentFilter" runat="server" CssClass="col-lg-12 mb-3">
                                            <div>
                                                <asp:PlaceHolder ID="placeCustomSearch" runat="server" Visible="false"></asp:PlaceHolder>
                                                <asp:PlaceHolder ID="panelGridSearch" runat="server"></asp:PlaceHolder>
                                            </div>
                                            <div style="text-align: left; padding: 10px 0px 5px 0px;">
                                                <span>
                                                    <userControl:ButtonExt ID="btSearchConfirm" runat="server" Text="Search" CssClass="btn btn-sm btn-success" OnClick="btSearchConfirm_Click" />
                                                </span>
                                            </div>
                                        </asp:Panel>
                                        <asp:HiddenField ID="hidFieldHasFilter" runat="server" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="row col-lg-12">
                                <div style="width: 102%;">
                                    <asp:UpdatePanel ID="updateContentView" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                        style="width: 101%; overflow: auto; overflow-y: hidden;">
                                        <ContentTemplate>
                                            <asp:PlaceHolder ID="placeCustomColumns" runat="server"></asp:PlaceHolder>
                                            <asp:PlaceHolder ID="placeCustomColumnGroups" runat="server"></asp:PlaceHolder>

                                            <asp:HiddenField ID="hidGridCheckVals" runat="server" />
                                            <asp:HiddenField ID="hidGridDBVals" runat="server" />
                                            <asp:HiddenField ID="hidSelectRowCountByPage" runat="server" />

                                            <asp:HiddenField ID="hfGridView1SV" runat="server" />
                                            <asp:HiddenField ID="hfGridView1SH" runat="server" />

                                            <customControls:GridViewExt ID="gvView" runat="server" DataSourceID="objDataSource" HorizontalAlign="Center"
                                                OnPageIndexChanging="gvView_PageIndexChanging" OnRowCommand="gvView_RowCommand" ShowHeaderWhenEmpty="true"
                                                OnRowDataBound="gvView_RowDataBound" AllowPaging="true" AutoGenerateColumns="False"
                                                OnPreRender="gvView_PreRender">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="labHeader" runat="server" Width="30px">NO</asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hidKey" runat="server" Value='<%#Eval(KeyField) %>' />
                                                            <asp:Label ID="labNo" runat="server" CssClass="label-no" Width="30px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="ctrl-fix" />
                                                    </asp:TemplateField>
                                                   <%-- <asp:TemplateField Visible="false">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="KeyHeader" runat="server" Visible="false">NO</asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hidKey" runat="server" Value='<%#Eval(KeyField) %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField Visible="false">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="cmdSelectAll" runat="server" CausesValidation="False"></asp:CheckBox>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cmdSelect" runat="server" CausesValidation="False"></asp:CheckBox>
                                                        </ItemTemplate>
                                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="ctrl-fix" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="VIEW" Visible="false" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="cmdView" runat="server" CausesValidation="False"
                                                                Width="18px" Height="18px" ToolTip="Select" CommandName='SEL' CommandArgument='<%#Eval(KeyField) %>'>
                                                <i class="fa fa-share-square-o fa-lg"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="ctrl-fix" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="EDIT" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="cmdEdit" runat="server" CausesValidation="False"
                                                                Width="18px" Height="18px" ToolTip="Edit" CommandName='EDI' CommandArgument='<%#Eval(KeyField) %>'>
                                        <i class="fa fa-sliders fa-lg"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="ctrl-fix" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="DELETE" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="cmdDelete" runat="server" CausesValidation="False"
                                                                Width="18px" Height="18px" ToolTip="Delete" CommandName='DEL' CommandArgument='<%#Eval(KeyField) %>'
                                                                OnClientClick="if (!confirm('Are you sure ?')) return false;">
                                        <i class="fa fa-remove fa-lg"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="ctrl-fix" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </customControls:GridViewExt>

                                            <asp:ObjectDataSource ID="objDataSource" EnablePaging="True" runat="server" SelectMethod="GetViewData" SelectCountMethod="GetCountData"
                                                MaximumRowsParameterName="iMaximumRows" StartRowIndexParameterName="iBeginRowIndex"
                                                SortParameterName="iSortField" OnSelecting="objDataSource_Selecting" OnSelected="objDataSource_Selected">
                                                <SelectParameters>
                                                    <asp:Parameter Name="_filterGrid" Type="Object" ConvertEmptyStringToNull="true" />
                                                    <asp:Parameter Name="_filterCustom" Type="Object" ConvertEmptyStringToNull="true" />
                                                    <asp:Parameter Name="_sortDefault" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <%--<asp:UpdatePanel ID="updateOptionField" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btOptField" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="row">
                                        <asp:Panel runat="server" ID="panelOptField" Visible="false" CssClass="col-lg-12">
                                            <asp:GridView ID="gridOptField" runat="server" AutoGenerateColumns="false" CssClass="grid-option-field">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="6px">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkOptSelect" runat="server" Checked='<%# Bind("IsSelect") %>' />
                                                            <asp:HiddenField ID="hidOptIndex" runat="server" Value='<%# Bind("Index") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Columns">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labOptField" runat="server" Text='<%# Bind("ColumnName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                            <%--<asp:TemplateField HeaderText="SORT" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtOptSort" runat="server" Text='<%# Bind("IndexNew") %>' MaxLength="2"
                                                                onkeypress="return onkeyInteger(event);"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                            <%--        </Columns>
                                            </asp:GridView>
                                            <div>
                                                <asp:Button ID="btOptSave" runat="server" CausesValidation="false"
                                                    Text="OK" CssClass="btn btn-success btn-ingrid" Style="width: 100%;" OnClick="btOptSave_Click" />
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

