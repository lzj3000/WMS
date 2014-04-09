/**
* jQuery EasyUI 1.2.5
* 
* Licensed under the GPL terms
* To use it on other terms please contact us
*
* Copyright(c) 2009-2011 stworthy [ stworthy@gmail.com ] 
* 
*/
(function ($) {
    function _1(_2) {
        var _3 = $("<span class=\"spinner\">" + "<span class=\"spinner-arrow\">"+ "</span>" + "</span>").insertAfter(_2);
        $(_2).addClass("spinner-text").prependTo(_3);
        return _3;
    };
    function _4(_5, _6) {
        var _7 = $.data(_5, "spinnerbox").options;
        var _8 = $.data(_5, "spinnerbox").spinner;
        if (_6) {
            _7.width = _6;
        }
        var _9 = $("<div style=\"display:none\"></div>").insertBefore(_8);
        _8.appendTo("body");
        if (isNaN(_7.width)) {
            _7.width = $(_5).outerWidth();
        }
        var _a = _8.find(".spinner-arrow").outerWidth();
        var _6 = _7.width - _a;
        if ($.boxModel == true) {
            _6 -= _8.outerWidth() - _8.width();
        }
        $(_5).width(_6);
        _8.insertAfter(_9);
        _9.remove();
    };
    function _b(_c) {
        var _d = $.data(_c, "spinnerbox").options;
        var _e = $.data(_c, "spinnerbox").spinner;
    };
    function _f(_10, _11) {
        var _12 = $.data(_10, "spinnerbox").options;
        if (_11) {
            _12.disabled = true;
            $(_10).attr("disabled", true);
        } else {
            _12.disabled = false;
            $(_10).removeAttr("disabled");
        }
    };
    $.fn.spinnerbox = function (_13, _14) {
        if (typeof _13 == "string") {
            var _15 = $.fn.spinnerbox.methods[_13];
            if (_15) {
                return _15(this, _14);
            } else {
                return this.validatebox(_13, _14);
            }
        }
        _13 = _13 || {};
        return this.each(function () {
            var _16 = $.data(this, "spinnerbox");
            if (_16) {
                $.extend(_16.options, _13);
            } else {
                _16 = $.data(this, "spinnerbox", { options: $.extend({}, $.fn.spinnerbox.defaults, $.fn.spinnerbox.parseOptions(this), _13), spinner: _1(this) });
                $(this).removeAttr("disabled");
            }
            $(this).val(_16.options.value);
            $(this).attr("readonly", !_16.options.editable);
            _f(this, _16.options.disabled);
            _4(this);
            $(this).validatebox(_16.options);
            _b(this);
        });
    };
    $.fn.spinnerbox.methods = { options: function (jq) {
        var _17 = $.data(jq[0], "spinnerbox").options;
        return $.extend(_17, { value: jq.val() });
    }, destroy: function (jq) {
        return jq.each(function () {
            var _18 = $.data(this, "spinnerbox").spinner;
            $(this).validatebox("destroy");
            _18.remove();
        });
    }, resize: function (jq, _19) {
        return jq.each(function () {
            _4(this, _19);
        });
    }, enable: function (jq) {
        return jq.each(function () {
            _f(this, false);
            _b(this);
        });
    }, disable: function (jq) {
        return jq.each(function () {
            _f(this, true);
            _b(this);
        });
    }, getValue: function (jq) {
        return jq.val();
    }, setValue: function (jq, _1a) {
        return jq.each(function () {
            var _1b = $.data(this, "spinnerbox").options;
            _1b.value = _1a;
            $(this).val(_1a);
        });
    }, clear: function (jq) {
        return jq.each(function () {
            var _1c = $.data(this, "spinnerbox").options;
            _1c.value = "";
            $(this).val("");
        });
    } 
    };
$.fn.spinnerbox.parseOptions = function (_1d) {
        var t = $(_1d);
        return $.extend({}, $.fn.spinner.parseOptions(_1d), { width: (parseInt(_1d.style.width) || undefined), value: (t.val() || undefined), min: t.attr("min"), max: t.attr("max"), increment: (parseFloat(t.attr("increment")) || undefined), editable: (t.attr("editable") ? t.attr("editable") == "true" : undefined), disabled: (t.attr("disabled") ? true : undefined) });
    };
    $.fn.spinnerbox.defaults = $.extend({}, $.fn.spinner.defaults, { width: "auto", value: "", min: null, max: null, increment: 1, editable: true, disabled: false, spin: function (_1e) {
    }, onSpinUp: function () {
    }, onSpinDown: function () {
    } 
    });
})(jQuery);

