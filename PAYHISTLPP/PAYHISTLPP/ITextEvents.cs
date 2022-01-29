using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using IText = iTextSharp.text;

namespace PAYHISTLPP
{
    public class ITextEvents : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte CB { get; set; }

        // we will put the final number of pages in a template
        PdfTemplate HeaderTemplate { get; set; }
        PdfTemplate FooterTemplate { get; set; }

        // this is the BaseFont we are going to use for the header / footer
        BaseFont BF { get; set; } = null;
        DateTime PrintTime { get; set; } = DateTime.Now;

        private Image UheaaImaging { get; set; } = Image.GetInstance(Properties.Resources.UheaaLogo, null, true);

        public ITextEvents() : base()
        {
            UheaaImaging.ScaleToFit(100f, 50f);
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                BF = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                CB = writer.DirectContent;
                HeaderTemplate = CB.CreateTemplate(100, 100);
                FooterTemplate = CB.CreateTemplate(50, 50);
            }
            catch (Exception)
            {
            }
        }

        public override void OnEndPage(IText.pdf.PdfWriter writer, IText.Document document)
        {
            base.OnEndPage(writer, document);

            //Create PdfTable object
            PdfPTable pdfTab = new PdfPTable(3);

            //We will have to create separate cells to include image logo and 2 separate strings
            //Row 1
            PdfPCell pdfCell1 = new PdfPCell(UheaaImaging);
            PdfPCell pdfCell2 = new PdfPCell();
            PdfPCell pdfCell3 = new PdfPCell();
            string text = "Page " + writer.PageNumber + " of ";

            //Add paging to header
            {
                CB.BeginText();
                CB.SetFontAndSize(BF, 12);
                CB.SetTextMatrix(document.PageSize.GetRight(200), document.PageSize.GetTop(45));
                CB.ShowText(text);
                CB.EndText();
                float len = BF.GetWidthPoint(text, 12);
                //Adds "12" in Page 1 of 12
                CB.AddTemplate(HeaderTemplate, document.PageSize.GetRight(200) + len, document.PageSize.GetTop(45));
            }
            //Add paging to footer
            {
                CB.BeginText();
                CB.SetFontAndSize(BF, 12);
                CB.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
                CB.ShowText(text);
                CB.EndText();
                float len = BF.GetWidthPoint(text, 12);
                CB.AddTemplate(FooterTemplate, document.PageSize.GetRight(180) + len, document.PageSize.GetBottom(30));
            }

            //Row 2
            PdfPCell pdfCell4 = new PdfPCell();

            //Row 3 
            PdfPCell pdfCell5 = new PdfPCell();
            PdfPCell pdfCell6 = new PdfPCell();
            PdfPCell pdfCell7 = new PdfPCell();

            //set the alignment of all three cells and set border to 0
            pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell3.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell4.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell5.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell7.HorizontalAlignment = Element.ALIGN_CENTER;

            pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell4.VerticalAlignment = Element.ALIGN_TOP;
            pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;

            pdfCell4.Colspan = 3;

            pdfCell1.Border = 0;
            pdfCell2.Border = 0;
            pdfCell3.Border = 0;
            pdfCell4.Border = 0;
            pdfCell5.Border = 0;
            pdfCell6.Border = 0;
            pdfCell7.Border = 0;

            //add all three cells into PdfTable
            pdfTab.AddCell(pdfCell1);
            pdfTab.AddCell(pdfCell2);
            pdfTab.AddCell(pdfCell3);
            pdfTab.AddCell(pdfCell4);
            pdfTab.AddCell(pdfCell5);
            pdfTab.AddCell(pdfCell6);
            pdfTab.AddCell(pdfCell7);

            pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.WidthPercentage = 70;
            //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;    

            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            //set pdfContent value

            //Move the pointer and draw line to separate header section from rest of page
            CB.MoveTo(40, document.PageSize.Height - 100);
            CB.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
            CB.Stroke();

            //Move the pointer and draw line to separate footer section from rest of page
            CB.MoveTo(40, document.PageSize.GetBottom(50));
            CB.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            CB.Stroke();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            HeaderTemplate.BeginText();
            HeaderTemplate.SetFontAndSize(BF, 12);
            HeaderTemplate.SetTextMatrix(0, 0);
            HeaderTemplate.ShowText((writer.PageNumber - 1).ToString());
            HeaderTemplate.EndText();

            FooterTemplate.BeginText();
            FooterTemplate.SetFontAndSize(BF, 12);
            FooterTemplate.SetTextMatrix(0, 0);
            FooterTemplate.ShowText((writer.PageNumber - 1).ToString());
            FooterTemplate.EndText();
        }
    }
}