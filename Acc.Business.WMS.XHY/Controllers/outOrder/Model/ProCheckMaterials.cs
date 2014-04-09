using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;
using Way.EAP.DataAccess.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Business.WMS.Model;

namespace Acc.Business.WMS.XHY.Model
{
    [EntityClassAttribut("Acc_WMS_ProCheckMaterials", "质检单", IsOnAppendProperty = true)]
    public class ProCheckMaterials : BasicInfo, IPropertyValueType, IEqualityComparer<ProCheckMaterials>
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

        /// <summary>
        /// 源行id
        /// </summary>
        [EntityField(100)]
        public int SourceRowID { get; set; }

        /// <summary>
        /// 源table
        /// </summary>
        [EntityField(100)]
        public string SourceTable { get; set; }

        ///// <summary>
        /////来源单号
        ///// </summary>
        //[EntityControl("来源单号", false, true, 1)]
        //[EntityForeignKey(typeof(ProCheck), "ID", "CODE")]
        //[EntityField(255)]
        //public int PARENTID { get; set; }
        [EntityControl("质检单类型", false, false, 10)]
        [EntityField(20)]
        [ValueTypeProperty]
        public string CheckTypeName { get; set; }

        /// <summary>
        /// 质检物料
        /// </summary>
        [EntityControl("质检物料", false, true, 2)]
        [EntityForeignKey(typeof(Materials), "ID", "FNAME")]
        [EntityField(255, IsNotNullable = true)]
        public string CHECKWARE { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        [EntityControl("物料编码", false, true, 3)]
        [EntityField(255)]
        public string MCODE { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>

        [EntityControl("规格型号", false, true, 4)]
        [EntityField(255)]
        public string FMODEL { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        [EntityControl("批次号", false, true, 5)]
        [EntityField(255)]
        public string BATCHNO { get; set; }

        /// <summary>
        /// 质检数量
        /// </summary>
        [EntityControl("质检数量", true, true, 6)]
        [EntityField(30)]
        public double CHECKNUM { get; set; }

        [EntityControl("是否合格", false, true, 7)]
        [EntityField(2)]
        public bool IsOK { get; set; }
        ///// <summary>
        ///// 质检数量
        ///// </summary>
        //[EntityControl("合格数量", false, true, 7)]
        //[EntityField(30, IsNotNullable = true)]
        //public double QUANUM { get; set; }

        public bool Equals(ProCheckMaterials x, ProCheckMaterials y)
        {
            if (x.ID == y.ID && x.SourceCode == y.SourceCode && x.CHECKWARE == y.CHECKWARE && x.BATCHNO == y.BATCHNO)
                return true;
            return false;
        }

        public int GetHashCode(ProCheckMaterials obj)
        {
            return this.GetHashCode();
        }
        PropertyValueType[] IPropertyValueType.GetValueType(ValueTypeArgs e)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
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
            }
            return list.ToArray();
        }

    }
}
