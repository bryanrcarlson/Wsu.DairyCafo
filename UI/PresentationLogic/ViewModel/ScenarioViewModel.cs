using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wsu.DairyCafo.UI.PresentationLogic.Helper;
using Wsu.DairyCafo.UI.PresentationLogic.Model;

namespace Wsu.DairyCafo.UI.PresentationLogic.ViewModel
{
    public class ScenarioViewModel : ObservableObject
    {
        #region Fields
        private ScenarioModel currentScenario;
        private ICommand newScenarioCommand;
        private ICommand getScenarioCommand;
        private ICommand selectWeatherCommand;

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

        #region Private Helpers
        private void newScenario()
        {
            CurrentScenario = new ScenarioModel();
        }
        private void getScenario()
        {
            // Mocked for now, will use ScenarioProvider
            ScenarioModel s = new ScenarioModel();
            s.PathToWeatherFile = "C:\\";
            s.StartDate = DateTime.Now;
            s.EndDate = DateTime.Now;

            CurrentScenario = s;
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