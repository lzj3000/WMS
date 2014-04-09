var searchform = function () {
    function sinit(f) {
        chtml(f);
        f.source.dblclickrow = function (i, data) {
            f.selectItem = true;
            if (f.dbclose)
                $("#" + f.name).dialog("close");
        }
        f.source.load = function (where) {
            load(f, where);
        }
        f.source.reload = function () {
            load(f, f.source.where);
        }
        f.grid = new datagrid();
        f.grid.check = f.ischeck;
        f.grid.applyTo = f.gridid;
        f.grid.showfooter = false;
        f.grid.loadselect = true;
        if (f.istool) {
            var tool = new toolstrip();
            tool.isRecycle = false;
            tool.isAsearch = false;
            tool.applyTo = f.toolid;
            tool.datasource = f.source;
            tool.ishelp = false;
            tool.clickcommand = function (sender, d) {
                if (d.handler) {
                    var obj = getselectrow(f);
                    d.handler(obj);
                    return;
                }
                if (d.command == "load") {
                    load(f, d.value);
                }
                if (d.command == "tree") {
                    var p = $("#" + f.name + "-layout").layout('panel', 'west');
                    // split="true" 
                    $(p).panel({ title: f.title, split: true });
                    $(p).panel('resize', {
                        width: 200
                    });
                    var width = $("#" + f.name).width() + 200;
                    $("#" + f.name).dialog('resize', {
                        width: width
                    });
                    var t = new tree();
                    t.datasource = f.source;
                    t.datasource.selecttype = f.selecttype;
                    t.onSelect = function (node) {
                        var child = t.getchild(node);
                        var data = gettreedata(node, child);
                        f.grid.load(data);
                    }
                    t.width = "auto";
                    t.applyTo = f.treeid;
                    t.init();
                    $("#" + f.name + " .easyui-layout").each(function () {
                        $(this).layout();
                    });
                    $(sender).linkbutton('disable')
                }
                if (d.command == "clear") {
                    f.selectItem = false;
                    $("#" + f.name).dialog("close");
                }
                if (d.command == "ok") {
                    f.selectItem = true;
                    $("#" + f.name).dialog("close");
                }
            }
            if (f.commands == null) {
                tool.commands = [];
                var ok = new Object();
                ok.command = "ok";
                ok.visible = true;
                ok.disabled = false;
                ok.name = "选择";
                ok.icon = "icon-ok";
                ok.title = "确定选择的成员"
                var no = new Object();
                no.command = "clear";
                no.visible = true;
                no.disabled = false;
                no.name = "清除";
                no.icon = "icon-cancel";
                no.title = "清除选择的成员"
                tool.commands.push(ok);
                tool.commands.push(no);
            }
            else {
                tool.commands = [];
                for (var i in f.commands) {
                    tool.commands.push(f.commands[i]);
                }
            }
            if (f.istree && f.source.treemodel) {
                var a = new Object();
                a.command = "tree";
                a.visible = true;
                a.disabled = false;
                a.td = "search";
                a.name = "树查询";
                a.icon = "icon-add";
                a.title = "打开树查询状态"
                tool.commands.unshift(a);
            }
            tool.init();
        }
        f.grid.datasource = f.source;
        if (f.source.items.length > 4)
            f.grid.fitcolumns = false;
        f.grid.init();
        var name = f.menuname + f.title;
        var mo = true;
        var w = 600;
        var h = 500;
        if (f.istool == false) {
            name = "";
            mo = false;
            w = 500;
            h = 250;
        }
        $("#" + f.name).dialog({
            title: name,
            width: w,
            modal: mo,
            resizable: true,
            cache: false,
            height: h,
            left: f.left,
            top: f.top,
            onClose: function () {
                if (f.onClose) {
                    if (f.selectItem === true) {
                        f.selectItem = getselectrow(f);
                        f.onClose(f.selectItem);
                    }
                    if (f.selectItem === false) {
                        f.onClose(false);
                    }
                }
                f.isshow = false;
                $("#" + f.name).remove();
            },
            onResize: function (width, height) {
                f.grid.resize();
            }
        });
        $("#" + f.name).mouseenter(function () {
            f.enter = true;
        });
        $("#" + f.name).mouseleave(function () {
            f.leave = true;
            f.onmouseout();
        });
    }
    function getselectrow(f) {
        var obj;
        if (f.ischeck) {
            obj = f.grid.getcheckrows();
        }
        else {
            obj = f.grid.getselectrow();
        }
        return obj;
    }
    function gettreedata(node, child) {
        var datas = [];
        if (node && node.attributes) {
            var item = node.attributes;
            if (!item.ROWINDEX)
                item.ROWINDEX = 1;
            datas.push(item);
        }
        if (child) {
            var ci = 1;
            for (var i in child) {
                var item = child[i].attributes;
                if (item) {
                    if (!item.ROWINDEX) {
                        ci++;
                        item.ROWINDEX = ci;
                    }
                    datas.push(item);
                }
            }
        }
        return { rows: datas, total: datas.length };
    }
    function fnpage(f, fn) {
        var grid = f.grid;
        if (grid) {
            var page = grid.getpage();
            if (page) {
                var nu = grid.datasource.page;
                if (fn == "f")
                    nu = nu - 1;
                if (fn == "n")
                    nu = nu + 1;
                if (nu < 0)
                    nu = 0;
                $(page).pagination("select", nu);
            }
        }
    }
    function chtml(f) {
        var h = [];
        var height = '32px';
        if (f.istool == false) {
            height = '0px';
        }
        h.push('<div id="' + f.name + '">');
        if (f.istree && f.source.treemodel) {
            h.push('<div class="easyui-layout" fit="true" id="' + f.name + '-layout">');
            h.push('<div region="west" style="height:auto;width:0"><div id="' + f.treeid + '"/></div>');
            h.push('<div region="center">');
            h.push('<div class="easyui-layout" fit="true">');
            h.push('<div region="north" style="height:' + height + ';"><div id="' + f.toolid + '"/></div><div region="center"><div id="' + f.gridid + '"/></div>');
            h.push('</div></div></div></div>');
        }
        else {
            h.push('<div class="easyui-layout" fit="true">');
            h.push('<div region="north" style="height:' + height + ';"><div id="' + f.toolid + '"/></div><div region="center"><div id="' + f.gridid + '"/></div></div></div>');
        }
        var html = h.join('');
        $(html).appendTo('body');
        $("#" + f.name + " .easyui-layout").each(function () {
            $(this).layout();
        });
    }
    function load(f, v) {
        f.source.selecttype = f.selecttype;
        v = f.onWhere(f, v);
        var e = f.source.fwhere(v);
        var row;
        var item;
        if (f.fm) {
            var obj = f.fm();
            if (obj.row && obj.item) {
                row = obj.row;
                item = obj.item;
            }
            else
                row = obj;
        }
        if (f.fdata && f.fdata.eventrow)
            f.fdata.eventrow = webjs.jsonval(row);
        if (f.fdata) {
            f.json = "fdata=" + webjs.jsonval(f.fdata);
        }
        if (item) {
            f.json += "&ActionItem=" + webjs.jsonval(item);
        }
        f.source.submit(f.m, f.json + "&" + e, function (w) {
            if (w && w.error) {
                $.messager.alert('异常', w.errormsg, 'error');
                return;
            }
            f.source.onLoad(w);
        });
    }

    return {
        menuname: "查询",
        model: null,
        load: false,
        source: null,
        m: "",
        title: "",
        json: "",
        id: "",
        name: "",
        onClose: null,
        selectItem: null,
        ischeck: false,
        istree: true,
        commands: null,
        dbclose: true,
        where: "",
        treemodel: false,
        istool: true,
        left: null,
        top: null,
        isshow: false,
        show: function () {
            this.id = webjs.newid();
            this.name = "searchform-" + this.id;
            this.treeid = "searchform-" + this.id + "-tree";
            this.toolid = "searchform-" + this.id + "-toolstrip";
            this.gridid = "searchform-" + this.id + "-datagrid";
            if (this.model == null) {
                alert("异常：model属性为空！");
                return;
            }
            if (this.source == null)
                this.source = new datasource();
            this.source.setmodel(this.model);
            sinit(this);
            if (this.load) {
                load(this, null);
            }
            this.isshow = true;
        },
        sload: function (w) {
            this.where = w;
            load(this, w);
        },
        reload: function () {
            load(this, this.where);
        },
        val: function () {
            return getselectrow(this);
        },
        onLoading: function (source, row) {

        },
        onWhere: function (sender, where) {
            return where;
        },
        close: function () {
            $("#" + this.name).dialog("close");
        },
        move: function (left, top) {
            $("#" + this.name).dialog("move", { left: left, top: top });
            $("#" + this.name).dialog("refresh");
        },
        remove: function () {
            $("#" + this.name).dialog('destroy');
        },
        up: function () {
            if (this.grid)
                this.grid.selectup();
        },
        down: function () {
            if (this.grid)
                this.grid.selectdown();
        },
        onmouseout: function () {

        },
        select: function () {
            this.selectItem = true;
            $("#" + this.name).dialog("close");
        },
        nextpage: function () {
            fnpage(this, "n");
        },
        firstpage: function () {
            fnpage(this, "f");
        }
    }
}