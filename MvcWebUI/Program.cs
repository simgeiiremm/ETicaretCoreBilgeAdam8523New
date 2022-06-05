//class Program
//{
//    static void Main(string[] args)
//    {
//          ... 9.satýr
//    }
//}

using AppCore.DataAccess.Configs;
using Business.Services;
using Business.Services.Bases;
using DataAccess.Contexts;
using DataAccess.Repositories;
using DataAccess.Repositories.Bases;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MvcWebUI.Settings;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

List<CultureInfo> _cultures = new List<CultureInfo>()
{
    new CultureInfo("tr-TR")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(_cultures.FirstOrDefault().Name);
    options.SupportedCultures = _cultures;
    options.SupportedUICultures = _cultures;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(config =>
    {
        config.LoginPath = "/Hesaplar/Giris";
        config.AccessDeniedPath = "/Hesaplar/YetkisizIslem";
        config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        config.SlidingExpiration = true;
    });
#endregion

ConnectionConfig.ConnectionString = builder.Configuration.GetConnectionString("ETicaretContext");
//IConfigurationSection section = builder.Configuration.GetSection("AppSettings");
IConfigurationSection section = builder.Configuration.GetSection(nameof(AppSettings));
section.Bind(new AppSettings());

// IoC Container: Inversion of Control Container: baðýmlýlýklarýn yönetimi: Autofac, Ninject
//builder.Services.AddDbContext<ETicaretContext>();
//builder.Services.AddScoped<KategoriRepoBase, KategoriRepo>();
builder.Services.AddScoped<IKategoriService, KategoriService>();
//builder.Services.AddSingleton<IKategoriService, KategoriService>();
//builder.Services.AddTransient<IKategoriService, KategoriService>();
builder.Services.AddScoped<IUrunService, UrunService>();
builder.Services.AddScoped<IHesapService, HesapService>();
builder.Services.AddScoped<IKullaniciService, KullaniciService>();
builder.Services.AddScoped<IUlkeService, UlkeService>();
builder.Services.AddScoped<ISehirService, SehirService>();
builder.Services.AddScoped<IMagazaService, MagazaService>();

var app = builder.Build();

app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(_cultures.FirstOrDefault().Name),
    SupportedCultures = _cultures,
    SupportedUICultures = _cultures
});

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

#region Authentication
app.UseAuthentication();
#endregion

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
