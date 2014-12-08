using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HamleyPaw.DailyList.ViewModels.Contexts {
    public interface IUserInterfaceContext {
        void ShowMessage(string title, string message, Action closeAction);
        void GetText(string title, string message, Action<string> closeAction);
    }
}
