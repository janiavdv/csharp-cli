namespace BookRecommender;

public class JSONBook
{
    // Represents the `books.json` key-value pairs
    public float average_rating { get; set; }
    public string description { get; set; }
    public string genres { get; set; }
    public string image_url { get; set; }
    public string language_code { get; set; }
    public int? original_publication_year { get; set; }
    public string original_title { get; set; }
    public int? pages { get; set; }
    public int ratings_1 { get; set; }
    public int ratings_2 { get; set; }
    public int ratings_3 { get; set; }
    public int ratings_4 { get; set; }
    public int ratings_5 { get; set; }
    public int ratings_count { get; set; }
    public string title { get; set; }
    public string authors { get; set; }
}