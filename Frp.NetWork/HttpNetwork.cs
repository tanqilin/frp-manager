using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace Frp.NetWork
{
    /// <summary>
    /// �������󹤾���
    /// </summary>
    public class HttpNetwork
    {
        #region ����GET����
        /// <summary>
        /// ����GET����
        /// </summary>
        /// <param name="url">�����ַ</param>
        /// <param name="dic">GET�������</param>
        /// <param name="auth">��֤��Ϣ</param>
        /// <returns>������</returns>
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
            // ����HTTPͷHttp Basic��֤
            if (!string.IsNullOrEmpty(auth))
                req.Headers.Add("Authorization", "Basic " + auth);

            //��Ӳ���
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                /// �����gzip���ܵķ�ʽ����Ҫ�Ƚ�ѹ
                if (resp.ContentEncoding != null && resp.ContentEncoding.ToLower().Equals("gzip"))
                    stream = new GZipStream(stream, CompressionMode.Decompress);

                //��ȡ����
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

        #region ����POST����
        /// <summary>
        /// ����POST����
        /// </summary>
        /// <param name="url">�����ַ</param>
        /// <param name="dic">POST����</param>
        /// <returns>������</returns>
        public static string Post(string url, Dictionary<string, string> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            #region ���Post ����
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
            //��ȡ��Ӧ����
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        #endregion

        #region ����DELETE����
        /// <summary>
        /// ����DELETE����
        /// </summary>
        /// <param name="url">�����ַ</param>
        /// <param name="dic">DELETE�������</param>
        /// <returns></returns>
        public static string Delete(string url, string auth = null)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "DELETE";
            // ����HTTPͷHttp Basic��֤
            if (!string.IsNullOrEmpty(auth))
                req.Headers.Add("Authorization", "Basic " + auth);

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //��ȡ��Ӧ����
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        #endregion
    }
}
