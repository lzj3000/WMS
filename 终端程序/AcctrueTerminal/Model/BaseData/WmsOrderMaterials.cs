using System;

using System.Collections.Generic;
using System.Text;

namespace AcctrueTerminal.Model.BaseData
{
    public class WmsOrderMaterials 
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
       
        public int PARENTID { get; set; }



        /// <summary>
        /// 产品名称
        /// </summary>       
        public string MATERIALCODE { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MCODE { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>      
        public string FMODEL { get; set; }
        /// <summary>
        /// 数量
        /// </summary>      
        public double NUM { get; set; }

        /// <summary>
        /// 备注
        /// </summary>       
        public string ROWDESC { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>       
        public string BATCHNO { get; set; }
        /// <summary>
        /// 货位
        /// </summary>       
        public string DEPOTWBS { get; set; }

        /// <summary>
        /// 托盘
        /// </summary>        
        public string PORTNAME { get; set; }


        /// <summary>
        /// 价格
        /// </summary>       
        public double PRICE { get; set; }

        /// <summary>
        /// 亚批次
        /// </summary>       
        public string SENBATCHNO { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string ACC_BUS_BUSINESSCOMMODITY_FNAME_MATERIALCODE { get; set; }     
    }
}
