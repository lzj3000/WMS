$.extend($.fn.datagrid.defaults.editors, {
    text: {
        init: function (container, options) {
            options.width = container.context.clientWidth-2;
            options.init();
            var input = $(options.input).appendTo(container);
            options.ready();
            options.onevent();
            return options;
        },
        getValue: function (target, row) {
            if (target.foreign != null && target.foreign.isfkey) {
                row[target.foreign.displayname] = target.foreign.displayvalue;
                target.foreign.displayvalue = "";
            }
            return target.val();
        },
        setValue: function (target, value, row) {
            if (target.foreign != null && target.foreign.isfkey) {
                if (row[target.foreign.displayname]) {
                    target.val(row[target.foreign.displayname]);
                    target.foreign.filedvalue = value;
                    target.foreign.displayvalue = row[target.foreign.displayname];
                }
            }
            else
                target.val(value);
        },
        resize: function (target, width) {
            var input = target;
            if ($.boxModel == true) {
                input.setwidth(width - (input.outerWidth() - input.width()));
            } else {
                input.setwidth(width);
            }
        }
    }
});  