using NodaTime;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamleyPaw.DailyList.Models
{
    public class DailyListItemAction
    {
        public Guid DailyListItemActionId { get; set; }
        public Guid DailyListItemId { get; set; }
        public DailyListItemActionType Type { get; set; }
        public string Text { get; set; }
        public DateTime ActionDateTimeUtc { get; set; }
        public DailyListItem DailyListItem { get; set; }

        [NotMapped]
        public Instant Time
        {
            get
            {
                return Instant.FromDateTimeUtc(ActionDateTimeUtc.ToUniversalTime());
            }
            set
            {
                ActionDateTimeUtc = value.ToDateTimeUtc();
            }
        }

        
    }
}
