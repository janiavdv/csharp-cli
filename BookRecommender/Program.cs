﻿using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;

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
            jsonData = JsonSerializer.Deserialize<List<JSONBook>>(json);
            if (jsonData == null)
                System.Environment.Exit(0);
            Console.WriteLine(jsonData.Count);
        }

        List<Book> books = new List<Book>();
        foreach (JSONBook rawData in jsonData)
        {
            Book book = new Book(rawData);
            books.Add(book);
        }
        
        // ensure all books were successfully converted
        Debug.Assert(jsonData.Count == books.Count);
        
        // Sets the connection URI
        const string connectionUri = "mongodb://localhost:27017/";
        // Creates a new client and connects to the server
        var client = new MongoClient(connectionUri);
        var db = client.GetDatabase("books");
        var collection = db.GetCollection<Book>("books");
        collection.InsertMany(books);
    }
}