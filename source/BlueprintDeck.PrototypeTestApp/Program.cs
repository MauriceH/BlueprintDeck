using System.IO;
using System.Reflection;
using BlueprintDeck.DependencyInjection;
using BlueprintDeck.Design;
using BlueprintDeck.Design.Registry;
using BlueprintDeck.Instance.Factory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;

namespace BlueprintDeck.PrototypeTestApp
{
    class Program
    {
        private const LogEventLevel LogLevel = LogEventLevel.Verbose;

       
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {

            var services = new ServiceCollection();
            
            services.AddBlueprintDeck(c =>
            {
                var thisAssembly = Assembly.GetExecutingAssembly();
                c.RegisterAssemblyNodes(thisAssembly);
                //c.RegisterNode<TestNode>();
            });
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            
            services.AddLogging(builder =>
            {
                builder.AddSerilog();
            });

            var container = services.BuildServiceProvider();

            var registry = container.GetRequiredService<IBlueprintDeckRegistryFactory>().CreateNodeRegistry();
            var json = JsonConvert.SerializeObject(registry, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            
            File.WriteAllText(@"C:\temp\BluePrint\NodeRegistration.json",json);
            
            
            var factory = container.GetRequiredService<IBlueprintFactory>();


            var design = TestDesign.CreateDesign();
            
            json = JsonConvert.SerializeObject(design, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(@"C:\temp\BluePrint\Design.json",json);
            
            var bluePrint = factory.CreateBlueprint(design);
            bluePrint.Activate();
            
            // while (true)
            // {
            //     var line = Console.ReadLine();
            //     if (line == "exit") return;
            // }

            var bluePrint2 = factory.CreateBlueprint(new Blueprint());

            
            
        }
    }
}