using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Acc.Business.Model;
using Acc.Contract.Data.ControllerData;
using Way.EAP.DataAccess.Data;
using Acc.Contract.Data;
using Newtonsoft.Json;
using Way.EAP.DataAccess.Entity;
using Way.EAP.DataAccess.Regulation;
using System.Data;

    namespace Acc.Business.Controllers
{
    public class ProcessStateController : ControllerBase
    {
        public ProcessStateController() : base(new ProcessStateView()) { }

        [WhereParameter]
        public string UserId { set; get; }

        /// <summary>
        /// 获取代办任务
        /// 汇总
        /// </summary>
        /// <returns></returns>
        public string GetProcessStates()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"columns\":[");

            //获取视图对象的列集合,转换成json格式
            ModelData md = this.model.GetModelData();
            string columns = ConvertItemDataToJson(md.childitem);

            sb.Append(columns);
            sb.Append("],\"data\":");

            //从数据库查询代办任务
            IDataAction action = this.model.GetDataAction();
            string sqlSelect = "SELECT distinct t.TaskID,t.WorkUserId,t.ProcessingTime, t.NodeDesc,t.NodeName,t.SubmitUser,t.WorkState,t.ArrivalTime,t1.Url,t.Opinion FROM Acc_Bus_ProcessState t Left join Acc_Bus_SystemModel t1 on t.controllerName=t1.controllerName where t.WorkUserId=" + this.UserId + " and t.WorkState is null order by  t.TaskID";
            string sqlTotal = "SELECT count(*) as total FROM Acc_Bus_ProcessState t Left join Acc_Bus_SystemModel t1 on t.controllerName=t1.controllerName where t.WorkUserId=" + this.UserId + " and t.WorkState is null"; 
            int total = Convert.ToInt32(action.GetValue(sqlTotal));
            DataTable oldDataTable = action.GetDataTable(sqlSelect);
            DataTable newDataTable = new DataTable();
                       
            foreach (DataColumn column in oldDataTable.Columns)
            {
                column.ColumnName = column.ColumnName.ToUpper();
                newDataTable.Columns.Add(new DataColumn(column.ColumnName, column.DataType));
            }

             //根据代办任务的处理单据进行汇总
            Dictionary<string, KeyValue> disDic = new Dictionary<string, KeyValue>();
            foreach (DataRow oldRow in oldDataTable.Rows)
            {
                if (disDic.Keys.Contains(oldRow["NODEDESC"].ToString()))
                {
                    disDic[oldRow["NODEDESC"].ToString()].Count = disDic[oldRow["NODEDESC"].ToString()].Count + 1;
                    disDic[oldRow["NODEDESC"].ToString()].Url = disDic[oldRow["NODEDESC"].ToString()].Url + "," + oldRow["TASKID"].ToString();
                }
                else
                {
                    disDic.Add(oldRow["NODEDESC"].ToString(), new KeyValue() { Count = 1, Url = oldRow["TASKID"].ToString() });
                }
            }

            foreach (DataRow oldRow in oldDataTable.Rows)
            {
                bool isExits = false;
                foreach (DataRow newRow in newDataTable.Rows)
                {
                    if (oldRow["NODEDESC"].ToString().Equals(newRow["NODEDESC"].ToString()))
                    {
                        isExits = true;
                        break;
                    }
                }

                if (!isExits)
                {
                 DataRow tmpRow=newDataTable.NewRow();
                 tmpRow.ItemArray = oldRow.ItemArray;
                 tmpRow["OPINION"] = "<a href='javascript:myOpentab(\"" + oldRow["NODEDESC"] + "\",\"" + oldRow["URL"] + "?Ids=" + disDic[oldRow["NODEDESC"].ToString()].Url + "\")'>处理</a>";
                 tmpRow["TASKID"] = disDic[oldRow["NODEDESC"].ToString()].Count;
                 newDataTable.Rows.Add(tmpRow);
                }
            }
            
            ReadTable rt = new ReadTable { total = total, rows = newDataTable };

            string data = JsonConvert.SerializeObject(rt);
            sb.Append(data);
            sb.Append("}");
            return sb.ToString();
         }

        public string GetProcessStateDetail()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"columns\":[");

            //获取视图对象的列集合,转换成json格式
            ModelData md = this.model.GetModelData();
            string columns = ConvertItemDataToJson(md.childitem);

            sb.Append(columns);
            sb.Append("],\"data\":");

            //从数据库查询代办任务
            IDataAction action = this.model.GetDataAction();
            string sqlSelect = "SELECT distinct t.TaskID,t.WorkUserId,t.ProcessingTime, t.NodeDesc,t.NodeName,t.SubmitUser,t.WorkState,t.ArrivalTime,t1.Url,t.Opinion FROM Acc_Bus_ProcessState t Left join Acc_Bus_SystemModel t1 on t.controllerName=t1.controllerName where t.WorkUserId=" + this.UserId + " and t.WorkState is null order by  t.TaskID";
            string sqlTotal = "SELECT count(*) as total FROM Acc_Bus_ProcessState t Left join Acc_Bus_SystemModel t1 on t.controllerName=t1.controllerName where t.WorkUserId=" + this.UserId + " and t.WorkState is null";
            int total = Convert.ToInt32(action.GetValue(sqlTotal));
            DataTable oldDataTable = action.GetDataTable(sqlSelect);           

            foreach (DataColumn column in oldDataTable.Columns)
            {
                column.ColumnName = column.ColumnName.ToUpper();
                
            }

           
            foreach (DataRow oldRow in oldDataTable.Rows)
            {
                oldRow["OPINION"] = "<a href='javascript:myOpentab(\"" + oldRow["NODEDESC"] + "\",\"" + oldRow["URL"] + "?Ids=" + oldRow["TASKID"] + "\")'>处理</a>";
             }

            ReadTable rt = new ReadTable { total = total, rows = oldDataTable };

            string data = JsonConvert.SerializeObject(rt);
            sb.Append(data);
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
         /// 手动将ItemData转换成json格式
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
         private string ConvertItemDataToJson(ItemData[] items)
         {
             StringBuilder sb = new StringBuilder();
             sb.Append("[");
          
             for (int i = 0; i < items.Length; i++)
             {
                 if (items[i].field.Equals("ROWINDEX", StringComparison.OrdinalIgnoreCase) || items[i].field.Equals("URL", StringComparison.OrdinalIgnoreCase))                {
                     continue;
                 }
                 else
                 {                    
                         if (i == items.Length - 1)
                         {
                             sb.Append("{\"field\":\"" + items[i].field + "\",\"title\":\"" + items[i].title + "\",\"width\":120,\"align\":\"center\",\"sortable\":true}");
                         }
                         else
                         {
                             sb.Append("{\"field\":\"" + items[i].field + "\",\"title\":\"" + items[i].title + "\",\"width\":120,\"align\":\"center\",\"sortable\":true},");
                         }                    
                 }
             }
             sb.Append("]");
           return sb.ToString();
         }

         private class KeyValue
         {
            public int Count { set; get; }
            public string Url { set; get; }
         }
    }
}
