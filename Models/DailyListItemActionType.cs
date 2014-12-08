using System.ComponentModel;

namespace HamleyPaw.DailyList.Models
{
    public enum DailyListItemActionType
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
