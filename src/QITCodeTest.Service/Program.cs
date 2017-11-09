﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace QITCodeTest.Service
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:50005")
                .UseStartup<Startup>()
                .Build();
    }
}