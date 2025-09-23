// demo of basic grouping, with extension, not specific LINQ syntax

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DemoGrouping
{
    internal class Program
    {
        // for generating more interesting test data
        private static Random _rnd = new Random();

        static void Main(string[] args)
        {
            // simple data for understanding grouping!
            string[] stringdata = { "cat", "bar", "but", "cat", "cut", "car", "dog" };

            // group by the first letter of the word
            IEnumerable<IGrouping<char, string>> stgroups = stringdata.GroupBy(w => w.First());

            // note that what we get back from this is scary, but, if you look at the 'spirit' of
            //  what we asked for, the details of this is not something we really need to worry about
            // I asked for the words to be grouped by first letter, so the groups will be words that
            //  have the same letter; that's the grouping IEnumerable.
            // Interestingly, each group will have a Key field that indicates the name of the group
            //  because I said w=>w.First(), this will be the first of a string, so a char that is the 
            //  group key.
            // Very interestingly, the group itself is IEnumerable, so I can iterate it to see the 
            //  items in the group; in this case, strings that fall in that grouping
            // Because of the way this is operated, you may not look at the return type at all!
            // This is also a case (a weak one) for using var, as it saves a lot of typing,
            //  but the argument could be made that a change to the grouping/ordering would radically
            //  change the return type, so var would provide immunity to that

            // move through each grouping
            foreach (IGrouping<char, string> group in stgroups)
            {
                // look at the key for this group
                Console.Write ($"{group.Key} : ");

                // look at the items in this group
                foreach (string word in group)
                    Console.Write ($"{word} ");
                Console.WriteLine();

                // easier:
                //Console.WriteLine(string.Join(" ", group));
            }

            //    c: cat cat cut car
            //    b: bar but
            //    d: dog

            // ordering may be applied as well, taking note of which ordering clause orders what
            var stgroupsord = stringdata
                .OrderBy (w=>w)                 // ascending words in the groups
                .GroupBy(w => w.First())        // group by first letter of words
                .OrderByDescending(g=>g.Key);   // descending group keys

            // show key and grouping
            foreach (var group in stgroupsord)
                Console.WriteLine ($"{group.Key} : {string.Join(" ", group)}");

            // d: dog
            // c: car cat cat cut
            // b: bar but

            // sometimes the simplest groupings can be the most complicated
            Console.WriteLine (string.Join(" ",
                "There are four lights!"
                .Where (c => !char.IsWhiteSpace (c))
                .GroupBy(c => c)
                .Select (g => new string(g.ToArray()))));
            // T hh eee rrr a f o u l i g t s !
            // What am I looking at here?

            Console.ReadKey();
        }
    }
}
