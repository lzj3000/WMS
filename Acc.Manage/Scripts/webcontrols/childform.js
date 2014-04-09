var childform = function () {
    function cinit(f) {
        var p = cname(f);
        var table = ctable(f);
        var isc = cischild(f);
        var win;
        if (!f.fit)
            win = $('<div class="easyui-dialog" id="' + f.name + '" closed="true" collapsible="true"/>').appendTo(p);
        else {
            $("#" + f.name).panel({ fit: true });
            win = $("#" + f.name);
        }
        if (isc) {
            $('<div class="easyui-layout" border="false" id="' + f.name + '-layout" fit="true"></div>').appendTo($("#" + f.name));
            $('<div region="north" border="true" split="true" style="height:auto;width:auto;padding:1px;">' + table + '</div>').appendTo($("#" + f.name + "-layout"));
            $('<div region="center" border="true" style="height:auto;width:auto;padding:1px;" id="' + f.name + '-center"></div>').appendTo($("#" + f.name + "-layout"));
            $("#" + f.name + "-layout").layout();
        }
        else {
            if (f.fit) {
                $(win).append(table);
                $("#" + f.name).panel();
            }
            else {
                $('<div class="easyui-panel" fit="true" id="' + f.name + '-table">' + table + '</div>').appendTo(win);
                $("#" + f.name + '-table').panel({ fit: true });
            }
        }

        if (isc) {
            ctabs(f);
        }
        setremarkwidth(f);
    }
    function setremarkwidth(f, ww) {
        if (f.isremark) {
            var tw = ww;
            if (tw == undefined)
                tw = $("#tableform-" + f.id).width();
            var td = $("#tableform-" + f.id).find("td");
            var w1 = $(td[0]).width();
            var w2 = $(td[1]).width();
            var w3 = w2 - 200;
            var w = tw - (w2 - 200) - w1 - w3;
            var text = f.isremark.val();
            f.isremark.setwidth(w);
            f.isremark.val(text);
        }
    }
    function cischild(f) {
        var h = false;
        if (f.parent.ischildform) {
            if (f.parent.modeldata.childmodel && f.parent.modeldata.childmodel.length > 0)
                h = true;
        }
        return h;
    }
    function cname(f) {
        f.id = webjs.newid();
        f.name = f.id + "-win";
        var rq;
        if (f.applyTo == "")
            rq = $("<div id='" + f.name + "'/>").appendTo($("#temp"));
        else {
            $("#" + f.applyTo).attr("id", f.name);
            rq = $("#" + f.name);
        }
        return rq;
    }
    function ctabs(f, tabs) {
        if (tabs == null)
            tabs = $('<div class="easyui-tabs" fit="true"/>').appendTo($("#" + f.name + '-center'));
        $(tabs).tabs({
            onSelect: function (title, index) {
                var name = f.parent.modeldata.childmodel[index].name;
                var grid = f.griditems[name];
                if (grid && !grid.isrowedit()) {
                    if (f.m == "" || f.m == "edit") {
                        if (f.unionobj != undefined && f.unionobj != null)
                            childgridload(f, name, f.unionobj);
                    }
                    f.onSelectTab(index, grid);
                }
            }
        });
        for (var i in f.parent.modeldata.childmodel) {
            var model = f.parent.modeldata.childmodel[i]; ;
            if (!model.visible) continue;
            var grid = new datagrid();
            grid.datasource = f.parent.getcmsource(i);
            grid.datasource.page = 1;
            grid.datasource.onAfterEdit = f.parent.onAfterEdit;
            grid.datasource.onSelectTab = f.parent.onSelectTab;
            grid.datasource.onCommandClick = f.parent.onCommandClick;
            grid.datasource.onForeigned = f.parent.onForeigned;
            grid.datasource.onsearchforeign = f.onsearchforeign;
            grid.applyTo = f.id + "tt" + i;
            grid.showfooter = f.childfooter;
            if (model.ischeck)
                grid.check = model.ischeck;
            if (f.m == "add")
                grid.sortcolumn = null;
            $(tabs).tabs("add", {
                title: model.title,
                content: '<div id="' + f.id + 'tt' + i + '"/>',
                fit: true
            })
            grid.fit = true;
            if (f.childfit) {
                if (grid.datasource.items.length < 8)
                    grid.fitcolumns = f.childfit;
                else
                    grid.fitcolumns = false;
            }
            //grid.check = f.childcheck;
            for (var cmd in f.editobj) {
                grid.editobj[cmd] = f.editobj[cmd];
            }
            if (model.disabled) {
                grid.editobj.add = false;
                grid.editobj.edit = false;
                grid.editobj.remove = false;
            }
            else {
                grid.editobj.add = model.isadd;
                grid.editobj.edit = model.isedit;
                grid.editobj.remove = model.isremove;
                if (grid.editobj.add) {
                    grid.onAdding = function (obj) {
                        f.parent.onchildAdding(f, grid, obj);
                    }
                }
            }
            if (model.isselect) {
                grid.editobj.select = { c: model.c };
            }
            f.griditems[model.name] = grid;
            f.childmodel[model.name] = model.modeldata;
            grid.init();
            grid.parent = f;
            if (f.childedit)
                grid.edited();

        }
    }
    //创建form表中控件
    function ctable(f, div) {
        var tb = '<table id="tableform-' + f.id + '" cellpadding="1" cellspacing="5" style="width:100%;align:center">';
        var editi = -1;
        var item;
        var isremark = false;
        for (var i in f.childitem) {
            item = f.childitem[i];
            var box = new textbox();
            for (var e in item) {
                box[e] = item[e];
            }
            box.id = f.id + '-' + item.field;
            box.parent = f;
            if ((item.field == 'REMARK' && item.visible) || (item.isremark && item.visible)) {
                box.isremark = true;
                isremark = box;
                continue;
            }
            if (!item.visible || !item.isedit)
                continue;
            else {
                editi++;
            }
            if (editi > 0 && editi % 3 == 0)
                tb += "</tr>";
            if (editi == 0 || editi % 3 == 0)
                tb += "<tr>";
            f.items[editi] = box;
            tb += "<td align='right'>" + box.title + "</td><td>" + box.box() + "</td>";
        }
        if (isremark) {
            f.items[f.items.length] = isremark;
            tb += "<tr><td align='right'>" + isremark.title + "</td><td colspan='5'>" + isremark.box() + "</td></tr>";
        }
        tb += "</table>";
        f.isremark = isremark;
        return tb;
    }
    //获取设置form中控件的value值
    function gsvalue(sender, obj) {
        var set = true;
        if (obj == undefined || obj == null) {
            set = false;
            obj = {};
        }
        if (set)
            setvalue(sender, obj);
        else
            obj = getvalue(sender);
        obj = setchildrows(sender, obj, set);
        if (set) {
            sender.selectTab(0);
        }
        return obj;
    }
    function setvalue(sender, obj) {
        sender.unionobj = webjs.clone(obj);
        for (var i in sender.items) {
            var e = sender.items[i];
            if (e.foreign && e.foreign.isfkey) {
                e.fval(obj);
            }
            else {
                e.val(obj[e.field]);
            }
        }
        sender.edititem = {};
        for (var i in obj) {
            sender.edititem[i] = obj[i];
        }
    }
    function getvalue(sender) {
        var obj = {};
        if (sender.unionobj != null)
            obj = sender.unionobj;
        for (var i in sender.items) {
            var e = sender.items[i];
            obj[e.field] = e.val();
        }
        if (sender.m == "edit") {
            obj.StateBase = 3;
            for (var i in obj) {
                if (obj[i] != sender.edititem[i]) {
                    obj.GetOldObject = {}
                    for (var n in sender.edititem) {
                        obj.GetOldObject[n] = sender.edititem[n];
                    }
                    break;
                }
            }
        }
        if (sender.m == "add" || sender.m == "addroot") {
            obj.StateBase = 0;
        }
        return obj;
    }
    function childgridload(sender, n, obj, where) {
        var index;
        if (typeof n === 'number')
            index = n;
        else
            index = sender.parent.getcmindex(n);
        var grid = sender.griditems[n];
        grid.load(null);
        grid.loading();
        sender.loadforeign(index, obj, where, function (e) {
            if (e.error) {
                $.messager.alert('异常', e.errormsg, 'error');
            }
            else
                grid.load(e);
        });
    }
    function setchildrows(sender, obj, set) {
        if (set) {
            if (sender.m == "add" || sender.m == "addroot") {
                if (sender.griditems != null) {
                    for (var n in sender.griditems) {
                        if (obj[n] != undefined && obj[n] != null) {
                            sender.griditems[n].insertRow(obj[n]);
                        }
                        else
                            sender.griditems[n].load({ rows: [], total: 0 });
                    }
                }
                return obj;
            }
            if (sender.griditems != null) {
                for (var n in sender.griditems) {
                    if (obj[n]) {
                        sender.griditems[n].load(obj[n]);
                    }
                    //                    else {
                    //                        childgridload(sender, n, obj);
                    //                    }
                }
            }
        }
        else {
            var grid;
            for (var n in sender.griditems) {
                grid = sender.griditems[n];
                if (grid.isedit) {
                    var rows = grid.geteditrows();
                    obj[n] = rows;
                }
            }
        }
        return obj;
    }
    function formsave(f) {
        if (f.m == "add")
            storesave(f.parent, f.m, f.val());
        var v = f.isValid();
        if (v) {
            if (f.parent) {
                f.parent.clickcommand("save", f);
                if (f.CommandClose) {
                    $("#" + f.name).dialog('close');
                }
            }
        }
        //        else
        //        { throw "数据异常！"; }
    }
    function storesave(d, m, v) {
        var sto = store.get(d.c);
        if (sto == null) {
            sto = {};
        }
        sto.m = m;
        sto.v = v;
        var vvv = webjs.jsonval(sto);
        store.set(d.c, vvv);
    }
    function cshowcd(f) {
        var save = {
            text: '保存',
            iconCls: 'icon-save',
            handler: function () {
                try {
                    formsave(f);
                }
                catch (e) {
                    $.messager.alert('异常', e, 'error');
                }
            }
        };
        var newsave = {
            text: '另存为新行',
            iconCls: 'icon-save',
            handler: function () {
                try {
                    var oldm = f.m;
                    f.m = "addcopy";
                    f.CommandClose = true;
                    formsave(f);
                    f.m = oldm;

                }
                catch (e) {
                    $.messager.alert('异常', e, 'error');
                }
            }
        };
        var saveadd = {
            text: '保存并新增',
            iconCls: 'icon-save',
            handler: function () {
                try {
                    var cc = f.CommandClose;
                    f.CommandClose = false;
                    formsave(f);
                    f.CommandClose = cc;
                    f.clear();
                }
                catch (e) {
                    $.messager.alert('异常', e, 'error');
                }
            }
        };
        var prev = {
            //text: "前一行",
            iconCls: 'pagination-prev',
            handler: function () {
                if (f.parent) {
                    f.parent.onPrev();
                    // f.val(f.parent.selectItem);
                }
            }
        };
        var next = {
            //text: "后一行",
            iconCls: 'pagination-next',
            handler: function () {
                if (f.parent) {
                    f.parent.onNext();
                    //f.val(f.parent.selectItem);
                }
            }
        };
        var bat = [];
        if (f.showSave) {
            bat.push(save);
            if (f.m == "edit") {
                var ds = f.parent;
                for (var c in ds.commands) {
                    var ccc = ds.commands[c];
                    if (ccc.command == "add") {
                        if (ccc.visible) {
                            bat.push(newsave);
                            break;
                        }
                    }
                }
            }
            if (f.m == "add")
                bat.push(saveadd);
            bat.push("-");
        }
        if (f.showFN) {
            bat.push(prev);
            bat.push(next);
        }
        if (f.tool) {
            bat.push("-");
            var tss = f.tool.getcmds();
            for (var i in tss) {
                if (tss[i].editshow) {
                    var obj = {};
                    obj.text = tss[i].name;
                    obj.iconCls = tss[i].icon;
                    obj.handler = function () {
                        f.tool.command(this.id);
                    }
                    obj.id = tss[i].command;
                    bat.push(obj);
                }
            }
            f.tool.onClicked = function (sender, comm) {
                if (f.CommandClose) {
                    $("#" + f.name).dialog('close');
                }
            }
        }
        bat.push("-");
        //        bat.push({
        //            text: '帮助',
        //            iconCls: 'icon-help',
        //            handler: function () { alert('help') }
        //        });
        return bat;
    }
    function cshow(f) {
        var isc = cischild(f);
        var h = 300;
        if (isc)
            h = 600;
        var opt = {};
        opt.title = f.title;
        opt.width = 900;
        opt.modal = true;
        opt.resizable = true;
        opt.cache = false;
        opt.height = h;
        opt.collapsible = true;
        opt.minimizable = false;
        opt.maximizable = true;
        //opt.inline = true;
        opt.onResize = function (width, height) {
            if (f.width == undefined || f.width != width) {
                f.width = width;
                $("#" + f.name + '-layout').layout('resize');
                setremarkwidth(f, width);
                $("#" + f.name + "-table").panel('resize');
                for (var n in f.griditems) {
                    grid = f.griditems[n];
                    grid.resize();
                }

            }
        };
        opt.buttons = [{
            text: '取消',
            handler: function () {
                $("#" + f.name).dialog('close');
            }
        }];
        opt.onClose = function () {
            f.remove();
        };
        if (f.IsSave)
            opt.toolbar = cshowcd(f);
        else {
            opt.buttons.unshift({
                text: '确定',
                handler: function () {
                    if (f.isValid()) {
                        f.onClickOk(f);
                        $("#" + f.name).dialog('close');
                    }
                }
            });
        }
        $("#" + f.name).dialog(opt);
        if (f.m == "edit") {
            if (f.parent) {
                f.val(f.parent.selectItem);
            }
        }
        //f.selectTab(0);
        // setremarkwidth(f,927);
    }
    return {
        name: "",
        title: "",
        parent: null,
        childmodel: [],
        childitem: [],
        childedit: true,
        childcheck: false,
        childfooter: true,
        id: "",
        items: [],
        applyTo: "",
        griditems: [],
        fit: false,
        childfit: true,
        disabled: false,
        editobj: { add: true, edit: true, remove: true },
        m: "",
        CommandClose: false,
        IsSave: true,
        showSave: true, //是否显示保存按钮
        showFN: true, //是否显示前进后退按钮
        unionobj: null,
        init: function () {
            cinit(this);
            for (var i in this.items) {
                var e = this.items[i];
                e.onevent();
                e.ready();
                e.clear();
            }
        },
        getpanel: function () {
            return ctable(this);
        },
        show: function () {
            this.init();
            var f = this;
            if (f.parent) {
                f.parent.onSelect = function (item) {
                    //f.parent.onSelect(item);
                    f.val(item);
                }
            }
            cshow(f);
        },
        setform: function (tp, o) {
            for (var i in this.items) {
                var e = this.items[i];
                if (e.field == tp || e.title == tp) {
                    e.val(o);
                    return;
                }
            }
        },
        findbox: function (field) {
            var box = null;
            var name = field.toUpperCase();
            for (var i in this.items) {
                var e = this.items[i];
                if (e.field == name) {
                    box = e;
                    break;
                }
            }
            return box;
        },
        findgrid: function (name) {
            var grid = null;
            for (var n in this.griditems) {
                if (name == n) {
                    grid = this.griditems[n];
                    break;
                }
            }
            return grid;
        },
        setmenu: function () {
            var form = $("#" + this.name);
            var tools = cshowcd(this);
            form.dialog({ toolbar: tools });
        },
        fval: function () {
            var obj = {};
            if (this.unionobj != undefined && this.unionobj != null)
                obj = this.unionobj;
            for (var i in this.items) {
                var e = this.items[i];
                obj[e.field] = e.val();
            }
            return obj;
        },
        val: function (o) {
            return gsvalue(this, o);
        },
        getallval: function () {
            var obj = {};
            for (var i in this.items) {
                var e = this.items[i];
                obj[e.field] = e.val();
                if (e.foreign) {
                    obj[e.foreign.displayname] = e.foreign.displayvalue
                }
            }
            for (var n in this.griditems) {
                var grid = this.griditems[n];
                if (grid.isedit) {
                    var rows = grid.geteditrows();
                    obj[n] = rows;
                }
            }
            return obj;
        },
        json: function () {
            try {
                return webjs.jsonval(this.val());
            }
            catch (e) {
                alert(e.Message);
            }
        },
        //todo:未修改完成
        onsearchforeign: function (box, obj) {
            if (this.parent && this.parent.onsearchforeign)
                this.parent.onsearchforeign(box, obj);
        },
        onforeignform: function (sender, value) {
            value.eventrow = this.json();
            var from = this;
            var ff = from.parent.onforeignform(sender, value, function () {
                if (from.parentgrid == undefined)
                    return from.val();
                else {
                    var grid = from.parentgrid;
                    if (grid.parent) {
                        var obj = {};
                        obj.row = from.val();
                        obj.item = grid.parent.fval();
                        return obj;
                    }
                    else
                        return from.val();
                }
            });
            return ff;
        },
        onforeign: function (sender, value) {
            value.eventrow = this.json();
            var from = this;
            this.parent.onforeign(sender, value, function () {
                if (from.parentgrid == undefined)
                    return from.val();
                else {
                    var grid = from.parentgrid;
                    if (grid.parent) {
                        var obj = {};
                        obj.row = from.val();
                        obj.item = grid.parent.fval();
                        return obj;
                    }
                    else
                        return from.val();
                }
            });
        },
        onselect: function (sender, item) {
            if (this.parent && this.parent.onFormSelect) {
                this.parent.onFormSelect(this, sender, item);
            }
        },
        onClickOk: function (form) {

        },
        remove: function () {
            //$("#" + this.name).remove();
            this.parent.onSelect = null;
            $(".validatebox-tip").remove();
            $("#" + this.name).panel('destroy');
        },
        resize: function (w, h) {
            $("#" + this.name).panel('resize',
            {
                width: w,
                height: h
            });
        },
        clear: function () {
            for (var i in this.items) {
                var e = this.items[i];
                e.clear();
            }
            for (var n in this.griditems) {
                var grid = this.griditems[n];
                grid.load(false);
            }
        },
        setdisabled: function (de) {
            this.disabled = de;
            for (var i in this.items) {
                var e = this.items[i];
                e.setdisabled(de);
            }
            for (var i in this.griditems) {
                this.griditems[i].setdisabled(de);
            }
        },
        tabs: function (tab) {
            cname(this);
            ctabs(this, tab);
        },
        onSelectTab: function (index, grid) {

        },
        selectTab: function (index) {
            var tabs = $("#" + this.name + " .easyui-tabs");
            if (tabs) {
                tabs.tabs('select', index);
            }
        },
        childload: function (name, obj, where) {
            childgridload(this, name, obj, where);
        },
        loadforeign: function (index, row, w, f) {
            var e = this.parent.loadforeign(index, row, w, f);
            if (e && f) {
                f(e);
            }
        },
        setforeigndisplay: function (obj) {
            for (var n in obj) {
                var tb = false;
                for (var i in this.items) {
                    if (this.items[i].field == n) {
                        tb = this.items[i];
                        break;
                    }
                }
                if (tb) {
                    if (tb.foreign && tb.foreign.isfkey)
                        tb.fval(obj);
                    else
                        tb.val(obj[n]);
                }
            }
        },
        rejectChanges: function () {
            for (var i in this.griditems) {
                this.griditems[i].rejectChanges();
            }
            if (this.m == "edit" && this.unionobj != null)
                this.val(this.unionobj);
        },
        itemshow: function (source, id, title) {
            var e = source.load("id=" + id);
            if (title == undefined || title == "") {
                title = source.modeldata.title;
            }
            if (e.total > 0) {
                source.selectItem = e.rows[0];
                this.parent = source;
                this.title = title;
                this.childitem = source.modeldata.childitem;
                this.m = 'edit';
                this.show();
            }
        },
        isValid: function () {
            var v = true;
            for (var i in this.items) {
                v = this.items[i].isValid();
                if (v === false)
                    return v;
            }
            for (var i in this.griditems) {
                v = this.griditems[i].isValid();
                if (v === false)
                    return v;
            }
            return v;
        }
    }
}