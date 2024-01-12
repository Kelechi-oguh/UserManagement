using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using UserManagement.Data;
using UserManagement.Services.CartService;
using UserManagement.Services.ProductService;
using UserManagement.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<ProductService, ProductService>();
builder.Services.AddTransient<CartService, CartService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization Header Using The Bearer Scheme(\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Description = "JWT Authorization Header Using The Bearer Scheme(\"Bearer {token}\")",
//        In = ParameterLocation.Header,
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer"
//    });
//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//           new OpenApiSecurityScheme
//           {
//               Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
//           },
//           new string[] { }
//        }
//    });
//});

//Auto Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add db context
builder.Services.AddDbContext<UserContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("UserContext")));

//Add authentication
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "issuer",
            ValidAudience = "audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(options =>
    //{
    //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");

    //    // Specify the security scheme to use (same as defined in AddSecurityDefinition)
    //    options.DocumentTitle = "Swagger UI - Your API";
    //    options.DocExpansion(DocExpansion.None);

    //    options.InjectStylesheet("/path/to/custom/styles.css"); // Optional: Include custom styles
    //});
}

app.UseHttpsRedirection();

// newly added for jwt
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

