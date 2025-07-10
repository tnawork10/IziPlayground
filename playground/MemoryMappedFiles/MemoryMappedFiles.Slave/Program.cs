namespace MemoryMappedFiles.Slave
{
    using System.IO.MemoryMappedFiles;
    using System.Text;

    internal class Program
    {
        static void Main(string[] args)
        {
            using var mmf = MemoryMappedFile.CreateFromFile("/dev/shm/test", FileMode.Open);
            using var accessor = mmf.CreateViewAccessor();

            int length = accessor.ReadInt32(0);
            byte[] buffer = new byte[length];
            accessor.ReadArray(4, buffer, 0, length);

            string message = Encoding.UTF8.GetString(buffer);
            Console.WriteLine("Reader read: " + message);
        }
    }
}
