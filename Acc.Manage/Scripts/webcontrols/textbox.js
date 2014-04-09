var textbox = function () {
    function cltype(type) {
        switch (type) {
            case "date":
                return 'datebox';
            case "datetime":
                return 'datetimebox';
            case "time":
                return 'timespinner';
            case "int":
            case "dint":
                return 'numberspinner';
            default:
                return 'spinnerbox';
        }
    }
    //初始化控件
    function cinit(tb) {
        if (tb.id == "")
            tb.id = webjs.newid();
        if (tb.applyTo != "") {
            var at = tb.id + "_applyTo";
            $("#" + tb.applyTo).attr("id", at);
            tb.applyTo = at;
        }
        var t = '<label style="width:130px">' + tb.title + '</label>';
        var i = cbox(tb);
        //var d = '<div nowrap style="float:left;display:block;" id="' + tb.id + '-div">' + t + i + '</div>';
        var d = '<table><tr><td>' + t + '</td><td id="' + tb.id + '-div">' + i + '</td></tr></talbe>';
        if (tb.applyTo != "") {
            $("#" + tb.applyTo).append(d);
            //            if (tb.ctype == "spinnerbox")
            //                $("#" + tb.id).spinnerbox()
        }
        return d;
    }
    function cbox(tb) {
        var dis = cpp(tb);
        if (tb.id == "")
            tb.id = webjs.newid();
        tb.parentid = "parent_" + tb.id;
        if (tb.type == "bool")
            tb.comvtp = { isvtp: true, items: [{ "Text": "否", "Value": 0 }, { "Text": "是", "Value": 1}] };

        if (tb.comvtp != null && tb.comvtp.isvtp) {
            tb.input = "<span id='parent_" + tb.id + "'>" + ccombobox(tb, dis) + "</span>";
        }
        else if ((tb.foreign != null && tb.foreign.isfkey) || tb.type == 'file') {
            tb.input = "<span id='parent_" + tb.id + "'>" + cforeign(tb, dis) + "</span>";
        }
        else {
            tb.ctype = cltype(tb.type)
            if (tb.isremark) {
                tb.input = "<span style='width: 100%' id='parent_" + tb.id + "'><textarea id='" + tb.id + "' name='" + tb.field + "' style='height: 60px; width: 505px'></textarea></span>";
            }
            else
                tb.input = "<span id='parent_" + tb.id + "'>" + '<input id="' + tb.id + '" name="' + tb.field + '" class="easyui-' + tb.ctype + '" ' + dis + ' "></input>' + "</span>";
        }
        return tb.input;
    }
    function cpp(tb) {
        var dis = "";
        var req = "";
        if (tb.disabled)
            dis = "disabled";
        if (tb.required && !tb.searchstate)
            req = ' required=true missingMessage="' + tb.title + '不能为空！"';
        return dis + req + ' style="width:' + tb.width + 'px;"';
    }
    function ccombobox(tb, dis) {
        tb.ctype = "combobox";
        var i = '<select align="right" class="easyui-combobox" panelHeight="auto" id="' + tb.id + '" name="' + tb.field + '"' + dis + '>';
        for (var n in tb.comvtp.items) {
            i += '<option value="' + tb.comvtp.items[n].Value + '">' + tb.comvtp.items[n].Text + '</option>';
        }
        i += '</select>';
        return i;
    }
    function cforeign(tb, dis) {
        tb.ctype = "searchbox";
        var i = '<input id="' + tb.id + '" name="' + tb.field + '" searcher="" class="easyui-searchbox" ' + dis + '></input>';
        if (tb.disabled) {
            tb.ctype = 'spinnerbox';
            i = '<input id="' + tb.id + '" name="' + tb.field + ' "' + dis + '></input>';
        }
        return i;
    }
    function zytext(str) {
        str = str.replace(/&/g, "");
        str = str.replace(/"/g, "");
        return str;
    }
    //获取设置textbox的value值
    function gsvalue(sender, v) {
        if (v === undefined) {
            if (sender.foreign && sender.foreign.isfkey) {
                return sender.foreign.filedvalue;
            }
            if (sender.type == 'file')
                return sender.file;
            var tt = '$("#" + sender.id).' + sender.ctype + '("getValue")';
            var et = eval(tt);
            return et;
        }
        else {
            if (sender.ctype == "spinnerbox")
                $("#" + sender.id).val(v);
            else {
                if (sender.type == "bool") {
                    if (v === true || v === "true" || v === 1 || v === "1")
                        $("#" + sender.id).combobox("setValue", 1);
                    else
                        $("#" + sender.id).combobox("setValue", 0);
                }
                else {
                    if (sender.type == 'date' || sender.type == 'datetime') {
                        if (v == '1900-01-01T00:00:00' || v == '0001/1/1')
                            v = "";
                    }
                    var tt = '$("#" + sender.id).' + sender.ctype + '("setValue",v)';
                    eval(tt);
                }
            }
        }
    }
    function filesubmit(box) {
        $.upload({
            // 上传地址
            url: webjs.filepost,
            // 其他表单数据
            params: { d: box.dic },
            // 文件域名字
            fileName: 'filedata',
            // 上传完成后, 返回json, text
            dataType: 'json',
            // 上传之前回调,return true表示可继续上传
            onSend: function () {
                return true;
            },
            // 上传之后回调
            onComplate: function (file) {
                if (file) {
                    var name = file[0];
                    if (name) {
                        var tt = name.split('$$');
                        box.val(tt[1]);
                        box.file = name;
                        if (box.onfilecomplate) {
                            box.onfilecomplate(box, box.file);
                        }
                    }
                }
            }
        });
    }
    function cevent(box) {
        if (box.ctype == "searchbox") {
            $('#' + box.id).searchbox({
                searcher: function (value, name) {
                    if (!this.disabled) {
                        if (box.type == 'file') {
                            filesubmit(box);
                        }
                        else {
                            box.onforeign(box, box.foreign);
                        }
                    }
                }
            });
            if (box.required) {
                var tb = $('#' + box.id).searchbox("textbox");
                $(tb).validatebox({ required: true, missingMessage: box.title + "不能为空！" });
            }
            if (box.type != 'file') {
                var tb = $('#' + box.id).searchbox('textbox');
                //tb.unbind(".searchbox")
                tb.bind("blur", function () {
                    if (box.sf) {
                        box.sf.close();
                    }
                    $(this).val(box.foreign.displayvalue);
                });
                tb.bind("keydown", function (key) {
                    if (key.keyCode == 13) {
                        var v = $(this).val();
                        //searchload(box, v);
                    }
                    if (key.keyCode == 27) {
                        if (box.sf) {
                            box.sf.close();
                        }
                    }
                    if (key.keyCode == 38) {
                        if (box.sf)
                            box.sf.up();
                    }
                    if (key.keyCode == 40) {
                        if (box.sf)
                            box.sf.down();
                    }
                    if ((key.shiftKey || key.ctrlKey) && key.keyCode == 39) {
                        if (box.sf)
                            box.sf.nextpage();
                    }
                    if ((key.shiftKey || key.ctrlKey) && key.keyCode == 37) {
                        if (box.sf)
                            box.sf.firstpage();
                    }
                    if (key.keyCode == 13 && (key.shiftKey || key.ctrlKey)) {
                        if (box.sf)
                            box.sf.select();
                    }
                });
            }
        }
        if (box.ctype == "combobox") {
            $('#' + box.id).combobox({
                onSelect: function (item) {
                    box.onselect(box, item);
                }
            });

        }
        $("#" + box.parentid + " input[type='text']").attr("readonly", true);
    }
    function searchload(box, value) {
        var obj = {};
        obj.ColumnName = "code";
        obj.Type = "";
        obj.Value = value;
        obj.Symbol = "like";
        if (box.parent && box.parent.onsearchforeign)
            box.parent.onsearchforeign(box,obj);
        if (box.sf == undefined) {
            if (box.parent) {
                var p = box.parent;
                var sf = p.onforeignform(box, box.foreign);
                box.sf = sf;
                var e = $("#parent_" + box.id);
                var left = e.offset().left;
                var top = e.offset().top;
                box.sf.left = left;
                box.sf.top = top + 19;
                box.sf.isblur = false;
                box.sf.show();
                box.parent.onMove = function (left, top) {
                    var e = $("#parent_" + box.id);
                    var left = e.offset().left;
                    var top = e.offset().top;
                    box.sf.left = left;
                    box.sf.top = top + 19;
                    box.sf.close();
                }
                box.sf.sload(obj);
            }
        }
        else {
            if (!box.sf.isshow) {
                box.sf.isblur = false;
                box.sf.show();
            }
            box.sf.sload(obj);
        }
    }
    return {
        type: "",
        field: "",
        disabled: false,
        visible: true,
        applyTo: "",
        title: "",
        id: "",
        index: 0,
        iskey: false,
        issearch: false,
        required: true,
        length: 0,
        isedit: true,
        searchstate: false,
        comvtp: null,
        foreign: null,
        width: 200,
        ctype: "",
        dic: "",
        isremark: false,
        init: function () {
            //try {
            var tx = cinit(this);
            return tx;
            // }
            //catch (e) {
            //    alert(e.Message);
            //}
        },
        text: function () {
            var tt = '$("#" + this.id).' + this.ctype + '("getValue")';
            return eval(tt);
        },
        val: function (v) {
            //try {

            return gsvalue(this, v);
            // }
            //catch (e) {
            //     alert(e.Message);
            //}
        },
        fval: function (row) {
            this.foreign.filedvalue = row[this.field];
            if (this.foreign.displayname != null && this.foreign.displayname != "")
                this.foreign.displayvalue = row[this.foreign.displayname];
            else
                this.foreign.displayvalue = row[this.field];
            if (this.foreign.objectname == this.foreign.foreignobject) {
                var v = this.foreign.displayvalue; // row[this.foreign.displayname];
                if (v === undefined)
                    v = "";
                this.val(v);
            }
            else {
                var v = this.foreign.displayvalue; // row[this.foreign.displayname];
                if (v === undefined)
                    v = "";
                this.val(v);
            }
        },
        cval: function (row) {
            this.foreign.filedvalue = row[this.foreign.foreignfiled];
            //var name = this.foreign.displayname.replace(/FOREIGN/, "");
            //this.foreign.displayvalue = row[name];
            if (this.foreign.displayfield != null && this.foreign.displayfield != "")
                this.foreign.displayvalue = row[this.foreign.displayfield];
            else
                this.foreign.displayvalue = this.foreign.filedvalue;
            this.val(this.foreign.displayvalue);
        },
        clear: function () {
            try {
                this.val("");
                if (this.foreign && this.foreign.isfkey) {
                    this.foreign.filedvalue = "";
                    this.foreign.displayvalue = "";
                }
            }
            catch (e) {
                alert(e.Message);
            }
        },
        box: function () {
            return cbox(this);
        },
        update: function () {
            var parent = $("#parent_" + this.id);
            parent.empty();
            var box = cbox(this);
            parent.append(box);
            this.ready();
        },
        ready: function () {
            if (this.ctype == 'numberspinner' && this.type == "dint") {
                $("#" + this.id).numberspinner({ precision: 4 });
            }
            else {
                eval('$("#" + this.id).' + this.ctype + '()'); //this.ctype == "searchbox" ||
                if (this.ctype == "combobox" || this.ctype == "datebox") {
                    $("#" + this.parentid + " input[type='text']").attr("readonly", true);
                }
            }
        },
        setwidth: function (w) {
            eval('$("#" + this.id).' + this.ctype + '({width:' + w + '})');
        },
        onforeign: function (sender, value) {
            if (this.parent) {
                var p = this.parent;
                p.onforeign(sender, value);
            }
        },
        onselect: function (sender, item) {
            if (this.parent && this.parent.onselect)
                this.parent.onselect(sender, item);
        },
        onfilecomplate: function (sender, file) {

        },
        onevent: function () {
            var box = this;
            cevent(box);
        },
        setdisabled: function (de) {
            if (this.ctype == "combobox" || this.ctype == "numberspinner") {
                this.disabled = de;
                var v = this.val();
                this.update();
                this.val(v);
            }
            else {
                if (de) {
                    if (!this.disabled) {
                        eval('$("#" + this.id).' + this.ctype + '("disable")');
                    }
                }
                else {
                    if (!this.disabled) {
                        eval('$("#" + this.id).' + this.ctype + '("enable")');
                    }
                }
            }
        },
        isValid: function () {
            try {
                if (this.disabled) return true;
                var v = false;
                if (this.ctype == "searchbox") {
                    var tb = $('#' + this.id).searchbox("textbox");
                    v = $(tb).validatebox('isValid');
                }
                else
                    v = $("#" + this.id).validatebox('isValid');
                return v;
            }
            catch (e) {
                //                if (this.ctype == "searchbox" && this.required) {
                //                    if (this.val() == '') {
                //                        if (!this.tip) {
                //                            this.tip = $("<div class=\"validatebox-tip\">" + "<span class=\"validatebox-tip-content\">" + "</span>" + "<span class=\"validatebox-tip-pointer\">" + "</span>" + "</div>").appendTo("body");
                //                        }
                //                        var _e = $("#parent_" + this.id);
                //                        this.tip.find(".validatebox-tip-content").html(this.title + "不能为空！");
                //                        var left = _e.offset().left + _e.outerWidth()
                //                        this.tip.css({ display: "block", left: left, top: _e.offset().top });
                //                        return false;
                //                    }
                //                }
                //                return true;
            }
        },
        getwhere: function () {
            var obj = {};

            obj.ColumnName = this.field;
            if (this.tablename) {
                obj.ColumnName = this.tablename + "." + obj.ColumnName;
            }
            obj.Type = this.type;
            obj.Value = this.val();
            if (obj.Type == "") {
                obj.Symbol = "like";
            }
            if (this.foreign && this.foreign.isfkey) {
                obj.Symbol = "=";
            }
            if (this.comvtp != null && this.comvtp.isvtp) {
                obj.Symbol = "=";
            }
            return obj;
        },
        getsymbol: function () {
            var ss = [];
            var t = this.type;
            if (this.foreign && this.foreign.isfkey) {
                t = "foreign";
            }
            if (this.comvtp != null && this.comvtp.isvtp) {
                t = "comvtp";
            }
            ss.push({ "Text": "等于", "Value": "=" });
            ss.push({ "Text": "不等于", "Value": "<>" });
            switch (t) {
                case "int":
                case "dint":
                    ss.push({ "Text": "大于等于", "Value": ">=" });
                    ss.push({ "Text": "小于等于", "Value": "<=" });
                    ss.push({ "Text": "大于", "Value": ">" });
                    ss.push({ "Text": "小于", "Value": "<" });
                    break;
                case "date":
                    ss.push({ "Text": "上周", "Value": "@fw" });
                    ss.push({ "Text": "本周", "Value": "@w" });
                    ss.push({ "Text": "下周", "Value": "@nw" });
                    ss.push({ "Text": "上月", "Value": "@fm" });
                    ss.push({ "Text": "本月", "Value": "@m" });
                    ss.push({ "Text": "下月", "Value": "@nm" });
                    ss.push({ "Text": "去年", "Value": "@fy" });
                    ss.push({ "Text": "本年", "Value": "@y" });
                    ss.push({ "Text": "明年", "Value": "@ny" });
                    ss.push({ "Text": "大于等于", "Value": ">=" });
                    ss.push({ "Text": "小于等于", "Value": "<=" });
                    ss.push({ "Text": "大于", "Value": ">" });
                    ss.push({ "Text": "小于", "Value": "<" });
                    break;
                case "foreign":
                    if (this.foreign.foreignobject == "Acc.Business.Model.OfficeWorker") {
                        ss.push({ "Text": "本人", "Value": "@me" });
                        ss.push({ "Text": "直接下属", "Value": "@mex" });
                        //ss.push({ "Text": "所有下属", "Value": "@mexall" });
                    }
                    if (this.foreign.foreignobject == "Acc.Business.Model.Organization") {
                        ss.push({ "Text": "本部门", "Value": "@mo" });
                        ss.push({ "Text": "下级部门", "Value": "@mox" });
                    }
                case "comvtp":
                    //                    ss.push({ "Text": "等于", "Value": "=" });
                    //                    ss.push({ "Text": "不等于", "Value": "<>" });
                    break;
                default:
                    ss.push({ "Text": "包含", "Value": "like" });
                    break;
            }
            if (!this.required) {
                ss.push({ "Text": "为空", "Value": "@is null" });
                ss.push({ "Text": "不为空", "Value": "@is not null" });
            }
            return ss;
        },
        remove: function () {
            if (this.sf) {
                this.sf.remove();
            }
        }
    }
} 