using MongoDB.Bson.Serialization.Attributes;

namespace BookRecommender;

[BsonIgnoreExtraElements]
public class Book
{
    [BsonElement("AverageRating")]
    public float AverageRating { get; set; }
    [BsonElement("Description")]
    public string Description { get; set; }
    [BsonElement("Genres")]
    public List<string> Genres { get; set; }
    [BsonElement("ImageURL")]
    public string ImageURL { get; set; }
    [BsonElement("LanguageCode")]
    public string LanguageCode { get; set; }
    [BsonElement("OriginalPublicationYear")]
    public int? OriginalPublicationYear { get; set; }
    [BsonElement("OriginalTitle")]
    public string OriginalTitle { get; set; }
    [BsonElement("NumPages")]
    public int? NumPages { get; set; }
    [BsonElement("Ratings1")]
    public int Ratings1 { get; set; }
    [BsonElement("Ratings2")]
    public int Ratings2 { get; set; }
    [BsonElement("Ratings3")]
    public int Ratings3 { get; set; }
    [BsonElement("Ratings4")]
    public int Ratings4 { get; set; }
    [BsonElement("Ratings5")]
    public int Ratings5 { get; set; }
    [BsonElement("NumRatings")]
    public int NumRatings { get; set; }
    [BsonElement("Title")]
    public string Title { get; set; }
    [BsonElement("Authors")]
    public List<string> Authors { get; set; }

    public Book(JSONBook rawData)
    {
        this.AverageRating = rawData.average_rating;
        this.Description = rawData.description;
        this.Genres = ParseGenres(rawData.genres);
        this.ImageURL = rawData.image_url;
        this.LanguageCode = rawData.language_code;
        this.OriginalPublicationYear = rawData.original_publication_year;
        this.OriginalTitle = rawData.original_title;
        this.NumPages = rawData.pages;
        this.Ratings1 = rawData.ratings_1;
        this.Ratings2 = rawData.ratings_2;
        this.Ratings3 = rawData.ratings_3;
        this.Ratings4 = rawData.ratings_4;
        this.Ratings5 = rawData.ratings_5;
        this.NumRatings = rawData.ratings_count;
        this.Title = rawData.title;
        this.Authors = ParseAuthors(rawData.authors);
    }
    
    private static List<string> ParseGenres(string genres)
    {
        // Remove the "c(" and ")" parts
        genres = genres.Replace("c(", "").Replace(")", "");
        // Split the string by commas and trim each genre
        string[] genreArray = genres.Split(',', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < genreArray.Length; i++)
        {
            // Remove leading and trailing whitespace, and quotes
            genreArray[i] = genreArray[i].Trim(' ', '"');
        }
    
        return new List<string>(genreArray);
    }
    
    private static List<string> ParseAuthors(string authors)
    {
        // Remove the "[" and "]" parts
        authors = authors.Replace("[", "").Replace("]", "");
        // Split the string by commas and trim each author
        string[] authorArray = authors.Split(',', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < authorArray.Length; i++)
        {
            // Remove leading and trailing whitespace, and quotes
            authorArray[i] = authorArray[i].Trim(' ', '"');
        }
    
        return new List<string>(authorArray);
    }

    public void PrintBook()
    {
        Console.WriteLine();
        Console.WriteLine("Original Title: " + OriginalTitle);
        Console.WriteLine("Authors: " + string.Join(", ", Authors));
        Console.WriteLine("Original Publication Year: " + OriginalPublicationYear);
        Console.WriteLine("Description: " + Description);
    }
}