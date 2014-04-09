var codescan = function () {
    function cshow(s) {
        var div = $("#codescan");
        if (div[0] == null) {
            div = $("<div id='codescan' style='overflow:scroll;overflow-x:hidden;overflow-y:hidden'/>").appendTo($("body"));
            $(div).window({
                title: "扫描条码",
                collapsible: true,
                minimizable: false,
                maximizable: false,
                resizable: false,
                modal: false,
                width: 320,
                height: 240,
                content: '<div id="webscan1" scroll="no"/>', 
                onClose: function () {
                    $(this).panel('destroy');
                },
                onOpen: function () {
                    $("#webscan1").webscan();
                }
            });
        }
    }
    return {
        time: 1000,
        show: function () {
            cshow(this);
        },
        onGetCode: function (text) {
            var obj = JSON.parse(text);
            if(typeof obj==='object')
            {
                if (obj.c)
                {
                    //webjs.showform(obj.c, obj.ID);
                    var ds = new datasource();
                    ds.c = obj.c;
                    ds.init();
                    var f = new childform();
                    f.itemshow(ds, obj.ID);
                }
            }
            else
            {
                var ele = document.activeElement;
                if (ele) {
                    $(ele).val(text);
                }
            }
        }
    }
} ();