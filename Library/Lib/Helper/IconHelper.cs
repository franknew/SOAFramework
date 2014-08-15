using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Athena.Unitop.Sure.Lib
{
    public class IconHelper
    {
        public static Icon ImageToIcon(Image image, string output)
        {
            Bitmap bitmap = new Bitmap(image);
            bitmap.SetResolution(72, 72);
            Icon icon = System.Drawing.Icon.FromHandle(bitmap.GetHicon());
            FileStream fs = File.OpenWrite(output);
            icon.Save(fs);
            fs.Flush();
            return icon;
        }
    }
}
