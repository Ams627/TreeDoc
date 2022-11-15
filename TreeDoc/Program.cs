using System.Xml;

namespace TreeDoc;

class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var doc = TDoc.Parse(TestDoc);
            var xdoc = doc.Content;

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };
            using (XmlWriter writer = XmlWriter.Create(Console.Out, settings))
            {
                xdoc.WriteTo(writer);
            }

            Console.WriteLine("done");

        }
        catch (Exception ex)
        {
            var fullname = System.Reflection.Assembly.GetEntryAssembly().Location;
            var progname = Path.GetFileNameWithoutExtension(fullname);
            Console.Error.WriteLine($"{progname}  Error: {ex}");
        }
    }

    private const string TestDoc = """
Hello said the man
= This is level 0
This is some text in level 0
== This is level 1
This is some level 1 text
== This is also level 1
This is some level 1 text as well
""";
}

