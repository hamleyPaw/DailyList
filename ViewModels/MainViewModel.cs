using System.Windows.Input;

using HamleyPaw.DailyList.ViewModels.Contexts;

namespace HamleyPaw.DailyList.ViewModels {
    public class MainViewModel : ViewModelBase {
        public MainViewModel(IViewModelContext context)
            : base(context) {
                this.DailyList = new ActionItemListViewModel();
                this.DailyTimeline = new ActionItemActionListViewModel();
        }

        // Responsibilities...
        // Close (could be called from a menu option)
        // Container for sub-lists
        // Co-ordinator of list contents..
            // The Daily list will raise new actions which will
            // be added to the daily timeline.
            // The Actions within the timeline will raise actions
            // which will need to be passed back to the Daily list

        private RelayCommand closeCommand;
        public ICommand CloseCommand {
            get {
                if (closeCommand == null) {
                    closeCommand = new RelayCommand(param => this.OnRequestClose());
                }

                return closeCommand;
            }
        }

        private ActionItemListViewModel dailyList = null;
        public ActionItemListViewModel DailyList
        {
            get
            {
                return this.dailyList;
            }
            set
            {
                this.dailyList = value;
                this.OnPropertyChanged(() => this.DailyList);
            }
        }


        // TODO To Be Implemented
        //private ActionItemListViewModel previousItems = null;
        //public ActionItemListViewModel PreviousItems
        //{
        //    get
        //    {
        //        return this.previousItems;
        //    }
        //    set
        //    {
        //        this.previousItems = value;
        //        this.OnPropertyChanged(() => this.PreviousItems);
        //    }
        //}

        private ActionItemActionListViewModel dailyTimeline = null;
        public ActionItemActionListViewModel DailyTimeline
        {
            get
            {
                return this.dailyTimeline;
            }
            set
            {
                this.dailyTimeline = value;
                this.OnPropertyChanged(() => this.DailyTimeline);
            }
        }
    }
}
