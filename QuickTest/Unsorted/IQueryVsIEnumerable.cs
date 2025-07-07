using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;

public class IQueryVsIEnumerable
{

    private class MyQuery : IQueryable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Type ElementType { get; }
        public Expression Expression { get; }
        public IQueryProvider Provider { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}