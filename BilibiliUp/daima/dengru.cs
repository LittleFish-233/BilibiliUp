using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliUp.daima
{
    public static class dengru
    {
        public static string oauthKey = "";
        public static string url_er = "";
        public static int shenyushijian = 0;
        public static string tupian_mingzi = "";

        /// <summary>
        /// 当前类文件的根目录
        /// </summary>
        private static string lujing { get { return Environment.CurrentDirectory + "\\配置\\"; } }

        /// <summary>
        /// 获取二维码图片
        /// </summary>
        public static void huoqu_erweima()
        {
            //获取链接
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //加上这一句 解决“请求被中止: 未能创建 SSL/TLS 安全通道”的问题
            string serviceAddress = "https://passport.bilibili.com/qrcode/getLoginUrl"; //请求地址
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            //解析url
            var wai = (JObject)JsonConvert.DeserializeObject(retString);
            url_er= wai["data"]["url"].ToString();
            oauthKey = wai["data"]["oauthKey"].ToString();

            //获取二维码图片
            Directory.CreateDirectory(lujing+"二维码\\");
            Random random = new Random();
            tupian_mingzi = random.Next(0, 999999).ToString()+".png";
            FileStream fs = new FileStream(lujing + "二维码\\"+tupian_mingzi, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            // 设置参数
            request = WebRequest.Create("https://api.isoyu.com/qr/?m=2&e=H&p=10&url=" + url_er) as HttpWebRequest;
            //发送请求并获取相应回应数据
            response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            //创建本地文件写入流
            //Stream stream = new FileStream(tempFile, FileMode.Create);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                //stream.Write(bArr, 0, size);
                fs.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            //stream.Close();
            fs.Close();
            fs.Dispose();
            responseStream.Close();
            responseStream.Dispose();

            //设置时间
            shenyushijian = 175;
        }

        public static string[] jiancha_shifou()
        {
            if(oauthKey=="")
            {
                return null;
            }

            //发送请求
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //加上这一句 解决“请求被中止: 未能创建 SSL/TLS 安全通道”的问题
            string serviceAddress = "https://passport.bilibili.com/qrcode/getLoginInfo?oauthKey=" + oauthKey; //请求地址
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);
            request.Method = "POST";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            //检查返回值
            var wai = (JObject)JsonConvert.DeserializeObject(retString);
            try
            {
                string url = wai["data"]["url"].ToString();
                //成功
                return gongju.huoqu_xinxi(url);
            }
            catch
            {
                //失败
                switch(wai["data"].ToString())
                {
                    case "-1":
                        throw new Exception("密匙错误");
                    case "-2":
                        huoqu_erweima();
                        break;
                    default:
                        return null;
                }
            }
            return null;
        }
    }
}
