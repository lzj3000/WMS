using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Data;
using System.IO;

namespace Acc.Business.Model.Interface
{
    /// <summary>
    /// 描述：接口模版
    /// 作者：胡文杰
    /// 创建日期:2013-07-19
    /// </summary>
    [EntityClassAttribut("Acc_Bus_InterfaceTemplate", "接口模版", IsOnAppendProperty = true)]
    public class InterfaceTemplate : BusinessBase, IPropertyValueType
    {
        /// <summary>
        /// 模版名称
        /// </summary>
        [EntityControl("模版名称", false, true, 1)]
        [EntityField(50, IsNotNullable = true)]
        public string TemplateName { get; set; }

        /// <summary>
        /// 服务器IP
        /// </summary>
        [EntityControl("服务器IP", false, true, 2)]
        [EntityField(50, IsNotNullable = true)]
        public string ServerIP { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        [EntityControl("数据库类型", false, true, 3)]
        [EntityField(50, IsNotNullable = true)]
        [ValueTypeProperty]
        public string DBType { get; set; }

        /// <summary>
        /// 配置路径
        /// </summary>
        [EntityControl("配置路径", false, true, 4)]
        [EntityField(50, IsNotNullable = true)]
        [ValueTypeProperty]
        public string URL { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        [EntityControl("数据库", false, true, 5)]
        [EntityField(50, IsNotNullable = true)]
        public string DBName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [EntityControl("用户名", false, true, 6)]
        [EntityField(50, IsNotNullable = true)]
        public string DBUserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [EntityControl("密码", false, true, 7)]
        [EntityField(50)]
        public string DBPassword { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [EntityControl("备注", false, true, 10)]
        [EntityField(200)]
        public string Remark { get; set; }


        /// <summary>
        /// 数据转换集合
        /// </summary>
        private HierarchicalEntityView<InterfaceTemplate, MappingInfo> _DataItems;
        //[HierarchicalEntityControl(visible = false)]
        [HierarchicalEntityControl(isremove = false, isadd = false,isedit=false)]
        public HierarchicalEntityView<InterfaceTemplate, MappingInfo> DataItems
        {
            get
            {
                if (_DataItems == null)
                    _DataItems = new HierarchicalEntityView<InterfaceTemplate, MappingInfo>(this);
                return _DataItems;
            }
        }

        public PropertyValueType[] GetValueType(ValueTypeArgs args)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (args.ColumnName == "DBType")
            {
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Value = "1";
                pro1.Text = "sqlserver";
                PropertyValueType pro2 = new PropertyValueType();
                pro2.Value = "2";
                pro2.Text = "oracle";
                list.Add(pro1);
                list.Add(pro2);
            }
            else if (args.ColumnName == "URL")
            {
                string url = AppDomain.CurrentDomain.BaseDirectory + "Interface\\"; //获取基目录，它由程序集冲突解决程序用来探测程序集。
                DirectoryInfo dir = new DirectoryInfo(url);
                DirectoryInfo[] listdir = dir.GetDirectories();//获取所有文件名称集合
                foreach (DirectoryInfo dirinfo in listdir)
                {
                    PropertyValueType pvt = new PropertyValueType();
                    pvt.Value = dirinfo.Name;
                    pvt.Text = dirinfo.Name;
                    list.Add(pvt);
                }
            }
            return list.ToArray();
        }
    }
}
