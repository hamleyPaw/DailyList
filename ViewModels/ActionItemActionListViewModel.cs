using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HamleyPaw.DailyList.Models;

namespace HamleyPaw.DailyList.ViewModels
{
    public class ActionItemActionListViewModel : ViewModelBase
    {
        private ObservableCollection<ActionItemActionViewModel> itemsToDisplay =
            new ObservableCollection<ActionItemActionViewModel>();

        public ActionItemActionListViewModel()
        {
            // Nothing at present
        }

        // Responsibilities for this VM
        // Dsiplay and admin of a given list of Action Item Actions.
        // Admin will be:
            // Changing the display template (e.g. List, Timeline)

        public ObservableCollection<ActionItemActionViewModel> ItemsToDisplay
        {
            get
            {
                return this.itemsToDisplay;
            }
            set
            {
                this.itemsToDisplay = value;
                this.OnPropertyChanged(() => this.ItemsToDisplay);
            }
        }

        public void AddActionToTimeLine(ActionItemAction actionToAdd)
        {
            this.ItemsToDisplay.Add(new ActionItemActionViewModel(actionToAdd));
        }

        // Commands
        // ??

        // Parse the list of items...
        // Place in chronological order
        // Highlight those actions that come from the same Item

        // Upon adding a new item the list will be parsed again.

    }
}
