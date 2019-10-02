using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using BlueprintDeck.Design;
using BlueprintDeck.Registration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;

namespace BlueprintDeck.PrototypeTestApp
{
    class Program
    {
        private const LogEventLevel LogLevel = LogEventLevel.Verbose;

        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterBlueprintDeck(c =>
            {
                var thisAssembly = Assembly.GetExecutingAssembly();
                c.RegisterAssemblyNodes(thisAssembly);
                
            });
            
            RegisterLogger(builder);

            var container = builder.Build();

            var nodeTypes = container.Resolve<IEnumerable<NodeRegistration>>();
            var json = JsonConvert.SerializeObject(new {Nodes = nodeTypes.ToList()}, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            
            var factory = container.Resolve<BluePrintFactory>();
            var design = TestDesign.CreateDesign();
            var bluePrint = factory.CreateBluePrint(design);
            bluePrint.Activate();
            
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "exit") return;
            }
        }

        private static void RegisterLogger(ContainerBuilder builder)
        {
            var logEventLevel = LogLevel;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(logEventLevel)
                .Enrich.FromLogContext()
                .MinimumLevel.Verbose()
                .WriteTo.Console(logEventLevel, outputTemplate:
                    "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
            var loggerFactory = new LoggerFactory().AddSerilog(Log.Logger);
            builder.RegisterInstance(loggerFactory)
                .As<ILoggerFactory>()
                .SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
        }

        
    }
}