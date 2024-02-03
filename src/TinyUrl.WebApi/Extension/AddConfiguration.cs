﻿using Microsoft.Extensions.Options;

namespace TinyUrl.WebApi.Extension
{
    public static class AddConfigurationExtension
    {
        public static IServiceCollection AddConfiguration<TImplementation>(this IServiceCollection services,
            ConfigurationManager configuration, string sectionName)
            where TImplementation : class, new()

        {
            var configInstance = new TImplementation();

            configuration.GetSection(sectionName).Bind(configInstance);
            services.AddSingleton<TImplementation>(configInstance);

            return services;
        }
    }
}
