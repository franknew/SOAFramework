using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.Barcode
{
    public class BarcodeHelper
    {
        public static Image ToImageWithCodeString(string barcode, BarcodeSymbology barcodeSymbology = BarcodeSymbology.Code128)
        {
            var generator = BarcodeDrawFactory.GetSymbology(barcodeSymbology);
            var metrics = generator.GetDefaultMetrics(60);
            metrics.Scale = 5;
            Image imgBarcode = generator.Draw(barcode, metrics);
            Bitmap image = new Bitmap(imgBarcode.Width, imgBarcode.Height + 20);
            Graphics g = Graphics.FromImage(image);
            Rectangle textBound = new Rectangle(new Point(0, imgBarcode.Height + 1), new Size(imgBarcode.Width, 20));
            Rectangle bound = new Rectangle(new Point(0, 0), new Size(image.Width, image.Height));
            Rectangle imageBound = new Rectangle(new Point(image.Width / 6, 10), new Size(image.Width / 3 * 2, imgBarcode.Height - 10));
            Font font = new Font(new FontFamily("宋体"), 15);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            g.FillRectangle(new SolidBrush(Color.White), bound);
            g.DrawImage(imgBarcode, imageBound);
            g.DrawString(barcode, font, new SolidBrush(Color.Black), textBound, format);

            return image;
        }
    }
}
