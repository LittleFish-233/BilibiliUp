using System;
using System.Collections.Generic;
using System.Linq;
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

namespace BilibiliUp
{
    /// <summary>
    /// shezhi.xaml 的交互逻辑
    /// </summary>
    public partial class shezhi : Window
    {
        public shezhi()
        {
            InitializeComponent();
            Loaded += Shezhi_Loaded;
        }

        private void Shezhi_Loaded(object sender, RoutedEventArgs e)
        {
            rec1.Opacity = daima.Peizhi.Toumingdu;
            //填入数据
            textbox1.Text = daima.Peizhi.Up;
            textbox2.Text = daima.Peizhi.DedeUserID;
            textbox3.Text = daima.Peizhi.DedeUserID__ckMd5;
            textbox4.Text = daima.Peizhi.Expires;
            textbox5.Text = daima.Peizhi.SESSDATA;
            textbox6.Text = daima.Peizhi.Bili_jct;
            textbox7.Text = daima.Peizhi.Sid;
            //滑动块
            textblock2.Text = "背景不透明度：" + (daima.Peizhi.Toumingdu*100).ToString("0")+"%";
            slider1.Value = daima.Peizhi.Toumingdu * 10;
            textblock3.Text = "动画延迟时间：" + daima.Peizhi.Donghua_shijian.ToString("0.0") + " s";
            slider2.Value = daima.Peizhi.Donghua_shijian;
            textblock4.Text = "数据获取间隔时间：" + daima.Peizhi.Huoqu_jiange.ToString("0.0") + " s";
            slider3.Value = (daima.Peizhi.Huoqu_jiange+0.001)/10;
            //判断是否登入
            if (daima.Peizhi.DedeUserID == "")
            {
                textblock1.Text = "未登入账号，请登入";
                button1.Visibility = Visibility.Collapsed;
                button2.Visibility = Visibility.Visible;
            }
            else
            {
                textblock1.Text = "已经登入的账号："+daima.Peizhi.Name;
                button1.Visibility = Visibility.Visible;
                button2.Visibility = Visibility.Collapsed;
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //填入数据
            daima.Peizhi.xiugai(textbox1.Text, textbox2.Text, textbox3.Text, textbox4.Text, textbox5.Text, textbox6.Text,textbox7.Text, slider1.Value/10, slider2.Value,slider3.Value*10) ;
            
            this.Close();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/LittleFish-233/BilibiliUp/releases");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            dengru dengru_ = new dengru();
            dengru_.Show();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            daima.Peizhi.tuichudengru();
            textbox2.Text = "";
            textbox3.Text = "";
            textbox4.Text = "";
            textbox5.Text = "";
            textbox6.Text = "";
            textblock1.Text = "未登入账号，请登入";
            button1.Visibility = Visibility.Collapsed;
            button2.Visibility = Visibility.Visible;

        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textblock2.Text = "背景不透明度：" + (slider1.Value * 10).ToString("0") + "%";

        }

        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textblock3.Text = "动画延迟时间：" + slider2.Value.ToString("0.0") + " s";

        }

        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textblock4.Text = "数据获取间隔时间：" + (slider3.Value*10).ToString("0.0") + " s";
        }
    }
}
