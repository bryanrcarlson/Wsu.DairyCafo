using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Wsu.DairyCafo.DataAccess.Core;
using Wsu.DairyCafo.DataAccess.Dto;
using Wsu.DairyCafo.UI.PresentationLogic.Helper;
using Wsu.DairyCafo.UI.PresentationLogic.Model;
using System.Windows.Forms;
using Wsu.IO.Core;
using Wsu.IO.Runner;
using System.IO;
using System.Windows.Data;

namespace Wsu.DairyCafo.UI.PresentationLogic.ViewModel
{
    public class ScenarioViewModel : ObservableObject
    {
        #region Fields
        private readonly IScenarioReader reader;
        private readonly IScenarioWriter writer;
        private ScenarioModel currentScenario;
        private ICommand newScenarioCommand;
        private ICommand getScenarioCommand;
        private ICommand selectWeatherCommand;
        private ICommand saveScenarioCommand;
        private ICommand runScenarioCommand;
        private DirectoryInfo currentWorkingDir;    //TODO: Clear/update this when appropiate
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
                    param => browseWeatherFileDialog()
                );
                return selectWeatherCommand;
            }
        }
        public ICommand RunScenarioCommand
        {
            get
            {
                runScenarioCommand = new RelayCommand(
                    param => runScenario()
                );
                return runScenarioCommand;
            }
        }
        #endregion // Public Properties/Commands

        #region 'structors
        public ScenarioViewModel(
            IScenarioReader scenarioReader,
            IScenarioWriter scenarioWriter)
        {
            this.reader = scenarioReader;
            this.writer = scenarioWriter;
        }
        #endregion
        #region Private Helpers
        private void newScenario()
        {
            string filePath = newScenarioDialog();
            if(!String.IsNullOrEmpty(filePath))
            {
                currentWorkingDir = new DirectoryInfo(Path.GetDirectoryName(filePath));

                CurrentScenario = new ScenarioModel(
                    new DefaultScenario().s);

                // Create directory structure
                try
                {
                    writer.SetupDir(filePath);
                }
                catch(InvalidOperationException e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }

                // Create new scenario files and load
                try
                {
                    writer.Write(CurrentScenario.GetScenario(), filePath);
                }
                catch(InvalidOperationException e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            }
        }
        private void getScenario()
        {
            // Get ScenarioDto using ScenarioReader.TryParse()
            // Create new ScenarioModel(scenarioDto)
            // Set CurrentScenario

            string sFile = loadScenarioFileDialog();
            if(!String.IsNullOrEmpty(sFile))
            {
                currentWorkingDir = new DirectoryInfo(Path.GetDirectoryName(sFile));
                try
                {
                    reader.Load(sFile);
                }
                catch(NullReferenceException e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }

                Scenario s = reader.Parse();

                ScenarioModel sm = new ScenarioModel(s);

                CurrentScenario = sm;
            } 
        }
        private void saveScenario()
        {
            try
            {
                writer.Write(this.currentScenario.GetScenario());
                if(currentScenario.FieldEnabled)
                    writer.WriteField(this.currentScenario.GetScenario());
                System.Windows.MessageBox.Show("File saved.");
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }
        private void runScenario()
        {
            if (currentWorkingDir == null
                || String.IsNullOrEmpty(currentWorkingDir.ToString()))
                throw new NullReferenceException("No scenario loaded");

            FileInfo program = new FileInfo(@"C:\Program Files (x86)\CS_suite_4\NIFA\dairy\scenario_run.exe");
            string arguments = "\"" + currentWorkingDir.ToString() + "\"";
            if (!currentScenario.FieldEnabled)
                arguments += " -f";

            var runner = new ExeRunner(program);

            int exitCode;
            
            runner.TryExecute(arguments, out exitCode);
        }
        private void browseWeatherFileDialog()
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
        private string newScenarioDialog()
        {
            using(FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select a folder";
                if(fbd.ShowDialog() == DialogResult.OK)
                {
                    return fbd.SelectedPath;
                }
            }

            // Did not select a dir
            return null;
        }
        private string loadScenarioFileDialog()
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg =
                new Microsoft.Win32.OpenFileDialog();

            // Set filter
            dlg.DefaultExt = ".NIFA_dairy_scenario";
            dlg.Filter = "Scenario Files (*.NIFA_dairy_scenario)|*.NIFA_dairy_scenario";

            // Display dialog
            Nullable<bool> result = dlg.ShowDialog();

            // Get selected file name and display
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                return filename;
            }
            else { return null; }
        }
        #endregion // Private Helpers
    }
}