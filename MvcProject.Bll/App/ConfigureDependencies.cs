using Microsoft.Extensions.DependencyInjection;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Bll.Services.Concrete;
using System;

namespace MvcProject.Bll.App
{
    public static class ConfigureDependencies
    {
        public static IServiceCollection InitializeBll(this IServiceCollection services)
        {
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IArtistTagService, ArtistTagService>();
            services.AddScoped<IArtistAssignedTagService, ArtistAssignedTagService>();
            services.AddScoped<IArtistRelationService, ArtistRelationService>();
            services.AddScoped<IArtistRelationTypeService, ArtistRelationTypeService>();
            services.AddScoped<IArtistLikeService, ArtistLikeService>();
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
