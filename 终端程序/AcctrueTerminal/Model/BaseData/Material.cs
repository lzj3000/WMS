using System;

using System.Collections.Generic;
using System.Text;

namespace AcctrueTerminal.Model.BaseData
{
    /// <summary>
    /// 描述：基础物料(产品、原料、备件、商品)表
    /// 作者：张朝阳
    /// 创建日期:2013-08-02
    /// </summary>
   public class Material:BasicInfo
    {
        /// <summary>
        /// 产品名称
        /// </summary>       
        public string FNAME { get; set; }
        /// <summary>
        /// 产品净重量
        /// </summary>      
        public double NETWEIGHT { get; set; }
        /// <summary>
        /// 批次管理
        /// </summary>       
        public bool BATCH { get; set; }
        /// <summary>
        /// 最低库存量
        /// </summary>        
        public double LOWESTSTOCK { get; set; }
        /// <summary>
        /// 换算单位
        /// </summary>       
        public string CONVERSION { get; set; }
        /// <summary>
        /// 状态
        /// </summary>       
        public string STATUS { get; set; }
        /// <summary>
        /// 默认存放数量
        /// </summary>       
        public double STOREAMOUNT { get; set; }
        /// <summary>
        /// 保质期
        /// </summary>
        public double SHELFLIFE { get; set; }
        /// <summary>
        /// 保质期单位
        /// </summary>        
        public string SHELFUNIT { get; set; }
        /// <summary>
        /// 预警提前时限
        /// </summary>       
        public double ALARMEARLIERAMOUNT { get; set; }     

        ///<summary>
        /// 是否允许负数库存
        /// </summary>      
        public bool NUMSTATE { get; set; }
        /// <summary>
        /// 基本计量单位
        /// </summary>        
        public int FUNITID { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>        
        public string FMODEL { get; set; }
        /// <summary>
        /// 商品类型
        /// </summary>       
        public int CommodityType { get; set; }
    }
}
