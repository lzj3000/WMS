using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;
using Way.EAP.DataAccess.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;


namespace Acc.Business.WMS.XHY.Model
{
    [EntityClassAttribut("Acc_WMS_ProCheck", "质检单", IsOnAppendProperty = true)]
     public class ProCheck : BasicInfo,IPropertyValueType
    {
        /// <summary>
        /// 源单数据模型
        /// </summary>
        [EntityField(100)]
        public int SourceID { get; set; }
        /// <summary>
        /// 源单控制器
        /// </summary>
        [EntityField(100)]
        public string SourceController { get; set; }
        /// <summary>
        /// 源单编码
        /// </summary>
        [EntityControl("源单编码", false, false, 1)]
        [EntityField(100)]
        public string SourceCode { get; set; }

        [EntityControl("质检类型", false, true, 5)]        
        [EntityField(15)]
        [ValueTypeProperty]
        public string CheckType { get; set; } 

        [EntityControl("报检部门", false, true,7)]
        [EntityForeignKey(typeof(Organization), "ID", "OrganizationName")]
        [EntityField(200)]
        public string JobName { get; set; }

        [EntityControl("业务员", false, true, 6)]
        [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName")]
        [EntityField(50)]
        public string CheckMan { get; set; }

        [EntityControl("生产负责人", false, true, 8)]
        [EntityField(50)]
        public string ProduceMan { get; set; }

        [EntityControl("是否合格",false,true,9)]
        [EntityField(2)]
        public bool IsOK { get; set; }

        [EntityControl("质检单类型", false, false, 10)]
        [EntityField(20)]
        [ValueTypeProperty]
        public string CheckTypeName { get; set; }

       
        //private HierarchicalEntityView<ProCheck, ProCheckMaterials> _Materials;
        ///// <summary>
        ///// 入库单入库物料集合
        ///// </summary>
        //[HierarchicalEntityControl(isadd=false,isremove=false)]
        //public HierarchicalEntityView<ProCheck, ProCheckMaterials> Materials
        //{
        //    get
        //    {
        //        if (_Materials == null)
        //        {
        //            _Materials = new HierarchicalEntityView<ProCheck, ProCheckMaterials>(this);
        //        }
        //        return _Materials;
        //    }
        //}

        PropertyValueType[] IPropertyValueType.GetValueType(ValueTypeArgs e)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            //list.AddRange(base.OnGetValueType(e));
            if (e.ColumnName == "CheckType")
            {
                PropertyValueType vt = new PropertyValueType();
                vt.Text = "全检";
                vt.Value = "0";
                list.Add(vt);
                PropertyValueType vt1 = new PropertyValueType();
                vt1.Text = "抽样";
                vt1.Value = "1";
                list.Add(vt1);
                //hidisger service 微软大数据展示
            }
            if (e.ColumnName == "CheckTypeName")
            {
                PropertyValueType vt = new PropertyValueType();
                vt.Text = "采购入库质检";
                vt.Value = "1";
                list.Add(vt);
                PropertyValueType vt1 = new PropertyValueType();
                vt1.Text = "生产入库质检";
                vt1.Value = "2";
                list.Add(vt1);
                //hidisger service 微软大数据展示
            }
            return list.ToArray();
        }
    }
}
