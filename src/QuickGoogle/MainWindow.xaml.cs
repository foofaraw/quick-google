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
            InitializeEvents();
            InitializeTrayIcon();
            ClearAndMinimize();
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

        private void InitializeEvents()
        {
            Loaded += (object sender, RoutedEventArgs e) => InputTextBox.Focus();
            KeyDown += new KeyEventHandler(OnKeyDown);
            LostFocus += new RoutedEventHandler(OnLostFocus);
            Deactivated += new EventHandler(OnLostFocus);
        }

        private void InitializeTrayIcon()
        {
            var notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = new System.Drawing.Icon("quick-google-icon.ico"),
                Visible = true
            };

            notifyIcon.DoubleClick += (object sender, EventArgs args) =>
            {
                Show();
                WindowState = WindowState.Normal;
            };
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                try
                {
                    if (RunSearch(InputTextBox.Text))
                    {
                        ClearAndMinimize();
                    }
                }
                catch (Exception ex)
                {
                    InputTextBox.Text = ex.Message;
                }
            }
            else if (e.Key == Key.Escape)
            {
                ClearAndMinimize();
            }
        }

        private void OnLostFocus<T>(object sender, T e) where T : EventArgs
        {
            ClearAndMinimize();
        }

        private bool RunSearch(string input)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(input))
            {
                System.Diagnostics.Process.Start($@"https://www.google.com/search?q={input}");
                result = true;
            }
            return result;
        }

        private void ClearAndMinimize()
        {
            InputTextBox.Text = string.Empty;
            WindowState = WindowState.Minimized;
        }
    }
}
