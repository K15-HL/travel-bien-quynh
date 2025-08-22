using travel_bien_quynh.Repositories.Interface;
using travel_bien_quynh.Repositories;
using travel_bien_quynh.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using travel_bien_quynh.Options;
using travel_bien_quynh.Contexts;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// C?u hình MongoDB settings

// C?u hình MVC và API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer token\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:1368", "https://dulichbienquynh.com", "http://127.0.0.1:5500")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    }
);

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection(nameof(MongoSettings)));
builder.Services.AddScoped<IMongoDbContext, travel_bien_quynh.Contexts.MongoDbContext>();
//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped<IRentSimRepository, RentSimRepository>();
//builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<ITourRepository, TourRepository>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<ISliderRepository, SliderRepository>();
builder.Services.AddScoped<IBookingTourRepository, BookingTourRepository>();
builder.Services.AddScoped<IBookingRoomRepository, BookingRoomRepository>();
builder.Services.AddScoped<IOrderFoodRepository, OrderFoodRepository>();
//builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
//builder.Services.AddScoped<IServiceListRepository, ServiceListRepository>();
//builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IInformationRepository, InformationRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
//builder.Services.AddScoped<IAtmHistory, AtmHistoty>();
//builder.Services.AddScoped<IAtmCheck, AtmCheckRepository>();
//builder.Services.AddScoped<ILogWallet, LogWalletRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVerificationCode, VerificationCodeRepository>();
builder.Services.AddScoped<IVerificationService, VerificationService>();
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
builder.Services.AddSingleton<IEmailConfiguration>(sp =>
sp.GetRequiredService<IOptions<EmailConfiguration>>().Value);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddScoped<IAtmService, AtmService>();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseRouting();

app.UseCors("AllowLocalhost");

app.UseAuthentication();  
app.UseAuthorization();
// C?u hình Swagger và các middleware
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapHub<travel_bien_quynh.Hubs.BookingHub>("/bookingHub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseCors("AllowLocalhost");
app.UseAuthorization();
app.MapControllers();
app.Run();
//app.Run("http://*:25565");
