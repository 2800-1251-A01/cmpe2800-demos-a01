using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsDemo
{// Sample utility class
  public static class ExtLib
  {
    static Random _rnd = new Random(); // random helper
    /// <summary>
    /// Rando - return a random element from the supplied collection
    /// </summary>
    /// <typeparam name="T">collection type</typeparam>
    /// <param name="stuff">collection to select from</param>
    /// <returns>random element selected</returns>
    public static T Rando<T>( IEnumerable<T> stuff ) where T : notnull, new()
    {
      if (stuff.Count() == 0)
        throw new ArgumentException("Collection can't be empty");
      return stuff.ElementAt(_rnd.Next(stuff.Count() - 1));
    }
  }
}
