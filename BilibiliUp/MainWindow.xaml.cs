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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Panuon.UI.Silver;

namespace BilibiliUp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        daima.Shuju shuju = new daima.Shuju();
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            DataContext = shuju;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(5000);
            timer.Tick += timer1_Tick;
            timer.Start();
            try
            {
                //读取配置
                daima.Peizhi.duqu_jiben();
                //检查是否登入
                if (daima.Peizhi.DedeUserID == "")
                {
                    dengru dengru_ = new dengru();
                    dengru_.Show();
                    this.Close();
                }
                else
                {
                    shuju.shuaxin_shuju(daima.Peizhi.Up,daima.Peizhi.huoqu_cook());
                    
                }   
            }
            catch (Exception exc)
            {
                Notice.Show("获取信息失败，详细信息<" + exc.Message + ">", "Error", 10, MessageBoxIcon.Error);
                daima.Peizhi.tuichudengru();
                dengru dengru_ = new dengru();
                dengru_.Show();
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //判断是否登入
                if (daima.Peizhi.DedeUserID != "")
                {
                    shuju.shuaxin_shuju(daima.Peizhi.Up, daima.Peizhi.huoqu_cook());
                }
            }
            catch (Exception exc)
            {
                Notice.Show("获取信息失败，详细信息<" + exc.Message + ">", "Error", 10, MessageBoxIcon.Error);
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

    }
}
