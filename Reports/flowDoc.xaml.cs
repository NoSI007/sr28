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
    /// Interaction logic for flowDoc.xaml
    /// </summary>
    public partial class flowDoc : Window
    {
        public List<FOOD_DES> _selected { get; set; }
        List<Nutr4Foo> repList = null;

        StringBuilder tab = new StringBuilder();

        Table qfoo = null;
        TableRow or = null;
        TableRowGroup rg0 = null;
        private int cfoods=0;

        public flowDoc()
        {
            InitializeComponent();
            Loaded += flowDoc_Loaded;
        }

        async void flowDoc_Loaded(object sender, RoutedEventArgs e)
        {
            await Nuts4SelFoo.fetch(_selected);

            if (Nuts4SelFoo.ReportData().Count > 0)
            {
                try
                {
                    repList = Nuts4SelFoo.repList;
                }
                catch (System.Exception a)
                {
                    MessageBox.Show(a.Message);
                }

            }

            Makedoc();
            cfoods = _selected.Count;
            hlab.Content = string.Format("<  Comparing ({0}) Foods.  >", cfoods);
        }

        int frc = 0;
        private void Makedoc()
        {
            // make the table ready.
            qfoo = this.T1;
            qfoo.RowGroups.Add(new TableRowGroup());
            rg0 = qfoo.RowGroups[0];
            docv.Background = Brushes.White;

            foreach (Nutr4Foo r in repList)
            {
                if (r.Type == 0)
                {
                    Foodrow(r);
                }
                else
                {
                    Nutrow(r);
                    frc++;
                }
            }
            //this.Title = T1.RowGroups[0].Rows.Count.ToString();

        }

        private void Nutrow(Nutr4Foo r)
        {
            float val = r.Val;
            string fmt = Helper.StrFormat(r.Dec);
            string nutrient = r.Name;
            AddRow(string.Format(fmt,val), nutrient, 1);
        }



        private void Foodrow(Nutr4Foo r)
        {
            string food = r.Name;
            AddRow(r.Unit, food, 0);
        }

        private void AddRow(string units, string des, int p)
        {
            foorow = p;

            Run r0 = new Run(units);
            Paragraph p1 = new Paragraph(r0);
            InitPara(p1, 0);


            Paragraph p2 = new Paragraph(new Run(des));
            InitPara(p2, 1);
            or = new TableRow();

            or.Cells.Add(new TableCell(p1));
            or.Cells.Add(new TableCell(p2));
            or.Cells[0].LineHeight = 28;
            or.Cells[1].LineHeight = 28;
            rg0.Rows.Add(or);
        }

        int foorow = 0;


        private void InitPara(Paragraph t, int pos)
        {

            t.FontSize = 12;
            t.Padding = new Thickness(5, 5, 5, 5);
            if (foorow == 0)
            {
                t.BorderBrush = Brushes.Coral;
                t.BorderThickness = new Thickness(1);

                if (pos == 0)
                {
                    t.TextAlignment = TextAlignment.Right;
                }
                else
                {
                    t.TextAlignment = TextAlignment.Left;
                }

                t.FontWeight = FontWeights.Bold;
                t.Background = Brushes.Silver;
                t.Foreground = Brushes.Blue;
                t.FontSize = 14;
                t.LineHeight = 28;
            }
            else
            {
                t.FontWeight = FontWeights.DemiBold;
                t.Foreground = Brushes.Black;
                t.TextIndent = 5;

                if (pos == 0)
                {
                    t.TextAlignment = TextAlignment.Right;
                }
                else
                {
                    t.TextAlignment = TextAlignment.Left;
                    if (frc % 2 == 0)
                        t.Background = Brushes.White;
                    else
                        t.Background = Brushes.Yellow;
                }
                //frc++;
            }

        }
    }
}
