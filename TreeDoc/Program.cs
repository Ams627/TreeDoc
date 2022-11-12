namespace TreeDoc;
class Program
{
    private static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Hello");
        }
        catch (Exception ex)
        {
            var fullname = System.Reflection.Assembly.GetEntryAssembly().Location;
            var progname = Path.GetFileNameWithoutExtension(fullname);
            Console.Error.WriteLine($"{progname}  Error: {ex}");
        }
    }
}

