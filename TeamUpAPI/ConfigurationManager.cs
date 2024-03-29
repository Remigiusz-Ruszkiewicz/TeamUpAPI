﻿using System.Diagnostics.CodeAnalysis;

namespace TeamUpAPI
{
    [ExcludeFromCodeCoverage]
    internal static class ConfigurationManager
    {
        public static IConfiguration AppSetting
        {
            get;
        }
        static ConfigurationManager()
        {
            AppSetting = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        }
    }
}
