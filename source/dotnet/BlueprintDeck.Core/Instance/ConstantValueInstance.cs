using System;
using System.Reactive.Subjects;

namespace BlueprintDeck.Instance
{
    public class ConstantValueInstance
    {
        private readonly Subject<object> _subject;
        public IObservable<object> Observable => _subject;
        
        public Design.ConstantValue Descriptor { get; }
        
        public Type DataType { get; }
        public object? CurrentValue { get; set; }

        public ConstantValueInstance(Design.ConstantValue constantValue, Type dataType)
        {
            Descriptor = constantValue;
            DataType = dataType;
            _subject = new Subject<object>();
        }

        public void Activate()
        {
            if(CurrentValue == null) throw new Exception("Current value not available");
            _subject.OnNext(CurrentValue);
        }
    }
}