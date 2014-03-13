using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HamleyPaw.DailyList.Models;

namespace HamleyPaw.DailyList.ViewModels
{
    public class ActionItemActionViewModel : ViewModelBase
    {
        private ActionItemAction itemToDisplay = null;

        public ActionItemActionViewModel(ActionItemAction itemToDisplay)
        {
            this.itemToDisplay = itemToDisplay;
        }

        public string ActionTime
        {
            get
            {
                return itemToDisplay.ActionTime.ToString();
            }
        }

        public string ActionType
        {
            get
            {
                return itemToDisplay.Type.GetDescription();
            }
        }

        public string ActionText
        {
            get
            {
                return itemToDisplay.ActionText;
            }
        }

        // Commands
            // Stop
            // Note
            // Pause/Unpause

        // Availability of the commands will be dependent on the
        // Type of item
    }
}
