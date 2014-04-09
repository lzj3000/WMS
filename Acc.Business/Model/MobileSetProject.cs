using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.Model
{
    /// <summary>
    /// 描述：终端项目配置
    /// 作者：张朝阳
    /// 创建日期:2013-08-02
    /// </summary>
    [EntityClassAttribut("Acc_WMS_MobileSetProject", "终端功能配置", IsOnAppendProperty = true)]
    public class MobileSetProject : BasicInfo
    {
        /// <summary>
        /// 数据传输地址
        /// </summary>
        [EntityControl("数据传输地址", false, true, 1)]
        [EntityField(50)]       
        public string TranUrl { get; set; }


        /// <summary>
        /// 项目名称
        /// </summary>
        [EntityControl("项目名称", false, true, 2)]
        [EntityField(50)]
        public string ProjectName { get; set; }       

        /// <summary>
        /// 是否离线操作
        /// </summary>
        [EntityControl("是否离线操作", false, true, 3)]
        [EntityField(50)]       
        public bool IsOffline { get; set; }

        private HierarchicalEntityView<MobileSetProject, MobileSetModel> _MobileSet;

        public HierarchicalEntityView<MobileSetProject, MobileSetModel> MobileSet
        {
            get
            {
                if (_MobileSet == null)
                {
                    _MobileSet = new HierarchicalEntityView<MobileSetProject, MobileSetModel>(this);
                }
                return _MobileSet;
            }
        }
    }
}
