using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace Frp.NetWork
{
    /// <summary>
    /// 网络请求工具类
    /// </summary>
    public class HttpNetwork
    {
        #region 发送GET请求
        /// <summary>
        /// 发送GET请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dic">GET请求参数</param>
        /// <param name="auth">认证信息</param>
        /// <returns>请求结果</returns>
        public static string Get(string url, Dictionary<string, string> dic, string auth = null)
        {
            string result = "";
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dic?.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in dic)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, System.Web.HttpUtility.UrlEncode(item.Value));
                    i++;
                }
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
            // 设置HTTP头Http Basic认证
            if (!string.IsNullOrEmpty(auth))
                req.Headers.Add("Authorization", "Basic " + auth);

            //添加参数
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                /// 如果以gzip加密的方式则需要先解压
                if (resp.ContentEncoding != null && resp.ContentEncoding.ToLower().Equals("gzip"))
                    stream = new GZipStream(stream, CompressionMode.Decompress);

                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            return result;
        }
        #endregion

        #region 发送POST请求
        /// <summary>
        /// 发送POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dic">POST参数</param>
        /// <returns>请求结果</returns>
        public static string Post(string url, Dictionary<string, string> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            #region 添加Post 参数
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        #endregion

        #region 发送DELETE请求
        /// <summary>
        /// 发送DELETE请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dic">DELETE请求参数</param>
        /// <returns></returns>
        public static string Delete(string url, string auth = null)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "DELETE";
            // 设置HTTP头Http Basic认证
            if (!string.IsNullOrEmpty(auth))
                req.Headers.Add("Authorization", "Basic " + auth);

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        #endregion
    }
}
