using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wsu.DairyCafo.DataAccess.Core;
using Wsu.DairyCafo.DataAccess.Dto;
using Wsu.DairyCafo.UI.PresentationLogic.Helper;
using Wsu.DairyCafo.UI.PresentationLogic.Model;

namespace Wsu.DairyCafo.UI.PresentationLogic.ViewModel
{
    public class ScenarioViewModel : ObservableObject
    {
        #region Fields
        private IScenarioReader reader;

        private ScenarioModel currentScenario;
        private ICommand newScenarioCommand;
        private ICommand getScenarioCommand;
        private ICommand selectWeatherCommand;
        private ICommand saveScenarioCommand;
        #endregion // Fields

        #region Public Properties/Commands
        public ScenarioModel CurrentScenario
        {
            get { return currentScenario; }
            set
            {
                if(value != currentScenario)
                {
                    currentScenario = value;
                    OnPropertyChanged("CurrentScenario");
                }
            }
        }
        public ICommand NewScenarioCommand
        {
            get
            {
                newScenarioCommand = new RelayCommand(
                    param => newScenario()
                );
                return newScenarioCommand;
            }
        }
        public ICommand GetScenarioCommand
        {
            get
            {
                if(currentScenario == null)
                {
                    getScenarioCommand = new RelayCommand(
                        param => getScenario()
                    );
                }
                return getScenarioCommand;
            }
        }
        public ICommand SaveScenarioCommand
        {
            get
            {
                if(saveScenarioCommand == null)
                {
                    saveScenarioCommand = new RelayCommand(
                        param => saveScenario()
                    );
                }
                return saveScenarioCommand;
            }
        }
        public ICommand SelectWeatherCommand
        {
            get
            {
                selectWeatherCommand = new RelayCommand(
                    param => openFileDialog()
                );
                return selectWeatherCommand;
            }
        }
        #endregion // Public Properties/Commands

        #region 'structors
        public ScenarioViewModel(IScenarioReader scenarioReader)
        {
            this.reader = scenarioReader;
        }
        #endregion
        #region Private Helpers
        private void newScenario()
        {
            CurrentScenario = new ScenarioModel(new Scenario());
        }
        private void getScenario()
        {
            // Get ScenarioDto using ScenarioReader.TryParse()
            // Create new ScenarioModel(scenarioDto)
            // Set CurrentScenario

            Scenario s = reader.Parse();

            ScenarioModel sm = new ScenarioModel(s);

            CurrentScenario = sm;
        }
        private void saveScenario()
        {
            // Mocked for now, will use ScenarioProvider.Save()?
            string foo = CurrentScenario.StartDate.ToShortTimeString();
            string bar = CurrentScenario.EndDate.ToShortTimeString();
        }
        private void openFileDialog()
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg =
                new Microsoft.Win32.OpenFileDialog();

            // Set filter
            dlg.DefaultExt = ".UED";
            dlg.Filter = "UED Files (*.UED)|*.UED";

            // Display dialog
            Nullable<bool> result = dlg.ShowDialog();

            // Get selected file name and display
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                CurrentScenario.PathToWeatherFile = filename;
            }
        }
        #endregion // Private Helpers
    }
}