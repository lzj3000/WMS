$.extend($.fn.searchbox.methods, {
    enable: function (jq) {
        return jq.each(function () {
            this.disabled = false;
            $(this).searchbox("textbox").removeAttr("disabled");
        });
    }, disable: function (jq) {
        return jq.each(function () {
            this.disabled = true;
            $(this).searchbox("textbox").attr("disabled", true)
        });
    }
});