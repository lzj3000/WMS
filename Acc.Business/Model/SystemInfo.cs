using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;

namespace Acc.Business.Model
{
    /// <summary>
    /// 系统信息
    /// </summary>
    [EntityClassAttribut("Acc_Bus_SystemInfo", "系统信息")]
    public class SystemInfo : BasicInfo
    {
        /// <summary>
        /// 系统唯一识别码
        /// </summary>
        [EntityField(200)]
        public string GUID { get; set; }

        /// <summary>
        /// 注册地区
        /// </summary>
        [EntityControl("注册地区", false, true, 2)]
        [EntityForeignKey(typeof(Region), "ID", "RegionName")]
        [EntityField(200)]
        public int RegionID { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        [EntityControl("系统名称", false, true, 3)]
        [EntityField(200)]
        public string SystemName { get; set; }
        /// <summary>
        /// 使用机构名称
        /// </summary>
        [EntityControl("用户名称", false, true, 4)]
        [EntityField(200)]
        public string UseAgencies { get; set; }
        /// <summary>
        /// 国家通用组织机构代码
        /// </summary>
        [EntityControl("用户代码", false, true, 5)]
        [EntityField(200)]
        public string UseAgenciesCode { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [EntityControl("联系人", false, true, 6)]
        [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName")]
        [EntityField(200)]
        public string OfficeWorkerID { get; set; }
        /// <summary>
        /// 用户地址
        /// </summary>
        [EntityControl("用户地址", false, true, 7)]
        [EntityField(200)]
        public string UseAddress { get; set; }
        /// <summary>
        /// 用户电话
        /// </summary>
        [EntityControl("用户电话", false, true, 8)]
        [EntityField(200)]
        public string UseTel { get; set; }
        /// <summary>
        /// 用户网址
        /// </summary>
        [EntityControl("用户网址", false, true, 9)]
        [EntityField(200)]
        public string UseWebAddress { get; set; }

        /// <summary>
        /// Logo图片
        /// </summary>
        [EntityControl("Logo图片", false, true, 11)]
        [EntityField(200)]
        public string ImgSrc { get; set; }

        /// <summary>
        /// Logo图片宽度
        /// </summary>
        [EntityControl("Logo图片宽度", false, true, 12)]
        [EntityField(20)]
        public double ImgWidth { get; set; }

        /// <summary>
        /// Logo图片高度
        /// </summary>
        [EntityControl("Logo图片高度", false, true, 13)]
        [EntityField(20)]
        public double ImgHeight { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        [EntityControl("是否默认", false, true, 10)]
        [EntityField(2)]
        public bool IsSelected { get; set; }

        private HierarchicalEntityView<SystemInfo, SystemInfoDetials> _Detials;
        /// <summary>
        /// 配置信息集合
        /// </summary>
        public HierarchicalEntityView<SystemInfo, SystemInfoDetials> Detials
        {
            get
            {
                if (_Detials == null)
                {
                    _Detials = new HierarchicalEntityView<SystemInfo, SystemInfoDetials>(this);
                }
                return _Detials;
            }
        }
    }
}
