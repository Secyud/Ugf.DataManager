using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Secyud.Ugf;
using Secyud.Ugf.Modularity;
using Secyud.Ugf.Modularity.Plugins;
using Serilog;
using Serilog.Events;
using Ugf.DataManager.PluginSource;

namespace Ugf.DataManager.Blazor
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File("Logs/logs.txt"))
                .WriteTo.Async(c => c.Console())
                .CreateLogger();

            // foreach (var file in Directory.GetFiles(@"D:\Projects\infinity-world-chess\obj\Debug"))
            // {
            //     if (file.EndsWith(".dll"))
            //     {
            //         Assembly.LoadFrom(file);
            //     }
            // }
            try
            {
                Log.Information("Starting web host.");
                var builder = WebApplication.CreateBuilder(args);

                var managedPath = @"D:\Projects\infinity-world-chess\Temp\Bin\Debug\Assembly-CSharp";
                //var managedPath = $"../{builder.Configuration["GameName"]}_Data/Managed";
                
                U.DataManager = true;
                
                UgfApplicationFactory factory = new();
                factory.Create(null, typeof(DataManagerStartModule), [
                    new FolderPluginSource(managedPath),
                    new LocalSteamPluginSource()
                ]);


                builder.Host.AddAppSettingsSecretsJson()
                    .UseAutofac()
                    .UseSerilog();
                await builder.AddApplicationAsync<DataManagerBlazorModule>();
                var app = builder.Build();
                await app.InitializeApplicationAsync();
                await app.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                if (ex is HostAbortedException)
                {
                    throw;
                }

                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}