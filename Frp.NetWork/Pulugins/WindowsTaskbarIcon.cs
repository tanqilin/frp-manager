using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Frp.NetWork.Pulugins
{
    public  class WindowsTaskbarIcon
    {
        static TaskbarIcon WindowsNotifyIcon { get; set; }
        public static byte[] _icon;
        public static RoutedEventHandler exitTaskRun;
        public static void Open()
        {
            if (WindowsNotifyIcon is null)
            {
                InitNotifyIcon();
            }
        }

        public static void Exit()
        {
            if (WindowsNotifyIcon is null) return;
            WindowsNotifyIcon.Visibility = System.Windows.Visibility.Collapsed;
            WindowsNotifyIcon.Dispose();
        }
        ///初始化托盘控件
        static void InitNotifyIcon()
        {
            WindowsNotifyIcon = new TaskbarIcon();
            WindowsNotifyIcon.Icon = Icon.FromHandle(new System.Drawing.Bitmap(new MemoryStream(_icon)).GetHicon()); 
            ContextMenu context = new ContextMenu();
            MenuItem show = new MenuItem();
            show.Header = "主页";
            show.Click += delegate (object sender, RoutedEventArgs e)
            {
                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.Topmost = true;
                Application.Current.MainWindow.Topmost = false;
            };
            context.Items.Add(show);

            MenuItem exit = new MenuItem();
            exit.Header = "退出";
            // exit.Icon = Icon.FromHandle(Resource.logout.GetHicon());
            exit.Click += exitTaskRun;
            exit.Click += delegate (object sender, RoutedEventArgs e)
            {
                Environment.Exit(0);
            };
            context.Items.Add(exit);

            WindowsNotifyIcon.ContextMenu = context;
        }

        /// <summary>
        /// byte[]转ImageSource
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            BitmapImage bmp = null;
            try
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(byteArray);
                bmp.EndInit();
            }
            catch
            {
                bmp = null;
            }
            return bmp;
        }
    }
}
