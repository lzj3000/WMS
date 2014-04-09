using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.Model
{
    /// <summary>
    /// 描述：终端功能配置子表
    /// 作者：张朝阳
    /// 创建日期:2013-08-02
    /// </summary>
    [EntityClassAttribut("Acc_WMS_MobileSetModelList", "终端功能配置子表", IsOnAppendProperty = true)]
    public class MobileSetModelList : BasicInfo
    {
        /// <summary>
        /// 模块ID
        /// </summary>
        [EntityControl("模块ID", false, true, 1)]
        [EntityField(50)]
        [EntityForeignKey(typeof(MobileSetModel), "ID", "code")]
        public int ModelID { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        [EntityControl("是否可见", false, true, 1)]
        [EntityField(50)]       
        public bool IsVisible { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        [EntityControl("功能名称", false, true, 2)]
        [EntityField(50)]
        public string ModelListName { get; set; }     

    }
}
