
var form = function () {
    function cinit(f) {
        f.name = f.id + "-win";
        var rq;
        if (f.applyTo == "")
            rq = $("body");
        else {
            var tt = f.id + "-" + f.applyTo;
            $("#" + f.applyTo).attr("id", tt);
            rq = $("#" + tt);
        }
        var win = $('<div class="easyui-dialog" id="' + f.name + '" style="width:900px;height:600px;padding:5px;" closed="true"/>').appendTo(rq);
        var layout = $('<div class="easyui-layout" data-options="fit:true"></div>').appendTo(win);
        var top = $('<div data-options="region:"east",split:true" style="width:100px"></div>').appendTo(layout);
        $("#" + f.name).dialog("open");
    }
    function ctabs(f, div) {
        var tabs = $('<div class="easyui-tabs" fit="true"/>').appendTo($("#" + div));
        for (var i in f.parent.modeldata.childmodel) {
            var model = new Object();
            model.modeldata = f.parent.modeldata.childmodel[i];
            model.name = f.parent.c;
            var source = new datasource();
            source.setmodel(model);
            var grid = new datagrid();
            grid.datasource = source;
            grid.applyTo = model.modeldata.name;
            var tab = $('<div title="' + model.modeldata.title + '" fit="true"/>').appendTo(tabs);
            $('<div id="' + grid.applyTo + '"/>').appendTo(tab);
            grid.fit = false;
            grid.init();
        }
    }
    //创建form表中控件
    function ctable(f, div) {
        var tb = '<table cellpadding="1" cellspacing="5" style="width:100%;align:center">';
        var editi = -1;
        var item;
        for (var i in f.childitem) {
            item = f.childitem[i];
            if (!item.visible || !item.isedit)
                continue;
            else {
                editi++;
            }
            if (editi > 0 && editi % 3 == 0)
                tb += "</tr>";
            if (editi == 0 || editi % 3 == 0)
                tb += "<tr>";
            var box = new textbox();
            for (var e in item) {
                box[e] = item[e];
            }
            box.id = f.id + '-' + item.field;
            box.parent = f;
            f.items[editi] = box;
            tb += "<td align='right'>" + box.title + "</td><td>" + box.box() + "</td>";
        }
        tb += "</table>";
        $("#" + div).attr("title", f.parent.modeldata.title);
        $(tb).appendTo($("#" + div));
    }
    //获取设置form中控件的value值
    function gsvalue(sender, obj) {
        var set = true;
        if (obj == null) {
            obj = new Object();
            set = false;
        }
        for (var i in sender.items) {
            var e = sender.items[i];
            if (set) {

                if (e.foreign && e.foreign.isfkey) {
                    e.foreign.filedvalue = obj[e.field];
                    e.val(obj[e.foreign.displayname]);
                }
                else {
                    e.val(obj[e.field]);
                }
            }
            else
                obj[e.field] = e.val();
        }
        return obj;
    }
    function cshow(f) {
        $("#" + f.name).dialog({
            title: f.title,
            width: 900,
            modal: true,
            resizable: true,
            cache: false,
            height: 600,
            collapsible: true,
            onClose: function () {
                f.remove();
            },
            toolbar: [{
                text: '保存',
                iconCls: 'icon-save',
                handler: function () {
                    try {
                        if (f.parent) {
                            f.parent.clickcommand("save", f);
                        }
                        $('#' + f.id).dialog('close');
                    }
                    catch (e) {
                        alert(e.Message);
                    }
                }
            },
            {
                text: "前一行",
                iconCls: 'pagination-prev',
                handler: function () {
                    f.parent.onPrev();
                    f.val(f.parent.selectItem);
                }
            },
            {
                text: "后一行",
                iconCls: 'pagination-next',
                handler: function () {
                    f.parent.onNext();
                    f.val(f.parent.selectItem);
                }
            },
            {
                text: '帮助',
                iconCls: 'icon-help',
                handler: function () { alert('help') }
            }
          ]
        });
        if (f.m == "edit") {
            if (f.parent) {
                f.val(f.parent.selectItem);
            }
        }
    }
    return {
        name: "",
        title: "",
        parent: null,
        childmodel: [],
        childitem: [],
        id: "",
        items: [],
        applyTo: "",
        m: "",
        init: function (d1, d2) {
            //try {
            //            this.id = webjs.newid();
            //            cinit(this);
            ctable(this, d1)
            //ctabs(this, d2);
            for (var i in this.items) {
                var e = this.items[i];
                e.ready();
                e.onevent();
            }
            // }
            // catch (e) {
            //     alert(e.Message);
            //}
        },
        getpanel: function () {
            return ctable(this);
        },
        show: function () {
            this.init();
            var f = this;
            cshow(f);
        },
        val: function (o) {
            return gsvalue(this, o);
        },
        json: function () {
            try {
                return webjs.jsonval(this.val());
            }
            catch (e) {
                alert(e.Message);
            }
        },
        onforeign: function (sender, value) {
            this.parent.onforeign(sender, value);
        },
        onselect: function (sender, item) {
            for (var n in item)
                alert(item[n]);
        },
        remove: function () {
            $("#" + this.name).panel('destroy');
        }
    }
}