using System;
using System.Collections.Generic;
using System.Text;
using Acctrue.Equipment.Print.Template;
using Acctrue.Library.Data.Definition;

namespace Acc.Manage.DesginManager
{
    /// <summary>
    /// Domian Object for 打印模板主表
    /// </summary>
    [Serializable]
    public class PrintTemplate
    {
        /// <summary>
        /// 打印模板ID
        /// </summary>
        [DbKey]
        public long ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        ///模板描述 
        /// </summary>
        [AllowNull]
        public string Detail { get; set; }

        /// <summary>
        ///模板数据 
        /// </summary>
        [AllowNull]
        public byte[] DesignData { get; set; }

        /// <summary>
        /// 模板创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 模板最后修改时间
        /// </summary>
        public DateTime LastMode { get; set; }
    }
}
