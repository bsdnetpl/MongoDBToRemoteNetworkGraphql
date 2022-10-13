using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using MongoDBToRemoteNetwork.Properties.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;

namespace MongoDBToRemoteNetwork.Properties.querys
{
    public class Query
    {
        private readonly BooksService _booksService;
        private readonly OrderService _orderService;
        private readonly UsersServices _usersService;
        private readonly IPasswordHasher<Users> _passwordHasher;

        //----------------------------------------------------------------------------------------
        public Query(BooksService booksService, OrderService orderService, UsersServices usersService, IPasswordHasher<Users> passwordHasher)
        {
            _booksService = booksService;
            _orderService = orderService;
            _usersService = usersService;
            _passwordHasher = passwordHasher;
        }
        //----------------------------------------------------------------------------------------
        [Authorize(Policy = "role-policy")]
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
        [UsePaging]
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
        public async Task<string> Login([Service] IOptions<TokenSettings> tokenSettings,string email, string password)
        {
            var mail = await _usersService.GetAsyncUseremail(email);
            if (mail is null)
            {
                throw new GraphQLException(new Error("No user or email in DB"));
            }

            var result = _passwordHasher.VerifyHashedPassword(mail, mail.Password, password); // verify pass
            if(result == PasswordVerificationResult.Failed)
            {
                throw new GraphQLException(new Error("No user or email in DB"));
            }


            var secureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Value.Key));
            var credentials = new SigningCredentials(secureKey, SecurityAlgorithms.HmacSha256);
            
            
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Role, "user"),
                    new Claim("usercountry", "Poland")
                };

            var jwtToken = new JwtSecurityToken(
                issuer: tokenSettings.Value.Issuer,
                audience: tokenSettings.Value.Audience,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: credentials,
                claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

    }
}
