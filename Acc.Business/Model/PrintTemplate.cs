using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.MVC;
using System.Configuration;
using Way.EAP.DataAccess.Data;

namespace Acc.Business.Model
{
    /// <summary>
    /// 描述：Domian Object for 打印模板主表
    /// 作者：路聪
    /// 创建日期:2013-7-19
    /// </summary>
    [EntityClassAttribut("PrintTemplate", "打印模板管理", IsOnAppendProperty = false)]
    public class PrintTemplate : BasicInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        [EntityControl("名称", false, true, 2)]
        [EntityField(255, IsNotNullable = true)]
        public string Name { get; set; }
        /// <summary>
        /// 模板描述
        /// </summary>
        [EntityControl("模板描述", true, true, 3)]
        [EntityField(200)]
        public string Detail { get; set; }
        /// <summary>
        /// 模板数据
        /// </summary>
        [EntityControl("模板数据", false, true, 4)]
        [EntityField(30, IsNotNullable = false)]
        public byte[] DesignData { get; set; }
        /// <summary>
        /// 模板创建时间
        /// </summary>
        [EntityControl("模板创建时间", true, true, 5)]
        [EntityField(50)]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 模板最后修改时间
        /// </summary>
        [EntityControl("模板最后修改时间", true, true, 6)]
        [EntityField(50)]
        public DateTime LastMode { get; set; }

        //protected override string GetSearchSQL()
        //{
        //    //string n = this.ToString();
        //    //string sql = "SELECT TOP 10 * FROM (SELECT ROW_NUMBER() OVER (ORDER BY [ID] desc) AS RowIndex,* FROM (SELECT PrintTemplate.[Name],PrintTemplate.[Detail],PrintTemplate.[DesignData],PrintTemplate.[CreateDate],PrintTemplate.[LastMode],PrintTemplate.[ID] FROM PrintTemplate ) a) A WHERE RowIndex > 0";
        //    //return sql;
        //}
    }
}
