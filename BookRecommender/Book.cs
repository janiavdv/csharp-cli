namespace BookRecommender;

public class Book
{
    public float AverageRating { get; set; }
    public string Description { get; set; }
    public List<string> Genres { get; set; }
    public string ImageURL { get; set; }
    public string LanguageCode { get; set; }
    public int? OriginalPublicationYear { get; set; }
    public string OriginalTitle { get; set; }
    public int? NumPages { get; set; }
    public int Ratings1 { get; set; }
    public int Ratings2 { get; set; }
    public int Ratings3 { get; set; }
    public int Ratings4 { get; set; }
    public int Ratings5 { get; set; }
    public int NumRatings { get; set; }
    public string Title { get; set; }
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
}