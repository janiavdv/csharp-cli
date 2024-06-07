using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookRecommender;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Application booted!"); 
        
        const string path = "../../../data/books.json";
        List<JSONBook>? jsonData = new List<JSONBook>(); 
        
        using (StreamReader reader = new StreamReader(path))
        {
	        string json = reader.ReadToEnd();
            jsonData = JsonSerializer.Deserialize<List<JSONBook>>(json);;
            Console.WriteLine(jsonData.Count);
        }
    }
}