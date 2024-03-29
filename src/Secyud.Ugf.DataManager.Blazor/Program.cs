using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Secyud.Ugf.Modularity;
using Secyud.Ugf.Modularity.Plugins;
using Serilog;
using Serilog.Events;

namespace Secyud.Ugf.DataManager.Blazor;

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

        try
        {
            Log.Information("Starting web host");
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson().UseAutofac().UseSerilog();
            
            string managedPath = builder.Configuration["ConfigPath"];
                
            UgfApplicationFactory factory = new(typeof(UgfCoreModule),
            [
#if !DISABLE_STEAM_MODULE
                    new LocalSteamPluginSource(),
#endif
                new FolderPluginSource(managedPath)
            ]);
            factory.ConfigureDataManager();
            
            await builder.AddApplicationAsync<DataManagerBlazorModule>();
            WebApplication app = builder.Build();
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
            await Log.CloseAndFlushAsync();
        }
    }
}
