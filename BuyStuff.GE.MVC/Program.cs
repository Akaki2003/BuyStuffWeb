using BuyStuff.GE.MVC.ApiServices;
using BuyStuff.GE.MVC.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.Configure<BaseUriConfiguration>(builder.Configuration.GetSection(nameof(BaseUriConfiguration)));

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddHttpClient();
builder.Services.AddScoped<ItemApiService>();
builder.Services.AddScoped<AuthenticationApiService>();

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

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
