using HamleyPaw.DailyList.Models;
using HamleyPaw.DailyList.SupportingTypes;
using HamleyPaw.DailyList.ViewModels.Contexts;
using NodaTime;
using System.Linq;
using System.Windows.Input;

namespace HamleyPaw.DailyList.ViewModels {
    public class MainViewModel : ViewModelBase {
        private DailyListItemsViewModel _DailyList;
        private ActionsViewModel _ActionTimeline;
        private string _NewItemText;
        
        public MainViewModel(IViewModelContext context)
            : base(context)
        {
            var dailyItemsVm = new DailyListItemsViewModel(context);
            dailyItemsVm.ShowItemsForPeriod(DailyListTimePeriod.AllTime);
            DailyList = dailyItemsVm;

            var actionsVm = new ActionsViewModel(context);
            actionsVm.ShowActionsForPeriod(DailyListTimePeriod.AllTime);
            ActionTimeline = actionsVm;
        }

        // Responsibilities...
        // Close (could be called from a menu option)
        // Container for sub-lists
        
        // ??
        // Co-ordinator of list contents..
            // The Daily list will raise new actions which will
            // be added to the daily timeline.
            // The Actions within the timeline will raise actions
            // which will need to be passed back to the Daily list

        public DailyListItemsViewModel DailyList
        {
            get
            {
                return _DailyList;
            }
            set
            {
                if (_DailyList != null)
                {
                    _DailyList.ActionAdded -= ActionAdded;
                }

                _DailyList = value;
                _DailyList.ActionAdded += ActionAdded;
                OnPropertyChanged(() => DailyList);
            }
        }

        public ActionsViewModel ActionTimeline
        {
            get
            {
                return _ActionTimeline;
            }
            private set
            {
                _ActionTimeline = value;
                OnPropertyChanged(() => ActionTimeline);
            }
        }

        public string NewItemText
        {
            get
            {
                return _NewItemText;
            }
            set
            {
                _NewItemText = value;
                OnPropertyChanged(() => NewItemText);
                OnPropertyChanged(() => CanAddItems);
            }
        }

        public bool CanAddItems
        {
            get
            {
                return !string.IsNullOrWhiteSpace(NewItemText);
            }
        }

        #region Commands

        private RelayCommand _CloseCommand;

        public ICommand CloseCommand
        {
            get { return _CloseCommand ?? (_CloseCommand = new RelayCommand(param => OnRequestClose(true))); }
        }

        private RelayCommand _AddItemCommand;
        public ICommand AddItem
        {
            get
            {
                return _AddItemCommand ?? (_AddItemCommand = new RelayCommand(
                    param => AddNewItem(false),
                    param => CanAddItems));
            }
        }

        private RelayCommand _AddItemAndStartCommand;
        public ICommand AddItemAndStart
        {
            get
            {
                return _AddItemAndStartCommand ?? (_AddItemAndStartCommand = new RelayCommand(
                    param => AddNewItem(true),
                    param => CanAddItems));
            }
        }

        #endregion Commands

        private void AddNewItem(bool startItemToo)
        {
            DailyList.AddNewItem(NewItemText, startItemToo);
            NewItemText = null;
        }

        private void ActionAdded(object sender, ItemActionEventArgs e)
        {
            // One of our Items has created an action...
            // Add it to the timeline collection which will decide how to display it.
            ActionTimeline.AddAction(e.ActionId);
        }
    }
}
