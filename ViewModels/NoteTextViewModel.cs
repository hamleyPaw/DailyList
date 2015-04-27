using HamleyPaw.DailyList.Models;
using HamleyPaw.DailyList.SupportingTypes;
using HamleyPaw.DailyList.ViewModels.Contexts;
using NodaTime;
using System.Linq;
using System.Windows.Input;

namespace HamleyPaw.DailyList.ViewModels
{
    public class NoteTextViewModel : ViewModelBase
    {
        
        private string _NoteText;

        public NoteTextViewModel()
        {
            // Do Nothing
        }

        public string NoteText
        {
            get
            {
                return _NoteText;
            }
            set
            {
                _NoteText = value;
                OnPropertyChanged(() => NoteText);
                OnPropertyChanged(() => CanProceed);
            }
        }

        public bool CanProceed
        {
            get { return !string.IsNullOrWhiteSpace(NoteText); }
        }

        #region Commands

        private RelayCommand _CancelCommand;
        private RelayCommand _AcceptCommand;

        public ICommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(param => OnRequestClose(false))); }
        }

        public ICommand AcceptCommand
        {
            get { return _AcceptCommand ?? (
                _AcceptCommand = new RelayCommand(
                    param => OnRequestClose(true),
                    param => CanProceed));
            }
        }

        #endregion Commands

    }
}
