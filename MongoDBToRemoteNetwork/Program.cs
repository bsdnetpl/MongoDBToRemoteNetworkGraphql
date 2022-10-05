using AppAny.HotChocolate.FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDBToRemoteNetwork.Properties.Data;
using MongoDBToRemoteNetwork.Properties.mutations;
using MongoDBToRemoteNetwork.Properties.querys;
using MongoDBToRemoteNetwork.Properties.subscription;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));

builder.Services.AddSingleton<BooksService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<UsersServices>();


builder.Services.AddFluentValidation();
builder.Services.AddScoped<UsersValidations>();
builder.Services.AddScoped<IPasswordHasher<Users>,PasswordHasher<Users>>();

builder.Services.AddInMemorySubscriptions();//subscription

builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<subscription>()
    .AddFiltering()
    .AddSorting()
    .AddFluentValidation()
    .AddAuthorization(); 
//.AddFluentValidation(Validate => Validate.RegisterValidatorsFromAssemblyContaining<UserVal>());
var app = builder.Build();

app.UseRouting();
app.UseWebSockets();//subscription

app.MapGraphQL();

//
//app.UseAuthentication();
//app.UseAuthorization();

app.Run();
