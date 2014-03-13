using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using HamleyPaw.DailyList.ViewModels;
using HamleyPaw.DailyList.Contexts;
using HamleyPaw.DailyList.Views;

namespace HamleyPaw.DailyList {
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            MainView window = new MainView();

            // Create the ViewModel to which the main window binds.
            var viewModel = new MainViewModel(new ViewModelContext(window));

            // When the ViewModel asks to be closed, 
            // close the window.
            EventHandler handler = null;
            handler = delegate {
                viewModel.RequestClose -= handler;
                window.Close();
            };
            viewModel.RequestClose += handler;

            // Allow all controls in the window to 
            // bind to the ViewModel by setting the 
            // DataContext, which propagates down 
            // the element tree.
            window.DataContext = viewModel;

            window.Show();
        }
    }
}
