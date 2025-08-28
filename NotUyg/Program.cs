using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NotUyg.Data;
using NotUyg.Data.Abstract;
using NotUyg.Data.Concrete.EfCore;
using NotUyg.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<NotContext>(options =>
{
    var a = builder.Configuration;
    var b = a.GetConnectionString("sql_connection" );
    options.UseSqlServer(b);
}
);

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<NotContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 5;
    options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = true;    
});

builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<INotRepository, NotRepository>();
builder.Services.AddScoped<IAnketRepository, AnketRepository>();
builder.Services.AddScoped<IUserVoteRepository, UserVoteRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

SeedData.VeriEkle(app);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
