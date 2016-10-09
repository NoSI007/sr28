using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace SR28
{
    /// <summary>
    /// Interaction logic for PanelRes.xaml
    /// </summary>
    public partial class PanelRes : Window
    {

        public List<FOOD_DES> _selected { get; set; }

        BackgroundWorker bw = new BackgroundWorker();

        DataTable PanRes = new DataTable("PanelResultsTable");

        List<NUT_DATA> Tempres = new List<NUT_DATA>();


        public PanelRes()
        {
            InitializeComponent();
            MakeDataTable();
            this.Loaded += PanelRes_Loaded;

        }

        /// <summary>
        /// Make and load the datatable for required foods.
        /// </summary>
        private void MakeDataTable()
        {

            DataColumn c1 = new DataColumn("Values", typeof(string));
            DataColumn c2 = new DataColumn("Foods", typeof(string));
            PanRes.Columns.Add(c1);
            PanRes.Columns.Add(c2);
            

        }

        void PanelRes_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selected == null || _selected.Count <= 0)
                {
                    this.Close();
                }

                if (App.db == null || _selected == null)
                {
                    MessageBox.Show("Required Program data error");
                    this.Close();
                }

              
                InitStart();
                ResGrid.ItemsSource = PanRes.DefaultView;
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }


        private async void InitStart()
        {
    
            var lst = await Nuts4SelFoo.fetch(_selected);
            Tempres.Clear();
            Tempres.AddRange(lst);
          
            LeftPanel();
       
            done();
        }

        private void LeftPanel()
        {
            ResList.DataContext = App.db.NUTR_DEF.ToList<NUTR_DEF>();

            ResListHead.Content = string.Format("( {0} ) Nutrients", App.db.NUTR_DEF.Count());
            ResList.Visibility = System.Windows.Visibility.Hidden;
            PageHead.Visibility = System.Windows.Visibility.Hidden;
        }

        #region background
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {

            Tempres.Clear();
            try
            {
                //int[] ndb = (from f in _selected select f.NDB_No).ToArray();
                //Array.Sort<int>(ndb);
                //Tempres = App.db.NUT_DATA.Where(f => Array.BinarySearch<int>(ndb,f.NDB_No) >= 0).ToList();
                //foreach (NUT_DATA n in App.db.NUT_DATA.ToList())
                //{
                //    if (Array.BinarySearch<int>(ndb, n.NDB_No) >= 0)
                //    {
                //        Tempres.Add(n);
                //    }
                //}
                foreach (FOOD_DES f in _selected)
                {
                    var xc = f.NUT_DATA.ToList();
                    Tempres.AddRange(xc);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }



        private void done()
        {
            zprog.Visibility = System.Windows.Visibility.Hidden;
            ResList.Visibility = System.Windows.Visibility.Visible;
            PageHead.Visibility = System.Windows.Visibility.Visible;
        }

        #endregion background




        #region UI
        private void ResList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (App.db.NUTR_DEF.Count() > 0)
            {
                try
                {
                    LoadRdg();
                }
                catch (System.Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        }

        private void LoadRdg()
        {
            if (Tempres == null || Tempres.Count <= 0)
                return;

            NUTR_DEF nd = (NUTR_DEF)ResList.SelectedItem;
            if (nd == null )
                return;

            short decp = nd.Num_Dec;

            var tisqry = Tempres.Where(n => n.Nutr_No == nd.Nutr_No).ToList<NUT_DATA>();


            string selection = string.Format("{0} Records found for {1}.", tisqry.Count(), nd.NutrDesc);
            ResGridHead.Content = selection;

            PanRes.Rows.Clear();


            ResGrid.ItemsSource = null;

            if (tisqry.Count() <= 0)
            {
                return;
            }
            //StringBuilder sss = new StringBuilder();

            foreach (NUT_DATA drow in tisqry)
            {
                DataRow d0 = PanRes.NewRow();
                float x = (float)drow.Nutr_Val;
                string rx = string.Format(Helper.StrFormat(decp), x);
                d0[0] = rx;
                string des = _selected.Where(f => f.NDB_No == drow.NDB_No).First().Long_Desc;
                d0[1] = des;
                PanRes.Rows.Add(d0);
                //sss.AppendLine(string.Format("NDB = {0}   {1}   {2} ", drow.NDB_No, rx, des));
            }
            //MessageBox.Show(sss.ToString());
            ResGrid.ItemsSource = PanRes.DefaultView;
        }

        #endregion UI

    }
}
