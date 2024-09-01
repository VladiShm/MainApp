using PluginInterface;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Transforms2
{
    [Version(8, 0)]
    internal class Chetkost : IPlugin
    {
        public string GetVersion()
        {
            Type type = typeof(Chetkost);
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
            int width = bitmap.Width;
            int height = bitmap.Height;

            // Создание матрицы улучшения чёткости
            float[,] matrix = {
            { -1, -1, -1 },
            { -1,  9, -1 },
            { -1, -1, -1 }
        };

            // Применение матрицы к изображению
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            int byteCount = bitmapData.Stride * height;
            byte[] pixels = new byte[byteCount];
            Marshal.Copy(bitmapData.Scan0, pixels, 0, byteCount);

            int stride = bitmapData.Stride;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float r = 0, g = 0, b = 0;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int offsetX = x + j;
                            int offsetY = y + i;

                            if (offsetX >= 0 && offsetX < width && offsetY >= 0 && offsetY < height)
                            {
                                int index = offsetY * stride + offsetX * bytesPerPixel;

                                r += pixels[index + 2] * matrix[i + 1, j + 1];
                                g += pixels[index + 1] * matrix[i + 1, j + 1];
                                b += pixels[index] * matrix[i + 1, j + 1];
                            }
                        }
                    }

                    int currentIndex = y * stride + x * bytesPerPixel;
                    pixels[currentIndex] = (byte)Math.Max(0, Math.Min(255, b));
                    pixels[currentIndex + 1] = (byte)Math.Max(0, Math.Min(255, g));
                    pixels[currentIndex + 2] = (byte)Math.Max(0, Math.Min(255, r));
                }
            }

            Marshal.Copy(pixels, 0, bitmapData.Scan0, byteCount);
            bitmap.UnlockBits(bitmapData);
        }
    }
    
}
