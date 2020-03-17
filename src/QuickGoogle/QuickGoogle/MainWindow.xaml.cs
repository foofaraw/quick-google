// Remaining work
// TODO: Keep the application in tray (start in tray as well)
// TODO: Stylize the textbox

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickGoogle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += WindowLoaded;
            this.KeyDown += new KeyEventHandler(HandleKeyDown);
        }
        
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            NativeMethods.RegisterHotKey(this);
        }

        protected override void OnClosed(EventArgs e)
        {
            NativeMethods.UnregisterHotKey();
            base.OnClosed(e);
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            InputTextBox.Focus();
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;

                case Key.Enter:
                    HandleSearch();
                    break;
            }
        }

        private void HandleSearch()
        {
            string input = GetUserInput();
            if (!string.IsNullOrWhiteSpace(input))
            {
                System.Diagnostics.Process.Start(GetGoogleSearchLink(input));
                // TODO: Close to tray
            }
        }

        private string GetUserInput()
            => InputTextBox.Text;

        private string GetGoogleSearchLink(string search)
            => $@"http://www.google.com/search?q={search}";

        private void ClearUserInput()
            => InputTextBox.Text = string.Empty;
    }
}
