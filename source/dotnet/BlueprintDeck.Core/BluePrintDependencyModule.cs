using Autofac;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports.Definitions;
using BlueprintDeck.Registration;

namespace BlueprintDeck
{
    public class BluePrintDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            
            builder.RegisterType<BluePrintFactory>().AsSelf().InstancePerDependency();
            builder.RegisterType<AutofacNodeRepository>().As<INodeRepository>().InstancePerDependency();
            

            
            
        }
    }
}