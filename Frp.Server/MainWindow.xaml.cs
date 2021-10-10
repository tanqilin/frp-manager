using Frp.NetWork;
using Frp.NetWork.FrpService;
using Frp.NetWork.Model;
using Frp.NetWork.Pulugins;
using Frp.Server.ViewModel;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region 属性和构造函数
        private MainViewModel _mainViewModel;
        private readonly IFrpFileService _frpFileService; 
        public MainWindow()
        {
            InitializeComponent();
            _frpFileService = new FrpFileService();
            _mainViewModel = new MainViewModel();

            this.Closing += MainWindow_Closing;

            DataContext = _mainViewModel;
            VerifyFrpVersion();
        }
        #endregion

        #region 一些初始化方法
        private void VerifyFrpVersion()
        {
            LoadFrpsConfig();
        }

        /// <summary>
        /// 读取Frps配置信息
        /// </summary>
        private void LoadFrpsConfig()
        {
            _mainViewModel.Config = new FrpsConfig
            {
                startConfig = ReadConfigInfo(new StartConfig()),
                authenConfig = ReadConfigInfo(new AuthenConfig()),
                adminConfig = ReadConfigInfo(new AdminConfig()),
                dashboardConfig = ReadConfigInfo(new DashboardConfig()),
                httpConfig = ReadConfigInfo(new HttpConfig()),
            };
        }
        #endregion

        #region 保存 / 读写配置信息
        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveFrpsConfig_btn(object sender, RoutedEventArgs e)
        {
            var config = _mainViewModel.Config;
            Dictionary<string, object> dic = FormatConfigInfo(config.startConfig);
            IniConfig.WriteIniInfo("common", dic);

            /// 认证信息
            if (config.authenConfig.enabled)
            {
                dic = FormatConfigInfo(config.authenConfig);
                IniConfig.WriteIniInfo("common", dic);
            }
            else
            {
                ClearConfigInfo(config.authenConfig);
            }

            /// 管理信息
            if (config.adminConfig.enabled)
            {
                dic = FormatConfigInfo(config.adminConfig);
                IniConfig.WriteIniInfo("common", dic);
            }
            else
            {
                ClearConfigInfo(config.adminConfig);
            }

            /// Web 管理信息
            if (config.dashboardConfig.enabled)
            {
                dic = FormatConfigInfo(config.dashboardConfig);
                IniConfig.WriteIniInfo("common", dic);
            }
            else
            {
                ClearConfigInfo(config.dashboardConfig);
            }

            /// HTTP & HTTPS
            if (config.httpConfig.enabled)
            {
                dic = FormatConfigInfo(config.httpConfig);
                IniConfig.WriteIniInfo("common", dic);
            }
            else
            {
                ClearConfigInfo(config.httpConfig);
            }

            Growl.Success("保存成功");
        }

        /// <summary>
        /// 格式化配置信息
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        private Dictionary<string, object> FormatConfigInfo<T>(T conf)
        {
            Dictionary<string, object> init = new Dictionary<string, object>();

            Type start = conf.GetType();
            foreach(var field in start.GetProperties())
            {
                var isString = typeof(string).IsAssignableFrom(field.PropertyType);
                var isInt = typeof(int).IsAssignableFrom(field.PropertyType);
                var isBool = typeof(bool).IsAssignableFrom(field.PropertyType);
                /// 判断查找string不为空，int不为0的字段
                if ((isString && !string.IsNullOrEmpty((string)field.GetValue(conf))) 
                    || (isInt && (int)field.GetValue(conf) != 0) 
                    || isBool)
                {
                    /// 找到特性，查看值是否为默认值
                    object val = field.GetValue(conf);
                    if (field.IsDefined(typeof(DefaultValueAttribute), true))
                    {
                        DefaultValueAttribute attrs = (DefaultValueAttribute)field.GetCustomAttributes(typeof(DefaultValueAttribute),true)[0];
                        if (!attrs.Value.Equals(val))
                        {
                            /// 如果不是默认值，则写入配置文件
                            init.Add(field.Name, val);
                        }
                        else
                        {
                            /// 如果是默认值，则删除配置项
                            init.Add(field.Name, null);
                        }
                    }
                    else
                    {
                        init.Add(field.Name, val);
                    }
                }
            }

            return init;
        }

        /// <summary>
        /// 清除节点配置信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conf"></param>
        private void ClearConfigInfo<T>(T conf)
        {
            Dictionary<string, object> init = new Dictionary<string, object>();

            Type start = conf.GetType();
            foreach (var field in start.GetProperties())
            {
                init.Add(field.Name, null);
            }

            /// 调用清楚接口
            IniConfig.WriteIniInfo("common", init);
        }

        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <typeparam name="T">读取对象</typeparam>
        /// <param name="conf"></param>
        private T ReadConfigInfo<T>(T conf)
        {
            var t = typeof(T);
            Type start = conf.GetType();
            foreach (var field in start.GetProperties())
            {
                var res = IniConfig.ReadIniInfo("common", field.Name);
                if (!string.IsNullOrEmpty(res))
                {
                    /// 查找enabled节点并赋值，走到这里表示配置文件中此模块是启用的
                    var enable = start.GetProperties().Where(e => e.Name == "enabled").FirstOrDefault();
                    if (enable != null) enable.SetValue(conf, true);

                    /// 类型转换
                    Type ty = field.PropertyType;
                    if (ty.Equals(typeof(string)))
                    {
                        field.SetValue(conf, res);
                    }
                    if (ty.Equals(typeof(int)))
                    {
                        field.SetValue(conf, int.Parse(res));
                    }
                    if (ty.Equals(typeof(bool)))
                    {
                        field.SetValue(conf, bool.Parse(res));
                    }
                }
            }
            return conf;
        }
        #endregion

        #region 窗口关闭事件
        /// <summary>
        /// 窗口关闭中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var res =  HandyControl.Controls.MessageBox.Show("是否允许程序在后台运行？", "退出程序", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(res == MessageBoxResult.Yes)
            {
                e.Cancel = true;
                this.Hide();
                WindowsTaskbarIcon._icon = Resource.songsu;
                WindowsTaskbarIcon.exitTaskRun = killProcess;
                WindowsTaskbarIcon.Open();
            }
            else
            {
                killProcess(sender,null);
            }
        }

        private void killProcess(object sender, RoutedEventArgs e)
        {
            /// 关闭程序时杀掉frps的后台
            foreach (var item in Process.GetProcesses())
            {
                if (item.ProcessName.ToLower().Contains("frps"))
                {
                    item.Kill();
                    break;
                }
            }
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

        #region 停止frps服务
        /// <summary>
        /// 停止frps服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopFrpsServer_btn(object sender, RoutedEventArgs e)
        {
            var res = HandyControl.Controls.MessageBox.Show("确定要停止frps服务吗？","停止frps服务",MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(res == MessageBoxResult.Yes)
            {
                killProcess(sender,e);
            }
        }
        #endregion
    }
}