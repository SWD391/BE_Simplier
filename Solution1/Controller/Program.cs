using Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.AssetHistoryRepository;
using Services;
using Services.AccountService;
using Services.AssetService;
using Services.FeedbackService;
using Services.FixTaskService;
using Services.JwtService;
using SignalR.NotificationHub;
using AutoMapper;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IFixTaskService, FixTaskService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.ConfigureSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
             {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
             },
             new string[] {}
        }
    });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("cuong",
        builder =>
        {
            builder.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});


builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "")),
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseCors("cuong");
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/NotificationHub");

app.Run();
