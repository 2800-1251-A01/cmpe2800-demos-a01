// select can change the collection type:
{
  List<int> vals = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

  // convert int to calculated doubles (collection type on return is deduced from expression)
  List<double> projection = vals.Select(i => i * 1.5).ToList();
}

// select can also (weirdly) filter values, kind of, by projecting a part of the source
{
  List<string> vals = new List<string>(new string[] { "Nothing", "is", "a", "test" });

  foreach (char c in vals.Select(s => s.First()).OrderBy(c => c))
    Console.Write(c);
  Console.WriteLine(); // produces 'Nait'
}

// use select to show the smallest letter in each word
{
  List<string> vals = new List<string>(new string[] { "Nothing", "is", "a", "test" });

  foreach (char c in vals.Select(s => s.Min()))
    Console.Write(c);
  Console.WriteLine(); // produces 'Niae'
}

// use select to show the largest of the smallest letters in all words
{
  List<string> vals = new List<string>(new string[] { "Nothing", "is", "a", "test" });

  Console.WriteLine(vals.Select(s => s.Min()).Max()); // produces 'i' from N, i, a, e
}

// SelectMany will project, but 'flatten' collections into a single collection
{
  List<string> vals = new List<string>(new string[] { "Nothing", "is", "a", "test" });

  // give all characters from all words where the characters are less than 'h'
  foreach (char c in vals.SelectMany(s => s.Where(c => c <= 'g')))
    Console.Write(c);
  Console.WriteLine(); // produces 'Ngae'

  // this is the collection of Ng from Nothing
  //                           a  from a
  //                           e  from test
  // combined into a single collection (flattened)
}
{
  var NestedLists = new List<List<string>>
        {
          new List<string>{ "Oh", "On" },
          new List<string>{ "To", "Ta", "Tu"},
          new List<string>{ "Ba", "Bo", "Bu", "By", "Be"},
          new List<string>{ "Me", "Mo", "Mu", "My"},
        };
  Console.WriteLine("A:");
  // SelectMany will flatten the nested lists into a single collection, can iterate over it
  foreach (string item in NestedLists.SelectMany(listOstr => listOstr))
  {
    Console.Write($"{item}, ");
  }
  // or nicer yet, use string.Join to concatenate the strings
  Console.WriteLine("B:");
  Console.WriteLine(string.Join(", ", NestedLists.SelectMany(listOstr => listOstr)));
  Console.WriteLine("\n");
  // Note Select() can do a similar thing, instead of flattening, it will project,
  //   making a collection of collections.. 
  Console.WriteLine("C:");
  Console.WriteLine(string.Join(", ", NestedLists.Select(listOstr => listOstr.ToString())));
  Console.WriteLine("\n");

  // or with selector, adding a transform function operating against each item flattend out.
  //   In this case, the tranform function is a lambda accepting the outer list and the inner list
  //   still returning a single element (a string in this case), but both outer/inner are available.
  Console.WriteLine(
    string.Join(":: ",
    NestedLists.SelectMany(listOstr => listOstr, (outer, inner) => $"[{outer.Count}:{inner}]")));
  Console.WriteLine("\n");
}

// SelectMany may also apply a transform to the intermediate results
{
  List<string> vals = new List<string>(new string[] { "Nothing", "is", "a", "test" });

  // give all characters from all words where the characters are less than 'h'
  foreach (char c in vals.SelectMany(s => s.Where(c => c <= 'g'), (_, c) => char.ToUpper(c)))
    Console.Write(c);
  Console.WriteLine(); // produces 'NGAE'

  // this is the collection of Ng from Nothing
  //                           a  from a
  //                           e  from test
  // combined into a single collection (flattened)
  // each result item uppercased
}

// similar to ICA #03 (but not exactly the same)
Dictionary<char, List<string>> dic = new Dictionary<char, List<string>>();
dic['a'] = new List<string>() { "apple", "again", "almost" };
dic['s'] = new List<string>() { "sugar", "some", "sweet" };

// although the where criteria is different in the ica, the mechancis are:
//  select many on a dictionary means the range variable is a keyvaluepair
//  you can look at the key or the value (or both) in the kvp to select what
//  type will be used to project the sequence. In this case, the value is used
//  (the value here being a list of string)
//  If a where clause is used on the collection of string, then the range variable
//  in that expression is a string (representing each string in the collection).
//  The return from this will be IEnumerable<string>, so that determines the projection
//  type for the SelectMany call. You would be pulling 0 or more strings from
//  each List<string> in the KVP, and SelectMany flattens the results from each
//  into a single collection (IEnumerable<string> from many IEnumerable<string>)
foreach (string s in dic.SelectMany(kvp => kvp.Value.Where(s => s.Contains('a'))))
{
  Console.WriteLine(s);
  // produces apple, again, almost, sugar (a flat collection of words that contain an 'a')
  //  from many collections of words
}

Console.ReadKey();
