using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Way.EAP;
using Acc.Contract.MVC;
using Acc.Contract.Data;
using Way.EAP.DataAccess.Data;

namespace Acc.Business.WMS.Controllers.BaseData
{
    public class BaseWMSController 
    {
        /// <summary>
        /// 通过名称空间获取外系统表数据
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public IEntityList GetList(Type tt, IDataAction taraction)
        {
            //获取本系统数据结构
            IEntityList targetlist = Common.GetEntityList(tt);//目标表(本系统)
            targetlist.DataAction = taraction;
            return targetlist;
        }
        public Type GetType(string classname)
        {
            Type tt = Common.GetType(classname);
            return tt;
        }

    }
}
