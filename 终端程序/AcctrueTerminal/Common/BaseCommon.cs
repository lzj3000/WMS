﻿using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using AcctrueTerminal.Model.BaseData;
using AcctrueTerminal.Model.BusinessLogic;
using System.Data.SQLite;
using AcctrueTerminal.Model.IsOffModel;
using System.Windows.Forms;
namespace AcctrueTerminal.Common
{
   public static class BaseCommon
   {
       #region 初始化变量
       public static DialogResult Result;
       private static string strSQL = string.Empty;
       public static string BusUrl = string.Empty;
       public static string LoginUrl = string.Empty;
       private static string ControllerName = string.Empty;
       private static SessionModel sm;
       private static ForeignData fd;
       private static UrlTypeData ud;       
       private static SQLWhere sw;
       private static SQLWhere sw1;
       private static LoadItem li;
       private static List<SQLWhere> ListSw;      
       private static string fdata;
       private static string strControllerName = string.Empty;
       public static string uuid = string.Empty;
       public static string PDASetControllerName = "Acc.Business.Controllers.MobileSetController";
       private static DataTable dt = null;

       static BaseCommon()
       {
           sm = new SessionModel();
           BusUrl = Config.Config.GetConfig(PDASet.AcctrueWebBusAddress, "");
           LoginUrl = Config.Config.GetConfig(PDASet.AcctrueWebLoginAddress, "");//登陆的url
       }
       #endregion

       #region 数据传输名称(对应值必须为小写)

       public static string Login = "login";
       public static string Query = "load";
       public static string Add = "add";
       public static string Edit = "edit";
       public static string Remove = "remove";
       public static string Foreignkey = "loadforeign";
       public static string SubmmitData = "SubmitData";
       public static string ReviewedData = "ReviewedData";
       public static string MoveData = "MoveDepots";
       #endregion      

       #region 终端模块名称
       public static string GroupTray = "组盘";
       public static string ProductsIn = "外购入库";
       public static string ProductsElseIn = "其他入库";
       public static string ProductsSemIn = "半成品入库";
       public static string ProductsProIn = "成品入库";
       public static string ProductsOut = "出库";
       public static string ProductsPicOut = "生产出库";
       public static string ProductsSellOut = "销售出库";
       public static string ProductsElseOut = "其他出库";
       public static string Added = "上架";
       public static string OffShelf = "下架";
       public static string MoveBin = "货位转移";
       public static string MoveTray = "托盘转移";
       public static string WarehouseInventory = "盘点";
       public static string StockSearch = "库存查询";
       public static string EarlyWarning = "预警";
       #endregion

       #region 终端数据库表名
       public static string Materials = "Acc_Bus_BusinessCommodity";
       public static string Whourse = "Acc_WMS_WareHouse";
       public static string Ports = "Acc_WMS_Ports";
       public static string StockIn = "Acc_WMS_InOrder";
       public static string StockInNoticeMaterials = "Acc_WMS_InNoticeMaterials";
       public static string StockInMaterials = "Acc_WMS_InOrderMaterials";
       public static string StockOut = "Acc_WMS_OutOrder";
       public static string StockOutNoticeMaterials = "Acc_WMS_OutNoticeMaterials";
       public static string StockOutMaterials = "Acc_WMS_OutOrderMaterials";
       public static string StockInfoMaterials = "Acc_WMS_InfoMaterials";
      
       #endregion

       #region 获取终端配置信息
       /// <summary>
       /// 获取终端配置信息
       /// </summary>
       /// <param name="ModelName"></param>
       /// <returns></returns>
       public static DataTable GetMobileSetInfo(string ModelName)
       {
           dt = new DataTable();
           fd = new ForeignData();
           ud = new UrlTypeData();
           sw = new SQLWhere();
           li = new LoadItem();
           fd.isfkey = true;
           fd.objectname = "Acc.Business.Model.MobileSetModel";
           fd.filedname = "PROJECTID";
           fd.foreignfiled = "ID";
           fd.foreignobject = "Acc.Business.Model.MobileSetProject";
           fd.displayname = "F1";
           fd.displayfield = "CODE";
           fd.tablename = "Acc_WMS_MobileSetProject";
           fd.parenttablename = "Acc_WMS_MobileSetModel";
           fd.isassociate = true;
           fd.eventrow = "{}";         
           fdata = UIHelper.FDataConversion(fd);
          
           ud.Type = (int)CheckEnum.Foreignkey;
           ud.m = BaseCommon.Foreignkey;
           ud.c = PDASetControllerName;
           if (!string.IsNullOrEmpty(ModelName))
           {               
               sw.ColumnName = "Acc_WMS_MobileSetModel.ModelName";
               sw.Type = "";
               sw.Value = ModelName;
               sw.Symbol = "=";
               ListSw = new List<SQLWhere>();
               ListSw.Add(sw);
           }
           else
           {
               sw.ColumnName = "Acc_WMS_MobileSetModel.IsVisible";
               sw.Type = "";
               sw.Value = "1";
               sw.Symbol = "=";
               ListSw = new List<SQLWhere>();
               ListSw.Add(sw);
           }
          
           li.selecttype = "Acc.Business.Model.MobileSetModel";
           li.page = "-1";
           li.rows = "10";          
           li.whereList = ListSw.ToArray();
           li.columns = new string[] {"ID","ISVISIBLE","ModelName", "CONTROLLERNAME" };
           
           ud.LoadItem = UIHelper.LoadItemConversion(li);
           ud.FDtata = fdata;
           try
           {
               dt = ToJson.getData(ud);            
               return dt;    
           }
           catch(Exception ex)
           {
               UIHelper.ErrorMsg("在获取终端配置信息时发生异常：" + ex.Message);
           }
           return null;
       }
       #endregion

       #region 获取终端是否启用离线功能
       public static string GetIsOFFLINE(string IP)
       {
           string IsOff = string.Empty;
           ud = new UrlTypeData();
           ud.Type = (int)CheckEnum.Query;
           ud.m = BaseCommon.Query;
           ud.c = "Acc.Business.WMS.Controllers.MobileSetController";
           sw = new SQLWhere();
           sw.ColumnName = "Acc.Bussiness.MobileSetModel.TRANURL";
           sw.Type = "";
           sw.Value =IP;
           sw.Symbol = "=";
           li = new LoadItem();
           li.page = "-1";
           li.rows = "10";
           ListSw = new List<SQLWhere>();
           ListSw.Add(sw);
           li.whereList = ListSw.ToArray();
           ud.LoadItem = UIHelper.LoadItemConversion(li);
           dt = ToJson.getData(ud);
           if (dt != null && dt.Rows.Count > 0)
           {
               IsOff = dt.Rows[0]["ISOFFLINE"].ToString();
           }
           return IsOff;
       }
       #endregion

       #region 验证及获取物料信息
       /// <summary>
       /// 验证及获取物料信息
       /// </summary>
       /// <param name="Code"></param>
       /// <returns></returns>
       public static DataTable GetMaterialsInfo(string Code)
       {
           dt = new DataTable();
           if (PDASet.IsOff)
           {
               strSQL = "select * from Acc_Bus_BusinessCommodity";
               if (!string.IsNullOrEmpty(Code))
               {
                   strSQL += " where Acc_Bus_BusinessCommodity.code ='" + Code + "'";
               }
               dt = SqliteHelper.ExecuteDataTable(DataBaseConnection.GetSQLiteConn(), CommandType.Text, strSQL);              
           }
           else
           {
               ud = new UrlTypeData();
               ud.Type = (int)CheckEnum.Query;
               ud.m = BaseCommon.Query;
               ud.c = "Acc.Business.WMS.Controllers.MaterialsController";
               if (string.IsNullOrEmpty(Code))
               {
                   li = new LoadItem();
                   li.page = "-1";
                   li.rows = "10";
               }
               else
               {
                   sw = new SQLWhere();
                   sw.ColumnName = "Acc_Bus_BusinessCommodity.code";
                   sw.Type = "";
                   sw.Value = Code;
                   sw.Symbol = "=";
                   li = new LoadItem();
                   li.page = "-1";
                   li.rows = "10";
                   ListSw = new List<SQLWhere>();
                   ListSw.Add(sw);
                   li.whereList = ListSw.ToArray();
                   li.columns = new string[] { "ID", "CODE", "FNAME", "FMODEL", "FUNITID", "BATCH", "SEQUENCECODE", "CommodityType"};
               }
               ud.LoadItem = UIHelper.LoadItemConversion(li);              
               dt = ToJson.getData(ud);               
           }
           return dt;
       }

