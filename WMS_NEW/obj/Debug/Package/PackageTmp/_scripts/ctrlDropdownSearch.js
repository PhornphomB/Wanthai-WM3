

function bindDropDownSearch(elmId, txtId, displayId, panelDataId, triggertId, isPostValueChanged, servWidth,
    servEnable, servCssClass, isMultiple, funcAysn, isPrimary) {

    var timerQuery;
    var timerDelay = 500;
    var width = 220;

    if (servWidth > 0) {
        width = servWidth;
    }

    var placeText = '';
    if (isMultiple) {
        placeText = "select tag data...";
    }

    $(elmId).select2({
        width: width,
        placeholder: placeText,
        multiple: isMultiple,
        closeOnSelect: !isMultiple,
        dropdownAutoWidth: false,
        dropdownCssOverrideClass: servCssClass,
        initSelection: function (element, callback) {

            if (isMultiple == true) {

                var _arg = $(displayId).val();
                var dataArr = jQuery.parseJSON(_arg);

                var data = [];
                $(dataArr).each(function (i, key) {
                    data.push({ id: key.id, text: key.text });
                });

                callback(data);
            }
            else {
                var display = "";
                if ($(displayId).val()) {
                    display = $(displayId).val();
                }

                var data = { id: element.val(), text: display };
                callback(data);
            }
        },
        query: function (query) {

            var data = { results: [] };

            var keywordHas = "0";
            var keysecondHas = "0";

            var keyword = query.term.trim();
            var keysecond = "";

          //  $(txtId).val(e.val);
         //   $("input:text").val(query.term.trim());

            if (keyword != "") {
                keywordHas = "1";
            }

            if ($(elmId).val() != "") {
                keysecond = $(elmId).val();
                keysecondHas = "1";
            }

            //var _arg = $(panelDataId).val() == "" ? "[]" : $(panelDataId).val();
            //var jsonTake = jQuery.parseJSON(_arg);

            //if (jsonTake.length > 1 && keyword == "" && isMultiple == false) {
            //    for (var i = 0; i < jsonTake.length; i++) {
            //        data.results.push(jsonTake[i]);
            //    }

            //    query.callback(data);
            //}
            //else {

                var timeSearch = timerDelay;
                if (keyword == "")
                    timeSearch = 0;

                clearTimeout(timerQuery);

                timerQuery = setTimeout(function () {

                    //Async callback function
                    funcAysn(keywordHas + "$$" + keysecondHas + "$$" + keyword + "$$" + keysecond, elmId.replace("#", ""), panelDataId.replace("#", ""));

                    //Binding event change for triger set data after Async callback set value at Panel
                    $(panelDataId).one("change", function () {

                        var _arg = $(this).val();

                        var jsonTake = jQuery.parseJSON(_arg);
                        for (var i = 0; i < jsonTake.length; i++) {
                            data.results.push(jsonTake[i]);
                        }

                        query.callback(data);
                    });

                }, timeSearch);
            //}

        }
    });

    if (servEnable == false) {
        $(elmId).select2("enable", false);
    }

    $(elmId)
        .on("select2-removing", function (e) {
            if (isMultiple == true) {

                var datas = $(this).select2("data");

                $(datas).each(function (i, key) {
                    if (key.id == e.val) {
                        datas.splice(i, 1); //remove item object at index
                        return false;
                    }
                });

                $(this).select2("data", datas);
            }
        })
        .on("change", function (e) {

            var data = $(this).select2("data");

            $(this).val(e.val);
            $(txtId).val(e.val);

            if (isMultiple == true) {
                var json_text = JSON.stringify(data, null, 2);
                $(displayId).val(json_text);
            }
            else {
                $(displayId).val(data.text);
            }

            //modify validate
            if (isPrimary && data.id == "") {
                var id = $(txtId).closest('div').find('label');
                $(id).css("color", "red");

                var ddl = $(txtId).closest('div').find('.select2-container');
                $(ddl).css({ "border": "1px solid red", "box-shadow": "0 0 0 0.1rem #F5B7B1" });

            } else {
                var id = $(txtId).closest('div').find('label');
                $(id).css("color", "black");

                var ddl = $(txtId).closest('div').find('.select2-container');
                $(ddl).css({ "border": "1px solid lightgray", "box-shadow": "0 0 0 0 lightgray" });

            }


            if (isPostValueChanged == true) {
                $(triggertId).click();
            }
        });

}