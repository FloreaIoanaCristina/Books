using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Books.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.Data
{
    public class SeedB
    {
        public static async Task SeedBooks(DataContext context)
        {
            if(await context.BooksTable.AnyAsync()) return;

            var genreData = await File.ReadAllTextAsync("Data/BooksGenres.json");

          //  var bookData = await File.ReadAllTextAsync("Data/BooksSeedData.json");

            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};
           // var books =  JsonSerializer.Deserialize<List<AppBooks>>(bookData);
            var genres = JsonSerializer.Deserialize<List<Genre>>(genreData);

            foreach(var genre in genres)
            {
                genre.Name = genre.Name;
                context.Genres.Add(genre);
            }
            // foreach(var book in books)
            // {
            //     book.Title = book.Title;
            //     context.BooksTable.Add(book);
            // }
            await context.SaveChangesAsync();
            
        }
    }
}