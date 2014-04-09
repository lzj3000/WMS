using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：借件单管理
    /// 作者：路聪
    /// 创建日期:2013-03-28
    /// </summary>
    [EntityClassAttribut("Acc_WMS_LoanOrder", "借件单管理", IsOnAppendProperty = true)]
    public class LoanOrder : BasicInfo
    {
        /// <summary>
        /// 盘点单号
        /// </summary>
        [EntityControl("借件单号", false, true, 1)]
        [EntityField(255, IsNotNullable = true)]
        public string LOANORDERNO { get; set; }

        private HierarchicalEntityView<LoanOrder, LoanOrderMaterials> _Materials;
        /// <summary>
        /// 盘点单明细
        /// </summary>
        public HierarchicalEntityView<LoanOrder, LoanOrderMaterials> Materials
        {
            get
            {
                if (_Materials == null)
                    _Materials = new HierarchicalEntityView<LoanOrder, LoanOrderMaterials>(this);
                return _Materials;
            }
        }
    }
}
