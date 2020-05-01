using System;
using System.Windows;
using System.Windows.Input;

namespace QuickGoogle
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (object sender, RoutedEventArgs e) => InputTextBox.Focus();
            this.KeyDown += new KeyEventHandler(OnKeyDown);
            this.LostFocus += (object sender, RoutedEventArgs args) => OnLostFocus();
            this.Deactivated += (object sender, EventArgs e) => OnLostFocus();

            var notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = new System.Drawing.Icon("quick-google-icon.ico"),
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
                Hide();
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
                ClearInput();
                WindowToTray();
            }
        }

        private void OnLostFocus()
        {
            WindowToTray();
            ClearInput();
        }

        private void WindowToTray()
            => WindowState = WindowState.Minimized;

        private void ClearInput()
            => InputTextBox.Text = string.Empty;

        private static string GetGoogleSearchLink(string search)
            => $@"https://www.google.com/search?q={search}";
    }
}