       #region 注视代码
       public static DataTable GetMaterialsInfoOff(string Code)
       {
           DataTable dt = null;
           strSQL = "select * from Acc_Bus_BusinessCommodity";
           if (!string.IsNullOrEmpty(Code))
           {
               strSQL += " where Acc_Bus_BusinessCommodity.code ='"+Code+"'";
           }
           dt=SqliteHelper.ExecuteDataTable(DataBaseConnection.GetSQLiteConn(),CommandType.Text,strSQL);
           return dt;
       }
       #endregion

       #endregion

       #region 获取组盘单据
       public static DataTable GetCheckOrder(string strCheckOrderID)
       {
           ud = new UrlTypeData();
           ud.Type = (int)CheckEnum.Query;
           ud.m = BaseCommon.Query;
           ud.c = "Acc.Business.WMS.Controllers.CheckOrderController";
           sw = new SQLWhere();
           sw.ColumnName = "Acc_WMS_CheckOrder.IsSubmited";
           sw.Type = "";
           sw.Value = "False";
           sw.Symbol = "=";
           ListSw = new List<SQLWhere>();
           ListSw.Add(sw);
           if (!string.IsNullOrEmpty(strCheckOrderID))
           {
               sw1 = new SQLWhere();
               sw1.ColumnName = "Acc_WMS_CheckOrder.ID";
               sw1.Type = "";
               sw1.Value = strCheckOrderID;
               sw1.Symbol = "=";
               ListSw.Add(sw1);
           }
           li = new LoadItem();
           li.page = "-1";
           li.rows = "10";
           li.whereList = ListSw.ToArray();
           li.columns = new string[] { "ID", "Code", "ORDERNAME", "DEPOTWBS", "WAREHOUSENAME", "CHECKORDERTYPE", "REMARK", "CREATEDBY", "CLASSID", "MATERIALCODE" };
           ud.LoadItem = UIHelper.LoadItemConversion(li);
           dt = ToJson.getData(ud);
           return dt;
       }

       public static DataTable GetCheckOrderInfo(string orderid)
       {
           if (!string.IsNullOrEmpty(fdata))
           {
               fdata = null;
           }
           fd = new ForeignData();
           ud = new UrlTypeData();
           li = new LoadItem();
           fd.isfkey = true;
           fd.filedname = "PARENTID";
           fd.foreignfiled = "ID";
           fd.isassociate = true;
           fd.eventrow = "{}";

           fd.objectname = "Acc.Business.WMS.Model.CheckOrderMaterials";
           fd.foreignobject = "Acc.Business.WMS.Model.CheckOrder";
           fd.tablename = StockIn;
           fd.parenttablename = "Acc_WMS_CheckOrderMaterials";
           
           sw = new SQLWhere();
           sw.ColumnName = "Acc_WMS_CheckOrderMaterials.PARENTID";
           sw.Value = orderid.ToString();
           sw.Symbol = "=";
           ListSw = new List<SQLWhere>();
           ListSw.Add(sw);
           li.whereList = ListSw.ToArray();

           li.selecttype = "Acc.Business.WMS.Model.CheckOrderMaterials";
           li.page = "-1";
           li.rows = "10";
           li.columns = new string[] { "ID", "PARENTID", "NEWMCODE", "MATERIALCODE", "PNUM", "NEWFMODEL", "PBATCHNO", "PDEPOTWBS", "PORTCODE" };
           ud.LoadItem = UIHelper.LoadItemConversion(li);

           fdata = UIHelper.FDataConversion(fd);

           ud.Type = (int)CheckEnum.Foreignkey;
           ud.m = BaseCommon.Foreignkey;
           ud.c = "Acc.Business.WMS.Controllers.CheckOrderController";
           ud.FDtata = fdata;
           DataTable dtlist = ToJson.getData(ud);
           if (dtlist != null && dtlist.Rows.Count > 0)
           {
               return dtlist;
           }
           return dtlist;
       }

       public static DataTable GetMoveOrderInfo(string orderid)
       {
           if (!string.IsNullOrEmpty(fdata))
           {
               fdata = null;
           }
           fd = new ForeignData();
           ud = new UrlTypeData();
           li = new LoadItem();
           fd.isfkey = true;
           fd.filedname = "PID";
           fd.foreignfiled = "ID";
           fd.isassociate = true;
           fd.eventrow = "{}";

           fd.objectname = "Acc.Business.WMS.Model.MoveOrderMaterials";
           fd.foreignobject = "Acc.Business.WMS.Model.MoveOrder";
           fd.tablename = StockIn;
           fd.parenttablename = "Acc_WMS_MoveOrderMaterials";

           sw = new SQLWhere();
           sw.ColumnName = "Acc_WMS_MoveOrderMaterials.PID";
           sw.Value = orderid.ToString();
           sw.Symbol = "=";
           ListSw = new List<SQLWhere>();
           ListSw.Add(sw);
           li.whereList = ListSw.ToArray();

           li.selecttype = "Acc.Business.WMS.Model.MoveOrderMaterials";
           li.page = "-1";
           li.rows = "10";
           li.columns = new string[] { "MATERIALCODE", "NUM", "FROMDEPOT", "FROMPORT", "TODEPOT", "TOPORT", "BATCHNO" };
           ud.LoadItem = UIHelper.LoadItemConversion(li);

           fdata = UIHelper.FDataConversion(fd);

           ud.Type = (int)CheckEnum.Foreignkey;
           ud.m = BaseCommon.Foreignkey;
           ud.c = "Acc.Business.WMS.Controllers.MoveOrderController";
           ud.FDtata = fdata;
           DataTable dtlist = ToJson.getData(ud);
           if (dtlist != null && dtlist.Rows.Count > 0)
           {
               return dtlist;
           }
           return dtlist;
       }

       #endregion 根据单号获取主单信息

       #region 获取生产附码单单号
       public static DataTable GetNoticeInfo(string strid,string Code)
       {
           ud = new UrlTypeData();
           ud.Type = (int)CheckEnum.Query;
           ud.m = BaseCommon.Query;
           ud.c = "Acc.Business.WMS.XHY.Controllers.EndowedInNoticeOrderController";
           sw = new SQLWhere();
           sw.ColumnName = "Acc_WMS_InNoticeMaterials1.IsSubmited";
           sw.Type = "";
           sw.Value = "True";
           sw.Symbol = "=";
           ListSw = new List<SQLWhere>();
           ListSw.Add(sw);
           if (!string.IsNullOrEmpty(strid))
           {
               sw.ColumnName = "Acc_WMS_InNoticeMaterials1.ID";
               sw.Type = "";
               sw.Value = strid;
               sw.Symbol = "=";
               ListSw.Add(sw);
               sw = new SQLWhere();
               sw.ColumnName = "Acc_WMS_InNoticeMaterials1.STATE";
               sw.Type = "";
               sw.Value = "0";
               sw.Symbol = "=";
               ListSw.Add(sw);
           }
           if (!string.IsNullOrEmpty(Code))
           {
               sw.ColumnName = "Acc_WMS_InNoticeMaterials1.CODE";
               sw.Type = "";
               sw.Value = Code;
               sw.Symbol = "=";
               ListSw.Add(sw);
           }
           li = new LoadItem();
           li.page = "-1";
           li.rows = "10";
           li.whereList = ListSw.ToArray();
           li.columns = new string[] { "ID", "SOURCECODE", "SOURCEID", "STOCKTYPE", "STATE", "Code", "DEPOTWBS", "REMARK", "MCODE", "MATERIALCODE", "NUM", "FMODEL", "BATCHNO", "STAYNUM", "STAY5", "CREATEDBY" };
          
           ud.LoadItem = UIHelper.LoadItemConversion(li);
           dt = ToJson.getData(ud);
           return dt;
       }
       #endregion 根据单号获取主单信息

