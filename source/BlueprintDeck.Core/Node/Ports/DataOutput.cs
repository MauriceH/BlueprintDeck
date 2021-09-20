using System;
using System.Reactive.Subjects;

namespace BlueprintDeck.Node.Ports
{
    public class DataOutput<T> : IOutput<T>
    {
        private readonly ReplaySubject<T> _subject;

        public DataOutput()
        {
            _subject = new ReplaySubject<T>(1024);
        }

        // Called by reflection
        // ReSharper disable once UnusedMember.Global
        public IObservable<T> Observable => _subject;
        
        public void Emit(T data)
        {
            _subject.OnNext(data);
        }

        public void Dispose()
        {
            _subject.Dispose();
        }
    }
}