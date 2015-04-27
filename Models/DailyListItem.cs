using NodaTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamleyPaw.DailyList.Models
{
    public class DailyListItem
    {
        public Guid DailyListItemId { get; set; }
        [StringLength(50)]
        public string ItemText { get; set; }
        public bool IsRunning { get; set; }
        public bool IsComplete { get; set; }
        
        public DateTime CreatedDateTimeUtc { get; set; }

        [NotMapped]
        public Instant CreatedTime
        {
            get
            {
                return Instant.FromDateTimeUtc(CreatedDateTimeUtc.ToUniversalTime());
            }
            set
            {
                CreatedDateTimeUtc = value.ToDateTimeUtc();
            }
        }

        public DateTime? CompletedDateTimeUtc { get; set; }

        [NotMapped]
        public Instant CompletedTime
        {
            get
            {
                return CompletedDateTimeUtc.HasValue
                    ? Instant.FromDateTimeUtc(CompletedDateTimeUtc.Value.ToUniversalTime())
                    : new Instant();
            }
            set
            {
                CompletedDateTimeUtc = value.ToDateTimeUtc();
            }
        }

        private List<DailyListItemAction> _Actions = new List<DailyListItemAction>();
        public List<DailyListItemAction> Actions
        {
            get
            {
                return _Actions;
            }
            set
            {
                _Actions = value;
            }
        }
    }
}
