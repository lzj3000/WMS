var spdf = (function () {
    function cconfig(pdf) {
        var div = $("<div><div id='printt1'/></div>").appendTo($("#temp"));
        var p = new singlepage();
        p.c = "Acc.Contract.MVC.PrintController";
        p.applyTo = "printt1";
        p.isprint = false;
        p.closesearch = true;
        p.showfooter = false;
        p.onCommandClick = function (command, form, item) {
            if (item && item.FILEPATH) {
                item.FILEPATH = "";
            }
        }
        p.onFormater = function (obj) {
            if (obj.field == 'FILEPATH') {
                return "<a href='" + obj.value + "'>下载</a>";
            }
            else
                return obj.value;
        }
        p.init();
        p.datasource.load("c=" + pdf.c);
        $(div).window({
            width: 800,
            height: 300,
            modal: true,
            minimizable: false,
            title: pdf.title + "打印模板管理",
            onClose: function () {
                $(this).panel('destroy');
            }
        });
    }
    function cshow(p) {
        var id = "pdf_" + webjs.newid();
        p.file = p.file + "?" + id;
        var con = '<!--[if IE]>'
         + '<object classid="clsid:CA8A9780-280D-11CF-A24D-444553540000" width="100%" height="100%" border="0">'
         + '<param name="_Version" value="65539">'
         + '<param name="_ExtentX" value="20108">'
         + '<param name="_ExtentY" value="10866">'
         + '<param name="_StockProps" value="0">'
         + '<param name="SRC" value="' + p.file + '">'
         + '</object>'
         + '<![endif]-->'
         + '<!--[if !IE]> <!--> '
         + '<object data="' + p.file + '" type="application/pdf" width="100%" height="100%">'
         + 'alt : <a href="http://get.adobe.com/cn/reader">Adobe Reader.pdf</a>'
         + '</object><!--<![endif]-->'
        var ttt = "<div id='" + id + ">" + con + "</div>";
        return con;

    }
    return {
        file: "",
        title: "",
        c: "",
        config: function () {
            cconfig(this);
        },
        show: function () {
            var p = cshow(this);
            var div = $("<div/>").appendTo($("#temp"));
            $(div).window({
                width: 800,
                height: 600,
                modal: true,
                content: p,
                cache:false,
                minimizable: false,
                title: "打印" + this.title,
                onClose: function () {
                    $(this).panel('destroy');
                }
            });
            $(div).window('open');
        }
    }
})