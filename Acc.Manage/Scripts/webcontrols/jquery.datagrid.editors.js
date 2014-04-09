$.extend($.fn.datagrid.defaults.editors, {
    text: {
        init: function (container, options) {
            //var w = $(container).width();
            options.width = container.context.clientWidth - 2;
            options.init();
            var input = $(options.input).appendTo(container);
            options.ready();
            options.onevent();
            return options;
        },
        getValue: function (target, row) {
            target.oldrow = row;
            if (target.foreign != null && target.foreign.isfkey) {
                if (target && target.foreign && target.foreign.displayname != "")
                    row[target.foreign.displayname] = target.foreign.displayvalue;
                target.foreign.displayvalue = "";
            }
            var v = target.val();
            return v;
            //return row[target.filed];
        },
        setValue: function (target, value, row) {
            target.oldrow = row;
            if (target.foreign != null && target.foreign.isfkey) {
                //                if (row[target.foreign.displayname]) {
                //                    target.val(row[target.foreign.displayname]);
                //                    target.foreign.filedvalue = value;
                //                    target.foreign.displayvalue = row[target.foreign.displayname];
                //                }
                target.fval(row);
            }
            else
                target.val(value);
        },
        resize: function (target, width) {
            var input = target;
            //            if ($.boxModel == true) {
            //                input.setwidth(width - (input.outerWidth() - input.width()));
            //            } else {
            input.setwidth(width);
            //            }
        }
    }
});  