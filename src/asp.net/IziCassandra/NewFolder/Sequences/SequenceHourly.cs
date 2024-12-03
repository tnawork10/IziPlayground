using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ZnModelModule.Shared.InternalCassandra.Storage
{
    public struct SequenceHourly
    {
        private IEnumerable<ValueAtDate> sequence;

        public SequenceHourly(IEnumerable<ValueAtDate> sequence)
        {
            this.sequence = sequence;
        }
        public SequenceHourly(SequenceAggregator sequenceAggregator)
        {
            this.sequence = sequenceAggregator.GetValues();
        }

        public IEnumerable<ValueAtDate> GetValuesAvaraged()
        {
            var iterator = new IteratorForFixedPeriods(sequence);
            var result = iterator.GetValues(TimeSpan.FromHours(1), EAggregationType.Average);//.ToArray();
            return result;
        }

        public static long AddHours(long timestampAsMilliseconds, int hours)
        {
            return (long)TimeSpan.FromMilliseconds(timestampAsMilliseconds).Add(TimeSpan.FromHours(hours)).TotalMilliseconds;
        }

        public SequenceDaily GetDailyFromAvaregedHourly()
        {
            var averagedHourly = GetValuesAvaraged();//.ToArray();
            return new SequenceDaily(averagedHourly);
        }
    }
}
