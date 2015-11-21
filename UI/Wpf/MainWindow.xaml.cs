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
using Wsu.DairyCafo.DataAccess.Dto;

namespace Wsu.DairyCafo.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Scenario scenario;

        public MainWindow()
        {
            InitializeComponent();

           //scenario = new Scenario();
           //scenario.StartDate = DateTime.Now;
           //scenario.EndDate = DateTime.Now;
           //scenario.PathToWeatherFile = @"C:\dev";
           //
           //scenario.Cow = new Cow
           //{
           //    BodyMass_kg = 500,
           //    CrudeProteinDiet_kg_d = 10,
           //    DryMatterIntake_kg_d = 40,
           //    MilkProduction_kg_d = 15
           //};
           //
           //DataContext = scenario;
        }

        private void btnScenarioSelectWeather_Click(object sender, RoutedEventArgs e)
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
            if(result == true)
            {
                // Open document
                string filename = dlg.FileName;
                txtScenarioWeatherFile.Text = filename;
            }
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mnuSave_Click(object sender, RoutedEventArgs e)
        {
            string foo = txtCowBodyMass.Text;
            //double moo = this.scenario.Cow.BodyMass_kg;
        }
    }
}
