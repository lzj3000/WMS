using System;

using System.Collections.Generic;
using System.Text;

namespace AcctrueTerminal.Model.BaseData
{
    /// <summary>
    /// 描述：出入库待操作明细表
    /// 作者：张朝阳
    /// 创建日期:2013-08-02
    /// </summary>
   public class OrderStayMaterials
    {
        /// <summary>
        /// 源单数据模型
        /// </summary>        
        public string SOURCEID { get; set; }
        /// <summary>
        /// 源单控制器
        /// </summary>       
        public string SOURCECONTROLLER { get; set; }
        /// <summary>
        /// 源单编码
        /// </summary>       
        public string SOURCECODE { get; set; }
        /// <summary>
        /// 源行ID
        /// </summary>       
        public int SOURCEROWID { get; set; }
        /// <summary>
        /// 源名称
        /// </summary>        
        public string SOURCENAME { get; set; }
        /// <summary>
        /// 源表
        /// </summary>        
        public string SOURCETABLE { get; set; }

        /// <summary>
        /// 通知单号
        /// </summary>
        //[EntityControl("通知单号", false, true, 1)]
        //[EntityForeignKey(typeof(StockInNotice), "ID", "CODE")]
        //[EntityField(30)]
        public int PARENTID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>       
        public string MATERIALCODE { get; set; }
        /// <summary>
        /// 编码
        /// </summary>       
        public string MCODE { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>        
        public string FMODEL { get; set; }
        /// <summary>
        /// 主单
        /// </summary>
        //[EntityControl("主单", false, false, 6)]
        //[EntityField(255)]
        //[EntityForeignKey(typeof(StockInOrder), "ID", "Code")]
        public int ORDERID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>        
        public double NUM { get; set; }
        /// <summary>
        /// 完成数量
        /// </summary>       
        public double FINISHNUM { get; set; }
        /// <summary>
        /// 待操作数量
        /// </summary>        
        public double STAYNUM { get; set; }
        /// <summary>
        /// 备注
        /// </summary>        
        public string ROWDESC { get; set; }
       
    }
}
