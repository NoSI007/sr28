using SR28;
using System.Collections.Generic;


public static class Helper
{
    public static List<FOOD_DES> cmp = new List<FOOD_DES>();
    public static string StrFormat(int dec)
    {
        string strfmt = null;

        switch (dec)
        {
            case 1:
                strfmt = "{0,14:F1}";
                break;
            case 2:
                strfmt = "{0,14:F2}";
                break;
            case 3:
                strfmt = "{0,14:F3}";
                break;
            default:
                strfmt = "{0,14:F0}";
                break;

        }
        return strfmt;
    }
}


public static class Comm
{
    const string F0 = "{0:F0}";
    const string F1 = "{0:F1}";
    const string F2 = "{0:F2}";
    const string F3 = "{0:F3}";

    public static string[] StrFormat = { F0, F1, F2, F3 };


}

public class NutrVal
{
    public NutrVal()
    {

    }
    private float? _val;

    public float? Value
    {
        get { return _val; }
        set { _val = value; }
    }

    private string _units;

    public string Units
    {
        get { return _units; }
        set { _units = value; }
    }

    public string Nutrient { get; set; }

}