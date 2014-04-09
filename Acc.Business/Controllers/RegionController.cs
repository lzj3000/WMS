using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;

namespace Acc.Business.Controllers
{
    public class RegionController : BusinessController
    {
        public RegionController() : base(new Region()) { }

        //public override bool IsReviewedState
        //{
        //    get
        //    {
        //        return true;
        //    }
        //}
        protected override string OnGetPath()
        {
            return "Views/manager/region.htm";
        }
    }
}
