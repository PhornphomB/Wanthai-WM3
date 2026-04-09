<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilterStip.ascx.cs" Inherits="_UControls.FilterStip" %>

<asp:UpdatePanel ID="updateFilter" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
        <asp:Panel ID="divFilterContent" runat="server" CssClass="" Visible="false"> <%--CssClass="content_filter"--%>

            <ul class="nav navbar-nav ml-auto">
            <li class="nav-item dropdown">
                <asp:LinkButton ID="btActive" runat="server" CausesValidation="false" CssClass="dropdown-header text-center dropdown-filter" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false"></asp:LinkButton>
                <asp:Panel runat="server" ID="linkFilterOpt" CssClass="dropdown-menu dropdown-menu-right">
                    <asp:LinkButton ID="btNone" runat="server" CausesValidation="false" TabIndex="-1" CssClass="dropdown-item"></asp:LinkButton>
                    <asp:LinkButton ID="btEq" runat="server" CausesValidation="false" TabIndex="-1" CssClass="dropdown-item"></asp:LinkButton>
                    <asp:LinkButton ID="btNotEq" runat="server" CausesValidation="false" TabIndex="-1" CssClass="dropdown-item"></asp:LinkButton>
                    <asp:LinkButton ID="btThMore" runat="server" CausesValidation="false" TabIndex="-1" CssClass="dropdown-item"></asp:LinkButton>
                    <asp:LinkButton ID="btLeMore" runat="server" CausesValidation="false" TabIndex="-1" CssClass="dropdown-item"></asp:LinkButton>
                    <asp:LinkButton ID="btThMoreEq" runat="server" CausesValidation="false" TabIndex="-1" CssClass="dropdown-item"></asp:LinkButton>
                    <asp:LinkButton ID="btLeMoreEq" runat="server" CausesValidation="false" TabIndex="-1" CssClass="dropdown-item"></asp:LinkButton>
                    <asp:LinkButton ID="btContain" runat="server" CausesValidation="false" TabIndex="-1" CssClass="dropdown-item"></asp:LinkButton>
                    <asp:LinkButton ID="btBetween" runat="server" CausesValidation="false" TabIndex="-1" CssClass="dropdown-item"></asp:LinkButton>
                </asp:Panel>
            </li>
            </ul>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
