﻿using AutoMapper;
using TinyUrl.WebApi;

namespace Munters.TeamViewerApi.Extensions
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AllowNullDestinationValues = true;
                config.AllowNullCollections = true;
                config.AddProfile(new MapperProfile());
            });

            var mapper = configuration.CreateMapper();

            configuration.CompileMappings();
            configuration.AssertConfigurationIsValid();

            services.AddSingleton(mapper);
            return services;
        }
    }


}
