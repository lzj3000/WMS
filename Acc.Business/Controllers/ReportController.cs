using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Acc.Contract.Data;

namespace Acc.Business.Controllers
{
    public class ReportController : ControllerBase
    {
        public ReportController() { }
        public ReportController(IModel model)
            : base(model)
        {
           
        }
        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }
        public override bool IsReviewedState
        {
            get
            {
                return false;
            }
        }
        public override bool IsSubmit
        {
            get
            {
                return false;
            }
        }
        protected override Contract.Data.ActionCommand[] OnInitCommand(Contract.Data.ActionCommand[] commands)
        {
            foreach (ActionCommand c in commands)
            {
                if (c.command == "add" || c.command == "edit" || c.command == "remove")
                    c.visible = false;
            }
            return commands;
        }
    }
}
