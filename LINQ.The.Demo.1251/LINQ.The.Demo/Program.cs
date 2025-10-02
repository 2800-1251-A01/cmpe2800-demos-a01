namespace LINQ.The.Demo
{
  using static System.Console;
  internal class Program
  {
    static Random _rnd = new Random();
    static void Main(string[] args)
    {
      List<int> nums = new List<int>();

      while (nums.Count < 20)
        nums.Add(_rnd.Next(1, 20));

      {
        WriteLine("Minimal Case Use :");
        var result =
          from i in nums
          where i >= 10
          orderby i
          select i;

        foreach (var item in result)
          Write(item + ", ");
        WriteLine();
        WriteLine();
      }

      {
        WriteLine("Minimal Case Use with Multiple Where clause version 1 :");
        var result =
          from i in nums
          where i >= 10 && i % 2 == 0 // using && operator
          orderby i
          select i;

        foreach (var item in result)
          Write(item + ", ");
        WriteLine();
        WriteLine();
      }
      {
        WriteLine("Minimal Case Use with Multiple Where clause version 2 :");
        var result =
          from i in nums
          where i >= 10     // using multiple where clauses
          where i % 2 == 0
          orderby i
          select i;

        foreach (var item in result)
          Write(item + ", ");
        WriteLine();
        WriteLine();
      }

      {
        Random _rnd = new();
        WriteLine("Into intermediate required due to Select transform");
        var result = (
            from i in nums
            where i >= 10
            select i * _rnd.Next(1,3)
            into q // stash into temporary intermediate identifier q due to select transform

            where q % 2 == 0  // now restrict on intermediate result of q
            orderby q         // orderby on secondary result
            select q);

        foreach (var item in result)
          Write(item + ", ");
        //_rnd = null; Don't do it laddy !
        foreach (var item in result)
          Write(item + ", ");

        WriteLine();
        WriteLine();
      }
      {
        WriteLine("Multiple Where clause use via intermediate result, using Distinct and immediate execution  :");
        var result = (
            from i in nums
            where i >= 10
            select new { 
              X = i%2==0, 
              Y = i
            }
            into q // stash into temporary intermediate identifier q

            where q.X // now restrict on intermediate result of q
            orderby q
            select q).Distinct().ToList();  // no LINQ equivalent of Distinct so extension method is used
                                            // illustrates result being IEnumerable, thus Distinct-able
                                            // Final ToList() forces execution
                                            // Now in List so use ForEach()
        result.ForEach(num => Write(num + ", "));
        WriteLine();
        WriteLine();
        // Obviously many other variations exist
      }
      {
        WriteLine("Aggregate LINQ vs Extension :");
        var result =
          from i in nums
          where i >= 10
          orderby i descending // since ascending is the optional default
          select i;

        WriteLine(result.Take(1).First());
        WriteLine("vs");
        WriteLine(nums.Where(num => num >= 10).Max()); // LINQ not always the way to go...
        WriteLine();
        WriteLine();
      }
      {
        WriteLine("Anonymous Types can be created and used too:");
        var result =
          from i in nums
          where i >= 10
          orderby i
          select new { Prop_Num = i, Prop_Val = i * 1.05 }; // new { prop_name = value [,..] } rinse and repeat

        // Compiler/IDE is aware of anonymous type throughout the use of the result
        var resList = result.ToList(); // Saved, now variants of access..
        foreach (var item in resList)
          WriteLine(item.Prop_Num + " : " + item.Prop_Val); // items anonymous named properties are accessible
                                                            // OR
        WriteLine("Now with ForEach:");
        resList.ForEach(an_type => WriteLine(an_type.Prop_Num + " : " + an_type.Prop_Val));
        WriteLine();
        WriteLine();
      }
      {
        // OOP style DB Entity Joining
        List<Widget> widgets = new List<Widget>();
        widgets.Add(new Widget("Coke", 1, 20));
        widgets.Add(new Widget("Diamond", 42000, 1));
        widgets.Add(new Widget("Diet Coke", 1, 20));
        widgets.Add(new Widget("KitKat", 1.25f, 56));

        List<Supplier> sups = new List<Supplier>();
        sups.Add(new Supplier("The Coka-Cola Company", 20));
        sups.Add(new Supplier("Nestle", 56));
        sups.Add(new Supplier("Mother Nature", 1));

        {
          WriteLine("Simple Grouping :");
          var items = from w in widgets group w by w._Price;
          // items is a key/value pair, where value is a IEnumerable collection - like a Dictionary<GroupKey,List<item>>
          foreach (var gp in items)
          {
            WriteLine("Items with price : " + gp.Key.ToString("f2"));
            foreach (var item in gp)
              Console.WriteLine(" " + item._Name + " : " + item._Price.ToString("f2"));
          }
          WriteLine();
          WriteLine();
        }
        {
          WriteLine("Simple Grouping with temp intermediate for ordering :");

          var items = from w in widgets group w by w._Price into q orderby q.Key select q; // order by intermediate result

          // items is a key/value pair, where value is a IEnumerable collection - like a Dictionary<GroupKey,List<item>>
          foreach (var gp in items)
          {
            WriteLine("Items with price : " + gp.Key.ToString("f2"));
            foreach (var item in gp)
              Console.WriteLine(" " + item._Name + " : " + item._Price.ToString("f2"));
          }
          WriteLine();
          WriteLine();
        }
        {
          WriteLine("Simple Joining :");
          var items = from w in widgets
                      join s in sups on w._SupplierUNID equals s._UNID  // Note use of equals NOT ==
                      orderby w._Name
                      select new              // anonymous type multi-row result set
                      {
                        WidgetName = w._Name,
                        SupplierName = s._Name,
                        WidgetPrice = w._Price
                      };

          foreach (var v in items)            // As before, anonymous type result accessible
            WriteLine(
                v.WidgetName + " (" +
                v.SupplierName + ") @ $" +
                v.WidgetPrice.ToString("f2"));

          WriteLine(" ");
          WriteLine(" ");
        }
      }
      Console.ReadKey();
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