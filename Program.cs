using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopOnline.Models;
using ShopOnline.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetSection("ConnectionString").GetValue<string>("ShopContext")));
builder.Services.AddScoped<IProduct, ProductRepository>();
builder.Services.AddScoped<IAccount, AccountRepository>();
builder.Services.AddScoped<IListQuantity, ListQuantityRepository>();
builder.Services.AddScoped<IOrder, OrderRepository>();
builder.Services.AddScoped<IOrderEntry, OrderEntryRepository>();
builder.Services.AddScoped<IItemCart, ItemCartRepository>();

builder.Services.AddScoped(typeof(ICPSS<>), typeof(CPSSRepository<>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var secretKey = Configuration["AppSetting:SecretKey"];
if (secretKey == null)
{
    throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null.");
}
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            //tự cấp token
            ValidateIssuer = false,
            ValidateAudience = false,

            //ký vào token
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyWebApiApp", Version = "v1" });
});


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
