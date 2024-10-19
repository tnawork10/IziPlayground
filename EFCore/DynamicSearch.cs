using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IziHardGames.Playgrounds.ForEfCore
{
    public interface ISearchProvider<T>
    {
        Func<T, SearchItem, bool> this[string key] { get; }
        bool TryGetCached(string key, out ISearch<T> existed);
        ISearch<T> GetContcatinationByDefault(SearchGroup group)
        {
            if (TryGetCached(group.Id, out var existed))
            {
                return existed;
            }
            var list = new List<Func<T, bool>>();
            foreach (var item in group.Items!)
            {
                var func = this[item.Id];
                list.Add((x) => func(x, item));
            }
            var search = new SearchAny<T>(list.ToFrozenSet());
            return search;
        }
    }
    public static class DynamicSearch
    {
        public static IQueryable Map<T>(this ISearchProvider<T> mapper, DbSet<T> set, SearchSchema searchOptions) where T : class
        {
            var query = set.AsQueryable();

            foreach (var groupe in searchOptions.Groups!)
            {
                query = query.Where(x => mapper.GetContcatinationByDefault(groupe).IsMetCondition(x));
            }
            return query;
        }
    }

    public class SearchSchema
    {
        public IEnumerable<SearchGroup>? Groups { get; set; }
    }

    public class SearchGroup
    {
        public string Id { get; set; } = null!;
        public IEnumerable<SearchItem>? Items { get; set; }
    }

    [System.Serializable]
    public class SearchItem
    {
        public string Id { get; set; } = null!;
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
        public ECompareAlgo CompareAlgo { get; set; }
    }

    public interface ISearch<T>
    {
        bool IsMetCondition(T item);
    }

    internal class SearchAny<T> : ISearch<T>, IDisposable
    {
        public IEnumerable<Func<T, bool>> funcsAny { get; set; }
        private static readonly Func<T, bool> hotPath = (T item) => true;
        private Func<T, bool> handler;
        private Func<T, bool> handlerAlt;

        public SearchAny()
        {
            handler = hotPath;
            handlerAlt = Handle;
            funcsAny = Enumerable.Empty<Func<T, bool>>();
        }

        public SearchAny(IEnumerable<Func<T, bool>> funcs)
        {
            this.funcsAny = funcs;
            handlerAlt = Handle;
            handler = handlerAlt;
        }

        public bool IsMetCondition(T item)
        {
            return handler(item);
        }
        private bool Handle(T item)
        {
            if (funcsAny is null)
            {
                return true;
            }
            foreach (var f in funcsAny)
            {
                if (f(item)) return true;
            }
            return false;
        }

        public void Dispose()
        {
            funcsAny = Enumerable.Empty<Func<T, bool>>();
        }
    }
    public enum ECompareAlgo
    {
        None = 0,
        StartWith,
        Contain,
        Equals,
        Except,
    }
}
