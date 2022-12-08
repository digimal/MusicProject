using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.UnitTests.Mocks
{
    internal static class AutoMapperMock
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MvcProject.Bll.MapperProfile());
            });

            return config;
        }
    }
}
