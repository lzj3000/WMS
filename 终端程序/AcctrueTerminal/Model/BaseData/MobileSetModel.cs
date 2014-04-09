using System;
using System.Collections.Generic;
using System.Text;

namespace AcctrueTerminal.Model.BaseData
{
    /// <summary>
    /// 描述：终端功能配置
    /// 作者：张朝阳
    /// 创建日期:2013-08-02
    /// </summary>
    
    public class MobileSetModel : BasicInfo
    {
        /// <summary>
        /// 项目ID
        /// </summary>       
        public int PROJECTID { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>       
        public bool ISVISIBLE { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>      
        public string MODELNAME { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>       
        public string CONTROLLERNAME { get; set; }

        /// <summary>
        /// 修改名称
        /// </summary>        
        public string EDITMODELNAME { get; set; }

        public string ACC_WMS_MOBILESETPROJECT_CODE_PROJECTID { get; set; }
       
    }
}
