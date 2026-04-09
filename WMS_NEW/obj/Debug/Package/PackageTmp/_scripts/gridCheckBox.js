function checkboxClick(objRef, hidGridCheckVals, hidGridDBVals, hidSelectRowCountByPage) {

    var table_id = "#" + $(objRef).closest('table').attr('id');  // cast table in checkbox

    checkboxChange(objRef, hidGridCheckVals, hidGridDBVals, hidSelectRowCountByPage);
    checkSelectAllRow(table_id, hidSelectRowCountByPage);
}

function checkboxAllClick(objRef, chkId, hidGridCheckVals, hidGridDBVals, hidSelectRowCountByPage) {

    var table = $(chkId).closest('table');  // cast table in checkbox
    var table_id = "#" + table.attr('id');

    $(table_id + " tr td:nth-child(2) input:checkbox:enabled").each(function (i) {

        if ($(this).prop('checked') != objRef.checked) {

            $(this).prop('checked', objRef.checked);

            var dom_obj = $(this).get(0); // cast jquery obj to dom obj
            checkboxChange(dom_obj, hidGridCheckVals, hidGridDBVals, hidSelectRowCountByPage);
        }
    });
}

function checkSelectAllRow(tableId, hidSelectRowCountByPage) {

    var chkAll = $(tableId + "Copy").find("input:checkbox");
    if (chkAll.length == 0) // if table default not using script fix header
    {
        chkAll = $(tableId + " tr th:nth-child(2) input:checkbox");
    }

    var rowSelectCount = parseInt($(hidSelectRowCountByPage).val());
    if (rowSelectCount == 0) {

        if (chkAll.prop('checked') == true) {
            chkAll.prop('checked', false);
        }
        return;
    }

    var rowCount = $(tableId + " tr td:nth-child(2) input:checkbox:enabled").size();

    if (rowSelectCount == rowCount) {
        chkAll.prop('checked', true);
    }
    else {
        if (chkAll.prop('checked') == true) {
            chkAll.prop('checked', false);
        }
    }
}

function setTableStyleColor(table_id) {

    var cssTrChecked = 'highlight';

    $(table_id + " tr td:nth-child(2) input:checkbox").each(function (i) {

        var row = $(this).closest('tr'); // cast tr in checkbox

        if ($(this).prop('checked') == true) {
            row.addClass(cssTrChecked);
        }
        else {
            row.removeClass(cssTrChecked);
        }

    });
}

function checkboxChange(objRef, hidGridCheckVals, hidGridDBVals, hidSelectRowCountByPage) {

    var cssTrChecked = 'highlight';

    // cast parameter from server
    var listKey = jQuery.parseJSON($(hidGridCheckVals).val());
    var listKeyDB = jQuery.parseJSON($(hidGridDBVals).val());
    var selectRowCountByPage = $(hidSelectRowCountByPage);

    var selectCount = parseInt(selectRowCountByPage.val());

    var row = $(objRef).closest('tr'); // cast tr in checkbox
    var keyId = row.children('td:nth-child(1)').find('input:hidden').val(); // get key_id in hidden field

    var hasKey = null;
    for (var i = 0; i < listKey.length; i++) {
        if (listKey[i].KeyId == keyId) {
            hasKey = listKey[i];
            break;
        }
    }

    var hasKeyDB = null;
    $(listKeyDB).each(function (i, val) {
        if (val.KeyId == keyId) {
            hasKeyDB = val;
            return false;
        }
    });

    if (objRef.checked) {

        // ถ้า Key นั้นยังมีอยู่ใน DB
        if (hasKeyDB != null) {

            //Checked
            $(listKey).each(function (i, val) {
                if (val.KeyId == hasKey.KeyId) // delete index
                {
                    listKey.splice(i, 1); //remove item object at index
                    return false;
                }
            });

        }
            //เคสปกติ
        else {

            if (hasKey == null) {
                listKey.push({
                    KeyId: keyId,
                    Active: true
                });
            }
        }


        selectCount++;
        row.addClass(cssTrChecked);
    }
    else {

        // ถ้า Key นั้นยังมีอยู่ใน DB
        if (hasKeyDB != null) {

            //Unchecked
            if (hasKey == null) {
                listKey.push({
                    KeyId: keyId,
                    Active: false
                });
            }

        }
            //เคสปกติ
        else {
            $(listKey).each(function (i, val) {
                if (val.KeyId == hasKey.KeyId) // delete index
                {
                    listKey.splice(i, 1); //remove item object at index
                    return false;
                }
            });
        }

        selectCount--;
        row.removeClass(cssTrChecked);
    }

    var json_text = JSON.stringify(listKey, null, 2);

    $(hidGridCheckVals).val(json_text);
    selectRowCountByPage.val(selectCount);
}