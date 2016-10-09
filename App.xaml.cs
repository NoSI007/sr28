using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SR28
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SR28e db = new SR28e();
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);
        //}
    }
}
