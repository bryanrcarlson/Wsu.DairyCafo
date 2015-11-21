using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.UI.PresentationLogic.Helper;

namespace Wsu.DairyCafo.UI.PresentationLogic.Model
{
    public class ScenarioModel : ObservableObject
    {
        #region Fields
        private string pathToWeatherFile;
        private DateTime startDate;
        private DateTime endDate;
        #endregion //Fields

        #region Properties
        public string PathToWeatherFile
        {
            get { return pathToWeatherFile; }
            set
            {
                if (value != pathToWeatherFile)
                {
                    pathToWeatherFile = value;
                    OnPropertyChanged("PathToWeatherFile");
                }
            }
        }
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    OnPropertyChanged("StartDate");
                }
            }
        }
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }
        #endregion // Properties
    }
}
