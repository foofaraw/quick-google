﻿using QuickGoogle.HotkeyRegistration;
using System;
using System.Windows;
using System.Windows.Input;

namespace QuickGoogleWpf
{
    public partial class MainWindow : Window
    {
        private uint _hotKey;

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
            var _hotKeys = new HotKeyHelper(this, OnHotKeyDown);
            _hotKey = _hotKeys.ListenForHotKey(System.Windows.Forms.Keys.Space, ModifierKeys.Control);
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
                Visible = true,
                ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(),
            };

            try
            {
                notifyIcon.Icon = new System.Drawing.Icon("appicon.ico");
            }
            catch (Exception)
            {
                notifyIcon.Icon = default;
            }

            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (object sender, EventArgs e) => Close());
            notifyIcon.DoubleClick += (object sender, EventArgs e) =>
            {
                Show();
                WindowState = WindowState.Normal;
            };
        }

        private void OnHotKeyDown(int keyId)
        {
            if (keyId == _hotKey)
            {
                Activate();
                Show();
                WindowState = WindowState.Normal;
                Center();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                try
                {
                    if (SearchHelper.RunSearch(InputTextBox.Text))
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
