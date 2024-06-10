using System.Diagnostics;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookRecommender;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter name: ");
        string? username = Console.ReadLine();
        Console.WriteLine("Hi, " + username + ". Welcome to the Book Recommender.");
        Console.WriteLine("Do you want to reset the database? ('y' or 'n')");
        string resetStr = Console.ReadLine();
        if (resetStr != "y" && resetStr != "n")
        {
            Console.WriteLine("\""+ resetStr + "\" was not one of the choices. Application quitting...");
            return; 
        }

        bool reset = resetStr == "y";
        
        // Sets the connection URI
        const string connectionUri = "mongodb://localhost:27017/";
        // Creates a new client and connects to the server
        var client = new MongoClient(connectionUri);
        var db = client.GetDatabase("books");
        var collection = db.GetCollection<Book>("books");

        if (collection.EstimatedDocumentCount() != 10000)
        {
            if (reset || collection.EstimatedDocumentCount() > 10000)
                // Delete all documents in the database
                collection.DeleteMany(Builders<Book>.Filter.Empty);
            
            const string path = "../../../data/books.json";
            List<JSONBook>? jsonData;

            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                jsonData = JsonSerializer.Deserialize<List<JSONBook>>(json);
            }

            List<Book> books = new List<Book>();
            foreach (JSONBook rawData in jsonData)
            {
                Book book = new Book(rawData);
                books.Add(book);
            }

            // ensure all books were successfully converted
            Debug.Assert(jsonData.Count == books.Count);
        
            collection.InsertMany(books);
        }
        
        while (true) // App continues until user quits
        {
            Console.WriteLine("Do you want to find a book ('f'), update your read books ('u'), or quit the app ('q')?");
            string action = Console.ReadLine();

            switch (action)
            {
                case "q":
                    Console.WriteLine("Application quitting...");
                    return;
                case "f":
                {
                    Console.WriteLine("How do you want to find a book today?");
                    Console.WriteLine("Choose from these options:");

                    List<string> options = new List<string>
                        { "OriginalTitle", "Authors", "Genres", "OriginalPublicationYear", "LanguageCode" };
                    foreach (string option in options)
                        Console.WriteLine("- " + option);

                    string column = Console.ReadLine();

                    if (!options.Contains(column))
                    {
                        Console.WriteLine(
                            "\"" + column + "\" was not one of the specified options. Application quitting...");
                        return;
                    }

                    Console.WriteLine("What " + column + " are you looking for?");
                    string search = Console.ReadLine();

                    FilterDefinitionBuilder<Book> builder = Builders<Book>.Filter;
                    FilterDefinition<Book> filter;

                    switch (column)
                    {
                        case "OriginalTitle":
                        case "Authors":
                        case "Genres":
                        {
                            filter = builder
                                .Regex(column, new BsonRegularExpression(".*" + search + ".*"));
                            break;
                        }
                        case "OriginalPublicationYear":
                        {
                            try
                            {
                                int year = Convert.ToInt32(search);
                                filter = builder
                                    .Eq(column, year);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Invalid year: \"" + search + "\". Application quitting...");
                                return;
                            }

                            break;
                        }
                        case "LanguageCode":
                        {
                            filter = builder
                                .Eq(column, search);
                            break;
                        }
                        default:
                            // This block is never entered since we verify validity of column above.
                            return;
                    }


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
                        return;
                    }

                    var booksFound = collection
                        .Find(filter)
                        .Limit(count)
                        .ToList();

                    if (booksFound.Count == 0)
                    {
                        Console.WriteLine("No books found. Application quitting...");
                        return;
                    }

                    foreach (Book bookFound in booksFound)
                        bookFound.PrintBook();
                    break;
                }
                case "u":
                {
                    Console.WriteLine("How do you want to search for the book you've read?");
                    Console.WriteLine("Choose from these options:");

                    List<string> options = new List<string>
                        { "OriginalTitle", "Authors" };
                    foreach (string option in options)
                        Console.WriteLine("- " + option);

                    string column = Console.ReadLine();

                    if (!options.Contains(column))
                    {
                        Console.WriteLine(
                            "\"" + column + "\" was not one of the specified options. Application quitting...");
                        return;
                    }

                    Console.WriteLine("What " + column + " are you looking for? Be as specific as possible.");
                    string search = Console.ReadLine();

                    FilterDefinition<Book> filter = Builders<Book>
                        .Filter
                        .Regex(column, new BsonRegularExpression(".*" + search + ".*"));

                    int count = 10;

                    var booksFound = collection
                        .Find(filter)
                        .Limit(count)
                        .ToList();

                    if (booksFound.Count == 0)
                    {
                        Console.WriteLine("No books found. Application quitting...");
                        return;
                    }

                    for (int i = 0; i < booksFound.Count; i++)
                    {
                        Book bookFound = booksFound[i];
                        Console.Write("[" + i + "] ");
                        bookFound.PrintBookOneLine();
                    }

                    Console.WriteLine("Which book have you read? Enter the number beside the book.");
                    string posStr = Console.ReadLine();
                    int pos;
                    try
                    {
                        pos = Convert.ToInt32(posStr);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Invalid integer \"" + posStr + "\". Application quitting...");
                        return;
                    }

                    if (pos < 0 || pos > count - 1)
                    {
                        Console.WriteLine("Integer " + posStr + " out of range [0, " + (count - 1) +
                                          "]. Application quitting...");
                        return;
                    }

                    Book read = booksFound[pos];

                    var bookToUpdate = Builders<Book>
                        .Filter
                        .Eq(b => b.Title, read.Title);
                    var update = Builders<Book>
                        .Update
                        .Set(b => b.Read, true);

                    UpdateResult status = collection.UpdateOne(bookToUpdate, update);
                    if (status.IsAcknowledged && status.ModifiedCount == 1)
                    {
                        Console.WriteLine("You have read " + read.OriginalTitle + ".");
                    }
                    else
                    {
                        Console.WriteLine("No updates were made. The book was already marked as read or was not found.");
                    }

                    break;
                }
                default:
                    Console.WriteLine("Invalid action \"" + action + "\".");
                    break;
            }
        }
    }
}