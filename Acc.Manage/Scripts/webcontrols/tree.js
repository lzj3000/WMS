var tree = function () {
    function cinit(t) {
        t.name = t.id + "-" + t.applyTo
        cinitsource(t);
        var tree = '<ul id="' + t.name + '" class="easyui-tree"/>';
        var layout = '<div class="easyui-layout" fit="true" border="false"><div border="true" region="north" style="height:32px;"><div scroll=no id="' + t.id + '_search" /></div><div border="false" region="center">' + tree + '</div></div></div>';
        var div = '<div id="' + t.id + '" border="false" class="easyui-panel" style="width:' + t.width + 'px;height:auto;">' + tree + '</div>';
        if (t.applyTo != "") {
            $("#" + t.applyTo).attr("id", t.id + "_tree");
            if (t.ismenu)
                $("#" + t.id + "_tree").append(layout);
            else
                $("#" + t.id + "_tree").append(div);
            // t.render();
            if (t.isload) {
                t.load();
            }
        }
        return div;
    }
    function cinitsource(t) {
        if (t.datasource != null) {
            if (t.datasource.treemodel) {
                t.displayid = t.datasource.treemodel.displayid;
                t.displayname = t.datasource.treemodel.displayname;
                t.parentid = t.datasource.treemodel.parentid;
                t.modelname = t.datasource.treemodel.modelname;
                t.tablename = t.datasource.treemodel.tablename;
                t.displayfield = t.datasource.treemodel.displayfield;
            }
        }
    }
    function expand(t, node) {
        var name = "#" + t.name;
        var rows = loadchild(t, node.id);
        if (rows != null) {
            var n = $(name).tree("find", node.id + "-child");
            if (n != null)
                $(name).tree('remove', n.target);
            var child = $(name).tree('getChildren', node.target);
            if (child) {
                for (var r in child) {
                    try {
                        $(name).tree('remove', child[r].target);
                    }
                    catch (e) { }
                }
            }
            $(name).tree('append', { parent: node.target, data: rows });
        }
    }
    function inittree(t) {
        if (!t.inttree) {
            var name = "#" + t.name;
            $(name).tree({
                checkbox: t.checkbox,
                animate: false,
                dnd: t.dnd,
                cascadeCheck: t.cascadeCheck,
                onlyLeafCheck: t.onlyLeafCheck,
                onCollapse: function (node) {

                },
                onExpand: function (node) {
                    if (t.onExpand(node) === true) {
                        expand(t, node);
                    }
                },
                onBeforeSelect: function (node) {
                    return !t.disabled;
                },
                onSelect: function (node) {
                    t.onSelect(node);
                },
                onDrop: function (target, source, point)
                { }
            });
            t.inttree = true;
            if (t.ismenu) {
                var tool = new toolstrip();
                tool.applyTo = t.id + "_search";
                tool.datasource = t.datasource;
                tool.onlysearch = true;
                tool.isAsearch = false;
                tool.init();
                tool.clickcommand = function (sender, data) {
                    updateload(t, data);
                }
            }
        }
    }
    function updateload(t, data) {
        var nodes = $('#' + t.name).tree('getRoots');
        for (var i in nodes) {
            $('#' + t.name).tree('remove', nodes[i].target);
        }
        if ((data.field && data.field == "*") || data.field == "") {
            t.load();
        }
        else {
            t.datasource.treeload(data.value, function (rows) {
                if (rows && rows.error) {
                    $.messager.alert('异常', rows.errormsg, 'error');
                }
                else {
                    var nr = getfromatrows(t, rows.rows);
                    addnode(t, null, nr);
                }
            });
        }
    }
    function getfromatrows(t, rows) {
        var treerows = [];
        for (var r in rows) {
            treerows[r] = formatdata(t, rows[r]);
        }
        return treerows;
    }
    function formatdata(t, row) {
        var o = {};
        o.id = row[t.displayid];
        o.text = row[t.displayfield];
        o.attributes = row;
        if (row.CC && row.CC > 0) {
            o.state = "closed";
            o.children = [{ id: o.id + "-child", Text: ""}];
        }
        return o;
    }
    function newrow(t) {
        var obj = new Object();
        if (t.datasource) {
            for (var n in t.datasource.items) {
                var f = t.datasource.items[n];
                obj[f.field] = "";
            }
        }
        return obj;
    }
    function addnode(t, node, rows) {
        t.onAdding(node, rows);
        $('#' + t.name).tree('append', {
            parent: (node ? node.target : null),
            data: rows
        });
    }
    function loadchild(tree, id, where) {
        var data = tree.tablename + "." + tree.parentid + "=" + id;
        if (where != undefined) {
            data += " and " + tree.tablename + "." + where;
        }
        if (tree.datasource != null) {
            var e = tree.datasource.treeload(data);
            var rows = getfromatrows(tree, e.rows);
            return rows;
        }
        return null;
    }
    function loadchildall(tree, id, where) {
        var rows = loadchild(tree, id, where);
        getcall(rows, tree, where);
        return rows;
    }
    function getcall(rows, t, where) {
        if (rows != null) {
            for (var i in rows) {
                var cr = rows[i];
                if (cr.children) {
                    cr.children = loadchild(t, cr.attributes.ID, where);
                    if (cr.children != null);
                    {
                        getcall(cr.children, t, where);
                    }
                }
            }
        }
        return rows;
    }
    return {
        id: "",
        applyTo: "",
        displayname: "",
        displayid: "",
        parentid: "",
        datasource: null,
        width: 200,
        checkbox: false,
        cascadeCheck: false,
        onlyLeafCheck: false,
        isload: true,
        region: "west",
        title: "",
        isevent: false,
        dnd: false,
        disabled: false,
        ismenu: false,
        init: function () {
            this.id = webjs.newid();
            return cinit(this);
        },
        render: function () {
            $("#" + this.id).panel();
            $("#" + this.name).tree();
        },
        load: function (id, where) {
            inittree(this);
            if (!id) {
                id = 0;
            }
            var rows = loadchild(this, id, where);
            if (rows != null)
                addnode(this, null, rows);
        },
        loadAll: function (id, where) {
            inittree(this);
            if (!id) {
                id = 0;
            }
            var rows = loadchildall(this, id, where);
            if (rows != null) {
                addnode(this, null, rows);
            }
        },
        onSelect: function (node) {

        },
        onAdding: function (node, rows) {

        },
        onExpand: function (node) {
            return true;
        },
        expand: function (id) {
            var node = $('#' + this.name).tree('find', id);
            if (node)
                expand(this, node);
        },
        formatnode: function (row) {
            return formatdata(this, row);
        },
        add: function (node) {
            var t = this;
            var sn = $('#' + this.name).tree('getSelected');
            if (node == undefined) {
                var obj = newrow(t);
                if (sn != null) {
                    obj[t.parentid] = sn.attributes[t.displayid];
                    //obj["FOREIGN" + t.displayname] = node.attributes[t.displayname];
                    obj[t.displayname] = sn.attributes[t.displayfield];
                }
                var rows = [{
                    id: -1,
                    text: '新' + t.datasource.modeldata.title,
                    state: 'open',
                    attributes: obj
                }];
                addnode(this, sn, rows);
                var newnode = $('#' + this.name).tree('find', -1);
                $('#' + this.name).tree('select', newnode.target);
            }
            else {
                var rows = [node];
                addnode(this, sn, rows);
            }
        },
        addroot: function (node) {
            if (node == undefined) {
                var t = this;
                var obj = newrow(t);
                obj[t.parentid] = 0;
                $('#' + this.name).tree('append', {
                    parent: null,
                    data: [{
                        id: -1,
                        text: '新' + t.datasource.modeldata.title,
                        state: 'open',
                        attributes: obj
                    }]
                });
                var newnode = $('#' + this.name).tree('find', -1);
                $('#' + this.name).tree('select', newnode.target);
            }
            else {
                $('#' + this.name).tree('append', {
                    parent: null,
                    data: [node]
                });
            }
        },
        update: function (row) {
            var node = $('#' + this.name).tree('getSelected');
            if (node) {
                var d = formatdata(this, row);
                node.id = d.id;
                node.text = d.text;
                node.attributes = row;
                $('#' + this.name).tree('update', node);
            }
        },
        getparent: function (node) {
            if (node && node.target)
                return $('#' + this.name).tree('getParent', node.target);
            return null;
        },
        remove: function () {
            var node = $('#' + this.name).tree('getSelected');
            var p = $('#' + this.name).tree('getParent', node.target);
            $('#' + this.name).tree('remove', node.target);
            if (p != null) {
                // this.select(p.id);
                $('#' + this.name).tree('select', p.target);
            }
            else {
                this.datasource.selectItem = null;
            }
        },
        select: function (id) {
            var node = $('#' + this.name).tree('find', id);
            if (node)
                $('#' + this.name).tree('select', node.target);
        },
        check: function (id, c) {
            var node = $('#' + this.name).tree('find', id);
            var cmd = c ? "check" : "uncheck";
            if (node)
                $('#' + this.name).tree(cmd, node.target);
        },
        uncheck: function () {
            var nodes = $('#' + this.name).tree('getChecked');
            for (var i in nodes) {
                $('#' + this.name).tree('uncheck', nodes[i].target);
            }
        },
        getchecked: function () {
            var tree = $('#' + this.name);
            return tree.tree('getChecked');
        },
        saveupdate: function (row) {
            var node = $('#' + this.name).tree('getSelected');
            if (node && node.target) {
                var id = row[this.displayid];
                if (id != "0")
                    node.id = id;
                node.text = row[this.displayfield]
                for (var i in row) {
                    var r = row[i];
                    var tyr = typeof r;
                    if (tyr == "object" && (r != null && r.length > 0)) {
                        row[i] = undefined;
                    }
                }
                node.attributes = row;
                $('#' + this.name).tree('update', node);
                return node;
            }
            return null;
        },
        expandAll: function () {
            var roots = $('#' + this.name).tree('getRoots');
            for (var i in roots) {
                $('#' + this.name).tree('expandAll', roots[i].target);
            }
        },
        getchild: function (node) {
            var child = null;
            if (typeof node === 'number')
                node = $('#' + this.name).tree('find', node);
            if (node)
                child = $("#" + this.name).tree('getChildren', node.target);
            return child;
        },
        loaddata: function (data) {
            inittree(this);
            $("#" + this.name).tree('loadData', data);
        },
        search: function (data) {
            if (data.field == undefined || data.field == "")
                data.field = "*";
            updateload(this, data);
        }
    }
}