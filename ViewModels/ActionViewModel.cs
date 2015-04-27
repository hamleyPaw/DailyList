using System;
using System.IO.Packaging;
using HamleyPaw.DailyList.Models;
using HamleyPaw.DailyList.ViewModels.Contexts;

namespace HamleyPaw.DailyList.ViewModels
{
    public class ActionViewModel : ViewModelBase
    {
        private readonly DailyListItemAction _ItemToDisplay = null;

        public ActionViewModel(DailyListItemAction itemToDisplay, IViewModelContext context)
            : base(context)
        {
            _ItemToDisplay = itemToDisplay;
        }

        public string ItemText
        {
            get { return _ItemToDisplay.DailyListItem.ItemText ?? null; }
        }

        public DateTime ActionDateTimeUtc
        {
            get { return _ItemToDisplay.ActionDateTimeUtc; }
        }

        public string ActionTime
        {
            get
            {
                return _ItemToDisplay.Time.ToString("HH:mm (dd/MM/YYYY)", null);
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
