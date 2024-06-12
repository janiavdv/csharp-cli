# C# Command Line Book Recommender

A brief command line app I built to learn C# and the [MongoDB C# Driver](https://www.mongodb.com/docs/drivers/csharp/current/). This project is adapted from [Book Recommender](https://github.com/janiavdv/book-recommender), a similar project with more of a data science flavor which has a [web app](https://janiavdv.shinyapps.io/book_recommender/) to accompany it. 

## Use

Clone the repo and run

```shell
cd BookRecommender
dotnet run
```

with MongoDB running on your computer. Replace the `connectionUri` [here](https://github.com/janiavdv/csharp-cli/blob/19d91d4629a38eb015a951c6300ef160b2e83c2f/BookRecommender/Program.cs#L20) if you have changed your local MongoDB port from the default. 

Follow the prompts, inputting your name and populating the database with the books. Then, get recommended books using the 'f' action or update your read books to avoid getting them recommended using the 'u' action. To quit the app, use 'q'. To clear your read books, reboot the app and enter 'y' in the opening command that asks if you want to reset the database. 

## Data

The data for this application is sourced from the ["Goodbooks 10k Extended" repository](https://github.com/malcolmosh/goodbooks-10k-extended/tree/master), using the `books_enriched.csv` file. This file contains 10,000 records of book data from Goodreads, as of September 2017. Irrelevant columns were removed and the CSV file was cleaned and converted to JSON before being inserted to the database. 
