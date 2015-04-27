using System.Data.Entity;
using HamleyPaw.DailyList.Models;
using HamleyPaw.DailyList.SupportingTypes;
using NodaTime;
using System;
using System.Linq;
using System.Windows.Input;
using HamleyPaw.DailyList.ViewModels.Contexts;

namespace HamleyPaw.DailyList.ViewModels
{
    public class DailyListItemViewModel : ViewModelBase
    {
        private readonly DailyListItem _ItemToDisplay;

        public DailyListItemViewModel(DailyListItem itemToDisplay, IViewModelContext context)
            : base(context)
        {
            _ItemToDisplay = itemToDisplay;
        }

        public event EventHandler<ItemActionEventArgs> ActionAdded;

        public DateTime CreatedDateTimeUtc
        {
            get { return _ItemToDisplay.CreatedDateTimeUtc; }
        }

        public string CreatedTime
        {
            get
            {
                return "Created: " + _ItemToDisplay
                    .CreatedTime
                    .ToString("HH:mm (dd/MM/YYYY)", null);
            }
        }

        public string CompletedTime
        {
            get
            {
                string completedTime = null;

                if(_ItemToDisplay.IsComplete)
                {
                    completedTime = "Completed: " + _ItemToDisplay
                        .CompletedTime
                        .ToString("HH:mm (dd/MM/YYYY)", null);
                }

                return completedTime;
            }
        }

        public string ItemText
        {
            get
            {
                return _ItemToDisplay.ItemText;
            }
        }

        public string TotalTime
        {
            get
            {
                string timeToDisplay = null;

                if(_ItemToDisplay.IsComplete)
                {
                    // Show the total time that this item has been in existence...
                    //        // i.e. since creation time til Completion
                    //        // Calculate this from...
                    //            // If the item isCompleted...
                    //                // Diff Created Time to Completed Time
                    //            // If the item is not isCompleted...
                    //                // Diff Created Time and Now.

                    var totalDuration = _ItemToDisplay.CompletedTime.Minus(_ItemToDisplay.CreatedTime);

                    timeToDisplay = "Total Time: " + totalDuration.ToString();
                }

                return timeToDisplay;
            }
        }

        public string RunningTime
        {
            get
            {
                string timeToDisplay = null;

                // Display a different time value depending on the current status...
                // Is Running == Running Since...
                // Is Stopped == Elapsed time...
                // Is Completed == Total Time...

                if(_ItemToDisplay.IsRunning)
                {
                    // When was the last time we started this task...
                    var dailyListItemAction = _ItemToDisplay.Actions
                        .OrderByDescending(i => i.Time)
                        .FirstOrDefault(i => i.Type == DailyListItemActionType.Start);

                    if (dailyListItemAction != null)
                    {
                        var lastStartTime = dailyListItemAction.Time;

                        timeToDisplay = "Running Since: " + lastStartTime.ToString("HH:mm (dd/MM/YYYY)", null);
                    }
                }
                else if(!_ItemToDisplay.IsRunning)
                {
                    if(_ItemToDisplay.Actions.Count() > 1)
                    {
                        // Show the total time that this item has been running...
                        //        // i.e. since creation time for isRunning.
                        //        // Calculate this from...
                        //        // loop the actions and note time elapsed
                        // between starts and stops, ignore time inbetween.

                        var runTime = new Duration();
                        DailyListItemAction lastRelevantAction = null;

                        _ItemToDisplay.Actions
                            .OrderBy(i => i.Time)
                            .ToList()
                            .ForEach(i => {
                                switch(i.Type)
                                {
                                    case DailyListItemActionType.Start:
                                        if(lastRelevantAction != null)
                                        {
                                            // If we are a start and last relevant action was a stop
                                            // then we're off and running again...
                                            if(lastRelevantAction.Type == DailyListItemActionType.Stop)
                                            {
                                                lastRelevantAction = i;
                                            }

                                            // TODO If the last relevant action is Start or Complete
                                            // then we should probably throw an exception.
                                        }
                                        else
                                        {
                                            // We're off and running...
                                            lastRelevantAction = i;
                                        }
                                        break;
                                    case DailyListItemActionType.Stop:
                                        if(lastRelevantAction != null)
                                        {
                                            // If we are a stop and last relevant action was a start
                                            // then we need to log a duration...
                                            if(lastRelevantAction.Type == DailyListItemActionType.Start)
                                            {
                                                runTime += i.Time.Minus(lastRelevantAction.Time);

                                                lastRelevantAction = i;
                                            }

                                            // TODO If the last relevant action is Stop or Complete
                                            // then we should probably throw an exception.
                                        }
                                        else
                                        {
                                            // TODO we should probably flag this situation as an exception
                                        }
                                        break;
                                    case DailyListItemActionType.Complete:
                                        if(lastRelevantAction != null)
                                        {
                                            // If we are a stop and last relevant action was a start
                                            // then we need to log a duration...
                                            if(lastRelevantAction.Type == DailyListItemActionType.Start)
                                            {
                                                runTime += i.Time.Minus(lastRelevantAction.Time);

                                                lastRelevantAction = i;
                                            }

                                            // If the last relevant action is Stop then we will just carry on
                                            // then we should probably throw an exception.
                                        }
                                        else
                                        {
                                            // This is a valid situation as the item could have been completed with
                                            // having been started.
                                        }
                                        break;
                                    case DailyListItemActionType.Note:
                                        // Do Nothing, ken?
                                        break;
                                }
                            });

                        timeToDisplay = "Running Time: " + runTime.ToString();
                    }
                    // Hide this if the item has never been run.
                }

                return timeToDisplay;
            }
        }