       #region 根据单号获取主单信息
       public static DataTable GetOrderInfo(string ConName,string OrderCode,string ModelName)
       {
           string TableName = string.Empty;
           if (string.IsNullOrEmpty(ModelName))
           {
               return dt;
           }
           if (ModelName == ProductsIn || ModelName == ProductsElseIn || ModelName == ProductsSemIn || ModelName == GroupTray || ModelName == ProductsProIn)
           {
               TableName = StockIn;
           }
           if (ModelName == ProductsPicOut || ModelName == ProductsSellOut || ModelName == ProductsElseOut)
           {
               TableName = StockOut;
           }
           if (PDASet.IsOff)
           {
               strSQL = "select *  from " + TableName + "";
               dt = SqliteHelper.ExecuteDataTable(DataBaseConnection.GetSQLiteConn(), CommandType.Text, strSQL);
           }
           else
           {
               ud = new UrlTypeData();
               ud.Type = (int)CheckEnum.Query;
               ud.m = BaseCommon.Query;
               ud.c = ConName;
               if (string.IsNullOrEmpty(OrderCode))
               {
                   sw = new SQLWhere();
                   sw.ColumnName =TableName+".IsSubmited";
                   sw.Type = "";
                   sw.Value = "False";
                   sw.Symbol = "=";
                   //sw1 = new SQLWhere();
                   //sw1.ColumnName = TableName+".STATE";
                   //sw1.Type = "";
                   //sw1.Value = "0";
                   //sw1.Symbol = "=";
                   ListSw = new List<SQLWhere>();
                   ListSw.Add(sw);
                   //ListSw.Insert(1, sw1);
               }
               else
               {
                   sw = new SQLWhere();
                   sw.ColumnName = TableName+".CODE";
                   sw.Type = "";
                   sw.Value = OrderCode;
                   sw.Symbol = "=";
                   ListSw = new List<SQLWhere>();
                   ListSw.Add(sw);
               }
               li = new LoadItem();
               li.page = "-1";
               li.rows = "10";
               li.whereList = ListSw.ToArray();
               li.columns = new string[] { "ID", "SOURCECODE", "SOURCEID", "STOCKTYPE", "STATE", "Code", "TOWHNO", "REMARK", "CLIENTNO", "CREATEDBY" };
               ud.LoadItem = UIHelper.LoadItemConversion(li);
               dt = ToJson.getData(ud);
           }
           return dt;
       }

       #region 注释代码
       //public static DataTable GetOrderInfoOff(string TableName)
       //{
       //    DataTable dt=null;
       //    if (string.IsNullOrEmpty(TableName))
       //    {
       //        return dt ;
       //    }
       //    if (TableName == ProductsIn)
       //    {
       //        TableName = StockIn;
       //    }
       //    if (TableName == ProductsOut)
       //    {
       //        TableName = StockOut;
       //    }
       //    strSQL = "select *  from " + TableName + "";
       //    dt= SqliteHelper.ExecuteDataTable(DataBaseConnection.GetSQLiteConn(), CommandType.Text, strSQL);
       //    return dt;
       //}
       #endregion

       #endregion

       #region 根据主单获取对应的待操作明细
       public static DataTable GetOrderNoticeInfo(string SourceID, string ConName, string ModelName)
       {
           if (!string.IsNullOrEmpty(fdata))
           {
               fdata = null;
           }
           fd = new ForeignData();
           ud = new UrlTypeData();
           li = new LoadItem();
           fd.isfkey = true;
           fd.filedname = "PARENTID";
           fd.foreignfiled = "SOURCEID";
           fd.isassociate = true;
           fd.eventrow = "{}";
           if (ModelName == ProductsIn || ModelName == ProductsElseIn || ModelName == ProductsSemIn || ModelName == GroupTray || ModelName == ProductsProIn)
           {
               fd.objectname = "Acc.Business.WMS.Model.StockInNoticeMaterials";
               fd.foreignobject = "Acc.Business.WMS.Model.StockInOrder";
               fd.tablename = StockIn;
               fd.parenttablename = StockInNoticeMaterials;
               if (!string.IsNullOrEmpty(SourceID))
               {
                   sw = new SQLWhere();
                   sw.ColumnName = StockInNoticeMaterials+".PARENTID";
                   sw.Value = SourceID.ToString();
                   sw.Symbol = "=";
                   ListSw = new List<SQLWhere>();
                   ListSw.Add(sw); 
                   li.whereList = ListSw.ToArray();
               }
               li.selecttype = "Acc.Business.WMS.Model.StockInNoticeMaterials";
               li.page = "-1";
               li.rows = "10";
               li.columns = new string[] { "ID", "PARENTID", "MCODE", "MATERIALCODE", "NUM", "FMODEL", "FUNITID", "BATCHNO", "STAYNUM" };              
               ud.LoadItem = UIHelper.LoadItemConversion(li);
               //ud.LoadItem = "{'selecttype':'Acc.Business.WMS.Model.StockInNoticeMaterials','page':1,'rows':10,'whereList':[{'ColumnName':'Acc_WMS_StockInNoticeMaterials.PARENTID','Value':'" + SourceCodeID + "'}]}";
           }
           if (ModelName == ProductsPicOut || ModelName == ProductsSellOut || ModelName == ProductsElseOut)
           {
               fd.objectname = "Acc.Business.WMS.Model.StockOutNoticeMaterials";
               fd.foreignobject = "Acc.Business.WMS.Model.StockOutOrder";
               fd.tablename = StockOut;
               fd.parenttablename = StockOutNoticeMaterials;
               //ud.LoadItem = "{'selecttype':'Acc.Business.WMS.Model.StockOutNoticeMaterials','page':1,'rows':10,'whereList':[{'ColumnName':'Acc_WMS_StockOutNoticeMaterials.PARENTID','Value':'" + SourceCodeID + "'}]}";
               if (!string.IsNullOrEmpty(SourceID))
               {
                   sw = new SQLWhere();
                   sw.ColumnName = StockOutNoticeMaterials+".PARENTID";
                   sw.Value = SourceID.ToString();
                   sw.Symbol = "=";
                   ListSw = new List<SQLWhere>();
                   ListSw.Add(sw);
                   li.whereList = ListSw.ToArray();
               }              
               li.selecttype = "Acc.Business.WMS.Model.StockOutNoticeMaterials";
               li.page = "-1";
               li.rows = "10";
               li.columns = new string[] { "ID", "PARENTID", "MCODE", "MATERIALCODE", "NUM", "FMODEL", "FUNITID", "BATCHNO", "STAYNUM" };               
               ud.LoadItem = UIHelper.LoadItemConversion(li);
           }

           fdata = UIHelper.FDataConversion(fd);

           ud.Type = (int)CheckEnum.Foreignkey;
           ud.m = BaseCommon.Foreignkey;
           ud.c = ConName;
           ud.FDtata = fdata;
           DataTable dtlist = ToJson.getData(ud);
           if (dtlist.Rows.Count > 0 && ModelName != ProductsSellOut)
           {
               DataRow[] dr = dtlist.Select("STAYNUM>0.00");
               DataTable dtn = new DataTable();
               dtn = dtlist.Clone();//克隆A的结构
               foreach (DataRow row in dr)
               {
                   dtn.ImportRow(row);//复制行数据
               }
               dtlist = dtn;
           }
           return dtlist;
       }

