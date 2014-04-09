var toolstrip = function () {
    function setprint(ts, td) {

    }
    function tinit(ts) {
        var name = ts.id + "-toolstrip";
        if (ts.applyTo != "") {
            var div = $("#" + ts.applyTo).attr("id", name);
            var table = $('<table cellpadding="0" cellspacing="0" style="width:100%;"></table>').appendTo($("#" + name)); //background:#fafafa;
            var tr = $("<tr></tr>").appendTo(table);
            var tds = $('<td nowrap style="padding-left:2px;width:0%"></td>').appendTo(tr);
            ts.searchTd = tds;
            inits(ts, tr, tds);
            if (ts.onlysearch) {
                ts.applyTo = name;
                return;
            }
            var td = $('<td nowrap style="text-align:left;padding-left:2px;width:80%"/>').appendTo(tr);
            ts.commandTd = td;
            var helptd = $('<td nowrap style="text-align:right;padding-right:2px;width:50%"></td>').appendTo(tr);
            ts.helpTd = helptd;
            initcommand(ts, td);
            if (ts.isprint) {
                setprint(ts, helptd);
            }
            if (ts.isRecycle) {
                $('<a href="#" class="easyui-linkbutton" plain="true" icon="icon-garbage"  title="已删除单据">回收站</a>').appendTo(helptd).bind("click", function () {
                    var form = new searchform();
                    form.ischeck = true;
                    var model = new Object();
                    model.modeldata = ts.datasource.modeldata
                    model.modeldata.isclear = false;
                    model.name = ts.datasource.c;
                    form.model = model;
                    form.menuname = model.modeldata.title;
                    form.title = "回收站";
                    form.m = "load";
                    form.load = true;
                    form.dbclose = false;
                    form.istree = false;
                    var ok = new Object();
                    ok.command = "ok";
                    ok.visible = true;
                    ok.disabled = false;
                    ok.name = "还原";
                    ok.icon = "icon-ok";
                    ok.title = "撤消删除状态"
                    ok.handler = function (obj) {
                        var rows = form.val();
                        var id = "";
                        for (var i in rows) {
                            if (id === "")
                                id = rows[i].ID;
                            else
                                id += "," + rows[i].ID;
                        }
                        if (id !== "") {
                            ts.datasource.submit('Reduction', "reductionid=" + id, function (e) {
                                ts.datasource.reload();
                                form.reload();
                                if (ts.datasource.onReduction) {
                                    ts.datasource.onReduction(rows);
                                }
                            })
                        }

                    }
                    var ok1 = {};
                    ok1.command = "clear";
                    ok1.visible = true;
                    ok1.disabled = false;
                    ok1.name = "清除";
                    ok1.icon = "icon-no";
                    ok1.title = "彻底删除选中单据"
                    ok1.handler = function (obj) {
                        $.messager.confirm('清除', '请确认要执行清除操作吗？清除的单据无法从系统还原！', function (ok) {
                            if (ok) {
                                var rows = form.val();
                                var id = "";
                                for (var i in rows) {
                                    if (id === "")
                                        id = rows[i].ID;
                                    else
                                        id += "," + rows[i].ID;
                                }
                                if (id !== "") {
                                    ts.datasource.submit('ClearAway', "reductionid=" + id, function (e) {
                                        form.reload();
                                    })
                                }
                            }
                        });
                    }
                    form.commands = [];
                    form.commands.push(ok);
                    form.commands.push(ok1);
                    form.onWhere = function (f, v) {
                        if (v)
                            v += " and IsDelete=1";
                        else
                            v = "IsDelete=1";
                        return v;
                    }
                    form.show();
                });
            }
            if (ts.ishelp) {
                var helpmenu = name + "mm1";
                var hmm = $('<a href="#" class="easyui-menubutton" plain="true" icon="icon-help" menu="#' + helpmenu + '"  title="帮助">帮助</a>').appendTo(helptd);
                var menu = $('<div id="' + helpmenu + '" style="width:100px;"></div>').appendTo(helptd);
                $('<div iconCls="icon-search">查看帮助</div>').appendTo(menu).bind("click", function () {
                    //alert("查看帮助");
                    if (ts.datasource && ts.datasource.url) {
                        //var hurl = ts.datasource.url.replace(/Views/, "help") + "l";
                        var hurl = "http://192.168.100.149:802/load.aspx?project=Acc_WMS&controller=" + ts.datasource.c;
                        ts.showhelp(hurl, ts.datasource.title);
                    }
                });
                $('<div class="menu-sep"></div>').appendTo(menu);
                $('<div>关于</div>').appendTo(menu).bind("click", function () {
                    var s = "名称：" + ts.datasource.title + "</br>" + "说明：" + ts.datasource.description + "</br>" + "开发人：" + ts.datasource.author;
                    $.messager.alert('关于', s);
                });
                $(hmm).menubutton();
            }
            ts.applyTo = name;
        }
    }
    function initcommand(ts, div) {
        if (ts.commands) {
            var splitcomm = [];
            for (var c in ts.commands) {
                var comm = ts.commands[c];
                if (comm.visible && comm.issplit) {
                    if (splitcomm[comm.splitname] === undefined) {
                        splitcomm[comm.splitname] = [];
                        splitcomm[comm.splitname].push(comm);
                    }
                    else
                        splitcomm[comm.splitname].push(comm);
                }
            }
            for (var c in ts.commands) {
                var comm = ts.commands[c];
                if (comm.visible && !comm.issplit) {
                    if (comm.td) {
                        if (comm.td == 'help')
                            div = ts.helpTd;
                        if (comm.td == 'search')
                            div = ts.searchTd;
                    }
                    else
                        div = ts.commandTd;
                    var cn = ts.id + "-" + comm.command;
                    var key = comm.command;
                    if (comm.Tag)
                        key += comm.Tag;
                    $('<a href="#" id="' + cn + '" plain="true" icon="' + comm.icon + '" accessKey="' + key + '" title="' + comm.title + '">' + comm.name + '</a>').appendTo(div);
                    var name = "#" + ts.id + "-" + comm.command;
                    $(name).click(function () {
                        if ($(this).linkbutton('options').disabled) return;
                        bindclick(this, ts);
                    });
                    if (splitcomm[comm.command] === undefined) {
                        $(name).linkbutton();
                        $(name).linkbutton(comm.disabled ? 'disable' : 'enable');
                    }
                    else {
                        var items = splitcomm[comm.command];
                        var mmid = cn + '-mm';
                        var mm = $("<div id='" + mmid + "' style='width:100px;'/>").appendTo(div);
                        for (var i in items) {
                            var cmd = items[i];
                            key = cmd.command;
                            if (cmd.Tag)
                                key += cmd.Tag;
                            $('<div id="' + mmid + '-' + cmd.command + '" iconCls="' + cmd.icon + '" accessKey="' + key + '">' + cmd.name + '</div>').appendTo(mm).bind('click', function () {
                                if ($(this).attr('disabled')) return;
                                bindclick(this, ts);
                            });
                        }
                        $(name).splitbutton({ menu: '#' + mmid });
                    }
                }
            }
        }
    }
    function bindclick(but, ts) {
        if (but.accessKey == "down") return;
        var tc = but;
        var comm = false;
        var key = "";
        for (var t in ts.commands) {
            key = ts.commands[t].command;
            if (ts.commands[t].Tag)
                key += ts.commands[t].Tag;
            if (key == tc.accessKey) {
                comm = ts.commands[t];
                break;
            }
        }
        try {
            if (comm) {
                //                if (!comm.isclicking) {
                //                    comm.isclicking = true;
                //                    comm.istb = true;
                commclick(ts, tc, comm);
                //                    if (comm.istb)
                //                        comm.isclicking = false;
                //                }
            }
        }
        catch (e) {
            if (comm.istb)
                comm.isclicking = false;
        }
    }
    function commclick(ts, tc, comm) {
        if (comm.isselectrow) {
            if (ts.datasource.selectItem == null) {
                $.messager.alert('异常', "执行" + comm.name + "必须选择数据！");
                return;
            }
            else {
                if (ts.datasource.selectItem.StateBase === undefined)
                    ts.datasource.selectItem.StateBase = 3;
            }
        }
        var data = {};
        data.command = comm.command;
        data.item = ts.datasource.selectItem;
        data.sender = ts;
        data.toolstrip = tc;
        data.title = comm.name;
        data.comm = comm;
        data.url = "web.aspx?c=" + ts.datasource.c + "&m=" + data.command + "&guid=" + webjs.guid;
        if (comm.handler)
            data.handler = comm.handler;
        if (comm.isalert) {
            $.messager.confirm(comm.name, '请确认要执行' + comm.name + '操作吗？', function (ok) {
                if (ok) {
                    if (!comm.isclicking) {
                        comm.isclicking = true;
                        comm.istb = true;
                        commonclick(ts, tc, comm, data);
                        if (comm.istb)
                            comm.isclicking = false;
                    }
                }
            });
        }
        else {
            if (!comm.isclicking) {
                comm.isclicking = true;
                comm.istb = true;
                commonclick(ts, tc, comm, data);
                if (comm.istb)
                    comm.isclicking = false;
            }
        }

    }
    function commonclick(ts, tc, comm, data) {
        if (ts.onClicking(tc, data)) {
            if (comm.onclick) {
                var sub = eval(comm.onclick + "(data);");
                if (sub === true)
                    ts.clickcommand(tc, data);
            }
            else
                ts.clickcommand(tc, data);
            ts.onClicked(tc, comm);
        }
    }
    function inits(ts, tr, div) {
        if (ts.issearch) {
            $('<td><div class="pagination-btn-separator"></div></td>').appendTo(tr);
            var selectid = ts.id + '-select';
            var butid = ts.id + '-button';
            setsearch(ts);
            var select = ts.select;
            var box = ts.box.input; // '<span id="' + ts.box.id + '-div">' + +'</span>';
            var a = $('<a href="#" id="' + butid + '" plain="true" icon="icon-search" title="查询">查询</a>');
            if (ts.onlysearch)
                a = $('<a href="#" id="' + butid + '" plain="true" icon="icon-search" title="查询"></a>');
            div.append(select);
            div.append(box);
            div.append(a);
            if (ts.isAsearch) {
                $(div).advancedsearch({
                    title: "高级查询",
                    source: ts.datasource,
                    butid: butid
                });
            }
            else {
                $("#" + butid).linkbutton();
            }
            ts.box.ready();
            $("#" + butid).click(function () {
                if (ts.clickcommand) {
                    var data = {};
                    data.command = "load";
                    var f = ts.box.field;
                    var v = ts.box.val();
                    data.field = f;
                    if (v != "") {
                        if (f == "" || f == "*")
                            ts.box.field = "code";
                        data.value = ts.box.getwhere();
                    }
                    else {
                        // if (f != "*")
                        //  data.value = f + '=';
                    }
                    ts.clickcommand(ts, data);
                }
            });
            var ss = [];
            for (var i in ts.searchitems) {
                ss[i] = ts.searchitems[i];
            }
            $("#" + selectid).combobox({
                data: ss,
                valueField: 'field',
                textField: 'title',
                value: '*',
                multiple: false,
                panelHeight: "auto",
                editable: false,
                onSelect: function (r) { onselect(ts, r); }
            });
        }
    }
    function setsearch(ts) {
        var sw = 80, bw = 130;
        if (ts.onlysearch) {
            sw = 60;
            bw = 90;
        }
        var selectid = ts.id + '-select';
        var select = $('<select id="' + selectid + '" class="easyui-combobox" panelHeight="auto" style="width:' + sw + 'px;"></select>');
        ts.box = new textbox();
        ts.box.width = bw;
        ts.box.required = false;
        ts.box.init();
        ts.select = select;
    }
    function onselect(sender, item) {
        var bw = 130;
        if (sender.onlysearch)
            bw = 90;
        var f = item.field;
        var sitem = sender.datasource.getitem(f);
        if (sitem) {
            if (sender.box) {
                sender.box.required = false;
                sender.box.comvtp = null;
                sender.box.foreign = null;
                sender.box.type = "";
                sender.box.field = item.field;
                sender.box.width = bw;
                for (var n in sitem) {
                    sender.box[n] = sitem[n];
                }
                sender.box.tablename = sender.datasource.modeldata.tablename;
                sender.box.searchstate = true;
                sender.box.disabled = false;
                sender.box.visible = true;
                sender.box.parent = sender.datasource;
                sender.box.update();
                sender.box.onevent();
            }
        }
        else {
            if (item.field == "*") {
                sender.box.required = false;
                sender.box.comvtp = null;
                sender.box.foreign = null;
                sender.box.type = "";
                sender.box.field = "*";
                sender.box.width = bw;
                sender.box.disabled = false;
                sender.box.visible = true;
                sender.box.update();
            }
        }
    }
    function print(tool) {
        var pt = {};
        pt.command = "startprint";
        pt.name = "打印";
        pt.istb = false;
        pt.title = "打印预览";
        pt.icon = "icon-print";
        pt.isselectrow = true;
        pt.td = "help";
        pt.visible = true;
        pt.editshow = true;
        tool.commands.push(pt);
        var pc = {};
        pc.command = "pringconfig";
        pc.name = "模板设置";
        pc.title = "打印模板设置";
        pc.icon = "icon-reload";
        pc.issplit = true;
        pc.splitname = "startprint";
        pc.visible = true;
        tool.commands.push(pc);
        tool.onClicking = function (sender, data) {
            if (data.command == 'pringconfig') {
                var p = new spdf();
                p.c = tool.datasource.c;
                p.title = tool.datasource.modeldata.title;
                p.config();
                return false;
            }
            return true;
        }
    }
    return {
        id: "",
        applyTo: null,
        commands: null,
        searchitems: null,
        issearch: true,
        onlysearch: false,
        datasource: null,
        parent: null,
        isRecycle: true,
        isAsearch: true,
        ishelp: true,
        iscopy: false,
        init: function () {
            this.id = webjs.newid();
            if (this.datasource) {
                if (this.searchitems == null)
                    this.searchitems = this.datasource.searchitems;
                if (this.commands == null && !this.onlysearch) {
                    this.commands = this.datasource.commands;
                    var isadd = false;
                    for (var i in this.commands) {
                        if (this.commands[i].command === 'add') {
                            isadd = {};
                            for (var n in this.commands[i]) {
                                isadd[n] = this.commands[i][n];
                            }
                            break;
                        }
                    }
                    if (isadd && this.iscopy) {
                        isadd.splitname = 'add';
                        isadd.issplit = true;
                        isadd.isselectrow = true;
                        isadd.command = 'copy';
                        isadd.name = '复制';
                        this.commands.push(isadd);
                    }
                    if (this.datasource.IsPrint) {
                        print(this);
                    }
                    this.isRecycle = this.datasource.IsClearAway;
                }
            }
            tinit(this);
        },
        onClicking: function (sender, comm) {
            return true;
        },
        onClicked: function (sender, comm) {

        },
        clickcommand: function (sender, data) {
            var ts = this;
            if (ts.parent)
                ts.parent.clickcommand(ts, data);
            else {
                if (ts.datasource)
                    ts.datasource.clickcommand(data.command, data);
            }
        },
        disabled: function (command, dis) {
            var comm;
            for (var i in this.commands) {
                if (this.commands[i].command == command) {
                    comm = this.commands[i];
                    break;
                }
            }
            if (comm) {
                var d = dis ? 'disable' : 'enable';
                var cn = this.id + "-" + comm.command;
                if (!issplit) {
                    $("#" + cn).linkbutton(d);
                }
                else {
                    var ccn = this.id + '-' + comm.splitname + "-mm-" + comm.command;
                    $("#" + ccn).attr('disable', dis);
                }
            }
        },
        remove: function (command) {
            var comm;
            for (var i in this.commands) {
                if (this.commands[i].command == command) {
                    comm = this.commands[i];
                    break;
                }
            }
            if (comm) {
                var cn = this.id + "-" + comm.command;
                $("#" + cn).remove();
            }
        },
        getcmds: function () {
            var comm = [];
            for (var t in this.commands) {
                if (this.commands[t].command != 'add' && this.commands[t].command != 'edit' && this.commands[t].command != 'remove'
                && this.commands[t].splitname != 'add' && this.commands[t].splitname != 'edit' && this.commands[t].splitname != 'remove') {
                    if (this.commands[t].visible)
                        comm.push(this.commands[t]);
                }
            }
            return comm;
        },
        command: function (cmd) {
            var comm = false;
            for (var t in this.commands) {
                if (this.commands[t].command == cmd) {
                    comm = this.commands[t];
                    break;
                }
            }
            if (comm)
                commclick(this, null, comm);
        },
        addCommand: function (comm, wz) {
            switch (wz) {
                case 1: //最左边前
                    break;
                case 2: //中间命令位置
                    break;
                case 3: //最右边帮助位置
                    break;

            }
        },
        getsearch: function (source) {
            if (this.datasource == null)
                this.datasource = source;
            setsearch(this);
        },
        setname: function (comm, n) {
            var name = "";
            if (!comm.issplit) {
                name = this.id + "-" + comm.command;
                $("#" + name).linkbutton({ text: n });
            }
            else {
                name = this.id + "-" + comm.splitname + "-mm-" + comm.command;
                $("#" + name).splitbutton({ text: n });
            }
            comm.oldname = comm.name;
            comm.name = n;
        },
        showhelp: function (url, title) {
            var hid = webjs.newid();
            var div = $("<div id='" + hid + "'><object data='" + url + "' type='text/html' width='100%' height='100%'></object></div>").appendTo($("#temp"));
            $('#' + hid).window({
                title: title + "帮助",
                width: 800,
                height: 600,
                modal: false,
                collapsible: true,
                minimizable: false
            });
            $('#' + hid).window("open")
        }
    }
}