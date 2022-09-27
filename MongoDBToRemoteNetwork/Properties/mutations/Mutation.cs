using AppAny.HotChocolate.FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDBToRemoteNetwork.Properties.Data;

namespace MongoDBToRemoteNetwork.Properties.mutations
{
    public class Mutation
    {
        private readonly BooksService _booksService;
        private readonly UsersServices _usersServices;
        //private readonly UsersValidations _validationRules;
        private readonly IPasswordHasher<Users>_passwordHasher;

        public Mutation(BooksService booksService, UsersServices usersServices, IPasswordHasher<Users> passwordHasher)/*, UsersValidations validationRules)*/
        {
            _booksService = booksService;
            _usersServices = usersServices;
            //_validationRules = validationRules;
            _passwordHasher = passwordHasher;
        }
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
                throw new GraphQLException(new Error("No this book id","NO_UPDATE"));
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
                throw new GraphQLException(new Error("No this book id","NO_DELETED"));
            }

            await _booksService.RemoveAsync(id);

            return $"Book deleted id: {id}";
        }
        //------------------------------------------------------------------
         public async Task<Users> CreateUser(Users newUsers)
        {
            newUsers.Password = _passwordHasher.HashPassword(newUsers, newUsers.Password);

            await _usersServices.CreateUserAsync(newUsers);

            return newUsers;
        }

        //private void ValidateUser(Users newUsers)
        //{
        //    ValidationResult validationResult = _validationRules.Validate(newUsers);
        //    if (!validationResult.IsValid)
        //    {
        //        throw new GraphQLException("Invalid Input.");
        //    }
        //}
    }
}
