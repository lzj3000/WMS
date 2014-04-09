using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Contract.Data;
using Acc.Business.Model;

namespace Acc.Business.Controllers
{

    public class MobileSetController : BusinessController
    {
       public MobileSetController() : base(new MobileSetProject()) { }

       /// <summary>
       /// 显示在菜单
       /// </summary>
       /// <returns></returns>
       protected override string OnControllerName()
       {
           return "终端配置管理";
       }

       /// <summary>
       /// 菜单中URL路径
       /// </summary>
       /// <returns></returns>
       protected override string OnGetPath()
       {
           return "Views/manager/MobileSet.htm";
       }

       /// <summary>
       /// 开发人
       /// </summary>
       /// <returns></returns>
       protected override string OnGetAuthor()
       {
           return "张朝阳";
       }


       protected override void OnInitViewChildItem(ModelData data, ItemData item)
       {
           base.OnInitViewChildItem(data, item);
           if (data.name.EndsWith("MobileSetProject"))
           {
                switch (item.field.ToLower())
                {
                   case "tranurl":
                   case "projectname":
                   case "isoffline":                  
                   item.visible=true;                  
                   break;
                   default:
                   item.visible=false;
                   break;
                }
           }
           if (data.name.EndsWith("MobileSetModel"))
           {
               switch (item.field.ToLower())
               {
                   case "isvisible":
                   case "modelname":
                   case "controllername":                   
                       item.visible = true;                      
                       break;
                   default:
                       item.visible = false;
                       break;
               }
           }
           if (data.name.EndsWith("MobileSetModelList"))
           {
               switch (item.field.ToLower())
               {
                   case "isvisible":
                   case "modellistname":                   
                       item.visible = true;                       
                       break;
                   default:
                       item.visible = false;
                       break;
               }
           }
       }
       /// <summary>
       /// 说明
       /// </summary>
       /// <returns></returns>
       protected override string OnGetControllerDescription()
       {
           return "终端配置管理";
       }

       protected override void OnAdding(Contract.MVC.ControllerBase.SaveEvent item)
       {
           base.OnAdding(item);          
       }

       protected override void OnEditing(Contract.MVC.ControllerBase.SaveEvent item)
       {
           base.OnEditing(item);          
       }
    }
}
