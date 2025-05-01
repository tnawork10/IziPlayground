using System;
using System.Collections.Generic;

namespace ZnModelModule.Shared.InternalCassandra.Storage
{
    public struct SequenceDaily
    {
        public IEnumerable<ValueAtDate> Sequence { get; }

        public SequenceDaily(IEnumerable<ValueAtDate> averagedHourly)
        {
            this.Sequence = averagedHourly;
        }

        public IEnumerable<ValueAtDate> GetSum()
        {
            var itor = new IteratorForFixedPeriods(Sequence);
            var values = itor.GetValues(TimeSpan.FromDays(1), EAggregationType.Sum);
            return values;
        }

        public ValueAtDateSequence ToSequenceAsSum()
        {
            var resultSeq = GetSum();//.ToArray();
            return new ValueAtDateSequence(resultSeq);
        }
    }
}
