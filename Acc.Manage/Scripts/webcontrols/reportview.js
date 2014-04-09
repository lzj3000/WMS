var reportview = function () {
    function rinit(rv) {
        if ($("#" + rv.applyTo + "")) {
            var name = rv.id + rv.applyTo;
            $("#" + rv.applyTo + "").attr('id', name);
            $("#" + name).append("<div data-options=\"region:'north'\" style='height: 100px'><form><table style='width: 100%; align: center;' id='" + rv.id + "tableform' cellspacing='5' cellpadding='1'></table></form></div><div data-options=\"region:'center'\"><div id='" + rv.id + "divObject' style='width: 100%; height: 100%;'><object id='" + rv.id + "objReport' type=\"text/html\" height=\"100%\" width=\"100%\"></object></div></div><div data-options=\"region:'south',split:true\" style='height: 0px;'></div>");

            var accitem = $('#mainmenu').accordion('getSelected');  //选中的accordion
            var treenode = $("#" + accitem[0].firstChild.id).tree('getSelected') || $("#" + accitem[0].firstChild.id).tree('getChecked');  //选中的tree-node
            var url = treenode.attributes.url;  //获取url
            var controller = treenode.attributes.mmm.ControllerName || "Acc.Business.Controllers.ReportBaseController";  //获取控制器名称
            var index = url.indexOf("?");

            rv.result = webjs.post(controller, "Getwheres", url.substring(index + 1));
            if (rv.result.error) {
                $.messager.alert('异常', rv.result.errormsg, 'info');
            }
            else {
                var selectTab = $('#tabcenter').tabs('getSelected');
                var h = selectTab[0].clientHeight || selectTab[0].offsetHeight;
                var w = selectTab[0].clientWidth || selectTab[0].offsetWidth;
                $("#" + name).css({ "width": w, "height": h });
                initSearch(rv);
                rv.query = "guid=" + webjs.guid + "&table=" + rv.result['TableName'].title + "&dataset=" + rv.result['DataSetName'].title + "&c=" + controller + "&" + url.substring(index + 1);
                $("#" + rv.id + "objReport").attr("data", "Reports/Report_aspx/ReportCommon.aspx?" + rv.query);
            }
        }
    }


    function initSearch(rv) {
        var sb = '<tbody>', index = 0, temp;
        for (var key in rv.result) {
            if (key == "DataSetName" || key == "TableName")
            { continue; }
            if (index % 3 == 0) {
                sb = sb + '<tr>';
            }
            temp = new textbox();
            temp.required = false;
            var value = rv.result[key];
            switch (value.wType) {
                case 5: //select
                    temp.field = value.field;
                    temp.title = value.title;
                    temp.id = value.field;
                    temp.comvtp = { isvtp: true, items: JSON.parse(value.comboxvalue) };
                    sb = sb + "<td align='right'>" + value.title + "</td><td>" + temp.box() + "</td>";
                    break;
                case 4:  //date
                case 3:
                    temp.type = "date";
                    temp.field = value.field + "_begin";
                    temp.title = "开始" + value.title;
                    temp.id = value.field + "_begin";
                    sb = sb + "<td align='right'>" + "开始" + value.title + "</td><td>" + temp.box() + "</td>";

                    if (index % 3 == 2) {
                        sb = sb + '</tr>';
                    }
                    index++;
                    if (index % 3 == 2) {
                        sb = sb + '</tr>';
                    }
                    if (index % 3 == 0) {
                        sb = sb + '<tr>';
                    }

                    temp.type = "date";
                    temp.field = value.field + "_end";
                    temp.title = "终止" + value.title;
                    temp.id = value.field + "_end";
                    sb = sb + "<td align='right'>" + "终止" + value.title + "</td><td>" + temp.box() + "</td>";

                    break;
                default: //string
                    temp.type = "string";
                    temp.field = value.field;
                    temp.title = value.title;
                    temp.id = value.field;
                    sb = sb + "<td align='right'>" + value.title + "</td><td>" + temp.box() + "</td>";
                    break;
            }

            if (index % 3 == 2) {
                sb = sb + '</tr>';
            }
            index++;
        }

        var combine = 3 - (index % 3);
        if (combine < 3) {
            for (var i = 0; i < combine; i++) {
                sb = sb + '<td align="right"></td>';
                if (i == combine - 1) {
                    sb = sb + '<td> <a id="' + rv.id + '" class="easyui-linkbutton">查 询</a></td></tr>';
                } else {
                    sb = sb + '<td></td>';
                }
            }
        }
        else {
            sb = sb + '<tr><td align="right"></td><td></td><td align="right"></td><td></td><td align="right"></td><td><a id="' + rv.id + '" class="easyui-linkbutton">查 询</a></td></tr>';
        }
        sb = sb + '</tbody>';
        $("#" + rv.id + "tableform").append(sb);
        $("#" + rv.id).click(function () { rv.mySearch(rv); });
    }

    function Search(rv) {
        var whereList = '[', where, result, tmpquery;

        var inputs = $("#" + rv.id + "tableform input");
        for (var i = 0; i < inputs.length; i++) {
            var iname = inputs[i].name;
            if (iname == "" || iname == null) {
                continue;
            }
            if (iname.indexOf("_begin") > -1) {
                iname = iname.substring(0, iname.indexOf("_begin"));
                if (rv.result[iname]) {
                    if (inputs[i].value) {
                        whereList = whereList + '{"ColumnName":"' + iname + '","StartFH":"","Symbol":">=","Value":"' + inputs[i].value + '","EndFH":"","Relation":"and","Type":"date"},';
                    }
                }
            } else if (iname.indexOf("_end") > -1) {
                iname = iname.substring(0, iname.indexOf("_end"));
                if (rv.result[iname]) {
                    if (inputs[i].value) {
                        whereList = whereList + '{"ColumnName":"' + iname + '","StartFH":"","Symbol":"<=","Value":"' + inputs[i].value + '","EndFH":"","Relation":"and","Type":"date"},';
                    }
                }
            } else {
                if (rv.result[iname]) {
                    if (rv.result[iname].wType == 5 && isInteger(inputs[i].value)) {                   
                        whereList = whereList + '{"ColumnName":"' + iname + '","StartFH":"","Symbol":"=","Value":"' + inputs[i].value + '","EndFH":"","Relation":"and","Type":"int"},';
                    }
                    else {
                        if (inputs[i].value) {
                            whereList = whereList + '{"ColumnName":"' + iname + '","StartFH":"","Symbol":"like","Value":"' + inputs[i].value + '","EndFH":"","Relation":"and","Type":""},';
                        }
                    }
                }
            }
        }
        if (whereList.length > 1) {
            whereList = whereList.substring(0, whereList.length - 1);
        }
        whereList = whereList + ']';
        where = rv.query + "&wheres=" + whereList;

        //先删除后添加 否则谷歌浏览器不刷新
        var d = $("#" + rv.id + "objReport").remove()
        $(d).attr("data", "Reports/Report_aspx/ReportCommon.aspx?" + where);
        $("#" + rv.id + "divObject").append(d);
    }

    function isInteger( str ){  
    var regu = /^[-]{0,1}[0-9]{1,}$/;
    return regu.test(str);
}



    return {
        id: "",
        applyTo: "",
        result: null,
        query: "",
        init: function () {
            this.id = webjs.newid();
            rinit(this);
        },
        mySearch: function (rv) {
            Search(rv);
        }
    }
}