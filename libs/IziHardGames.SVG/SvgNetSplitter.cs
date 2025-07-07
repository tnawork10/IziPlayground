using System.Drawing;
using System.Drawing.Drawing2D;
using ExCSS;
using Svg;
using Svg.Transforms;
using Color = System.Drawing.Color;
namespace IziHardGames.SVG
{
    public class SvgNetSplitter
    {
        public static async Task RunTest()
        {
            //var document = SvgDocument.Open(@"C:\Users\adyco\Downloads\test.svg");
            var document = SvgDocument.Open(@"C:\Users\adyco\Downloads\2custom.svg");
            var descs = document.Descendants().ToArray();
            var groups = descs.OfType<SvgGroup>();
            var shapes = groups.Where(x => x.CustomAttributes.TryGetValue("http://schemas.microsoft.com/visio/2003/SVGExtensions/:groupContext", out var val) && val == "shape").ToArray();
            var grps = groups.Where(x => x.CustomAttributes.TryGetValue("http://schemas.microsoft.com/visio/2003/SVGExtensions/:groupContext", out var val) && val == "group").ToArray();
            var el = document.GetElementById("shape2-2");
            //var el511 = document.GetElementById("shape5-11");
            var el511 = document.GetElementById("group1-1");

            var cloned = (SvgGroup)el511.DeepCopy();
            var translate = cloned.Transforms.FirstOrDefault(x => x is SvgTranslate);
            var box = new SvgViewBox(0, 0, 500, 500);

            if (translate != null)
            {
                box.MinX = (translate as SvgTranslate).X;
                box.MinY = (translate as SvgTranslate).Y;
                //(translate as SvgTranslate).X = 250;
                //(translate as SvgTranslate).Y = 250;
                //cloned.Transforms.Remove(translate);
                //cloned.Transforms.Clear();
            }
            //cloned.
            //cloned.Transforms = null;
            //var bounds = cloned.();
            var newDoc = new SvgDocument
            {
                Width = document.Width,
                Height = document.Height,
                //ViewBox = document.ViewBox,
                //Width = 500,
                //Height = 500,
                ViewBox = document.ViewBox,
            };

            var outputPath = $"C:\\Users\\adyco\\Downloads\\{DateTime.Now.Ticks}.svg";
            newDoc.Children.Add(cloned);
            //newDoc.Children.Add(new SvgRectangle()
            //{
            //    X = 0,
            //    Y = 0,
            //    Width = 50,
            //    Height = 50,
            //    Fill = new SvgColourServer(Color.Yellow)
            //});
            newDoc.Write(outputPath);

            foreach (var desc in descs)
            {
                //desc.
            }
        }
    }

    //public static class SvgExtensions
    //{
    //    public static Matrix GetGlobalTransform(this SvgVisualElement element)
    //    {
    //        var transform = new Matrix();

    //        // Traverse up through parents and combine their transforms
    //        SvgElement? current = element;
    //        while (current != null)
    //        {
    //            if (current is SvgVisualElement visual && visual.Transforms != null)
    //            {
    //                foreach (var t in visual.Transforms.AsEnumerable().Reverse())
    //                {
    //                    transform.Multiply(t.Matrix, MatrixOrder.Prepend);
    //                }
    //            }
    //            current = current.Parent;
    //        }

    //        return transform;
    //    }

    //    public static PointF TransformPoint(this SvgVisualElement element, PointF localPoint)
    //    {
    //        var globalMatrix = element.GetGlobalTransform();
    //        PointF[] points = { localPoint };
    //        globalMatrix.TransformPoints(points);
    //        return points[0];
    //    }
    //}
}
