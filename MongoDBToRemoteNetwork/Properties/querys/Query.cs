using HotChocolate;
using Microsoft.AspNetCore.Mvc;
using MongoDBToRemoteNetwork.Properties.Data;

namespace MongoDBToRemoteNetwork.Properties.querys
{
    public class Query
    {
        private readonly BooksService _booksService;
        private readonly OrderService _orderService;
        private readonly UsersServices _usersService;
        //----------------------------------------------------------------------------------------
        public Query(BooksService booksService, OrderService orderService, UsersServices usersService)
        {
            _booksService = booksService;
            _orderService = orderService;
            _usersService = usersService;
        }
        //----------------------------------------------------------------------------------------

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<List<Book>> Get()
        {
            var book = await _booksService.GetAsync();
            if (book is null)
            {
                throw new GraphQLException("No book in this DBs");
            }
            return book;
        }
        //----------------------------------------------------------------------------------------
       
        [UseFiltering]
        [UseSorting]
        public async Task<List<Order>> GetOrdersAsync()
        {
            var order = await _orderService.GetOrderAsync();
            if (order is null)
            {
                throw new GraphQLException("No book in this DBs");
            }
            return order;
        }
        //----------------------------------------------------------------------------------------
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<List<Users>> GetUsersAsync()
        {
         var users = await _usersService.GetUsersAsync();
            if(users is null)
            {
                throw new GraphQLException("No users can you see");
            }
            return users;
        }

        //----------------------------------------------------------------------------------------

    }
}
