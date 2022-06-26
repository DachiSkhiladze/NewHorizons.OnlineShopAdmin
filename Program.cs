using Microsoft.EntityFrameworkCore;
using OnlineShop.DataAccess.DbContexts;
using OnlineShop.DataAccess.Models;
using OnlineShop.DataAccess.Repository.Abstractions;
using OnlineShop.DataAccess.Repository.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Server=tcp:newhorizons.database.windows.net,1433;Initial Catalog=AdventureWorks2019;Persist Security Info=False;User ID=Dachi;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
//builder.Services.AddDbContext<AdventureWorks2019Context>(x => x.UseSqlServer("Data Source=localhost;Initial Catalog=AdventureWorks2019;Integrated Security=True"), ServiceLifetime.Transient);
builder.Services.AddDbContext<AdventureWorks2019Context>(x => x.UseSqlServer("Server=tcp:newhorizons.database.windows.net,1433;Initial Catalog=AdventureWorks2019;Persist Security Info=False;User ID=Dachi;Password=Bubunita34;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"), ServiceLifetime.Transient);
builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();
builder.Services.AddScoped<IRepository<ProductModel>, Repository<ProductModel>>();
builder.Services.AddScoped<IRepository<ProductCategory>, Repository<ProductCategory>>();
builder.Services.AddScoped<IRepository<ProductSubcategory>, Repository<ProductSubcategory>>();
builder.Services.AddScoped<IRepository<SalesOrderHeader>, Repository<SalesOrderHeader>>();
builder.Services.AddScoped<IRepository<SalesOrderDetail>, Repository<SalesOrderDetail>>();
builder.Services.AddScoped<IRepository<Customer>, Repository<Customer>>();
builder.Services.AddScoped<IRepository<Person>, Repository<Person>>();
builder.Services.AddScoped<IRepository<SpecialOfferProduct>, Repository<SpecialOfferProduct>>();
builder.Services.AddScoped<IRepository<Address>, Repository<Address>>();
builder.Services.AddScoped<IRepository<BusinessEntity>, Repository<BusinessEntity>>();
builder.Services.AddScoped<IRepository<BusinessEntityAddress>, Repository<BusinessEntityAddress>>();
builder.Services.AddScoped<IRepository<EmailAddress>, Repository<EmailAddress>>();
builder.Services.AddScoped<IRepository<Password>, Repository<Password>>();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
