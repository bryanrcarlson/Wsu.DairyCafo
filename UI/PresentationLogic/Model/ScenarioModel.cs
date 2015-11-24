using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Dto;
using Wsu.DairyCafo.UI.PresentationLogic.Helper;

namespace Wsu.DairyCafo.UI.PresentationLogic.Model
{
    public class ScenarioModel : ObservableObject
    {
        #region Fields
        //       private string pathToWeatherFile;
        //       private DateTime startDate;
        //       private DateTime endDate;
        private Scenario scenario;
        #endregion //Fields

        #region Properties
        public string PathToWeatherFile
        {
            get { return scenario.PathToWeatherFile; }
            set
            {
                if (value != scenario.PathToWeatherFile)
                {
                    scenario.PathToWeatherFile = value;
                    OnPropertyChanged("PathToWeatherFile");
                }
            }
        }
        public DateTime StartDate
        {
            get { return scenario.StartDate; }
            set
            {
                if (value != scenario.StartDate)
                {
                    scenario.StartDate = value;
                    OnPropertyChanged("StartDate");
                }
            }
        }
        public DateTime EndDate
        {
            get { return scenario.EndDate; }
            set
            {
                if (value != scenario.EndDate)
                {
                    scenario.EndDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }
        //== Barn
        //public double NumberCows
        //{
        //    get { return scenario.Barn.Number_cows_cnt; }
        //    set
        //    {
        //        if (value != scenario.Barn.Number_cows_cnt)
        //        {
        //            scenario.Barn.Number_cows_cnt = value;
        //            OnPropertyChanged("NumberCows");
        //        }
        //    }
        //}
        #endregion // Properties
        #region 'structors
        public ScenarioModel(Scenario scenario)
        {
            this.scenario = scenario;
        }
        #endregion
    }
}
