using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Data;
using Acc.Business.Controllers;
using System.Data;
using Acc.Business.WMS.Model;
using Acc.Contract.MVC;
using System.Data.SqlClient;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.WMS.Controllers
{
    public class WMShelp 
    {
        /// <summary>
        /// 描述：获取数据库当前时间
        /// 作者：路聪
        /// 创建日期:2012-12-21
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public DateTime GetDatatime(IDataAction action)
        {
            //IDataAction action = this.model.GetDataAction();
            return Convert.ToDateTime(action.GetValue("select CONVERT(varchar,GETDATE(),120) nowtime"));
        }
        /// <summary>
        /// 描述：自动获取入库单单号
        /// 作者：路聪
        /// 创建日期:2012-12-21
        /// </summary>
        /// <returns></returns>
        public static string GetInOrderNo(string str, IModel table)
        {
            IDataAction action = table.GetDataAction();
            string strInorder = str + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
            string strInorderend = action.GetValue("select MAX(SUBSTRING(ORDERNO,12,14)) maxorderno from Acc_WMS_InOrder where IsDelete=0 and SUBSTRING(ORDERNO,1,11)='" + strInorder + "'").ToString();
            if (strInorderend.Trim() != "")
            {
                return strInorder + (Convert.ToInt32(strInorderend) + 1).ToString().PadLeft(3,'0');
            }
            else
            {
                return strInorder + "001";
            }
        }

        /// <summary>
        /// 描述：自动获取入库单生产线yyyymmdd
        /// 作者：路聪
        /// 创建日期:2012-12-24
        /// </summary>
        /// <returns></returns>
        public static string GetProductionline()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
        }

        /// <summary>
        /// 获取出库单号
        /// </summary>
        /// <param name="table">stockOutOrder</param>
        /// <returns>创建人：柳强</returns>
       // public static string GetStockOutOrderNo(IModel table)
        //{
        //    string order = string.Empty;
        //    switch (table.ToString().ToLower())
        //    {
        //        //原因：为了区别每个表的字段不重名，使用不同的列名
        //        case "Acc_WMS_OutOrder":
        //            order = "code";
        //            break;
        //        case "Acc_WMS_OutNotice":
        //            order = "tzorderno";
        //            break;
        //        case "acc_wms_shippingorder":
        //            order = "code";
        //            break;
        //        case "Acc_WMS_InNotice":
        //            order = "code";
        //            break;
        //        default:
        //            break;
        //    }
           
        //    string strStockOutOrder = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
        //    IDataAction action = table.GetDataAction();
        //    string month = DateTime.Now.Month.ToString();
        //    string str =action.GetValue("select MAX(SUBSTRING("+order+",12,14)) maxorderno from "+table.ToString()+" where IsDelete=0 and SUBSTRING("+order+",4,8)='" + strStockOutOrder + "'").ToString();
        //    if (str.Trim() != "")
        //    {
        //        return strStockOutOrder + (Convert.ToInt32(str) + 1).ToString().PadLeft(3, '0');
        //    }
        //    else
        //    {
        //        return strStockOutOrder + "001";
        //    }
        //}

        /// <summary>
        /// 获取产品库存，根据编码
        /// </summary>
        /// <param name="table"></param>
        /// <param name="materialCode"></param>
        /// <returns></returns>
        public static Decimal GetMaterialNumByCode(string materialCode,IModel table )
        {
            IDataAction action = table.GetDataAction();
            DataTable dt = action.GetDataTable("select materialNo,(SUM(isnull(CAST(NUM as float),0)) - SUM(isnull(CAST(FREEZENUM AS float),0))) as NUM from Acc_WMS_StockInfo_Materials where ISLOCK = 0 and  IsDelete is null and materialNo ='" + materialCode + "' group by Code");
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToDecimal(dt.Rows[0]["NUM"]);
            }
            else
            {
                return 0;
            }
        }

        ///// <summary>
        ///// 添加货区时判断是否存在仓库
        ///// </summary>
        ///// <param name="table"></param>
        ///// <param name="whType">参数：货区</param>
        ///// <returns></returns>
        //public static void GetExistsWareHouse(Contract.MVC.ControllerBase.SaveEvent item)
        //{
        //    WareHouse ware = item.Item as WareHouse;
            
        //    //DataTable dataTable = action.GetDataTable("select * from acc_bus_warehouse where whtype='001'");
        //    //if (dataTable.Rows.Count <= 0)
        //    //{
        //    //    throw new Exception("货区不能添加，请添加仓库后再添加货区！");
        //    //}
        //}

        /// <summary>
        /// 描述：根据物料包装单位ID获得物料包装信息
        /// 作者：路聪
        /// 创建日期:203-01-23
        /// </summary>
        /// <param name="materialid"></param>
        /// <param name="packunitid"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static DataTable getmaterialpack(string materialpackid, IModel table)
        {
            IDataAction action = table.GetDataAction();
            string strSql = "select * from Acc_WMS_MaterialPack aaa left join Acc_WMS_PackUnit bbb on aaa.PACKUNITID=bbb.ID ";
            strSql = strSql + "left join Acc_WMS_PackUnitList  ccc on ccc.PACKUNITID=bbb.ID ";
            strSql = strSql + "left join Acc_WMS_PackUnitMaterials ddd on ccc.PACKUNITCODE=ddd.ID where  aaa.ID='" + materialpackid + "'";
            return action.GetDataTable(strSql);
        }

        /// <summary>
        /// 判断数据库表中行数
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="sqlwhere"></param>
        /// <returns></returns>
        public static int ValiSole(IModel tablename, string sqlwhere)
        {
            IDataAction action = tablename.GetDataAction();
            string strSql = "select * from " + tablename.ToString() + " where 1=1 " + sqlwhere;
            DataTable dt = action.GetDataTable(strSql);
            return dt.Rows.Count;
        }

        /// <summary>
        /// 下推前判断
        /// </summary>
        /// <param name="sin"></param>
        public static void IsPush(ControllerBase sin)
        {
            if (sin.IsReviewedState == true && sin.ActionItem["IsReviewed"].ToString() =="False")
            {
                throw new Exception("异常：单据未审核，不能下推！");
            }
            if (sin.IsSubmit == true && sin.ActionItem["IsSubmited"].ToString() == "False")
            {
                throw new Exception("异常：单据未提交，不能下推！");
            }
        }


        public static void VoliInOrder(StockInOrderMaterials sm)
        { 
            StockInOrder sio = sm.GetForeignObject<StockInOrder>(sm.GetDataAction());
            if (sio.FullName() == "Acc.Business.WMS.Model.StockFinishInOrder")
            {
        
            }
        }


        /// <summary>
        /// 入库下推前数据判断
        /// </summary>
        /// <param name="sin"></param>
        public static void VoliNumPush( StockInOrder sio,StockInOrderMaterials sm,ShippingOrderMaterials spm,StockOutOrderMaterials soom,string strcon)
        {
            //总共需要下推的数量
            double pushsumnum = 0;
            //已经下推的数量
            double pushfinishnum = 0;
            if (sm != null && sm.SourceID != 0 && sm.SourceController != null)
            {
                if (sio.FullName() == "Acc.Business.WMS.Model.StockFinishInOrder")
                {
                    StockInNoticeMaterials sinm = new StockInNoticeMaterials();
                    sinm.PARENTID =Convert.ToInt32(sm.SourceID) ;
                    sinm.MATERIALCODE = sm.MATERIALCODE;
                    pushsumnum = Convert.ToDouble(InNoticepushsumnum(sinm));
                }
                else
                {
                    pushsumnum = Convert.ToDouble(InOrderpushsumnum(sm));
                }
                pushfinishnum=Convert.ToDouble(InOrderpushfinishsumnum(sm,null,null));
                double xunum = pushsumnum - pushfinishnum;
                if (sm.NUM > xunum)
                {
                    Materials m = sm.GetForeignObject<Materials>(sm.GetDataAction());
                    throw new Exception("异常：本次下推中 " + m.FNAME + " 的数量大于需要下推的数量，操作终止！");
                }
            }
            if (spm != null && spm.SourceID != 0 && spm.SourceController != null)
            {
                StockInNoticeMaterials sinm = new StockInNoticeMaterials();
                sinm.PARENTID = Convert.ToInt32(spm.SourceID);
                sinm.MATERIALCODE = spm.SHIPPINGMATERIALS;
                pushsumnum = Convert.ToDouble(InNoticepushsumnum(sinm));
                pushfinishnum = Convert.ToDouble(InOrderpushfinishsumnum(null, spm, null));
                double xunum = pushsumnum - pushfinishnum;
                if (sm.NUM > xunum)
                {
                    Materials m = sm.GetForeignObject<Materials>(sm.GetDataAction());
                    throw new Exception("异常：本次下推中 " + m.FNAME + " 的数量大于需要下推的数量，操作终止！");
                }
            }
            if (soom != null && soom.SourceID!=0 && soom.SourceController!=null)
            {
                if (strcon == "Acc.Business.WMS.Controllers.AcctrueProductionPlanController")
                {
                    StockInNoticeMaterials sinm = new StockInNoticeMaterials();
                    sinm.PARENTID = Convert.ToInt32(soom.SourceID);
                    sinm.MATERIALCODE = soom.MATERIALCODE;
                    pushsumnum = Convert.ToDouble(InNoticepushsumnum(sinm));
                }
                else
                {
                    StockInOrderMaterials sm1 = new StockInOrderMaterials();
                    sm1.PARENTID = soom.PARENTID;
                    sm1.MATERIALCODE = soom.MATERIALCODE;
                    pushsumnum = Convert.ToDouble(InOrderpushsumnum(sm1));
                }
                pushfinishnum = Convert.ToDouble(InOrderpushfinishsumnum(null, null, soom));
                double xunum = pushsumnum - pushfinishnum;
                if (Convert.ToDouble(soom.NUM) > xunum)
                {
                    Materials m = soom.GetForeignObject<Materials>(soom.GetDataAction());
                    throw new Exception("异常：本次下推中 " + m.FNAME + " 的数量大于需要下推的数量，操作终止！");
                }
            }



            //if (spm != null && Convert.ToInt32(spm.SourceID) != 0 && spm.SourceController != null)
            //{
            //    double xunum = Convert.ToDouble(InOrderpushsumnum(null,spm,null)) - Convert.ToDouble(InOrderpushfinishsumnum(null,spm,null));
            //    if (Convert.ToDecimal(spm.SHIPPINGNUM) > Convert.ToDecimal(xunum))
            //    {
            //        //Materials m = spm.GetForeignObject<Materials>(sm.GetDataAction());
            //        EntityList<Materials> list = new EntityList<Materials>(spm.GetDataAction());
            //        list.GetData("ID ='"+spm.SHIPPINGMATERIALS+"'");
            //        throw new Exception("异常：本次下推中 " + list[0].FNAME + " 的数量大于需要下推的数量，操作终止！");
            //    }
            //}
        }

        /*
         */


        //需要下推物料的总数——生产计划明细
        public static string InNoticepushsumnum(StockInNoticeMaterials som)
        {
            EntityList<StockInNoticeMaterials> sooList = new EntityList<StockInNoticeMaterials>(som.GetDataAction());
            sooList.GetData(" PARENTID='" + som.PARENTID + "' and isdelete = 0");
            if (sooList.Count > 0)
            {
                sooList.GetData();
                foreach (StockInNoticeMaterials so in sooList)
                {
                    if (so.MATERIALCODE == som.MATERIALCODE)
                    {
                        return so.NUM.ToString();
                    }
                }
            }
            return "0";
        }

        //需要下推物料的总数——入库明细
        public static string InOrderpushsumnum(StockInOrderMaterials sm)
        {
            if (sm != null)
            {
                EntityList<StockInOrderMaterials> sooList = new EntityList<StockInOrderMaterials>(sm.GetDataAction());
                sooList.GetData("PARENTID='" + sm.PARENTID + "'");
                sooList.GetData();
                foreach (StockInOrderMaterials so in sooList)
                {
                    if (so.MATERIALCODE == sm.MATERIALCODE)
                    {
                        return so.NUM.ToString();
                    }
                }
            }
            return "0";
        }


        //需要下推物料的总数——出库明细
        public static string OutOrderpushsumnum(StockOutOrderMaterials soo)
        {
            if (soo != null)
            {
                EntityList<StockOutOrderMaterials> sooList = new EntityList<StockOutOrderMaterials>(soo.GetDataAction());
                sooList.GetData();
                sooList.GetData(" PARENTID='" + soo.PARENTID + "' and isdelete =0");
                sooList.GetData();
                foreach (StockOutOrderMaterials so in sooList)
                {
                    if (so.MATERIALCODE == soo.MATERIALCODE)
                    {
                        return so.NUM.ToString();
                    }
                }
            }
            return "0";
        }


        //已经下推物料的总数--退货入库
        public static string InOrderpushfinishsumnum(StockInOrderMaterials sm, ShippingOrderMaterials spm,StockOutOrderMaterials sim)
        {
            double num = 0;
            if (sm != null)
            {
                var sooList = new EntityList<StockInOrderMaterials>(sm.GetDataAction());
                sooList.GetData();
                string str = " SourceID='" + sm.SourceID + "' and MATERIALCODE='" + sm.MATERIALCODE + "' ";
                if(sm.ID!=0)
                {
                    str = str + " and ID<>'"+sm.ID+"'";
                }
                sooList.GetData(str);
                foreach (StockInOrderMaterials sm1 in sooList)
                {
                    num = num + sm1.NUM;
                }
            }
            if (spm != null)
            {
                var sooList = new EntityList<ShippingOrderMaterials>(spm.GetDataAction());
                string str = " SourceCode='" + spm.SourceCode + "' and SHIPPINGMATERIALS='" + spm.SHIPPINGMATERIALS + "' ";
                if (spm.ID!=0)
                {
                    str = str + " and ID<>'" + spm.ID + "'";
                }
                sooList.GetData(str);
                foreach (ShippingOrderMaterials sm1 in sooList)
                {
                    num = num + Convert.ToDouble(sm1.SHIPPINGNUM);
                }
            }
            if (sim != null)
            {
                var sooList = new EntityList<StockOutOrderMaterials>(sim.GetDataAction());
                string str = " sourceController= '" + sim.SourceController + "' and SourceCode='" + sim.SourceCode + "' and MaterialCode='" + sim.MATERIALCODE + "'";
                if (sim.ID != 0)
                {
                    str = str + " and ID<>'" + sim.ID + "'";
                }
                sooList.GetData(str);
                foreach (StockOutOrderMaterials sm1 in sooList)
                {
                    num = num + Convert.ToDouble(sm1.NUM);
                }
            }
            return num.ToString();
        }
    }
}
