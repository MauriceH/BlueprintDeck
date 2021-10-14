using System.IO;
using System.Text;
using System.Text.Json;
using BlueprintDeck.Design;
using BlueprintDeck.Instance;
using BlueprintDeck.Instance.Factory;

namespace BlueprintDeck.AspNetCoreTestApp
{
    public class BlueprintInstance
    {
        private readonly IBlueprintFactory _factory;
        private IBlueprintInstance _blueprint;
        private readonly string _activeBlueprintFileName;

        public Blueprint DesignBlueprint { get; private set; }
        
        public BlueprintInstance(IBlueprintFactory factory)
        {
            _factory = factory;
            _activeBlueprintFileName = @"C:\temp\BluePrint\active.json";
            if (File.Exists(_activeBlueprintFileName))
            {
                var json = File.ReadAllText(_activeBlueprintFileName,Encoding.UTF8);
                DesignBlueprint = JsonSerializer.Deserialize<Blueprint>(json)!;    
            }
            else
            {
                DesignBlueprint = new Blueprint();
            }
            
        }

        public void Start()
        {
            var json = JsonSerializer.Serialize(DesignBlueprint)!;
            File.WriteAllText(@"C:\temp\BluePrint\active.json",json, Encoding.UTF8);
            _blueprint?.Dispose();
            _blueprint = _factory.CreateBlueprint(DesignBlueprint);
            _blueprint.Activate();
        }
        
        public void Start(Blueprint design)
        {
            DesignBlueprint = design;
            Start();
        }
        
    }
}