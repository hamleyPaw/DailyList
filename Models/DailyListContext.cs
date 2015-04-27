using System.Data.Entity;

namespace HamleyPaw.DailyList.Models
{
    public class DailyListContext : DbContext
    {
        public DbSet<DailyListItem> DailyListItems { get; set; }
        public DbSet<DailyListItemAction> DailyListItemActions { get; set; }
    }
}
