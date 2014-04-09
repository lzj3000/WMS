var datagrid = function () {
    function initgrid(grid) {
        var name = grid.name;
        var cls = ccolumns(grid);
        var isfoot = setfooter(grid);
        $("#" + name).datagrid({
            cache: false,
            showFooter: isfoot,
            fit: grid.fit,
            fitColumns: grid.fitcolumns,
            sortOrder: "desc",
            columns: [cls],
            rownumbers: true,
            singleSelect: !grid.check,
            loadMsg: "数据读取中，请稍后。。。",
            height: grid.height,
            onSortColumn: function (sort, col) {
                if (sort != "ROWINDEX") {
                    if (grid.sortcolumn) {
                        grid.sortcolumn(sort, col);
                    }
                }
            },
            onSelect: function (rowIndex, rowData) {
                grid.selectIndex = rowIndex;
                if (grid.clickrow) {
                    grid.clickrow(rowIndex, rowData);
                }
            },
            onDblClickRow: function (rowIndex, rowData) {
                grid.selectIndex = rowIndex;
                if (grid.dblclickrow)
                    grid.dblclickrow(rowIndex, rowData);
            },
            onClickRow: function (rowIndex, rowData) {
                grid.selectIndex = rowIndex;
                //                if (grid.clickrow) {
                //                    grid.clickrow(rowIndex, rowData);
                //                }
            }
        });
    }
    function parentOrder(row, value) {
        if (row.SOURCEID && row.SOURCECONTROLLER && value != null) {
            return '<a href="#" onclick="javascript:webjs.showform(\'' + row.SOURCECONTROLLER + '\',\'' + row.SOURCEID + '\')">' + value + '<a>';
        }
        return value;
    }
    function ccolumns(grid) {
        csource(grid);
        var rows = [];
        var n = 0;
        if (grid.check)
            rows[0] = { field: "ck", checkbox: "true" };
        for (var i = 0; i < grid.items.length; i++) {
            if (grid.check)
                n = i + 1;
            else
                n = i;

            rows[n] = grid.items[i];
            var field = rows[n].field;
            if (field === "ROWINDEX")
                rows[n].width = 60;
            rows[n].align = "center";
            var fitem = grid.datasource.getitem(field)
            if (fitem.isedit) {
                var box = new textbox();
                box.type = fitem.type;
                box.field = fitem.field;
                box.readonly = fitem.readonly;
                box.required = fitem.required;
                box.disabled = fitem.disabled;
                box.comvtp = fitem.comvtp;
                box.foreign = fitem.foreign;
                box.parent = grid;
                var ed = {};
                ed.type = "text";
                ed.options = box;
                rows[n].editor = ed;

            }

            rows[n].formatter = function (value, row, index) {
                if (grid.datasource.onFormater) {
                    var obj = {};
                    obj.grid = grid;
                    obj.item = this;
                    obj.value = value;
                    obj.row = row;
                    obj.index = index;
                    obj.field = this.field;
                    obj.title = this.title;
                    var v = grid.datasource.onFormater(obj);
                    if (v != undefined)
                        return v;
                    else
                        return value;
                }
                return value;
            }

            if (fitem.type == "bool") {
                rows[n].formatter = function (value, row, index) {
                    if (row.footer)
                        return value;
                    var b = false;
                    if (value === 1 || value === "1" || value === true || value === "true")
                        b = true;
                    if (b === true)
                        return "是";
                    else
                        return "否";

                }
            }
            if (fitem.type == 'date') {

                //1900-01-01T00:00:00
                rows[n].formatter = function (value, row, index) {
                    if (row.footer)
                        return value;
                    if (value == '1900-01-01T00:00:00')
                        return "";
                    else
                        return value;
                }
            }
            var combox = grid.combox[field];
            var foreign = grid.foreign[field];
            if (combox) {
                rows[n].formatter = function (value, row, index) {
                    if (row.footer)
                        return value;
                    var tt = this.field;
                    var com = grid.combox[tt];
                    if (value !== "" && com) {
                        for (var i in com.items) {
                            if (com.items[i].Value == value)
                                return com.items[i].Text;
                        }
                    }
                }
            }
            if (foreign) {
                rows[n].formatter = function (value, row, index) {
                    if (row.footer)
                        return value;
                    var tt = this.field;
                    if (grid.foreign[tt]) {
                        var fd = grid.foreign[tt];
                        if (fd.displayname != null && fd.displayname != "") {
                            var d = row[fd.displayname];
                            return d;
                        }
                        else
                            return value;
                    }
                    else
                        return value;
                }
            }
            if (field == "SOURCECODE") {
                rows[n].formatter = function (value, row, index) {
                    return parentOrder(row, value);
                }
            }
            if (grid.onInitRow) {
                grid.onInitRow(rows[n]);
            }
        }
        return rows;
    }
    function cpage(grid) {
        $("#" + grid.name).datagrid({ pagination: true });
        grid.pageinit = true;
        var p = $("#" + grid.name).datagrid('getPager');
        if (p) {
            $(p).pagination({
                beforePageText: "页",
                afterPageText: "共{pages}页",
                displayMsg: "显示{from}到{to}行  共{total}行",
                onSelectPage: function (page, rows) {
                    grid.pageindex = page;
                    if (grid.selectpage) {
                        grid.selectpage(page, rows);
                    }
                },
                onBeforeRefresh: function (page, rows) {
                    var ge = grid.isrowedit();
                    if (ge) {
                        $.messager.confirm('查询', '检测到有编辑数据未保存，请确认要执行查询操作吗？如操作将清除未保存数据！', function (ok) {
                            grid.pageindex = page;
                            if (grid.selectpage) {
                                grid.selectpage(page, rows);
                            }
                        });
                    }
                }
            });
            if (grid.buttondisabled)
                grid.setdisabled(grid.buttondisabled);
        }
    }
    function cselect(g, i) {
        var row = $("#" + g.name).datagrid("getSelected");
        if (row) {
            var index = $("#" + g.name).datagrid("getRowIndex", row);
            var ix = index + i;
            var count = $("#" + g.name).datagrid('getRows').length;
            if (ix >= count)
                ix = count - 1;
            if (ix > -1) {
                $("#" + g.name).datagrid("selectRow", ix);
            }
            row = $("#" + g.name).datagrid("getSelected");
        }
        return row;
    }
    function csource(g) {
        if (g.datasource) {
            g.keys = g.datasource.keys;
            g.items = g.datasource.items;
            g.modelname = g.datasource.modeldata.name;
            for (var n in g.datasource.modeldata.childitem) {
                var i = g.datasource.modeldata.childitem[n];
                if (i.comvtp != null && i.comvtp.isvtp)
                    g.combox[i.field] = i.comvtp;
                if (i.foreign != null && i.foreign.isfkey) {
                    g.foreign[i.field] = i.foreign;
                }
            }

            g.datasource.onLoad = function (data) {
                if (!g.pageinit) {
                    if (data.total > 10)
                        g.pagination();
                }
                g.load(data);
            };
            g.datasource.onPrev = function () {
                g.datasource.selectItem = cselect(g, -1);
            };
            g.datasource.onNext = function () {
                g.datasource.selectItem = cselect(g, 1);
            };
        }
    }
    function gridremove(g, row, check) {
        var name = "#" + g.name;
        if (row) {
            if (row.StateBase === undefined || row.StateBase === 3) {
                row.StateBase = 2;
                g.deleteRows.push(row);
            }
            removeUpdateRows(g, row);
            var index = $(name).datagrid('getRowIndex', row);
            if (!check) {
                $(name).datagrid('deleteRow', index);
                $(name).datagrid('acceptChanges');
                //$(name).datagrid('rejectChanges');
                var t = index - 1;
                if (t < 0) t = 0;
                return t;
            }
            return index;
        }
    }
    function gridinsert(g, obj, index) {
        var name = "#" + g.name;
        obj.StateBase = 0;
        obj.ido = webjs.newid();
        var rows = g.getrows();
        var ri = 0;
        if (rows && rows.length) {
            ri = rows.length + 1;
        }
        else
            ri = 1;
        obj.ROWINDEX = ri;
        if (g.onAdding) {
            g.onAdding(obj);
        }
        var cou = 0;
        if (index != undefined) {
            $(name).datagrid('insertRow', { index: index, row: obj });
            cou = index;
        }
        else {
            if (g.isdownadd) {
                $(name).datagrid('appendRow', obj);
                cou = $(name).datagrid('getRows').length - 1;
            }
            else
                $(name).datagrid('insertRow', { index: 0, row: obj });
        }
        $(name).datagrid('acceptChanges');
        $(name).datagrid('rejectChanges');
        return cou;
    }
    function gjaddoredit(g, a) {
        var s = g.datasource;
        if (s && s.modeldata && s.modeldata.childmodel && s.modeldata.childmodel.length > 0) {
            for (var i in s.modeldata.childmodel) {
                var cm = s.modeldata.childmodel[i];
                if (a == 'add' && !cm.disabled && cm.isadd) {
                    return true;
                }
                if (a == 'edit' && !cm.disabled && cm.isedit) {
                    return true;
                }
            }
        }
        return false;
    }
    function gjform(g, name, title, fun) {
        var f = new childform();
        var ds = g.datasource;
        f.title = ds.modeldata.title + title;
        f.childitem = ds.modeldata.childitem;
        f.parent = ds;
        f.parentgrid = g;
        f.onSelectTab = function (index, grid) {
            if (ds.onSelectTab) {
                ds.onSelectTab(index, grid);
            }
        }
        f.IsSave = false;
        f.m = name;
        f.onClickOk = function (f) {
            fun(f);
        }
        f.show();
        if (ds.onCommandClick) {
            ds.onCommandClick(name, f, ds.selectItem);
        }
        return f;
    }
    function selectup(g) {
        var name = "#" + g.name;
        var row = $(name).datagrid('getSelected');
        var index = $(name).datagrid('getRowIndex', row);
        index = index - 1;
        if (index >= 0) {
            $(name).datagrid('selectRow', index);
        }
    }
    function selectdown(g) {
        var name = "#" + g.name;
        var row = $(name).datagrid('getSelected');
        var index = $(name).datagrid('getRowIndex', row);
        index = index + 1;
        var count = $(name).datagrid('getRows').length;
        if (index < count) {
            $(name).datagrid('selectRow', index);
        }
    }
    function getcmd(g) {
        var list = [];
        var name = "#" + g.name;
        if (g.editobj.remove) {
            list.push({
                text: '删除',
                iconCls: 'icon-remove',
                handler: function () {
                    var row = g.getselectrows();
                    if (row) {
                        var tc = 0;
                        if (typeof row === "object" && typeof row.length === "number") {
                            var count = row.length;
                            for (var i = 0; i < count; i++) {
                                var r = row[i];
                                tc = gridremove(g, r);
                            }

                        }
                        else {
                            tc = gridremove(g, row);
                            $(name).datagrid('selectRow', i);
                        }
                        $(name).datagrid('rejectChanges');
                        $(name).datagrid('selectRow', tc);
                        ///g.rejectRows.push("remove");
                    }
                }
            });
        }
        if (g.editobj.add) {
            list.unshift({
                text: '新增',
                iconCls: 'icon-add',
                handler: function () {
                    var gj = gjaddoredit(g, "add");
                    if (gj) {
                        gjform(g, "add", "新增", function (f) {
                            var row = f.getallval();
                            var index = gridinsert(g, row);
                            $(name).datagrid('selectRow', index);
                        });
                    }
                    else {
                        if (g.editIndex >= 0) {
                            try {
                                $(name).datagrid('endEdit', g.editIndex);
                                $(name).datagrid('acceptChanges');
                                $(name).datagrid('rejectChanges');
                            } catch (e)
                        { return; }
                        }
                        var obj = {};
                        for (var n in g.items) {
                            obj[g.items[n].field] = "";
                        }
                        g.editIndex = gridinsert(g, obj); ;
                        $(name).datagrid('selectRow', g.editIndex);
                        $(name).datagrid('beginEdit', g.editIndex);
                    }
                }
            });
            list.push("-");
            list.push({
                text: '粘贴',
                iconCls: 'icon-cut',
                handler: function () {
                    var row = $(name).datagrid('getSelected');
                    if (row) {
                        if (g.editIndex > -1) {
                            $(name).datagrid('endEdit', g.editIndex);
                            $(name).datagrid('acceptChanges');
                            $(name).datagrid('rejectChanges');
                        }
                        var obj = {};
                        for (var r in row) {
                            obj[r] = row[r];
                        }
                        g.editIndex = gridinsert(g, obj); ;
                        $(name).datagrid('selectRow', g.editIndex);
                        $(name).datagrid('beginEdit', g.editIndex);
                    }
                }
            });
            if (g.isupdown) {
                var upobj = {};
                upobj.text = "上移";
                upobj.iconCls = "";
                upobj.handler = function () {
                    g.edited();
                    var row = $(name).datagrid('getSelected');
                    var index = $(name).datagrid('getRowIndex', row);
                    index = index - 1;
                    if (index >= 0) {
                        gridremove(g, row);
                        index = gridinsert(g, row, index)
                        $(name).datagrid('selectRow', index);
                    }
                };
                var downobj = {};
                downobj.text = "下移";
                downobj.iconCls = "";
                downobj.handler = function () {
                    g.edited();
                    var row = $(name).datagrid('getSelected');
                    var index = $(name).datagrid('getRowIndex', row);
                    index = index + 1;
                    var count = $(name).datagrid('getRows').length;
                    if (index < count) {
                        gridremove(g, row);
                        index = gridinsert(g, row, index)
                        $(name).datagrid('selectRow', index);
                    }
                };
                list.push("-");
                list.push(upobj);
                list.push(downobj);
            }
        }
        if (g.editobj.select) {
            var s = {
                text: "选择",
                iconCls: "icon-add",
                handler: function () {
                    var f = new searchform();
                    f.ischeck = true;
                    var source = new datasource();
                    source.c = g.editobj.select.c;
                    source.init();
                    var model = {};
                    model.modeldata = source.modeldata
                    model.modeldata.isclear = false;
                    model.name = g.datasource.c; // source.c;
                    f.model = model;
                    f.menuname = model.modeldata.title;
                    f.title = "选择";
                    f.m = "outsearchload&outc=" + source.c;
                    f.fm = function () {
                        return g.parent.parent.selectItem;
                    }
                    f.load = true;
                    f.dbclose = false;
                    f.onClose = function (rows) {
                        if (rows) {
                            g.isdownadd = true;
                            if (model.modeldata.name == g.datasource.modeldata.name) {
                                var nrows = [];
                                var grows = g.getrows();
                                for (var i in grows)
                                    nrows.push(grows[i]);
                                for (var i in rows) {
                                    rows[i].StateBase = 3;
                                    //gridinsert(g, rows[i]);
                                    //setupdateRows(g, rows[i]);
                                    nrows.push(rows[i]);
                                }
                                var ddd = {};
                                ddd.rows = nrows;
                                ddd.total = nrows.length;
                                g.load(ddd);
                            }
                            else {
                                var obj = {};
                                for (var i in g.foreign) {
                                    if (model.modeldata.name == g.foreign[i].foreignobject && g.foreign[i].objectname == g.datasource.modeldata.name) {
                                        obj.field = i;
                                        obj.foreign = g.foreign[i];
                                        break;
                                    }
                                }
                                if (obj && obj.field) {
                                    for (var i in rows) {
                                        var o = {};
                                        o.ROWINDEX = parseInt(i) + 1;
                                        o[obj.field] = rows[i][obj.foreign.foreignfiled];
                                        o[obj.foreign.displayname] = rows[i][obj.foreign.displayfield];
                                        gridinsert(g, o);
                                        setupdateRows(g, o);
                                    }
                                }
                            }
                        }
                    }
                    f.show();
                }
            };
            list.unshift(s);
        }
        return list;
    }
    function cedit(g) {
        var name = "#" + g.name;
        g.isedit = true;
        var config = {
            fit: g.fit,
            fitColumns: g.fitcolumns,
            onCheck: function (rowIndex, rowData) {

            },
            onUncheck: function (rowIndex, rowData) {

            },
            onDblClickRow: function (rowIndex, rowData) {
                if (g.editobj.edit) {
                    var gj = gjaddoredit(g, 'edit');
                    if (gj) {
                        var f = gjform(g, 'edit', "编辑", function (f) {
                            var row = f.getallval();
                            $('#' + g.name).datagrid('updateRow', {
                                index: rowIndex,
                                row: row
                            });
                        });
                        f.val(rowData);
                    }
                    else {
                        if (g.editIndex != undefined && g.editIndex != rowIndex) {
                            $(name).datagrid('endEdit', g.editIndex);
                            $(name).datagrid('acceptChanges');
                            //$(name).datagrid('rejectChanges');
                        }
                        $(name).datagrid('beginEdit', rowIndex);
                        g.editIndex = rowIndex;
                    }
                }
            },
            onClickRow: function (rowIndex) {
                if (g.editobj.edit) {
                    if (g.editIndex != undefined && g.editIndex != rowIndex) {
                        $(name).datagrid('endEdit', g.editIndex);
                    }
                }
            },
            onBeforeEdit: function (rowIndex, rowData) {
                g.editrow = new Object();
                for (var i in rowData) {
                    g.editrow[i] = rowData[i];
                }

            },
            onBeforeEdited: function (sender, index, row, ed) {
                if (g.onBeforeEdit) {
                    g.onBeforeEdit(index, row, ed);
                }
                if (g.datasource.onBeforeEdited != null)
                    g.datasource.onBeforeEdited(g, sender, index, row, ed);
                else {
                    if (g.parent && g.parent.parent && g.parent.parent.onBeforeEdited) {
                        g.parent.parent.onBeforeEdited(g, sender, index, row, ed);
                    }
                }
            },
            onAfterEditing: function (rowIndex, rowData, changes) {
                if (g.onAfterEdit) {
                    g.onAfterEdit(g, rowIndex, rowData, changes);
                }
            },
            onAfterEdit: function (rowIndex, rowData, changes) {
                if (g.editrow) {
                    if (g.editrow.StateBase === undefined) {
                        for (var i in rowData) {
                            if (rowData[i] != g.editrow[i]) {
                                rowData.StateBase = 3;
                                rowData.GetOldObject = new Object();
                                for (var n in g.editrow) {
                                    rowData.GetOldObject[n] = g.editrow[n];
                                }
                                break;
                            }
                        }
                    }
                    setupdateRows(g, rowData);
                    var cz = "add";
                    if (rowData.StateBase == 3)
                        cz = "edit";
                    //                    if (rowData.StateBase != undefined) {
                    //                        g.rejectRows.push(cz);
                    //                    }
                }
                $(name).datagrid('acceptChanges');
                // $(name).datagrid('rejectChanges');
                if (g.datasource.onAfterEdit != null)
                    g.datasource.onAfterEdit(g, rowIndex, rowData, changes);
            }
        };
        var tool = getcmd(g);
        if (tool.length > 0) {
            config.toolbar = tool;
        }
        $(name).datagrid(config);
    }
    function setupdateRows(g, row) {
        var gx = true;
        if (row.StateBase == 3) {
            for (var i in g.updateRows) {
                if (g.updateRows[i].ID == row.ID) {
                    g.updateRows[i] = row;
                    gx = false;
                    break;
                }
            }
            if (gx)
                g.updateRows.push(row);
        }
        if (row.StateBase == 0) {
            for (var i in g.myinsertRows) {
                if (g.myinsertRows[i].ido == row.ido) {
                    g.myinsertRows[i] = row;
                    gx = false;
                    break;
                }
            }
            if (gx)
                g.myinsertRows.push(row);
        }
        rowstotal(g, "i");
    }
    function removeUpdateRows(g, row, ri) {
        var index = -1;
        if (row.StateBase === 3) {
            for (var i = 0; i < g.updateRows.length; i++) {
                if (g.updateRows[i].ID == row.ID) {
                    index = i;
                    break;
                }
            }
            if (index > -1) {
                g.updateRows.splice(index, 1);
            }
        }
        if (row.StateBase === 0) {
            for (var i = 0; i < g.myinsertRows.length; i++) {
                if (g.myinsertRows[i].ido == row.ido) {
                    index = i;
                    break;
                }
            }
            if (index > -1) {
                g.myinsertRows.splice(index, 1);
            }
        }
        rowstotal(g, "i");
    }
    function setfooter(g) {
        var isfooter = false;
        if (g.showfooter) {
            var introws = [];
            for (var i in g.items) {
                var fitem = g.datasource.getitem(g.items[i].field)
                if (fitem.type == "bool")
                    continue;
                if (fitem.field == "ROWINDEX")
                    continue;
                if (fitem.comvtp && fitem.comvtp.isvtp)
                    continue;
                if (fitem.foreign && fitem.foreign.isfkey)
                    continue;
                if (fitem.type == 'int' || fitem.type == 'dint') {
                    introws.push(g.items[i].field);
                    isfooter = true;
                }
            }
            if (isfooter) {
                g.isfooter = introws;
            }
        }
        return isfooter;
    }
    function getfooter(g, rows) {
        introws = g.isfooter;
        var frow = {};
        for (var c in introws) {
            var tot = 0;
            var f = introws[c];
            for (var n in rows) {
                var r = rows[n];
                if (r[f] != null && r[f] != "") {
                    var t = parseFloat(r[f]);
                    tot += t;
                }
            }
            frow[f] = tot.toFixed(4);
        }
        var field = "";
        for (var cc in g.items) {
            var f = g.items[cc].field;
            if (f == introws[0])
                break;
            field = f;
        }
        frow[field] = "合计:";
        frow.footer = true;
        return frow;
    }
    function rowstotal(g, cmd, rows) {
        if (g.isfooter && g.showfooter) {
            introws = g.isfooter;
            switch (cmd) {
                case "load":
                    var frow = getfooter(g, rows.rows);
                    rows.footer = [];
                    rows.footer.push(frow);
                    break;
                default:
                    var rows = g.getrows();
                    var frows = $('#' + g.name).datagrid('getFooterRows');
                    if (frows) {
                        for (var c in introws) {
                            var cot = 0;
                            var f = introws[c];
                            for (var r in rows) {
                                if (rows[r][f] != null && rows[r][f] != "")
                                    cot += parseFloat(rows[r][f]);
                            }
                            frows[0][f] = cot.toFixed(4);
                        }
                        $('#' + g.name).datagrid('reloadFooter');
                    }
                    else {
                        frows = getfooter(g, rows);
                        $('#' + g.name).datagrid('reloadFooter', [frows]);
                    }
                    break;
            }
        }
    }
    return {
        applyTo: "",
        keys: null,
        id: false,
        items: null,
        datasource: null,
        name: "",
        check: false,
        combox: [],
        foreign: [],
        fit: true,
        pageinit: false,
        fitcolumns: true,
        isedit: false,
        editobj: { add: true, edit: true, remove: true },
        modelname: "",
        height: "auto",
        width: "auto",
        deleteRows: [],
        myinsertRows: [],
        updateRows: [],
        detailRows: [],
        rejectRows: [],
        loadselect: false,
        isdownadd: false,
        iseventrow: true,
        isupdown: false,
        isfooter: false,
        showfooter: true,
        init: function () {
            var g = this;
            g.id = webjs.newid();
            g.name = g.id + "_grid";
            $("#" + g.applyTo).attr("id", g.name);
            initgrid(g);
        },
        reinit: function () {
            initgrid(this);
        },
        selectrow: function () {
            var name = "#" + this.name;
            var index = this.editIndex;
            var row = $(name).datagrid('selectRow', index);
            return row;
        },
        selectup: function () {
            selectup(this);
        },
        selectdown: function () {
            selectdown(this);
        },
        insertRow: function (rows) {
            if (typeof rows === "object" && typeof rows.length === "number") {
                for (var i in rows) {
                    gridinsert(this, rows[i], i);
                    setupdateRows(this, rows[i]);
                }
            }
            else {
                gridinsert(this, rows, 0);
                setupdateRows(this, rows);
            }
        },
        addRow: function (row) {
            var name = "#" + this.name;
            $(name).datagrid('appendRow', row);
            $(name).datagrid('acceptChanges');
            $(name).datagrid('rejectChanges');
        },
        removeRow: function (index) {
            var name = "#" + this.name;
            $(name).datagrid('deleteRow', index);
            $(name).datagrid('acceptChanges');
            $(name).datagrid('rejectChanges');
        },
        edited: function () {
            cedit(this);
        },
        rejectChanges: function () {
            this.removeedit();
            $("#" + this.name).datagrid('rejectChanges');
        },
        endedit: function () {
            $("#" + this.name).datagrid('endEdit', this.editIndex);
        },
        setforeigndisplay: function (obj) {
            for (var n in obj) {
                var tb = $("#" + this.name).datagrid('getEditor', { index: this.editIndex, field: n });
                if (tb && tb.actions)
                    tb.actions.setValue(tb.target, obj[n], obj);
            }
        },
        geteditor: function (field) {
            var tb = $("#" + this.name).datagrid('getEditor', { index: this.editIndex, field: field });
            return tb;
        },
        isValid: function () {
            var v = true;
            if (this.isedit) {
                for (var i in this.items) {
                    var tb = $("#" + this.name).datagrid('getEditor', { index: this.editIndex, field: this.items[i].field });
                    if (tb && tb.target) {
                        v = tb.target.isValid();
                        if (v === false)
                            return v;
                    }
                    continue;
                }
            }
            return v;
        },
        editmenu: function (de) {
            var e = !de ? 'enable' : 'disable';
            var opt = $("#" + this.name + " a");
            for (var b in opt) {
                $(b).linkbutton(e);
            }
        },
        setdisabled: function (de) {
            this.buttondisabled = de;
            this.editobj.edit = !de;
            var opt = $("#" + this.name).datagrid("getPanel").find(".datagrid-toolbar a");
            var e = !de ? 'enable' : 'disable';
            opt.each(function () {
                $(this).linkbutton(e);
            });
        },
        loading: function () {
            $("#" + this.name).datagrid('loading');
        },
        load: function (data) {
            var dg = this;
            $("#" + dg.name).datagrid('loaded');
            if (!data || !data.rows) {
                data = {};
                data.rows = [];
                data.total = 0;
            }
            else
                rowstotal(dg, "load", data);
            if (data && data.total && data.total > 9)
                this.pagination();
            $("#" + dg.name).datagrid('loadData', data);
            if (dg.loadselect) {
                if (dg.selectIndex) {
                    $("#" + dg.name).datagrid('selectRow', dg.selectIndex);
                }
                else
                    $("#" + dg.name).datagrid('selectRow', 0);
            }
            if (dg.isedit && dg.pageindex) {
                if (dg.pageindex == 1) {
                    for (var i in dg.myinsertRows) {
                        $("#" + dg.name).datagrid('insertRow', { index: 0, row: dg.myinsertRows[i] });
                    }
                    $("#" + dg.name).datagrid('acceptChanges');
                    $("#" + dg.name).datagrid('rejectChanges');
                }
            }
            dg.detailRows = [];
            if (dg.onLoaded) {
                dg.onLonded();
            }
        },
        reload: function () {
            if (this.datasource) {
                this.datasource.reload();
            }
        },
        toolbar: function (value) {
            $("#" + this.name).datagrid({
                toolbar: value
            });
        },
        hide: function (col) {
            $(name).datagrid("hideColumn", col);
        },
        getrows: function () {
            var rows = $("#" + this.name).datagrid('getRows');
            return rows;
        },
        checked: function (check, index) {
            if (check) {
                $("#" + this.name).datagrid('checkRow', index);
                $("#" + this.name).datagrid('refreshRow', index);
            }
            else
                $("#" + this.name).datagrid('uncheckRow', index);
        },
        geteditrows: function () {
            //            if (this.check) {
            //                return this.getcheckrows();
            //            }
            this.endedit();
            var rows = this.getrows();
            var editrows = [];
            for (var d in this.deleteRows) {
                editrows.push(this.deleteRows[d]);
            }
            for (var r in rows) {
                editrows.push(rows[r]);
            }
            this.myinsertRows = [];
            this.updateRows = [];
            this.deleteRows = [];


            return editrows;
        },
        getcheckrows: function () {
            this.endedit();
            var rows = $("#" + this.name).datagrid('getChecked');
            return rows;
        },
        getselectrows: function () {
            if (this.check) {
                return this.getcheckrows();
            }
            else {
                return $("#" + this.name).datagrid('getSelected');
            }
        },
        pagination: function (p) {
            if (!this.pageinit)
                cpage(this);
        },
        resize: function () {
            $("#" + this.name).datagrid('resize');
        },
        setresize: function (width, height) {
            var p = $("#" + this.name).datagrid('getPanel');
            $(p).panel('resize', {
                width: width,
                height: height
            });
        },
        isrowedit: function () {
            var dg = this;
            if (dg.deleteRows.length > 0 || dg.myinsertRows.length > 0 || dg.updateRows.length > 0)
                return true;
            else
                return false;
        },
        removeedit: function () {
            this.myinsertRows = [];
            this.updateRows = [];
            this.deleteRows = [];
        },
        sortcolumn: function (sort, col) {
            var dg = this;
            this.endedit();
            if (dg.isrowedit()) {
                $.messager.confirm('排序', '检测到有编辑数据未保存，请确认要执行排序操作吗？如操作将清除未保存数据！', function (ok) {
                    if (ok) {
                        dg.removeedit();
                        if (dg.datasource) {
                            dg.datasource.sortcolumn(sort, col);
                        }
                    }
                });
            }
            else {
                if (dg.datasource) {
                    dg.datasource.sortcolumn(sort, col);
                }
            }
        },
        dblclickrow: function (rowIndex, rowData) {
            if (!this.editobj.edit) return;
            if (this.datasource) {
                this.datasource.dblclickrow(rowIndex, rowData);
                if (this.datasource.searchmodel) {

                }
            }
        },
        clickrow: function (rowIndex, rowData) {
            if (this.datasource)
                this.datasource.clickrow(rowIndex, rowData);
        },
        selectpage: function (page, rows) {
            var dg = this;
            this.endedit();
            if (dg.isrowedit()) {
                $.messager.confirm('查询', '检测到有编辑数据未保存，请确认要执行查询操作吗？如操作将清除未保存数据！', function (ok) {
                    if (ok) {
                        dg.removeedit();
                        if (dg.datasource) {
                            $("#" + dg.name).datagrid('loading');
                            dg.datasource.selectpage(page, rows);
                        }
                    }
                });
            }
            else {
                if (dg.datasource) {
                    $("#" + dg.name).datagrid('loading');
                    dg.datasource.selectpage(page, rows);
                }
            }
        },
        columns: function () {
            return ccolumns(this);
        },
        getselectrow: function () {
            var row = $("#" + this.name).datagrid('getSelected');
            var index = $("#" + this.name).datagrid("getRowIndex", row);
            if (index == this.editIndex) {
                for (var n in row) {
                    var tb = this.geteditor(n);
                    if (tb)
                        row[n] = tb.target.val();
                }
            }
            return row;
        },
        onsearchforeign: function (box, obj) {
            if (this.datasource.onsearchforeign)
                this.datasource.onsearchforeign(box, obj);
        },
        onforeignform: function (sender, value) {
            if (this.iseventrow)
                value.eventrow = webjs.jsonval(this.getselectrow());
            var grid = this;
            var ff = grid.datasource.onforeignform(sender, value, function () {
                if (grid.parent) {
                    var obj = {};
                    obj.row = grid.getselectrow();
                    obj.item = grid.parent.fval();
                    return obj;
                }
                else
                    return grid.getselectrow();
            });
            return ff;
        },
        onforeign: function (sender, value) {
            if (this.iseventrow)
                value.eventrow = webjs.jsonval(this.getselectrow());
            var grid = this;
            this.datasource.onforeign(sender, value, function () {
                if (grid.parent) {
                    var obj = {};
                    obj.row = grid.getselectrow();
                    obj.item = grid.parent.fval();
                    return obj;
                }
                else
                    return grid.getselectrow();
            });
        },
        loadforeign: function (index, row, w, f) {
            var e = this.datasource.loadforeign(index, row, w);
            if (f) {
                f(e);
            }
            return e;
        },
        clear: function () {
            var rows = this.getrows();
            var name = "#" + this.name;
            for (var i = 0; i < rows.length; i++) {
                $(name).datagrid('deleteRow', i);
                $(name).datagrid('acceptChanges');
                $(name).datagrid('rejectChanges');
            }
        },
        getpage: function () {
            return $("#" + this.name).datagrid('getPager');
        },
        detail: function () {
            var dg = this;
            var name = "detail" + dg.name;
            function getid(index, row) {
                var iid = null;
                if (row["ID"] != undefined && row["ID"] != 0)
                    iid = row["ID"];
                if (iid == null) {
                    if (row["ROWINDEX"] != undefined && row["ROWINDEX"] != 0)
                        iid = row["ROWINDEX"];
                }
                if (iid == null)
                    iid = index;
                return iid;
            }
            $('#' + dg.name).datagrid({
                view: detailview,
                detailFormatter: function (index, row) {
                    var iid = getid(index, row);
                    return '<div style="padding:1px;"><div id="' + name + "-" + iid + '" /><div>';
                },
                onExpandRow: function (index, row) {
                    var iid = getid(index, row);
                    if (dg.detailRows[iid] === undefined) {
                        var name = "detail" + dg.name + "-" + iid;
                        var tab = $("#" + name);
                        var p = $("#" + dg.name).datagrid('getPanel');
                        var width = $(p).panel('options').width - 77;
                        $(tab).tabs({ width: width,
                            onSelect: function (title, i) {
                                if (dg.detailRows[iid] != undefined) {
                                    var items = dg.detailRows[iid];
                                    var grid = items[i];
                                    var e = grid.parent.loadforeign(i, row, null);
                                    if (grid.datasource == null) {
                                        var os = dg.datasource.getcmsource(i);
                                        var ns = new datasource();
                                        ns.c = os.c;
                                        ns.modeldata = os.modeldata;
                                        ns.keys = os.keys;
                                        ns.items = os.items;
                                        ns.sort = os.sort;
                                        ns.page = os.page;
                                        ns.where = os.where;
                                        ns.commands = dg.datasource.commands;
                                        ns.selecttype = os.selecttype;
                                        ns.tempfdata = os.tempfdata;
                                        ns.onCommand = function (name, data) {
                                            data.save = false;
                                            if (name == "save") {
                                                if (data.m) {
                                                    var sdata = data.val();
                                                    var source = grid.datasource;
                                                    grid.loadselect = true;
                                                    if (source.selectItem) {
                                                        var old = {};
                                                        for (var n in source.selectItem)
                                                            old[n] = source.selectItem[n];
                                                        old.GetOldObject = source.selectItem;
                                                        old.StateBase = 3;
                                                        for (var n in sdata)
                                                            old[n] = sdata[n];
                                                        row.StateBase = 3;
                                                        row[source.modeldata.name] = [old];
                                                        var item = webjs.jsonval(row);
                                                        var e = dg.datasource.post(data.m, "ActionItem=" + item);
                                                        if (e && e.error) {
                                                            $.messager.alert('异常', e.errormsg, 'error');
                                                        }
                                                        else {
                                                            source.reload();
                                                            data.val(source.selectItem);
                                                            $.messager.show({ title: '消息', msg: '保存完成。', showType: 'show' });
                                                        }
                                                    }

                                                }
                                                return false;
                                            }
                                            return true;
                                        }
                                        grid.datasource = ns;
                                        grid.init();
                                        ns.reload = function () {
                                            ns.load(ns.where);
                                        }
                                        ns.load = function (w) {
                                            var where = ns.fwhere(w);
                                            var dataw = ns.tempfdata + "&" + where;
                                            var e = ns.post("loadforeign", dataw);
                                            grid.load(e);
                                            $('#' + dg.name).datagrid('fixDetailRowHeight', index);
                                        }

                                    }
                                    grid.load(e);
                                    $('#' + dg.name).datagrid('fixDetailRowHeight', index);
                                }

                            }
                        });
                        var griditems = [];
                        for (var i in dg.datasource.modeldata.childmodel) {
                            var id = name + '-' + i;
                            $(tab).tabs("add", {
                                title: dg.datasource.modeldata.childmodel[i].title,
                                content: '<div id="' + id + '"/>'
                            });
                            var model = dg.datasource.modeldata.childmodel[i];
                            var grid = new datagrid();
                            grid.parent = dg;
                            grid.applyTo = id;
                            grid.fit = false;
                            grid.fitcolumns = true;
                            if (model.disabled) {
                                grid.editobj.add = false;
                                grid.editobj.edit = false;
                                grid.editobj.remove = false;
                            }
                            else {
                                grid.editobj.add = model.isadd;
                                grid.editobj.edit = model.isedit;
                                grid.editobj.remove = model.isremove;
                            }
                            grid.onLonded = function () {
                                $('#' + dg.name).datagrid('fixDetailRowHeight', index);
                            }
                            griditems.push(grid);
                        }
                        dg.detailRows[iid] = griditems;
                        if (dg.datasource.onExpandRow) {
                            var er = {};
                            er.row = row;
                            er.index = index;
                            er.tabs = $(tab);
                            er.grids = dg.detailRows[iid];
                            er.childmodel = dg.datasource.modeldata.childmodel;
                            er.datasource = dg.datasource;
                            var bb = dg.datasource.onExpandRow(er);
                            if (bb === false)
                                return;
                        }
                        $(tab).tabs('select', 0);
                    }
                    $('#' + dg.name).datagrid('fixDetailRowHeight', index);
                }
            });

        }
    }
}
