using System;
using System.Collections.Generic;
using System.Linq;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Acc.Contract.Center;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Acc.Business.WMS.Controllers;
using System.Data;
using Acc.Business.WMS.XHY.Model;
using Way.EAP.DataAccess.Regulation;
using Acc.Contract.Data;
using Acc.Business.Model;
namespace Acc.Business.WMS.XHY.Controllers
{
    public class ReCheckOrderController : XHY_CheckOrderController
    {
        public ReCheckOrderController() : base(new ProCheckMaterials()) { }
        #region 页面设置
       
        //显示在菜单
        protected override string OnControllerName()
        {
            return "复检单";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/ReCheck.htm";
        }
       
        //说明
        protected override string OnGetControllerDescription()
        {
            return "复检单管理";
        }
        #endregion

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
           
            if (data.name.EndsWith("ProCheckMaterials"))
            {
                data.title = "复检单";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "复检单号";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "sourcecode":
                        item.visible = false;
                        break;
                   
                }
            }
        }
        #endregion

        #region 重写方法
        public override string CheckTypeName()
        {
            return "3";
        }

        public override string CreateBy()
        {
            return "1";
        }

        public override string SetRemark()
        {
            return "来源库存管理下推";
        }

        

        public override void SetInfoCheckStatus(ProCheckMaterials pc, string status)
        {
            
        }

        public override void HG()
        {
            ProCheckMaterials pc = base.getinfo(this.ActionItem["ID"].ToString()) as ProCheckMaterials;
            if(pc.IsOK==false)
            {
           StockInfoMaterials sim = new StockInfoMaterials();
            this.model.GetDataAction().Execute("update " + pc.ToString() + " set IsOK = 1,issubmited= 1,submitedby=" + this.user.ID + ",submiteddate= '" + DateTime.Now + "' where id='" +pc.ID + "' ");
             this.model.GetDataAction().Execute("update "+sim.ToString()+" set lastintime='"+DateTime.Now+"',remark='最后操作：复检单' where id='"+pc.SourceID+"'");
               
            }
            else
            {
                throw new Exception("异常：质检单已质检合格不能重复质检！");
            }
        }

        public override void BHG()
        {
            ProCheckMaterials pc = base.getinfo(this.ActionItem["ID"].ToString()) as ProCheckMaterials;
            if (pc.IsSubmited == true)
            {
                throw new Exception("异常：该批次已质检完成不能设置为不合格");
            }
            this.model.GetDataAction().Execute("update " + pc.ToString() + " set IsOK = 0,issubmited=1,submitedby=" + this.user.ID + ",submiteddate= '" + DateTime.Now + "' where id='" + pc.ID + "' ");
        }

        #endregion


    }
}
