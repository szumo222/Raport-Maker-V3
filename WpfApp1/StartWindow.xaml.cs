﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfApp1
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            DispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            DispatcherTimer.Start();
        }

        public DispatcherTimer DispatcherTimer { get; set; } = new DispatcherTimer();

        //Przełączenie okna po czasie
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Configuration_Window configurationWindow = new Configuration_Window();
            MainWindow mainWindow = new MainWindow();
            if (File.Exists("config_raport_maker.xml"))
                mainWindow.Show();
            else
                configurationWindow.Show();
            this.Close();
            DispatcherTimer.Stop();
        }

        //Możliwość ruszania oknem
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}