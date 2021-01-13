using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliUp.daima
{
    public static class gongju
    {
        /// <summary>
        /// 拆分cookie
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string[] huoqu_xinxi(string url)
        {
            string[] jieguo = new string[5];
            string linshi = "";
            bool shifou_fuzhi = false;
            int jishu = 0;
            for(int i=0;i<url.Length;i++)
            {
                if(url[i]=='=')
                {            
                    shifou_fuzhi = true;
                    linshi = "";
                    continue;
                }
                if(url[i]=='&')
                {
                    jieguo[jishu] = linshi;
                    jishu++;
                    shifou_fuzhi = false;
                    continue;
                }
                if(shifou_fuzhi==true)
                {
                    linshi += url[i];
                }
            }
            return jieguo;
        }

        /// <summary>
        /// http请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cook"></param>
        /// <returns></returns>
        public static string http_get(string url,string[] cook)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //加上这一句 解决“请求被中止: 未能创建 SSL/TLS 安全通道”的问题
            string serviceAddress = url; //请求地址
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);
            request.Method = "GET";
            for (int i = 0; i < cook.Length; i++)
            {
                request.Headers.Add("Set-Cookie", cook[i]);
            }
            var cookies = request.Headers.GetValues("Set-Cookie");

            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static string fengli_id(string linshi)
        {
            string jieguo = "";
            bool shifou_kaishi = false;
            for(int i=0;i<linshi.Length;i++)
            {
                if(linshi[i]=='=')
                {
                    shifou_kaishi = true;
                    continue;
                }
                if(linshi[i]==';')
                {
                    break;
                }
                if(shifou_kaishi==true)
                {
                    jieguo += linshi[i];
                }
            }
            return jieguo;
        }
    }
}
