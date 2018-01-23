//封装的一个JQuery小插件 
//$(“#table1″).mergecell(0);//传入的参数是对应的列数从0开始，哪一列有相同的内容就输入对应的列数值
jQuery.fn.mergecell = function (colIdx) {
    return this.each(function () {
        var that;
        $('tr', this).each(function (row) {
            $('td:eq(' + colIdx + ')', this).filter(':visible').each(function (col) {
                if (that != null && $(this).html() == $(that).html()) {
                    rowspan = $(that).attr("rowSpan");
                    if (rowspan == undefined) {
                        $(that).attr("rowSpan", 1);
                        rowspan = $(that).attr("rowSpan");
                    }
                    rowspan = Number(rowspan) + 1;
                    $(that).attr("rowSpan", rowspan);
                    $(this).hide();
                } else {
                    that = this;
                }
            });
        });
    });
}

function init_chk_event() {
    $("input:visible:checkbox").change(function () {
        var id = $(this).attr("id");
        //设置子节点
        $("input:visible:checkbox[id^='" + id + "']").attr("checked", $(this).is(':checked'));
        //设置父节点
        fn_unchk($(this));
    });
}

function fn_unchk(el) {
    var id = $(el).attr("id");
    var lastIndex = id.lastIndexOf("-");
    if (lastIndex >= 0) {
        var cur_id = id.substring(0, lastIndex);
        var cur = $("input:visible:checkbox[id='" + cur_id + "']");
        if ($(el).is(':checked')) {
            var unChk = $("input:visible:checkbox[id^='" + cur_id + "']:not(:checked)");
            if (unChk.length == 1 && unChk.first().attr("id") == cur_id && !unChk.first().is(':checked')) {
                unChk.first().attr("checked", true);
            }
        } else {
            cur.attr("checked", false);
        }
        fn_unchk(cur);
    } else {
        return;
    }
}