        public bool CanStartItem
        {
            get
            {
                return !_ItemToDisplay.IsRunning && !_ItemToDisplay.IsComplete;
            }
        }

        public bool CanStopItem
        {
            get
            {
                return _ItemToDisplay.IsRunning;
            }
        }

        public bool CanCompleteItem
        {
            get
            {
                return !_ItemToDisplay.IsComplete;
            }
        }

        public bool IsRunning
        {
            get
            {
                return _ItemToDisplay.IsRunning;
            }
        }

        public bool IsComplete
        {
            get
            {
                return _ItemToDisplay.IsComplete;
            }
        }

        #region Commands

        private RelayCommand _StartItemCommand;
        public ICommand StartItem
        {
            get {
                return _StartItemCommand ??
                       (_StartItemCommand = new RelayCommand(param => StartThisItem(), param => CanStartItem));
            }
        }

        private RelayCommand _StopItemCommand;
        public ICommand StopItem
        {
            get {
                return _StopItemCommand ??
                       (_StopItemCommand = new RelayCommand(param => StopThisItem(), param => CanStopItem));
            }
        }

        private RelayCommand _CompleteItemCommand;
        public ICommand CompleteItem
        {
            get {
                return _CompleteItemCommand ??
                       (_CompleteItemCommand =
                           new RelayCommand(param => CompleteThisItem(), param => CanCompleteItem));
            }
        }

        private RelayCommand _AddNoteToItemCommand;
        public ICommand AddNoteToItem
        {
            get {
                return _AddNoteToItemCommand ??
                       (_AddNoteToItemCommand = new RelayCommand(param => AddNoteToThisItem()));
            }
        }

        #endregion Commands

        #region Methods

        private DailyListItemAction RaiseNewAction(DailyListItemActionType type, string noteText = null)
        {
            DailyListItemAction actionToRaise = null;

            using (var itemModel = new DailyListContext())
            {
                actionToRaise = itemModel.DailyListItemActions.Create();

                actionToRaise.DailyListItemActionId = Guid.NewGuid();
                actionToRaise.DailyListItemId = _ItemToDisplay.DailyListItemId;
                actionToRaise.Type = type;
                actionToRaise.Time = SystemClock.Instance.Now;
                actionToRaise.Text = noteText;

                itemModel.DailyListItemActions.Add(actionToRaise);
                itemModel.SaveChanges();
            }

            actionToRaise.DailyListItem = _ItemToDisplay;

            return actionToRaise;
        }

        private void StartThisItem()
        {
            // Mark this item as IsRunning.
            // Create a new Action Item Action for the start event.
            // Raise event to get the action added to the daily timeline
            var actionToRaise = RaiseNewAction(DailyListItemActionType.Start);

            using (var itemModel = new DailyListContext())
            {
                _ItemToDisplay.Actions.Add(actionToRaise);
                _ItemToDisplay.IsRunning = true;
                
                itemModel.Entry(_ItemToDisplay).State = EntityState.Modified;
                itemModel.SaveChanges();
            }

            OnActionAdded(actionToRaise);

            OnPropertyChanged(() => IsRunning);
            OnPropertyChanged(() => RunningTime);
        }

        private void StopThisItem()
        {

            var actionToRaise = RaiseNewAction(DailyListItemActionType.Stop);

            using (var itemModel = new DailyListContext())
            {
                _ItemToDisplay.Actions.Add(actionToRaise);
                _ItemToDisplay.IsRunning = false;
                itemModel.Entry(_ItemToDisplay).State = EntityState.Modified;
                itemModel.SaveChanges();
            }

            OnActionAdded(actionToRaise);

            OnPropertyChanged(() => IsRunning);
            OnPropertyChanged(() => RunningTime);
        }

        private void CompleteThisItem()
        {
            var actionToRaise = RaiseNewAction(DailyListItemActionType.Complete);

            using (var itemModel = new DailyListContext())
            {
                _ItemToDisplay.Actions.Add(actionToRaise);
                _ItemToDisplay.IsRunning = false;
                _ItemToDisplay.IsComplete = true;
                _ItemToDisplay.CompletedTime = SystemClock.Instance.Now;
                itemModel.Entry(_ItemToDisplay).State = EntityState.Modified;
                itemModel.SaveChanges();
            }

            OnActionAdded(actionToRaise);

            OnPropertyChanged(() => IsRunning);
            OnPropertyChanged(() => IsComplete);
            OnPropertyChanged(() => RunningTime);
            OnPropertyChanged(() => TotalTime);
            OnPropertyChanged(() => CompletedTime);
        }

        private void AddNoteToThisItem()
        {
            Context.UserInterface.GetNoteText(RaiseNoteAction);
        }

        private void RaiseNoteAction(string noteText)
        {
            var actionToRaise = RaiseNewAction(DailyListItemActionType.Note, (string)noteText);

            _ItemToDisplay.Actions.Add(actionToRaise);

            OnActionAdded(actionToRaise);
        }

        #endregion Methods

        protected void OnActionAdded(DailyListItemAction actionRaised)
        {
            if(ActionAdded != null)
            {
                ActionAdded(this, new ItemActionEventArgs(actionRaised.DailyListItemActionId));
            }
        }
    }
}
