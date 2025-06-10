#pragma warning disable
namespace VisioDiagrams
{
    using System.Xml;
    using System.Xml.Linq;
    using System.IO;
    using System.IO.Packaging;
    using System.Text;
    using NetOffice.VisioApi;
    using System.Globalization;

    public class ShapeToSvg
    {
        public static async Task Execute()
        {
            var fi = new FileInfo("inputs/ShapeToSvg.xml");
            using var s = fi.Open(FileMode.Open, FileAccess.Read);
            var xml = await XDocument.LoadAsync(s, LoadOptions.None, default);
            await Execute(xml);
        }
        public static async Task Execute(XDocument document)
        {
            var svg = ConvertVisioToSvg(document);
            var page = $"<html><body>{svg}</body></html>";
            await File.WriteAllTextAsync("C:\\Users\\adyco\\Downloads\\result.html", page);
        }
        static string ConvertVisioToSvg(XDocument doc)
        {
            XElement rootShape = doc.Root;

            // Extract root shape properties
            double rootPinX = GetCellValue(rootShape, "PinX");
            double rootPinY = GetCellValue(rootShape, "PinY");
            double rootWidth = GetCellValue(rootShape, "Width");
            double rootHeight = GetCellValue(rootShape, "Height");
            double rootLocPinX = GetCellValue(rootShape, "LocPinX");
            double rootLocPinY = GetCellValue(rootShape, "LocPinY");

            // Calculate viewBox dimensions
            double viewBoxX = rootPinX - rootLocPinX;
            double viewBoxY = rootPinY - rootLocPinY;
            string viewBox = $"{viewBoxX.ToString(CultureInfo.InvariantCulture)} " +
                            $"{viewBoxY.ToString(CultureInfo.InvariantCulture)} " +
                            $"{rootWidth.ToString(CultureInfo.InvariantCulture)} " +
                            $"{rootHeight.ToString(CultureInfo.InvariantCulture)}";

            StringBuilder svgBuilder = new StringBuilder();
            svgBuilder.AppendLine($"<svg viewBox=\"{viewBox}\" xmlns=\"http://www.w3.org/2000/svg\">");

            // Process nested shapes
            XElement shapesContainer = rootShape.Element("Shapes");
            if (shapesContainer != null)
            {
                foreach (XElement shape in shapesContainer.Elements("Shape"))
                {
                    ProcessShape(shape, svgBuilder, rootPinX, rootPinY, rootLocPinX, rootLocPinY);
                }
            }

            svgBuilder.AppendLine("</svg>");
            return svgBuilder.ToString();
        }

        static void ProcessShape(XElement shape, StringBuilder svgBuilder,
                                double parentPinX, double parentPinY,
                                double parentLocPinX, double parentLocPinY)
        {
            string shapeType = shape.Attribute("Type")?.Value;

            if (shapeType == "Group")
            {
                ProcessGroupShape(shape, svgBuilder, parentPinX, parentPinY);
            }
            else
            {
                ProcessSimpleShape(shape, svgBuilder, parentPinX, parentPinY);
            }
        }

        static void ProcessGroupShape(XElement group, StringBuilder svgBuilder,
                                     double parentPinX, double parentPinY)
        {
            double groupPinX = GetCellValue(group, "PinX");
            double groupPinY = GetCellValue(group, "PinY");
            double groupLocPinX = GetCellValue(group, "LocPinX");
            double groupLocPinY = GetCellValue(group, "LocPinY");
            double groupAngle = GetCellValue(group, "Angle");

            XElement shapesContainer = group.Element("Shapes");
            if (shapesContainer != null)
            {
                foreach (XElement shape in shapesContainer.Elements("Shape"))
                {
                    ProcessSimpleShape(shape, svgBuilder, groupPinX, groupPinY);
                }
            }
        }

