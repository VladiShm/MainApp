using PluginInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transforms
{
    [Version(3, 0)]
    internal class Contrast : IPlugin
    {
        public string GetVersion()
        {
            Type type = typeof(Contrast);
            VersionAttribute version = (VersionAttribute)Attribute.GetCustomAttribute(type, typeof(VersionAttribute));
            return ("Версия: " + version.Major + "." + version.Minor);
        }
        public string Name
        {
            get
            {
                return "Контраст";
            }
        }

        public string Author
        {
            get
            {
                return "Me";
            }
        }
        public void Transform(Bitmap bitmap)
        {
            double contrast = 30;
            if (contrast < -100) contrast = -100;
            if (contrast > 100) contrast = 100;
            contrast = (100.0 + contrast) / 100.0;
            contrast *= contrast;
            Color c;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    c = bitmap.GetPixel(i, j);
                    double pR = c.R / 255.0;
                    pR -= 0.5;
                    pR *= contrast;
                    pR += 0.5;
                    pR *= 255;
                    if (pR < 0) pR = 0;
                    if (pR > 255) pR = 255;

                    double pG = c.G / 255.0;
                    pG -= 0.5;
                    pG *= contrast;
                    pG += 0.5;
                    pG *= 255;
                    if (pG < 0) pG = 0;
                    if (pG > 255) pG = 255;

                    double pB = c.B / 255.0;
                    pB -= 0.5;
                    pB *= contrast;
                    pB += 0.5;
                    pB *= 255;
                    if (pB < 0) pB = 0;
                    if (pB > 255) pB = 255;

                    bitmap.SetPixel(i, j,
        Color.FromArgb((byte)pR, (byte)pG, (byte)pB));
                }
            }
        }
    }
}
