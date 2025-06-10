#pragma warning disable
namespace VisioDiagrams
{
    using System.Xml;
    using System.Xml.Linq;
    using System.IO;
    using System.IO.Packaging;
    using System.Text;
    using NetOffice.VisioApi;

    public class VisioDiagram
    {
        private const string path = "inputs/Visio.vsdx";

        public static async Task WithNetOffice()
        {
            var visioApp = new Application { Visible = false };
            var document = visioApp.Documents.Open(path);

            foreach (Page page in document.Pages)
            {
                Console.WriteLine($"Page: {page.Name}");

                foreach (Shape shape in page.Shapes)
                {
                    ExportShapeInfo(shape);
                }
            }
        }

        public static async Task Explore()
        {
            //var raw = await File.ReadAllTextAsync(path);
            //var str = Convert.FromBase64String(raw);
            var pack = OpenPackage(path, null);

            if (pack != null)
            {
                var parts = pack.GetParts();
                foreach (var part in parts)
                {
                    // https://learn.microsoft.com/en-us/office/client-developer/visio/how-to-manipulate-the-visio-file-format-programmatically
                    Console.WriteLine("Package part URI: {0}", part.Uri);
                    Console.WriteLine("Content type: {0}", part.ContentType.ToString());
                    if (part.ContentType.Contains("+xml"))
                    {
                        using var s = part.GetStream();
                        var fn = part.Uri.ToString().Split('/').Last();
                        var fi = new FileInfo(fn);
                        using var fs = fi.Open(FileMode.OpenOrCreate);
                        await s.CopyToAsync(fs);
                        Console.WriteLine($"File created: {fi.FullName}");
                    }
                    if (part.ContentType.StartsWith("image/"))
                    {
                        using var s = part.GetStream();
                        var fn = part.Uri.ToString().Split('/').Last();
                        var fi = new FileInfo(fn);
                        using var fs = fi.Open(FileMode.OpenOrCreate);
                        await s.CopyToAsync(fs);
                        Console.WriteLine($"File created: {fi.FullName}");
                    }
                    else if (part.ContentType == "application/vnd.ms-visio.master+xml")
                    {
                        await HandleMasterPageAsync(part);
                    }
                }

                var visioRoot = pack.GetPart(new Uri("/visio/document.xml", UriKind.Relative));
                var pagePart = pack.GetPart(new Uri("/visio/pages/page1.xml", UriKind.Relative));
                using var stream = pagePart.GetStream();
                var xml = XDocument.Load(stream);
                var children = xml.Elements().ToArray();
                var ns = (XNamespace)"http://schemas.microsoft.com/office/visio/2012/main";
                var shapes = xml.Descendants(ns + "Shape").ToArray();

                foreach (var shape in shapes)
                {
                    var pinX = shape.Descendants().First(x => x.Attribute("N")?.Value == "PinX");
                    var pinXVal = pinX.Attribute("V")?.Value;
                    var pinY = shape.Descendants().First(x => x.Attribute("N")?.Value == "PinY");
                    var pinYVal = pinY.Attribute("V")?.Value;
                    var width = shape.Descendants().FirstOrDefault(x => x.Attribute("N")?.Value == "Width");
                    var widthVal = width?.Attribute("V")?.Value;
                    var height = shape.Descendants().FirstOrDefault(x => x.Attribute("N")?.Value == "Width");
                    var heightVal = height?.Attribute("V")?.Value;
                    var id = shape.Attribute("ID")?.Value;
                    //$.Section(N==Character).Row.Cell(V->value) - цвет обводки
                    //$.Cell(N==FillBkgnd)(c->V) - цвет заливки (немного отличается от фактического в Visio)
                    Console.WriteLine($"Shape ID {pinXVal} at (X: {pinXVal}, Y: {pinYVal}), Size: {widthVal} x {heightVal}");
                }
                if (pack is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                var svg = ConvertVisioPageToSvg(path, 1);
            }
        }

        private static async Task HandleMasterPageAsync(PackagePart part)
        {
            using var stream = part.GetStream();
            var xml = XDocument.Load(stream);
            //xml.Descendants(part.)
        }

        static readonly XNamespace V = "http://schemas.microsoft.com/office/visio/2012/main";

        static string ConvertVisioPageToSvg(string vsdxPath, int pageIndex)
        {
            // Open the package
            using var package = Package.Open(vsdxPath, FileMode.Open, FileAccess.Read);
            // Construct the part URI for the requested page
            var pageUri = new Uri($"/visio/pages/page{pageIndex}.xml", UriKind.Relative);
            var pagePart = package.GetPart(pageUri);

            // Load the page XML
            XDocument doc;
            using (var stream = pagePart.GetStream())
                doc = XDocument.Load(stream);

            var sb = new StringBuilder();
            sb.AppendLine(@"<svg xmlns=""http://www.w3.org/2000/svg"" version=""1.1"">");

            // Iterate each Shape
            foreach (var shape in doc.Root
                                     .Elements(V + "Shapes")
                                     .Elements(V + "Shape"))
            {
                // Read translation from XForm
                var xform = shape.Element(V + "XForm");
                double tx = xform != null
                            ? double.Parse(xform.Element(V + "PinX")?.Value ?? "0")
                            : 0;
                double ty = xform != null
                            ? double.Parse(xform.Element(V + "PinY")?.Value ?? "0")
                            : 0;

                // For each Geometry stream (a path)
                foreach (var geom in shape.Elements(V + "Geometry"))
                {
                    // Build path commands
                    var d = new StringBuilder();
                    foreach (var cmd in geom.Elements())
                    {
                        string tag = cmd.Name.LocalName;
                        // Read all cells in this command into a dictionary
                        var cells = cmd.Elements(V + "Cell").ToDictionary(c => (string)c.Attribute("N"), c => (string)c.Attribute("V"));

                        switch (tag)
                        {
                            case "MoveTo":
                                d.AppendFormat("M {0} {1} ", cells["X"], cells["Y"]);
                                break;
                            case "LineTo":
                                d.AppendFormat("L {0} {1} ", cells["X"], cells["Y"]);
                                break;
                            case "ArcTo":
                                // ArcTo: use A rx ry 0 0 1 x y
                                // Visio uses a single radius, assume rx=ry
                                d.AppendFormat("A {0} {0} 0 0 1 {1} {2} ", cells["R"], cells["X"], cells["Y"]);
                                break;
                                // Add more cases (Ellipse, Spline, etc.) as needed
                        }
                    }

                    // Close path if necessary
                    if (!d.ToString().TrimEnd().EndsWith("Z", StringComparison.OrdinalIgnoreCase))
                        d.Append("Z");

                    // Write a <path> with translation applied
                    sb.AppendLine($@"  <path d=""{d}"" transform=""translate({tx},{ty})"" fill=""none"" stroke=""black""/>");
                }
            }

            sb.AppendLine("</svg>");
            return sb.ToString();
        }


        // https://learn.microsoft.com/en-us/office/client-developer/visio/how-to-manipulate-the-visio-file-format-programmatically#to-open-a-vsdx-file-as-a-package
        private static Package? OpenPackage(string path, Environment.SpecialFolder? folder = null)
        {
            Package? visioPackage = null;
            // Get a reference to the location 
            // where the Visio file is stored.
            if (folder.HasValue)
            {
                string directoryPath = System.Environment.GetFolderPath(folder.Value);
                DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
                // Get the Visio file from the location.
                FileInfo[] fileInfos = dirInfo.GetFiles(path);

                if (fileInfos.Count() > 0)
                {
                    FileInfo fileInfo = fileInfos[0];
                    string filePathName = fileInfo.FullName;
                    // Open the Visio file as a package with
                    // read/write file access.
                    visioPackage = Package.Open(
                        filePathName,
                        FileMode.Open,
                        FileAccess.ReadWrite);
                }
                // Return the Visio file as a package.
            }
            else
            {
                var fi = new FileInfo(path);
                visioPackage = Package.Open(fi.FullName, FileMode.Open, FileAccess.Read);
            }
            return visioPackage;
        }

        static void ExportShapeInfo(Shape shape)
        {
            Console.WriteLine($"Shape Name: {shape.Name}");
            Console.WriteLine($"Shape Text: {shape.Text}");

            // Example: Export shape text to a file, database, or UI
            // You can also access properties like shape.CellsU["Width"].ResultIU etc.
        }
    }
}
