<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagerGridViewExt.ascx.cs" Inherits="_UControls.PagerGridViewExt" %>

<asp:Panel ID="Panel1" runat="server" Style="margin-top: 0px;" HorizontalAlign="NotSet">
    <customControls:DataPagerExt runat="server" ID="DataPagerExt1" class="pagination float-right">
        <Fields>
            <asp:NextPreviousPagerField ShowPreviousPageButton="true" ShowNextPageButton="false" ShowFirstPageButton="true" FirstPageText="&#10072;&#10094;" LastPageText="&#10095;&#10072;" NextPageText="&#10095;" PreviousPageText="&#10094;"
                RenderDisabledButtonsAsLabels="true" RenderNonBreakingSpacesBetweenControls="false" />
            <asp:NextPreviousPagerField ShowPreviousPageButton="false" ShowNextPageButton="true" ShowLastPageButton="true" FirstPageText="&#10072;&#10094;" LastPageText="&#10095;&#10072;" NextPageText="&#10095;" PreviousPageText="&#10094;"
                RenderDisabledButtonsAsLabels="true" RenderNonBreakingSpacesBetweenControls="false" />
        </Fields>
    </customControls:DataPagerExt>
    <asp:LinkButton ID="btGridRefresh" runat="server" CssClass="float-right btn-refresh" Text="&#x21bb;" ToolTip="Refresh Data" CausesValidation="false" OnClick="btGridRefresh_Click"></asp:LinkButton>
    <customControls:DataPagerExt runat="server" ID="DataPagerExt2" class="pagination float-right">
        <Fields>
            <asp:TemplatePagerField>
                <PagerTemplate>
                    <div class="pager-detail">
                        <%# Container.TotalRowCount > 0 ? (Container.StartRowIndex + 1) : 0 %> to <%# (Container.StartRowIndex + Container.PageSize) <= Container.TotalRowCount ? (Container.StartRowIndex + Container.PageSize) : Container.TotalRowCount %>
                         of <%# Container.TotalRowCount%> rows 
                    </div>
                </PagerTemplate>
            </asp:TemplatePagerField>
        </Fields>
    </customControls:DataPagerExt>
</asp:Panel>
