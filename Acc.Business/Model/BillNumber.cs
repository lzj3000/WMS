using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.Model
{
    /// <summary>
    /// 描述：单据编号
    /// 作者：胡文杰
    /// 创建日期:2012-12-17
    /// </summary>
    [EntityClassAttribut("Acc_Bus_BillNumber", "单据编号", IsOnAppendProperty = true)]
    public class BillNumber : BasicInfo, IPropertyValueType
    {
        /// <summary>
        /// 单据名称
        /// </summary>
        [EntityControl("单据名称", false, true, 2)]
        [EntityField(100, IsNotNullable = true)]
        public string FBILLNAME { get; set; }

        /// <summary>
        /// 控制器名称（作为生成编号的判断依据）
        /// </summary>
        [EntityControl("控制器名称", false, true, 3)]
        [EntityField(100, IsNotNullable = true)]
        public string FCONTROLLERNAME { get; set; }

        /// <summary>
        /// 表名称（用于检测重复编号）
        /// </summary>
        [EntityControl("表名称", false, true, 3)]
        [EntityField(100, IsNotNullable = true)]
        public string FTABLENAME { get; set; }

        ///// <summary>
        ///// 字段名称
        ///// </summary>
        //[EntityControl("字段名称", false, true, 4)]
        //[EntityField(100, IsNotNullable = true)]
        //public string FFIELDNAME { get; set; }

        /// <summary>
        /// 前缀标识（编号规则）
        /// </summary>
        [EntityControl("前缀标识", false, true, 7)]
        [EntityField(100, IsNotNullable = true)]
        public string FPARTMARK { get; set; }

        /// <summary>
        /// 日期标识（编号规则）
        /// </summary>
        [EntityControl("日期标识", false, true, 8)]
        [EntityField(50)]
        [ValueTypeProperty]
        public string FPARTDATESTYLE { get; set; }

        /// <summary>
        /// 流水长度（编号规则）
        /// </summary>
        [EntityControl("流水长度", false, true, 9)]
        [EntityField(8, IsNotNullable = true)]
        public int FPARTNUMBERLENGTH { get; set; }

        /// <summary>
        /// 最大流水号（流水记录）
        /// </summary>
        [EntityControl("最大流水号", true, true, 10)]
        [EntityField(8)]
        public int FMAXNUMBER { get; set; }

        /// <summary>
        /// 是否消除流水记录
        /// </summary>
        [EntityControl("是否消除流水记录", false, true, 11)]
        [EntityField(1)]
        public bool ISCLEARMAXNUM { get; set; }

        /// <summary>
        /// 日期标识-选项值
        /// </summary>
        public PropertyValueType[] GetValueType(ValueTypeArgs valueArgs)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (valueArgs.ColumnName == "FPARTDATESTYLE")
            {
                PropertyValueType pro = new PropertyValueType();
                pro.Text = "请选择";
                pro.Value = "请选择";
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Text = "yy";
                pro1.Value = "yy";
                PropertyValueType pro2 = new PropertyValueType();
                pro2.Text = "yymm";
                pro2.Value = "yymm";
                list.Add(pro);
                list.Add(pro1);
                list.Add(pro2);
            }
            return list.ToArray();
        }

    }
}
