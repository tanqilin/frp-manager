using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;

namespace Frp.Server.ViewModel
{
    /// <summary>
    /// FrpsManagerUserController 视图模型
    /// </summary>
    public class FrpsManagerViewModel : INotifyPropertyChanged
    {
        #region 属性和构造函数
        private ImageSource _frpsIcon;
        private object _frpsOutputInfo;
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

        public FrpsManagerViewModel()
        {

        }
        #endregion

        public ImageSource FrpsIcon { get => _frpsIcon; set => SetProperty(ref _frpsIcon, value); }


        public object FrpsOutputInfo { get => _frpsOutputInfo; set => SetProperty(ref _frpsOutputInfo, value); }

    }
}
