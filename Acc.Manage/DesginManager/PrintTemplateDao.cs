using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Acctrue.Library.Data;

namespace Acc.Manage.DesginManager
{
    public class PrintTemplateDao : BaseDao<PrintTemplate>
    {


        public IList<PrintTemplate> GetDesign(int pageSize, int pageIndex, out int count)
        {
            IPagedSelector ps = DbEntry.From<PrintTemplate>()
                .Where(Condition.Empty)
                .OrderByDescending(d => d.CreateDate)
                .PageSize(pageSize)
                .GetPagedSelector();

            long recordCount = ps.GetResultCount();
            count = (int)recordCount;

            if (recordCount / pageSize > pageIndex)
                return ps.GetCurrentPage(pageIndex) as IList<PrintTemplate>;
            else
                return ps.GetCurrentPage(recordCount / pageSize) as IList<PrintTemplate>;
        }

    }
}