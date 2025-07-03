using SkiaSharp;
using Svg;
using Svg.Skia;
using System.Xml;
using F = System.IO.File;

namespace IziHardGames.SVG
{
    public class SvgSplitter
    {
        public static async Task ExecuteTest()
        {
            var xml = await F.ReadAllTextAsync(@"C:\Users\adyco\Downloads\test.svg");
            var result = SvgSplitter.Split(xml);
        }

        public static ICollection<XmlDocument> Split(string text)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(text);

            XmlElement svgRoot = xmlDoc.DocumentElement!;
            if (svgRoot.Name != "svg")
            {
                Console.WriteLine("Invalid SVG file: root is not <svg>");
                return Array.Empty<XmlDocument>();
            }


            var result = new List<XmlDocument>();
            foreach (XmlNode child in svgRoot.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element)
                    continue;

                if (child.Name != "g")
                    continue;

                var tempDoc = new XmlDocument();
                // Create <svg> root element for new file
                var newSvgRoot = tempDoc.CreateElement("svg");
                foreach (XmlAttribute attr in svgRoot.Attributes)
                {
                    newSvgRoot.SetAttribute(attr.Name, attr.Value);
                }
                var imported = tempDoc.ImportNode(child, true);
                newSvgRoot.AppendChild(imported);
                tempDoc.AppendChild(newSvgRoot);
                result.Add(tempDoc);
            }
            return result;
        }

        public static async Task WriteGNodesToFile(XmlDocument xmlDoc)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("svg", "http://www.w3.org/2000/svg");
            var gElements = xmlDoc.SelectNodes("//svg:g", ns);

            XmlElement svgRoot = xmlDoc.DocumentElement!;
            if (svgRoot.Name != "svg")
            {
                Console.WriteLine("Invalid SVG file: root is not <svg>");
                return;
            }

            for (int i = 0; i < gElements.Count; i++)
            {
                var node = gElements[i];
                if (node != null)
                {
                    var xml = node.OuterXml;
                    var tempDoc = new XmlDocument();
                    var newSvgRoot = tempDoc.CreateElement("svg");
                    foreach (XmlAttribute attr in svgRoot.Attributes)
                    {
                        newSvgRoot.SetAttribute(attr.Name, attr.Value);
                    }
                    var imported = tempDoc.ImportNode(node, true);
                    newSvgRoot.AppendChild(imported);
                    tempDoc.AppendChild(newSvgRoot);
                    tempDoc.Save($"g-{i}.svg");
                }
            }
        }
    }
}
