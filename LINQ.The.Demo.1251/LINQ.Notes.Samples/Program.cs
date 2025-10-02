namespace LINQ.Notes.Samples
{
  internal class Program
  {
    static Random _rnd = new Random();
    static void Main(string[] args)
    {
      {
        List<int> nums = new List<int>();

        while (nums.Count < 20)
          nums.Add(_rnd.Next(1, 20));

        Console.WriteLine(" ");
        Console.WriteLine(" ");

        var result = (
            from i in nums
            where i >= 10
            select i into q

            where q % 2 == 0
            orderby q
            select q).ToList().Distinct();

        foreach (var s in result)
          Console.WriteLine(s.ToString());

        Console.WriteLine(" res2 ");
        var res2 = from i in nums where i >= 10 where i % 2 == 0 orderby i select i;
        var res3 = from i in nums where i >= 10 && i % 2 == 0 orderby i select i;
        foreach (var s in res2.Distinct()) Console.WriteLine(s.ToString());

        // Console.WriteLine(" ");
        Console.WriteLine(" ");
        Console.WriteLine(" ");


        List<Widget> widgets = new List<Widget>();
        widgets.Add(new Widget("Coke", 1, 20));
        widgets.Add(new Widget("Diamond", 42000, 1));
        widgets.Add(new Widget("Diet Coke", 1, 20));
        widgets.Add(new Widget("KitKat", 1.25f, 56));

        List<Supplier> sups = new List<Supplier>();
        sups.Add(new Supplier("The Coka-Cola Company", 20));
        sups.Add(new Supplier("Nestle", 56));
        sups.Add(new Supplier("Mother Nature", 1));

        var items = from w in widgets
                    join s in sups on w._SupplierUNID equals s._UNID
                    orderby w._Name
                    select new
                    {
                      WidgetName = w._Name,
                      SupplierName = s._Name,
                      WidgetPrice = w._Price
                    };

        foreach (var v in items)
          Console.WriteLine(
              v.WidgetName + " (" +
              v.SupplierName + ") @ $" +
              v.WidgetPrice.ToString("f2"));

        Console.WriteLine(" ");
        Console.WriteLine(" ");
      }

      //int[] stuff = new int[] { 3, 5, 4, 0, 17, 21, 44 };

      //var result = from n in stuff where n > 0 select new { Price = n, GST = n * 0.05 };

      //result.ToList().ForEach((o) => Console.WriteLine(
      //    o.Price.ToString ("f2") + ", " + o.GST.ToString("f2")));

      //Console.WriteLine(" ");
      //Console.WriteLine(" ");


      //(from n in nums where n % 2 == 0 orderby n select n).
      //    Distinct().
      //    Take(5).
      //    ToList().
      //    ForEach ((o) => Console.Write(o.ToString() + " "));

      {
        var stuff = new List<string>() { "cat", "bat", "sat", "hat", "mat", "cat", "MAT", "hAt" };
        
        var result = from str in stuff
                     let label = char.ToUpper(str.First()) + str.Substring(1).ToLower()
                     orderby label ascending
                     select new
                     {
                       Label = label,
                       Original = str,
                       Sum = str.ToLower().Sum(c => c)
                     }
                into projected
                     orderby projected.Sum
                     // From NOTES but not required
                     //select projected 
                     //into orderedprojected
                     //group orderedprojected by orderedprojected.Label;
                     group projected by projected.Label;

        foreach (var item in result)
        {
          Console.WriteLine($"Category : {item.Key}: ");
          Console.WriteLine(string.Join(", ", item));
        }

      }
    }
  }

  public class Widget
  {
    public string _Name;
    public float _Price;
    public int _SupplierUNID;

    public Widget(string name, float price, int supplierUNID)
    {
      _Name = name.Trim();
      _Price = price;
      _SupplierUNID = supplierUNID;
    }
  }

  public class Supplier
  {
    public string _Name;
    public int _UNID;

    public Supplier(string name, int UNID)
    {
      _Name = name;
      _UNID = UNID;
    }
  }
}