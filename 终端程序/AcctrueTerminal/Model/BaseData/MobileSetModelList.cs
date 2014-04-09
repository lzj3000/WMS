using System;
using System.Collections.Generic;

using System.Text;



namespace AcctrueTerminal.Model.BaseData
{
    /// <summary>
    /// 描述：终端功能配置明细表
    /// 作者：张朝阳
    /// 创建日期:2013-08-02
    /// </summary>
   
    public class MobileSetModelList : BasicInfo   
    {
        /// <summary>
        /// 模块名称
        /// </summary>       
        public int MODELID { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>        
        public bool ISVISIBLE { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>       
        public string MODELLISTNAME { get; set; }

        public string ACC_WMS_MOBILESETMODEL_CODE_MODELID { get; set; }     
        
    }
}