        static void ProcessSimpleShape(XElement shape, StringBuilder svgBuilder,
                                      double parentPinX, double parentPinY)
        {
            double pinX = GetCellValue(shape, "PinX");
            double pinY = GetCellValue(shape, "PinY");
            double locPinX = GetCellValue(shape, "LocPinX");
            double locPinY = GetCellValue(shape, "LocPinY");
            double angle = GetCellValue(shape, "Angle");
            double width = GetCellValue(shape, "Width");
            double height = GetCellValue(shape, "Height");
            double angleDeg = angle * (180 / Math.PI);

            // Calculate absolute position
            double absX = parentPinX + pinX;
            double absY = parentPinY + pinY;

            // Create transformation string
            string transform =
                $"translate({absX.ToString(CultureInfo.InvariantCulture)}," +
                $"{absY.ToString(CultureInfo.InvariantCulture)}) " +
                $"rotate({angleDeg.ToString(CultureInfo.InvariantCulture)}) " +
                $"translate({(-locPinX).ToString(CultureInfo.InvariantCulture)}," +
                $"{(-locPinY).ToString(CultureInfo.InvariantCulture)})";

            svgBuilder.AppendLine($"<g transform=\"{transform}\">");

            // Process geometry sections
            foreach (XElement geom in shape.Elements("Section"))
            {
                if (geom.Attribute("N")?.Value == "Geometry")
                {
                    ProcessGeometry(geom, shape, svgBuilder);
                }
            }

            // Process text elements
            XElement textElement = shape.Element("Text");
            if (textElement != null)
            {
                ProcessText(textElement, shape, width, height, svgBuilder);
            }

            svgBuilder.AppendLine("</g>");
        }

        static void ProcessGeometry(XElement geom, XElement shape, StringBuilder svgBuilder)
        {
            List<string> pathCommands = new List<string>();
            bool noFill = GetGeometryCellValueAsString(geom, "NoFill") == "1";
            bool noLine = GetGeometryCellValueAsString(geom, "NoLine") == "1";

            // Get styling properties
            string stroke = noLine ? "none" : "black";
            string strokeWidth = "0.02";
            string fill = noFill ? "none" : "black";

            XElement lineWeight = shape.Element("Cell");
            if (lineWeight != null && lineWeight.Attribute("N")?.Value == "LineWeight")
            {
                strokeWidth = lineWeight.Attribute("V")?.Value;
            }

            // Process geometry rows
            foreach (XElement row in geom.Elements("Row"))
            {
                string rowType = row.Attribute("T")?.Value;
                switch (rowType)
                {
                    case "MoveTo":
                        double x1 = GetGeometryCellValue(row, "X");
                        double y1 = GetGeometryCellValue(row, "Y");
                        pathCommands.Add($"M {x1.ToString(CultureInfo.InvariantCulture)} " +
                                         $"{y1.ToString(CultureInfo.InvariantCulture)}");
                        break;
                    case "LineTo":
                        double x2 = GetGeometryCellValue(row, "X");
                        double y2 = GetGeometryCellValue(row, "Y");
                        pathCommands.Add($"L {x2.ToString(CultureInfo.InvariantCulture)} " +
                                         $"{y2.ToString(CultureInfo.InvariantCulture)}");
                        break;
                    case "RelMoveTo":
                        double rx = GetGeometryCellValue(row, "X");
                        double ry = GetGeometryCellValue(row, "Y");
                        pathCommands.Add($"m {rx.ToString(CultureInfo.InvariantCulture)} " +
                                         $"{ry.ToString(CultureInfo.InvariantCulture)}");
                        break;
                    case "RelLineTo":
                        double rlx = GetGeometryCellValue(row, "X");
                        double rly = GetGeometryCellValue(row, "Y");
                        pathCommands.Add($"l {rlx.ToString(CultureInfo.InvariantCulture)} " +
                                         $"{rly.ToString(CultureInfo.InvariantCulture)}");
                        break;
                    case "RelCubBezTo":
                        double cx = GetGeometryCellValue(row, "X");
                        double cy = GetGeometryCellValue(row, "Y");
                        double ca = GetGeometryCellValue(row, "A");
                        double cb = GetGeometryCellValue(row, "B");
                        double cc = GetGeometryCellValue(row, "C");
                        double cd = GetGeometryCellValue(row, "D");
                        pathCommands.Add(
                            $"c {ca.ToString(CultureInfo.InvariantCulture)}," +
                            $"{cb.ToString(CultureInfo.InvariantCulture)} " +
                            $"{cc.ToString(CultureInfo.InvariantCulture)}," +
                            $"{cd.ToString(CultureInfo.InvariantCulture)} " +
                            $"{cx.ToString(CultureInfo.InvariantCulture)}," +
                            $"{cy.ToString(CultureInfo.InvariantCulture)}");
                        break;
                }
            }

            if (pathCommands.Count > 0)
            {
                string pathData = string.Join(" ", pathCommands);
                svgBuilder.AppendLine(
                    $"<path d=\"{pathData}\" fill=\"{fill}\" " +
                    $"stroke=\"{stroke}\" stroke-width=\"{strokeWidth}\"/>");
            }
        }

