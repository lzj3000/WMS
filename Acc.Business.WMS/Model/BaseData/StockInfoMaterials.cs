using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using System.Data;
using Way.EAP.DataAccess.Data;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：库存表
    /// 作者：liuqiang
    /// 创建日期:2012-01-09
    /// </summary>
    [EntityClassAttribut("Acc_WMS_InfoMaterials", "库存产品表", IsOnAppendProperty = true)]
    public class StockInfoMaterials : BasicInfo,IPropertyValueType
    {
        public StockInfoMaterials()
        {
            this.PORTCODE = "";
            this.DEPOTWBS = "";
        }
        /// <summary>
        /// 产品
        /// </summary>
        [EntityControl("产品", false, true, 1)]
        [EntityForeignKey(typeof(Materials), "ID", "FNAME")]
        [EntityField(100)]
        public new string Code { get; set; }


        /// <summary>
        /// 产品编码
        /// </summary>
        //[NotSearchAttribut]
        [EntityControl("产品编码", false, true, 2)]
        [EntityField(255)]
        public string MCODE { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        //[NotSearchAttribut]
        [EntityControl("规格型号", false, true, 3)]
        [EntityField(255)]
        public string FMODEL { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        [EntityControl("是否锁定", false, true, 6)]
        [EntityField(255)]
        public bool ISLOCK { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        [EntityControl("批次", false, true, 4)]
        [EntityField(255)]
        public string BATCHNO { get; set; }

        /// <summary>
        /// 亚批次
        /// </summary>
        [EntityControl("亚批次", false, true, 7)]
        [EntityField(255)]
        public string SENBATCHNO { get; set; }


        /// <summary>
        /// 存储仓库
        /// </summary>
        [EntityControl("存储仓库", false, true, 8)]
        [EntityForeignKey(typeof(WareHouse), "ID", "WAREHOUSENAME")]
        [EntityField(255, IsNotNullable = true)]
        public int WAREHOUSEID { get; set; }

        //[EntityControl("仓库名称", false, true, 9)]
        //[EntityForeignKey(typeof(WareHouse), "ID", "WAREHOUSENAME")]
        //public new string STAY1 { get; set; }


        /// <summary>
        /// 存储位置
        /// </summary>
        [EntityControl("存储位置", false, true, 10)]
        [EntityForeignKey(typeof(WareHouse), "ID", "Code")]
        [EntityField(255)]
        public string DEPOTWBS { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        [EntityControl("库存数量", false, true,5)]
        [EntityField(30,Scale=2, IsNotNullable = true)]
        public double NUM { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        [EntityControl("入库时间",false,true,9)]
        [EntityField(255)]
        public DateTime LASTINTIME { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        [EntityControl("出库时间", false, true, 10)]
        [EntityField(255)]
        public DateTime LASTOUTTIME { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [EntityControl("状态", false, true, 11)]
        [EntityField(255)]
        [ValueTypeProperty]
        public string STATUS { get; set; }

        
        /// <summary>
        /// 状态
        /// </summary>
        [NotSearchAttribut]
        [EntityControl("是否过期", false, true, 11)]
        [EntityField(255)]
        public string STATUS1 { get; set; }

        /// <summary>
        /// 销售区域
        /// </summary>
        [EntityControl("销售区域", false, true, 12)]
        [EntityField(255)]
        public string SALEAREA { get; set; }

        /// <summary>
        /// 托盘编码
        /// </summary>
        [EntityControl("托盘编码", false, true, 13)]
        [EntityForeignKey(typeof(Ports), "ID", "PORTNO")]
        [EntityField(255)]
        public string PORTCODE { get; set; }

        /// <summary>
        /// 入库仓库
        /// </summary>
        //[EntityControl("入库仓库", false, true, 14)]
        //[EntityField(255)]
        //public string HOUSECODE { get; set; }

        /// <summary>
        /// 入库单号
        /// </summary>
        [EntityControl("入库单号", false, true, 15)]
        [EntityField(255)]
        public string ORDERNO { get; set; }


        /// <summary>
        /// 冻结数量
        /// </summary>
        [EntityControl("冻结数量",false,true,16)]
        [EntityField(30,Scale=2)]
        public double FREEZENUM { get; set; }

        PropertyValueType[] IPropertyValueType.GetValueType(ValueTypeArgs e)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (e.ColumnName == "STATUS")
            {
                PropertyValueType vt = new PropertyValueType();
                vt.Text = "待检验";
                vt.Value = "1";
                list.Add(vt);

                PropertyValueType vt1 = new PropertyValueType();
                vt1.Text = "合格";
                vt1.Value = "2";
                list.Add(vt1);

                PropertyValueType vt2 = new PropertyValueType();
                vt2.Text = "不合格";
                vt2.Value = "3";
                list.Add(vt2);

                PropertyValueType vt3 = new PropertyValueType();
                vt3.Text = "免质检";
                vt3.Value = "4";
                list.Add(vt3);
                //hidisger service 微软大数据展示
            }

            return list.ToArray();
        }

        //protected override string GetSearchSQL()
        //{
        //    string n = this.ToString();
        //    string sql = "select " + n + ".*,Acc_Bus_BusinessCommodity.FMODEL as FMODEL,Acc_Bus_BusinessCommodity.Code as MCODE  from (" + base.GetSearchSQL() + ") " + n + "  left join Acc_Bus_BusinessCommodity on " + n + ".Code=Acc_Bus_BusinessCommodity.ID";
        //    return sql;
        //}

        private HierarchicalEntityView<StockInfoMaterials, InfoInSequence> _StockSequence;
        /// <summary>
        /// 序列码
        /// </summary>
        public HierarchicalEntityView<StockInfoMaterials, InfoInSequence> StockSequence
        {
            get
            {
                if (_StockSequence == null)
                {
                    _StockSequence = new HierarchicalEntityView<StockInfoMaterials, InfoInSequence>(this);
                }
                return _StockSequence;
            }
        }
    }
    /// <summary>
    /// 库存视图
    /// </summary>
    public class StockInfoView : BusinessBase
    {
        public StockInfoView()
        {
            this.SortIndex = "BATCHNO";
        }
        /// <summary>
        /// 商品ID
        /// </summary>
        public int SPID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public int KID { get; set; }
        /// <summary>
        /// 货位ID
        /// </summary>
        public int HWID { get; set; }
        /// <summary>
        /// 托盘ID
        /// </summary>
        [EntityForeignKey(typeof(StockInfoView), "TPID", "TPCODE")]
        public int TPID { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
         [EntityControl("批次号", false, true, 6)]
        public string BATCHNO { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        [EntityControl("商品编码", false, true, 1)]
        public string SPCODE { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [EntityControl("商品名称", false, true, 2)]
        public string SPNAME { get; set; }
        /// <summary>
        /// 库名称
        /// </summary>
       [EntityControl("仓库", false, true, 3)]
        public string KNAME { get; set; }
        /// <summary>
        /// 货位编码
        /// </summary>
        [EntityControl("货位编码", false, true, 4)]
        public string HWCODE { get; set; }
        /// <summary>
        /// 托盘编码
        /// </summary>
       [EntityControl("托盘编码", false, true, 5)]
        public string TPCODE { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        [EntityControl("库存数量", false, true, 7)]
       public double NUM { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        [EntityControl("生产日期", false, true, 125)]
        public virtual DateTime STAY11 { get; set; }

        public string getGetSearchSQL() {
            return GetSearchSQL();
        }

        protected override string GetSearchSQL()
        {
            string sql = "select * from (select a.id,a.Code SPID,a.MCODE SPCODE,e.FNAME SPNAME,a.WAREHOUSEID KID,b.WAREHOUSENAME KNAME,a.DEPOTWBS HWID, c.Code HWCODE,a.PORTCODE TPID, d.Code TPCODE,a.BATCHNO,a.NUM,a.lastintime as STAY11 "
                     + " from Acc_WMS_InfoMaterials a"
                     + " left join Acc_WMS_WareHouse b"
                     + " on a.WAREHOUSEID=b.ID"
                     + " left join Acc_WMS_WareHouse c"
                     + " on a.DEPOTWBS=c.ID"
                     + " left join Acc_WMS_Ports d "
                     + " on a.PORTCODE=d.ID"
                     + " left join Acc_Bus_BusinessCommodity e"
                     + " on a.Code=e.ID) a";
            string w = " where 1=1";
            if (this.SPID > 0)
                w += " and SPID=" + this.SPID;
            if (this.KID > 0)
                w += " and KID=" + this.KID;
            if (this.HWID > 0)
                w += " and HWID=" + this.HWID;
            if (this.TPID > 0)
                w += " and TPID=" + this.TPID;
            if (!string.IsNullOrEmpty(this.BATCHNO))
                w += " and BATCHNO='" + BATCHNO + "'";
            sql = sql + w ;
            return sql;
        }
    }
}
