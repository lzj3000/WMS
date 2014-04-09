using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Contract.Data.ControllerData;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class HZInOrderMaterials : XHY_ReportBaseController
    {
        [WhereParameter(wType = WhereParameter.WhereType.String, title = "仓库名称", field = "wh")]
        public string wh { get; set; }
        [WhereParameter(wType = WhereParameter.WhereType.Date, title = "报表时间", field = "riqi")]
        public string riqi { get; set; }
    }
}
