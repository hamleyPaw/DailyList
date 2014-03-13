using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HamleyPaw.DailyList.Models;
using HamleyPaw.DailyList.SupportingTypes;

namespace HamleyPaw.DailyList.ViewModels
{
    public class ActionItemListViewModel : ViewModelBase
    {
        public ActionItemListViewModel()
        {
            this.ItemsToDisplay = new ObservableCollection<ActionItemViewModel>();
            this.ActionTimeline = new ActionItemActionListViewModel();
        }

        // Responsibilities for this VM
        // Dsiplay and admin of a given list of Action Items.
        // Admin will be:
            // Creating new items
            // Deleting items
            // Adding/Removing Items

        private ObservableCollection<ActionItemViewModel> itemsToDisplay;
        public ObservableCollection<ActionItemViewModel> ItemsToDisplay
        {
            get
            {
                return this.itemsToDisplay;
            }
            set
            {
                this.itemsToDisplay = value;

                foreach(var item in this.itemsToDisplay)
                {
                    item.ActionAdded += ActionItemActionAdded;
                }

                this.OnPropertyChanged(() => this.ItemsToDisplay);
            }
        }

        private ActionItemViewModel selectedItem = null;
        public ActionItemViewModel SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                this.selectedItem = value;
                this.OnPropertyChanged(() => this.SelectedItem);
            }
        }

        private ActionItemActionListViewModel actionTimeline;
        public ActionItemActionListViewModel ActionTimeline
        {
            get
            {
                return this.actionTimeline;
            }
            set
            {
                this.actionTimeline = value;
                this.OnPropertyChanged(() => this.ActionTimeline);
            }
        }

        private string newItemText = null;
        public string NewItemText
        {
            get
            {
                return this.newItemText;
            }
            set
            {
                this.newItemText = value;
                this.OnPropertyChanged(() => this.NewItemText);
                this.OnPropertyChanged(() => this.CanAddItems);
            }
        }

        public bool CanAddItems
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.NewItemText);
            }
        }

        private RelayCommand addItemCommand;
        public ICommand AddItem
        {
            get
            {
                if(addItemCommand == null)
                {
                    addItemCommand = new RelayCommand(
                        param => this.AddNewItem(false),
                        param => this.CanAddItems);
                }

                return addItemCommand;
            }
        }

        private RelayCommand addItemAndStartCommand;
        public ICommand AddItemAndStart
        {
            get
            {
                if(addItemAndStartCommand == null)
                {
                    addItemAndStartCommand = new RelayCommand(
                        param => this.AddNewItem(true),
                        param => this.CanAddItems);
                }

                return addItemAndStartCommand;
            }
        }

        private void AddNewItem(bool startItemToo)
        {
            // Create an instance of the View Model required.
            // Call a method on that to get the data we require.
            // The details of how that is done are left to the VM
            // (for instance, putting up a UI dialog, etc.)

            var newActionItem = new ActionItem();
            newActionItem.ActionItemId = Guid.NewGuid();
            newActionItem.CreatedTime = NodaTime.SystemClock.Instance.Now;
            newActionItem.ItemText = this.NewItemText;

            var actionVM = new ActionItemViewModel(newActionItem);

            actionVM.ActionAdded += ActionItemActionAdded;

            this.ItemsToDisplay.Add(actionVM);

            if(startItemToo)
            {
                actionVM.StartItem.Execute(null);
            }

            this.NewItemText = null;
        }

        void ActionItemActionAdded(object sender, ActionItemActionEventArgs e)
        {
            // ONe of our Action Items has created an action...
            // Add it to the timeline collection which will decide how to display it.
            this.ActionTimeline.AddActionToTimeLine(e.Action);
        }
    }
}
