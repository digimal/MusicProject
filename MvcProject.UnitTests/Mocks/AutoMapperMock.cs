using AutoMapper;
using MvcProject.Bll.App;

namespace MvcProject.UnitTests.Mocks
{
    internal static class AutoMapperMock
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });

            return config;
        }
    }
}
