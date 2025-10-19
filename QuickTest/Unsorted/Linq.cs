namespace QuickTest;

public class Linq
{
    public static async Task Run()
    {
        await Task.CompletedTask;
        ListCase();

    }

    public static void ArrayCase()
    {

        var eble = typeof(Program).Assembly.GetTypes().ToList();
        // https://source.dot.net/System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Array.cs.html#34fb54e46eb30d27
        // создает новый класс System.ArrayEnumerator
        var etor = eble.GetEnumerator();
        var t = etor.GetType();

        // System.RuntimeType[], System.Private.CoreLib, Version=9.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
        Console.WriteLine(eble.GetType().AssemblyQualifiedName);

        Console.WriteLine(t.AssemblyQualifiedName);
        Console.WriteLine(t.IsValueType);

        // Debug 
        // System.SZGenericArrayEnumerator`1[[System.Type, System.Private.CoreLib, Version = 9.0.0.0, Culture = neutral, PublicKeyToken = 7cec85d7bea7798e]], System.Private.CoreLib, Version = 9.0.0.0, Culture = neutral, PublicKeyToken = 7cec85d7bea7798e
        // False

        // Release
        // System.SZGenericArrayEnumerator`1[[System.Type, System.Private.CoreLib, Version = 9.0.0.0, Culture = neutral, PublicKeyToken = 7cec85d7bea7798e]], System.Private.CoreLib, Version = 9.0.0.0, Culture = neutral, PublicKeyToken = 7cec85d7bea7798e
        // False


    }
    public static void ListCase()
    {
        var eble = typeof(Program).Assembly.GetTypes().ToList();
        // https://source.dot.net/System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Array.cs.html#34fb54e46eb30d27
        // создает новый класс System.ArrayEnumerator
        var etor = eble.GetEnumerator(); // нет боксинга
        var etorBox = (eble.AsEnumerable().Where(x => x != null)).GetEnumerator();

        var t = etor.GetType();
        var tBox = etorBox.GetType();

        // System.Collections.Generic.List`1[[System.Type, System.Private.CoreLib, Version = 9.0.0.0, Culture = neutral, PublicKeyToken = 7cec85d7bea7798e]], System.Private.CoreLib, Version = 9.0.0.0, Culture = neutral, PublicKeyToken = 7cec85d7bea7798e
        Console.WriteLine(eble.GetType().AssemblyQualifiedName);

        Console.WriteLine(t.AssemblyQualifiedName);
        Console.WriteLine(t.IsValueType);
        // System.Collections.Generic.List`1 + Enumerator[[System.Type, System.Private.CoreLib, Version = 9.0.0.0, Culture = neutral, PublicKeyToken = 7cec85d7bea7798e]], System.Private.CoreLib, Version = 9.0.0.0, Culture = neutral, PublicKeyToken = 7cec85d7bea7798e
        // True


        Console.WriteLine(tBox.AssemblyQualifiedName);
        Console.WriteLine(tBox.IsValueType);
        // System.Linq.Enumerable + ListWhereIterator`1[[System.Type, System.Private.CoreLib, Version = 9.0.0.0, Culture = neutral, PublicKeyToken = 7cec85d7bea7798e]], System.Linq, Version = 9.0.0.0, Culture = neutral, PublicKeyToken = b03f5f7f11d50a3a
        // False

        /**
List<T> возвращает самого себя при вызове IEnumerable.GetEnumerator() от non Generic интерфейса 
https://source.dot.net/System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Collections/Generic/List.cs.html#5d3accf5b217bdbf
          
При дженерик имплементации (боксинг):
IEnumerator<T> IEnumerable<T>.GetEnumerator() =>
Count == 0 ? SZGenericArrayEnumerator<T>.Empty :
GetEnumerator();

При гусиной имплементации: 
public Enumerator GetEnumerator() => new Enumerator(this);
public struct Enumerator 
         */
    }
}