       public static DataTable GetOrderNoticeInfoOff(string OrderID,string TableName)
       {
           DataTable dt = null;
           if (string.IsNullOrEmpty(TableName))
           {
               return dt;
           }
           if (TableName == ProductsIn)
           {
               TableName =StockInNoticeMaterials;
           }
           if (TableName == ProductsOut)
           {
               TableName = StockOutNoticeMaterials;
           }

           strSQL = "select * from " + TableName + " as Tab where Tab.OrderID=" + OrderID + "";
           dt = SqliteHelper.ExecuteDataTable(DataBaseConnection.GetSQLiteConn(), CommandType.Text, strSQL);
           return dt;
       }
       #endregion

       #region 根据主单获取对应的已操作明细
       public static DataTable GetOrderMaterialsInfo(string OrderCodeID, string ConName, string ModelName, string TableName)
       {
           DataTable dt = null;
           if (PDASet.IsOff)
           {               
               strSQL = "select * from '" + TableName + "' as Tab where Tab.PARENTID ='" + OrderCodeID + "'";
               dt = SqliteHelper.ExecuteDataTable(DataBaseConnection.GetSQLiteConn(), CommandType.Text, strSQL);              
           }
           else
           {
               if (!string.IsNullOrEmpty(fdata))
               {
                   fdata = null;
               }
               fd = new ForeignData();
               ud = new UrlTypeData();
               fd.isfkey = true;
               fd.filedname = "PARENTID";
               fd.foreignfiled = "ID";
               fd.displayname = "F4";
               fd.displayfield = "CODE";
               fd.tablename = "BasicInfo";
               fd.isassociate = true;
               fd.eventrow = "{}";

               if (ModelName == ProductsIn)
               {
                   fd.objectname = "Acc.Business.WMS.Model.StockInOrderMaterials";
                   fd.foreignobject = "Acc.Business.WMS.Model.StockInOrder";
                   fd.parenttablename = StockInMaterials;
                   //ud.LoadItem = "{'selecttype':'Acc.Business.WMS.Model.StockInOrder_Materials','page':1,'rows':20,'whereList':[{'ColumnName':'Acc_WMS_StockInOrder_Materials.PARENTID','Type':'','Value':'" + OrderCodeID + "','Symbol':'='}]}";
                   sw = new SQLWhere();
                   sw.ColumnName = StockInMaterials+".PARENTID";
                   sw.Value = OrderCodeID.ToString();
                   sw.Symbol = "=";
                   ListSw = new List<SQLWhere>();
                   ListSw.Add(sw);
                   li = new LoadItem();
                   li.selecttype = "Acc.Business.WMS.Model.StockInOrderMaterials";
                   li.page = "-1";
                   li.rows = "10";
                   li.columns = new string[] { "ID", "DEPOTWBS", "MCODE", "MATERIALCODE", "NUM", "FMODEL", "BATCHNO", "PORTNAME", "STAY4", "IsCheckOk" };
                   li.whereList = ListSw.ToArray();
                   ud.LoadItem = UIHelper.LoadItemConversion(li);
               }
               if (ModelName == ProductsOut)
               {
                   fd.objectname = "Acc.Business.WMS.Model.StockOutOrderMaterials";
                   fd.foreignobject = "Acc.Business.WMS.Model.StockOutOrder";
                   fd.parenttablename = StockOutMaterials;
                   //ud.LoadItem = "{'selecttype':'Acc.Business.WMS.Model.StockOutOrderMaterials','page':1,'rows':20,'whereList':[{'ColumnName':'Acc_WMS_StockOutOrderMaterials.PARENTID','Type':'','Value':'" + OrderCodeID + "','Symbol':'='}]}";

                   sw = new SQLWhere();
                   sw.ColumnName = StockOutMaterials+".PARENTID";
                   sw.Value = OrderCodeID.ToString();
                   sw.Symbol = "=";
                   sw1 = new SQLWhere();
                   sw1.ColumnName = StockOutMaterials + ".STAY4";
                   sw1.Value = "已确认";
                   sw1.Symbol = "<>";
                   ListSw = new List<SQLWhere>();
                   ListSw.Add(sw);
                   if (ConName != "Acc.Business.WMS.XHY.Controllers.PickOutOrderController")
                       ListSw.Insert(1, sw1);
                   li = new LoadItem();
                   li.selecttype = "Acc.Business.WMS.Model.StockOutOrderMaterials";
                   li.page = "-1";
                   li.rows = "10";
                   li.columns = new string[] { "ID", "DEPOTWBS", "MCODE", "MATERIALCODE", "NUM", "FMODEL", "BATCHNO", "PORTNAME", "STAY4", "IsCheckOk" };
                   li.whereList = ListSw.ToArray();
                   ud.LoadItem = UIHelper.LoadItemConversion(li);
               }
               fdata = UIHelper.FDataConversion(fd);

               ud.Type = (int)CheckEnum.Foreignkey;
               ud.m = BaseCommon.Foreignkey;
               ud.c = ConName;
               ud.FDtata = fdata;
               dt = ToJson.getData(ud);               
           }
           return dt;
       }

       #region 注释代码
       public static DataTable GetOrderMaterialsInfoOff(string OrderCodeID, string TableName)
       {
           DataTable dt=null;
           strSQL = "select * from '" + TableName + "' as Tab where Tab.PARENTID ='"+OrderCodeID+"'";
           dt= SqliteHelper.ExecuteDataTable(DataBaseConnection.GetSQLiteConn(),CommandType.Text,strSQL);
           return dt;
       }
       #endregion
       #endregion

       #region 验证托盘及获取托盘信息
       public static DataTable GetPortsInfo(string TrayCode)
       {
           dt = new DataTable();
           if (PDASet.IsOff)
           {
               strSQL = "select * from Acc_WMS_Ports";
               if (!string.IsNullOrEmpty(TrayCode))
               {
                   strSQL += " and Acc_WMS_Ports.PORTNO='" + TrayCode + "'";
               }
               dt = SqliteHelper.ExecuteDataTable(DataBaseConnection.GetSQLiteConn(), CommandType.Text, strSQL);
           }
           else
           {
               li = new LoadItem();
               ud = new UrlTypeData();
               ud.Type = (int)CheckEnum.Query;
               ud.m = BaseCommon.Query;
               ud.c = "Acc.Business.WMS.Controllers.PortsController";

               if (string.IsNullOrEmpty(TrayCode))
               {
                   li.page = "-1";
                   li.rows = "10";
                   li.columns = new string[] { "ID", "PORTNO", "PORTNAME", "STATUS" };
               }
               else
               {
                   sw = new SQLWhere();
                   sw.ColumnName = "Acc_WMS_Ports.PORTNO";
                   sw.Type = "";
                   sw.Value = TrayCode;
                   sw.Symbol = "=";
                   li.page = "-1";
                   li.rows = "10";
                   ListSw = new List<SQLWhere>();
                   ListSw.Add(sw);
                   li.whereList = ListSw.ToArray();
                   li.columns = new string[] { "ID", "PORTNO", "PORTNAME", "STATUS" };
               }
               ud.LoadItem = UIHelper.LoadItemConversion(li);
               dt = ToJson.getData(ud);               
           }
           return dt;
       }
       #region 注释代码
       //public static DataTable GetPortsInfoOff(string TrayCode)
       //{
       //    DataTable dt = null;
       //    strSQL = "select ID, PORTNO, PORTNAME, STATUS from Acc_WMS_Ports";
       //    if (!string.IsNullOrEmpty(TrayCode))
       //    {
       //        strSQL += " and Acc_WMS_Ports.PORTNO='"+TrayCode+"'";
       //    }
       //    dt=SqliteHelper.ExecuteDataTable(DataBaseConnection.GetSQLiteConn(),CommandType.Text,strSQL);
       //    return dt;
       //}
       #endregion
       #endregion

