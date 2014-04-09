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

namespace Acc.Business.WMS.XHY.Controllers
{
    public class EChartStockController : EChartsController
    {
        #region 重写基类

        protected override void InitOptions(title title, toolbox toobox, tooltip tootip)
        {
            title.text = "";
        }

        protected override string GetSqlString()
        {
            return "select top 8 BC.FNAME,SUM(IM.NUM) TOTAL from Acc_WMS_InfoMaterials IM LEFT JOIN Acc_Bus_BusinessCommodity BC ON IM.MCODE=BC.CODE GROUP BY BC.FNAME";
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
            ec.dataColumn.lengedType = "pie";
            ec.dataColumn.isSplit = false;
        }      
               
        #endregion
    }
}
