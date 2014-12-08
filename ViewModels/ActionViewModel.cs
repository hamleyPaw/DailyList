using HamleyPaw.DailyList.Models;

namespace HamleyPaw.DailyList.ViewModels
{
    public class ActionViewModel : ViewModelBase
    {
        private readonly DailyListItemAction _ItemToDisplay = null;

        public ActionViewModel(DailyListItemAction itemToDisplay)
        {
            _ItemToDisplay = itemToDisplay;
        }

        public string ActionTime
        {
            get
            {
                return _ItemToDisplay.Time.ToString();
            }
        }

        public string ActionType
        {
            get
            {
                return _ItemToDisplay.Type.GetDescription();
            }
        }

        public string ActionText
        {
            get
            {
                string textToDisplay = null;
                
                // Parse the Action Text which will depend on the Type
                //  e.g. Note type will have the note text
                // Start will have the time // Or will it?

                switch(_ItemToDisplay.Type)
                {
                    case DailyListItemActionType.Start:
                    case DailyListItemActionType.Stop:
                    case DailyListItemActionType.Complete:
                        // Nothing
                        break;
                    case DailyListItemActionType.Note:
                        textToDisplay = _ItemToDisplay.Text;
                        break;
                    default:
                        break;
                }

                return textToDisplay;
            }
        }
    }
}
