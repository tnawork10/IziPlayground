using System;

namespace ZnModelModule.Shared.InternalCassandra.Storage
{
    public struct ValueAtDate
    {
        public long timestamp;
        public double? value;

        public DateTime DateTime => DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
        public double? Value => value;

        private ValueAtDate(long unixTimestampAsMilliseconds, double? value)
        {
            this.timestamp = unixTimestampAsMilliseconds;
            this.value = value;
        }

        public ValueAtDate(DateTime dt, double? value)
        {
            this.timestamp = (long)(dt - DateTime.UnixEpoch).TotalMilliseconds;
            this.value = value;
        }

        public TimeSpan TimestampAsDateTimeSpan()
        {
            return TimeSpan.FromMilliseconds(timestamp);
        }

        public long GetLowerBoundryOfTimestampAsMilliseconds(TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.FromSeconds(1))
            {
                throw new NotImplementedException($"Нет реализации получения нижней границы для TimeSpan={timeSpan}");
            }
            else if (timeSpan == TimeSpan.FromMinutes(1))
            {
                return GetLowerBoundryOfTimestampWithTimeframeAtMinuteAsMilliseconds();
            }
            else if (timeSpan == TimeSpan.FromHours(1))
            {
                return GetLowerBoundryOfTimestampHourlyAsMilliseconds();
            }
            else if (timeSpan == TimeSpan.FromDays(1))
            {
                return GetLowerBoundryOfTimestampDailyAsMilliseconds();
            }
            else
            {
                throw new NotImplementedException($"Нет реализации получения нижней границы для TimeSpan={timeSpan}");
            }
        }
        /// <summary>
        /// Получить временную метку в милисекундах значения, округленного по нижней границе до ближайшей минуты
        /// </summary>
        public long GetLowerBoundryOfTimestampWithTimeframeAtMinuteAsMilliseconds()
        {
            return (long)TimeSpan.FromHours(TimestampAsDateTimeSpan().TotalMinutes).TotalMilliseconds;
        }
        /// <summary>
        /// Получить временную метку в милисекундах значения, округленного по верхней границе до ближайшей минуты
        /// </summary>
        public long GetHigherBoundryOfTimestampWithTimeframeAtMinuteAsMilliseconds()
        {
            return GetLowerBoundryOfTimestampWithTimeframeAtMinuteAsMilliseconds() + (long)TimeSpan.FromMinutes(1).TotalMinutes;
        }

        /// <summary>
        /// Получить временную метку в милисекундах значения, округленного по нижней границе до ближайшего часа
        /// </summary>
        public long GetLowerBoundryOfTimestampHourlyAsMilliseconds()
        {
            return (long)TimeSpan.FromHours(TimestampAsDateTimeSpan().TotalHours).TotalMilliseconds;
        }

        /// <summary>
        /// Получить временную метку в милисекундах значения, округленного по верхней границе до ближайшего часа
        /// </summary>
        public long GetHigherBoundryOfTimestampHourlyAsMilliseconds()
        {
            return GetLowerBoundryOfTimestampHourlyAsMilliseconds() + (long)TimeSpan.FromHours(1).TotalMilliseconds;
        }


        /// <summary>
        /// Получить временную метку в милисекундах значения, округленного по нижней границе до ближайшего часа
        /// </summary>
        public long GetLowerBoundryOfTimestampDailyAsMilliseconds()
        {
            return (long)TimeSpan.FromDays(TimestampAsDateTimeSpan().TotalDays).TotalMilliseconds;
        }

        /// <summary>
        /// Получить временную метку в милисекундах значения, округленного по верхней границе до ближайшего часа
        /// </summary>
        public long GetHigherBoundryOfTimestampDailyAsMilliseconds()
        {
            return GetLowerBoundryOfTimestampDailyAsMilliseconds() + (long)TimeSpan.FromHours(1).TotalMilliseconds;
        }

        public static ValueAtDate FromSeconds(long seconds, double? value)
        {
            return FromMilliseconds(seconds * 1000, value);
        }

        public static ValueAtDate FromMilliseconds(long milliseconds, double? value)
        {
            return new ValueAtDate(milliseconds, value);
        }

        public void AddValue(double? valueAdd)
        {
            value += valueAdd;
        }

        public void AvaragedBy(int countAtHourlySpan)
        {
            value /= countAtHourlySpan;
        }
    }
}
