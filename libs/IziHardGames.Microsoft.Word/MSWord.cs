namespace IziHardGames.Microsoft.Word
{
    using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;

    public class MSWord
    {
        public async Task CreateWord()
        {
            var path = "output.docx";
            //using var outStream = File.OpenWrite();
            using (WordprocessingDocument document = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = new Body();
                var ps = new PageSize
                {
                    Width = 16838,  // landscape width
                    Height = 11906, // landscape height
                    Orient = PageOrientationValues.Landscape
                };
                var pm = new PageMargin
                {
                    Top = 1440,
                    Right = 1440,
                    Bottom = 1440,
                    Left = 1440,
                    Header = 720,
                    Footer = 720,
                    Gutter = 0
                };
                var sectionProps = new SectionProperties(ps, pm);
                body.Append(sectionProps);


                Paragraph h1Paragraph = new Paragraph(new ParagraphProperties(new ParagraphStyleId() { Val = "Heading1" }),
                                                      new Run(new Text("Заголовок 123")));
                body.AppendChild(h1Paragraph);
                body.AppendChild(new Paragraph(new Run(new Text("Hello from landscape document!"))));


                var table = new Table();
                TableProperties tblProps = new TableProperties(
        new TableBorders(new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                         new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                         new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                         new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                         new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                         new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }));
                table.AppendChild(tblProps);

                for (int r = 0; r < 3; r++)
                {
                    TableRow tr = new TableRow();
                    for (int c = 0; c < 3; c++)
                    {
                        TableCell tc = new TableCell(
                            new Paragraph(new Run(new Text($"R{r + 1}C{c + 1}")))
                        );

                        // Optional: Set uniform cell width
                        tc.Append(new TableCellProperties(
                            new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "2400" }
                        ));

                        tr.Append(tc);
                    }
                    table.Append(tr);
                }

                body.Append(table);
                mainPart.Document.Append(body);
            }
        }
    }
}
