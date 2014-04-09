using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.Model
{
    /// <summary>
    /// 描述：数据初始化
    /// 作者：胡文杰
    /// 创建日期:2013-09-02
    /// </summary>
    [EntityClassAttribut("Acc_Bus_DataInitialize", "数据初始化", IsOnAppendProperty = true)]
    public class DataInitialize : BasicInfo
    {
    //    /// <summary>
    //    /// 单据名称
    //    /// </summary>
    //    [EntityControl("单据名称", true, true, 2)]
    //    [EntityField(100)]
    //    public string FBILLNAME { get; set; }
        
        /// <summary>
        /// 表名称
        /// </summary>
        [EntityControl("表名称", true, true, 3)]
        [EntityField(100, IsNotNullable = true)]
        public string FTABLENAME { get; set; }
        

    }
}
