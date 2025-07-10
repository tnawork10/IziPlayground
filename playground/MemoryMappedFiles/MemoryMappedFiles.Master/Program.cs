namespace MemoryMappedFiles.Master
{
    using System.IO.MemoryMappedFiles;
    using System.Text;

    internal class Program
    {
        static void Main(string[] args)
        {
            using var mmf = MemoryMappedFile.CreateFromFile("/dev/shm/test", System.IO.FileMode.OpenOrCreate, "test", 100_000);
            using var accessor = mmf.CreateViewAccessor();

            string message = "Hello from Writer!";
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            accessor.Write(0, buffer.Length);
            accessor.WriteArray(4, buffer, 0, buffer.Length);

            Console.WriteLine("Writer wrote: " + message);
            Console.ReadLine(); // Keep open
        }
    }
}
