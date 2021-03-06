﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hunter.WebUI
{
    public class Program
    {

        public static void Main(string[] args)
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<Managers.MapperProfile>();
            });
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:8080")
                .UseStartup<Startup>()
                .Build();
    }
}
