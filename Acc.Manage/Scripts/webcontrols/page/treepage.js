var treepage = function () {
    function cinit(t) {
        t.id = webjs.newid();
        t.datasource = setsource(t);
        t.datasource.p = t;
        cwindows(t, t.datasource.modeldata.title);
        var r = new tree();
        r.datasource = t.datasource;
        r.applyTo = t.id + '-west';
        r.width = "auto";
        r.ismenu = true;
        r.init();

        var tool = settoolstrip(t);
        var f = setchildform(t);
        t.tool = tool;
        t.from = f;
        t.tree = r;
        setcommand(tool, r, f, t);
        r.onSelect = function (node) {
            t.node = node;
            f.val(node.attributes);
            t.onSelect(node);
            //tool.datasource.selectItem = node.attributes;
            t.datasource.selectItem = node.attributes;
        }

        //回收站还原
        t.datasource.onReduction = function (rows) {
            var row;
            for (var i in rows) {
                row = rows[i];
                var id = row[r.parentid];
                if (id && id > 0) {
                    r.expand(id);
                }
                else {
                    var node = r.formatnode(row);
                    if (node) {
                        r.addroot(node);
                    }
                }
            }
        }

        $("#" + t.id + '-center').panel({
            onResize: function (width, height) {
                f.resize(width, height);
            }
        });
        f.setdisabled(true);
    }
    function setsource(t) {
        var source = new datasource();
        source.c = t.c;
        source.init();
        if (source.modeldata) {
            for (var n in source.modeldata.childitem) {
                var citem = source.modeldata.childitem[n];
                if (citem.field == "ROWINDEX") {
                    citem.visible = false;
                    break;
                }
            }
        }
        source.onFormSelect = function (form, box, item) {
            t.onFormSelect(form, box, item);
        }
        source.onSelectTab = function (index, grid) {
            t.onSelectTab(index, grid.parent, grid);
        }
        source.onForeigned = function (sender, fdata, rows) {
            t.onForeigned(sender, fdata, rows);
        }

        if (source.commands != null && source.commands.length > 0) {
            var cmd;
            for (var c in source.commands) {
                cmd = source.commands[c];
                if (cmd.command == "add" && cmd.visible == true && cmd.disabled == false)
                    t.issave = true;
                if (cmd.command == "edit" && cmd.visible == true && cmd.disabled == false)
                    t.issave = true;
            }
        }
        if (t.issave) {
            var a = {};
            a.command = "addroot";
            a.visible = true;
            a.disabled = false;
            a.name = "新增根" + source.modeldata.title;
            a.icon = "icon-add";
            a.title = "新增最顶级的" + source.modeldata.title;
            source.commands.unshift(a);

            var obj = {};
            obj.command = "undo";
            obj.visible = true;
            obj.disabled = true;
            obj.name = "撤消";
            obj.icon = "icon-undo";
            obj.title = "撤消操作";
            var save = {};
            save.command = "save";
            save.visible = true;
            save.disabled = true;
            save.name = "保存";
            save.icon = "icon-save";
            save.title = "保存操作";
            source.commands.push(obj);
            source.commands.push(save);
        }
        return source;
    }
    function setcommand(tool, r, f, t) {
        var source = t.datasource;
        tool.clickcommand = function (sender, data) {
            //f.m = data.command;
            switch (data.command) {
                case "addroot":
                    source.m = "add";
                    f.m = "add";
                    r.addroot();
                    setdisabled(tool, true, f, r);
                    t.onCommandClick(data.command, f, null);
                    break;
                case "add":
                    source.m = data.command;
                    f.m = "add";
                    r.add();
                    setdisabled(tool, true, f, r);
                    t.onCommandClick(data.command, f, null);
                    break;
                case "edit":
                    source.m = data.command;
                    f.m = "edit";
                    setdisabled(tool, true, f, r);
                    t.onCommandClick(data.command, f, source.selectItem);
                    break;
                case "remove":
                    var res = t.onCommandClick(data.command, f, source.selectItem);
                    if (res === false)
                        return;
                    if (t.node.id > 0) {
                        source.m = data.command;
                        var e = source.post(source.m, "ActionItem=" + webjs.jsonval(source.selectItem));
                        if (e && e.error) {
                            $.messager.alert('异常', e.errormsg, 'error');
                        }
                        else {
                            r.remove();
                            if (source.selectItem == null)
                                f.clear();
                            if (t.onSubmited) {
                                var res = t.onSubmited('remove', e);
                                if (res === false)
                                    return;
                            }
                            $.messager.show({ title: '消息', msg: '删除完成。', showType: 'show' });
                        }
                    }
                    break;
                case "undo":
                    try {
                        
                        setdisabled(tool, false, f, r);
                        if (t.node.id == -1) {
                            r.remove();
                        }
                        f.rejectChanges();
                        f.m = "";
                    }
                    finally {
                        // setdisabled(tool, false, f, r);
                    }
                    break;
                case "save":
                    var data = f.val();
                    if (source.selectItem != null) {
                        for (var i in data)
                            source.selectItem[i] = data[i];
                    }
                    else
                        source.selectItem = data;
                    var res = t.onSaveing(f.m, source.selectItem);
                    if (res === false)
                        return;
                    var p = null;
                    if (t.node.id == -1) {
                        p = r.getparent(t.node);
                    }
                    var e = source.post(f.m, "ActionItem=" + webjs.jsonval(source.selectItem));
                    if (e && e.error) {
                        $.messager.alert('异常', e.errormsg, 'error');
                    }
                    else {
                        if (e == undefined || e == null)
                            e = source.selectItem;
                        setdisabled(tool, false, f, r);
                        f.m = "";
                        t.node = r.saveupdate(e);
                        if (t.onSubmited) {
                            var res = t.onSubmited(f.m, e);
                            if (res === false)
                                return;
                        }
                        if (p != null)
                            r.select(p.id);
                        // else
                        // r.select(t.node.id);
                        $.messager.show({ title: '消息', msg: '保存完成。', showType: 'show' });
                    }
                    break;
                default:
                    var s = this;
                    if (s.datasource) {
                        s.datasource.onSubmited = function (cmd, data, item) {
                            if (cmd == "startprint") {
                                var pdf = new spdf();
                                pdf.file = item.results;
                                pdf.title = source.modeldata.title;
                                pdf.show();
                                data.title = "打印预览";
                            }
                            if (cmd == "pushdown") {
                                if (data.comm && data.comm.Tag) {
                                    var so = new datasource();
                                    so.c = data.comm.Tag;
                                    so.init();
                                    data.title = "下推";
                                    var win = so.openwin(data, "add");
                                    win.val(item);
                                }
                            }
                            t.onSubmited(cmd, item);
                        }
                        s.datasource.notreload = true;
                        s.datasource.clickcommand(data.command, data);
                        //f.m = "";
                        r.saveupdate(source.selectItem);
                    }
                    break;
            }
        }
        source.onCommand = function (name, data) {
            if (name == "add") {
                return false;
            }
            if (name == "edit") {
                return false;
            }
            return true;
        }
    }
    function settoolstrip(t) {
        var tool = new toolstrip();
        tool.applyTo = t.id + '-menu';
        tool.datasource = t.datasource;
        tool.issearch = false;
        tool.init();
        return tool;
    }
    function setdisabled(t, dis, f, r) {
        var d = dis ? 'disable' : 'enable';
        var e = dis ? 'enable' : 'disable';
        f.setdisabled(!dis);
        r.disabled = dis;
        for (var c in t.commands)
            $("#" + t.id + "-" + t.commands[c].command).linkbutton(d);
        $("#" + t.id + "-undo").linkbutton(e);
        $("#" + t.id + "-save").linkbutton(e);
    }
    function setchildform(t) {
        var f = new childform();
        f.childitem = t.datasource.modeldata.childitem;
        f.parent = t.datasource;
        t.datasource.onAfterEdit = function (rowIndex, rowData, changes) {
            t.onAfterEdit(rowIndex, rowData, changes);
        }
        f.fit = true;
        f.childfit = t.childfit;
        f.applyTo = t.id + '-form';
        f.init();
        return f;
    }
    function cwindows(tree, title) {
        tree.name = tree.id + "-div";
        var id = webjs.newid();
        $("#" + tree.applyTo).attr("id", tree.name);
        var div = $("#" + tree.id + "-div").panel({ fit: true });
        var layout = $('<div class="easyui-layout" fit="true" border="false"></div>').appendTo(div);
        var west = $('<div region="west" split="true" border="true" style="width:200px" id="' + tree.id + '-west" title="' + title + '"></div>').appendTo(layout);
        var center = $('<div region="center" border="false" id="' + tree.id + '-center"><div id="' + id + '" border="false" class="easyui-layout" fit="true"><div border="true" region="north" style="height:30px;"><div id="' + tree.id + '-menu"/></div><div border="false" region="center"><div id="' + tree.id + '-form"/></div></div></div>').appendTo(layout);
        $(layout).layout();
        $("#" + id).layout();
    }
    return {
        c: "",
        applyTo: "",
        issave: false,
        datasource: null,
        childfit: false,
        showfooter: true,
        init: function () {
            try {
                cinit(this);
            }
            catch (e) {
                $.messager.alert('异常', e, 'error');
            }
        },
        //树选择事件
        //item:选中的树节点数据
        onSelect: function (item) {

        },
        //Form编辑界面中的select事件
        //from选择的界面，box发生事件的控件,item选择的数据成员
        onFormSelect: function (form, box, item) {

        },
        //子模型Grid行编辑事件
        //index:编辑的行索引
        //item:行数据
        //changes:改变的属性成员
        onAfterEdit: function (index, item, changes) {

        },
        //命令点击事件(保存按钮不会执行该事件)
        //command:命令
        //form:已打开的可用窗体，如命令不含窗体则为null
        //item:命令执行的数据成员
        onCommandClick: function (command, form, item) {

        },
        //保存前事件
        //command:保存时执行的命令
        //item:待保存的成员
        onSaveing: function (command, item) {

        },
        //提交到服务端完成事件
        //command:提交服务时执行的命令
        //item:服务端执行命令完成返回的数据
        onSubmited: function (command, item) {

        },
        //选择Tab事件
        onSelectTab: function (index, form, grid) {

        },
        //外键选择完成
        //sender:执行外键选择的控件
        //fdata：外键对象
        //rows:已选择的数据
        onForeigned: function (sender, fdata, rows)
        { }
    }
}