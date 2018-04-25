using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Common.AutoMapper
{
    public static class AutoMapperHelper
    {
        private static readonly MapperConfiguration _mapperConfiguration;

        static AutoMapperHelper()
        {
            _mapperConfiguration = new MapperConfiguration(ConfigureProfiles);
        }

        private static void ConfigureProfiles(IMapperConfiguration mapperConfiguration)
        {
            mapperConfiguration.AddProfile(new ViewModelToModelMap());
        }

        public static IMapper GetInstance()
        {
            return _mapperConfiguration.CreateMapper();
        }
    }
}
