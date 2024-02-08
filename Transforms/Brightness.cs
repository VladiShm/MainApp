using PluginInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transforms
{
    [VersionAttribute(4, 0)]
    internal class Brightness : IPlugin
    {
        public string Name
        {
            get
            {
                return "Яркость";
            }
        }

        public string Author
        {
            get
            {
                return "Me";
            }
        }

        public string GetVersion()
        {
            Type type = typeof(MedianFiltr);
            VersionAttribute version = (VersionAttribute)Attribute.GetCustomAttribute(type, typeof(VersionAttribute));
            return ("Версия: " + version.Major + "." + version.Minor);
        }
        public void Transform(Bitmap bitmap)
        {
            int brightness = 100;
            if (brightness < -255) brightness = -255;
            if (brightness > 255) brightness = 255;
            Color c;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    c = bitmap.GetPixel(i, j);
                    int cR = c.R + brightness;
                    int cG = c.G + brightness;
                    int cB = c.B + brightness;

                    if (cR < 0) cR = 1;
                    if (cR > 255) cR = 255;

                    if (cG < 0) cG = 1;
                    if (cG > 255) cG = 255;

                    if (cB < 0) cB = 1;
                    if (cB > 255) cB = 255;

                    bitmap.SetPixel(i, j,
        Color.FromArgb((byte)cR, (byte)cG, (byte)cB));
                }
            }
        }
    }
}
