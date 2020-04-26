using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace QuickGoogle
{
    /// <summary>
    /// Unmanaged code to globally register Ctrl + Space hotkey.
    /// </summary>
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID = 9000;
        private const uint MOD_CONTROL = 0x0002; // CTRL
        private const uint MOD_SPACE = 0x20; // SPACEBAR

        private static IntPtr _windowHandle;
        private static HwndSource _source;
        private static Window _window;

        public static void RegisterHotKey(Window window)
        {
            _window = window;
            _windowHandle = new WindowInteropHelper(_window).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL, MOD_SPACE); //CTRL + SPACEBAR
        }

        public static void UnregisterHotKey()
        {
            _source.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
        }

        private static IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                int vkey = (((int)lParam >> 16) & 0xFFFF);
                if (vkey == MOD_SPACE)
                {
                    ActivateWindow();
                }
            }
            return IntPtr.Zero;
        }

        private static void ActivateWindow()
        {
            _window.Activate();
            _window.Show();
            _window.WindowState = WindowState.Normal;
        }
    }
}
