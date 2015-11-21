﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Wsu.DairyCafo.PresentationLogic;

namespace Wsu.DairyCafo.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // The following method does not allow multiple WPF Windows, need
            //to use some form of Factory pattern for that.  
            //See: http://stackoverflow.com/a/25508012/1621156
            MainWindow app = new MainWindow();
            ScenarioViewModel context = new ScenarioViewModel();
            app.DataContext = context;

            app.Show();

            
        }
    }
}
