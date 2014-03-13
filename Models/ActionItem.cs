using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace HamleyPaw.DailyList.Models
{
    public class ActionItem
    {
        public Guid ActionItemId { get; set; }
        public string ItemText { get; set; }
        public bool IsRunning { get; set; }
        public bool IsComplete { get; set; }
        public Instant CreatedTime { get; set; }
        public Instant CompletedTime { get; set; }

        private List<ActionItemAction> actions = new List<ActionItemAction>();
        public List<ActionItemAction> Actions
        {
            get
            {
                return this.actions;
            }
            set
            {
                this.actions = value;
            }
        }
    }
}
