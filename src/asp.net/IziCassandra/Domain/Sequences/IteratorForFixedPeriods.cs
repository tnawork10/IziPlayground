namespace ZnModelModule.Shared.InternalCassandra.Storage
{
    public struct IteratorForFixedPeriods
    {
        private IEnumerable<ValueAtDate> sequence;
        public int Counter { get; private set; }

        public IteratorForFixedPeriods(IEnumerable<ValueAtDate> sequence)
        {
            ArgumentNullException.ThrowIfNull(sequence);
            this.sequence = sequence;
        }

        /// <summary>
        /// Получить усрдненное по часам значение
        /// </summary>
        /// <param name="offset">Сдвиг по часовому поясу</param>
        /// <returns></returns>
        /// <see cref="Trend{TValue}.ReduceTrend"/>
        public IEnumerable<ValueAtDate> GetValues(TimeSpan interval, EAggregationType aggregationType)
        {
            var currentDt = DateTime.MinValue;
            var currentDate = DateOnly.MinValue;

            var etor = sequence.GetEnumerator();
            var timeSpanMilliseconds = interval.TotalMilliseconds;
            // если открыли самое первое часовое окно
            if (etor.MoveNext())
            {
                this.Counter = 1;
                var valFirst = etor.Current;
                // берем верхнюю границу самого первого часового окна
                var openTimestamp = valFirst.GetLowerBoundryOfTimestampAsMilliseconds(interval);
                // устанавливаем окно закрытия  до начала следующего часа (исключительно)
                var closingTimestamp = openTimestamp + timeSpanMilliseconds;
#if DEBUG
                var dateTime = new DateTime(year: 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.Add(TimeSpan.FromMilliseconds(closingTimestamp));
#endif
                var currentValueAgr = new ValueAtDateAggregator(ValueAtDate.FromMilliseconds(openTimestamp, valFirst.value));
                int countAtHourlySpan = 1;

                while (etor.MoveNext())
                {
                    var val = etor.Current;
                    if (closingTimestamp <= val.timestamp)
                    {
                        var result = currentValueAgr.Aggregate(aggregationType);
                        // закрываем текущее окно
                        yield return result;
                        openTimestamp = val.GetLowerBoundryOfTimestampAsMilliseconds(interval);
                        closingTimestamp = openTimestamp + timeSpanMilliseconds;
                        // и создаем новое окно
                        currentValueAgr = new ValueAtDateAggregator(ValueAtDate.FromMilliseconds(openTimestamp, val.value));
                        countAtHourlySpan = 1;
                    }
                    else
                    {
#if DEBUG
                        // если получили более раннюю метку времени чем текущая, то значит последовательность не отсортирована
                        if (val.timestamp < currentValueAgr.valueAtDate.timestamp)
                        {
                            throw new Exception($"Последовательность должна быть отсортирована от ранней даты к поздней. Текущий часовой интервал:c {currentValueAgr.Timestamp} по {closingTimestamp}. Получен: {val.timestamp}");
                        }
#endif
                        // если остаемся в текущем окне то аккомулируем его
                        currentValueAgr.AddValue(val.value);
                        countAtHourlySpan++;
                    }
                    this.Counter++;
                }
                // закрываем последнее окно
                var resultLast = currentValueAgr.Aggregate(aggregationType);
                yield return resultLast;
            }
        }

        /// <summary>
        /// Получить усрдненное по часам значение
        /// </summary>
        /// <param name="offset">Сдвиг по часовому поясу</param>
        /// <returns></returns>
        /// <see cref="Trend{TValue}.ReduceTrend"/>
        public IEnumerable<ValueAtDate> GetValuesAverage(TimeSpan timeSpan)
        {
            var currentDt = DateTime.MinValue;
            var currentDate = DateOnly.MinValue;

            var etor = sequence.GetEnumerator();
            var timeSpanMilliseconds = timeSpan.TotalMilliseconds;
            // если открыли самое первое часовое окно
            if (etor.MoveNext())
            {
                this.Counter = 1;
                var valFirst = etor.Current;
                // берем верхнюю границу самого первого часового окна
                var openTimestamp = valFirst.GetLowerBoundryOfTimestampAsMilliseconds(timeSpan);
                // устанавливаем окно закрытия  до начала следующего часа (исключительно)
                var closingTimestamp = openTimestamp + timeSpanMilliseconds;
#if DEBUG
                var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.Add(TimeSpan.FromMilliseconds(closingTimestamp));
#endif      
                var currentValue = ValueAtDate.FromMilliseconds(openTimestamp, valFirst.value);
                int countAtHourlySpan = 1;

                while (etor.MoveNext())
                {
                    var val = etor.Current;
                    if (closingTimestamp <= val.timestamp)
                    {
                        // высчитываем срднее 
                        currentValue.AvaragedBy(countAtHourlySpan);
                        // закрываем текущее окно
                        yield return currentValue;
                        openTimestamp = val.GetLowerBoundryOfTimestampAsMilliseconds(timeSpan);
                        closingTimestamp = openTimestamp + timeSpanMilliseconds;
                        // и создаем новое окно
                        currentValue = ValueAtDate.FromMilliseconds(openTimestamp, val.value);
                        countAtHourlySpan = 1;
                    }
                    else
                    {
#if DEBUG
                        // если получили более раннюю метку времени чем текущая, то значит последовательность не отсортирована
                        if (val.timestamp < currentValue.timestamp)
                        {
                            throw new Exception($"Последовательность должна быть отсортирована от ранней даты к поздней. Текущий часовой интервал:c {currentValue.timestamp} по {closingTimestamp}. Получен: {currentValue.timestamp}");
                        }
#endif
                        // если остаемся в текущем окне то аккомулируем его
                        currentValue.AddValue(val.value);
                        countAtHourlySpan++;
                    }
                    this.Counter++;
                }
                // закрываем последнее окно
                currentValue.AvaragedBy(countAtHourlySpan);
                yield return currentValue;
            }
        }
    }
}
