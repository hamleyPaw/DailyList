using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HamleyPaw.DailyList.Models;

namespace HamleyPaw.DailyList.SupportingTypes
{
    public class ActionItemActionEventArgs
    {
        private readonly ActionItemAction action;
        public ActionItemAction Action
        {
            get
            {
                return this.action;
            }
        }

        public ActionItemActionEventArgs(ActionItemAction action)
    {
        this.action = action;
    }
    }

}
