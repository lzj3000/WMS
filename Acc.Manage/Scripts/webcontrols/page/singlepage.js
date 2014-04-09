var singlepage = function () {
    function cinit(page) {
        page.name = "singlepage" + page.id;
        if (page.applyTo != "") {
            $("#" + page.applyTo).attr("id", page.name);
            var name = "#" + page.name;
            $(name).append("<div region='north' border='false' style='height:32px;' id='tool_north'/>");
            $(name).append("<div region='center' border='false'><div id='grid_center'/></div>");
            $(name).layout({ fit: true });
            var source = new datasource();
            source.ischildgrid = page.ischildgrid;
            source.ischildform = page.ischildform;
            source.ischildfooter = page.ischildfooter;
            page.datasource = source;
            source.c = page.c;
            var tool = new toolstrip();
            tool.datasource = source;
            tool.applyTo = 'tool_north'; // page.applyTo;

            source.tool = tool;
            tool.issearch = !page.closesearch;
            var grid = new datagrid();
            grid.datasource = source;
            grid.applyTo = "grid_center";
            grid.showfooter = page.showfooter;
            grid.loadselect = true;
            source.grid = grid;
            source.init();
            eventRegion(page, source);
            tool.init();
            if (source.items != null && source.items.length > 10)
                grid.fitcolumns = false;
            grid.check = page.isgridcheck;
            grid.init();
            if (page.ischildgrid) {
                if (source.modeldata.childmodel.length > 0)
                    grid.detail();
            }
            source.p = page;
            // grid.toolbar('#' + tool.applyTo);

            //代办任务
            var processState = $('#pgrid');
            if (processState != null && processState !== undefined) {
                var Ids = processState.attr("Ids");
                if (Ids != null && Ids !== undefined) {
                    var where = 'LoadItem={"page":1,"rows":10,"whereList":[{"ColumnName":"ID","Type":"","Value":"(' + Ids + ')","Symbol":"in"}]}';
                    source.m = "load";
                    var data = webjs.post(source.c, source.m, where);
                    if (data && data.error) {
                        $.messager.alert('异常', data.errormsg, 'error');
                    }
                    else {
                        source.onLoad(data);
                        return data;
                    }
                }
            }
        }
    }


    function eventRegion(page, source) {
        source.onAfterEdit = function (rowIndex, rowData, changes) {
            page.onAfterEdit(rowIndex, rowData, changes);
        }
        source.onSelect = function (item) {
            page.onSelect(item);
        }
        source.onCommandClick = function (command, form, item) {
            if (form && command != 'add') {
                form.tool = this.tool;
                form.setmenu();
            }
            return page.onCommandClick(command, form, item);
        }
        source.onSubmited = function (cmd, data, item) {
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
                    so.onSaveing = function (cmd, item) {
                        data.sender.datasource.onSaveing(cmd, item);
                    };
                    so.onFormSelect = function (form, box, item) {
                        data.sender.datasource.onFormSelect(form, box, item);
                    }
                    so.onSelectTab = function (index, grid) {
                        data.sender.datasource.onSelectTab(index, grid.parent, grid);
                    }
                    so.onForeigned = function (sender, fdata, rows) {
                        data.sender.datasource.onForeigned(sender, fdata, rows);
                    }
                    so.onchildAdding = function (form, grid, row) {
                        data.sender.datasource.onchildAdding(form, grid, row);
                    }
                    so.onExpandRow = function (data) {
                        return page.onExpandRow(data);
                    }
                    data.title = "生成";
                    var win = so.openwin(data, "add");
                    win.val(item);
                }
            }
            page.onSubmited(cmd, item);
        }
        source.onSaveing = function (cmd, item) {
            page.onSaveing(cmd, item);
        }
        source.onFormSelect = function (form, box, item) {
            page.onFormSelect(form, box, item);
        }
        source.onSelectTab = function (index, grid) {
            page.onSelectTab(index, grid.parent, grid);
        }
        source.onForeigned = function (sender, fdata, rows) {
            page.onForeigned(sender, fdata, rows);
        }
        source.onFormater = function (obj) {
            return page.onFormater(obj);
        }
        source.onchildAdding = function (form, grid, row) {
            page.onChildAdding(page, form, grid, row);
        }
        source.onOpenWin = function (form, data, command) {
            page.onOpenWin(form, data, command);
        }
        source.onOpenWinShow = function (form, data, command) {
            page.onOpenWinShow(form, data, command);
        }
        source.onExpandRow = function (data) {
            return page.onExpandRow(data);
        }
    }
    return {
        c: "",
        applyTo: "",
        id: "",
        datasource: null,
        ischildgrid: true,
        ischildform: true,
        closesearch: false,
        isprint: true,
        showfooter: true,
        ischildfooter: true,
        isgridcheck: false,
        init: function () {
            try {
                this.id = webjs.newid();
                cinit(this);
            }
            catch (e) {
                $.messager.alert('异常', e, 'error');
            }
        },
        //行选择事件
        //item:选中行数据成员
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
        //子模型Grid行增加事件
        //page:主界面
        //form:发生增加的form
        //grid:发生增加的Grid
        //row:正在增加的行数据
        onChildAdding: function (page, form, grid, row) {

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
        { },
        onFormater: function (obj) {
            return obj.value;
        },
        onOpenWin: function (form, data, command) {
            //form.editobj.add = false;
        },
        onOpenWinShow: function (form, data, command) {

        },
        //点击主界面打开明细行事件
        onExpandRow: function (data) {

        }
    }
}