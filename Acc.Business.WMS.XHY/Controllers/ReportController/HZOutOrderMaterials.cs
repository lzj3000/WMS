using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Contract.Data.ControllerData;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class HZOutOrderMaterials : ReportBaseController
    {
        [WhereParameter(wType = WhereParameter.WhereType.String, title = "物料编码", field = "MCODE")]
        public string MCODE { get; set; }
        [WhereParameter(wType = WhereParameter.WhereType.String, title = "物料名称", field = "FNAME")]
        public string FNAME { get; set; }
        [WhereParameter(wType = WhereParameter.WhereType.String, title = "批次号", field = "BATCHNO")]
        public string BATCHNO { get; set; }
        [WhereParameter(wType = WhereParameter.WhereType.Date, title = "出库提交时间", field = "Submiteddate")]
        public string Submiteddate { get; set; }
    }
}
