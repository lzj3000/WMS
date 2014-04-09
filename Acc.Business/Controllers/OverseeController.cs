using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Acc.Business.Model.Purview;
using Acc.Contract.Data;

namespace Acc.Business.Controllers
{
    public class OverseeController : ControllerBase
    {
        public OverseeController() : base(new OverseeModel()) { }

        public override bool IsPrint
        {
            get
            {
                return false;
            }
        }
        protected override Contract.Data.ActionCommand[] OnInitCommand(Contract.Data.ActionCommand[] commands)
        {
            ActionCommand[] coms= base.OnInitCommand(commands);
            foreach (ActionCommand a in coms)
            {
                if (a.command == "add" || a.command == "edit" || a.command == "remove")
                    a.visible = false;
            }
            return coms;
        }
        [ActionCommand(name = "清空", title = "清空监视的数据行", index = 1, icon = "icon-remove", isalert = false, isselectrow = false)]
        public void clear()
        { 
           
        }
    }
}
