using System;
using System.IO;

namespace BookRecommender;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Application booted!");

        string path = "../../../data/books.csv";

        using (var reader = new StreamReader(path))
        {
	        while (!reader.EndOfStream)
	        {
		        string? line = reader.ReadLine();
		        Console.WriteLine(line);
	        }
        }
    }
}