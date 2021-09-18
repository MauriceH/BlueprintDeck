using System;
using System.Reactive.Subjects;

namespace BlueprintDeck.Node.Ports
{
    public class SimpleOutput : IOutput
    {
        private readonly ReplaySubject<object> _replaySubject;

        public SimpleOutput()
        {
            _replaySubject = new ReplaySubject<object>(1024);
        }

        public IObservable<object> Observable => _replaySubject;

        public void Emit()
        {
            _replaySubject.OnNext(new object());
        }
    }
}