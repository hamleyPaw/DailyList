using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace HamleyPaw.DailyList.Models
{
    public class ActionItemAction
    {
        public Guid ActionItemActionId { get; set; }
        public Guid ActionItemId { get; set; }
        public Instant ActionTime { get; set; }
        public ActionItemActionType Type { get; set; }
        public string ActionText { get; set; }
    }
}
