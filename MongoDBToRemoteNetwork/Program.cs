using FluentValidation.AspNetCore;
using MongoDBToRemoteNetwork.Properties.Data;
using MongoDBToRemoteNetwork.Properties.mutations;
using MongoDBToRemoteNetwork.Properties.querys;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));

builder.Services.AddFluentValidation();
builder.Services.AddScoped<UsersValidations>();

builder.Services.AddSingleton<BooksService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<UsersServices>();

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddSorting()
    .AddSorting();

var app = builder.Build();
app.MapGraphQL();

app.Run();
