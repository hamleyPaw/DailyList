using HamleyPaw.DailyList.ViewModels.Contexts;
using System.Windows;

namespace HamleyPaw.DailyList.Contexts
{
    public class ViewModelContext : IViewModelContext
    {
        public ViewModelContext(Window mainWindow)
        {
            _UserInterface = new UserInterfaceContext(mainWindow);
        }

        // Rename this as Interaction context

        private readonly IUserInterfaceContext _UserInterface;

        public IUserInterfaceContext UserInterface
        {
            get { return _UserInterface; }
        }
    }
}
