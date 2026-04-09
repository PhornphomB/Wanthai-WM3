<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadFileEx.ascx.cs" Inherits="_UControls.UploadFileEx" %>


<form class="md-card">
    <div class="row">
        <div class="col-md-12">
            <asp:Panel ID="panelUploader" runat="server">
                <%--<p>Your browser doesn't have HTML4 or HTML5 support.</p>--%>
            </asp:Panel>

            <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btRefreshGrid" runat="server" Text="Refresh" OnClick="btRefreshGrid_Click" Style="display: none;" />
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        <div class="col-md-12">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card">
                        <div class="card-header">Detail</div>
                        <div class="card-body">
                            <asp:Panel runat="server" ID="panelGallery" Style="padding: 10px 0px">
                                <div class="wrapper-image">
                                    <ul class="auto-grid">
                                        <asp:ListView runat="server" ID="listGallery" DataKeyNames="Name" OnItemDeleting="listGallery_ItemDeleting">
                                            <ItemTemplate>
                                                <li class="li-image">
                                                    <img class="image-list" src="<%#Eval("FileUrl") %>" <% if (FileTypes == _UControls.FileType.File){ %>
                                                        style="width: 64px !important" <%} %> />
                                                    <div class="gallery_grid_image_caption">
                                                        <div class="pl-3 pt-2 pb-2 pr-3">
                                                            <%--<div class="mb-0 image-name"><%#Eval("Name") %></div>--%>
                                                            <div class="mb-0 image-name">
                                                                <a href="<%#Eval("LinkUrl") %>"><%#Eval("Name") %></a>
                                                            </div>
                                                            <div class="image-size"><%#Eval("Size") %></div>
                                                        </div>
                                                        <div class="chart-wrapper px-3 pb-2">
                                                            <div class="bars pb-2">
                                                                <div class="progress progress-xs" style="height: 3px;">
                                                                    <div id="ctl00_ContentPlaceHolder1_progInb" class="progress-bar bg-black" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
                                                                </div>
                                                            </div>
                                                            <asp:LinkButton ID="btDel" runat="server" Text="delete" CssClass="link-del" OnClientClick="return confirm('ยืนยันการลบภาพนี้ ?');" CommandName="Delete" />
                                                        </div>
                                                    </div>
                                                </li>

                                            </ItemTemplate>
                                        </asp:ListView>
                                    </ul>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</form>

