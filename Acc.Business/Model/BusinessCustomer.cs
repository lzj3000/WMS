using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;

namespace Acc.Business.Model
{
    /// <summary>
    /// 往来企业
    /// </summary>
    [EntityClassAttribut("Acc_Bus_BusinessCustomer", "往来企业", IsOnAppendProperty = true)]
    public class BusinessCustomer : BasicInfo,IPropertyValueType
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        [EntityControl("企业名称", false, true, 2)]
        [EntityField(255, IsNotNullable = true)]
        public string CUSTOMERNAME { get; set; }
        ///// <summary>
        ///// 客户级别
        ///// </summary>
        //[EntityControl("企业级别", false, true, 3)]
        //[EntityField(255)]
        ////[ValueTypeProperty]
        //public int FCLASS { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        [EntityControl("企业类型", false, true, 4)]
        [EntityField(200, IsNotNullable = true)]
        [ValueTypeProperty]
        public string FTYPE { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [EntityControl("电话", false, true, 5)]
        [EntityField(50)]
        public string FPHONE { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [EntityControl("传真", false, true, 6)]
        [EntityField(50)]
        public string FFAX { get; set; }
        /// <summary>
        /// 工商注册号
        /// </summary>
        [EntityControl("工商注册号", false, true, 7)]
        [EntityField(80)]
        public string REGISTRATIONID { get; set; }
        /// <summary>
        /// 所在地区
        /// </summary>
        [EntityControl("所在地区", false, true, 10)]
        [EntityForeignKey(typeof(Region), "ID", "RegionName")]
        [EntityField(10)]
        public int FREGIONID { get; set; }

        /// <summary>
        /// 行业
        /// </summary>
        [EntityControl("行业", false, true, 11)]
        [EntityField(80)]
        [ValueTypeProperty]
        public string FTRADE { get; set; }
        /// <summary>
        /// 公司网址
        /// </summary>
        [EntityControl("企业网址", false, true, 12)]
        [EntityField(80)]
        public string FHOMEPAGE { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [EntityControl("企业地址", false, true, 13)]
        [EntityField(255)]
        public string FADDRESS { get; set; }

        /// <summary>
        /// 纳税人识别号
        /// </summary>
        [EntityControl("纳税识别号", false, true, 14)]
        [EntityField(100)]
        public string TaxpayerIdentificationNumber { get; set; }
        /// <summary>
        /// 开户银行
        /// </summary>
        [EntityControl("开户银行", false, true, 15)]
        [EntityField(100)]
        public string Bark { get; set; }
        /// <summary>
        /// 银行帐号
        /// </summary>
        [EntityControl("银行帐号", false, true, 16)]
        [EntityField(100)]
        public string BarkCode { get; set; }

        /// <summary>
        /// 上级客户
        /// </summary>
        [EntityControl("上级企业", false, true, 17)]
        [EntityForeignKey(typeof(BusinessCustomer), "ID", "CustomerName")]
        [EntityField(10)]
        public int FCUSTOMERID { get; set; }

        /// <summary>
        /// 是否供应商
        /// </summary>
        [EntityControl("是否供应商", false, false, 18)]
        [EntityField(1)]
        public bool IsSuppliers { get; set; }
        /// <summary>
        /// 是否客户
        /// </summary>
        [EntityControl("是否客户", false, false, 19)]
        [EntityField(1)]
        public bool IsCustomer { get; set; }

        public PropertyValueType[] GetValueType(ValueTypeArgs args)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (args.ColumnName == "FTYPE")
            {
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Value = "1";
                pro1.Text = "供应商";
                PropertyValueType pro2 = new PropertyValueType();
                pro2.Value = "2";
                pro2.Text = "客户";
                PropertyValueType pro3 = new PropertyValueType();
                pro3.Value = "3";
                pro3.Text = "经销商";
                list.Add(pro1); list.Add(pro2);
                list.Add(pro3);
            }
            return list.ToArray();
        }
   
    }
}
