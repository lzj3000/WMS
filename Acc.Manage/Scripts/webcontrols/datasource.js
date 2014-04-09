var datasource = function () {
    function setcm(ds, items) {
        var cm = [];
        var ikey = 0;
        var cmc = 0;
        var scm = [];
        var iscm = 1;
        var keys = [];
        scm[0] = { field: "*", title: "全部" };
        for (var i = 0; i < items.length; i++) {
            if (items[i].visible) {
                cm[cmc++] = { field: items[i].field, title: items[i].title, width: 120, align: 'center', sortable: true, editor: null };
                if (items[i].issearch)
                    scm[iscm++] = { field: items[i].field, title: items[i].title };
            }
            if (items[i].iskey) {
                keys[ikey] = items[i].field;
                ikey++;
            }
            if (items[i].foreign && items[i].foreign.isfkey) {
                gettreemodel(ds, items[i].foreign);
            }
        }
        ds.searchitems = scm;
        ds.keys = keys;
        ds.items = cm;
        setmodel(ds, ds.modeldata);
    }
    function setmodel(ds, model) {
        if (model && model.childmodel.length > 0) {
            for (var i = 0; i < model.childmodel.length; i++) {
                var m = model.childmodel[i];
                m.parent = model;
                setmodel(ds, m);
            }
        }
    }
    function gettreemodel(ds, foreign) {
        if (foreign.objectname == foreign.foreignobject && foreign.isassociate) {
            var obj = {};
            obj.displayid = foreign.foreignfiled;
            obj.displayname = foreign.displayname;
            obj.displayfield = foreign.displayfield;
            obj.parentid = foreign.filedname;
            obj.modelname = foreign.objectname;
            obj.tablename = foreign.tablename;
            obj.foreignobject = foreign.foreignobject;
            ds.treemodel = obj;
        }
        return false;
    }
    function getwhere(ds) {
        ds.selectItem = null;
        var data = {};
        if (ds.selecttype) {
            data.selecttype = ds.selecttype;
        }
        data.page = ds.page;
        data.rows = ds.rows;
        data.whereList = [];
        if (ds.sort && ds.sort != "") {
            data.sort = ds.sort;
        }
        if (ds.where) {
            if (typeof ds.where === "string") {
                var and = ds.where.split(' and ');
                for (var a in and) {
                    var dh = and[a].split('=');
                    var where = {};
                    if (dh.length == 2) {
                        where.ColumnName = dh[0];
                        where.Value = dh[1];
                        data.whereList.push(where);
                    }
                }
            }
            else {
                if (typeof ds.where === "object" && typeof ds.where.length === "number") {
                    for (var i in ds.where) {
                        data.whereList.push(ds.where[i]);
                    }
                }
                else
                    data.whereList.push(ds.where);
            }

        }
        return "LoadItem=" + webjs.jsonval(data);
    }
    function openform(ds, data, name) {
        var f = new childform();
        f.title = ds.modeldata.title + data.title;
        f.childitem = ds.modeldata.childitem;
        f.parent = ds;
        f.childfooter = ds.ischildfooter;
        f.onSelectTab = function (index, grid) {
            if (ds.onSelectTab) {
                ds.onSelectTab(index, grid);
            }
        }
        if (name == 'add') {
            f.CommandClose = true;
        }
        f.m = name;
        if (data.save != undefined && data.save == false)
            f.showSave = false;
        if (ds.onOpenWin) {
            var cmd = name;
            if (data.command)
                cmd = data.command;
            ds.onOpenWin(f, data, cmd);
        }
        f.show();
        if (ds.onOpenWinShow) {
            var cmd = name;
            if (data.command)
                cmd = data.command;
            ds.onOpenWinShow(f, data, cmd);
        }
        if (ds.onCommandClick) {
            ds.onCommandClick(name, f, ds.selectItem);
        }
        return f;
    }
    function storeget(d, m) {
        var sto = store.get(d.c);
        if (sto != null) {
            var ooo = JSON.parse(sto);
            if (ooo.m == m)
                return ooo.v;
        }
        return null;
    }
    return {
        c: "",
        commands: null,
        modeldata: null,
        searchitems: null,
        keys: null,
        items: null,
        sort: "",
        page: 1,
        rows: 10,
        where: "",
        author: "",
        selectItem: null,
        searchmodel: false,
        treemodel: false,
        ischildgrid: true,
        ischildform: true,
        IsClearAway: false,
        IsPrint: false,
        init: function () {
            this.m = "init";
            var ds = this;
            var data = webjs.post(ds.c, ds.m, null);
            if (data && data.error) {
                throw data.errormsg;
            }
            if (data && data.name == ds.c) {
                ds.commands = data.commands;
                ds.modeldata = data.modeldata;
                ds.title = data.title;
                ds.description = data.description;
                ds.IsClearAway = data.IsClearAway;
                ds.IsPrint = data.IsPrint;
                ds.author = data.author;
                ds.url = data.url;
                if (ds.modeldata && ds.modeldata.childitem)
                    setcm(ds, ds.modeldata.childitem);
            }
        },
        openwin: function (data, name) {
            return openform(this, data, name);
        },
        setmodel: function (model) {
            this.modeldata = model.modeldata;
            this.c = model.name;
            setcm(this, this.modeldata.childitem);
        },
        getitem: function (name) {
            var ds = this;
            if (ds.modeldata && ds.modeldata.childitem) {
                var items = ds.modeldata.childitem;
                for (var i = 0; i < items.length; i++) {
                    if (items[i].field == name)
                        return items[i];
                }
            }
            return false;
        },
        load: function (where) {
            this.m = "load";
            if (where != this.where) {
                this.where = where;
                this.sort = "";
                this.page = 1;
            }
            var wd = getwhere(this);
            var ds = this;
            var data = webjs.post(ds.c, ds.m, wd);
            if (data && data.error) {
                $.messager.alert('异常', data.errormsg, 'error');
            }
            else {
                this.onLoad(data);
                return data;
            }
        },
        fwhere: function (w) {
            this.where = w;
            return getwhere(this);
        },
        onLoad: function (data) {

        },
        onPrev: function () {
        },
        onNext: function () {
        },
        reload: function () {
            this.m = "load";
            var ds = this;
            var data = getwhere(ds);
            webjs.mysubmit(ds.c, ds.m, data,
                function (e) {
                    throw e;
                }, function (e) {
                    ds.onLoad(e);
                });
        },
        sortcolumn: function (sort, ad) {
            this.sort = sort + " " + ad;
            this.reload();
        },
        dblclickrow: function (i, data) {
            this.selectItem = data;
            var d = {};
            d.title = "编辑成员";
            for (var c in this.commands) {
                if (this.commands[c].command == "edit") {
                    if (this.commands[c].disabled || this.commands[c].visible == false) {
                        d.save = false;
                        break;
                    }
                }
            }
            this.clickcommand("edit", d);
        },
        clickrow: function (i, data) {
            this.selectItem = data;
            if (this.onSelect)
                this.onSelect(this.selectItem);
        },
        onSelect: function (item)
        { },
        onFormSelect: function (form, box, item) {

        },
        onCommand: function (name, data) {
            return true;
        },
        onchildAdding: function (form, grid, row) {

        },
        clickcommand: function (name, data) {
            if (!this.onCommand(name, data)) return;
            switch (name) {
                case "load":
                    this.load(data.value);
                    break;
                case "copy":
                    var f = openform(this, data, 'add');
                    f.val(this.selectItem);
                    break;
                case "add":
                    var win = openform(this, data, name);
                    var data = storeget(this, "add");
                    if (data != null) {
                        $.messager.confirm("提示", '检测到有未完成保存的数据，是否重新加载？', function (ok) {
                            if (ok) {
                                win.val(data);
                            }
                        });
                    }
                    break;
                case "edit":
                    openform(this, data, name);
                    break;
                case "save":
                    if (data.m) {
                        var sdata = data.val();
                        var source = this;
                        if (source.selectItem && data.m == 'edit') {
                            var old = {};
                            for (var n in source.selectItem)
                                old[n] = source.selectItem[n];
                            for (var n in sdata)
                                old[n] = sdata[n];
                            sdata = old;
                        }
                        if (source.onSaveing) {
                            var res = source.onSaveing(data.m, sdata);
                            if (res === false)
                                return;
                        }
                        var str = webjs.jsonval(sdata);
                        //                        str = this.encode(str);
                        var e = this.post(data.m, "ActionItem=" + str);
                        if (e && e.error) {
                            throw e.errormsg;
                        }
                        else
                            store.remove(this.c);
                        source.reload();
                        if (source.onSubmited) {
                            var res = source.onSubmited(data.m, data, e);
                            if (res === false)
                                return;
                        }
                        $.messager.show({ title: '消息', msg: '保存完成。', showType: 'show' });
                    }
                    break;
                case "remove":
                    var source = this;
                    if (source.onCommandClick) {
                        var res = source.onCommandClick('remove', null, source.selectItem);
                        if (res === false)
                            return;
                    }
                    if (source.selectItem) {
                        var str = webjs.jsonval(source.selectItem);
                        //                        str = this.encode(str);
                        var e = source.post('remove', "ActionItem=" + str);
                        if (e && e.error) {
                            $.messager.alert('异常', e.errormsg, 'error');
                        }
                        else {
                            source.reload();
                            if (source.onSubmited) {
                                var res = source.onSubmited('remove', data, e);
                                if (res === false)
                                    return;
                            }
                            $.messager.show({ title: '消息', msg: '删除完成。', showType: 'show' });
                        }
                    }
                    break;
                default:
                    var s = this;
                    if (s.onCommandClick) {
                        var res = s.onCommandClick(name, null, s.selectItem);
                        if (res === false)
                            return;
                    }
                    var d = "ActionItem=" + webjs.jsonval(s.selectItem);
                    if (data.comm && data.comm.Tag) {
                        d += "&Tag=" + data.comm.Tag;
                    }
                    s.submit(name, d, function (e) {
                        data.comm.isclicking = false;
                        if (e && e.error) {
                            $.messager.alert('异常', e.errormsg, 'error');
                        }
                        else {
                            if (!s.notreload)
                                s.reload();
                            if (s.onSubmited) {
                                var res = s.onSubmited(name, data, e);
                                if (res === false)
                                    return;
                            }
                            $.messager.show({ title: '消息', msg: data.title + '完成。', showType: 'show' });
                        }
                    });
                    break;
            }

        },
        selectpage: function (page, rows) {
            this.page = page;
            this.rows = rows;
            this.reload();
        },
        submit: function (m, data, f) {
            var ds = this;
            ds.m = m;
            webjs.mysubmit(ds.c, ds.m, data, f, f);
        },
        post: function (m, data) {
            var ds = this;
            ds.m = m;
            var d = webjs.post(ds.c, ds.m, data);
            return d;
        },
        onForeigned: function (sender, fdata, rows) {

        },
        onforeignform: function (sender, value, f) {
            var s = new searchform();
            var data = "fdata=" + webjs.jsonval(value);
            var sou = this;
            var e = sou.post("onforeign", data);
            if (e && e.error) {
                $.messager.alert('异常', e.errormsg, 'error');
                return;
            }
            s.model = e;
            s.m = "loadforeign";
            s.title = e.modeldata.title;
            s.fdata = value;
            s.selecttype = value.foreignobject;
            s.fm = f;
            s.parent = sou;
            s.istool = false;
            s.onClose = function (d) {
                if (d != null) {
                    if (d === false) {
                        sender.clear();
                        return;
                    }
                    if (sender.foreign && sender.foreign.isfkey) {
                        sender.cval(d);
                        for (var n in sender.foreign.rowdisplay) {
                            var fn = n.toUpperCase();
                            var zn = sender.foreign.rowdisplay[n].toUpperCase();
                            var obj = new Object();
                            obj[zn] = d[fn];
                            for (var i in e.modeldata.childitem) {
                                var item = e.modeldata.childitem[i];
                                if (item.field === fn) {
                                    if (item.foreign && item.foreign.isfkey) {
                                        for (var i in this.parent.modeldata.childitem) {
                                            var znitem = this.parent.modeldata.childitem[i];
                                            if (znitem.field === zn) {
                                                if (item.foreign && item.foreign.isfkey) {
                                                    obj[znitem.foreign.displayname] = d[item.foreign.displayname];
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                            sender.parent.setforeigndisplay(obj);
                        }
                    }
                    else {
                        var fv = d[value.foreignfiled]
                        var fn = d[value.displayname];
                        value.filedvalue = fv;
                        value.displayvalue = fn;
                        sender.val(fn);
                    }
                    sou.onForeigned(sender, value, d);
                }
            };
            return s;
        },
        onforeign: function (sender, value, f) {
            var data = "fdata=" + webjs.jsonval(value);
            var sou = this;
            sou.submit("onforeign", data, function (e) {
                if (e && e.error) {
                    $.messager.alert('异常', e.errormsg, 'error');
                    return;
                }
                var s = new searchform();
                s.model = e;
                s.m = "loadforeign";
                s.title = e.modeldata.title;
                s.fdata = value;
                s.selecttype = value.foreignobject;
                s.fm = f;
                s.parent = sou;
                s.onClose = function (d) {
                    if (d != null) {
                        if (d === false) {
                            sender.clear();
                            return;
                        }
                        if (sender.foreign && sender.foreign.isfkey) {
                            sender.cval(d);
                            for (var n in sender.foreign.rowdisplay) {
                                var fn = n.toUpperCase();
                                var zn = sender.foreign.rowdisplay[n].toUpperCase();
                                var obj = new Object();
                                obj[zn] = d[fn];
                                for (var i in e.modeldata.childitem) {
                                    var item = e.modeldata.childitem[i];
                                    if (item.field === fn) {
                                        if (item.foreign && item.foreign.isfkey) {
                                            for (var i in this.parent.modeldata.childitem) {
                                                var znitem = this.parent.modeldata.childitem[i];
                                                if (znitem.field === zn) {
                                                    if (item.foreign && item.foreign.isfkey) {
                                                        obj[znitem.foreign.displayname] = d[item.foreign.displayname];
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                                sender.parent.setforeigndisplay(obj);
                            }
                        }
                        else {
                            var fv = d[value.foreignfiled]
                            var fn = d[value.displayname];
                            value.filedvalue = fv;
                            value.displayvalue = fn;
                            sender.val(fn);
                        }
                        sou.onForeigned(sender, value, d);
                    }
                };
                s.show();
            });
        },
        getcmindex: function (name) {
            for (var i in this.modeldata.childmodel) {
                if (this.modeldata.childmodel[i].name == name)
                    return i;
            }
            return null;
        },
        getcmsource: function (index, clone) {
            var cm = this.modeldata.childmodel[index];
            if (clone) {
                var model = new Object();
                model.modeldata = cm
                model.name = this.c;
                var source = new datasource();
                source.setmodel(model);
                return source;
            }
            else {
                if (cm && cm.source == null) {
                    var model = new Object();
                    model.modeldata = cm
                    model.name = this.c;
                    cm.source = new datasource()
                    cm.source.setmodel(model);
                }
                return cm.source;
            }
        },
        getforeign: function (index) {
            var cm = this.modeldata.childmodel[index];
            for (var i in cm.childitem) {
                var item = cm.childitem[i];
                if (item.foreign && item.foreign.isfkey) {
                    if (item.foreign.foreignobject == cm.parent.name) {
                        return item.foreign;
                    }
                }
            }
            return null;
        },
        getforeignwhere: function (index, row) {
            var where = "";
            var cm = this.modeldata.childmodel[index];
            for (var i in cm.childitem) {
                var item = cm.childitem[i];
                if (item.foreign && item.foreign.isfkey) {
                    if (item.foreign.foreignobject == cm.parent.name) {
                        if (where == "")
                            where = item.foreign.parenttablename + "." + item.field + "=" + row[item.foreign.foreignfiled];
                        else
                            where += " and " + item.foreign.parenttablename + "." + item.field + "=" + row[item.foreign.foreignfiled];
                    }
                }
            }
            if (cm.where) {
                where += " and " + cm.where;
            }
            return where;
        },
        loadforeign: function (cmindex, row, w, f) {
            var source = this.getcmsource(cmindex);
            source.reload = function () {
                return source.load(source.where);
            }
            var where = this.getforeignwhere(cmindex, row);
            var fdata = this.getforeign(cmindex);
            if (fdata)
                fdata.eventrow = webjs.jsonval(row);
            if (w != null) {
                where += " and " + w;
            }
            source.selecttype = source.modeldata.name;
            if (this.page == "-1")
                source.page = "-1";
            else
                source.page = 1;
            var data = 'fdata=' + webjs.jsonval(fdata) + "&" + source.fwhere(where);
            source.tempfdata = 'fdata=' + webjs.jsonval(fdata) + "&" + "ActionItem=" + webjs.jsonval(source.selectItem);
            source.load = function (w) {
                var where = source.fwhere(w);
                var dataw = source.tempfdata + "&" + where;
                var e = source.post("loadforeign", dataw);
                source.onLoad(e);
            }
            if (f == undefined || f == null) {
                var e = source.post("loadforeign", data);
                return e;
            }
            else {
                source.submit("loadforeign", data, function (e) {
                    f(e);
                });
            }
        },
        treeload: function (where, f) {
            if (where != this.where) {
                this.where = where;
            }
            var wd = getwhere(this);
            if (f != undefined) {
                this.submit('treeload', wd, f);
            }
            else {
                var rows = this.post('treeload', wd);
                return rows;
            }
        },
        encode: function (str) {
            if (typeof str == "string") {
                //                if (str.indexOf("%") == -1)
                str = encodeURIComponent(str);
            }
            else {
                if (typeof str == "object") {
                    for (var n in str) {
                        str[n] = this.encode(str[n]);
                    }
                }
            }
            return str;
        }
    }
}

