using Frp.NetWork.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Frp.NetWork.FrpService
{
    /// <summary>
    /// Frp文件管理接口
    /// </summary>
    public class FrpFileService : IFrpFileService
    {
        /// <summary>
        /// 下载Frp文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool FrpDownload(string name)
        {
            return true;
        }

        /// <summary>
        /// 获取Frp版本信息接口实现
        /// </summary>
        /// <returns></returns>
        public LatestInfo GetFrpVersion()
        {
            string url = (string)Application.Current.FindResource("LatestVersion");
            string res = HttpNetwork.Get(url,null,null);

            return null;
        }
    }
}
