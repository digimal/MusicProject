using Microsoft.AspNetCore.Identity;
using MvcProject.Dal;
using MvcProject.Domain;
using Microsoft.EntityFrameworkCore;
using MvcProject.Bll.App;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MusicContextConnection") ?? throw new InvalidOperationException("Connection string 'MusicContextConnection' not found.");

builder.Services.AddDbContext<MusicContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>()
           .AddRoles<Role>()
           .AddDefaultUI()
           .AddEntityFrameworkStores<MusicContext>()
                           .AddDefaultTokenProviders();

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User, Role>>();

builder.Services.InitializeBll();

builder.Services.AddMvc()
    .AddRazorRuntimeCompilation()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options => {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(MvcProject.Bll.Resources.SharedResource).GetTypeInfo().Assembly.FullName);

            return factory.Create("SharedResource", assemblyName.Name);
        };
    });

//builder.Services.AddControllersWithViews()
//    .AddRazorRuntimeCompilation()
//    .AddDataAnnotationsLocalization()
//    .AddViewLocalization();

//builder.Services.AddRazorPages()
//    .AddRazorRuntimeCompilation()
//    .AddDataAnnotationsLocalization()
//    .AddViewLocalization();

builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureApplicationCookie(conf => { });

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("uk")
    };

    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var catalogContext = scopedProvider.GetRequiredService<MusicContext>();
        await MusicContextSeed.SeedAsync(catalogContext, app.Logger);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
    //endpoints.MapBlazorHub("/admin");
    endpoints.MapFallbackToFile("index.html");
});

app.Run();
