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

namespace SR28.Reports
{
    /// <summary>
    /// Interaction logic for Qdoc.xaml
    /// </summary>
    public partial class Qdoc : Page
    {
        public List<FOOD_DES> _selected { get; set; }
        public Qdoc()
        {
            InitializeComponent();
        }
    }
}
