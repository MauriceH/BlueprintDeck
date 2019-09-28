using Autofac;
using BlueprintDeck.Node.Ports.Definitions.DataTypes;

namespace BlueprintDeck.Node.Ports.Definitions
{
    public class PortAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PdtInt32DataNode>().AsSelf().As<INode>().InstancePerDependency();
            builder.RegisterType<PdtInt32DataNode.Descriptor>().AsSelf().InstancePerDependency();
            
            builder.RegisterType<PdtDurationDataNode>().AsSelf().As<INode>().InstancePerDependency();
            builder.RegisterType<PdtDurationDataNode.Descriptor>().AsSelf().InstancePerDependency();
        }
    }
}