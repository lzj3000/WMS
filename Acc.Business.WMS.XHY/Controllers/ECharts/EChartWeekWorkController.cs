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
   public class EChartWeekWorkController:EChartsController
    {
        #region 重写基类

        protected override void InitOptions(title title, toolbox toobox, tooltip tootip)
        {
            title.text = "";
        }

        protected override string GetSqlString()
        {
            return "SELECT FNAME,SUM(NUM) TOTAL,WEEKNAME FROM (select BC.FNAME,IOM.NUM,datename(weekday, IOM.MODIFIEDDATE) WEEKNAME  from dbo.Acc_WMS_InOrderMaterials IOM LEFT JOIN Acc_Bus_BusinessCommodity BC ON IOM.MCODE=BC.CODE WHERE IOM.MODIFIEDDATE >='2014-03-03' AND IOM.MODIFIEDDATE<='2014-03-08') TMP WHERE TMP.FNAME LIKE '%通型' GROUP BY TMP.FNAME,TMP.WEEKNAME";
        }

        protected override void SetDataConfig(DataTable dt, EChartsController.DataConfig dataConfig)
        {
            //维度
            Dimension dsn1 = new Dimension();
            dsn1.name = "FNAME";
            dsn1.orderType = OrderType.ASC;
            Dimension dsn2 = new Dimension();
            dsn2.name = "WEEKNAME";
            dsn2.orderType = OrderType.ASC;
            dataConfig.dimensions.Add(dsn2);
            dataConfig.dimensions.Add(dsn1);

            //指标
            Indicator ind1 = new Indicator();
            ind1.name = "TOTAL";
            ind1.gatherType = GatherType.Sum;
            dataConfig.indicators.Add(ind1);
        }

        protected override void SetEchartConfig(DataTable dt, EChartsController.EchartConfig ec)
        {
            ec.category = "WEEKNAME";
            ec.lenged = "FNAME";
            ec.dataColumn.name = "TOTAL";
            ec.dataColumn.lengedType = "line";
            ec.dataColumn.isSplit = false;
        }

        #endregion
    }
}
