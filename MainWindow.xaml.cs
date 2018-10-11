using System.Windows;
using System;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using Microsoft.Win32;

namespace clipppBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(
            [In] IntPtr hWnd,
            [In] int id,
            [In] uint fsModifiers,
            [In] uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);


        private HwndSource _source;
        private const int HOTKEY_ID = 9000;
        private const int HOTKEY2_ID = 8000;
        private const int HOTKEY3_ID = 7000;

        //main window
        public MainWindow()
        {

            InitializeComponent();
            
        }

        
        //register keys on start
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var helper = new WindowInteropHelper(this);
            _source = HwndSource.FromHwnd(helper.Handle);
            _source.AddHook(HwndHook);
            RegisterHotKey();
        }

        //unregister keys on closing the window
        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            _source = null;
            UnregisterHotKey();
            base.OnClosed(e);
        }

        private void RegisterHotKey()
        {
            var helper = new WindowInteropHelper(this);            
            if (!RegisterHotKey(helper.Handle, HOTKEY_ID, (uint)ModifierKeys.Control | (uint)ModifierKeys.Shift, 67 /*67 is the character c in the keyboard*/))
            {
                // handle error
            }
            if (!RegisterHotKey(helper.Handle, HOTKEY2_ID, (uint)ModifierKeys.Control | (uint)ModifierKeys.Shift, 86 /*86 is the character v in the keyboard*/))
            {
                // handle error
            }
            if (!RegisterHotKey(helper.Handle, HOTKEY3_ID, (uint)ModifierKeys.Control | (uint)ModifierKeys.Shift, 88 /*88 is the character v in the keyboard*/))
            {
                // handle error
            }

        }

        private void UnregisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            UnregisterHotKey(helper.Handle, HOTKEY_ID);
            UnregisterHotKey(helper.Handle, HOTKEY2_ID);
            UnregisterHotKey(helper.Handle, HOTKEY3_ID);

        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            OnHotKey1Pressed();
                            handled = true;
                            break;
                        case HOTKEY2_ID:
                            OnHotKey2Pressed();
                            handled = true;
                            break;
                        case HOTKEY3_ID:
                            OnHotKey3Pressed();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        //ctrl + shift + c
        private void OnHotKey1Pressed()
        {
            copiedResult.AppendText( Clipboard.GetText());
            copiedResult.AppendText(Environment.NewLine);
        }

        // ctrl + shift + v
        private void OnHotKey2Pressed()
        {
            Clipboard.SetText(copiedResult.Text);
        }

        //ctrl + shift + x
        private void OnHotKey3Pressed()
        {
            copiedResult.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text file (*.txt)|*.txt";
            if (dialog.ShowDialog() == true)
            {
            StreamWriter sr = new StreamWriter(dialog.FileName);
            sr.Write(copiedResult.Text);
            sr.Flush();

            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(copiedResult.Text);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            copiedResult.Text = "";
        }
    }
}
