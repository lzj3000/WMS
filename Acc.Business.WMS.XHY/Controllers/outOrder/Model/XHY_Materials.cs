using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Data;
using Acc.Business.WMS.Model;

namespace Acc.Business.WMS.XHY.Model
{
    /// <summary>
    /// 描述：产品表
    /// 作者：柳强
    /// 创建日期:2013-09-02
    /// </summary>

    public class XHY_Materials:Materials
    {
        public XHY_Materials[] GetMaterials()
        {
            return null;
        }
        public XHY_Materials()
        {
          
        }
        ///<summary>
        ///KG/单品
        /// </summary>
        [EntityControl("单重", false, true, 20)]
        [EntityField(10)]
        public double WeightNUM { get; set; }
    }
}
