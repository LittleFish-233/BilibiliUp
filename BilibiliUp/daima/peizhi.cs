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
        public static string Name { get { return name; } }
        private static string name = "";
        public static string Sid { get { return sid; } }
        private static string sid = "";
        public static bool Shifou_zhiding { get { return shifouz_zhiding; } }
        private static bool shifouz_zhiding = false;
        public static bool Shifou_suoding { get { return shifou_suoding; } }
        private static bool shifou_suoding = false;
        public static double Toumingdu { get { return toumingdu; } }
        private static double toumingdu = 0.5;
        public static double Donghua_shijian { get { return donghua_shijian; } }
        private static double donghua_shijian = 3;
        public static double Huoqu_jiange { get { return huoqu_jiange; } }
        private static double huoqu_jiange = 10;




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
                      new JProperty("bili_jct", bili_jct),
                      new JProperty("name", name),
                      new JProperty("sid", sid),
                      new JProperty("shifouz_zhiding", shifouz_zhiding),
                      new JProperty("shifou_suoding", shifou_suoding),
                      new JProperty("toumingdu", toumingdu),
                      new JProperty("donghua_shijian", donghua_shijian),
                      new JProperty("huoqu_jiange", huoqu_jiange)).ToString();
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
                    if (result[7] != "")
                    {
                        name = result[7];
                    }
                    if (result[8] != "")
                    {
                        sid = result[8];
                    }
                    if (result[9] != "")
                    {
                        shifouz_zhiding = bool.Parse(result[9]);
                    }
                    if (result[10] != "")
                    {
                        shifou_suoding = bool.Parse(result[10]);
                    }
                    if (result[11] != "")
                    {
                        toumingdu = double.Parse(result[11]);
                    }
                    if (result[12] != "")
                    {
                        donghua_shijian = double.Parse(result[12]);
                    }
                    if (result[13] != "")
                    {
                        huoqu_jiange = double.Parse(result[13]);
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
            var name = json["name"];
            var sid = json["sid"];
            var shifouz_zhiding = json["shifouz_zhiding"];
            var shifou_suoding = json["shifou_suoding"];
            var toumingdu = json["toumingdu"];
            var donghua_shijian = json["donghua_shijian"];
            var huoqu_jiange = json["huoqu_jiange"];

            //保存结果
            string[] re = new string[14];

            re[0] = banben_.ToString();
            re[1] = up.ToString();
            re[2] = dedeUserID.ToString();
            re[3] = dedeUserID__ckMd5.ToString();
            re[4] = expires.ToString();
            re[5] = sESSDATA.ToString();
            re[6] = bili_jct.ToString();
            re[7] = name.ToString();
            re[8] = sid.ToString();
            re[9] = shifouz_zhiding.ToString();
            re[10] = shifou_suoding.ToString();
            re[11] = toumingdu.ToString();
            re[12] = donghua_shijian.ToString();
            re[13] = huoqu_jiange.ToString();
            return re;
        }
        /// <summary>
        /// 退出登入 清空信息
        /// </summary>
        public static void tuichudengru()
        {
            dedeUserID = "";
            dedeUserID__ckMd5 = "";
            expires = "";
            sESSDATA = "";
            bili_jct = "";
            name = "";

            baocun_jiben();
        }
        /// <summary>
        /// 写入登入后获取的信息
        /// </summary>
        /// <param name="sid_"></param>
        /// <param name="DedeUserID_"></param>
        /// <param name="DedeUserID__ckMd5_"></param>
        /// <param name="SESSDATA_"></param>
        /// <param name="bili_jct_"></param>
        public static void xieru_dengru(string sid_,string DedeUserID_,string DedeUserID__ckMd5_,string SESSDATA_,string bili_jct_)
        {
            //写入
            dedeUserID = DedeUserID_;
            dedeUserID__ckMd5 = DedeUserID__ckMd5_;
            expires = "100000";
            sESSDATA = SESSDATA_;
            bili_jct = bili_jct_;
            sid = sid_;
            //获取用户名
            string serviceAddress = "http://api.bilibili.com/x/space/acc/info?mid=" + gongju.fengli_id(dedeUserID); //请求地址
            string retString = gongju.http_get(serviceAddress, huoqu_cook());

            //解析json
            var wai = (JObject)JsonConvert.DeserializeObject(retString);
            name = wai["data"]["name"].ToString();

            baocun_jiben();
        }
        /// <summary>
        /// 获取 登入信息用于传递
        /// </summary>
        /// <returns></returns>
        public static string[] huoqu_cook()
        {
            string[] jieguo = new string[5];  
            jieguo[0] = sid;
            jieguo[1] = DedeUserID;
            jieguo[2] = DedeUserID__ckMd5;
            jieguo[3] = SESSDATA;
            jieguo[4] = bili_jct;

            return jieguo;
        }
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="up_"></param>
        /// <param name="DedeUserID_"></param>
        /// <param name="DedeUserID__ckMd5_"></param>
        /// <param name="Expires_"></param>
        /// <param name="SESSDATA_"></param>
        /// <param name="bili_jct_"></param>
        public static void xiugai(string up_,string DedeUserID_, string DedeUserID__ckMd5_, string Expires_, string SESSDATA_, string bili_jct_,string sid_,double toumingdu_,double donghua_shijian_,double huoqu_jiange_)
        {
            up = up_;
            dedeUserID = DedeUserID_;
            dedeUserID__ckMd5 = DedeUserID__ckMd5_;
            expires = Expires_;
            sESSDATA = SESSDATA_;
            bili_jct = bili_jct_;
            toumingdu = toumingdu_;
            donghua_shijian = donghua_shijian_;
            huoqu_jiange = huoqu_jiange_;
            sid = sid_;

            baocun_jiben();
        }

        public static void xiugai(bool shifou_zhiding_,bool shifou_suoding_)
        {
            shifouz_zhiding = shifou_zhiding_;
            shifou_suoding = shifou_suoding_;

            baocun_jiben();
        }
    }
}
