using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;
using System.Text;
using TaskManagement_April_.Context;
using TaskManagement_April_.Model;
using TaskManagement_April_.Service;
using TaskManagement_April_.Service.Implementation;

var builder = WebApplication.CreateBuilder(args);

string smtpServer = "smtp.gmail.com";
int port = 587; 
string username = "bhagyashree.gaikwad@wonderbiz.in";
string password = "tpli pogm axqq egvk";



//Jwt token

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtIssuer,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
     };
 });
//Jwt token 

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TaskManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connect")));
var allowedOrigin = builder.Configuration.GetSection("AllowedHosts").Get<string[]>();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Internal_Cors", policy =>
    {
        policy.WithOrigins(allowedOrigin).AllowAnyMethod().AllowAnyHeader();
    });
});
#region Service
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ISubTaskService, SubTaskService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddHostedService<TaskReminderService>();
//builder.Services.AddScoped<IEmailService>(s =>
//    new EmailService("smtp.gmail.com", 587,"bhagyashree.gaikwad@wonderbiz.in","tpli pogm axqq egvk"));
#endregion

var app = builder.Build();
app.Use(async (context, next) => {
    context.Response.Headers.Add("Access-Control-Allow-Origin", allowedOrigin);

    await next();
});

// Configure CORS
app.UseCors("Internal_Cors");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
