using System;

namespace BlueprintDeck.Instance
{
    public interface IBlueprintInstance : IDisposable
    {
        void Activate();
    }
}