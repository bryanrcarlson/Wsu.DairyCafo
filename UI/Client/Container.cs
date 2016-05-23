using System.IO;
using Wsu.DairyCafo.DataAccess;
using Wsu.DairyCafo.DataAccess.Core;
using Wsu.DairyCafo.UI.PresentationLogic.ViewModel;

namespace Wsu.DairyCafo.UI.Client
{
    class Container
    {
        public MainWindow ResolveMainWindow()
        {
            MainWindow client = new MainWindow();
            ScenarioFile dairyScenario = new ScenarioFile();
            ScenarioFile fieldScenario = new ScenarioFile();
            ScenarioDefaults defaults = new ScenarioDefaults();
            string pathToWeatherDatabase = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Assets", "Database", "Weather");
            WeatherGrabber grabber = new WeatherGrabber(pathToWeatherDatabase);
            ScenarioReader reader =
                new ScenarioReader(dairyScenario, fieldScenario);
            ScenarioWriter writer =
                new ScenarioWriter(dairyScenario, fieldScenario, defaults, grabber);
            ScenarioViewModel context = new ScenarioViewModel(reader, writer);

            client.DataContext = context;

            return client;
        }
    }
}
