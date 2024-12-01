using System.Collections;

namespace Enumerating
{
    public class Class1 : IEnumerable<EnumValue>
    {
        public IEnumerator<EnumValue> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public struct EnumValue
    {

    }
}
