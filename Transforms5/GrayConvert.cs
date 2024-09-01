using PluginInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Transforms5
{
    [Version(1, 0)]
    internal class GrayConvert : IPlugin
    {
        public string GetVersion()
        {
            Type type = typeof(GrayConvert);
            VersionAttribute version = (VersionAttribute)Attribute.GetCustomAttribute(type, typeof(VersionAttribute));
            return ("Версия: " + version.Major + "." + version.Minor);
        }
        public string Name
        {
            get
            {
                return "Оттенки серого";
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
            Color c;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    c = bitmap.GetPixel(i, j);
                    byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);

                    bitmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
        }
    }
}
