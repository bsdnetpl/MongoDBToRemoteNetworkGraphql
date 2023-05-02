
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDBToRemoteNetwork.Properties.Data;
using MongoDBToRemoteNetwork.Properties.mutations;
using MongoDBToRemoteNetwork.Properties.querys;
using MongoDBToRemoteNetwork.Properties.subscription;
using NLog.Web;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));

builder.Services.Configure<TokenSettings>(
    builder.Configuration.GetSection("TokenSettings"));


builder.Services.AddSingleton<BooksService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<UsersServices>();

//nlog
builder.Logging.ClearProviders();
builder.Host.UseNLog();

//builder.Services.AddFluentValidation(Validate => Validate.RegisterValidatorsFromAssemblyContaining<UsersValidations>());
builder.Services.AddFluentValidation();
//builder.Services.AddScoped<UsersValidations>();
builder.Services.AddScoped<IPasswordHasher<Users>,PasswordHasher<Users>>();

builder.Services.AddInMemorySubscriptions();//subscription

builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<subscription>()
    .AddFiltering()
    .AddSorting();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration.GetSection("TokenSettings").GetValue<string>("Issuer"),
            ValidateIssuer = true,
            ValidAudience = builder.Configuration.GetSection("TokenSettings").GetValue<string>("Audience"),
            ValidateAudience = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("TokenSettings").GetValue<string>("Key"))),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("role-policy", policy =>
    {
        policy.RequireRole(new[] { "superadmin", "user" });
    });
    options.AddPolicy("cliams-policy", policy =>
    {
        policy.RequireClaim("usercountry", "Poland", "USA");
    });
});


//.AddFluentValidation(Validate => Validate.RegisterValidatorsFromAssemblyContaining<UserVal>());
var app = builder.Build();

app.UseRouting();
app.UseWebSockets();//subscription
app.UseAuthorization();
app.UseAuthentication();

app.MapGraphQL();

app.Run();
