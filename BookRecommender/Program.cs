using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookRecommender;

class Program
{
    static void Main(string[] args)
    {
        const string path = "../../../data/books.json";
        List<JSONBook>? jsonData = new List<JSONBook>(); 
        
        using (StreamReader reader = new StreamReader(path))
        {
	        string json = reader.ReadToEnd();
            jsonData = JsonSerializer.Deserialize<List<JSONBook>>(json);
            if (jsonData == null)
                System.Environment.Exit(0);
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
        
        Console.WriteLine("Enter name: ");
        string? username = Console.ReadLine();
        Console.WriteLine("Hi, " + username +
                          ". Welcome to the Book Recommender - how do you want to search for a book today?");
        Console.WriteLine("Choose from these options or use q to quit:");

        List<string> options = new List<string>
            { "OriginalTitle", "Authors", "Genres", "OriginalPublicationYear", "LanguageCode" };
        foreach (string option in options)
            Console.WriteLine("- " + option);

        string column = Console.ReadLine();

        if (column == "q")
        {
            Console.WriteLine("Application quitting...");
            Environment.Exit(0);
        }

        if (!options.Contains(column))
        {
            Console.WriteLine("\"" + column + "\" was not one of the specified options. Application quitting...");
            Environment.Exit(0);
        }

        Console.WriteLine("What " + column + " are you looking for?");
        string search = Console.ReadLine();

        var filter = Builders<Book>
            .Filter
            .Eq(column, search);
        
        Console.WriteLine("How many results (at most) do you want?");
        string countStr = Console.ReadLine();
        int count = 0;
        
        try
        {
            count = Convert.ToInt32(countStr);
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid integer \"" + countStr + "\". Application quitting...");
            Environment.Exit(0);
        }
        
        var booksFound = collection
            .Find(filter)
            .Limit(count)
            .ToList();

        if (booksFound.Count == 0)
        {
            Console.WriteLine("No books found. Application quitting...");
            Environment.Exit(0);
        }

        foreach (Book bookFound in booksFound)
            bookFound.PrintBook();
       
    }
}