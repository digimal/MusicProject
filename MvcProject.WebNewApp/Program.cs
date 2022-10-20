using Microsoft.AspNetCore.Identity;
using MvcProject.Dal;
using MvcProject.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Win32;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Bll.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MusicContextConnection") ?? throw new InvalidOperationException("Connection string 'MusicContextConnection' not found.");

builder.Services.AddDbContext<MusicContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>()
           .AddDefaultUI()
           .AddEntityFrameworkStores<MusicContext>()
                           .AddDefaultTokenProviders();

builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IPictureService, PictureService>();
builder.Services.AddScoped<IArtistTagService, ArtistTagService>();
builder.Services.AddScoped<IArtistAssignedTagService, ArtistAssignedTagService>();
builder.Services.AddScoped<IArtistRelationService, ArtistRelationService>();
builder.Services.AddScoped<IArtistRelationTypeService, ArtistRelationTypeService>();
builder.Services.AddScoped<IArtistLikeService, ArtistLikeService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMvc(setupAction => { });
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
    //endpoints.MapBlazorHub("/admin");
    endpoints.MapFallbackToFile("index.html");
});

app.Run();
