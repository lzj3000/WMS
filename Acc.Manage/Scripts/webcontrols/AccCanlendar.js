/**  
*************************简要说明********************************
* 此版本日历控件仿照OutLook日期控件编写
*options{   //参数
*   weeks:周
*   month:月
*   hours:时
*   renderType:渲染类型 {month,week,day} 
*   cDate：日期对象
*   buttonText:{   //按钮
*        today: "今天",
*        month: "月",
*        week: "周",
*        day: "日"
*        prev: "&lsaquo;",
*        next: "&rsaquo;"
*       }
*   isShowHeader: true,  //是否显示表头
*   isShowType: true,  //是否显示表头右侧【日】【周】【年】
*   ondayDbClick: null,  //月模式下 天单击
*   dayHalfDbClick: null, //星期、日 模式下的天单击                
*   isShowTask: true  //是否加载代办任务
*}
*
*以上参数可以自定义
*************************使用说明********************************
*1、定义参数 
*   var option opt={}
*2、实例化
* var cancedar= $("#container").AcctureCancendar(opt);
**/
(function ($) {
    //参数
    var options = {
        weeks: new Array("星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"),
        months: new Array("1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"),
        hours: new Array("0:00", "1:00", "2:00", "3:00", "4:00", "5:00", "6:00", "7:00", "8:00", "9:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00"),
        renderType: 'month',
        cwidth: 0,  //容器宽
        cheigh: 0,  //容器高
        today: new Date(),  //当天
        cDate: null,  //日期对象
        buttonText: {
            today: "今天",
            month: "月",
            week: "周",
            day: "日",
            prev: "<",  //&lsaquo;
            next: ">"  //&rsaquo;
        },
        isShowHeader: true,  //是否显示表头
        isShowType: true,  //是否显示表头右侧【日】【周】【年】
        ondayDbClick: null,  //月模式下 天单击
        dayHalfDbClick: null, //星期、日 模式下的天单击                
        isShowTask: true,  //是否加载代办任务
        cStyles: {  //样式
            content: {   //代办任务容器
                minheight: "20px",
                maxheight: "32px"
            }
        }
    }

    //当前上下文
    var Context = {
        container: null,
        header: {
            btnPrev: null,
            btnNext: null,
            btnToday: null,
            title: null
        },
        body: null,   //用户存放日历主体部分
        weekDate: null,  //用户存放【周】模式下的一周日期   Array类型
        waitTask: null   //代办任务
    }

    $.fn.AcctureCancendar = function (opt) {
        //初始化
        options = myCopy(opt, options);
        var self = Context.container = this;
        //容器进行清空  防止一个容器重复添加
        self.empty();

        options.cwidth = self.width();
        options.cheigh = self.height();
        options.cDate = ConvertDateToObject(options.today);

        //是否显示 日历title
        if (options.isShowHeader) {
            var h = Header(self, options);
            self.append(h);
        }

        switch (options.renderType) {
            case 'day':
                return RenderDay(self, options);
                break;
            case 'week':
                return RenderWeek(self, options);
                break;
            default:
                return RenderMonth(self, options);
                break;
        }
    }

    //渲染月
    function RenderMonth(parent, options) {
        CommonUpdate('month');
        var tb = $('<table style="width: 100%;height: 100%;" class="tb-cancedar" cellSpacing="0"></table>').append(RenderMonthWeek()).append(RenderMonthBody(options.cDate, options));
        Context.body.append(tb);
        parent.append(Context.body);
        return parent;
    }

    //渲染星期
    function RenderWeek(parent, options) {
        CommonUpdate('week');
        var thead = RenderWeekWeek(options.cDate, options);
        var tbody = RenderWeekBody(options);
        Context.body.append(tbody);
        parent.append(Context.body);

        //自动计算滚动条宽度  
        var h = $(".week-header").outerHeight() || $(".week-header").outerHeight();
        $(".week-header").height(h - 0.7);

        return parent;
    }

    //渲染日
    function RenderDay(parent, options) {
        CommonUpdate('day');
        var thead = RenderDayWeek(options.cDate, options);
        var tbody = RenderDayBody(options.cDate);
        Context.body.append(tbody);
        parent.append(Context.body);
        return parent;
    }

    //渲染表头 
    function Header(self, options) {
        var headTable = $('<table style=" width:100%;" class="tb-header" cellspacing="0"></table>');
        var tr = $('<tr></tr>');
        var trLeft = $('<td class="tb-header-left"></td>');
        Context.header.btnPrev = $('<span class="tb-button tb-button-prev"></span>').append($('<span class="tb-text-arrow">' + options.buttonText.prev + '</span>').click(function (event) { PrevClick(self, options); event.stopPropagation(); })).hover(function () { $(this).addClass("hover"); }, function () { $(this).removeClass("hover"); });
        Context.header.btnNext = $('<span class="tb-button tb-button-next"></span>').append($('<span class="tb-text-arrow">' + options.buttonText.next + '</span>').click(function (event) { NextClick(self, options); event.stopPropagation(); })).hover(function () { $(this).addClass("hover"); }, function () { $(this).removeClass("hover"); });
        Context.header.btnToday = $('<span class="tb-button tb-button-today">' + options.buttonText.today + '</span>').click(function (event) { CurrentClick(self, options); event.stopPropagation(); }).hover(function () { $(this).addClass("hover"); }, function () { $(this).removeClass("hover"); });
        Context.header.title = $('<td class="tb-header-center .tb-header-title">' + GetTitleText() + '</td>');
        var tdRight = $('<td class="tb-header-right"></td>');
        //是否显示 【日】【周】【年】
        if (options.isShowType) {
            $('<span class="tb-button">' + options.buttonText.month + '</span>').bind('click', function (event) { return RenderMonth(self, options); event.stopPropagation(); }).appendTo(tdRight).hover(function () { $(this).addClass("hover"); }, function () { $(this).removeClass("hover"); });
            $('<span class="tb-button">' + options.buttonText.week + '</span>').bind('click', function (event) { return RenderWeek(self, options); event.stopPropagation(); }).appendTo(tdRight).hover(function () { $(this).addClass("hover"); }, function () { $(this).removeClass("hover"); });
            $('<span class="tb-button">' + options.buttonText.day + '</span>').bind('click', function (event) { return RenderDay(self, options); event.stopPropagation(); }).appendTo(tdRight).hover(function () { $(this).addClass("hover"); }, function () { $(this).removeClass("hover"); });
        }
        trLeft.append(Context.header.btnPrev).append(Context.header.btnNext).append(Context.header.btnToday);
        tr.append(trLeft).append(Context.header.title).append(tdRight).appendTo(headTable);
        return headTable;
    }

    //单击上一个
    function PrevClick(parent, options) {
        switch (options.renderType) {
            case 'day':
                preDay(options.cDate);
                RenderDay(parent, options);
                break;
            case 'week':
                for (var i = 0; i < 7; i++) {
                    preDay(options.cDate);
                }
                return RenderWeek(parent, options);
                break;
            default:
                preMonth(options.cDate);
                return RenderMonth(parent, options);
                break;
        }
    }

    //单击下一个
    function NextClick(parent, options) {
        switch (options.renderType) {
            case 'day':
                nextDay(options.cDate);
                RenderDay(parent, options);
                break;
            case 'week':
                for (var i = 0; i < 7; i++) {
                    nextDay(options.cDate);
                }
                return RenderWeek(parent, options);
                break;
            default:
                nextMonth(options.cDate);
                return RenderMonth(parent, options);
                break;
        }
    }

    //单击今天
    function CurrentClick(parent, options) {
        options.cDate = ConvertDateToObject(options.today);
        switch (options.renderType) {
            case 'day':
                RenderDay(parent, options);
                break;
            case 'week':
                return RenderWeek(parent, options);
                break;
            default:
                return RenderMonth(parent, options);
                break;
        }
    }

    //封装一些公共操作
    function CommonUpdate(type) {
        options.renderType = type;
        if (options.isShowHeader) {
            Context.header.title.html(GetTitleText());
        }
        if (options.isShowTask) {
            var btime, etime;
            switch (type) {
                case "month":
                    btime = options.cDate.y + '-' + (options.cDate.m + 1) + '-' + 1;
                    etime = options.cDate.y + '-' + (options.cDate.m + 1) + '-' + GetMaxDayInMonth(options.cDate.m, options.cDate.y);
                    break;
                case "week":
                    var d = new Date(options.cDate.y, options.cDate.m, options.cDate.d);
                    var tempDate = {};
                    myCopy(options.cDate, tempDate);
                    var start = d.getDay();
                    for (var i = 0; i < start; i++) {
                        preDay(tempDate);
                    }
                    btime = cDateTostring(tempDate);
                    for (var i = 0; i < 6; i++) {
                        nextDay(tempDate);
                    }
                    etime = cDateTostring(tempDate);
                    break;
                default:
                    btime = etime = cDateTostring(options.cDate);
                    break;
            }
            Context.waitTask = GetWaitTasks(btime, etime);
        }
        if (Context.body != null) {
            Context.body.empty();
        }
        else {
            Context.body = $('<div style="padding:0px; margin:0px;"></div>');
        }
    }

    //构建星期  --月
    function RenderMonthWeek() {
        var sb = '<thead><tr class="head-week">';
        for (var i = 0; i < options.weeks.length; i++) {
            if (i % 7 == 0) {
                sb += '<th style="width:14.3%;" class="head-weekday th-left">' + options.weeks[i] + '</th>';
            }
            else {
                sb += '<th style="width: 14.3%;" class="head-weekday">' + options.weeks[i] + '</th>';
            }
        }
        sb += '</tr></thead>';
        return $(sb)
    }

    //渲染主体 月
    function RenderMonthBody(cDate, options) {
        var body = $('<tbody></tbody>');
        var tmpDate = {};
        myCopy(cDate, tmpDate);

        var cur_firstDate = new Date(tmpDate.y, tmpDate.m, 1);
        var cur_firstDay = cur_firstDate.getDay();
        var cur_month = tmpDate.m;
        cur_firstDay = cur_firstDay == 0 ? 7 : cur_firstDay;
        preMonth(tmpDate);
        var pre_days = GetMaxDayInMonth(tmpDate.m, tmpDate.y);
        tmpDate.d = pre_days - cur_firstDay + 1;

        var today = ConvertDateToObject(options.today);

        //固定显示6周
        var tr = null, firstClass = '', dayClass = '', j = 0; ;
        for (var i = 0; i < 42; i++) {
            if (i % 7 == 0) {
                tr = $(dc('tr')).appendTo(body);
                firstClass = ' td-left';
            }
            else {
                firstClass = '';
            }
            if (today.m == tmpDate.m && today.d == tmpDate.d) {
                dayClass = 'body-today' + firstClass;
            } else {
                dayClass = (cur_month == tmpDate.m) ? 'body-day' + firstClass : 'body-other-day' + firstClass;
            }

            var td = $('<td class="' + dayClass + '" curDate="' + cDateTostring(tmpDate) + '"></td>')
            .append($('<div class="body-day-num">' + tmpDate.d + '</div>'))
            .dblclick(function (event) {
                createTask($(this).attr('curDate'), "");
                event.stopPropagation();
            })
            .click(function (event) {
                contentSelected(this);
                event.stopPropagation();
            });

            var content = $('<div class="body-day-content" style="min-height:' + options.cStyles.content.minheight + '; max-height:' + options.cStyles.content.maxheight + ';" ></div>');
            if (options.isShowTask) {
                var tasks = GetDayTask(cDateTostring(tmpDate));
                if (tasks) {
                    for (var j = 0; j < tasks.length; j++) {
                        $('<div class="event-div-month" taskId="' + tasks[j].Id + '">' + tasks[j].nodeDesc + '</div>')
                        .dblclick(function (event) { createTask("", "", $(this).attr('taskId')); event.stopPropagation(); })
                        .appendTo(content);
                    }
                }
            }
            td.append(content);
            tr.append(td);

            nextDay(tmpDate);
        }
        return body;
    }

    //渲染表头  星期  缩小后  样式变形暂时舍弃
    function RenderWeekWeek(cDate, options) {
        //        var d = new Date(cDate.y, cDate.m, cDate.d);
        //        var tempDate = {};
        //        myCopy(cDate, tempDate);
        //        var start = d.getDay();
        //        for (var i = 0; i < start; i++) {
        //            preDay(tempDate);
        //        }
        //        Context.weekDate = new Array();
        //        for (var i = 0; i < options.weeks.length; i++) {
        //            var tDate = {};
        //            myCopy(tempDate, tDate);
        //            Context.weekDate.push(tDate);
        //            nextDay(tempDate);
        //        }

        //        var sb = '<div class="week-header">';
        //        sb += '<table style="width: 100%;height: 100%;" cellspacing="0" style="padding:0px; margin:0px;">'
        //        sb += '<thead><tr>';
        //        sb += '<th class="th-left head-weekday" width="25px"></th>';
        //        for (var i = 0; i < options.weeks.length; i++) {
        //            sb += '<th class="head-weekday" width="13.5%" >' + Context.weekDate[i].d + '日  ' + options.weeks[i] + '</th>';
        //        }
        //        sb += '</tr></thead>';
        //        sb += '<tbody><tr>';
        //        sb += '<td class="td-left body-allday" width="25px" >全天</td>';
        //        for (var i = 0; i < Context.weekDate.length; i++) {
        //            sb += '<td class="body-allday" width="13.5%" curDate="' + cDateTostring(Context.weekDate[i]) + '" ></td>';
        //        }
        //        sb += '</tr></tbody>';
        //        sb += '</table>';
        //        sb += '<div class="head-weekday" style="height:1px;"></div>';
        //        sb += '</div>';
        //        return $(sb);
    }

    //渲染主体  星期
    function RenderWeekBody(options) {
        //head
        var d = new Date(options.cDate.y, options.cDate.m, options.cDate.d);
        var tempDate = {};
        myCopy(options.cDate, tempDate);
        var start = d.getDay();
        for (var i = 0; i < start; i++) {
            preDay(tempDate);
        }
        Context.weekDate = new Array();
        for (var i = 0; i < options.weeks.length; i++) {
            var tDate = {};
            myCopy(tempDate, tDate);
            Context.weekDate.push(tDate);
            nextDay(tempDate);
        }
        var sbhead = "", sbday = "";
        sbhead += '<thead><tr>';
        sbhead += '<th class="th-left head-weekday" width="25px"></th>';
        for (var i = 0; i < options.weeks.length; i++) {
            sbhead += '<th class="head-weekday" width="13.5%" >' + Context.weekDate[i].d + '日  ' + options.weeks[i] + '</th>';
        }
        sbhead += '</tr></thead>';
        sbday += '<tr>';
        sbday += '<td class="td-left body-allday" width="25px" >全天</td>';
        for (var i = 0; i < Context.weekDate.length; i++) {
            sbday += '<td class="body-allday" width="13.5%" curDate="' + cDateTostring(Context.weekDate[i]) + '" ></td>';
        }
        sbday += '</tr>';

        //body
        var body = $('<div id="weekbody" class="week-body"></div>');
        var tb = $('<table style="width: 100%;height: 100%;" class="tb-header">');
        var tbody = $('<tbody></tbody>');
        tb.append($(sbhead));
        tbody.append($(sbday));
        tb.append(tbody).appendTo(body);
        for (var i = 0; i < options.hours.length; i++) {
            var trup = $(dc('tr')).appendTo(tbody);
            trup.append($('<td class="td-left body-day" rowspan="2" width="25px" >' + options.hours[i] + '</td>'));
            for (var j = 0; j < Context.weekDate.length; j++) {
                var tdup = $('<td class="body-day-up" width="13.5%" curDate="' + cDateTostring(Context.weekDate[j]) + ' ' + i + ':00:00" ></td>')
               .dblclick(function () {
                   var cdate = $(this).attr("curDate");
                   createTask(RegExpDay(cdate), RegExpTime(cdate));
               })
                .click(function () {
                    contentSelected(this);
                });

                if (options.isShowTask) {
                    var tasks = GetDayTimeTask(cDateTostring(Context.weekDate[j]), i + ":00");
                    if (tasks) {
                        var content = $('<div class="body-day-content" style="min-height:' + options.cStyles.content.minheight + '; max-height:' + options.cStyles.content.maxheight + ';" ></div>');
                        for (var k = 0; k < tasks.length; k++) {
                            $('<div class="event-div-other" taskId="' + tasks[k].Id + '">' + tasks[k].nodeDesc + '</div>')
                        .dblclick(function (event) { createTask("", "", $(this).attr('taskId')); event.stopPropagation(); })
                        .appendTo(content);
                        }
                        tdup.append(content);
                    }
                }
                trup.append(tdup);
            }
            var trdown = $(dc('tr')).appendTo(tbody);
            for (var j = 0; j < Context.weekDate.length; j++) {
                var tddown = $('<td class="body-day" width="13.5%" curDate="' + cDateTostring(Context.weekDate[j]) + ' ' + i + ':30:00" ></td>')
               .dblclick(function () {
                   var cdate = $(this).attr("curDate");
                   createTask(RegExpDay(cdate), RegExpTime(cdate));
               })
                .click(function () {
                    contentSelected(this);
                });
                if (options.isShowTask) {
                    var tasks = GetDayTimeTask(cDateTostring(Context.weekDate[j]), i + ":30");
                    if (tasks) {
                        var content = $('<div class="body-day-content" style="min-height:' + options.cStyles.content.minheight + '; max-height:' + options.cStyles.content.maxheight + ';" ></div>');
                        for (var k = 0; k < tasks.length; k++) {
                            $('<div class="event-div-other" taskId="' + tasks[k].Id + '">' + tasks[k].nodeDesc + '</div>')
                        .dblclick(function (event) { createTask("", "", $(this).attr('taskId')); event.stopPropagation(); })
                        .appendTo(content);
                        }
                        tddown.append(content);
                    }
                }
                trdown.append(tddown);
            }
        }
        return body;
    }

    //渲染表头 日  缩小后  样式变形暂时舍弃
    function RenderDayWeek(cDate, options) {
        //        var d = new Date(cDate.y, cDate.m, cDate.d);
        //        var start = d.getDay();

        //        var sb = '<div style="padding:0px; margin:0px;">';
        //        sb += '<table style="width: 100%;height: 100%;" cellspacing="0" style="padding:0px; margin:0px;">'
        //        sb += '<thead><tr>';
        //        sb += '<th class="th-left head-weekday" width="25px"></th>';
        //        sb += '<th class="head-weekday" width="94.5%" >' + cDate.d + '日  ' + options.weeks[start] + '</th>';
        //        sb += '</tr></thead>';
        //        sb += '</table></div>';
        //        return $(sb);
    }

    //渲染主体  日
    function RenderDayBody(cDate) {
        //head
        var d = new Date(cDate.y, cDate.m, cDate.d);
        var start = d.getDay();

        var sbhead = "", sbday = "";
        sbhead += '<thead><tr>';
        sbhead += '<th class="th-left head-weekday" width="25px"></th>';
        sbhead += '<th class="head-weekday" width="94.5%" >' + cDate.d + '日  ' + options.weeks[start] + '</th>';
        sbhead += '</tr></thead>';

        sbday += '<tr>';
        sbday += '<td class="td-left body-allday" width="25px" >全天</td>';
        sbday += '<td class="body-allday" width="94.5%" curDate="' + cDateTostring(cDate) + '" ></td>';
        sbday += '</tr>';

        //body
        var body = $('<div  class="week-body"></div>');
        var tb = $('<table style="width: 100%;height: 100%;"  class="tb-header">');
        tb.append($(sbhead));
        var tbody = $('<tbody></tbody>');
        tbody.append($(sbday));
        tb.append(tbody).appendTo(body);
        for (var i = 0; i < options.hours.length; i++) {
            var trup = $(dc('tr')).appendTo(tbody);
            trup.append($('<td class="td-left body-day" rowspan="2" width="25px" >' + options.hours[i] + '</td>'));
            var tdup = $('<td class="body-day-up" width="94.5%" curDate="' + cDateTostring(cDate) + ' ' + i + ':00:00"></td>')
            .dblclick(function () {
                var cdate = $(this).attr("curDate");
                createTask(RegExpDay(cdate), RegExpTime(cdate));
            })
            .click(function () {
                contentSelected(this);
            });
            if (options.isShowTask) {
                var tasks = GetDayTimeTask(cDateTostring(cDate), i + ":00");
                if (tasks) {
                    var content = $('<div class="body-day-content" style="min-height:' + options.cStyles.content.minheight + '; max-height:' + options.cStyles.content.maxheight + ';" ></div>');
                    for (var k = 0; k < tasks.length; k++) {
                        $('<div class="event-div-other" taskId="' + tasks[k].Id + '">' + tasks[k].nodeDesc + '</div>')
                        .dblclick(function (event) { createTask("", "", $(this).attr('taskId')); event.stopPropagation(); })
                        .appendTo(content);
                    }
                    tdup.append(content);
                }
            }
            trup.append(tdup);

            var trdown = $(dc('tr')).appendTo(tbody);
            var tddown = $('<td class="body-day" width="94.5%" curDate="' + cDateTostring(cDate) + ' ' + i + ':30:00" ></td>')
             .dblclick(function () {
                 var cdate = $(this).attr("curDate");
                 createTask(RegExpDay(cdate), RegExpTime(cdate));
             })
            .click(function () {
                contentSelected(this);
            });
            if (options.isShowTask) {
                var tasks = GetDayTimeTask(cDateTostring(cDate), i + ":30");
                if (tasks) {
                    var content = $('<div class="body-day-content" style="min-height:' + options.cStyles.content.minheight + '; max-height:' + options.cStyles.content.maxheight + ';" ></div>');
                    for (var k = 0; k < tasks.length; k++) {
                        $('<div class="event-div-other" taskId="' + tasks[k].Id + '">' + tasks[k].nodeDesc + '</div>')
                        .dblclick(function (event) { createTask("", "", $(this).attr('taskId')); event.stopPropagation(); })
                        .appendTo(content);
                    }
                    tddown.append(content);
                }
            }
            trdown.append(tddown);
        }
        return body;
    }

    //获取表头信息
    function GetTitleText() {
        switch (options.renderType) {
            case 'day':
                return options.cDate.y + '年' + (options.cDate.m + 1) + '月' + options.cDate.d + '日';
                break;
            case 'week':
                var d = new Date(options.cDate.y, options.cDate.m, options.cDate.d);
                var tempDate = {};
                myCopy(options.cDate, tempDate);
                var start = d.getDay();
                for (var i = 0; i < start; i++) {
                    preDay(tempDate);
                }
                var sb = tempDate.y + '年' + (tempDate.m + 1) + '月' + tempDate.d + '日 - ';
                for (var i = 0; i < 6; i++) {
                    nextDay(tempDate);
                }
                sb += tempDate.y + '年' + (tempDate.m + 1) + '月' + tempDate.d + '日';
                return sb;
                break;
            default:
                return options.cDate.y + '年' + (options.cDate.m + 1) + '月';
                break;
        }
    }

    //双击天   月模式
    function dayDbClick(current, curDate) {
        if (options.ondayDbClick) {
            options.ondayDbClick(current, curDate);
        }
    }

    //双击半天   星期、日模式
    function dayHalfDbClick(current, curDate) {
        if (options.dayHalfDbClick) {
            options.dayHalfDbClick(current, curDate);
        }
    }

    //上一天
    function preDay(cDate) {
        if (cDate.d == 1) {
            preMonth(cDate);
            cDate.d = GetMaxDayInMonth(cDate.m, cDate.y);
        }
        else {
            cDate.d -= 1;
        }
    }

    //上个月
    function preMonth(cDate) {
        if (cDate.m == 0) {
            cDate.m = 11;
            cDate.y = cDate.y - 1;
        }
        else {
            cDate.m -= 1;
        }
    }

    //后一天
    function nextDay(cDate) {
        if (cDate.d == GetMaxDayInMonth(cDate.m, cDate.y)) {
            cDate.d = 1;
            nextMonth(cDate);
        }
        else {
            cDate.d += 1;
        }
    }

    //后个月
    function nextMonth(cDate) {
        if (cDate.m == 11) {
            cDate.m = 0;
            cDate.y = cDate.y + 1;
        }
        else {
            cDate.m += 1;
        }
    }

    //代办任务
    //代办任务有两种,每种分有两个状态  日历新建{新建、完成},审批新建{新建、提交}
    //单击【处理】分3种情况{新建、处理、跳转业务界面}
    function createTask(data, time, taskId) {
        //如果已经存在 则删除重新创建
        var old = $('.wait-task');
        if (old.length > 0) {
            old.remove();
        }

        var tbId = "", nodeDesc = "", nodeName = "", arrivalDate = data, arrivalTime = time, isAllday = false, opinion = "", workUser = "", workUserId = "";
        var name = "接收人", btnSure = "新建";
        var task = GetTaskById(taskId);
        if (task) {
            tbId = task.ID;
            nodeDesc = task.NODEDESC;
            nodeName = task.NODENAME;
            arrivalDate = RegExpDay(task.ARRIVALTIME);
            arrivalTime = RegExpTime(task.ARRIVALTIME);
            isAllday = false;
            opinion = task.OPINION;
            taskId = task.TASKID;
            workUserId = task.WORKUSERID;
            workUser = task.WORKUSER;
            name = "提交人";
            btnSure = "处理";
        }
        var selectOptions = Array("0:00", "0:30", "1:00", "1:30", "2:00", "2:30", "3:00", "3:30", "4:00", "4:30", "5:00", "5:30", "6:00", "6:30", "7:00", "7:30", "8:00", "8:30", "9:00", "9:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:00", "21:30", "22:00", "22:30", "23:00", "23:30");
        var sb = '<div id="waitTask" class="wait-task">';
        sb += '<table width="100%" cellspacing="0" cellpadding="1px">';
        sb += '<tr><td class="body-task body-task-left task-text-desc">任务名称</td><td colspan="3" class="body-task"><input type="text" name="NodeDesc" value="' + nodeDesc + '" class="task-text"/></td></tr>';
        sb += ' <tr><td class="body-task body-task-left task-text-desc">任务节点</td><td colspan="3" class="body-task"><input type="text" name="NodeName" value="' + nodeName + '" class="task-text"/></td></tr>';
        sb += ' <tr><td class="body-task body-task-left task-text-desc">' + name + '</td><td colspan="3" class="body-task"><input type="hidden" value="' + workUserId + '" name="WorkUserId" id="WorkUserId"/><div><input class="easyui-searchbox" value="' + workUser + '" type="text" id="WorkUser" name="WorkUser"></div></td></tr>';
        sb += '<tr><td class="body-task body-task-left task-text-desc">时&nbsp;&nbsp;间</td>';
        sb += '<td class="body-task" style="width:150px;" ><input id="dd" name="ArrivalDate" value="' + arrivalDate + '" class="easyui-datebox" required="required"></input></td>';
        sb += '<td class="body-task" style="width:150px;" ><select id="cc" class="easyui-combobox" name="ArrivalTime" style="width:200px;">';
        for (var i = 0; i < selectOptions.length; i++) {
            if (arrivalTime == selectOptions[i]) {
                sb += '<option selected="selected" value="' + selectOptions[i] + '">' + selectOptions[i] + '</option>';
            }
            else {
                sb += '<option value="' + selectOptions[i] + '">' + selectOptions[i] + '</option>';
            }
        }
        sb += '</select></td>';
        sb += ' <td class="body-task" style="text-align:left;"><span><input style="vertical-align:middle" name="isAllday" type="checkbox" /><label style="vertical-align:middle">全天</label></span> </td></tr>';
        sb += '<tr><td colspan="4" class="body-task body-task-left"><textarea class="task-text" style="min-height:200px;">' + opinion + '</textarea></td></tr>';
        sb += '<tr><td colspan="4" class="foot-task"> <input name="tbId" value="' + tbId + '" type="hidden"><input type="button" class="button gray" value="' + btnSure + '" />&nbsp;&nbsp;&nbsp;&nbsp;<input type="button" class="button gray" value="取消" />&nbsp;&nbsp;&nbsp;&nbsp;</td></tr>';
        sb += '</table></div>';
        var taskdom = $(sb);
        if (document.body) {
            taskdom.appendTo(document.body);
        }
        else {
            taskdom.appendTo(document);
        }

        //手动调用 easyui
        $('#dd').datebox({
            required: true
        });
        $('#cc').combobox();

        //按钮绑定事件
        var p = $('.wait-task');
        taskdom.find('input[type=button]').click(function () { taskClick(p, this); });

        //初始化接收人查询 
        $('#WorkUser').searchbox({
            width: 200,
            searcher: function (value, name) {
                GetWorkUserInfo(value, name);
            }
        });

        $('#waitTask').dialog({ title: "代办任务"
        });
    }

    //选择接收人
    function GetWorkUserInfo(value, name) {
        var f = new searchform();
        f.ischeck = true;
        var source = new datasource();
        source.c = "Acc.Business.Controllers.OfficeWorkerController";
        source.init();
        var model = {};
        model.modeldata = source.modeldata
        model.modeldata.isclear = false;
        model.name = source.c;
        f.model = model;
        f.menuname = model.modeldata.title;
        f.title = "选择";
        f.m = "outsearchload&outc=" + source.c;
        f.load = true;
        f.dbclose = false;
        f.onClose = function (rows) {
            if (rows && rows.length > 0) {
                $('.wait-task').find('input[name=WorkUserId]').val(rows[0].ID);
                $('#WorkUser').searchbox('setValue', rows[0].WORKNAME);
            }
        }
        f.show();
    }

    //任务单击
    function taskClick(parent, target) {
        switch (target.value) {
            case "处理":
                dealTaskValues(parent, false);
                break;
            case "新建":
                dealTaskValues(parent, true);
                break;
            case "取消":
                $('#waitTask').dialog('close');
                break;
        }
    }

    //处理任务 新建 处理(根据taskId=0 更新 or 页面跳转)
    function dealTaskValues(parent, isNew) {
        var data = "", taskId = 0;
        var inputs = parent.find(":input");

        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].value == undefined) {
                inputs[i].value = "";
            }
            if (isNew) {
                switch (inputs[i].name) {
                    case "NodeDesc":
                        data += "&NodeDesc=" + inputs[i].value;
                        break;
                    case "NodeName":
                        data += "&NodeName=" + inputs[i].value;
                        break;
                    case "ArrivalDate":
                        data += "&ArrivalDate=" + inputs[i].value;
                        break;
                    case "ArrivalTime":
                        data += "&ArrivalTime=" + inputs[i].value;
                        break;
                    case "isAllday":
                        data += "&isAllday=" + inputs[i].checked;
                        break;
                    case "WorkUser":
                        data += "&WorkUser=" + inputs[i].value;
                        break;
                    case "WorkUserId":
                        data += "&WorkUserId=" + inputs[i].value;
                        break;
                    //为了清除上次缓存  controller              
                    case "tbId":
                        data += "&tbId=null";
                        break;
                }
            }
            else {
                switch (inputs[i].name) {
                    case "tbId":
                        data += "&tbId=" + inputs[i].value;
                        taskId = inputs[i].value;
                        break;
                }
            }
        }
        //如果是处理 and taskId!=0 则跳转
        var isDeal = false;
        if (!isNew) {
            var task = GetTaskById(taskId);
            //是否已经处理过
            if (task.WORKSTATE) {
                $.messager.alert('异常', '此单据已经处理.', 'info');
                isDeal = true;
            }
            else {
                if (task.TASKID != 0) {
                    myOpentab(task.NODEDESC, task.URL);
                    $('#waitTask').dialog('close');
                    isDeal = true;
                }
            }
        }
        if (isDeal)
            return false;
        data += "&Opinion=" + parent.find("textarea").val();
        var result = webjs.post("Acc.Business.Controllers.ProcessStateController", "DealProcessState", data.substring(1, data.length));
        if (result.error) {
            $.messager.alert('异常', result.errormsg, 'info');
        }
        else {
            $.messager.alert('成功', result, 'info');
            $('#waitTask').dialog('close');
        }
    }

    //获取当月最大天数
    function GetMaxDayInMonth(month, year) {
        var daysInMonth = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if ((month == 1) && (year % 4 == 0) && ((year % 100 != 0) || (year % 400 == 0))) {
            return 29;
        } else {
            return daysInMonth[month];
        }
    }

    //将时间转换成对象
    function ConvertDateToObject(date) {
        if (date == null || date === undefined) {
            date = new Date();
        }
        return {
            d: date.getDate(),
            m: date.getMonth(),
            y: date.getFullYear()
        };
    }

    //将日期对象转换成字符串   yyyy-mm-dd
    function cDateTostring(cDate) {
        var m = cDate.m < 9 ? '0' + (cDate.m + 1) : (cDate.m + 1);
        var d = cDate.d < 10 ? '0' + cDate.d : cDate.d;
        return cDate.y + '-' + m + '-' + d;
    }

    //将sourceObj属性复制到targetObj,如果sourceObj不是数组或者对象类型，这将自身作为一个属性添加到targetObj
    function myCopy(sourceObj, targetObj) {
        var t = $.type(sourceObj).toLocaleLowerCase();
        if (t == 'array' || t == 'object') {
            for (var key in sourceObj) {
                targetObj[key] = sourceObj[key];
            }
        }
        else {
            targetObj[sourceObj.toString()] = sourceObj;
        }

        return targetObj;
    }

    //选中
    function contentSelected(tag) {
        $(".day-selected").removeClass("day-selected");
        $(tag).addClass("day-selected");
    }

    //创建元素
    function dc(tagName) {
        return document.createElement(tagName);
    }

    //获取代办任务   时间范围当前月  当前天  当前星期
    function GetWaitTasks(beginDate, endDate) {
        var result = webjs.post("Acc.Business.Controllers.ProcessStateController", "GetProcessStateDetail", "UserId=" + webjs.obj.ID + "&BeginDate=" + beginDate + "&EndDate=" + endDate);
        if (result.error) {
            $.messager.alert('异常', result.errormsg, 'info');
        }
        return result;
    }

    //获取代办任务  根据时间匹配(只是日期yyyy-mm-dd)
    function GetDayTask(cData) {
        if (Context.waitTask) {
            var tasks = new Array();
            var datas = Context.waitTask.data.rows;
            for (var i = 0; i < datas.length; i++) {
                if (cData == RegExpDay(datas[i].ARRIVALTIME)) {
                    tasks.push({ Id: datas[i].ID, nodeDesc: datas[i].NODEDESC });
                }
            }
            return tasks;
        }
    }

    //获取代办任务  根据时间匹配(日期yyyy-mm-dd，时间hh-mm)
    function GetDayTimeTask(date, time) {
        if (Context.waitTask) {
            var tasks = new Array();
            var datas = Context.waitTask.data.rows;
            for (var i = 0; i < datas.length; i++) {
                if (date == RegExpDay(datas[i].ARRIVALTIME) && time == RegExpTime(datas[i].ARRIVALTIME)) {
                    tasks.push({ Id: datas[i].ID, nodeDesc: datas[i].NODEDESC });
                }
            }
            return tasks;
        }
    }

    //根据Id获取代办任务
    function GetTaskById(taskId) {
        if (Context.waitTask) {
            var tasks = new Array();
            var datas = Context.waitTask.data.rows;
            for (var i = 0; i < datas.length; i++) {
                if (taskId == datas[i].ID) {
                    return datas[i];
                }
            }
            return null;
        }
        return null;
    }

    //只支持简单的日期匹配  将时间字符串转换成严格的yyyy-mm-dd格式
    function RegExpDay(dt) {
        var result;
        // yyyy-mm-dd
        var pattern = new RegExp("[0-9]{4}-[0-9]{1,2}-[0-9]{1,2}");
        result = pattern.exec(dt);
        if (result != null) {
            var arr = result.toString().split("-");
            arr[1] = (arr[1].length < 2) ? '0' + arr[1] : arr[1];
            arr[2] = (arr[2].length < 2) ? '0' + arr[2] : arr[2];
            return arr[0] + '-' + arr[1] + '-' + arr[2];
        }
        else {
            //mm/dd/yyyy 
            var pattern1 = new RegExp("[0-9]{1,2}/[0-9]{1,2}/[0-9]{4}");
            result = pattern1.exec(dt);
            if (result != null) {
                var arr = result.toString().split("/");
                arr[1] = (arr[1].length < 2) ? '0' + arr[1] : arr[1];
                arr[0] = (arr[0].length < 2) ? '0' + arr[0] : arr[0];
                return arr[2] + '-' + arr[1] + '-' + arr[1];
            }
            return "";
        }
    }

    //只支持简单的时间匹配 将hh:mm:ss转换成hh:30/hh:00
    function RegExpTime(dt) {
        var result;
        var pattern = new RegExp("[0-9]{1,2}:[0-9]{2}:[0-9]{2}");
        result = pattern.exec(dt);
        if (result != null) {
            var arr = result.toString().split(":");
            var mm = arr[1] >= 30 ? "30" : "00";
            return arr[0] + ":" + mm;
        }
        return "";
    }
})(jQuery);