using System;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Entities.Extensions
{
    public static class OptionsBuilderExtension
    {
        public static DbContextOptionsBuilder OptionsBuilderSetting(this DbContextOptionsBuilder optionsBuilder, string connectionString)
        {
            optionsBuilder.UseSqlServer(connectionString,
            //https://learn.microsoft.com/zh-tw/ef/core/miscellaneous/connection-resiliency
            options => options.EnableRetryOnFailure());
            return optionsBuilder;
        }
    }
}

