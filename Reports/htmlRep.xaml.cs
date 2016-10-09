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
using System.Windows.Shapes;

namespace SR28.Reports
{
    /// <summary>
    /// Interaction logic for htmlRep.xaml
    /// </summary>
    public partial class htmlRep : Window
    {
        public List<FOOD_DES> _selected { get; set; }

        public htmlRep()
        {
            InitializeComponent();
            Loaded += htmlRep_Loaded;
        }

        async void  htmlRep_Loaded(object sender, RoutedEventArgs e)
        {
             if( _selected.Count <= 0 )
             {
                 return ;
             }
             
             await Nuts4SelFoo.fetch(_selected);

             if (Nuts4SelFoo.ReportData().Count > 0)
             {
                 try
                 {
                     htmlReport htmrep = new htmlReport();
                     string rep = htmrep.TransformText();
                     //MessageBox.Show(rep);
                     Clipboard.SetText(rep);
                     web.NavigateToString(rep);
                 }
                 catch (System.Exception a)
                 {
                     MessageBox.Show(a.Message);
                 }
        
             }
        }
    }
}
