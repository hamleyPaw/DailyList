using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using HamleyPaw.DailyList.Models;
using NodaTime;

using System.Linq;
using HamleyPaw.DailyList.ViewModels.Contexts;

namespace HamleyPaw.DailyList.ViewModels
{
    public enum DailyListTimePeriod
    {
        Today = 0,
        Yesterday,
        Last7Days,
        ThisMonth,
        LastMonth,
        AllTime
    }

    public class ActionsViewModel : ViewModelBase
    {
        private ObservableCollection<ActionViewModel> _Actions = null;

        public ActionsViewModel(IViewModelContext context)
            : base(context)
        {
            Actions = new ObservableCollection<ActionViewModel>();
        }

        // Responsibilities for this VM
        // Display and admin of a given list of Actions.
        // Admin will be:
        // Changing the display template (e.g. List, Timeline)

        public ObservableCollection<ActionViewModel> Actions
        {
            get
            {
                return _Actions;
            }
            private set
            {
                _Actions = value;
                OnPropertyChanged(() => Actions);
            }
        }

        public void ShowActionsForItem(Guid dailyListItemId)
        {
            Actions = new ObservableCollection<ActionViewModel>();

            using (var itemModel = new DailyListContext())
            {
                var actionEntities = itemModel.DailyListItemActions
                    .Where(i => i.DailyListItemId == dailyListItemId)
                    .Include(i => i.DailyListItem)
                    .ToList();

                actionEntities.ForEach(i => Actions.Add(new ActionViewModel(i, Context)));
            }
        }

        public void ShowActionsForPeriod(DailyListTimePeriod periodToRetrieve)
        {
            Actions = new ObservableCollection<ActionViewModel>();

            using (var itemModel = new DailyListContext())
            {
                var nowDate = SystemClock.Instance.Now.InUtc().Date;

                var periodStart = nowDate.AtMidnight().ToDateTimeUnspecified();
                var periodEnd = nowDate.AtMidnight().PlusDays(1).ToDateTimeUnspecified();

                //var actionEntities = itemModel.DailyListItemActions
                //    .Where(i => (i.ActionDateTimeUtc >= periodStart) &&
                //        (i.ActionDateTimeUtc < periodEnd))
                //    .Include(i => i.DailyListItem)
                //    .ToList();

                // TODO Implement the period properly
                var actionEntities = itemModel.DailyListItemActions
                    .Include(i => i.DailyListItem)
                    .ToList();

                actionEntities.ForEach(i => Actions.Add(new ActionViewModel(i, Context)));
            }
        }

        public void AddAction(Guid actionId)
        {
            using (var itemModel = new DailyListContext())
            {
                var actionToAdd = 
                    itemModel.DailyListItemActions
                    .Include(i => i.DailyListItem)
                    .SingleOrDefault(i => i.DailyListItemActionId == actionId);
                
                if (actionToAdd != null)
                {
                    Actions.Add(new ActionViewModel(actionToAdd, Context));
                }
            }
        }

        // Commands
        // ??

        // Parse the list of items...
        // Place in chronological order
        // Highlight those actions that come from the same Item

        // Upon adding a new item the list will be parsed again.

    }
}
