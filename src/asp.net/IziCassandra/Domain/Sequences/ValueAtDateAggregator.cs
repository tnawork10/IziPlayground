using System;
using System.Collections.Generic;

namespace ZnModelModule.Shared.InternalCassandra.Storage
{
    public struct ValueAtDateAggregator
    {
        public ValueAtDate valueAtDate;
        public int Count { get; set; }
        public long Timestamp => valueAtDate.timestamp;
#if DEBUG
        public List<double?> DEBUG_VALUES = new List<double?>();
#endif

        public ValueAtDateAggregator(ValueAtDate valueAtDate)
        {
            this.valueAtDate = valueAtDate;
            Count = 1;
        }

        public ValueAtDate Aggregate(EAggregationType aggregationType)
        {
            switch (aggregationType)
            {
                case EAggregationType.None: goto case default;
                case EAggregationType.Sum:
                    {
                        return valueAtDate;
                    }
                case EAggregationType.Average:
                    {
                        var result = valueAtDate;
                        result.AvaragedBy(Count);
                        return result;
                    }
                default: throw new ArgumentOutOfRangeException($"Unepected type: {aggregationType}");
            }
        }

        public void AddValue(double? value)
        {
            valueAtDate.AddValue(value);
#if DEBUG
            //DEBUG_VALUES.Add(value);
#endif
            this.Count++;
        }
    }
}
