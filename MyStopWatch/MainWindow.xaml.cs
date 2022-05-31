﻿using System;
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
using System.Windows.Threading;
using System.Windows.Shapes;

namespace MyStopWatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool running = false;
        bool running1 = false;
        DispatcherTimer dispatcherTimer, dispatcherTimer1;
        TimeSpan offset, offset1;
        SolidColorBrush ButtonBorderColor = new SolidColorBrush(Color.FromRgb(255, 170, 250));


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if ((sender as DispatcherTimer).Tag == "timer0")
            {
                offset = offset + new TimeSpan(0, 0, 1);
                //var dd = App.Current.Properties["offset"];
                // var time = DateTime.Parse("00:00:" + i);
                lbl_time.Content = offset.ToString(@"hh\ \:\ mm\ \:\ ss");
                if (offset.Seconds == 59)
                    save_time(0);
            }
            else
            {
                offset1 = offset1 + new TimeSpan(0, 0, 1);
                lbl_time1.Content = offset1.ToString(@"hh\ \:\ mm\ \:\ ss");
                if (offset1.Seconds == 59)
                    save_time(1);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                App.Current.MainWindow.DragMove();
            //else if (e.RightButton == MouseButtonState.Pressed)
        }

        private void btn_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Button).BorderBrush = Brushes.Cyan;
        }

        private void btn_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Button).BorderBrush = ButtonBorderColor;
        }

        private void btn_playPause_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Name == "btn_playPause")
            {
                if (running)
                {
                    running = false;
                    dispatcherTimer.Stop();
                    btn_playPause.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/play_icon.png")));
                }
                else
                {
                    running = true;
                    dispatcherTimer.Start();
                    btn_playPause.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/pause_icon.png")));
                }
            }
            else
            {
                if (running1)
                {
                    running1 = false;
                    dispatcherTimer1.Stop();
                    btn_playPause1.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/play_icon.png")));
                }
                else
                {
                    running1 = true;
                    dispatcherTimer1.Start();
                    btn_playPause1.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/pause_icon.png")));
                }
            }
        }
        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Name == "btn_stop")
            {
                running = false;
                dispatcherTimer.Stop();
                offset = new TimeSpan(0, 0, 0);
                btn_playPause.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/play_icon.png")));
                lbl_time.Content = "00 : 00 : 00";
            }
            else
            {
                running1 = false;
                dispatcherTimer1.Stop();
                offset1 = new TimeSpan(0, 0, 0);
                btn_playPause1.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/play_icon.png")));
                lbl_time1.Content = "00 : 00 : 00";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width;
            Top = desktopWorkingArea.Bottom - Height;

            offset = new TimeSpan(Properties.Settings.Default.hours,
                Properties.Settings.Default.minutes,
                Properties.Settings.Default.seconds);
            offset1 = new TimeSpan(Properties.Settings.Default.hours1,
                Properties.Settings.Default.minutes1,
                Properties.Settings.Default.seconds1);

            lbl_time.Content = offset.ToString(@"hh\ \:\ mm\ \:\ ss");
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tag = "timer0";
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);

            lbl_time1.Content = offset1.ToString(@"hh\ \:\ mm\ \:\ ss");
            dispatcherTimer1 = new DispatcherTimer();
            dispatcherTimer1.Tag = "timer1";
            dispatcherTimer1.Tick += dispatcherTimer_Tick;
            dispatcherTimer1.Interval = TimeSpan.FromSeconds(1);
        }


        private void btn_mini_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Cyan;
        }

        private void btn_mini_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Button).Foreground = Brushes.White;
        }

        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            save_time(0);
            save_time(1);
            // App.Current.Properties["offset"] = offset.ToString(@"hh\ \:\ mm\ \:\ ss");
            App.Current.MainWindow.Close();
        }

        private void save_time(int id)
        {
            if (id == 0)
            {
                Properties.Settings.Default.hours = offset.Hours;
                Properties.Settings.Default.minutes = offset.Minutes;
                Properties.Settings.Default.seconds = offset.Seconds;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.hours1 = offset1.Hours;
                Properties.Settings.Default.minutes1 = offset1.Minutes;
                Properties.Settings.Default.seconds1 = offset1.Seconds;
                Properties.Settings.Default.Save();
            }
        }

        private void transparent_Checked(object sender, RoutedEventArgs e)
        {
            MyStopWatch.Opacity = 0.7;
            contextMenu.Opacity = 0.7;
        }

        private void transparent_Unchecked(object sender, RoutedEventArgs e)
        {
            MyStopWatch.Opacity = 1;
            contextMenu.Opacity = 1;
        }

        private void btn_setting_Click(object sender, RoutedEventArgs e)
        {
            contextMenu.IsOpen = true;
        }

        private void stayOnTop_Checked(object sender, RoutedEventArgs e)
        {
            MyStopWatch.Topmost = true;
        }

        private void stayOnTop_Unchecked(object sender, RoutedEventArgs e)
        {
            MyStopWatch.Topmost = false;
        }


        // For minimize
        // App.Current.MainWindow.WindowState = WindowState.Minimized;
    }
}