        static void ProcessText(XElement textElement, XElement shape,
                                double width, double height,
                                StringBuilder svgBuilder)
        {
            string textContent = textElement.Value.Trim();
            if (!string.IsNullOrEmpty(textContent))
            {
                // Get font properties
                string fontFamily = "Calibri";
                double fontSize = 0.1667;

                XElement charSection = shape.Element("Section");
                if (charSection != null && charSection.Attribute("N")?.Value == "Character")
                {
                    XElement row = charSection.Element("Row");
                    if (row != null)
                    {
                        XElement fontCell = row.Elements("Cell")
                            .FirstOrDefault(c => c.Attribute("N")?.Value == "Font");
                        if (fontCell != null)
                        {
                            fontFamily = fontCell.Attribute("V")?.Value;
                        }

                        XElement sizeCell = row.Elements("Cell")
                            .FirstOrDefault(c => c.Attribute("N")?.Value == "Size");
                        if (sizeCell != null)
                        {
                            fontSize = double.Parse(sizeCell.Attribute("V")?.Value,
                                CultureInfo.InvariantCulture);
                        }
                    }
                }

                svgBuilder.AppendLine(
                    $"<text x=\"{(width / 2).ToString(CultureInfo.InvariantCulture)}\" " +
                    $"y=\"{(height / 2).ToString(CultureInfo.InvariantCulture)}\" " +
                    $"font-family=\"{fontFamily}\" " +
                    $"font-size=\"{fontSize.ToString(CultureInfo.InvariantCulture)}\" " +
                    $"text-anchor=\"middle\" dominant-baseline=\"middle\">" +
                    $"{textContent}</text>");
            }
        }

        // Helper methods to extract values
        static double GetCellValue(XElement shape, string cellName)
        {
            XElement cell = shape.Elements("Cell")
                .FirstOrDefault(c => c.Attribute("N")?.Value == cellName);
            return cell != null ?
                double.Parse(cell.Attribute("V")?.Value, CultureInfo.InvariantCulture) : 0;
        }

        static double GetGeometryCellValue(XElement row, string cellName)
        {
            XElement cell = row.Elements("Cell")
                .FirstOrDefault(c => c.Attribute("N")?.Value == cellName);
            return cell != null ?
                double.Parse(cell.Attribute("V")?.Value, CultureInfo.InvariantCulture) : 0;
        }

        static string GetGeometryCellValueAsString(XElement geom, string cellName)
        {
            XElement cell = geom.Elements("Cell")
                .FirstOrDefault(c => c.Attribute("N")?.Value == cellName);
            return cell?.Attribute("V")?.Value ?? "0";
        }
    }
}
