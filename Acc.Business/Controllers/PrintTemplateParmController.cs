using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Contract.Data;
using Acc.Contract.Data.ControllerData;
using Acc.Business.Model;
using System.Configuration;
using Way.EAP.DataAccess.Data;

namespace Acc.Business.Controllers
{
    public class PrintTemplateParmController : BusinessController
    {
        /// <summary>
        /// 描述：打印参数维护
        /// 作者：路聪
        /// 创建日期:2013-08-01
        /// </summary>
        public PrintTemplateParmController() : base(new PrintTemplateParm()) { }

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
            return "打印参数维护";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/manager/PrintTemplateParm.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "路聪08-01";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "打印参数维护";
        }

        #region 初始化数据方法

        #endregion

        protected override void OnAdding(Contract.MVC.ControllerBase.SaveEvent item)
        {
            PrintTemplateParm ptp=item.Item as PrintTemplateParm;
            ptp.SID = ptp.ID;
            ptp.CreateDate = ptp.Creationdate;
            base.OnAdding(item);
        }


    }
}
