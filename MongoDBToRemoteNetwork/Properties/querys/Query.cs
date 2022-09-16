using Microsoft.AspNetCore.Mvc;
using MongoDBToRemoteNetwork.Properties.Data;

namespace MongoDBToRemoteNetwork.Properties.querys
{
    public class Query
    {
        private readonly BooksService _booksService;

        public Query(BooksService booksService) =>
            _booksService = booksService;
        [UseFiltering]
        public async Task<List<Book>> Get()
        {
            var book = await _booksService.GetAsync();
            if(book is null)
            {
                throw new Exception("No this book in this DBs");
            }
            return book;
        }
        
    }
}
