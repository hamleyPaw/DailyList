using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;

using HamleyPaw.DailyList.Views;
using HamleyPaw.DailyList.ViewModels;
using HamleyPaw.DailyList.ViewModels.Contexts;

namespace HamleyPaw.DailyList.Contexts {
    public class UserInterfaceContext : IUserInterfaceContext {
        private Stack<Window> windowStack = new Stack<Window>();

        public UserInterfaceContext(Window mainWindow) {
            windowStack.Push(mainWindow);
        }

        #region IUserInterfaceContext Members

        void IUserInterfaceContext.ShowMessage(string title, string message, Action closeAction) {
            var result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

            if (result == MessageBoxResult.OK && closeAction != null) {
                closeAction();
            }
        }

        void IUserInterfaceContext.GetText(string title, string message, Action<string> closeAction) {

        }

        #endregion

        private void ShowDialog(Window dialog, bool subDialog = false)
        {
            dialog.Closed += (sender, args) =>
            {
                windowStack.Pop();
            };

            if(subDialog && windowStack.Count > 0)
            {
                dialog.Owner = windowStack.Peek();
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                var mask = new SolidColorBrush(Colors.White);
                mask.Opacity = 0.5;

                windowStack.Peek().OpacityMask = mask;
            }

            windowStack.Push(dialog);

            dialog.ShowDialog();

            if(windowStack.Count > 0)
            {
                windowStack.Peek().OpacityMask = null;
            }
        }

        private void ShowDialog(Window dialog, Action<bool> onClosed, bool subDialog = false) {
            dialog.Closed += (sender, args) => {
                windowStack.Pop();

                if (onClosed != null) {
                    onClosed(dialog.DialogResult ?? false);
                }
            };

            if (subDialog && windowStack.Count > 0) {
                dialog.Owner = windowStack.Peek();
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                var mask = new SolidColorBrush(Colors.White);
                mask.Opacity = 0.5;

                windowStack.Peek().OpacityMask = mask;
            }

            windowStack.Push(dialog);

            dialog.ShowDialog();

            if (windowStack.Count > 0) {
                windowStack.Peek().OpacityMask = null;
            }
        }
    }
}
