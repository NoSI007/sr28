using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SR28
{
    /// <summary>
    /// Interaction logic for List2.xaml
    /// </summary>
    public sealed partial class List2 : Window, IDisposable
    {

        SR28e db = null;
        short fg = -1;
        //Ext_Groups sg;//Hack change too.
        NUTR_DEF ndr;

        BackgroundWorker bw = new BackgroundWorker();

        object qryres = null;

        public int RecCount { get; set; }



        public List2()
        {
            InitializeComponent();

            //bw.DoWork += bw_DoWork;
            //bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            Loaded += List2_Loaded;
        }

        //Nutdef

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            //DisplayTheData();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            
        }

        private void dataFetchDone()
        {
            Ok.IsEnabled = true;
            SetHeaders();
            dataTable1ListView.DataContext = qryres;
            Hidep();
        }

        void List2_Loaded(object sender, RoutedEventArgs e)
        {
            db = App.db;
            // Load data into the table Fd_Grp. You can modify this code as needed.
            if (db == null)
            {
                MessageBox.Show("Null Data set");
                this.Close();
            }



            Group.DataContext = db.FD_GROUP.ToList();

            Nutdef.DataContext = db.NUTR_DEF.ToList<NUTR_DEF>();
            this.NutdefCBX.TextChanged += NutdefCBX_TextChanged;
        }

        void NutdefCBX_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typedword = NutdefCBX.Text;
            if (string.IsNullOrWhiteSpace(typedword) == true)
            {
                Nutdef.DataContext = db.NUTR_DEF.ToList<NUTR_DEF>();
                Nutdef.IsDropDownOpen = false;
                return;
            }

            List<NUTR_DEF> found = (from fi in db.NUTR_DEF
                                   where fi.NutrDesc.StartsWith(typedword)
                                   select fi).ToList<NUTR_DEF>();

            Nutdef.DataContext = found;
            Nutdef.IsDropDownOpen = true;
        }

   
        private async void Ok_Click(object sender, RoutedEventArgs e)
        {

            if (SetParam() == true)
            {
                SetVisible();
                Ok.IsEnabled = false;
                await DisplayTheData();
                this.UpdateLayout();
                dataFetchDone();
            }
            
        }

        private async Task DisplayTheData()
        {
            var sg = (FD_GROUP)Group.SelectedItem;
            fg = sg.FdGrp_CD;

            if (ndr == null || fg < 0)
                return;

            var foodds = db.FOOD_DES.Where(f => fg == 0 ? (true) : f.FdGrp_Cd == fg).ToList<FOOD_DES>();
            await Task.Delay(1);
            var dds = db.NUT_DATA.Where(d => d.Nutr_No == ndr.Nutr_No).ToList<NUT_DATA>();
            short dp = ndr.Num_Dec;
            //MessageBox.Show(string.Format("Foods = {0}    Data Records = {1}",foodds.Count(),dds.Count()));

            var qry = from sf in foodds
                      join fd in dds on sf.NDB_No equals fd.NDB_No
                      orderby fd.Nutr_Val descending
                      select new { Nutr_Val = string.Format(Comm.StrFormat[dp], fd.Nutr_Val), Desc = sf.Long_Desc };
            
            await Task.Delay(2);

            qryres = qry.ToList();
            RecCount = qry.Count();
        }

        private void SetHeaders()
        {
            Nutrient.Content = string.Format(" Containing {0} ", ndr.NutrDesc);
            Units.Content = String.Format("in ( {0} ) ", ndr.Units);
            FdGrp.Content = string.Format("{0}", db.FD_GROUP.Where(g=>g.FdGrp_CD == fg).First().FdGrp_Desc);
            Recf.Content = string.Format("{0} Records Found.", RecCount);
            dtgen.Content = string.Format("Content generated {0} - Local Time.", DateTime.Now.ToLocalTime());
        }

        private bool SetParam()
        {
            if (Group.SelectedItem == null || Nutdef.SelectedItem == null)
            {
                MessageBox.Show("The Comboboxes are not set!!!");
                return false;
            }

            ndr = Nutdef.SelectedItem as NUTR_DEF;

            if (ndr == null)
                return false;

            return true;
        }


        public void SetVisible()
        {
            zprog.Visibility = System.Windows.Visibility.Visible;
        }

        public void Hidep()
        {
            this.zprog.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void Dispose()
        {

            GC.SuppressFinalize(this);
        }

        TextBox NutdefCBX
        {
            get { return (TextBox)Nutdef.Template.FindName("PART_EditableTextBox", Nutdef); }
        }

    }
}
