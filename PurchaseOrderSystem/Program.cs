using PurchaseOrderSystem.Data;
using PurchaseOrderSystem.Services.Implementations;
using PurchaseOrderSystem.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// File Logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddProvider(
    new PurchaseOrderSystem.Logging.FileLoggerProvider("Logs/app-log.txt")
);

// Add services to the container.
builder.Services.AddControllersWithViews();



//Register Repository Paths
builder.Services.AddSingleton(new JsonRepository<object>(
    Path.Combine(builder.Environment.ContentRootPath,
    "Data/JsonFiles/vendors.json")
    ));

builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IVendorItemPriceService, VendorItemPriceService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();




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
