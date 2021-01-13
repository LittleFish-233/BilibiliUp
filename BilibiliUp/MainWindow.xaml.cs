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
using Panuon.UI.Silver.Core;

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
            //读取配置
            daima.Peizhi.duqu_jiben();
            //初始化计时器
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += timer1_Tick;
            timer.Start();
            //透明度
            rec1.Opacity = daima.Peizhi.Toumingdu;
            //初始化按钮
            if (daima.Peizhi.Shifou_suoding==true)
            {
                btn_suo.Visibility = Visibility.Collapsed;
                btn_suo_1.Visibility = Visibility.Visible;
            }
            else
            {
                btn_suo.Visibility = Visibility.Visible;
                btn_suo_1.Visibility = Visibility.Collapsed;
            }
            if(daima.Peizhi.Shifou_zhiding==true)
            {
                btn_zhiding.Visibility = Visibility.Collapsed;
                btn_zhiding_1.Visibility = Visibility.Visible;
                this.Topmost = true;
            }
            else
            {
                btn_zhiding.Visibility = Visibility.Visible;
                btn_zhiding_1.Visibility = Visibility.Collapsed;
                this.Topmost = false;
            }
            //获取数据
            try
            {
               
                //检查是否登入
                if (daima.Peizhi.DedeUserID == "")
                {
                    var result = MessageBoxX.Show("未登入Bilibili账号，可能会导致API调用受阻，点击“是的”将打开二维码登入界面，点击“不”将继续获取信息。后续可以点击设置按钮进行登录和其他高级设置", "错误", Application.Current.MainWindow, MessageBoxButton.YesNo, new MessageBoxXConfigurations()
                    {
                        MessageBoxStyle = MessageBoxStyle.Classic,
                        MessageBoxIcon = MessageBoxIcon.Error,
                        ButtonBrush = "#FF4C4C".ToColor().ToBrush(),
                    });
                    if (result == MessageBoxResult.Yes)
                    {
                        dengru dengru_ = new dengru();
                        dengru_.ShowDialog();
                    }
                }
                //检查up主信息
                if(daima.Peizhi.Up=="")
                {
                    var result = MessageBoxX.Show("绑定的UP主的uuid为空，将为你打开设置，请输入正确的uuid", "错误", Application.Current.MainWindow, MessageBoxButton.YesNo, new MessageBoxXConfigurations()
                    {
                        MessageBoxStyle = MessageBoxStyle.Classic,
                        MessageBoxIcon = MessageBoxIcon.Error,
                        ButtonBrush = "#FF4C4C".ToColor().ToBrush(),
                    });
                    if(result==MessageBoxResult.Yes)
                    {
                        shezhi shezhi_ = new shezhi();
                        shezhi_.Show();
                        return;
                    }
                    else
                    {
                        this.Close();
                        return;
                    }
                }
                //shuju.shuaxin_shuju(daima.Peizhi.Up,daima.Peizhi.huoqu_cook());
                    
            }
            catch (Exception exc)
           {
                Notice.Show("获取信息失败，详细信息<" + exc.Message + ">", "Error", 10, MessageBoxIcon.Error);
               daima.Peizhi.tuichudengru();
                dengru dengru_ = new dengru();
                dengru_.Show();
           }

        }
        int jishu = 0;
        string linshi_up = "";
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                jishu++;
                //透明度
                rec1.Opacity = daima.Peizhi.Toumingdu;
                //判断是否登入
                if (daima.Peizhi.Up != linshi_up && daima.Peizhi.Up != "")
                {
                    jishu = 0;
                    linshi_up = daima.Peizhi.Up;
                    shuju.shuaxin_shuju(daima.Peizhi.Up, daima.Peizhi.huoqu_cook());
                    //刷新缓存
                    qingkong_huancun();
                }
                if (jishu > daima.Peizhi.Huoqu_jiange*2 && daima.Peizhi.Up != "")
                {
                    jishu = 0;
                    linshi_up = daima.Peizhi.Up;
                    shuju.shuaxin_shuju(daima.Peizhi.Up, daima.Peizhi.huoqu_cook());
                    //播放动画
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        donghua();
                    }));
                }
               
            }
            catch (Exception exc)
            {
                Notice.Show("获取信息失败，详细信息<" + exc.Message + ">", "Error", 10, MessageBoxIcon.Error);
            }
        }
        static int[] linshi_data = new int[8];

        private void donghua()
        {
            int[] chaju = new int[8];
            chaju[0] = int.Parse(shuju.Fenshishu) - linshi_data[0];
            chaju[1] = int.Parse(shuju.Bofanshu) - linshi_data[1];
            chaju[2] = int.Parse(shuju.Dianzan) - linshi_data[2];
            chaju[3] = int.Parse(shuju.Toubi) - linshi_data[3];
            chaju[4] = int.Parse(shuju.Shoucang) - linshi_data[4];
            chaju[5] = int.Parse(shuju.Pinglun) - linshi_data[5];
            chaju[6] = int.Parse(shuju.Danmu) - linshi_data[6];
            chaju[7] = int.Parse(shuju.Fenxiang) - linshi_data[7];

            if (chaju[0] != 0)
            {
                donghua(grid1, 50, chaju[0]);
            }
            if (chaju[1] != 0)
            {
                donghua(grid2, 20, chaju[1]);
            }
            if (chaju[2] != 0)
            {
                donghua(grid3, 17, chaju[2]);
            }
            if (chaju[3] != 0)
            {
                donghua(grid4, 17, chaju[3]);
            }
            if (chaju[4] != 0)
            {
                donghua(grid5, 17, chaju[4]);
            }
            if (chaju[5] != 0)
            {
                donghua(grid6, 17, chaju[5]);
            }
            if (chaju[6] != 0)
            {
                donghua(grid7, 17, chaju[6]);
            }
            if (chaju[7] != 0)
            {
                donghua(grid8, 17, chaju[7]);
            }
            qingkong_huancun();
        }
        //同步缓存
        private void qingkong_huancun()
        {
            linshi_data[0] = int.Parse(shuju.Fenshishu);
            linshi_data[1] = int.Parse(shuju.Bofanshu);
            linshi_data[2] = int.Parse(shuju.Dianzan);
            linshi_data[3] = int.Parse(shuju.Toubi);
            linshi_data[4] = int.Parse(shuju.Shoucang);
            linshi_data[5] = int.Parse(shuju.Pinglun);
            linshi_data[6] = int.Parse(shuju.Danmu);
            linshi_data[7] = int.Parse(shuju.Fenxiang);
        }

        private void donghua(Grid fukuanjia,int zihao,int shuliang)
        {
            string linshi = "";
            if(shuliang<0)
            {
                linshi = shuliang.ToString();
            }
            else
            {
                linshi = "+" + shuliang.ToString();
            }
            var textblock1 = new TextBlock()
            {
                Foreground = textblock2.Foreground,
                Text = linshi,
                FontSize = zihao
            };
            if(shuliang>0)
            {
                textblock1.Foreground = textblock3.Foreground;
            }
            else
            {
                textblock1.Foreground = textblock2.Foreground;
            }
            fukuanjia.Children.Add(textblock1);
            AnimationHelper.SetBeginTimeSeconds(textblock1, 0);
            AnimationHelper.SetDurationSeconds(textblock1, daima.Peizhi.Donghua_shijian);
            AnimationHelper.SetFadeOut(textblock1, true);
            //AnimationHelper.SetSlideInFromBottom(textblock1, true);
        }

        /// <summary>
        /// 窗口移动事件
        /// </summary>
        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (daima.Peizhi.Shifou_suoding == true)
            {
                return;
            }

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
            if(daima.Peizhi.Shifou_suoding==true)
            {
                return;
            }
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
            Application.Current.Shutdown();
        }

        private void btn_she_Click(object sender, RoutedEventArgs e)
        {
            shezhi shezhi_ = new shezhi();
            shezhi_.Show();
        }

        private void btn_zhiding_Click(object sender, RoutedEventArgs e)
        {
            daima.Peizhi.xiugai(true, daima.Peizhi.Shifou_suoding);
            btn_zhiding.Visibility = Visibility.Collapsed;
            btn_zhiding_1.Visibility = Visibility.Visible;
            this.Topmost = true;
        }

        private void btn_zhiding_1_Click(object sender, RoutedEventArgs e)
        {
            daima.Peizhi.xiugai(false, daima.Peizhi.Shifou_suoding);
            btn_zhiding.Visibility = Visibility.Visible;
            btn_zhiding_1.Visibility = Visibility.Collapsed;
            this.Topmost = false;
        }

        private void btn_suo_1_Click(object sender, RoutedEventArgs e)
        {
            daima.Peizhi.xiugai(daima.Peizhi.Shifou_zhiding, false);
            btn_suo.Visibility = Visibility.Visible;
            btn_suo_1.Visibility = Visibility.Collapsed;

        }

        private void btn_suo_Click(object sender, RoutedEventArgs e)
        {
            daima.Peizhi.xiugai(daima.Peizhi.Shifou_zhiding, true);
            btn_suo.Visibility = Visibility.Collapsed;
            btn_suo_1.Visibility = Visibility.Visible;

        }
    }
}
