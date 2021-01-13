using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliUp.daima
{
    static class Peizhi
    {
        /// <summary>
        /// 版本
        /// </summary>
        public static int banben = 1;

        /// <summary>
        /// up主的uuid
        /// </summary>
        public static string Up
        {
            get
            {
                return up;
            }
        }
        private static string up = "";

        public static string DedeUserID { get { return dedeUserID; } }
        private static string dedeUserID = "";

        public static string DedeUserID__ckMd5 { get { return dedeUserID__ckMd5; } }
        private static string dedeUserID__ckMd5 = "";

        public static string Expires { get { return expires; } }
        private static string expires = "";

        public static string SESSDATA { get { return sESSDATA; } }
        private static string sESSDATA = "";

        public static string Bili_jct { get { return bili_jct; } }
        private static string bili_jct = "";


        private static bool shifou_zhengzai=false;

        /// <summary>
        /// 当前类文件的根目录
        /// </summary>
        private static string lujing { get { return Environment.CurrentDirectory + "\\配置\\"; } }

        /// <summary>
        /// 保存基本信息
        /// </summary>
        public static void baocun_jiben()
        {
            if (shifou_zhengzai == true)
            {
                return;
            }
            shifou_zhengzai = true;

            //序列化数据
            //添加基本数据
            string data = new JObject(
                      new JProperty("banben", banben),
                      new JProperty("up", up),
                      new JProperty("dedeUserID", dedeUserID),
                      new JProperty("dedeUserID__ckMd5", dedeUserID__ckMd5),
                      new JProperty("expires", expires),
                      new JProperty("sESSDATA", sESSDATA),
                      new JProperty("bili_jct", bili_jct)).ToString();
            //保存到文件
            Directory.CreateDirectory(lujing);
            //新建文件
            FileStream xiaFile = new FileStream(lujing + "infor.json", FileMode.Create);
            //写入
            byte[] buf = Encoding.UTF8.GetBytes(data);
            xiaFile.Write(buf, 0, buf.Length);
            xiaFile.Flush();
            xiaFile.Close();

            shifou_zhengzai = false;
        }
        /// <summary>
        /// 读取基本信息
        /// </summary>
        /// <returns>是否成功</returns>
        public static bool duqu_jiben()
        {
            try
            {
                //打开文件
                FileInfo fi = new FileInfo(lujing + "infor.json");
                long len = fi.Length;

                FileStream fs = new FileStream(lujing + "infor.json", FileMode.Open);
                byte[] buffer = new byte[len];
                fs.Read(buffer, 0, (int)len);//读取文件
                fs.Close();//关闭
                string str = Encoding.UTF8.GetString(buffer);//转码
                string[] result = peizhi_jiben_jiexi(str);//解析Json
                //根据读取的数据初始化相应变量
                try
                {
                    if (result[1] != "")
                    {
                        up = result[1];
                    }
                    if (result[2] != "")
                    {
                        dedeUserID = result[2];
                    }
                    if (result[3] != "")
                    {
                        dedeUserID__ckMd5 = result[3];
                    }
                    if (result[4] != "")
                    {
                        expires = result[4];
                    }
                    if (result[5] != "")
                    {
                        sESSDATA = result[5];
                    }
                    if (result[6] != "")
                    {
                        bili_jct = result[6];
                    }
                }
                catch
                {

                }
                //检查版本号是否一致
                if (banben.ToString() != result[0])
                {
                    //保存
                    baocun_jiben();
                }
                return true;
            }
            catch (Exception exc)
            {
                baocun_jiben();
                return false;
            }
        }
        /// <summary>
        /// 解析读取的配置文件Json 外部 负责匹配版本号
        /// </summary>
        /// <param name="str">配置文件json字符串</param>
        /// <returns>按照最新的变量列表返回字符串数组</returns>
        public static string[] peizhi_jiben_jiexi(string str)
        {
            var json = (JObject)JsonConvert.DeserializeObject(str);
            switch (int.Parse(json["banben"].ToString()))
            {
                case 1:
                    return peizhi_jiben_jiexi_1(str);
                default:
                    return null;
            }
        }
        /// <summary>
        /// 解析读取的配置文件 版本1
        /// </summary>
        /// <param name="str">配置文件json字符串</param>
        /// <returns>按照最新的变量列表返回字符串数组</returns>
        private static string[] peizhi_jiben_jiexi_1(string str)
        {
            var json = (JObject)JsonConvert.DeserializeObject(str);
            var banben_ = json["banben"];
            var up = json["up"];
            var dedeUserID = json["dedeUserID"];
            var dedeUserID__ckMd5 = json["dedeUserID__ckMd5"];
            var expires = json["expires"];
            var sESSDATA = json["sESSDATA"];
            var bili_jct = json["bili_jct"];

            //保存结果
            string[] re = new string[7];

            re[0] = banben_.ToString();
            re[1] = up.ToString();
            re[2] = dedeUserID.ToString();
            re[3] = dedeUserID__ckMd5.ToString();
            re[4] = expires.ToString();
            re[5] = sESSDATA.ToString();
            re[6] = bili_jct.ToString();
            return re;
        }

        public static void tuichudengru()
        {
            dedeUserID = "";
            baocun_jiben();
        }

        public static void xieru_dengru(string DedeUserID_,string DedeUserID__ckMd5_,string Expires_,string SESSDATA_,string bili_jct_)
        {
            dedeUserID = DedeUserID_;
            dedeUserID__ckMd5 = DedeUserID__ckMd5_;
            expires = Expires_;
            sESSDATA = SESSDATA_;
            bili_jct = bili_jct_;

            baocun_jiben();
        }

        public static string[] huoqu_cook()
        {
            string[] jieguo = new string[4];
            jieguo[0] = DedeUserID;
            jieguo[1] = DedeUserID__ckMd5;
            jieguo[2] = SESSDATA;
            jieguo[3] = bili_jct;

            return jieguo;
        }
    }
}
