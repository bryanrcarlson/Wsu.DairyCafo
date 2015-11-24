using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Wsu.DairyCafo.DataAccess;
using Wsu.DairyCafo.UI.PresentationLogic.Model;
using Wsu.DairyCafo.UI.PresentationLogic.ViewModel;
using Wsu.IO.DataAccess;

namespace Wsu.DairyCafo.UI.Client
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

            //TODO: Move this to Container
            MainWindow client = new MainWindow();
            IniFile iniFile = new IniFile(@"D:\WsuData\NIFA\Simulation\Scenarios\Sc1\.NIFA_dairy_scenario");
            ScenarioReader reader = new ScenarioReader(iniFile);
            ScenarioViewModel context = new ScenarioViewModel(reader);

 //           context.CurrentScenario = new ScenarioModel()
 //           {
 //               PathToWeatherFile = @"C:\",
 //               StartDate = DateTime.Now,
 //               EndDate = DateTime.Now
 //           };

            client.DataContext = context;

            client.Show();
        }
    }
}
