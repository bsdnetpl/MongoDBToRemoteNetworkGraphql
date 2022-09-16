using Microsoft.AspNetCore.Mvc;
using MongoDBToRemoteNetwork.Properties.Data;

namespace MongoDBToRemoteNetwork.Properties.mutations
{
    public class Mutation
    {
        private readonly BooksService _booksService;

        public Mutation(BooksService booksService) =>
            _booksService = booksService;
        //----------------------------------------------------------------
        public async Task<Book> AddBook(Book newBook)
        {
            await _booksService.CreateAsync(newBook);

            return newBook;
        }
        //------------------------------------------------------------------
        public async Task<Book> Update(string id, Book updatedBook)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                throw new Exception("No this book id");
            }

            updatedBook.Id = book.Id;

            await _booksService.UpdateAsync(id, updatedBook);

            return updatedBook;
        }
        //------------------------------------------------------------------
        public async Task<string> Delete(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                throw new Exception("No this book id");
            }

            await _booksService.RemoveAsync(id);

            return $"Book deleted id: {id}";
        }
        //------------------------------------------------------------------

    }
}
