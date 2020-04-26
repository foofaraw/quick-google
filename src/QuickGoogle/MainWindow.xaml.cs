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

            this.Loaded += (object sender, RoutedEventArgs e) => InputTextBox.Focus();
            this.KeyDown += new KeyEventHandler(OnKeyDown);
            this.LostFocus += (object sender, RoutedEventArgs args) => WindowToTray();
            this.Deactivated += (object sender, EventArgs e) => WindowToTray();

            var notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = new System.Drawing.Icon("TrayIcon.ico"),
                Visible = true
            };
            notifyIcon.DoubleClick += OnTrayIconDoubleClick;

            WindowState = WindowState.Minimized;
        }

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
                    WindowToTray();
                    break;

                case Key.Enter:
                    e.Handled = true;
                    HandleSearch();
                    break;
            }
        }

        private void HandleSearch()
        {
            string input = InputTextBox.Text;
            if (!string.IsNullOrWhiteSpace(input))
            {
                System.Diagnostics.Process.Start(GetGoogleSearchLink(input));
                InputTextBox.Text = string.Empty;
                WindowToTray();
            }
        }

        private void WindowToTray()
            => WindowState = WindowState.Minimized;

        private static string GetGoogleSearchLink(string search)
            => $@"https://www.google.com/search?q={search}";
    }
}
