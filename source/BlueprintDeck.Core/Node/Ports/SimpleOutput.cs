using System;
using System.Reactive.Subjects;

namespace BlueprintDeck.Node.Ports
{
    public class SimpleOutput : IOutput
    {
        private readonly ReplaySubject<object> _subject;

        public SimpleOutput()
        {
            _subject = new ReplaySubject<object>(1024);
        }

        public IObservable<object> Observable => _subject;

        public void Emit()
        {
            _subject.OnNext(new object());
        }

        public void Dispose()
        {
            _subject.Dispose();
        }
    }
}