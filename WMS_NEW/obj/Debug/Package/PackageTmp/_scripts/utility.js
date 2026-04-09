
function doPostBackAsync(eventName, eventArgs) {

    var prm = Sys.WebForms.PageRequestManager.getInstance();

    if (!Array.contains(prm._asyncPostBackControlIDs, eventName)) {
        prm._asyncPostBackControlIDs.push(eventName);
    }

    if (!Array.contains(prm._asyncPostBackControlClientIDs, eventName)) {
        prm._asyncPostBackControlClientIDs.push(eventName);
    }

    __doPostBack(eventName, eventArgs);
}

function gridFixHeader(gridId, hidVerId, hidHorId) {
    var gridViewScroll = new GridViewScroll({
        elementID: gridId,
        width: '100%',
        height: '50%',
        onscroll: function (scrollTop, scrollLeft) {
            $('#' + hidVerId).val(scrollTop);
            $('#' + hidHorId).val(scrollLeft);
        }
    });

    gridViewScroll.enhance();
    gridViewScroll.scrollPosition = { scrollTop: $('#' + hidVerId).val(), scrollLeft: $('#' + hidHorId).val() }; // set scroll position
}

function ReceiveCustomCallBackData(arg, context) {

    var elm = $('#' + context);
    var spliter = '$$';
    var _arg = arg.split(spliter);
    var excutablescript = _arg[1];
    var resultforshow = _arg[0];


    if (elm && resultforshow.length != 0) {
        elm.val(resultforshow);
        elm.change();
    }
    if (excutablescript.length != 0) eval(excutablescript);

}

function bindCuteTip(id) {
    $('#' + id).cluetip({
        cluetipClass: 'jtip',
        sticky: true,
        activation: 'click',
        arrows: true,
        width: '400px',
        height: '300px',
        closePosition: 'title',
        closeText: '<input  type="button" value="x" class="bt_search" />'
    });
}

function gridSelectedRows(element) {
    $(element).find("input:checkbox").click(function () {
        if ($(this).is(":checked") == true) {
            $(this).parent().parent().addClass("highlight");
        } else {
            $(this).parent().parent().removeClass("highlight");
        }
    });
}

function equalHeight(group) {
    tallest = 0;
    group.each(function () {
        thisHeight = $(this).height();
        if (thisHeight > tallest) {
            tallest = thisHeight;
        }
    });
    group.height(tallest);
}

function dateselect(ev) {
    var calendarBehavior1 = $find("Calendar1");
    var d = calendarBehavior1._selectedDate;
    var now = new Date();
    calendarBehavior1.get_element().value = d.format("MM/dd/yyyy") + " " + now.format("HH:mm:ss")
}

function onkeyNotAllowKey(e) {
    var key;
    if (window.event) {
        key = window.event.keyCode; // IE
        window.event.returnValue = false;
    }
    else {
        key = e.which; // Firefox
        keyPressed = e.preventDefault();
    }

}

function pad(number, length) {

    var str = '' + number;
    while (str.length < length) {
        str = '0' + str;
    }

    return str;
}

function trimZero(s) {
    while (s.substr(0, 1) == '0' && s.length > 1) {
        s = s.substr(1, 9999);
    }

    return s;
}


function defaultTimeUnit(elm, value, length) {

    var number = trimZero($.trim(value));

    if (number == "") {
        $("#" + elm).val(pad(0, length));
    }
    else if (parseInt(value) > 59) {
        $("#" + elm).val(pad(59, length));
    }
    else {
        $("#" + elm).val(pad(parseInt(number), length));
    }
}

function defaultInteger(elm, value) {

    var number = trimZero($.trim(value));

    if (number == "") {
        $("#" + elm).val(0);
    }
    else {
        $("#" + elm).val(parseInt(number));
    }
}

function defaultNumber(elm, value, degit) {

    var cutdot = ($.trim(value)).replace(".", "");
    var number = trimZero(cutdot);

    if ((number == "") || (number == "0")) {
        $("#" + elm).val((0).toFixed(degit));
    }
    else {
        $("#" + elm).val(parseFloat(trimZero($.trim(value))).toFixed(degit));
    }
}

function onkeyInteger(e) {
    var key;

    if (window.event) {
        key = window.event.keyCode; // IE 
        if ((key < 48 || key > 59) && (key != 8)) window.event.returnValue = false;
    }
    else {
        key = e.which; // Firefox        
        if ((key < 48 || key > 59) && (key != 8)) keyPressed = e.preventDefault();
    }
}


function onkeyNumber(e) {
    var key;

    if (window.event) {
        key = window.event.keyCode; // IE 
        if ((key < 48 || key > 59) && (key != 8) && (key != 46)) window.event.returnValue = false;
    }
    else {
        key = e.which; // Firefox        
        if ((key < 48 || key > 59) && (key != 8) && (key != 46)) keyPressed = e.preventDefault();
    }
}

function onkeyEng(e) {
    var key;

    if (window.event) {
        key = window.event.keyCode; // IE
        if ((key >= 3585 && key <= 3661) && (key != 8)) window.event.returnValue = false;
    }
    else {
        key = e.which; // Firefox
        if ((key >= 3585 && key <= 3661) && (key != 8)) keyPressed = e.preventDefault();
    }

}

function OpenBox(eid) {
    $(eid).fadeIn(500);
}

function CloseBox(eid) {
    $(eid).fadeOut(500);
}

function Back(prm) {
    window.location = prm;
}

function Page(url) {
    window.location = url;
}

function Popup(prm) {
    var width = 1024;
    var height = 500;
    var left = (screen.width / 2) - (width / 2);
    var top = (screen.height / 3) - (height / 3);

    var dimensions = "resizable=yes, status=no, toolbar=no, scrollbars=yes, location=no, menubar=no,addressbar=no,width=" + width + ",height=" + height + ",top=" + top + ",left=" + left;
    window.open(prm, '_blank', dimensions);
}

function checkImageName(getid) {
    arr = getid.split("\\");
    i = arr.length - 1;
    image = arr[i].split(".");
    check = false;
    if ((image[1] == "gif") || (image[1] == "jpg") || (image[1] == "jpeg") || (image[1] == "GIF") || (image[1] == "JPG") || (image[1] == "JPEG")) {
        check = true;
    }
    return check;
}

function sleep(milliseconds) {
    setTimeout(function () {
        var start = new Date().getTime();
        while ((new Date().getTime() - start) < milliseconds) {
            // Do nothing
        }
    }, 0);
}


