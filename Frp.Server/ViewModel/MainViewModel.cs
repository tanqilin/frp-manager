using Frp.NetWork.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Frp.Server.ViewModel
{
    /// <summary>
    /// MainWindow 视图模型
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        #region 属性和构造函数
        private FrpsConfig _frpsConfig;
        public event PropertyChangedEventHandler PropertyChanged;
        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
            => args => PropertyChanged?.Invoke(this, args);
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        public MainViewModel()
        {
            _frpsConfig = new FrpsConfig();
        }
        #endregion

        public FrpsConfig Config { get => _frpsConfig; set => SetProperty(ref _frpsConfig, value); }
    }
}
