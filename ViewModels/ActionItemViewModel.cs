using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HamleyPaw.DailyList.Models;
using HamleyPaw.DailyList.SupportingTypes;

namespace HamleyPaw.DailyList.ViewModels
{
    public class ActionItemViewModel : ViewModelBase
    {
        private ActionItem itemToDisplay;

        public ActionItemViewModel(ActionItem itemToDisplay)
        {
            this.itemToDisplay = itemToDisplay;
        }

        public event EventHandler<ActionItemActionEventArgs> ActionAdded;

        public string CreatedTime
        {
            get
            {
                return itemToDisplay.CreatedTime.ToString();
            }
        }
        
        public string CompletedTime
        {
            get
            {
                if(this.itemToDisplay.IsComplete)
                {
                    return this.itemToDisplay.CompletedTime.ToString();
                }

                return null;
            }
        }

        public string ActionText
        {
            get
            {
                return itemToDisplay.ItemText;
            }
        }

        public string ElapsedTime
        {
            get
            {
                // Show the total time that this item has been running...
                // i.e. since creation time for isRunning.
                // Calculate this from...
                // 

                return "Elapsed Time will go here";
            }
        }

        public string TotalTime
        {
            get
            {
                // Show the total time that this item has been in existence...
                // i.e. since creation time til Completion
                // Calculate this from...
                    // If the item isCompleted...
                        // Diff Created Time to Completed Time
                    // If the item is not isCompleted...
                        // Diff Created Time and Now.

                return "Elapsed Time will go here";
            }
        }

        public bool CanStartItem
        {
            get
            {
                return !this.itemToDisplay.IsRunning && !this.itemToDisplay.IsComplete;
            }
        }

        public bool CanStopItem
        {
            get
            {
                return this.itemToDisplay.IsRunning;
            }
        }

        public bool CanCompleteItem
        {
            get
            {
                return !this.itemToDisplay.IsComplete;
            }
        }

        public bool IsRunning
        {
            get
            {
                return this.itemToDisplay.IsRunning;
            }
        }

        public bool IsComplete
        {
            get
            {
                return this.itemToDisplay.IsComplete;
            }
        }

        private RelayCommand startItemCommand;
        public ICommand StartItem
        {
            get
            {
                if(startItemCommand == null)
                {
                    startItemCommand = new RelayCommand(param => this.StartThisItem(), param => this.CanStartItem);
                }

                return startItemCommand;
            }
        }

        private RelayCommand stopItemCommand;
        public ICommand StopItem
        {
            get
            {
                if(stopItemCommand == null)
                {
                    stopItemCommand = new RelayCommand(param => this.StopThisItem(), param => this.CanStopItem);
                }

                return stopItemCommand;
            }
        }

        private RelayCommand completeItemCommand;
        public ICommand CompleteItem
        {
            get
            {
                if(completeItemCommand == null)
                {
                    completeItemCommand = new RelayCommand(param => this.CompleteThisItem(), param => this.CanCompleteItem);
                }

                return completeItemCommand;
            }
        }

        private RelayCommand addNoteToItemCommand;
        public ICommand AddNoteToItem
        {
            get
            {
                if(addNoteToItemCommand == null)
                {
                    addNoteToItemCommand = new RelayCommand(param => this.AddNoteToThisItem(param));
                }

                return addNoteToItemCommand;
            }
        }

        private void StartThisItem()
        {
            // Mark this item as IsRunning.
            // Create a new Action Item Action for the start event.
            // Raise event to get the action added to the daily timeline

            this.itemToDisplay.IsRunning = true;

            var actionToRaise = new ActionItemAction();
            actionToRaise.ActionItemActionId = Guid.NewGuid();
            actionToRaise.ActionItemId = itemToDisplay.ActionItemId;
            actionToRaise.Type = ActionItemActionType.Start;
            actionToRaise.ActionTime = NodaTime.SystemClock.Instance.Now;
            actionToRaise.ActionText = this.itemToDisplay.ItemText;

            this.itemToDisplay.Actions.Add(actionToRaise);

            OnActionAdded(actionToRaise);

            this.OnPropertyChanged(() => this.IsRunning);
        }

        private void StopThisItem()
        {
            this.itemToDisplay.IsRunning = false;

            var actionToRaise = new ActionItemAction();
            actionToRaise.ActionItemActionId = Guid.NewGuid();
            actionToRaise.ActionItemId = itemToDisplay.ActionItemId;
            actionToRaise.Type = ActionItemActionType.Stop;
            actionToRaise.ActionTime = NodaTime.SystemClock.Instance.Now;
            actionToRaise.ActionText = "Test_" + actionToRaise.ActionItemActionId.ToString();

            this.itemToDisplay.Actions.Add(actionToRaise);

            OnActionAdded(actionToRaise);

            this.OnPropertyChanged(() => this.IsRunning);
        }

        private void CompleteThisItem()
        {
            this.itemToDisplay.IsRunning = false;
            this.itemToDisplay.IsComplete = true;
            this.itemToDisplay.CompletedTime = NodaTime.SystemClock.Instance.Now;

            var actionToRaise = new ActionItemAction();
            actionToRaise.ActionItemActionId = Guid.NewGuid();
            actionToRaise.ActionItemId = itemToDisplay.ActionItemId;
            actionToRaise.Type = ActionItemActionType.Complete;
            actionToRaise.ActionTime = NodaTime.SystemClock.Instance.Now;
            actionToRaise.ActionText = "Test_" + actionToRaise.ActionItemActionId.ToString();

            this.itemToDisplay.Actions.Add(actionToRaise);

            OnActionAdded(actionToRaise);

            this.OnPropertyChanged(() => this.IsRunning);
            this.OnPropertyChanged(() => this.IsComplete);
        }

        private void AddNoteToThisItem(object noteText)
        {
            var actionToRaise = new ActionItemAction();
            actionToRaise.ActionItemActionId = Guid.NewGuid();
            actionToRaise.ActionItemId = itemToDisplay.ActionItemId;
            actionToRaise.Type = ActionItemActionType.Note;
            actionToRaise.ActionTime = NodaTime.SystemClock.Instance.Now;
            actionToRaise.ActionText = (string)noteText;

            this.itemToDisplay.Actions.Add(actionToRaise);

            OnActionAdded(actionToRaise);
        }

        protected void OnActionAdded(ActionItemAction actionRaised)
        {
            if(this.ActionAdded != null)
            {
                this.ActionAdded(this, new ActionItemActionEventArgs(actionRaised));
            }
        }
    }
}
