using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using IziHardGames.IziAsyncEnumerables;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;


await TestAsync();


partial class Program
{
    public static async Task TestAsync()
    {
        var props = typeof(WithProp).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var prop in props)
        {
            Console.WriteLine(prop.PropertyType.AssemblyQualifiedName);
            Console.WriteLine($"\t {Nullable.GetUnderlyingType(prop.PropertyType) != null}");
        }
    }
}

public class WithProp : BaseClass
{
    public int PropInt { get; set; }
    private int PropIntAAA { get; set; }
    public int? PropIntBBB { get; set; }
    public WithProp Value { get; set; }
    public WithProp? ValueNullable { get; set; }
}

public class BaseClass
{
    private int BaseProperty0 { get; set; }
    protected int BaseProperty1 { get; set; }
    public int BaseProperty2 { get; set; }
}