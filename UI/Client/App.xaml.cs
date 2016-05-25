using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Wsu.DairyCafo.DataAccess;
using Wsu.DairyCafo.DataAccess.Core;
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

            Container container = new Container();
            container.ResolveMainWindow(e.Args).Show();
        }
    }
}
