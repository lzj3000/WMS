/**
* jsbase
* 说明：基于JQuery1.7.2
* 作者：李晓超
* 联系：xiaochao.li@qq.com
*/
var webjs = (function () {
    function mySubmit(url, data, error, success) {
        $.ajax({
            type: "POST",
            url: url,
            dataType: "json",
            data: data,
            error: function (err) {
                //$.messager.alert('异常', err.responseText, 'info');
                var obj = {};
                obj.error = true;
                obj.errormsg = err.responseText;
                if (success)
                    success(obj);
            },
            success: function (er) {
                if (success)
                    success(er);
            }
        });
    }
    function mypost(url, data, success) {
        var obj = {};
        $.ajax({
            type: "POST",
            url: url,
            dataType: "json",
            data: data,
            async: false,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                obj.error = true;
                obj.errormsg = XMLHttpRequest.responseText;
                //$.messager.alert('异常', err.responseText, 'info');
            },
            success: function (er) {
                if (success)
                    obj = success(er);
                else {
                    if (er && er.results)
                        obj = er.results;
                    else
                        obj = er;
                }
            },
            complete: function (XMLHttpRequest, textStatus) {

            }
        });
        return obj;
    }
    function md5(str) {
        return CryptoJS.MD5(str);
    }
    function sslogin(name, pass, success) {
        var ss = new socket();
        ss.init();
        ss.onmessage = function (obj) {
            webjs.loging = false;
            if (obj && obj.error) {
                $.messager.alert('异常', obj.errormsg, 'info');
                return;
            }
            webjs.guid = obj.tempid;
            webjs.username = obj.name;
            webjs.userid = obj.ID;
            webjs.obj = obj;
            webjs.obj.ss = ss;
            webjs.isLogin = obj.IsLogin;
            ss.onmessage = function (e) {
                ssmsg(e);
            }
            if (success)
                success(obj);
        }
        ss.onopen = function () {
            ss.send("login," + name + "," + pass);
        }
    }
    function ssmsg(data) {
        if (data && data !== "" && data != "1") {
            var msg = data
            if (msg.c == "scan") {
                var v = msg.MsgDesc;
                var scan = new codescan();
                scan.onGetCode(v);
                return;
            }
            if (msg.IsAlert) {
                $.messager.alert(msg.MsgTitle, msg.MsgDesc, 'warning');
            }
            else {
                webjs.addMsg(msg);
                showmsg(msg);
            }
        }
    }
    function myLogin(name, pass, success) {
        var data = "name=" + name + "&pass=" + pass;
        mySubmit(webjs.userpost + "?m=login", data, null, function (obj) {
            webjs.loging = false;
            if (obj && obj.error) {
                $.messager.alert('异常', obj.errormsg, 'info');
                return;
            }
            webjs.guid = obj.tempid;
            webjs.username = obj.name;
            webjs.userid = obj.ID;
            webjs.obj = obj;
            webjs.isLogin = obj.IsLogin;
            if (success) {
                success(obj);
                getmsg();
            }
        });
    }
    function myuppass(newpass, oldpass) {
        var data = "np=" + newpass + "&op=" + oldpass + "&guid=" + webjs.guid;
        mySubmit(webjs.userpost + "?m=uppass", data, null, function (obj) {
            if (obj && obj.error) {
                $.messager.alert('异常', obj.errormsg, 'info');
            }
            else {
                $.messager.show({ title: '消息', msg: '修改密码完成。', showType: 'show' });
            }
        });
    }
    function myOutLogin() {
        mySubmit(webjs.userpost + "?m=outlogin", "guid=" + webjs.guid, null, function (obj) {
            window.location.reload();
        });
    }

    function getmsg() {
        setTimeout(function () {
            mySubmit(webjs.userpost, "m=getmsg&guid=" + webjs.guid, function () { getmsg(); }, function (data) {
                try {
                    if (data && data !== "" && data != "1") {
                        var msg = data
                        webjs.addMsg(msg);
                        showmsg(msg);
                        getmsg();
                    }
                    else {
                        getmsg();
                    }
                }
                catch (e) {
                    getmsg();
                }
            });
        }, 60000);
    }
    function showmsg(msg) {
        $.messager.show({
            title: msg.MsgTitle,
            msg: '<b>' + msg.MsgDesc + '</b></br><a href="#" onclick="webjs.test(\'' + msg.c + '\',\'' + msg.ID + '\',\'' + msg.MsgTitle + '\',\'' + msg.mesCode + '\',this)">查看</a>',
            timeout: 5000,
            showType: 'slide'
        });
    }
    function newguid() {
        var guid = "";
        for (var i = 1; i <= 32; i++) {
            var n = Math.floor(Math.random() * 16.0).toString(16);
            guid += n;
            if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
                guid += "-";
        }
        return guid;
    }
    return {
        guid: "",
        username: "",
        obj: null,
        charpath: "webchars",
        controlpath: "webcontrols",
        basefile: "jsbase",
        postbase: "web.aspx",
        userpost: "user.aspx",
        filepost: "file.aspx",
        messageBox: [],
        login: function (name, pass, success) {
            try {
                //                var isIE = (navigator.appVersion.indexOf("MSIE") != -1) ? true : false;
                //                if (isIE)
                myLogin(name, pass, success);
                //                else
                //                    sslogin(name, pass, success);
            }
            catch (e) {
                $.messager.alert('异常', e.Message, 'info');
            }
        },
        outlogin: function () {
            myOutLogin();
        },
        submit: function (url, data, error, success) {
            try {
                if (data && data != "")
                    data = data + "&guid=" + webjs.guid;
                else
                    data = "guid=" + webjs.guid;
                mySubmit(url, data, error, success)
            }
            catch (e) {
                $.messager.alert('异常', e.Message, 'info');
            }
        },
        mysubmit: function (c, m, data, error, success) {
            webjs.submit(this.postbase + "?c=" + c + "&m=" + m, data, error, success);
        },
        newid: function () {
            return newguid();
        },
        uppass: function (newpass, oldpass) {
            myuppass(newpass, oldpass);
        },
        geturlQS: function (name) {
            var reg = new RegExp("(^|\\?|&)" + name + "=([^&]*)(\\s|&|$)", "i");
            if (reg.test(location.href)) return unescape(RegExp.$2.replace(/\+/g, " "));
            return "";
        },
        fmoney: function (val) {
            var s = parseFloat(val); //获取小数型数据
            s += "";
            if (s.indexOf(".") == -1) s += ".0"; //如果没有小数点，在后面补个小数点和0
            if (/\.\d$/.test(s)) s += "0";   //正则判断
            while (/\d{4}(\.|,)/.test(s))  //符合条件则进行替换
                s = s.replace(/(\d)(\d{3}(\.|,))/, "$1,$2"); //每隔3位添加一个，
            return s;
        },
        post: function (c, m, data) {
            try {
                if (data && data != "")
                    data = data + "&guid=" + webjs.guid;
                else
                    data = "guid=" + webjs.guid;
                return mypost(this.postbase + "?c=" + c + "&m=" + m, data, null);
            }
            catch (e) {
                var obj = {};
                obj.error = true;
                obj.errormsg = e.responseText;
                return obj;
            }
        },
        loadJs: function (name, file) {
            var js = $('#' + name);
            if (js.length == 0) {
                $("<scri" + "pt>" + "</scr" + "ipt>").attr({ src: file, type: 'text/javascript', id: name }).appendTo($('head'));
            }
        },
        getbasepath: function () {
            var p = "";
            var jb = this;
            $('script').each(function () {
                var sc = this;
                var scs = sc.src.split("/");
                if (scs.length > 0) {
                    var name = scs[scs.length - 1];
                    var n = name.split(".")[0];
                    if (n == jb.basefile) {
                        for (var i = 0; i < scs.length - 1; i++) {
                            p += scs[i] + "/";
                        }
                        return;
                    }
                }
            });
            return p;
        },
        setscript: function () {
            var path = this.getbasepath();
            var cp = path + this.charpath + "/webchar.js";
            var conp = path + this.controlpath + "/webcontrol.js";
            //this.loadJs("webchar", cp);
            // this.loadJs("webcontrol", conp);
        },
        wherelayout: function (id, applyTo, split, height) {
            var name = id + "-layout";
            var north = id + "-north";
            $("#" + applyTo).attr("id", name);
            $("#" + name).addClass("easyui-layout");
            $("#" + name).append('<div id="' + north + '" region="north"  split="' + split + '" style="height:' + height + 'px;background:#efefef;overflow:hidden;"></div><div id="center" region="center" style="overflow:hidden;"><div id="' + applyTo + '"/></div>');
            $('#' + name).panel({
                fit: true,
                title: "查询条件",
                collapsible: false,
                minimizable: false,
                maximizable: false,
                closable: false
            });
            return $('#' + north);
        },
        clone: function (obj) {
            function clone2(obj) {
                var o, obj;
                if (obj.constructor == Object) {
                    o = new obj.constructor();
                } else {
                    o = new obj.constructor(obj.valueOf());
                }
                for (var key in obj) {
                    if (o[key] != obj[key]) {
                        if (typeof (obj[key]) == 'object') {
                            o[key] = clone2(obj[key]);
                        } else {
                            o[key] = obj[key];
                        }
                    }
                }
                o.toString = obj.toString;
                o.valueOf = obj.valueOf;
                return o;
            }
            return clone2(obj);
        },
        showform: function (c, id) {
            var source = new datasource();
            source.c = c;
            source.notreload = true;
            source.init();
            var f = new childform();
            f.CommandClose = true;
            f.showSave = false;
            f.showFN = false;
            f.itemshow(source, id, source.modeldata.title);
            f.setdisabled(true);
        },
        test: function (c, id, title, mmid, sender) {
            var msg;
            for (var i in webjs.messageBox) {
                var m = webjs.messageBox[i];
                if (m.mesCode == mmid) {
                    msg = m;
                }
            }
            var source = new datasource();
            source.c = c;
            source.notreload = true;
            source.init();
            var f = new childform();
            if (msg.IsTask) {
                for (var c in source.commands) {
                    var cmd = source.commands[c];
                    if (cmd.command == "SubmitData") {
                        cmd.editshow = false;
                    }
                    if (cmd.command == "UnReviewedData") {
                        cmd.editshow = true;
                    }
                }
            }
            else {
                for (var c in source.commands) {
                    var cmd = source.commands[c];
                    if (cmd.command == "print") {
                        cmd.editshow = true;
                    }
                    else
                        cmd.editshow = false;
                }
                f.showSave = false;
                f.showFN = false;
            }
            var tool = new toolstrip();
            tool.datasource = source;
            tool.init();
            f.tool = tool;
            f.CommandClose = true;
            f.itemshow(source, id, title);
            webjs.removeMsg(mmid);
            var p = $(sender).parent(".messager-body");
            $(p).window("destroy");
        },
        removeMsg: function (id) {
            var msg;
            for (var i in webjs.messageBox) {
                msg = webjs.messageBox[i];
                if (msg.mesCode == id) {
                    webjs.messageBox.splice(i);
                    break;
                }
            }
            var c = webjs.messageBox.length;
            $("#msgaaa").text(c);
            if (msg.IsTask)
                this.submit(webjs.userpost, "m=clertmsg&code=" + id, null, null);
        },
        addMsg: function (obj) {
            webjs.messageBox.push(obj);
            var c = webjs.messageBox.length;
            $("#msgaaa").text(c);
        },
        showMsg: function () {
            for (var i in webjs.messageBox) {
                showmsg(webjs.messageBox[i]);
            }
        },
        jsonval: function (data) {
            function zh(k, v) {
                if (k == undefined || k == null || k == 'oldrow' || k == 'parent')
                    return undefined;
                else {
                    if (typeof v == "string") {
                        if (v.indexOf("{") != 0) {
                            var patrn = /^[C-Z]:\\.+\\.+$/;
                            if (patrn.exec(v)) {
                                return v;
                            }
                            else {
                                v = v.replace(/(^\s*)|(\s*$)/g, "");
                                return encodeURIComponent(v);
                            }
                        }
                        else
                            return v;
                    }
                    else
                        return v;
                }
            }
            var v = JSON.stringify(data, zh);
            return v;
        }
    }
})();
//webjs.setscript();
var socket = (function () {
    function local(soc) {
        var str = window.location.host;
        var ss = str.split(":");
        var s = ss[0];
        var loc = "ws://" + s + ":" + soc.port + "/userweb";
        return loc;
    }
    function copen(soc) {
        var loc = local(soc);
        try {
            if ("WebSocket" in window) {
                soc.ws = new WebSocket(loc);
            }
            else if ("MozWebSocket" in window) {
                soc.ws = new MozWebSocket(loc);
            }

            soc.socketcreated = true;
            soc.ws.onopen = function () {
                var myDate = new Date();
                soc.starttime = myDate.toLocaleString();
                soc.onopen();
            }
            soc.ws.onmessage = function (e) {
                if (e && e.data) {
                    var obj = null;
                    var ess = e.data.split(":");
                    if (ess[0]) {
                        if (ess[0] == "error") {
                            obj = {};
                            obj.error = true;
                            obj.errormsg = ess[1];
                        }
                    }
                    if (obj == null)
                        obj = JSON.parse(e.data);
                    soc.onmessage(obj);
                }
            }
            soc.ws.onclose = function () {
                var myDate = new Date();
                soc.endtime = myDate.toLocaleString();
                alert("用户超时退出！开始时间" + soc.starttime + "======" + "结束时间" + soc.endtime);
                soc.onclose();
            }
            soc.ws.onerror = function (e) {
                soc.onerror(e);
            }
        }
        catch (e) {

        }
    }
    return {
        port: 443,
        socketcreated: false,
        ws: null,
        init: function () {
            copen(this);
        },
        send: function (msg) {
            if (this.ws)
                this.ws.send(msg);
        },
        onopen: function () {
        },
        onmessage: function (e) {

        },
        onclose: function () {

        },
        onerror: function (e) {
            alert(e);
        }
    }
})



