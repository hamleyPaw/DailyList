using System;

namespace HamleyPaw.DailyList.SupportingTypes
{
    public class ItemActionEventArgs
    {
        private readonly Guid _ActionId;
        public Guid ActionId
        {
            get
            {
                return _ActionId;
            }
        }

        public ItemActionEventArgs(Guid actionId)
        {
            _ActionId = actionId;
        }
    }
}
