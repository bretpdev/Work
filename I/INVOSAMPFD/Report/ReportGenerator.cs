using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace INVOSAMPFD
{
    class ReportGenerator
    {
        public ReportGenerator()
        {
            cachedTables = new Dictionary<ReportTable, Table>();
        }
        /// <summary>
        /// Compiles a list of reports into one word document.
        /// </summary>
        /// <param name="location">The path of the generated file.</param>
        /// <param name="reports">The reports that should be aggregated</param>
        /// <param name="beforeEachReport">This Func is executed before processing of each report.  The integer parameter represents the report number.</param>
        /// <param name="beforeSave">This action is executed before the word doc is saved to disc.</param>
        public void AggregateAndGenerate(string location, IEnumerable<Report> reports, Func<int, bool> beforeEachReport, Action beforeSave)
        {
            //Notes:
            //.InsertParagraphAfter() seems to be the only way to get new text to not overwrite old text.  There's probably a better way around this.
            //Setting font size, color, boldness, underline, all seem to retain their settings when the next piece is written.  Manually setting the correct values
            //seem to be the only way around this for now as well.
            Application app = new Application();
            Document doc = app.Documents.Add();
            int reportCount = 1;
            foreach (var report in reports)
            {
                if (!beforeEachReport(reportCount))
                {
                    app.Quit();
                    return; //user cancelled
                }
                Paragraph reportHeader = doc.Paragraphs.Add();
                reportHeader.Range.Text = string.Format("SSN {0}", report.Ssn);
                reportHeader.Range.Font.Size = 20;
                reportHeader.Range.Font.Underline = WdUnderline.wdUnderlineSingle;
                reportHeader.Range.InsertParagraphAfter();

                foreach (var section in report.Sections)
                {
                    Paragraph sectionHeader = doc.Paragraphs.Add();
                    sectionHeader.Range.Text = section.Header;
                    sectionHeader.Range.Font.Size = 14;
                    sectionHeader.Range.Font.ColorIndex = WdColorIndex.wdBlack;
                    sectionHeader.Range.Bold = 1;
                    sectionHeader.Range.Underline = WdUnderline.wdUnderlineNone;
                    sectionHeader.Range.InsertParagraphAfter();

                    foreach (var item in section.Items)
                    {
                        Paragraph itemParagraph = doc.Paragraphs.Add();
                        itemParagraph.Range.Bold = 0;
                        ProcessItem(doc, itemParagraph, item);
                        itemParagraph.Range.InsertParagraphAfter();
                    }
                }
                reportCount++;
            }
            if (reports.Any(o => o.HasErrors))
                location = location.Replace(".doc", "_has_errors.doc");
            object finalLocation = location;
            beforeSave();

            if (!Repeater.TryRepeatedly(() => doc.SaveAs2(ref finalLocation)).Successful)
            {
                Uheaa.Common.Dialog.Warning.Ok("Unable to save " + location + ".  Please ensure an existing file is not in use.");
            }
            app.Quit();
        }

        Dictionary<ReportTable, Table> cachedTables = new Dictionary<ReportTable, Table>();
        private void ProcessItem(Document doc, Paragraph itemParagraph, ReportItem item)
        {
            itemParagraph.Range.Font.ColorIndex = WdColorIndex.wdBlack;
            if (item is ReportError)
                ProcessError(item as ReportError, itemParagraph);
            else if (item is ReportText)
                itemParagraph.Range.Text = (item as ReportText).Text;
            else if (item is ReportImage)
                ProcessImage(item as ReportImage, itemParagraph, doc);
            else if (item is ReportTable)
                ProcessTable(item as ReportTable, itemParagraph, doc);
        }

        private void ProcessError(ReportError error, Paragraph itemParagraph)
        {
            itemParagraph.Range.Font.ColorIndex = WdColorIndex.wdRed;
            itemParagraph.Range.Text = error.Text;
        }

        private void ProcessImage(ReportImage image, Paragraph itemParagraph, Document doc)
        {
            object missing = Type.Missing;
            object range = itemParagraph.Range;
            string imageLocation = Path.Combine(EnterpriseFileSystem.TempFolder, Guid.NewGuid().ToString() + ".png");
            image.Image.Save(imageLocation, ImageFormat.Png); //create temp image
            doc.InlineShapes.AddPicture(imageLocation, ref missing, ref missing, ref range);
            Repeater.TryRepeatedly(() => File.Delete(imageLocation)); //delete temp image
        }

        private void ProcessTable(ReportTable reportTable, Paragraph itemParagraph, Document doc)
        {
            //re-generating duplicate tables wastes a lot of time.
            //caching their Word Table objects is much faster.
            if (cachedTables.ContainsKey(reportTable))
            {
                cachedTables[reportTable].Range.Copy();
                itemParagraph.Range.Paste();
            }
            else
            {
                var table = doc.Tables.Add(itemParagraph.Range, 1, reportTable.TableRows.First().Length);
                var headerRow = table.Rows[1];
                foreach (var tableRow in reportTable.TableRows)
                {
                    var row = table.Rows.Add();
                    for (int i = 0; i < tableRow.Length; i++)
                    {
                        var cell = row.Cells[i + 1];
                        cell.Range.Text = tableRow[i];
                        FormatCell(cell);
                    }
                }
                while (headerRow.Cells.Count > 1) //merge all cells into one title cell.  if this is done before rows are added, the rows lose their cells as well
                    headerRow.Cells[1].Merge(headerRow.Cells[2]);
                headerRow.Cells[1].Range.Text = reportTable.TableHeader;
                FormatCell(headerRow.Cells[1]);
                headerRow.Cells[1].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                //cache table to it can be copied in the future
                table.Range.Paragraphs.SpaceAfter = 0;
                cachedTables[reportTable] = table;
            }

        }

        private static void FormatCell(Cell c)
        {
            c.Range.Font.Size = 8;
            c.Range.Bold = 0;
            c.Range.Borders[WdBorderType.wdBorderLeft].LineStyle =
                c.Range.Borders[WdBorderType.wdBorderRight].LineStyle =
                c.Range.Borders[WdBorderType.wdBorderTop].LineStyle =
                c.Range.Borders[WdBorderType.wdBorderBottom].LineStyle =
            WdLineStyle.wdLineStyleSingle;
        }
    }
}
