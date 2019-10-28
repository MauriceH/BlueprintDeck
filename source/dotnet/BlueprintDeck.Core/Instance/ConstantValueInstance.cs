using System;
using System.Reactive.Subjects;

namespace BlueprintDeck.Instance
{
    public class ConstantValueInstance
    {
        private readonly Subject<object> _subject;
        public IObservable<object> Observable => _subject;
        
        public Design.ConstantValue Description { get; }
        
        public Type DataType { get; }
        public object Value { get; set; }

        public ConstantValueInstance(Design.ConstantValue constantValue, Type dataType)
        {
            Description = constantValue;
            DataType = dataType;
            _subject = new Subject<object>();
        }


        public void Activate()
        {
            _subject.OnNext(Value);
        }
    }
}