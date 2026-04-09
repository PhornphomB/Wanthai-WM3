
function gridSetStyleHeaderFilter(gridRef, hidFieldHasFilter) {
    // cast parameter from server
    var listFilters = jQuery.parseJSON($('#' + hidFieldHasFilter).val());

    $(listFilters).each(function (i, ent) {

        var grid = $("body table[id^='" + gridRef + "']").first();
        //alert($(grid).attr("id"));

        var linkFilter = $(grid).find('tr th span[name="link_filter_name_' + ent.DataField + '"]');
        linkFilter.empty();
        linkFilter.attr("title", "Filter");
        linkFilter.attr("data-placement", "bottom");
        linkFilter.attr("onclick", "__doPostBack('" + gridRef + "','FILTER|" + ent.DataField + "');");

        if (ent.Active == true) {
            linkFilter.attr("class", "grid-filter-active");
            linkFilter.append("<i class='fa fa-filter'></i>");
            linkFilter.attr("onmousedown", "if(event.button == 2){__doPostBack('" + gridRef + "','FILTER|" + ent.DataField + "'); }");
        }
        else {
            linkFilter.attr("class", "grid-filter");
            linkFilter.append("<i class='fa fa-filter'></i>");
        }

    });
}