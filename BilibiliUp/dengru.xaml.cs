using BundleTransformer.Core.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BilibiliUp
{
    /// <summary>
    /// dengru.xaml 的交互逻辑
    /// </summary>
    public partial class dengru : Window
    {
        /// <summary>
        /// 当前类文件的根目录
        /// </summary>
        private static string lujing { get { return Environment.CurrentDirectory + "\\配置\\"; } }


        DispatcherTimer timer;
        bool shifou_zhengzai_zhixing = false;


        public dengru()
        {
            InitializeComponent();
            Loaded += Dengru_Loaded;
        }

        private void Dengru_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(900);
            timer.Tick += timer1_Tick;
            timer.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(shifou_zhengzai_zhixing==true)
            {
                return;
            }
            shifou_zhengzai_zhixing = true;

            //检查扫码是否成功
            string[] jieguo= daima.dengru.jiancha_shifou();
            if(jieguo!=null)
            {
                //写入
                daima.Peizhi.xieru_dengru(jieguo[0], jieguo[1], jieguo[2], jieguo[3],jieguo[4]);
                //新建
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                //关闭窗口
                this.Close();
            }
            
            //判断是否重新获取
            if (daima.dengru.shenyushijian < 1)
            {
                daima.dengru.huoqu_erweima();
                image1.Source = new BitmapImage(new Uri(lujing + "二维码\\"+daima.dengru.tupian_mingzi));
            }
            //减少时间
            daima.dengru.shenyushijian--;
            //同步UI
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                textblock1.Text = "将在 " + daima.dengru.shenyushijian + "s 后刷新";
            }));
            //刷新图片
            image1.Source = new BitmapImage(new Uri(lujing + "二维码\\" + daima.dengru.tupian_mingzi));
            shifou_zhengzai_zhixing = false;
        }
        /// <summary>
        /// 窗口移动事件
        /// </summary>
        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        int i = 0;
        /// <summary>
        /// 标题栏双击事件
        /// </summary>
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            i += 1;
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer.Tick += (s, e1) => { timer.IsEnabled = false; i = 0; };
            timer.IsEnabled = true;

            if (i % 2 == 0)
            {
                timer.IsEnabled = false;
                i = 0;
                this.WindowState = this.WindowState == WindowState.Maximized ?
                              WindowState.Normal : WindowState.Maximized;
            }
        }

        /// <summary>
        /// 窗口最小化
        /// </summary>
        private void btn_min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; //设置窗口最小化
        }

        /// <summary>
        /// 窗口最大化与还原
        /// </summary>
        private void btn_max_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal; //设置窗口还原
            }
            else
            {
                this.WindowState = WindowState.Maximized; //设置窗口最大化
            }
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
