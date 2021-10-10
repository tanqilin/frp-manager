using Frp.Server.ViewModel;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Frp.Server
{
    /// <summary>
    /// FrpsManagerUserController.xaml 的交互逻辑
    /// </summary>
    public partial class FrpsManagerUserController : UserControl
    {
        #region 属性和构造函数
        private static Process _myprocess;
        private readonly FrpsManagerViewModel _managerViewModel;
        public FrpsManagerUserController()
        {
            InitializeComponent();
            _myprocess = new Process();
            _managerViewModel = new FrpsManagerViewModel();
            DataContext = _managerViewModel;

            LoadConfigInfo(); 
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _managerViewModel.FrpsIcon = ByteArrayToBitmapImage(Resource.woniu);
        }
        #endregion

        #region 加载Frps配置信息
        /// <summary>
        /// 加载Frps配置信息
        /// </summary>
        private void LoadConfigInfo()
        {
            Task.Run(() =>
            {
                using (StreamReader sr = new StreamReader("./Frps/frps.ini"))
                {
                    string line;
                    string result = "";
                    // 从文件读取并显示行，直到文件的末尾
                    while ((line = sr.ReadLine()) != null)
                    {
                        result += line + "\n";
                    }
                    _managerViewModel.FrpsOutputInfo = result;
                }
            });
        }
        #endregion

        #region 切换frps运行状态
        /// <summary>
        /// 切换frps运行状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusBtn_Click(object sender, RoutedEventArgs e)
        {
            StartFrps();
        }
        
        /// <summary>
        /// 启动frps服务
        /// </summary>
        private async void StartFrps()
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(_myprocess.StartInfo.FileName))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "./Frps/frps",
                        Arguments = " -c ./Frps/frps.ini",
                        UseShellExecute = false,
                        RedirectStandardInput = true,//接受来自调用程序的输入信息
                        RedirectStandardOutput = true,//由调用程序获取输出信息
                        RedirectStandardError = true,//重定向标准错误输出
                        CreateNoWindow = true//不显示程序窗口
                    };
                    _myprocess.StartInfo = startInfo;
                }
                _myprocess.Start();
                //获取cmd窗口的输出信息
                string output = _myprocess.StandardOutput.ReadToEnd();

                Dispatcher.Invoke(() =>
                {
                    Growl.Success("启动成功");
                    _managerViewModel.FrpsIcon = ByteArrayToBitmapImage(Resource.songsu);
                    LoadConfigInfo();
                });               
            });
        }
        #endregion

        #region byte[]转ImageSource
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
        #endregion
    }
}
