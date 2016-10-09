using SR28.Reports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SR28
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
 

        List<FOOD_DES> food_res = null;
        List<FOOD_DES> _selected = new List<FOOD_DES>();

        public MainWindow()
        {
            InitializeComponent();
          
            this.Loaded += MainWindow_Loaded;
            //FooList.SelectionMode = SelectionMode.Multiple;
        }




        private async Task Loadndata()
        {
            
            await Task.Delay(1);
            //App.db.NUT_DATA.ToList();
            await Task.Delay(2);
           
        }

        async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CompareButtons(false);
            await Loadndata();
            Browse();
            CompareButtons(true);
        }


        private void CompareButtons(bool shw)
        {
            if (shw == false)
            {
                z_comp.Visibility = System.Windows.Visibility.Hidden;
                z_comp1.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                z_comp.Visibility = System.Windows.Visibility.Visible;
                z_comp1.Visibility = System.Windows.Visibility.Visible;
            }
        }




        private void z_PanelComp(object sender, RoutedEventArgs e)
        {
            PanComp();
        }


        bool _browse = true;

        private void grpList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grpList.SelectedItem != null)
            {
                short fgc = (short)grpList.SelectedValue;
                Title = fgc.ToString();

                if (_browse)
                {
                    var fl = App.db.FOOD_DES.Where(f => f.FdGrp_Cd == fgc);
                    FooList.DataContext = fl.ToList();
                    foolisting.Header = string.Format("Foods ( {0} )", fl.Count());
                }
                else
                {
                    var fl = food_res.Where(f => f.FdGrp_Cd == fgc);
                    FooList.DataContext = fl;
                    foolisting.Header = string.Format("Foods ( {0} )", fl.Count());
                }
            }
        }




        /// <summary>
        /// Browsing all groups/foods.
        /// </summary>
        private void Browse()
        {
            _browse = true;
            grpList.DataContext = App.db.FD_GROUP.ToList();
        }

        private void Search()
        {
            _browse = false;

            if (string.IsNullOrWhiteSpace(Wild.Text) == true)
            {
                Browse();
                return;
            }

            var _fs_res = App.db.FOOD_DES.Where(f => f.Long_Desc.Contains(Wild.Text));//HACK if nothing found return to browse.

            var gcd = (from f in _fs_res
                       select f.FdGrp_Cd).Distinct();

            var _gRes = App.db.FD_GROUP.Where(g => gcd.Contains(g.FdGrp_CD));

            grpList.DataContext = _gRes;
            food_res = _fs_res.ToList<FOOD_DES>();
        }



        /// <summary>
        /// store the selected food in the cmp listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void z_add_Click(object sender, RoutedEventArgs e)
        {
            AddItem2cmp();
        }
        /// <summary>
        /// remove an item from the cmp ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void z_del_Click(object sender, RoutedEventArgs e)
        {
            RemoveSelectedItem();
        }

        private void RemoveSelectedItem()
        {
            if (cmp.SelectedItem == null)
                return;

            ListBoxItem lbi = cmp.SelectedItem as ListBoxItem;
            FOOD_DES sr = (FOOD_DES)lbi.Tag;
            cmp.Items.Remove(lbi);

            UpdateCount();
        }

        private void UpdateCount()
        {
            z_cmp_count.Content = string.Format("{0} Items To Compare.", cmp.Items.Count);
        }
        /// <summary>
        /// Clear cmp and all TheResTab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void z_clear_Click(object sender, RoutedEventArgs e)
        {
            ClearWorkspace();
        }

        private void ClearWorkspace()
        {
            cmp.Items.Clear();
            UpdateCount();
        }
        /// <summary>
        /// Load all data from ACCES for the selected foods
        /// and display in datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void z_comp_Click(object sender, RoutedEventArgs e)
        {
            Compare();
        }

        //int round = 0;

        private void Compare()
        {
            if (TempList2pass())
            {

                SRTable res = new SRTable();

                res._selected = _selected;

                res.ShowDialog();
            }
            //round++;
        }

        private bool TempList2pass()
        {
            _selected.Clear();

            foreach (ListBoxItem lbi in cmp.Items)
            {
                _selected.Add((FOOD_DES)lbi.Tag);
            }
            return _selected.Count > 0;
           
        }





        private void PanComp()
        {
            if (TempList2pass())
            {
                PanelRes res = new PanelRes();

                res._selected = _selected;

                res.ShowDialog();
            }
        }

        private void z_go_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }



        private void AddItem2cmp()
        {
            foreach (var x in FooList.SelectedItems)
            {
                FOOD_DES fr = (FOOD_DES)x;
                if (fr == null)
                    return;
                Add2cmpList(fr);
            }

        }

        private void Add2cmpList(FOOD_DES fr)
        {
            foreach (ListBoxItem li in cmp.Items)
            {
                if (li.Tag.Equals(fr) == true)
                {
                    MessageBox.Show("Items already in comparison table");
                    return;
                }
            }
            ListBoxItem lbi = new ListBoxItem();
            lbi.Tag = fr;
            lbi.Content = fr.Long_Desc;
            cmp.Items.Add(lbi);
            //z_cmp_count.Content = string.Format("{0} Items To Compare.", cmp.Items.Count);
            UpdateCount();

          
        }

 
        private async Task RenderNuts4(int ndb)
        {
            var foodes = (from f in App.db.FOOD_DES where f.NDB_No == ndb select f).FirstOrDefault();
            if (foodes == null)
                return;

            List<NUT_DATA> temp = foodes.NUT_DATA.ToList();

            var res = (from v in temp
                      join n in App.db.NUTR_DEF.ToList() on v.Nutr_No equals n.Nutr_No
                      select new NutrVal { Value = v.Nutr_Val, Units = n.Units, Nutrient = n.NutrDesc }).ToList();

            await Task.Delay(1);

            Lv4Nuts.DataContext = res.ToList<NutrVal>();
            var foo = (from f in App.db.FOOD_DES.ToList()
                       where f.NDB_No == ndb
                       select f).Distinct().FirstOrDefault();
            await Task.Delay(2);
            BTHead.Text = string.Format("{0} Results Found for {1}", res.Count, foo.Long_Desc);
            await Task.Delay(3);
        }

        private async void z_show_click(object sender, RoutedEventArgs e)
        {
            ListBoxItem lbi = cmp.SelectedItem as ListBoxItem;
            if (cmp == null)
                return;

            FOOD_DES si = (FOOD_DES)lbi.Tag;
            if( si != null)
            {
                await RenderNuts4(si.NDB_No);
            }
 
      
        }

        private void z_list_click(object sender, RoutedEventArgs e)
        {
            List2 wpfForm = new List2();
            wpfForm.Topmost = true;
            wpfForm.ShowDialog();
            wpfForm.Dispose();

        }

       

        private void z_htmlRep_Click(object sender, RoutedEventArgs e)
        {
            if (TempList2pass() == true)
            {
                SR28.Reports.htmlRep Runrep = new Reports.htmlRep();
                Runrep._selected = _selected;
                Runrep.Show();
            }
    
        }
        /// <summary>
        /// make and show a FlowDocumentReader page
        /// for the comparison report.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void z_xamlRep_Click(object sender, RoutedEventArgs e)
        {
            if (TempList2pass() == true)
            {
                flowDoc qd = new flowDoc();
                //var x = qd.Uid;
                qd._selected = _selected;
                qd.ShowDialog();

            }
        }
    }
}
