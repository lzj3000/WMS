//非负整数检查
function CheckInt(tempTxt,tempValue)
{
    var r = /^\+?[1-9][0-9]*$/;
    if(!r.test(tempValue) && tempValue.length==0)
    {   
        alert("必须是非负整数!");
        tempTxt.value="1";
        tempTxt.focus();   
    }
}

//获取地址栏参数
function getUrlParam(name){
nk="";
var reg=new RegExp("(^|&)"+name+"=([^&]*)(&|$)");
var r=window.location.search.substr(1).match(reg);
if (r!=null) return unescape(r[2]);return nk;
}

//清空文本框
function clearText()
{
    var inputs=document.getElementsByTagName("input");
    for(var i=0;i<inputs.length;i++)
        {
            if(inputs[i].getAttribute("type")=="text" || inputs[i].getAttribute("type")=="password")
                inputs[i].value="";
        }
    var textareas=document.getElementsByTagName("textarea");
    for(var i=0;i<textareas.length;i++)
        {
            if(textareas[i])
                textareas[i].value="";
        }
}

//弹出模式窗口
var isIe=(document.all)?true:false;
function setBack()
{
    var bWidth=parseInt(document.documentElement.scrollWidth);
    var bHeight=parseInt(document.documentElement.scrollHeight);
    var styleStr="top:0px;left:0px;position:absolute;background:#666;width:"+bWidth+"px;height:"+bHeight+"px;z-index:9;";
    styleStr+=(isIe)?"filter:alpha(opacity=0);":"opacity:0;";
    var back=document.createElement("div");
    back.id="back";
    back.style.cssText=styleStr;
    document.body.appendChild(back);
    showBackground(back,50);
}

function OpenDialog(msgTitle,msgHeight,msgWidth,paramUrl)//传递参数
{
    setBack();
    var msgW=document.createElement("div");
    msgW.id="msgW";
    var msgTop="<label style='position:absolute;left:0px;width:100%;height:28px;text-align:left;font-weight:600;'><p style='padding-left:5px;left:0px;position:absolute;'>"+msgTitle+"</p></label><input type='button' onclick='closeMsgW();' style='border:solid #fff 1px;cursor:pointer;position:absolute;right:3px;top:3px;' value='关闭' />";
    msgW.innerHTML=msgTop;
    var v_top=(document.body.clientHeight-msgHeight)/2+document.documentElement.scrollTop;
    v_top=(v_top>0)?v_top:document.documentElement.scrollTop+10;
    styleStr="top:"+v_top+"px;left:"+(document.body.clientWidth/2-msgWidth/2)+"px;"+"height:"+msgHeight+"px;width:"+msgWidth+"px;position:absolute;z-index:10;border:#666 1px solid;background:#d7d9e3;overflow:hidden;";
    msgW.style.cssText=styleStr;
    document.body.appendChild(msgW);
    var ifrStyle="position:absolute;top:28px;left:-2px;width:101%;background:#ccffcc;overflow:auto;";
    var ifrWindow=document.createElement("iframe");
    ifrWindow.id="ifrShow";
    ifrWindow.style.cssText=ifrStyle;
    var ifrHeight=msgHeight-26;
    ifrWindow.style.height=ifrHeight+"px";
    msgW.appendChild(ifrWindow);
    document.getElementById("ifrShow").src=paramUrl;
}

function showBackground(obj,endInt)//背景渐变
{
    if(isIe)
    {
        obj.filters.alpha.opacity+=5;
        if(obj.filters.alpha.opacity<endInt)
        {
            setTimeout(function(){showBackground(obj,endInt)},5);
        }
    }
    else
    {
        var al=parseFloat(obj.style.opacity);
        al+=0.05;
        obj.style.opacity=al;
        if(al<(endInt/100))
        {
            setTimeout(function(){showBackground(obj,endInt)},5);
        }
    }
}

function closeMsgW()//父窗体关闭弹出窗体
{
    if(document.getElementById('back')!=null)
    {
        document.getElementById('back').parentNode.removeChild(document.getElementById('back'));
    }
    if(document.getElementById('msgW')!=null)
    {
        document.getElementById('msgW').parentNode.removeChild(document.getElementById('msgW'));
    }
}

function mousePosition(ev)//获取鼠标位置
{
    if(ev.pageX || ev.pageY)
    {
        return {x:ev.pageX, y:ev.pageY};
    }
    return {
        x:ev.clientX + document.body.scrollLeft - document.body.clientLeft,y:ev.clientY + document.body.scrollTop - document.body.clientTop
    };
}

function closeParentMsgW()//弹出窗体关闭自身
{
    if(parent.document.getElementById('back')!=null)
    {
        parent.document.getElementById('back').parentNode.removeChild(parent.document.getElementById('back'));
    }
    if(parent.document.getElementById('msgW')!=null)
    {
        parent.document.getElementById('msgW').parentNode.removeChild(parent.document.getElementById('msgW'));
    }
}

function GetOne(tbID,tbValue)//弹出窗体选择参数返回父窗体
{
    parent.document.getElementById(tbID).value=tbValue;
    closeParentMsgW(); 
}

//功能码管理页面选择功能码函数
var inputsCheckbox=document.getElementsByTagName("input");
function GetCheckedNum()
{
    var checkboxNum=0,checkedNum=0;
    for(var i=0;i<inputsCheckbox.length;i++)
    {
        if(inputsCheckbox[i].getAttribute("type")=="checkbox")
        {   
            checkboxNum+=1;
            if(inputsCheckbox[i].checked)
                checkedNum+=1;
        } 
    }
    return checkedNum+"$"+checkboxNum;
}

function selectCheck(tempObject)
{
    var temp=GetCheckedNum().split('$');
    if((parseInt(temp[0])+parseInt(1))==parseInt(temp[1]))
    {
        if(tempObject.checked)
        {
            for(var i=0;i<inputsCheckbox.length;i++)
            {
                if(inputsCheckbox[i].getAttribute("type")=="checkbox")
                    inputsCheckbox[i].checked=true;
            }
        }
        else
        {
            document.getElementById("ckbSelectAll").checked=false;
        }
    }
}

function selectAll()
{
    if(document.getElementById("ckbSelectAll").checked==false)
    {
        for(var i=0;i<inputsCheckbox.length;i++)
        {
            if(inputsCheckbox[i].getAttribute("type")=="checkbox")
                inputsCheckbox[i].checked=false;
        }
    }
    else
    {
        for(var j=0;j<inputsCheckbox.length;j++)
        {
            if(inputsCheckbox[j].getAttribute("type")=="checkbox")
                inputsCheckbox[j].checked=true;
        }
    }
}


//弹出模式窗口
function GetDialog(paramUrl,paramTitle,paramWidth,paramHeight)
{
    ymPrompt.win({message:paramUrl,width:paramWidth,height:paramHeight,title:paramTitle,maxBtn:true,minBtn:true,iframe:true});
}

//页面跳转
function changePage(url)
{
    document.location=url; 
}