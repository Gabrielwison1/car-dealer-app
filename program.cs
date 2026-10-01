using CarDealerApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Register MVC Controllers and Views + API Support
builder.Services.AddControllersWithViews();

<<<<<<< HEAD

// 2. Register EF Core In-Memory Database
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseInMemoryDatabase("CarDealerDb"));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Add swagger api explorer and generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4. Register Cookie Authentication for Admin access
=======
// 2. Add Swagger API Explorer & Generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Register EF Core Database (SQL Server if configured, otherwise SQLite file DB)
var sqlServerConnectionString = builder.Configuration.GetConnectionString("SqlServerConnection");
var sqliteConnectionString = builder.Configuration.GetConnectionString("SqliteConnection");

// Uses SQL Server if running locally on Windows with LocalDB, otherwise falls back to SQLite
if (builder.Environment.IsDevelopment() && !string.IsNullOrEmpty(sqlServerConnectionString))
{
    // Toggle between UseSqlServer or UseSqlite based on team setup
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(sqliteConnectionString)); 
        // Note: Change 'UseSqlite' to 'UseSqlServer(sqlServerConnectionString)' if your teammate uses local SQL Server!
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(sqliteConnectionString));
}

// 4. Register Cookie Authentication
>>>>>>> d575b8db7046dd7f906b905e1bfde9fc33b2304e
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
    });

var app = builder.Build();

// Ensure the database file/tables are created on startup without overwriting existing entries
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

<<<<<<< HEAD
// Enable Swagger UI 
app.UseSwagger();
app.UseSwaggerUI(c => 
{ 
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Dealer API v1");
    c.RoutePrefix = "swagger";
}   
);

=======
// 5. Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Dealer API V1");
    c.RoutePrefix = "swagger";
});
>>>>>>> d575b8db7046dd7f906b905e1bfde9fc33b2304e

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();