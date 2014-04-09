using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.Model
{
    /// <summary>
    /// 描述：终端功能配置
    /// 作者：张朝阳
    /// 创建日期:2013-08-02
    /// </summary>
    [EntityClassAttribut("Acc_WMS_MobileSetModel", "终端功能配置", IsOnAppendProperty = true)]
    public class MobileSetModel : BasicInfo
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        [EntityControl("项目ID", false, true, 1)]
        [EntityField(50)]
        [EntityForeignKey(typeof(MobileSetProject), "ID", "code")]
        public int ProjectID { get; set; }

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
        public string ModelName { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        [EntityControl("控制器名称", false, true, 3)]
        [EntityField(200)]
        public string ConTrollerName { get; set; }

        /// <summary>
        /// 修改名称
        /// </summary>
        [EntityControl("修改名称", true, false, 4)]
        [EntityField(50)]
        public string EditModelName { get; set; }


        public HierarchicalEntityView<MobileSetModel,MobileSetModelList> _MobileSetList;
        public HierarchicalEntityView<MobileSetModel, MobileSetModelList> MobileSetList
        {
            get
            {
                if (_MobileSetList == null)
                {
                    _MobileSetList = new HierarchicalEntityView<MobileSetModel, MobileSetModelList>(this);
                }
                return _MobileSetList;
            }
        }       

    }
}
