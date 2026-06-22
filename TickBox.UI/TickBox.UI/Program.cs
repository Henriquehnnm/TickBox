// This will make all the constants available in the global namespace, 
// so you can use them without the Constants prefix.
global using static TickBox.UI.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TickBox.UI;

public class Program
{
    public static void Main(string[] args)
    {
        // Required to load DLLs properly
        Gio.Module.Initialize();

        var services = CreateServices();
        
        var app = services.GetRequiredService<Application>();
        app.Run(args);
    }
    
    private static ServiceProvider CreateServices()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            })
            .AddSingleton<Application>()
            .BuildServiceProvider();

        return serviceProvider;
    }
}