using System.IO;
using System.Text;
using BlueprintDeck.Design;
using BlueprintDeck.Instance.Factory;

namespace BlueprintDeck.AspNetCoreTestApp
{
    public class BluePrintInstance
    {
        private readonly IBluePrintFactory _factory;
        private Instance.BluePrint _bluePrint;

        public BluePrint DesignBluePrint { get; private set; }
        
        public BluePrintInstance(IBluePrintFactory factory)
        {
            _factory = factory;
            var json = File.ReadAllText(@"C:\temp\BluePrint\active.json",Encoding.UTF8);
            DesignBluePrint = System.Text.Json.JsonSerializer.Deserialize<BluePrint>(json)!;
        }

        public void Start()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(DesignBluePrint)!;
            File.WriteAllText(@"C:\temp\BluePrint\active.json",json, Encoding.UTF8);
            _bluePrint?.Dispose();
            _bluePrint = _factory.CreateBluePrint(DesignBluePrint);
            _bluePrint.Activate();
        }
        
        public void Start(BluePrint design)
        {
            DesignBluePrint = design;
            Start();
        }
        
    }
}