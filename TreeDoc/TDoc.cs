using System;
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
            if (line.Contains("Back"))
            {
                global::System.Console.WriteLine();
            }
            var nextLevel = 0;
            int i = 0;
            string title = "";
            while (i < line.Length)
            {
                if (line[i++] == '=')
                {
                    nextLevel++;
                }
                else
                {
                    title = line[i..].Trim();
                    break;
                }
            }

            if (nextLevel == 0)
            {
                currentContent.AppendLine(line);
            }
            else if (nextLevel < currentLevel)
            {
                currentElement?.Add(GetContent(currentContent));
                var element = GetElement(nextLevel - 1, title);
                var appropriateAncestorr = GetAncestor(currentElement, currentLevel - nextLevel + 1);
                appropriateAncestorr?.Add(element);
            }
            else if (nextLevel == currentLevel)
            {
                currentElement?.Add(GetContent(currentContent));
                var element = GetElement(nextLevel - 1, title);
                currentElement?.AddAfterSelf(element);
                currentElement = element;
            }
            else if (nextLevel == currentLevel + 1)
            {
                currentElement?.Add(GetContent(currentContent));
                var element = GetElement(currentLevel, title);
                currentElement?.Add(element);
                currentElement = element;
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

    private static XElement GetContent(StringBuilder currentContent)
    {
        var xelement =  new XElement("C", currentContent.ToString().Trim());
        currentContent.Clear();
        return xelement;
    }

    private static XElement? GetAncestor(XElement? currentElement, int levelsBack)
    {
        while (levelsBack-- > 0)
        {
            currentElement = currentElement?.Parent;
        }
        return currentElement;
    }

    private static XElement GetElement(int currentLevel, string title)
    {
        var elementName = $"L{currentLevel}";
        var titleAttribute = string.IsNullOrEmpty(title) ? null : new XAttribute("title", title);
        var element = new XElement(elementName, titleAttribute);
        return element;

    }

    public XDocument Content => _content;

}

