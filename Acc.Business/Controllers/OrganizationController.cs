using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;
using Acc.Contract.Data;

namespace Acc.Business.Controllers
{
    public class OrganizationController : BusinessController
    {
        public OrganizationController() : base(new Organization()) { }

        protected override void OnInitViewChildItem(Acc.Contract.Data.ModelData data, Acc.Contract.Data.ItemData item)
        {
            
            if (item.IsField("ROWINDEX"))
            { 
               item.visible=false;
            }
            switch (item.field.ToLower())
            {
                case "issubmited":
                case "submiteddate":
                case "submitedby":
                case "modifiedby":
                case "modifieddate":
                case "reviewedby":
                case "revieweddate":
                case "isreviewed":
                    item.visible = false;
                    break;
            }
            if (data.name.EndsWith("OfficeWorker"))
            {
                if (item.IsField("OrganizationID"))
                    item.disabled = true;
            }
        }
        
        protected override string OnGetPath()
        {
            return "Views/manager/organization.htm"; 
        }

        protected override Contract.Data.ControllerData.ReadTable OnForeignLoad(Contract.MVC.IModel model, Contract.Data.ControllerData.loadItem item)
        {
            return base.OnForeignLoad(model, item);
        }

        protected override Contract.Data.ControllerData.ReadTable OnSearchData(Contract.Data.ControllerData.loadItem item)
        {
            return base.OnSearchData(item);
        }
    }
}
