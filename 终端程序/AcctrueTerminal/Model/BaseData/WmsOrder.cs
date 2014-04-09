using System;

using System.Collections.Generic;
using System.Text;

namespace AcctrueTerminal.Model.BaseData
{
   public class WmsOrder:BasicInfo
   {       
       /// <summary>
       /// 源单控制器
       /// </summary>       
       public string SOURCECONTROLLER { get; set; }
       /// <summary>
       /// 源单编码
       /// /// </summary>       
       public string SOURCECODE { get; set; }
       /// <summary>
       /// 源单ID
       /// </summary>      
       public int SOURCEID { get; set; } 
        /// <summary>
        /// 客户编号
        /// </summary>        
        public string CLIENTNO { get; set; }

        /// <summary>
        /// 类型
        /// </summary>       
        public int STOCKTYPE { get; set; }

        /// <summary>
        /// 状态
        /// </summary>       
        public int STATE { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>        
        public string TOWHNO { get; set; }
        /// <summary>
        /// 源表
        /// </summary>       
        public string SOURCETABLE { get; set; }
    }
}
