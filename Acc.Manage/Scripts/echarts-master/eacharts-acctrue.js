/**  
*************************事件说明********************************
*       RESTORE: 'restore',  //还原
*       CLICK: 'click',  //单击
*       HOVER: 'hover',  //鼠标移到该元素
*       MOUSEWHEEL: 'mousewheel',
* 业务交互逻辑
*       DATA_CHANGED: 'dataChanged', //数据更改  如果拖动数据项
*       DATA_ZOOM: 'dataZoom',  //数据区域缩放
*       DATA_RANGE: 'dataRange',
*       LEGEND_SELECTED: 'legendSelected',  //图例选中(单击)
*       MAP_SELECTED: 'mapSelected',   //地图区域选中(单击)
*       PIE_SELECTED: 'pieSelected',  //饼图选择
*       MAGIC_TYPE_CHANGED: 'magicTypeChanged',  //当值线图和柱状图互换 
*
*************************使用说明********************************
*  1、创建对象(容器ID,控制器)
* var ec = new ECharts("main", "Acc.Business.WMS.XHY.Controllers.EChartLineController");
*  2、添加事件  目前只支持"click", "hover", "dataChanged", "mapSelected", "pieSelected"
*     ec.addEvent("click", function (parm,opt) {
*         var msg="";
*         if (opt) {
*             msg += opt.series[parm.seriesIndex].name;
*             msg += opt.series[parm.seriesIndex].data[parm.dataIndex];
*             alert(msg);
*         }
*     });
*   3、初始化
*     ec.init();
*
**/
(function (global) {
    //全局变量
    var fileLocation = "../../Scripts/echarts-master/echarts-original-map";
    var hasParam = false;  //是否有查询条件
    //默认的事件类型
    var defaultEvents = ["click", "hover", "dataChanged", "mapSelected", "pieSelected"];

    //配置加载项
    require.config({
        paths: {
            echarts: fileLocation,  //引入echarts
            'echarts/chart/line': fileLocation, //折线图
            'echarts/chart/bar': fileLocation,  //柱形图
            'echarts/chart/map': fileLocation,  //地图（支持中国及全国34个省市自治区地图） 
            'echarts/chart/pie': fileLocation //饼图
        }
    });

    //初始化
    function Init() {
        var self = this;
        //删除子节点
        $("#" + self.newId).empty();
        //初始化查询界面
        initSearch(self);

        var selfDom = $("#" + self.newId)[0];
        var parenth = selfDom.parentNode.clientHeight || selfDom.parentNode.offsetHeight;
        while (parenth <= 0) {
            selfDom = $(selfDom)[0].parentNode;
            parenth = selfDom.clientHeight || selfDom.offsetHeight;
        }
        var h;
        if (hasParam) {
            h = parenth - 32 - 5;
        }
        else {
            h = parenth;
        }
        $('<div style="height:' + h + 'px;border:1px solid #ccc;padding:1px;" id="' + self.newId + 'Echart"></div>').appendTo($("#" + self.newId));
        self.domMain = document.getElementById(self.newId + 'Echart');
        //        //进行按需加载  同时渲染
        //        require(
        //                [
        //                'echarts',
        //                'echarts/chart/line',
        //                'echarts/chart/map',
        //                'echarts/chart/pie',
        //                'echarts/chart/bar'
        //                ],
        //                function (ec) {
        //                    var echarts = ec;
        //                    self.option = webjs.post(self.controller, "load", "");
        //                    if (self.myChart && self.myChart.dispose) {
        //                        self.myChart.dispose();
        //                    }
        //                    self.myChart = echarts.init(self.domMain);

        //                    self.myChart.setOption(self.option);
        //                    //自动调整大小
        //                    if (global.onresize)
        //                        global.onresize = self.myChart.resize;
        //                    else
        //                        window.onresize = self.myChart.resize;


        //                    addEvents(self);
        //                });

    }



    //生成查询条件
    function initSearch(context) {
        var tbform = $('<div style="border:1px solid #ccc;padding:1px;" id="tableform"></div>');
        tbform.appendTo($("#" + context.newId));
        var ds = new datasource();
        ds.c = context.controller;
        ds.init();
        ds.onLoad = function (data) {
            if (context.myChart && context.myChart.dispose) {
                context.myChart.dispose();
            }
            require(
                [
                'echarts',
                'echarts/chart/line',
                'echarts/chart/map',
                'echarts/chart/pie',
                'echarts/chart/bar'
                ],
                function (ec) {
                    var echarts = ec;
                    context.option = data;

                    context.myChart = echarts.init(context.domMain);

                    context.myChart.setOption(context.option);
                    //自动调整大小
                    if (global.onresize)
                        global.onresize = context.myChart.resize;
                    else
                        window.onresize = context.myChart.resize;


                    addEvents(context);
                });
        }
        var t = new toolstrip();
        t.datasource = ds;
        t.applyTo = "tableform";
        t.issearch = true;
        t.onlysearch = false;
        t.ishelp = false;
        t.init();

        //判断是否存在查询条件
        if (ds.modeldata.childitem && ds.modeldata.childitem.length > 0) {
            for (var i = 0; i < ds.modeldata.childitem.length; i++) {
                if (ds.modeldata.childitem[i].visible) {
                    hasParam = true;
                    break;
                }
            }
        }
    }

    function ECharts(parent, c, notGuid) {
        this.myChart;  //echart对象集合
        this.domMain;  //容器对象
        this.controller;  //控制器
        this.newId;  //guid
        this.events = new Dictionary(); ; //事件集合
        this.option;
        this.isFirstSearch = true;  //是否第一次初始化查询
        if (c == undefined || c == "" || c == null) {
            alert("参数c不能为空.");
        }
        else {
            if (notGuid) {
                this.newId = parent;
            }
            else {
                this.newId = webjs.newid() + parent;
                $("#" + parent).attr("id", this.newId);
            }
            this.controller = c;
        }
    }

    //查询  
    function Search(context, params) {
        context.isFirstSearch = false;
        if (context.myChart && context.myChart.clear) {
            context.myChart.clear();
        }
        context.option = webjs.post(context.controller, "load", params);
        context.myChart.setOption(context.option, true);
    }

    //注册事件
    function addEvents(self) {
        if (self.events) {
            for (var i = 0; i < self.events.Keys.length; i++) {
                var eventType = self.events.Keys[i];
                var opt = self.option;
                var fun = self.events.Values[i];
                self.myChart.on(eventType, function (parm) {
                    fun(parm, opt);
                });
            }
        }
    }

    //自定义字典  只是支持 key:string  value:fun
    function Dictionary() {
        var self = this;
        this.Keys = new Array();
        this.Values = new Array();
        this.Add = function (key, value) {
            if (typeof key == "string" && typeof value == "function") {
                if (ContainsKey(key)) {
                    throw "key值无效或者key值已经添加到字典.";
                }
                else {
                    if (isValid(key)) {
                        this.Keys.push(key);
                        this.Values.push(value);
                    }
                    else {
                        throw "key值无效.";
                    }
                }
            }
            else {
                throw "key/value类型错误.key值必须是字符串类型,value是函数类型.";
            }
        }

        function ContainsKey(key) {
            for (var i = 0; i < self.Keys.length; i++) {
                if (key == self.Keys[i]) {
                    return true;
                }
            }
            return false;
        }

        function isValid(key) {
            var valid = false;
            for (var i = 0; i < defaultEvents.length; i++) {
                if (key == defaultEvents[i]) {
                    valid = true;
                }
            }
            return valid;
        }
    }


    //绑定事件
    function AddEvent(type, fun) {
        this.events.Add(type, fun);
    }

    //option克隆 
    function GetOption() {
        if (this.myChart) {
            return this.myChart.getOption();
        }
    }

    //注意：所有ECharts对象共享prototype内存
    ECharts.prototype = {
        init: Init,
        search: Search,
        addEvent: AddEvent
    };
    global.ECharts = ECharts;
})(this);
  