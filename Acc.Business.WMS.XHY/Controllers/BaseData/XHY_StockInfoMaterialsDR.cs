using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using System.Data;
using Way.EAP.DataAccess.Data;

namespace Acc.Business.WMS.XHY.Controllers
{
   public  class XHY_StockInfoMaterialsDR : ExcelDRController 
    {
       public override Dictionary<string, EntityForeignKeyAttribute> getST_WJ_items()
       {
           BasicInfo info = bc.model as BasicInfo;
           IEntityBase ieb = (IEntityBase)info;
           Dictionary<string, EntityForeignKeyAttribute> itemss = ((IEntityBase)ieb).GetForeignKey();
           Dictionary<string, EntityForeignKeyAttribute> itemxu = new Dictionary<string,EntityForeignKeyAttribute>();
           foreach (KeyValuePair<string, EntityForeignKeyAttribute> kv in itemss)
           {
               switch( kv.Key.ToUpper())
               {
                   case "DEPOTWBS": itemxu.Add(kv.Key, kv.Value); break;
                   case "CODE": itemxu.Add(kv.Key, kv.Value); break;
                   case "WAREHOUSEID": itemxu.Add(kv.Key, kv.Value); break;
                   case "PORTCODE": itemxu.Add(kv.Key, kv.Value); break;
               }
           }
           //itemxu = itemxu.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value); 
           return itemxu;
       }
       public  override string Construction_of_SQL(string key,string fjid, string fjtable, string fjvalue, DataRow dr, ImportMapping i)
       {
           if (key.ToUpper().Equals("CODE")) {
               fjvalue = key;
               return "select WeightNUM,CommodityType," + fjid + " from  " + fjtable + "  where  " + fjvalue + "='" + dr[i.TcName] + "'";
           }
           if (key.ToUpper().Equals("WAREHOUSEID"))
           {
               fjvalue = "CODE";
               return "select  " + fjid + " from  " + fjtable + "  where  " + fjvalue + "='" + dr[i.TcName] + "'";
           }
           if (key.ToUpper().Equals("DEPOTWBS"))
           {
               return "select " + fjid + " from  " + fjtable + "  where  " + fjvalue + "='" + dr[i.TcName] + "'  and  PARENTID='" + dr["WAREHOUSEID"] + "'";
           }
           return "select " + fjid + " from  " + fjtable + "  where  " + fjvalue + "='" + dr[i.TcName] + "'";
       }
       public override string getInsertZD(List<ImportMapping> im)
       {
           var sortedList = im.OrderByDescending(a => a.TcName).ThenBy(a => a.TcName);
           im = sortedList.ToList(); 
           string strColumns = "ISDELETE,";
           foreach (ImportMapping i in im)
           {
               if (i.TcName.ToUpper().Equals("NUM"))
               {
                   continue;
               }
               else
               {
                   strColumns += i.TcName + ",";
               }
           }
           if (IsMasterTable.Equals("0"))
           {
               strColumns += Z_FKey();
           }
           else
           {
               strColumns = strColumns.Substring(0, strColumns.Length - 1);
           }
           return strColumns + ",NUM,Remark";
           //return base.getInsertZD(im) + ",Remark";
       }
       //public override string getInsertValue(DataRow dr, List<ImportMapping> im, DataTable dt)
       //{
       //    string file = base.FilePath.Substring((base.FilePath.LastIndexOf('$') + 1));
       //    return base.getInsertValue(dr, im, dt) +","+dr["NUM"]+ ",'初始化数据导入文件:" + file + "'";
       //}
       //public override void Entity_Foreign_Key_Processing(ImportMapping i, DataRow dr, double WeightNUM, Dictionary<string, EntityForeignKeyAttribute> itemss, int rownum, IDataAction action)
       //{
       //    #region 实体外键处理
       //    foreach (KeyValuePair<string, EntityForeignKeyAttribute> kv in itemss)
       //    {
       //        /*
       //         *  /// <summary>
       //        /// 申请人ID
       //        /// </summary>
       //        [EntityControl("申请人", false, true, 2)]
       //        [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName")]
       //        [EntityField(50, IsNotNullable = true)]
       //        public int ApplyID { get; set; }
       //         */
       //        string dd = kv.Key;
       //        if (i.TcName.Equals(dd.ToUpper()))
       //        {
       //            string fjtable = kv.Value.ObjectName;//Acc_Bus_OfficeWorker
       //            string fjvalue = kv.Value.DisplayField;//WorkName
       //            string fjid = kv.Value.FieldName;//ID
       //            string fjsql = Construction_of_SQL(dd, fjid, fjtable, fjvalue, dr, i);
       //            //string fjsql = "select " + fjid + " from  " + fjtable + "  where  " + fjvalue+"='"+dr[i.TcName]+"'";
       //            DataTable fj_value_table = action.GetDataTable(fjsql);
       //            if (fj_value_table.Rows.Count == 1)
       //            {
       //                if (i.TcName.Equals("CODE"))
       //                {
       //                    WeightNUM = Convert.ToDouble(fj_value_table.Rows[0]["WeightNUM"]);
       //                    if (fj_value_table.Rows[0]["CommodityType"].ToString() == "2")
       //                    {
       //                        dr["NUM"] = Convert.ToDecimal(dr["NUM"]) / Convert.ToDecimal(WeightNUM);
       //                    }
       //                }
       //                dr[i.TcName] = fj_value_table.Rows[0][FKey()].ToString();
       //                break;
       //            }
       //            else if (fj_value_table.Rows.Count > 1)
       //            {

