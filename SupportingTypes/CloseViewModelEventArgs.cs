using System;

namespace HamleyPaw.DailyList.SupportingTypes
{
    public class CloseViewModelEventArgs
    {
        private readonly bool _Success;
        public bool Success
        {
            get
            {
                return _Success;
            }
        }

        public CloseViewModelEventArgs(bool success)
        {
            _Success = success;
        }
    }
}