       #region 获取仓库及货位信息
       public static DataTable GetWhouseInfo(string WhourseInfo,string strParentid)
       {
           dt = new DataTable();
           if (PDASet.IsOff)
           {
               strSQL = "select * from Acc_WMS_WareHouse as wh";
               if (string.IsNullOrEmpty(WhourseInfo))
               {
                   strSQL += " where wh.WHTYPE=0";
               }
               else
               {
                   strSQL += " where wh.code='" + WhourseInfo + "'";
               }
               dt = SqliteHelper.ExecuteDataTable(DataBaseConnection.GetSQLiteConn(), CommandType.Text, strSQL);
           }
           else
           {
               ud = new UrlTypeData();
               ud.Type = (int)CheckEnum.Query;
               ud.m = BaseCommon.Query;
               ud.c = "Acc.Business.WMS.Controllers.WareHouseController";
               sw = new SQLWhere();
               ListSw = new List<SQLWhere>();
               li = new LoadItem();
               if (!string.IsNullOrEmpty(WhourseInfo))
               {
                   sw.ColumnName = "Acc_WMS_WareHouse.code";
                   sw.Type = "";
                   sw.Value = WhourseInfo;
                   sw.Symbol = "=";
                   ListSw.Add(sw);
                   //li.whereList = ListSw.ToArray();
               }
               if (!string.IsNullOrEmpty(strParentid))
               {
                   sw = new SQLWhere();
                   sw.ColumnName = "Acc_WMS_WareHouse.PARENTID";
                   sw.Type = "";
                   sw.Value = strParentid;
                   sw.Symbol = "=";
                   ListSw.Add(sw);
               }
               li.whereList = ListSw.ToArray();
               li.page = "-1";
               li.rows = "10";
               li.columns = new string[] { "ID", "WAREHOUSENAME", "CODE", "WHTYPE", "STATUS" };
               ud.LoadItem = UIHelper.LoadItemConversion(li);
               dt = ToJson.getData(ud);
           }
           return dt;
       }
       #region 注视代码
       //public static DataTable GetWhouseInfoOff(string WhourseInfo)
       //{
       //    DataTable dt = null;
       //    strSQL = "select * from Acc_WMS_WareHouse as wh";
       //    if (string.IsNullOrEmpty(WhourseInfo))
       //    {
       //        strSQL += " where wh.WHTYPE=0";
       //    }
       //    else
       //    {
       //        strSQL += " where wh.code='" + WhourseInfo + "'";
       //    }
       //    dt=SqliteHelper.ExecuteDataTable(DataBaseConnection.GetSQLiteConn(),CommandType.Text,strSQL);
       //    return dt;
       //}
       #endregion
       #endregion

       #region 获取组盘数据

       public static DataTable GetZPSUM(string strOrderCode)
       {
           ud = new UrlTypeData();
           ud.Type = 2;
           ud.c = "Acc.Business.WMS.XHY.Controllers.ProduceInOrderController";
           ud.m = "GetZPSUM";
           ud.LoadItem = "{}&CPorderid=" + strOrderCode;
           DataTable dt = ToJson.getData(ud);
           if (dt != null && dt.Rows.Count > 0)
               return dt;
           return dt;
       }


       /// <summary>
       /// 如果入库单不为空将加载当前入库单下面的组盘数据，反之则加载入库单为空的组盘数据。
       /// </summary>
       /// <param name="TrayCode"></param>
       /// <returns></returns>
       public static DataTable GetTrayInfo(string OrderCode, string TrayCode, string strFModel)
       {
           DataTable dtName=GetMobileSetInfo(BaseCommon.GroupTray);
           ud = new UrlTypeData();
           ud.Type = (int)CheckEnum.Query;
           ud.m = BaseCommon.Query;
           ud.c = dtName.Rows[0]["ControllerName"].ToString();

           li = new LoadItem();
           ListSw = new List<SQLWhere>();
           if (!string.IsNullOrEmpty(TrayCode))
           {
               sw = new SQLWhere();
               sw.ColumnName = "Acc_WMS_GroupTray.TrayCode";
               sw.Type = "";
               sw.Value = TrayCode;
               sw.Symbol = "=";
               ListSw.Add(sw);
           }
           if (!string.IsNullOrEmpty(OrderCode))
           {
               sw = new SQLWhere();
               sw.ColumnName = "Acc_WMS_GroupTray.OrderCode";
               sw.Type = "";
               sw.Value = OrderCode;
               sw.Symbol = "=";
               ListSw.Add(sw);
           }
           if (!string.IsNullOrEmpty(strFModel))
           {
               sw = new SQLWhere();
               sw.ColumnName = "Acc_WMS_GroupTray.FModel";
               sw.Type = "";
               sw.Value = strFModel;
               sw.Symbol = "=";
               ListSw.Add(sw);
           }   
           li.page = "-1";
           li.rows = "10"; 
           li.columns = new string[] { "ID", "OrderCode", "WHNAME", "TrayCode", "GName", "GCode", "BatchNo", "TrayNum", "FModel", "FUNITID", "BATCH" };
           li.whereList = ListSw.ToArray();
           ud.LoadItem = UIHelper.LoadItemConversion(li);
           DataTable dt = ToJson.getData(ud);
           if (dt != null && dt.Rows.Count > 0)
           {
               return dt;
           }
           return dt;
       }
       #endregion       

       #region 权限控制
       public static UrlTypeData ModelControlFdata(int ModelId)
       {
           fd = new ForeignData();
           fd.isfkey = true;
           fd.objectname = "Acc.Business.Model.MobileSetModelList";
           fd.filedname = "MODELID";
           fd.foreignfiled = "ID";
           fd.foreignobject = "Acc.Business.Model.MobileSetModel";
           fd.displayname = "F1";
           fd.displayfield = "CODE";
           fd.tablename = "Acc_WMS_MobileSetModel";
           fd.parenttablename = "Acc_WMS_MobileSetModelList";
           fd.isassociate = true;
           fd.eventrow = "{}";
           fdata = UIHelper.FDataConversion(fd);

           ud = new UrlTypeData();
           ud.Type = (int)CheckEnum.Foreignkey;
           ud.m = BaseCommon.Foreignkey;
           ud.c = "Acc.Business.Controllers.MobileSetController";
           //ud.LoadItem = "{'selecttype':'Acc.Business.LDMWMS.Model.MobileSetModelList','page':1,'rows':10,'whereList':[{'ColumnName':'Acc_WMS_MobileSetModelList.MODELID','Value':'" + ModelId + "'},{'ColumnName':'Acc_WMS_MobileSetModelList.IsVisible','Value':'0'}]}";
           //ud.FDtata = fdata;
           
           sw = new SQLWhere();
           sw.ColumnName = "Acc_WMS_MobileSetModelList.MODELID";
           sw.Type = "";
           sw.Value = ModelId.ToString();
           sw.Symbol = "=";

           sw1 = new SQLWhere();
           sw1.ColumnName = "Acc_WMS_MobileSetModelList.IsVisible";
           sw1.Type = "";
           sw1.Value = "0";
           sw1.Symbol = "=";

           li = new LoadItem();
           li.selecttype = "Acc.Business.Model.MobileSetModelList";
           li.page = "-1";
           li.rows = "10";
           ListSw = new List<SQLWhere>();
           ListSw.Add(sw);
           ListSw.Insert(1, sw1);
          
           li.whereList = ListSw.ToArray();           
           li.columns = new string[] { "ModelListName", "ISVISIBLE" };
           ud.LoadItem = UIHelper.LoadItemConversion(li);
           ud.FDtata = fdata;              
           return ud;
       }
       #endregion

