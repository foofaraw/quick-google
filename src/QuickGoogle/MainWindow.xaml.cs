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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += OnWindowLoaded;
            this.KeyDown += new KeyEventHandler(OnKeyDown);
            this.LostFocus += OnLostFocus;

            var notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = new System.Drawing.Icon("TrayIcon.ico"),
                Visible = true
            };
            notifyIcon.DoubleClick += OnTrayIconDoubleClick;

            WindowState = WindowState.Minimized;
        }

        #region OVERRIDE_EVENTS
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            NativeMethods.RegisterHotKey(this);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
            base.OnStateChanged(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            NativeMethods.UnregisterHotKey();
            base.OnClosed(e);
        }
        #endregion

        #region EVENT_HANDLERS
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            InputTextBox.Focus();
        }

        private void OnLostFocus(object sender, RoutedEventArgs args)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnTrayIconDoubleClick(object sender, EventArgs args)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;

                case Key.Enter:
                    e.Handled = true;
                    HandleSearch();
                    break;
            }
        }
        #endregion

        #region PRIVATE_METHODS
        private void HandleSearch()
        {
            string input = GetUserInput();
            if (!string.IsNullOrWhiteSpace(input))
            {
                System.Diagnostics.Process.Start(GetGoogleSearchLink(input));
                ClearUserInput();
                // TODO: Close to tray
            }
        }

        private string GetUserInput()
            => InputTextBox.Text;

        private void ClearUserInput()
            => InputTextBox.Text = string.Empty;
        #endregion

        #region PRIVATE_STATIC_METHODS
        private static string GetGoogleSearchLink(string search)
            => $@"http://www.google.com/search?q={search}";
        #endregion
    }
}
