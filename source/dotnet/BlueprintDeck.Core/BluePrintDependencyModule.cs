using Autofac;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck
{
    public class BluePrintDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<PortAutofacModule>();
            builder.RegisterType<BluePrintFactory>().AsSelf().InstancePerDependency();
            builder.RegisterType<AutofacNodeRepository>().As<INodeRepository>().InstancePerDependency();
            builder.RegisterType<TestNode>().AsSelf().As<INode>().InstancePerDependency();
            builder.RegisterType<TestNode.Descriptor>().AsSelf().InstancePerDependency();
            
            builder.RegisterType<ActivateNode>().AsSelf().As<INode>().InstancePerDependency();
            builder.RegisterType<ActivateNodeDescriptor>().AsSelf().InstancePerDependency();
            
            builder.RegisterType<DelayNode>().AsSelf().As<INode>().InstancePerDependency();
            builder.RegisterType<DelayNodeDescriptor>().AsSelf().InstancePerDependency();
            
        }
    }
}