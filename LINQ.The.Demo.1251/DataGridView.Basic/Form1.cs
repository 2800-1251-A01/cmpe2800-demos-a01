namespace DataGridView.Basic
{
  public partial class Form1 : Form
  {
    BindingSource _bs = new();
    static Random _rnd = new();
    public Form1()
    {
      InitializeComponent();
      Load += (s, e) => _dgv.DataSource = _bs; // Bind the DataSource at startup
      _btnPopulate.Text = "POPULATE";
      _btnPopulate.Click += _btnPopulate_Click;
    }

    private void _btnPopulate_Click(object? sender, EventArgs e)
    {
      // generate some data
      List<int> iNums = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
      iNums.SequenceFill(10);
      var Squares = iNums.ToDictionary((k) => k, (k) => (int)Math.Pow(k, 2));

      // take first 5 values, create anon type with elements we want, and apply to datasource
      _bs.DataSource = from kvp in Squares.Take(5)
                       select new
                       {
                         Number = kvp.Key,                       // original number
                         Square = kvp.Value.ToString("f2"),      // its square
                         Random = _rnd.Next(kvp.Key, kvp.Value)  // manufactured column
                       };
      // Set last column to consume all the leftover space, looks nicer !
      _dgv.Columns[^1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      // change the column name in the UI (can use more complex names than annon type name)
      _dgv.Columns[0].HeaderText = "Base Number";
    }
  }
  public static class Utils
  {
    public static List<int> SequenceFill(this List<int> lst, int num)
    {
      lst.Clear();
      int incr = 0;
      while (lst.Count < num)
        lst.Add(incr++);
      return lst;
    }
  }
}