       #region 获取库存信息
       /// <summary>
       /// 根据编码、托盘码等信息查询数据
       /// </summary>
       /// <param name="TrayCode"></param>
       /// <returns></returns>
       public static DataTable GetStockInfoMaterials(string TrayCode,string Code,string BinCode)
       {
           dt = new DataTable();
           ud = new UrlTypeData();
           ud.Type = (int)CheckEnum.Query;
           ud.m = BaseCommon.Query;
           ud.c = "Acc.Business.WMS.Controllers.StockInfoMaterialsController";
           ListSw = new List<SQLWhere>();
           if (!string.IsNullOrEmpty(TrayCode))
           {
               sw = new SQLWhere();
               sw.ColumnName = StockInfoMaterials+".PORTCODE";
               sw.Value = TrayCode;
               sw.Symbol = "=";
               ListSw.Add(sw);
              
           }
           if (!string.IsNullOrEmpty(Code))
           {
               sw = new SQLWhere();
               sw.ColumnName = StockInfoMaterials+".CODE";
               sw.Value = Code;
               sw.Symbol = "=";
               ListSw.Add(sw);
               
           }
           if(!string.IsNullOrEmpty(BinCode))
           {
               sw = new SQLWhere();
               sw.ColumnName = StockInfoMaterials+".DEPOTWBS";
               sw.Value = BinCode;
               sw.Symbol = "=";
               ListSw.Add(sw);            
           }
           else if(!string.IsNullOrEmpty(TrayCode)&&!string.IsNullOrEmpty(Code))
           {
               ud.LoadItem = "{'page':-1,'rows':10,'whereList':[{'ColumnName':'" + StockInfoMaterials + ".PORTCODE','Type':'','Value':'" + TrayCode + "','Symbol':'='},{'ColumnName':'" + StockInfoMaterials + ".CODE','Type':'','Value':'" + Code + "','Symbol':'='}]}";
           }
           li = new LoadItem();
           li.page = "-1";
           li.rows = "10";
           li.whereList = ListSw.ToArray();
           li.columns = new string[] { "ID", "WAREHOUSEID", "DEPOTWBS", "PORTCODE", "CODE", "MCODE", "FMODEL", "BATCHNO", "NUM", "LASTINTIME" };
           ud.LoadItem = UIHelper.LoadItemConversion(li);
           dt = ToJson.getData(ud);           
           return dt;
       }

       /// <summary>
       /// 根据编码、托盘码等信息查询数据
       /// </summary>
       /// <param name="TrayCode"></param>
       /// <returns></returns>
       public static DataTable GetStockInfoMaterialsBywarehouse(string TrayCode, string Code, string BinCode,string warehouseid,string strBatchno)
       {
           dt = new DataTable();
           ud = new UrlTypeData();
           ud.Type = (int)CheckEnum.Query;
           ud.m = BaseCommon.Query;
           ud.c = "Acc.Business.WMS.Controllers.StockInfoMaterialsController";
           ListSw = new List<SQLWhere>();
           if (!string.IsNullOrEmpty(TrayCode))
           {
               sw = new SQLWhere();
               sw.ColumnName = StockInfoMaterials + ".PORTCODE";
               sw.Value = TrayCode;
               sw.Symbol = "=";
               ListSw.Add(sw);

           }
           if (!string.IsNullOrEmpty(Code))
           {
               sw = new SQLWhere();
               sw.ColumnName = StockInfoMaterials + ".CODE";
               sw.Value = Code;
               sw.Symbol = "=";
               ListSw.Add(sw);

           }
           if (!string.IsNullOrEmpty(BinCode))
           {
               sw = new SQLWhere();
               sw.ColumnName = StockInfoMaterials + ".DEPOTWBS";
               sw.Value = BinCode;
               sw.Symbol = "=";
               ListSw.Add(sw);
           }
           if (!string.IsNullOrEmpty(warehouseid))
           {
               sw = new SQLWhere();
               sw.ColumnName = StockInfoMaterials + ".WAREHOUSEID";
               sw.Value = warehouseid;
               sw.Symbol = "=";
               ListSw.Add(sw);
           }
           if (!string.IsNullOrEmpty(strBatchno))
           {
               sw = new SQLWhere();
               sw.ColumnName = StockInfoMaterials + ".BATCHNO";
               sw.Value = ToJson.UrlEncode(strBatchno);
               sw.Symbol = "=";
               ListSw.Add(sw);
           }
           li = new LoadItem();
           li.page = "-1";
           li.rows = "10";
           li.whereList = ListSw.ToArray();
           li.columns = new string[] { "ID", "WAREHOUSEID", "DEPOTWBS", "PORTCODE", "CODE", "MCODE", "FMODEL", "BATCHNO", "NUM", "LASTINTIME" };
           ud.LoadItem = UIHelper.LoadItemConversion(li);
           dt = ToJson.getData(ud);
           return dt;
       }


       /// <summary>
       /// 报含OR的查询
       /// </summary>
       /// <param name="TrayCode"></param>
       /// <returns></returns>
       public static DataTable GetStockInfoMaterialsBywarehouseAndOr(string TrayCode, string Code, string BinCode, string warehouseid, string strBatchno)
       {
           dt = new DataTable();
           ud = new UrlTypeData();
           ud.Type = (int)CheckEnum.Query;
           ud.m = BaseCommon.Query;
           ud.c = "Acc.Business.WMS.Controllers.StockInfoMaterialsController";
           ListSw = new List<SQLWhere>();
           if (!string.IsNullOrEmpty(TrayCode))
           {
               sw = new SQLWhere();
               sw.ColumnName = StockInfoMaterials + ".PORTCODE";
               sw.Value = TrayCode;
               sw.Symbol = "=";
               ListSw.Add(sw);

           }
           if (!string.IsNullOrEmpty(Code))
           {
               sw = new SQLWhere();
               sw.ColumnName = StockInfoMaterials + ".CODE";
               sw.Value = Code;
               sw.Symbol = "=";
               ListSw.Add(sw);

           }
           //if (!string.IsNullOrEmpty(Code))
           //{
           //    sw = new SQLWhere();
           //    sw.ColumnName = StockInfoMaterials + ".F1";
           //    sw.Value = Code;
           //    sw.Symbol = "like";
           //    sw.Relation = "or";
           //    ListSw.Add(sw);

           //}
           if (!string.IsNullOrEmpty(BinCode))
           {
               sw = new SQLWhere();
               sw.ColumnName = StockInfoMaterials + ".DEPOTWBS";
               sw.Value = BinCode;
               sw.Symbol = "=";
               ListSw.Add(sw);
           }
           if (!string.IsNullOrEmpty(warehouseid))
           {
               sw = new SQLWhere();
               sw.ColumnName = StockInfoMaterials + ".WAREHOUSEID";
               sw.Value = warehouseid;
               sw.Symbol = "=";
               ListSw.Add(sw);
           }
           if (!string.IsNullOrEmpty(strBatchno))
           {
               sw = new SQLWhere();
               sw.ColumnName = StockInfoMaterials + ".BATCHNO";
               sw.Value = strBatchno;
               sw.Symbol = "=";
               ListSw.Add(sw);
           }
           li = new LoadItem();
           li.page = "-1";
           li.rows = "10";
           li.whereList = ListSw.ToArray();
           li.columns = new string[] { "ID", "WAREHOUSEID", "DEPOTWBS", "PORTCODE", "CODE", "MCODE", "FMODEL", "BATCHNO", "NUM", "LASTINTIME" };
           ud.LoadItem = UIHelper.LoadItemConversion(li);
           dt = ToJson.getData(ud);
           return dt;
       }
       #endregion

