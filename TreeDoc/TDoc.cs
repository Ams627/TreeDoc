using System.Text;
using System.Xml.Linq;

namespace TreeDoc;

class TDoc
{
    private XDocument _content;
    private TDoc(XDocument content)
    {
        _content = content;
    }

    public static TDoc Parse(string content)
    {
        using var reader = new StringReader(content);

        string? line;
        int lineNumber = 1;
        int currentLevel = 0;
        StringBuilder currentContent = new();
        var xdoc = new XDocument();
        xdoc.Add(new XElement("Document"));
        var currentElement = xdoc.Root;
        
        while ((line = reader.ReadLine()) != null)
        {
            var nextLevel = 0;
            int i = 0;
            while (i < line.Length)
            {
                if (line[i++] == '=')
                {
                    nextLevel++;
                }
            }

            if (nextLevel == 0)
            {
                currentContent.AppendLine(line);
            }
            else if (nextLevel < currentLevel)
            {

            }
            else if (nextLevel == currentLevel)
            {
                currentElement.Add(currentContent.ToString().Trim());
                var elementName = $"L{currentLevel - 1}";
                var element = new XElement(elementName);
                currentElement?.AddAfterSelf(element);
                currentElement = element;
                currentContent.Clear();
            }
            else if (nextLevel == currentLevel + 1)
            {
                var elementName = $"L{currentLevel}";
                var element = new XElement(elementName);
                currentElement.Add(element);
                currentElement.Add(currentContent.ToString().Trim());
                currentElement = element;
                currentContent.Clear();
                currentLevel = nextLevel;
            }
            else if (nextLevel > currentLevel)
            {
                throw new Exception($"bad level at line {lineNumber}");
            }
            lineNumber++;
        }
        if (currentContent.Length > 0)
        {
            currentElement?.Add(currentContent.ToString().Trim());
        }

        var doc = new TDoc(xdoc);
        return doc;
    }

    public XDocument Content => _content;

}

