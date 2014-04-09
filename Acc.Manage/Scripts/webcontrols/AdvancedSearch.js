(function ($) {
    function _initsource(ele, opt) {
        var fs = [];
        for (var i in opt.source.items) {
            var item = opt.source.items[i];
            if (item.field != "ROWINDEX")
                fs.push({ "Text": item.title, "Value": item.field });
        }
        var gridsource = new datasource();
        gridsource.modeldata = { childitem: [] };
        gridsource.items = [
         { field: "Relation", title: "逻辑", width: "50", align: 'center' },
        { field: "StartFH", title: "(", width: "40", align: 'center' },
        { field: "ColumnName", title: "字段", width: "160", align: 'center' },
        { field: "Symbol", title: "比较", width: "80", align: 'center' },
        { field: "Value", title: "比较值", width: "160", align: 'center' },
        { field: "EndFH", title: ")", width: "40", align: 'center' }

        ];
        gridsource.modeldata.childitem = [
        { field: "Relation", title: "逻辑", visible: true, isedit: true, comvtp: { isvtp: true, items: [{ "Text": "并且", "Value": "and" }, { "Text": "或者", "Value": "or"}]} },
        { field: "StartFH", title: "(", visible: true, isedit: true },
        { field: "ColumnName", title: "字段", visible: true, isedit: true, comvtp: { isvtp: true, items: fs} },
        { field: "Symbol", title: "比较", visible: true, isedit: true, comvtp: { isvtp: true, items: [{ "Text": "等于", "Value": "=" }, { "Text": "大于", "Value": ">" }, { "Text": "小于", "Value": "<"}]} },
        { field: "Value", title: "比较值", visible: true, isedit: true },
        { field: "EndFH", title: ")", visible: true, isedit: true }
         ];
        return gridsource;
    };
    function _initdescsource(ele, opt) {
        var fs = [];
        for (var i in opt.source.items) {
            var item = opt.source.items[i];
            if (item.field != "ROWINDEX")
                fs.push({ "Text": item.title, "Value": item.field });
        }
        var s = new datasource();
        s.modeldata = { childitem: [] };
        s.items = [
        { field: "cn", title: "字段", width: "350", align: 'center' },
        { field: "ve", title: "排序", width: "160", align: 'center' }
        ];
        s.modeldata.childitem = [
        { field: "cn", title: "字段", visible: true, isedit: true, comvtp: { isvtp: true, items: fs} },
        { field: "ve", title: "排序", visible: true, isedit: true, comvtp: { isvtp: true, items: [{ "Text": "升序", "Value": "asc" }, { "Text": "降序", "Value": "desc"}]} }
         ];
        return s;
    };
    function _pxcl(data, source, id) {
        var g = new datagrid();
        g.applyTo = id;
        g.onAdding = function (row) {
            row.ve = "desc";
        }
        g.datasource = data;
        g.isdownadd = true;
        g.iseventrow = false;
        g.fitcolumns = false;
        g.isupdown = true;
        g.init();
        g.edited();
        return g;
    };
    function _clone(rows) {
        var nr = [];
        for (var i in rows) {
            var obj = {};
            for (var c in rows[i]) {
                obj[c] = rows[i][c];
            }
            nr.push(obj);
        }
        return nr;
    }
    function _save(grid, grid1, opt, tree, name) {
        var fa = {};
        fa.title = name;
        fa.c = opt.source.c;
        grid.endedit();
        grid1.endedit();
        var wrows = _clone(grid.getrows());
        var srows = _clone(grid1.getrows());
        fa.tj = wrows;
        fa.px = srows;
        var isadd = _setsojb(fa);
        if (isadd) {
            $(tree).tree('append', {
                parent: null,
                data: [{
                    text: name,
                    state: 'open',
                    attributes: fa
                }]
            });
            upmenu(opt);
        }
        else {
            var node = $(tree).tree('getSelected');
            if (node) {
                $(tree).tree('update', {
                    target: node.target,
                    attributes: fa
                });
            }
        }
    };

    function _savefa(grid, grid1, opt, tree, name) {
        if (name === undefined) {
            $.messager.prompt('方案名称', '请输入查询方案名称。', function (r) {
                if (r) {
                    _save(grid, grid1, opt, tree, r);
                }
            });
        }
        else {
            _save(grid, grid1, opt, tree, name);
        }
    };

    function _inittool(grid, grid1, opt, tree) {
        var t = new toolstrip();
        t.ishelp = false;
        t.applyTo = "tool1";
        t.issearch = false;
        t.isRecycle = false;
        t.datasource = opt.source;
        t.datasource.IsClearAway = false;
        t.clickcommand = function (sender, data) {
            switch (data.command) {
                case "search":
                    try {
                        grid.endedit();
                        var wrows = grid.getrows();
                        grid1.endedit();
                        var srows = grid1.getrows();
                        search(opt.source, wrows, srows);
                    }
                    catch (e) {
                        alert(e.message);
                    }
                    break;
                case "save":
                    var node = $(tree).tree('getSelected');
                    if (node && node.attributes) {
                        _savefa(grid, grid1, opt, tree, node.attributes.title);
                    }
                    else
                        _savefa(grid, grid1, opt, tree);
                    break;
                case "lsave":
                    _savefa(grid, grid1, opt, tree);
                    break;
                case "remove":
                    var node = $(tree).tree('getSelected');
                    if (node == null) {
                        $.message.alert("请先选择要删除的查询方案！");
                        return;
                    }
                    _remove(node.text, opt);
                    $(tree).tree('remove', node.target);
                    break;

            }
        }
        t.commands = [
                            { icon: "icon-save", command: "save", title: "保存覆盖原查询方案", name: "保存", visible: true },
                            { icon: "icon-save", command: "lsave", title: "保存为新查询方案", name: "另存为", visible: true },
                             { icon: "icon-cancel", command: "remove", title: "删除查询方案", name: "删除", visible: true },
                            { icon: "icon-search", command: "search", title: "", td: "help", name: "查询", visible: true }
                      ];
        t.init();
    };
    function _tjcl(data, source, id) {
        var g = new datagrid();
        g.applyTo = id;
        g.onAdding = function (row) {
            row.Symbol = "=";
            row.Relation = "and";
        }
        g.onInitRow = function (row) {
            if (row.field == 'Value') {
                row.formatter = function (value, row, index) {
                    if (row.displayValue)
                        return row.displayValue;
                    return value;
                }
            }
        }
        g.onBeforeEdit = function (index, row, ed) {
            if (row.box) {
                if (ed && ed.field == 'Value') {
                    for (var i in row.box) {
                        ed.target[i] = row.box[i];
                    }
                    ed.target.update();
                    ed.target.onevent();
                }
                if (ed && ed.field == 'Symbol') {
                    ed.target.comvtp.items = row.box.getsymbol();
                    ed.target.update();
                    ed.target.onevent();
                }
            }
        }
        g.onAfterEdit = function (grid,rowIndex, rowData, changes) {
            var editor = g.geteditor("Value");
            if (editor && editor.target) {
                var box = editor.target;
                if (box.comvtp && box.comvtp.isvtp) {
                    var items = box.comvtp.items;
                    for (var i in items) {
                        if (items[i].Value == parseInt(rowData.Value))
                            rowData.displayValue = items[i].Text;
                    }

                }
                if (box.foreign && box.foreign.isfkey) {
                    rowData.displayValue = box.text();
                }
                var obj = {};
                for (var i in box) {
                    obj[i] = box[i];
                }
                rowData.box = obj;
            }

        }
        g.onselect = function (sender, item) {
            if (sender.field == 'ColumnName') {
                var sitem = source.getitem(item.value);
                if (sitem) {
                    var editor = g.geteditor("Value")
                    var txt = seteditor(editor, sitem);
                    txt.parent = this;
                    var symbol = g.geteditor("Symbol");
                    symbol.target.comvtp.items = txt.getsymbol();
                    symbol.target.update();
                    symbol.target.onevent();
                }
            }
            if (sender.field == 'Symbol') {
                if (item.value[0] == "@") {
                    var editor = g.geteditor("Value")
                    var txt = editor.target;
                    txt.disabled = true;
                    txt.update();
                }
                else {
                    var editor = g.geteditor("Value")
                    var row = g.getselectrow();
                    var sitem = source.getitem(row["ColumnName"]);
                    var txt = seteditor(editor, sitem);
                    txt.parent = this;
                }
            }
        }
        g.datasource = data;
        g.isdownadd = true;
        g.iseventrow = false;
        g.isupdown = true;
        g.init();
        g.fitcolumns = false;
        g.edited();
        return g;
    };
    function seteditor(editor, sitem) {
        var txt = editor.target;
        txt.required = false;
        txt.comvtp = null;
        txt.foreign = null;
        txt.type = "";
        //txt.field = item.field;
        //txt.width = sender.width;
        for (var n in sitem) {
            txt[n] = sitem[n];
        }
        txt.searchstate = true;
        txt.field = "Value";
        txt.disabled = false;
        txt.visible = true;
        txt.update();
        txt.onevent();
        return txt;
    }
    function search(source, wrows, srows) {
        source.sort = "";
        source.where = "";
        var tj = [];
        for (var i in wrows) {
            var r = wrows[i];
            if (r.ColumnName != "") {//&& r.Value != ""
                tj.push(
                        {
                            ColumnName: r.ColumnName,
                            StartFH: r.StartFH,
                            Symbol: r.Symbol,
                            Value: r.Value,
                            EndFH: r.EndFH,
                            Relation: r.Relation,
                            Type: r.box.type
                        });
            }
        }
        var sort = "";
        for (var i in srows) {
            if (srows[i].cn != "") {
                if (sort == "")
                    sort = srows[i].cn + " " + srows[i].ve;
                else
                    sort += "," + srows[i].cn + " " + srows[i].ve;
            }
        }
        if (sort != "")
            source.sort = sort;
        if (tj.length > 0)
            source.where = tj;
        source.reload();
    };
    function _treeinit(tree, grid, grid1, source) {
        $(tree).tree();
        var fa = _getsobj()
        if (fa && fa.searchFA) {
            $(tree).tree({
                onSelect: function (node) {
                    if (node.attributes) {
                        var rows = {};
                        rows.total = node.attributes.tj.length;
                        rows.rows = node.attributes.tj;
                        grid.load(rows);
                        var rows1 = {};
                        rows1.total = node.attributes.px.length;
                        rows1.rows = node.attributes.px;
                        grid1.load(rows1);
                    }
                }
            });
            for (var i in fa.searchFA) {
                var f = fa.searchFA[i];
                if (f && f.c == source.c) {
                    $(tree).tree('append', {
                        parent: null,
                        data: [{
                            text: f.title,
                            state: 'open',
                            attributes: f
                        }]
                    });
                }
            }
        }
    }
    function _getsobj() {
        var fa = store.get(webjs.userid);
        fa = JSON.parse(fa);
        return fa;
    };
    function _setsojb(fa) {
        var isadd = true;
        var oldfa = _getsobj();
        if (oldfa == undefined || oldfa == null) {
            oldfa = {};
            oldfa.searchFA = [];
        }
        var old = oldfa.searchFA;
        for (var i in old) {
            var f = old[i];
            if (f && f.c == fa.c && f.title == fa.title) {
                isadd = false;
                f.tj = fa.tj;
                f.px = fa.px;
                break;
            }
        }
        if (isadd) {
            old.push(fa);
        }
        _searchsave(oldfa);
        return isadd;
    };
    function _remove(name, opt) {
        var sv = _getsobj()
        for (var i in sv.searchFA) {
            if (sv.searchFA[i] && sv.searchFA[i].title == name && sv.searchFA[i].c == opt.source.c) {
                sv.searchFA[i] = undefined;
                sv.searchFA.splice(i);
            }
        }
        _searchsave(sv);
        upmenu(opt);
    }
    function _searchsave(sv) {
        var vvv = JSON.stringify(sv, function (key, value) {
            if (key == 'oldrow' || key == 'parent')
                return undefined;
            else
                return value;
        });
        store.set(webjs.userid, vvv);
    };
    function _win(ele, opt) {
        var layout = $('<div class="easyui-layout" border="false" fit="true"/>').appendTo(ele);
        $('<div region="north" border="false"><div id="tool1"/></div>').appendTo(layout);
        var center = $('<div region="center" border="false"></div>').appendTo(layout);
        var lt = $('<div class="easyui-layout" fit="true" border="false"></div>').appendTo(center);
        var west = $('<div region="west" split="true" border="true" style="width:200px" title="查询方案"></div>').appendTo(lt);
        var tree = $('<ul class="easyui-tree"/>').appendTo(west);
        var ct = $('<div region="center" border="false"></div>').appendTo(lt);
        var tabs = $('<div class="easyui-tabs"></div>').appendTo(ct);
        $(tabs).tabs();
        $(tabs).tabs("add", {
            title: "条件",
            content: '<div id="grid1"/>',
            fit: true
        })
        $(tabs).tabs("add", {
            title: "排序",
            content: '<div id="grid2"/>',
            fit: true
        })
        $(tabs).tabs({ fit: true });
        $(lt).layout();
        var data = _initsource(ele, opt);
        data.c = opt.source.c;
        var tjg = _tjcl(data, opt.source, "grid1");

        var data1 = _initdescsource(ele, opt);
        data1.c = opt.source.c;
        var pxcl = _pxcl(data1, opt.source, "grid2");

        _treeinit(tree, tjg, pxcl, opt.source);
        _inittool(tjg, pxcl, opt, tree);
        return layout;
    };
    function _show(ele) {
        var opt = $.data(ele, "advancedsearch").options;
        var div = $("<div/>").appendTo(ele);
        _win(div, opt).layout();
        $(div).dialog({
            title: opt.source.modeldata.title + opt.title,
            width: opt.width,
            modal: true,
            resizable: false,
            cache: false,
            height: opt.height,
            collapsible: true,
            minimizable: false,
            maximizable: false,
            onClose: function () {
                $(div).panel('destroy');
                //$.data(ele, "advancedsearch").remove();
            },
            buttons: [{
                text: '取消',
                handler: function () {
                    $(div).dialog('close');
                }
            }]
        });
    };
    function _init(ele) {
        var menuid = "search" + webjs.newid();
        var menu = $("<div id='" + menuid + "'style='width:150px;overflow:hidden;' />").appendTo(ele);
        var opt = $.data(ele, "advancedsearch").options;
        opt.menuid = menuid;
        opt.ele = ele;
        $('<div iconCls="icon-reload">高级查询</div>').appendTo(menu).bind('click', function () {
            $(ele).advancedsearch('show');
        });
        $('<div class="menu-sep"></div>').appendTo(menu);
        var fa = _getsobj()
        if (fa && fa.searchFA) {
            for (var i in fa.searchFA) {
                var f = fa.searchFA[i];
                _cmenu(f, opt);
            }
        }
        $("#" + opt.butid).splitbutton({ menu: "#" + menuid });
    };
    function _cmenu(f, opt) {
        if (f && f.c == opt.source.c) {
            var mb = $('<div iconCls="icon-search">' + f.title + '</div>').appendTo($("#" + opt.menuid)).bind('click', function () {
                var title = $(this).text();
                var nfa = _getsobj();
                for (var n in nfa.searchFA) {
                    var sfa = nfa.searchFA[n];
                    if (sfa && sfa.c == opt.source.c && sfa.title == title) {
                        search(opt.source, sfa.tj, sfa.px);
                    }
                }
            });
        }
    };
    function upmenu(opt) {
        $("#" + opt.menuid).remove();
        _init(opt.ele);
    }
    $.fn.advancedsearch = function (method, sender) {
        if (typeof method == "string") {
            return $.fn.advancedsearch.methods[method](this, sender);
        }
        method = method || {};
        return this.each(function () {
            var sender = $.data(this, "advancedsearch");
            if (sender) {
                $.extend(sender.options, method);
            } else {
                $.data(this, "advancedsearch", { options: $.extend({}, $.fn.advancedsearch.defaults, $.fn.advancedsearch.parseOptions(this), method) });
                _init(this);
            }
        });
    };
    $.fn.advancedsearch.methods = {
        show: function (jq) {
            _show(jq[0]);
        }
    };
    $.fn.advancedsearch.defaults = {
        source: null,
        title: "高级查询",
        width: "800",
        height: "500",
        butid: ""
    };
    $.fn.advancedsearch.parseOptions = function (sender) {
        var t = $(sender);
        return $.extend({}, $.parser.parseOptions(sender, ["width", "height", { fit: "boolean", border: "boolean", animate: "boolean"}]));
    };
})(jQuery);