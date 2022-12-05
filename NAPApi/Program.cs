using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using NAPApi.Context;
using NAPApi.Help;
using NAPApi.Repository;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dealing with JWT in confige Use model 
builder.Services.Configure<JwtHelper>(builder.Configuration.GetSection("JWT"));



//dealing with connection string by dependence injection
//builder.Services.AddIdentity<UserModel, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb"));
});

//dealing with repository
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();

//authentication 
/*
 app.UseAuthentication();
 */
builder.Services.AddAuthentication(option => {
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    op =>
    {
        op.RequireHttpsMetadata = false;
        op.SaveToken = false;
        op.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });




var app = builder.Build();


app.UseExceptionHandler(
    option =>
    {
        option.Run(
            async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var ex = context.Features.Get<IExceptionHandlerFeature>();
                if (ex != null)
                {
                    await context.Response.WriteAsync(ex.Error.Message);
                }
            });
    });

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{*/
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Uploads"
});
app.MapControllers();

app.Run();
