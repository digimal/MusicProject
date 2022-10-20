using Autofac;
using MvcProject.Dal;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Bll.Services.Concrete;

namespace MvcProject.DI
{
    public static class DependecyInjection
    {
        public static ContainerBuilder Register(ContainerBuilder builder)
        {
            builder.RegisterType<MusicContext>().InstancePerRequest();
            builder.RegisterType<ArtistService>().As<IArtistService>();
            builder.RegisterType<PictureService>().As<IPictureService>();
            builder.RegisterType<ArtistTagService>().As<IArtistTagService>();
            builder.RegisterType<ArtistAssignedTagService>().As<IArtistAssignedTagService>();
            builder.RegisterType<ArtistRelationService>().As<IArtistRelationService>();
            builder.RegisterType<ArtistRelationTypeService>().As<IArtistRelationTypeService>();
            builder.RegisterType<ArtistLikeService>().As<IArtistLikeService>();
            return builder;
        }
    }
}
