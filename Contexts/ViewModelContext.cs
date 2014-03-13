using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using HamleyPaw.DailyList.ViewModels.Contexts;

namespace HamleyPaw.DailyList.Contexts {
    public class ViewModelContext : IViewModelContext {

        // This should not pass a Window, pass in the UserContext
        // Is it testable with a window?

        // This should also include a service that will deal
        // with interaction with the repository

        public ViewModelContext(Window mainWindow) {
            this.userInterface = new UserInterfaceContext(mainWindow);
        }

        // Rename this as Interaction context

        private IUserInterfaceContext userInterface;
        public IUserInterfaceContext UserInterface {
            get { return userInterface; }
        }
    }
}