       #region 修改托盘状态
       public static bool EditPortsState(string loaditem)
       {
           ud = new UrlTypeData();
           ud.c = "Acc.Business.WMS.Controllers.PortsController";
           ud.m = BaseCommon.Edit;
           ud.LoadItem = loaditem;
           sm.strResult = ToJson.ExecuteMethod(ud);
           if (sm.strResult == "Y")
           {
               return true;
           }
           return false;
       }
       #endregion

       #region 查看产品是否允许单件码管理
       public static bool CheckMaterialSequenState(string materialscode)
       {
           SessionModel.DtMaterialsInfo = GetMaterialsInfo(materialscode);
           if (SessionModel.DtMaterialsInfo.Rows[0]["SEQUENCECODE"].ToString() == "True")
               return true;
           else
               return false;
       }
       #endregion

       #region 下载【物料】数据到终端本地数据库
       public static bool DownLoadMaterials(DataTable dt)
       {
           try
           {               
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   strSQL = "insert into Acc_Bus_BusinessCommodity values("+dt.Rows[i]["ID"].ToString()+",'" + dt.Rows[i]["CODE"].ToString() + "','" + dt.Rows[i]["FNAME"].ToString() + "','" + dt.Rows[i]["FMODEL"].ToString() + "','" + dt.Rows[i]["BATCH"].ToString() + "','" + dt.Rows[i]["SEQUENCECODE"].ToString() + "','" + dt.Rows[i]["FUNITID"].ToString() + "'," + dt.Rows[i]["CommodityType"].ToString() + ")";
                   SqliteHelper.ExecuteNonQuery(DataBaseConnection.StartSqlliteTransaction(), CommandType.Text, strSQL);
               }
               DataBaseConnection.Commint();
               return true;
           }
           catch (Exception ex)
           {
               DataBaseConnection.Rollback();
               UIHelper.ErrorMsg(ex.Message);
               return false;
           }

       }
       #endregion

