namespace GenericsDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"{ExtLib.Rando(new List<double>(){1.2, 1.5 })}");
        }
    }
}
