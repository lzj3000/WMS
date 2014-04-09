using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;

namespace Acc.Business.WMS.Controllers
{
    public class PackUnitRecordController : BusinessController
    {
        /// <summary>
        /// 描述：包装出入库管理控制器
        /// 作者：路聪
        /// 创建日期:2012-12-25
        /// </summary>
        public PackUnitRecordController() : base(new PackUnitRecord()) { }

        //是否启用审核
        public override bool IsReviewedState
        {
            get
            {
                return false;
            }
        }
        //是否启用提交
        public override bool IsSubmit
        {
            get
            {
                return false;
            }
        }
        //是否启用回收站
        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }

        //显示在菜单
        protected override string OnControllerName()
        {
            return "包装出入库管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/Materials/PackUnitRecord.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "路聪";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "包装出入库管理";
        }

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            if (data.name.EndsWith("PackUnitRecord"))
            {
                switch (item.field.ToLower())
                {
                    case "submiteddate":
                    case "submitedby":
                    case "modifiedby":
                    case "modifieddate":
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                    case "isdisable":
                    case "isdelete":
                    case "id":
                    case "code":
                        item.visible = false;
                        item.isedit = false;
                        break;
                    default:
                        item.visible = true;
                        break;
                }
            }
        }

        #endregion
    }
}