       #region 下载【仓库】数据到终端本地数据库
       public static bool DownLoadWhourse(DataTable dt)
       {
           try
           {
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   strSQL = "insert into Acc_WMS_WareHouse values(" + dt.Rows[i]["ID"].ToString() + ",'" + dt.Rows[i]["CODE"].ToString() + "','" + dt.Rows[i]["WAREHOUSENAME"].ToString() + "'," + dt.Rows[i]["WHTYPE"].ToString() + "," + dt.Rows[i]["STATUS"].ToString() + ")";
                   SqliteHelper.ExecuteNonQuery(DataBaseConnection.StartSqlliteTransaction(), CommandType.Text, strSQL);
               }
               DataBaseConnection.Commint();
               return true;
           }
           catch (Exception ex)
           {
               DataBaseConnection.Rollback();
               UIHelper.ErrorMsg(ex.Message);
               return false;
           }

       }
       #endregion

       #region 下载【托盘】数据到终端本地数据库
       public static bool DownLoadPorts(DataTable dt)
       {
           try
           {
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   strSQL = "insert into Acc_WMS_Ports values(" + dt.Rows[i]["ID"].ToString() + ",'" + dt.Rows[i]["PORTNO"].ToString() + "','" + dt.Rows[i]["PORTNAME"].ToString() + "'," + dt.Rows[i]["STATUS"].ToString() + ")";
                   SqliteHelper.ExecuteNonQuery(DataBaseConnection.StartSqlliteTransaction(), CommandType.Text, strSQL);
               }
               DataBaseConnection.Commint();
               return true;
           }
           catch (Exception ex)
           {
               DataBaseConnection.Rollback();
               UIHelper.ErrorMsg(ex.Message);
               return false;
           }
       }
       #endregion

       #region 下载【入库】数据到终端本地数据库
       public static bool DownLoadStockInOrderInfo(DataTable dt,DataTable dtList)
       {
           try
           {
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   strSQL = "insert into Acc_WMS_StockInOrder values(" + dt.Rows[i]["ID"].ToString() + "," + dt.Rows[i]["SourceID"].ToString() + ",'" + dt.Rows[i]["SourceCode"].ToString() + "'," + dt.Rows[i]["STOCKTYPE"].ToString() + "," + dt.Rows[i]["STATE"].ToString() + ",'" + dt.Rows[i]["CODE"].ToString() + "','" + dt.Rows[i]["TOWHNO"].ToString() + "','" + dt.Rows[i]["TOWHNO_WAREHOUSENAME"].ToString() + "')";
                   SqliteHelper.ExecuteNonQuery(DataBaseConnection.StartSqlliteTransaction(),CommandType.Text,strSQL);
               }
               for (int i = 0; i < dtList.Rows.Count; i++)
               {
                   strSQL = "insert into Acc_WMS_StockInNoticeMaterials values(" + dtList.Rows[i]["ID"].ToString() + "," + dtList.Rows[i]["ORDERID"].ToString() + "," + dtList.Rows[i]["PARENTID"].ToString() + ",'" + dtList.Rows[i]["MATERIALCODE"].ToString() + "','" + dtList.Rows[i]["MATERIALCODE_FNAME"].ToString() + "','" + dtList.Rows[i]["MCODE"].ToString() + "','" + dtList.Rows[i]["FMODEL"].ToString() + "','" + dtList.Rows[i]["FUNITID"].ToString() + "','暂无'," + dtList.Rows[i]["NUM"].ToString() + ")";
                   SqliteHelper.ExecuteNonQuery(DataBaseConnection.StartSqlliteTransaction(), CommandType.Text, strSQL);
               }
               DataBaseConnection.Commint();
               return true;
           }
           catch (Exception ex)
           {
               DataBaseConnection.Rollback();
               UIHelper.ErrorMsg(ex.Message);
               return false;
           }
       }

       #region 注释代码
       //public static bool DownLoadStockInNoticeMaterialsInfo(DataTable dt)
       //{
       //    try
       //    {               
       //        for (int i = 0; i < dt.Rows.Count; i++)
       //        {
       //            strSQL = "insert into Acc_WMS_StockInNoticeMaterials values(" + dt.Rows[i]["ID"].ToString() + "," + dt.Rows[i]["ORDERID"].ToString() + "," + dt.Rows[i]["PARENTID"].ToString() + ",'" + dt.Rows[i]["MATERIALCODE"].ToString() + "','" + dt.Rows[i]["MATERIALCODE_FNAME"].ToString() + "','" + dt.Rows[i]["MCODE"].ToString() + "','" + dt.Rows[i]["FMODEL"].ToString() + "','" + dt.Rows[i]["FUNITID"].ToString() + "','" + dt.Rows[i]["FUNITID_UNITNAME"].ToString() + "'," + dt.Rows[i]["NUM"].ToString() + ")";
       //            SqliteHelper.ExecuteNonQuery(DataBaseConnection.StartSqlliteTransaction(), CommandType.Text, strSQL);
       //        }
       //        DataBaseConnection.Commint();
       //        return true;
       //    }
       //    catch (Exception ex)
       //    {
       //        DataBaseConnection.Rollback();
       //        UIHelper.ErrorMsg(ex.Message);
       //        return false;
       //    }
       //}
       #endregion

       #endregion

       #region 下载【出库】数据到终端本地数据库
       public static bool DownLoadStockOutOrderInfo(DataTable dt,DataTable dtList)
       {
           try
           {
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   strSQL = "insert into Acc_WMS_StockOutOrder values(" + dt.Rows[i]["ID"].ToString() + "," + dt.Rows[i]["SourceID"].ToString() + ",'" + dt.Rows[i]["SourceCode"].ToString() + "'," + dt.Rows[i]["STOCKTYPE"].ToString() + "," + dt.Rows[i]["STATE"].ToString() + ",'" + dt.Rows[i]["CODE"].ToString() + "','" + dt.Rows[i]["TOWHNO"].ToString() + "','" + dt.Rows[i]["TOWHNO_WAREHOUSENAME"].ToString() + "')";
                   SqliteHelper.ExecuteNonQuery(DataBaseConnection.StartSqlliteTransaction(), CommandType.Text, strSQL);
               }
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   strSQL = "insert into Acc_WMS_StockOutNoticeMaterials values(" + dtList.Rows[i]["ID"].ToString() + "," + dtList.Rows[i]["ORDERID"].ToString() + "," + dtList.Rows[i]["PARENTID"].ToString() + ",'" + dtList.Rows[i]["MATERIALCODE"].ToString() + "','" + dtList.Rows[i]["MATERIALCODE_FNAME"].ToString() + "','" + dtList.Rows[i]["MCODE"].ToString() + "','" + dtList.Rows[i]["FMODEL"].ToString() + "','" + dtList.Rows[i]["FUNITID"].ToString() + "','暂无'," + dtList.Rows[i]["NUM"].ToString() + ")";
                   SqliteHelper.ExecuteNonQuery(DataBaseConnection.StartSqlliteTransaction(), CommandType.Text, strSQL);
               }
               DataBaseConnection.Commint();
               return true;
           }
           catch (Exception ex)
           {
               DataBaseConnection.Rollback();
               UIHelper.ErrorMsg(ex.Message);
               return false;
           }
       }

       #region 注释代码
       //public static bool DownLoadStockOutNoticeMaterialsInfo(DataTable dt)
       //{
       //    try
       //    {
       //        for (int i = 0; i < dt.Rows.Count; i++)
       //        {
       //            strSQL = "insert into Acc_WMS_StockOutNoticeMaterials values(" + dt.Rows[i]["ID"].ToString() + "," + dt.Rows[i]["ORDERID"].ToString() + "," + dt.Rows[i]["PARENTID"].ToString() + ",'" + dt.Rows[i]["MATERIALCODE"].ToString() + "','" + dt.Rows[i]["MATERIALCODE_FNAME"].ToString() + "','" + dt.Rows[i]["MCODE"].ToString() + "','" + dt.Rows[i]["FMODEL"].ToString() + "','" + dt.Rows[i]["FUNITID"].ToString() + "','" + dt.Rows[i]["FUNITID_UNITNAME"].ToString() + "'," + dt.Rows[i]["NUM"].ToString() + ")";
       //            SqliteHelper.ExecuteNonQuery(DataBaseConnection.StartSqlliteTransaction(), CommandType.Text, strSQL);
       //        }
       //        DataBaseConnection.Commint();
       //        return true;
       //    }
       //    catch (Exception ex)
       //    {
       //        DataBaseConnection.Rollback();
       //        UIHelper.ErrorMsg(ex.Message);
       //        return false;
       //    }
       //}
       #endregion

       #endregion

       #region 清除终端本地数据库数据
       public static bool DeletePDAData(string TableName,string TableListName)
       {
           try
           {
               if(!string.IsNullOrEmpty(TableName)&&string.IsNullOrEmpty(TableListName))
               {
                   strSQL = "delete  from "+TableName+"";
                   SqliteHelper.ExecuteNonQuery(DataBaseConnection.StartSqlliteTransaction(), CommandType.Text, strSQL);
                   DataBaseConnection.Commint();
                   return true;                   
               }
               if (!string.IsNullOrEmpty(TableName) && !string.IsNullOrEmpty(TableListName))
               {
                   strSQL = "delete from "+TableName+";delete from "+TableListName+";";
                   SqliteHelper.ExecuteNonQuery(DataBaseConnection.StartSqlliteTransaction(), CommandType.Text, strSQL);
                   DataBaseConnection.Commint();
                   return true;                 
               }
               return false;
           }
           catch (Exception ex)
           {
               DataBaseConnection.Rollback();
               UIHelper.ErrorMsg(ex.Message);
               return false;
           }
       }
       #endregion     

       #region 删除本地数据库数据
       public static bool DeleteData(string TableName,string SQL)
       {
           try
           {
               strSQL = "delete from " + TableName + " " + SQL + "";
               SqliteHelper.ExecuteNonQuery(DataBaseConnection.StartSqlliteTransaction(), CommandType.Text, strSQL);
               DataBaseConnection.Commint();
               return true;
           }
           catch (Exception ex)
           {
               DataBaseConnection.Rollback();
               return false;
           }
       }
       #endregion

       #region 修改主单状态
       public static bool EditOrderState(string ID,string OrderCode,string ConName)
       {
           ud = new UrlTypeData();
           ud.c = ConName;
           ud.m = BaseCommon.Edit;
           ud.LoadItem = "{'ID':" + ID+ ",'CODE':'"+OrderCode+"','STATE':'1','StateBase':3}";
           sm.strResult = ToJson.ExecuteMethod(ud);
           if (sm.strResult == "Y")
           {
               return true;
           }
           return false;
       }
       #endregion

       #region 离线状态下保存入库数据
       public static bool StockInSaveData(StockInMaterials sIn,string TableName)
       {
           try
           {
               strSQL = "INSERT INTO " + TableName + "(ID,PARENTID,PORTNAME,PORTNAME_PORTNO,MCODE,MATERIALCODE,MATERIALCODE_FNAME,FMODEL,BATCHNO,DEPOTWBS,DEPOTWBS_CODE,NUM,SourceRowID) VALUES "+
                          " (null," + sIn.PARENTID +
                         "," + sIn.PORTNAME + 
                         ",'"+ sIn.PORTNAME_PORTNO+
                       "','" + sIn.MCODE + 
                       "','" + sIn.MATERIALCODE + 
                       "','" + sIn.MATERIALCODE_FNAME + 
                       "','" + sIn.FMODEL + 
                       "','" + sIn.BATCHNO + 
                        "'," + sIn.DEPOTWBS +
                        ",'" + sIn.DEPOTWBS_CODE + 
                        "'," + sIn.NUM + 
                         "," + sm.SourceRowId + ")";
              
               SqliteHelper.ExecuteNonQuery(DataBaseConnection.StartSqlliteTransaction(), CommandType.Text, strSQL);
               DataBaseConnection.Commint();
               return true;
           }
           catch (Exception ex)
           {
               DataBaseConnection.Rollback();
               return false;
           }
       }
       #endregion

       #region 离线状态下保存出库数据
       public static bool StockOutSaveData(StockOutMaterials son, string TableName)
       {
           try
           {
               strSQL = "INSERT INTO " + TableName + "(ID,PARENTID,PORTNAME,PORTNAME_PORTNO,MCODE,MATERIALCODE,MATERIALCODE_FNAME,FMODEL,BATCHNO,DEPOTWBS,DEPOTWBS_CODE,NUM,SourceRowID) VALUES " +
                          " (null," + son.PARENTID +
                         "," + son.PORTNAME +
                         ",'" + son.PORTNAME_PORTNO +
                       "','" + son.MCODE +
                       "','" + son.MATERIALCODE +
                       "','" + son.MATERIALCODE_FNAME +
                       "','" + son.FMODEL +
                       "','" + son.BATCHNO +
                        "'," + son.DEPOTWBS +
                        ",'" + son.DEPOTWBS_CODE +
                        "'," + son.NUM +
                         "," + sm.SourceRowId + ")";

               SqliteHelper.ExecuteNonQuery(DataBaseConnection.StartSqlliteTransaction(), CommandType.Text, strSQL);
               DataBaseConnection.Commint();
               return true;
           }
           catch (Exception ex)
           {
               DataBaseConnection.Rollback();
               return false;
           }
       }
       #endregion
   }
}
