using System.Diagnostics;

internal static class ActivityProj
{
    public static readonly ActivitySource source = new ActivitySource(SOURCE_NAME);
    public const string SOURCE_NAME = "IziCassandra.Source";
}