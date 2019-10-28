using System;
using System.Reactive.Subjects;

namespace BlueprintDeck.Node.Ports
{
    public class DataOutput<T> : IOutput<T>
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