using GenericsDemo;
using System.Diagnostics;
namespace GenericsDemo_Test
{
  [TestClass]
  public sealed class Rando_Test
  {
    /// <summary>
    /// Empty Collection test
    /// </summary>
    [TestMethod]
    public void Empty()
    {
      ArgumentException err = Assert.ThrowsException<ArgumentException>(
        () => ExtLib.Rando(new List<bool>()) // ANY empty collection will do
      ); 
      Trace.WriteLine("Empty - Complete");
    }
    /// <summary>
    /// Verify return is somewhere in the collection
    /// </summary>
    [TestMethod]
    public void One()
    {
      double[] d = { 1.2, 4.5, 5.5 };
      CollectionAssert.Contains(d, ExtLib.Rando(d));
      //CollectionAssert.Contains(d, ExtLib.Rando(d) + 5); // FAIL version
    }
  }
  [TestClass]
  public sealed class Ditto_Test
  {
    [TestMethod]
    public void Empty()
    {
      Trace.WriteLine("Ditto_Test:Empty - Complete");
    }
    [TestMethod]
    public void One()
    {
      // Some algorithmic test, returns on success, but
      // if it falls through ( ie. element NOT found )
      // it will explicitly FAIL.
      Assert.Fail();
    }
  }
}