       //                //throw new Exception("外键表" + fjtable + "中" + fjvalue + "对应多个" + fjid+",请查看外键表");
       //                throw new Exception(i.ScName + ":" + dr[i.TcName] + "在外键表中没有找到，请检查Excel第" + rownum + "行");
       //            }
       //            else
       //            {
       //                //throw new Exception("外键表" + fjtable + "中" + fjvalue + "不存在对应" + fjid + ",请查看外键表");
       //                throw new Exception(i.ScName + ":" + dr[i.TcName] + "在外键表中没有找到，请检查Excel第" + rownum + "行");
       //            }
       //        }
       //        else
       //        {
       //            continue;
       //        }
       //    }
       //    #endregion
       //}


       public override string getInsertValue(DataRow dr, List<ImportMapping> im, DataTable dt, IDataAction action)
       {
           string file = base.FilePath.Substring((base.FilePath.LastIndexOf('$') + 1));
           var sortedList = im.OrderByDescending(a => a.TcName).ThenBy(a => a.TcName);
           im = sortedList.ToList(); //这个时候会排序
           bool is_Finished_Product_Warehouse = false;
           double WeightNUM = 0.0;
           string strValues = "0,";//CREATEDBY,APPLYID;
           int id = 0;
           Dictionary<string, EntityForeignKeyAttribute> itemss;
           if (IsMasterTable.Equals("0"))
           {
               itemss = getST_WJ_Main();
           }
           else
           {
               itemss = getST_WJ_items();
           }
           int rownum = dt.Rows.IndexOf(dr) + 2;
           foreach (ImportMapping i in im)
           {
               if (i.TcName.ToUpper().Equals("NUM"))
               {
                   continue;
               }
               //实体外键处理的方法，可以重写s
               // Entity_Foreign_Key_Processing(i, dr, isFinished_Warehouse, WeightNUM, itemss, rownum,action);
               #region 实体外键处理
               foreach (KeyValuePair<string, EntityForeignKeyAttribute> kv in itemss)
               {
                   /*
                    *  /// <summary>
                   /// 申请人ID
                   /// </summary>
                   [EntityControl("申请人", false, true, 2)]
                   [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName")]
                   [EntityField(50, IsNotNullable = true)]
                   public int ApplyID { get; set; }
                    */
                   string dd = kv.Key;
                   if (i.TcName.Equals(dd.ToUpper()))
                   {
                       string fjtable = kv.Value.ObjectName;//Acc_Bus_OfficeWorker
                       string fjvalue = kv.Value.DisplayField;//WorkName
                       string fjid = kv.Value.FieldName;//ID
                       string fjsql = Construction_of_SQL(dd, fjid, fjtable, fjvalue, dr, i);
                       //string fjsql = "select " + fjid + " from  " + fjtable + "  where  " + fjvalue+"='"+dr[i.TcName]+"'";
                       DataTable fj_value_table = action.GetDataTable(fjsql);
                       if (fj_value_table.Rows.Count == 1)
                       {
                           if (i.TcName.Equals("CODE"))
                           {
                               //WeightNUM = Convert.ToDouble(fj_value_table.Rows[0]["WeightNUM"]);
                               //if (fj_value_table.Rows[0]["CommodityType"].ToString() == "2" && WeightNUM>0)
                               //{
                               //    dr["NUM"] = Convert.ToDecimal(dr["NUM"]) / Convert.ToDecimal(WeightNUM);
                               //}
                               //if (is_Finished_Product_Warehouse)
                               // {
                               //     dr["NUM"] = Convert.ToDecimal(dr["NUM"]) / Convert.ToDecimal(WeightNUM);
                               // }
                           }
                           //else if (i.TcName.Equals("WAREHOUSEID"))
                           //{
                           //    if (dr["WAREHOUSEID"].ToString().Substring(0, 2).Equals("04"))
                           //    {
                           //        //dr["NUM"] = Convert.ToDecimal(dr["NUM"]) / Convert.ToDecimal(WeightNUM);
                           //        is_Finished_Product_Warehouse = true;
                           //    }
                           //}
                           dr[i.TcName] = fj_value_table.Rows[0][FKey()].ToString();
                           break;
                       }
                       else if (fj_value_table.Rows.Count > 1)
                       {

                           //throw new Exception("外键表" + fjtable + "中" + fjvalue + "对应多个" + fjid+",请查看外键表");
                           throw new Exception(i.ScName + ":" + dr[i.TcName] + "在外键表中没有找到，请检查Excel第" + rownum + "行");
                       }
                       else
                       {
                           //throw new Exception("外键表" + fjtable + "中" + fjvalue + "不存在对应" + fjid + ",请查看外键表");
                           throw new Exception(i.ScName + ":" + dr[i.TcName] + "在外键表中没有找到，请检查Excel第" + rownum + "行");
                       }
                   }
                   else
                   {
                       continue;
                   }
               }
               #endregion

               #region 插入子集
               if (IsMasterTable.Equals("0"))
               {
                   if (i.TcName == Z_Z_Field()[0])
                   {
                       if (dr[Z_Z_Field()[0]].ToString().Equals(""))
                       {
                           throw new Exception("所选的对照字段" + Z_Z_Field()[0] + "值不可以为空，请查看Excel文件!");
                       }
                       //string result = getFZDZ(i, dr, Z_Z_Field()[1]);
                       //if (result.Equals("0"))
                       //{
                       //    throw new Exception("所选的对照字段在父表中没有找到对应的ID，请检查Excel数据!");
                       //}
                       //else if (result.Equals("1"))
                       //{
                       //    throw new Exception("所选的对照字段在父表中找到多个对应的ID，请检查对照关系!");
                       //}
                       //else
                       //{
                       //   // id = Convert.ToInt32(getFZDZ(i, dr, Z_Z_Field()[1]));
                       //}
                   }
               }
               #endregion

               //自动编码处理
               if (i.IsAuto)
               {
                   //自动生成编码
                   string code = GetBillNo(bc);//bnc.GetBillNo(bc);
                   strValues += "'" + code + "',";
                   continue;
               }
               switch (i.Type.ToLower())
               {
                   case "int":
                       strValues += "" + Convert.ToInt32(dr[i.TcName]) + ",";
                       break;
                   case "dint":
                       strValues += "" + Convert.ToDecimal(dr[i.TcName]) + ",";
                       break;
                   case "bool":
                       int bl = (Convert.ToString(dr[i.TcName])).ToUpper() == "TRUE" ? 1 : 0;
                       strValues += "" + bl + ",";
                       break;
                   case "string":
                   case "date":
                   default:
                       strValues += "'" + Convert.ToString(dr[i.TcName]) + "',";
                       break;
               }
           }
           if (IsMasterTable.Equals("0"))
           {
               strValues += "'" + id + "'";
           }
           else
           {
               strValues = strValues.Substring(0, strValues.Length - 1);
           }
           return strValues + "," + dr["NUM"] + ",'初始化数据导入文件:" + file + "'";
       }
    }
}
