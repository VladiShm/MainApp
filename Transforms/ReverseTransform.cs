using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginInterface;
using System.Drawing;

namespace Transforms
{
    [Version(1, 0)]
    public class ReverseTransform: IPlugin
    {
        public string GetVersion()
        {
            Type type = typeof(MedianFiltr);
            VersionAttribute version = (VersionAttribute)Attribute.GetCustomAttribute(type, typeof(VersionAttribute));
            return ("Версия: " + version.Major + "." + version.Minor);
        }
        public string Name
        {
            get
            {
                return "Переворот изображения";
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
            for (int i = 0; i < bitmap.Width; ++i)
                for (int j = 0; j < bitmap.Height / 2; ++j)
                {
                    Color color = bitmap.GetPixel(i, j);
                    bitmap.SetPixel(i, j, bitmap.GetPixel(i, bitmap.Height - j - 1));
                    bitmap.SetPixel(i, bitmap.Height - j - 1, color);
                }
        }
    }

}

