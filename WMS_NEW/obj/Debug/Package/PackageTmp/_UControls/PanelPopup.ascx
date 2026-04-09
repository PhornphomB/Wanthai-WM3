<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PanelPopup.ascx.cs" Inherits="_UControls.PanelPopup" %>
<%@ Register Src="~/_UControls/ButtonExt.ascx" TagName="ButtonExt" TagPrefix="userControl" %>

<asp:UpdatePanel ID="updateFrom" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
    <ContentTemplate>
        <asp:Panel ID="viewContent" runat="server" CssClass="modal-bg"></asp:Panel>
        <!-- /.modal -->
        <asp:Panel ID="myModal" runat="server" CssClass="modal" TabIndex="-1" role="dialog" aria-hidden="true">
            <!-- /.modal-dialog -->
            <asp:Panel ID="container" runat="server" CssClass="modal-dialog" role="document">
                <!-- /.modal-content -->
                <asp:Panel ID="panelContent" runat="server" CssClass="modal-content">
                    <div id="contentHead" runat="server" class="modal-header">
                        <h5 runat="server" class="modal-title">
                            <asp:UpdatePanel ID="updateTitle" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <asp:Label ID="labTitle" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </h5>
                        <div class="float-right">
                            <button runat="server" type="button" id="btClose" class="close text-white" data-dismiss="modal" aria-label="Close" onserverclick="btClose_Click" causesvalidation="false">
                                <i class="fa fa-close"></i>
                            </button>
                            <button runat="server" type="button" id="btMini" class="close text-white" data-dismiss="modal" aria-label="Minimize" onserverclick="btResize_Click" causesvalidation="false">
                                <i class="fa fa-compress mr-2"></i>
                            </button>
                        </div>
                    </div>
                    <div id="contentData" runat="server" class="modal-body">
                        <asp:UpdatePanel ID="updateContent" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <asp:PlaceHolder ID="PlaceHolderTemp" runat="server"></asp:PlaceHolder>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="contentCommand" runat="server" class="modal-footer">
                        <div class="pull-right">
                            <asp:UpdatePanel ID="updateCommand" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <asp:PlaceHolder ID="PlaceHolderCommand" runat="server"></asp:PlaceHolder>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="pull-right">
                            <userControl:ButtonExt runat="server" ID="btCloseCommand" Text="Close" data-dismiss="modal" CssClass="btn btn-secondary"   CausesValidation="false" OnClick="btCloseCommand_Click" ResourceGroup="General" ResourceName="Close" />
<%--                            <ucControls:ButtonExt  />--%>
                        </div>
                    </div>
                </asp:Panel>
                <!-- /.modal-content -->
            </asp:Panel>
            <!-- /.modal-dialog -->
        </asp:Panel>
        <!-- /.modal -->

        <asp:Panel ID="panelTemp" runat="server" CssClass="modal-temp" Visible="false">
            <span class="float-left">
                <asp:Label ID="labTempTitle" runat="server">Window Popup</asp:Label>
            </span>
            <div class="float-right">
                <button runat="server" type="button" id="btMaxi" class="close text-white" data-dismiss="modal" aria-label="Maximize" onserverclick="btResize_Click" causesvalidation="false">
                    <i class="fa fa-expand"></i>
                </button>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
