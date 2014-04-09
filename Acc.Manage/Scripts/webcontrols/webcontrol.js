var webctrl = function () {
    return {
        setscript: function () {
            var path = webjs.getbasepath();
            var datagrid = path + webjs.controlpath + "/datagrid.js";
            var toolstrip = path + webjs.controlpath + "/toolstrip.js";
            var singletable = path + webjs.controlpath + "/page/singletable.js";
            webjs.loadJs("datagrid", datagrid);
            webjs.loadJs("toolstrip", toolstrip);
            webjs.loadJs("singletable", singletable);
        },
        createpage: function () {
            var t = new singletable();
            return t;
        }
    }
} ()
//webctrl.setscript();