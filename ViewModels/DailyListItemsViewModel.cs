using System.Data.Entity;
using HamleyPaw.DailyList.Models;
using HamleyPaw.DailyList.SupportingTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using NodaTime;
using HamleyPaw.DailyList.ViewModels.Contexts;

namespace HamleyPaw.DailyList.ViewModels
{
    public class DailyListItemsViewModel : ViewModelBase
    {
        public event EventHandler<ItemActionEventArgs> ActionAdded;

        private ObservableCollection<DailyListItemViewModel> _ItemsToDisplay;

        public DailyListItemsViewModel(IViewModelContext context)
            : base(context)
        {
            ItemsToDisplay = new ObservableCollection<DailyListItemViewModel>();
        }

        public DailyListItemsViewModel(IEnumerable<DailyListItem> itemsToDisplay, IViewModelContext context)
            : base(context)
        {
            ItemsToDisplay = 
                new ObservableCollection<DailyListItemViewModel>();

            itemsToDisplay.ToList().ForEach(AddExistingItem);
        }

        // TODO Get Items by Date
        // TODO Get Items by ID
        // TODO Get Items by QUERY?

        public void ShowItemsForPeriod(DailyListTimePeriod periodToRetrieve)
        {
            ItemsToDisplay = new ObservableCollection<DailyListItemViewModel>();

            using (var itemModel = new DailyListContext())
            {
                //var now = SystemClock.Instance.Now;
                //var dayNumber = now.InUtc().Day;

                //// Get all items for today
                //var currentItems = itemModel.DailyListItems
                //    .Where(i => i.CreatedDateTimeUtc.Day == dayNumber)
                //    .OrderByDescending(i => i.CreatedDateTimeUtc)
                //    .Include(i => i.Actions)
                //    .ToList();

                // TODO Stop ignoring the time period...
                // Get all items...
                var currentItems = itemModel.DailyListItems
                    .OrderByDescending(i => i.CreatedDateTimeUtc)
                    .Include(i => i.Actions)
                    .ToList();

                // TODO Actually we want to list the items in order of last run
                // I think. At the top of the list is not necessarily the last created
                // but the last run

                currentItems.ForEach(AddExistingItem);
            }
        }

        // Responsibilities for this VM
        // Display and admin of a given list of Action Items.
        // Display and admin of a list of running Action Items.

        // Admin will be:
            // Creating new items
            // Deleting items
            // Adding/Removing Items

        public ObservableCollection<DailyListItemViewModel> ItemsToDisplay
        {
            get
            {
                return _ItemsToDisplay;
            }
            private set
            {
                if(_ItemsToDisplay != null)
                {
                    foreach(var item in _ItemsToDisplay)
                    {
                        item.ActionAdded -= ActionItemActionAdded;
                    }
                }

                _ItemsToDisplay = value;

                foreach(var item in _ItemsToDisplay)
                {
                    item.ActionAdded += ActionItemActionAdded;
                }

                OnPropertyChanged(() => ItemsToDisplay);
            }
        }

        private void AddExistingItem(DailyListItem itemToAdd)
        {
            var actionVm = new DailyListItemViewModel(itemToAdd, Context);

            actionVm.ActionAdded += ActionItemActionAdded;

            ItemsToDisplay.Add(actionVm);
        }

        public void AddNewItem(string itemText, bool startItemToo)
        {
            DailyListItem newListItem = null;

            using (var itemModel = new DailyListContext())
            {
                newListItem = itemModel.DailyListItems.Create();

                newListItem.DailyListItemId = Guid.NewGuid();
                newListItem.CreatedTime = SystemClock.Instance.Now;
                newListItem.ItemText = itemText;

                itemModel.DailyListItems.Add(newListItem);

                itemModel.SaveChanges();
            }

            var newItemVm = new DailyListItemViewModel(newListItem, Context);

            newItemVm.ActionAdded += ActionItemActionAdded;

            ItemsToDisplay.Add(newItemVm);

            if (startItemToo)
            {
                newItemVm.StartItem.Execute(null);
            }
        }

        private void ActionItemActionAdded(object sender, ItemActionEventArgs e)
        {
            // One of our Items has created an action...
            // Reraise the event
            OnActionAdded(e);
        }

        protected void OnActionAdded(ItemActionEventArgs actionArgs)
        {
            if (ActionAdded != null)
            {
                ActionAdded(this, actionArgs);
            }
        }
    }
}
