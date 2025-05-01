using System.Diagnostics;
using Cassandra;
using Cassandra.Data.Linq;
using ZnModelModule.Shared.InternalCassandra.Storage;

namespace IziCassandra.Application
{
    public class CassandraClient
    {
        private readonly Guid guid = Guid.Parse("8e142080-7ea4-4e46-b5e3-4db7e4d43450");
        private readonly Cluster cluster;
        private readonly Cassandra.ISession sessionShared;
        private PreparedStatement statement;
        private PreparedStatement statementWithLimit;

        public CassandraClient()
        {
            var address = "192.168.190.143";
            var port = 9042;
            var socketOptions = new SocketOptions()
    .SetReadTimeoutMillis(60 * 60 * 24);

            this.cluster = Cluster.Builder()
                .AddContactPoint(address)
                .WithPort(port)
                .WithSocketOptions(socketOptions)
                .WithQueryTimeout(60 * 60 * 24)
                .WithQueryOptions(new QueryOptions().SetPageSize(100000))
                // увеличние размера страницы не влияет на скорость выполнения в текущем кейсе
                // уменьшение размера страницы увеличивает скорость выполнения а также количество аллоцируемой памяти в текущем кейсе
                .Build();

            this.sessionShared = cluster.Connect();
            var c = @$"
select *
from hottsdb.computeitemvalue
where uid = ?
AND time >= ? AND time < ?";

            var cqlLimited = @$"
select *
from hottsdb.computeitemvalue
where uid = ?
limit ?";
            this.statement = sessionShared.Prepare(c);
            this.statementWithLimit = sessionShared.Prepare(cqlLimited);

        }
        public async Task<IEnumerable<ValueAtDate>> QAsync(DateTime from, DateTime to)
        {
            var root = ActivityProj.source.StartActivity("QAsync.Root", ActivityKind.Internal, Activity.Current?.Context ?? default);

            try
            {
                var aConnectionStart = ActivityProj.source.StartActivity("QAsync.Root.Connection.Start", ActivityKind.Internal, root?.Context ?? default);
                var session = await cluster.ConnectAsync();
                aConnectionStart?.Stop();

                var aQuery = ActivityProj.source.StartActivity("QAsync.Root.Query.Start", ActivityKind.Internal, root?.Context ?? default);

                var c = @$"
select *
from hottsdb.computeitemvalue
where uid = {guid} 
AND time >= {new DateTimeOffset(from).ToUnixTimeMilliseconds()} AND time < {new DateTimeOffset(to).ToUnixTimeMilliseconds()}";
                var response = await session.ExecuteAsync(new SimpleStatement(c));
                aQuery?.Stop();
                var aMat = ActivityProj.source.StartActivity("QAsync.Root.Query.Materialization", ActivityKind.Internal, root?.Context ?? default);

                var q = response.Select(x => (
                 x.GetValue<long>("time"),
                 x.GetValue<double?>("value")));
                var result = q.Select(x => new ValueAtDate(DateTimeOffset.FromUnixTimeMilliseconds(x.Item1).UtcDateTime, x.Item2)).ToArray();
                aMat?.Stop();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ValueAtDate>> QAsyncWithTable()
        {
            var session = await cluster.ConnectAsync("HotTsdb");
            var usersTable = new Table<ValueModel>(session);
            var result = await usersTable.Where(x => x.Uid == guid).ExecuteAsync();
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ValueAtDate>> QAsyncWithKeySpace()
        {
            var address = "192.168.190.143";
            var port = 9042;

            var cluster = Cluster.Builder()
                     .AddContactPoint(address)
                     .WithPort(port)
                     .Build();

            try
            {
                var session = await cluster.ConnectAsync();
                var c = "select *\r\nfrom hottsdb.computeitemvalue \r\nwhere uid = 8e142080-7ea4-4e46-b5e3-4db7e4d43450\r\nlimit 2000";
                var response = await session.ExecuteAsync(new SimpleStatement(c));
                var q = response.Select(x => (
                 x.GetValue<long>("time"),
                 x.GetValue<double?>("value")));

                var result = q.Select(x => new ValueAtDate(DateTimeOffset.FromUnixTimeMilliseconds(x.Item1).UtcDateTime, x.Item2)).ToArray();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> QReadOnlySharedSession(DateTime from, DateTime to)
        {
            var root = ActivityProj.source.StartActivity("QAsync.Root", ActivityKind.Internal, Activity.Current?.Context ?? default);

            try
            {
                var aQuery = ActivityProj.source.StartActivity("QAsync.Root.Query.Start", ActivityKind.Internal, root?.Context ?? default);

                var c = @$"
select *
from hottsdb.computeitemvalue
where uid = {guid} 
AND time >= {new DateTimeOffset(from).ToUnixTimeMilliseconds()} AND time < {new DateTimeOffset(to).ToUnixTimeMilliseconds()}";
                var response = await sessionShared.ExecuteAsync(new SimpleStatement(c));
                aQuery?.Stop();
                var aMat = ActivityProj.source.StartActivity("QAsync.Root.Query.Materialization", ActivityKind.Internal, root?.Context ?? default);
                int readed = 0;
                foreach (var row in response)
                {
                    readed++;
                    var time = row.GetValue<long>("time");
                    var value = row.GetValue<double?>("value");
                }
                return readed;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> QReadOnly(DateTime from, DateTime to)
        {
            var root = ActivityProj.source.StartActivity("QAsync.Root", ActivityKind.Internal, Activity.Current?.Context ?? default);

            try
            {
                var aConnectionStart = ActivityProj.source.StartActivity("QAsync.Root.Connection.Start", ActivityKind.Internal, root?.Context ?? default);
                var session = await cluster.ConnectAsync();
                aConnectionStart?.Stop();

                var aQuery = ActivityProj.source.StartActivity("QAsync.Root.Query.Start", ActivityKind.Internal, root?.Context ?? default);

                var c = @$"
select *
from hottsdb.computeitemvalue
where uid = {guid} 
AND time >= {new DateTimeOffset(from).ToUnixTimeMilliseconds()} AND time < {new DateTimeOffset(to).ToUnixTimeMilliseconds()}";
                var response = await session.ExecuteAsync(new SimpleStatement(c));
                aQuery?.Stop();
                var aMat = ActivityProj.source.StartActivity("QAsync.Root.Query.Materialization", ActivityKind.Internal, root?.Context ?? default);
                int readed = 0;
                foreach (var row in response)
                {
                    readed++;
                    var time = row.GetValue<long>("time");
                    var value = row.GetValue<double?>("value");
                }
                return readed;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 11.789s - one read
        /// 1.392s - start 
        /// 10.397s - read
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public async Task<int> QReadOnlyPrepared(DateTime from, DateTime to)
        {
            using var root = ActivityProj.source.StartActivity("QAsync.Root", ActivityKind.Internal, Activity.Current?.Context ?? default);

            try
            {
                var aQuery = ActivityProj.source.StartActivity("QAsync.Root.Query.Start", ActivityKind.Internal, root?.Context ?? default);
                var binded = this.statement.Bind(guid, new DateTimeOffset(from).ToUnixTimeMilliseconds(), new DateTimeOffset(to).ToUnixTimeMilliseconds());
                var response = await sessionShared.ExecuteAsync(binded);
                aQuery?.Stop();
                var aMat = ActivityProj.source.StartActivity("QAsync.Root.Query.Read", ActivityKind.Internal, root?.Context ?? default);
                int readed = 0;
                foreach (var row in response)
                {
                    readed++;
                    //var time = row.GetValue<long>("time");
                    //var value = row.GetValue<double?>("value");
                }
                aMat?.Stop();
                return readed;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 10.752s - one read (фактически нет разницы когда нет where)
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<int> QReadOnlyPrepared(int limit)
        {
            using var root = ActivityProj.source.StartActivity("QAsync.Root", ActivityKind.Internal, Activity.Current?.Context ?? default);

            try
            {
                var aQuery = ActivityProj.source.StartActivity("QAsync.Root.Query.Start", ActivityKind.Internal, root?.Context ?? default);
                var binded = this.statementWithLimit.Bind(guid, limit);
                var response = await sessionShared.ExecuteAsync(binded);
                aQuery?.Stop();
                var aMat = ActivityProj.source.StartActivity("QAsync.Root.Query.Read", ActivityKind.Internal, root?.Context ?? default);
                int readed = 0;
                foreach (var row in response)
                {
                    readed++;
                    //var time = row.GetValue<long>("time");
                    //var value = row.GetValue<double?>("value");
                }
                aMat?.Stop();
                return readed;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
public class ValueModel
{
    public Guid Uid { get; set; }
    public long Time { get; set; }
    public double? Value { get; set; }
}
