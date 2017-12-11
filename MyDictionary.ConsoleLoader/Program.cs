using System;

namespace MyDictionary.ConsoleLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            WordsLoader.Loader.Load("slowa.txt");
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
