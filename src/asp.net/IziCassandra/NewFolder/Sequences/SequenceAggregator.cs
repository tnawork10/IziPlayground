using System;
using System.Collections.Generic;
using System.Linq;
using Cassandra;

namespace ZnModelModule.Shared.InternalCassandra.Storage
{
    public struct SequenceAggregator
    {
        private RowSet hotPart;
        private RowSet coolPart;
        public SequenceAggregator(RowSet hotPart, RowSet coolPart)
        {
            this.hotPart = hotPart;
            this.coolPart = coolPart;
        }

        public SequenceHourly ToHours()
        {
            return new SequenceHourly(this);
        }

        public int Count()
        {
            return hotPart.Count() + coolPart.Count();
        }

        public IEnumerable<ValueAtDate> GetValues()
        {
            // сначала более ранние данные
            foreach (var item in coolPart)
            {
                var time = item.GetValue<long>("time");
                var value = item.GetValue<double?>("value");
                yield return ValueAtDate.FromMilliseconds(time, value);
            }

            foreach (var item in hotPart)
            {
                var time = item.GetValue<long>("time");
                var value = item.GetValue<double?>("value");
                yield return ValueAtDate.FromMilliseconds(time, value);
            }
        }
    }
}
