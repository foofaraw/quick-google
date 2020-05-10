using System;
using System.Windows;
using System.Windows.Input;

namespace QuickGoogleWpf
{
    public partial class MainWindow : Window
    {
        uint hotKey1;

        public MainWindow()
        {
            InitializeComponent();
            Center();
            InitializeEvents();
            InitializeTrayIcon();
            ClearAndMinimize();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var _hotKeys = new HotKeyHelper(this, HandleHotKey);
            hotKey1 = _hotKeys.ListenForHotKey(System.Windows.Forms.Keys.Space, HotKeyModifiers.Control);
        }

        void HandleHotKey(int keyId)
        {
            if (keyId == hotKey1)
            {
                Activate();
                Show();
                WindowState = WindowState.Normal;
                Center();
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
            base.OnStateChanged(e);
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
                Icon = new System.Drawing.Icon("appicon.ico"),
                Visible = true,
                ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(),
            };

            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (object sender, EventArgs e) => Close());

            notifyIcon.DoubleClick += (object sender, EventArgs e) =>
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

        private void Center()
        {
            Left = (SystemParameters.PrimaryScreenWidth / 2) - (Width / 2);
            Top = (SystemParameters.PrimaryScreenHeight / 2) - (Height / 2);
        }
    }
}
