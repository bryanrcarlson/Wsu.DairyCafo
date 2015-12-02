using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            ScenarioReader reader =
                new ScenarioReader(dairyScenario, fieldScenario);
            ScenarioWriter writer =
                new ScenarioWriter(dairyScenario, fieldScenario, defaults);
            ScenarioViewModel context = new ScenarioViewModel(reader, writer);

            client.DataContext = context;

            return client;
        }
    }
}
