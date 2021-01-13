using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliUp.daima
{
    public  class Shuju:INotifyPropertyChanged
    {
        /// <summary>
        /// up主名字
        /// </summary>
        public  string Mingzi { get { return mingzi; } }
        private  string mingzi = "";

        public  List<Shiping_dan> shiping_liebiao = new List<Shiping_dan>();
        /// <summary>
        /// 粉丝数
        /// </summary>
        public  string Fenshishu { get { return fenshishu; } }
        private  string fenshishu = "";

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 首个视频名字 以下省略
        /// </summary>
        public  string Shiping_mingzi_shou { get { if (shiping_liebiao.Count != 0) {return shiping_liebiao[0].Shiping_mingzi_shou; } else { return ""; } } }
        /// <summary>
        /// 播放数
        /// </summary>
        public  string Bofanshu { get { if (shiping_liebiao.Count != 0) { return shiping_liebiao[0].Bofanshu; } else { return ""; } } }
        /// <summary>
        /// 点赞
        /// </summary>
        public  string Dianzan { get { if (shiping_liebiao.Count != 0) { return shiping_liebiao[0].Dianzan; } else { return ""; } } }
        /// <summary>
        /// 投币
        /// </summary>
        public  string Toubi { get { if (shiping_liebiao.Count != 0) { return shiping_liebiao[0].Toubi; } else { return ""; } } }
        /// <summary>
        /// 收藏
        /// </summary>
        public  string Shoucang { get { if (shiping_liebiao.Count != 0) { return shiping_liebiao[0].Shoucang; } else { return ""; } } }
        /// <summary>
        /// 评论
        /// </summary>
        public  string Pinglun { get { if (shiping_liebiao.Count != 0) { return shiping_liebiao[0].Pinglun; } else { return ""; } } }
        /// <summary>
        /// 弹幕
        /// </summary>
        public  string Danmu { get { if (shiping_liebiao.Count != 0) { return shiping_liebiao[0].Danmu; } else { return ""; } } }
        /// <summary>
        /// 分享
        /// </summary>
        public  string Fenxiang { get { if (shiping_liebiao.Count != 0) { return shiping_liebiao[0].Fenxiang; } else { return ""; } } }
        /// <summary>
        /// 使用给定uuid刷新数据
        /// </summary>
        /// <param name="uuid">用户id</param>
        public  void shuaxin_shuju(string uuid,string[] cook)
        {
            //清空
            shiping_liebiao.Clear();

            //获取视频列表
            string serviceAddress = "https://api.bilibili.com/x/space/arc/search?mid="+uuid+"&ps=10&tid=0&pn=1&keyword=&order=pubdate&jsonp=jsonp"; //请求地址
            string retString = gongju.http_get(serviceAddress,cook);

            //解析json 需要的信息 视频列表 视频id
            var wai = (JObject)JsonConvert.DeserializeObject(retString);
            var json_liebiao = wai["data"]["list"]["vlist"].Children().ToArray();
            foreach (var item in json_liebiao)
            {
                Shiping_dan shiping_ = new Shiping_dan();
                shiping_.chushihua(item["bvid"].ToString(),cook);
                shiping_liebiao.Add(shiping_);
            }

            //获取up主名字
            mingzi = json_liebiao[0]["author"].ToString();

            //获取up主粉丝
            serviceAddress = "https://api.bilibili.com/x/relation/stat?vmid=" + uuid + "&jsonp=jsonp"; //请求地址
            retString = gongju.http_get(serviceAddress, cook);

            //解析json
            wai = (JObject)JsonConvert.DeserializeObject(retString);
            fenshishu = wai["data"]["follower"].ToString();

            //发起改变事件
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Mingzi"));
                PropertyChanged(this, new PropertyChangedEventArgs("Fenshishu"));
                PropertyChanged(this, new PropertyChangedEventArgs("Shiping_mingzi_shou"));
                PropertyChanged(this, new PropertyChangedEventArgs("Bofanshu"));
                PropertyChanged(this, new PropertyChangedEventArgs("Dianzan"));
                PropertyChanged(this, new PropertyChangedEventArgs("Toubi"));
                PropertyChanged(this, new PropertyChangedEventArgs("Toubi"));
                PropertyChanged(this, new PropertyChangedEventArgs("Shoucang"));
                PropertyChanged(this, new PropertyChangedEventArgs("Pinglun"));
                PropertyChanged(this, new PropertyChangedEventArgs("Danmu"));
                PropertyChanged(this, new PropertyChangedEventArgs("Fenxiang"));
            }
        }

    }

    public class Shiping_dan
    {
        /// <summary>
        /// 视频id
        /// </summary>
        public string Id { get { return id; } }
        private string id = "";
        /// <summary>
        /// 名字
        /// </summary>
        public string Shiping_mingzi_shou
        {
            get
            {
                int maxLength = 17;//限制最大字符数,如果str超出这个数字,则显示省略号
                string Str = shiping_mingzi_shou.Length > maxLength ? shiping_mingzi_shou.Substring(0, maxLength) + "..." : shiping_mingzi_shou;
                return Str;
            }
        }
        private  string shiping_mingzi_shou = "";
        /// <summary>
        /// 播放数
        /// </summary>
        public  string Bofanshu { get { return bofenshu; } }
        private  string bofenshu = "";
        /// <summary>
        /// 点赞
        /// </summary>
        public  string Dianzan { get { return dianzan; } }
        private  string dianzan = "";
        /// <summary>
        /// 投币
        /// </summary>
        public  string Toubi { get { return toubi; } }
        private  string toubi = "";
        /// <summary>
        /// 收藏
        /// </summary>
        public  string Shoucang { get { return shoucang; } }
        private  string shoucang = "";
        /// <summary>
        /// 评论
        /// </summary>
        public  string Pinglun { get { return pinglun; } }
        private  string pinglun = "";
        /// <summary>
        /// 弹幕
        /// </summary>
        public  string Danmu { get { return danmu; } }
        private  string danmu = "";
        /// <summary>
        /// 分享
        /// </summary>
        public  string Fenxiang { get { return fenxiang; } }
        private  string fenxiang = "";
        /// <summary>
        /// 初始化
        /// </summary>
        public void chushihua(string id_,string[] cook)
        {
            id = id_;

            //获取视频信息
            string serviceAddress = "https://api.bilibili.com/x/web-interface/view?bvid=" + id ; //请求地址
            string retString = gongju.http_get(serviceAddress, cook);

            //解析json
            var wai = (JObject)JsonConvert.DeserializeObject(retString);
            dianzan = wai["data"]["stat"]["like"].ToString();
            toubi = wai["data"]["stat"]["coin"].ToString();
            shoucang = wai["data"]["stat"]["favorite"].ToString();
            pinglun = wai["data"]["stat"]["reply"].ToString();
            danmu = wai["data"]["stat"]["danmaku"].ToString();
            fenxiang = wai["data"]["stat"]["share"].ToString();
            bofenshu = wai["data"]["stat"]["view"].ToString();
            shiping_mingzi_shou = wai["data"]["title"].ToString();


        }
    }
}
