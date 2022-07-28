using ApplicationCore.RepositoryContracts;
using ApplicationCore.ServiceContracts;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
//need to use namespace for cookie
using Microsoft.AspNetCore.Authentication.Cookies;
using MovieShopMVC.Infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DI : used to create loosely coupled code to make code more maintainable 
// first class citizen (built-in support) 
builder.Services.AddScoped<IMovieService, MovieService>(); // when we see IMovieService, then inject MovieService
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<ICastRepository, CastRepository>();
builder.Services.AddScoped<ICastService, CastService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IUserService,UserService>();

builder.Services.AddHttpContextAccessor();

// read the connection string from appsetting.json and inject connection string into DbContext
builder.Services.AddDbContext<MovieShopDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieShopDbConnection"));
});

// injecting authentication seeting
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "MovieShopAuthCookie";
    options.ExpireTimeSpan = TimeSpan.FromHours(2); // we can also set sliding expiration
    options.LoginPath = "/account/login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// all these are also middlewares, HTTP Request goes through all these middlewares
// order of middleware is very important
// we can use any built-in middlewares, create our own custom middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Middlewares, add authentication middleware (checks cookie)
app.UseAuthentication(); // do not forget this step, and it should be before authentication
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
