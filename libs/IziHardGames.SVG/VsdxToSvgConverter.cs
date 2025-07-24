namespace Common.Diagrams;
using Aspose.Diagram;
using Aspose.Diagram.Saving;
public class VsdxToSvgConverter
{
    public byte[][] GetSvgs(Stream stream)
    {
        using var ms = new MemoryStream();
        Diagram diagram = new Diagram(stream);
        var result = new byte[diagram.Pages.Count][];
        // Export each page to separate SVG file
        for (int i = 0; i < diagram.Pages.Count; i++)
        {
            string outputFile = $"output_page_{i + 1}.svg";
            SVGSaveOptions options = new SVGSaveOptions
            {
                PageIndex = i,
                SaveFormat = SaveFileFormat.Svg,
            };
            // Save as SVG
            diagram.Save(ms, options);
            result[i] = ms.ToArray();
            ms.Position = 0;
        }
        return result;
    }
}
