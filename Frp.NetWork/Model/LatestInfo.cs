using System;
using System.Collections.Generic;
using System.Text;

namespace Frp.NetWork.Model
{
    /// <summary>
    /// Frp最新版本的版本信息
    /// </summary>
    public class LatestInfo
    {
        /// <summary>
        /// 版本ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Json信息获取接口
        /// </summary>
        public string url {  get; set; }

        /// <summary>
        /// Web详细信息
        /// </summary>
        public string html_url {  get; set; }

        /// <summary>
        /// frp 各个平台上的版本信息
        /// </summary>
        public List<FrpVersion> assets { get; set; }
    }

    /// <summary>
    /// 版本信息
    /// </summary>
    public class FrpVersion
    {
        /// <summary>
        /// 版本ID
        /// </summary>
        public int id {  get; set; }   
        
        /// <summary>
        /// 版本平台名称
        /// </summary>
        public string name {  get; set; }

        /// <summary>
        /// content_type
        /// </summary>
        public string content_type { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int size { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime created_at { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime updated_at {  get; set; }

        /// <summary>
        /// 下载链接
        /// </summary>
        public string browser_download_url { get; set; }
    }
}
