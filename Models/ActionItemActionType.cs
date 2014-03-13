using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamleyPaw.DailyList.Models
{
    public enum ActionItemActionType
    {
        [Description("Started")]
        Start = 0,
        [Description("Paused")]
        Pause,
        [Description("Unpaused")]
        Unpause,
        [Description("Stopped")]
        Stop,
        [Description("Completed")]
        Complete,
        [Description("Note Added")]
        Note,
    }
}
