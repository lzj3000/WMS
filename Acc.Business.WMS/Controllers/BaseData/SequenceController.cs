using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.WMS.Controllers
{
    public class InSequenceController : BusinessController
    {
        /// <summary>
        /// 描述：序列码控制器
        /// 作者：路聪
        /// 创建日期:2013-2-20
        /// </summary>
        public InSequenceController() : base(new InSequence()) {
            //EntityList<Sequence> list = new EntityList<Sequence>(this.model.GetDataAction());
            //list.CreateTable(new Sequence());
        }

        //显示在菜单
        protected override string OnControllerName()
        {
            return "序列码管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/StockInOrder/Sequence.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "路聪";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "序列码管理";
        }

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            if (data.name.EndsWith("Sequence"))
            {
                switch (item.field.ToLower())
                {
                    case "submiteddate":
                    case "submitedby":
                    case "createdby":
                    case "creationdate":
                    case "modifiedby":
                    case "modifieddate":
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                    case "isdisable":
                    case "isdelete":
                    case "remark":
                    case "id":
                    case "code":
                        item.visible = false;
                        break;
                    default:
                        item.visible = true;
                        break;
                }
            }
        }

        #endregion 

        /// <summary>
        /// 重写选择按钮的方法
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override Acc.Contract.Data.ControllerData.ReadTable OutSearchData(Acc.Contract.Data.ControllerData.loadItem item)
        {
            return base.OutSearchData(item);
        }
    }
}
