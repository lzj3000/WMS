using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;
using Acc.Business.Model;
using System.Data;
using Acc.Contract.Data.ControllerData;
using Acc.Business.Controllers;
using Acc.ECharts;
using Acc.Contract.Data;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class EChartStockAlarmController : EChartsController
    {
        #region 重写基类

        protected override void InitOptions(title title, toolbox toobox, tooltip tootip)
        {
            title.text = "";
        }

        protected override string GetSqlString()
        {
            return "select TOP 10 BC.FNAME,SUM(IM.NUM) TOTAL,WH.WAREHOUSENAME from Acc_WMS_InfoMaterials IM LEFT JOIN Acc_Bus_BusinessCommodity BC ON IM.MCODE=BC.CODE LEFT JOIN Acc_WMS_WareHouse WH ON IM.WAREHOUSEID=WH.ID GROUP BY BC.FNAME,WH.WAREHOUSENAME";
        }

        protected override void SetDataConfig(DataTable dt, EChartsController.DataConfig dataConfig)
        {
            //维度
            Dimension dsn1 = new Dimension();
            dsn1.name = "FNAME";
            dsn1.orderType = OrderType.DESC;
            dataConfig.dimensions.Add(dsn1);

            //指标
            Indicator ind1 = new Indicator();
            ind1.name = "TOTAL";
            ind1.gatherType = GatherType.Sum;
            dataConfig.indicators.Add(ind1);
        }

        protected override void SetEchartConfig(DataTable dt, EChartsController.EchartConfig ec)
        {
            ec.category = "FNAME";
            ec.lenged = "FNAME";
            ec.dataColumn.name = "TOTAL";
            ec.dataColumn.lengedType = "bar";
            ec.dataColumn.isSplit = false;
        }

        [WhereParameter(field = "WAREHOUSENAME", title = "仓库名称", type = "string", visible = true)]
        public string WAREHOUSENAME { set; get; }
        
        //protected override void OnGetParameter(WhereParameter param)
        //{
        //    if (param.field.Equals("WAREHOUSEID", StringComparison.OrdinalIgnoreCase))
        //    {
        //        param.comboxvalue = GetWarehouse();
        //    }
        //}

        //private string GetWarehouse()
        //{
        //    string sql = "select WH.ID,WH.WAREHOUSENAME from Acc_WMS_WareHouse WH WHERE WH.WHTYPE=0";
        //    IDataAction action = this.model.GetDataAction();
        //    DataTable dt = action.GetDataTable(sql);
        //    comboxData cd = new comboxData();
        //    cd.isvtp=true;
        //    List<PropertyValueType> pvts = new List<PropertyValueType>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        PropertyValueType pvt = new PropertyValueType();
        //        pvt.Text = row["WAREHOUSENAME"].ToString();
        //        pvt.Value = row["ID"].ToString();
        //        pvts.Add(pvt);
        //    }
        //    cd.items = pvts.ToArray();
        //    return Newtonsoft.Json.JsonConvert.SerializeObject(cd);
        //}
        #endregion
    }
}
