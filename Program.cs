using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using OnlineStore.Data;
using GemStore.Repositories;
using OnlineStore.Models;
using GemStore.Interfaces;
using GemStore.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

// Add authentication.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
        option => {
            option.LoginPath = "/user/login";
            option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        }
    );

// Add session variable for storing user's cart.
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

// Configure dependency injection to provide an instance of interface to the controller.
builder.Services.AddScoped<IUserRepository<UserModel>, UserRepository>();
builder.Services.AddScoped<ICartRepository<CartViewModel>, CartRepository>();
builder.Services.AddHttpClient<ICategoryRepository, CategoryRepository>();
builder.Services.AddHttpClient<IProductRepository<ProductViewModel>, ProductRepository>();

// Register database context.
builder.Services.AddDbContext<ApplicationDatabaseContext>(
    options => {
        // options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
         options.UseMySql(builder.Configuration.GetConnectionString("DeployedConnection"), 
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DeployedConnection")));
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
