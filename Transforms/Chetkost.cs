using PluginInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transforms
{
    [Version(8, 0)]
    internal class Chetkost : IPlugin
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
                return "Четкость";
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
            float[,] kernel = {
{ -1, -1, -1 },
{ -1, 9, -1 },
{ -1, -1, -1 }
};

            Bitmap result = new Bitmap(bitmap.Width, bitmap.Height);
            int kernelOffset = 1;

            for (int x = kernelOffset; x < bitmap.Width - kernelOffset; x++)
            {
                for (int y = kernelOffset; y < bitmap.Height - kernelOffset; y++)
                {
                    Color newColor = ApplyKernel(bitmap, x, y, kernel, kernelOffset);
                    result.SetPixel(x, y, newColor);
                }
            }

            // Replace the original bitmap with the sharpened result
            bitmap = result;
        }

        private Color ApplyKernel(Bitmap bitmap, int x, int y, float[,] kernel, int kernelOffset)
        {
            float r = 0, g = 0, b = 0;

            for (int i = -kernelOffset; i <= kernelOffset; i++)
            {
                for (int j = -kernelOffset; j <= kernelOffset; j++)
                {
                    Color pixel = bitmap.GetPixel(x + i, y + j);
                    r += pixel.R * kernel[i + kernelOffset, j + kernelOffset];
                    g += pixel.G * kernel[i + kernelOffset, j + kernelOffset];
                    b += pixel.B * kernel[i + kernelOffset, j + kernelOffset];
                }
            }

            r = Math.Min(Math.Max((int)r, 0), 255);
            g = Math.Min(Math.Max((int)g, 0), 255);
            b = Math.Min(Math.Max((int)b, 0), 255);

            return Color.FromArgb((int)r, (int)g, (int)b);
        }
    }
}
