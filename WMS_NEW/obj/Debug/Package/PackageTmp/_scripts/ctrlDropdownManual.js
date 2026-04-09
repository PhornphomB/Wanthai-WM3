function dropDownValueChanged(elmId, hidMultiVal, triggertId, isPostValueChanged) {

    var values = [];
    var value = $('#' + elmId).val();
    $('#' + hidMultiVal).val('');

    if (value != null) {
        if (isArray(value)) {
            values = value;
            $('#' + hidMultiVal).val(values);
        }
        else {
            try {
                values = value.split(',');
            }
            catch (err) { }
        }
    }

    if (isPostValueChanged) {
        $(triggertId).click();
    }
}

function contains(arr, obj) {
    var i = arr.length;
    while (i--) {
        if (arr[i] === obj) {
            return true;
        }
    }
    return false;
}
function isArray(object) {
    if (object.constructor === Array) return true;
    else return false;
}
