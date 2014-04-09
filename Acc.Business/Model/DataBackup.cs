using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.Model
{
    /// <summary>
    /// 描述：数据备份
    /// 作者：胡文杰
    /// 创建日期:2013-09-02
    /// </summary>
    [EntityClassAttribut("Acc_Bus_DataBackup", "数据备份", IsOnAppendProperty = true)]
    public class DataBackup : BasicInfo
    {
        ///// <summary>
        ///// 备份目录
        ///// </summary>
        //[EntityControl("备份目录", false, true, 2)]
        //[EntityField(1000, IsNotNullable = true)]
        //public string BackDirectory { get; set; }

        /// <summary>
        /// 文件名 
        /// </summary>
        [EntityControl("文件名", false, true, 3)]
        [EntityField(200, IsNotNullable = true)]
        public string FileName { get; set; }


    }
}
