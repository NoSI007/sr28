using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Windows;

namespace SR28
{

/// <summary>
/// place holder for T4 HTML report
/// </summary>
public class Nutr4Foo
{
    private int type;// 0 nutrient 1 food

    public int Type
    {
        get { return type; }
        set { type = value; }
    }
    private short dec;

    public short Dec
    {
        get { return dec; }
        set { dec = value; }
    }
    
    private string unit;// nutrient Units

    public string Unit
    {
        get { return unit; }
        set { unit = value; }
    }

    private string name;// either nutrient name /or/ food long_des

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    private float val;// the nutrient values in the food.

    public float Val
    {
        get { return val; }
        set { val = value; }
    }
        
        
}
public class Nuts4SelFoo
    {
        public static List<NUT_DATA> Tempres = new List<NUT_DATA>();
        public static List<Nutr4Foo> repList = new List<Nutr4Foo>();
        public async static Task<List<NUT_DATA>> fetch(List<FOOD_DES> _selected)
        {
            await Task.Delay(1);
            Tempres.Clear();
            try
            {
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

            return Tempres;
        }

        public  static List<Nutr4Foo> ReportData()
        {
            var groups = from t in Tempres
                         group t by t.NUTR_DEF into xyz
                         orderby xyz.Key.Nutr_No
                         select new { nutrient = xyz.Key, foo = xyz };

            repList.Clear();

            List<Nutr4Foo> temp = new List<Nutr4Foo>();

            foreach (var g in groups)
            {
                foreach (var f in g.foo)
                {
                    if (f.Nutr_Val != null && f.Nutr_Val > 0f)
                    {
                        Nutr4Foo n4f = new Nutr4Foo();
                        n4f.Name = f.FOOD_DES.Long_Desc;
                        n4f.Val = (float)f.Nutr_Val;
                        n4f.Type = 1;
                        n4f.Dec = g.nutrient.Num_Dec;
                        temp.Add(n4f);
                    }
                }

                if (temp.Count() > 0)
                {
                    Nutr4Foo nut = new Nutr4Foo();
                    nut.Name = g.nutrient.NutrDesc;
                    nut.Unit = g.nutrient.Units;

                    nut.Type = 0;

                    repList.Add(nut);
                    repList.AddRange(temp);
                }
                // AS I have added the temp as a range 
                temp.Clear();

                
            }

            return repList;

        }


        private void xtt()
        {
            
        }
    }
}