using System;

using System.Collections.Generic;
using System.Text;
using System.Data;

namespace AcctrueTerminal.Common
{
   public class SessionModel
   {
       #region 业务属性
       public  string ControllerName { get; set; }
       public  int OrderId { get; set; }
       public  int WhourseId { get; set; }
       public int StockId { get; set; }
       public int DepotwbsId { get; set; }
       public  int ModelId { get; set; }
       public  int SourceCodeId { get; set; }
       public int SourceRowId { get; set; }
       public  string strResult { get; set; }
       public  string fdata { get; set; }
       public  int NameId { get; set; }
       public  int PortsId { get; set; }
       public string PortsName { get; set; }
       public  string BinCodeId { get; set; }
       public int UomCodeId { get; set; }
       public int MoveOrderId { get; set; }      
       
       #endregion

       #region 业务DataTable属性
       private static DataTable dtTrayInfo;

       public static DataTable DtTrayInfo
       {
           get 
           {
               if (dtTrayInfo == null)
               {
                   dtTrayInfo = new DataTable();
               }
               return SessionModel.dtTrayInfo; 
           }
           set { SessionModel.dtTrayInfo = value; }
       }

       private static DataTable dtPortsInfo = null;

       public static DataTable DtPortsInfo
       {
           get 
           {
               if (dtPortsInfo == null)
               {
                   dtPortsInfo = new DataTable();
               }
               return SessionModel.dtPortsInfo; 
           }
           set { SessionModel.dtPortsInfo = value; }
       }

       private static DataTable dtMaterialsInfo = null;

       public static DataTable DtMaterialsInfo
       {
           get 
           {
               if (dtMaterialsInfo == null)
               {
                   dtMaterialsInfo = new DataTable();
               }
               return SessionModel.dtMaterialsInfo; 
           }
           set { SessionModel.dtMaterialsInfo = value; }
       }
       private static DataTable dtStockInfo = null;

       public static DataTable DtStockInfo
       {
           get 
           {
               if (dtStockInfo == null)
               {
                   dtStockInfo = new DataTable();
               }
               return SessionModel.dtStockInfo;
           }
           set { SessionModel.dtStockInfo = value; }
       }

       private static DataTable dtWHourseInfo = null;

       public static DataTable DtWHourseInfo
       {
           get 
           {
               if (dtWHourseInfo == null)
               {
                   dtWHourseInfo = new DataTable();
               }
               return SessionModel.dtWHourseInfo;
           }
           set { SessionModel.dtWHourseInfo = value; }
       }

       private static DataTable dtOrderInfo = null;

       public static DataTable DtOrderInfo
       {
           get 
           {
               if (dtOrderInfo == null)
               {
                   dtOrderInfo = new DataTable();
               }
               return SessionModel.dtOrderInfo;
           }
           set { SessionModel.dtOrderInfo = value; }
       }

       private static DataTable dtOrderNoticeMaterialsInfo = null;

       public static DataTable DtOrderNoticeMaterialsInfo
       {
           get 
           {
               if (dtOrderNoticeMaterialsInfo == null)
               {
                   dtOrderNoticeMaterialsInfo = new DataTable();
               }
               return SessionModel.dtOrderNoticeMaterialsInfo;
           }
           set { SessionModel.dtOrderNoticeMaterialsInfo = value; }
       }

       private static DataTable dtOrderMaterialsInfo = null;
       public static DataTable DtOrderMaterialsInfo
       {
           get
           {
               if (dtOrderMaterialsInfo == null)
               {
                   dtOrderMaterialsInfo = new DataTable();
               }
               return SessionModel.dtOrderMaterialsInfo;
           }
           set { SessionModel.dtOrderMaterialsInfo = value; }
       }

      
      

       #endregion      

       public static void Clear()
       {
           SessionModel.DtMaterialsInfo.Clear();
           SessionModel.DtWHourseInfo.Clear();
           SessionModel.DtPortsInfo.Clear();
           SessionModel.DtStockInfo.Clear();
           SessionModel.DtTrayInfo.Clear();
           SessionModel.DtOrderInfo.Clear();
           SessionModel.DtOrderNoticeMaterialsInfo.Clear();
       }
    }
}
