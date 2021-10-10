using Frp.NetWork.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frp.NetWork.FrpService
{
    /// <summary>
    /// Frp文件相关的管理接口
    /// </summary>
    public interface IFrpFileService
    {
        /// <summary>
        /// 获取FRP版本信息
        /// </summary>
        /// <returns></returns>
        LatestInfo GetFrpVersion();

        /// <summary>
        /// 下载Frp文件
        /// </summary>
        /// <returns></returns>
        bool FrpDownload(string name);
    }
}
