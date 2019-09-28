using System;
using System.Reactive.Subjects;
using BlueprintDeck.Node.Ports.Definitions.DataTypes;

namespace BlueprintDeck.Node.Ports
{
    public class DataOutput<T> : IOutput<T> where T : IPortData
    {
        private readonly ReplaySubject<T> _replaySubject;

        public DataOutput()
        {
            _replaySubject = new ReplaySubject<T>(1024);
        }

        public IObservable<T> Observable => _replaySubject;
        
        public void Emit(T data)
        {
            _replaySubject.OnNext(data);
        }
    }
}