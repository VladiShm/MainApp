using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using PluginInterface;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Transforms8
{
    [Version(8, 0)]
    public class MedianFiltr : IPlugin
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
                return "Медианный фильтр";
            }
        }

        public string Author
        {
            get
            {
                return "Me";
            }
        }

        public void Transform(Bitmap image)
        {
            int w = image.Width;
            int h = image.Height;

            BitmapData image_data = image.LockBits(
            new Rectangle(0, 0, w, h),
            ImageLockMode.ReadOnly,
            PixelFormat.Format24bppRgb);
            int bytes = image_data.Stride * image_data.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
            image.UnlockBits(image_data);
            int r = 1;
            int wres = w - 2 * r;
            int hres = h - 2 * r;

            BitmapData result_data = image.LockBits(
            new Rectangle(0, 0, wres, hres),
            ImageLockMode.WriteOnly,
            PixelFormat.Format24bppRgb);
            int res_bytes = result_data.Stride * result_data.Height;
            byte[] result = new byte[res_bytes];

            for (int x = r; x < w - r; x++)
            {
                for (int y = r; y < h - r; y++)
                {
                    int pixel_location = x * 3 + y * image_data.Stride;
                    int res_pixel_loc = (x - r) * 3 + (y - r) * result_data.Stride;
                    double[] median = new double[3];
                    byte[][] neighborhood = new byte[3][];

                    for (int c = 0; c < 3; c++)
                    {
                        neighborhood[c] = new byte[(int)Math.Pow(2 * r + 1, 2)];
                        int added = 0;
                        for (int kx = -r; kx <= r; kx++)
                        {
                            for (int ky = -r; ky <= r; ky++)
                            {
                                int kernel_pixel = pixel_location + kx * 3 + ky * image_data.Stride;
                                neighborhood[c][added] = buffer[kernel_pixel + c];
                                added++;
                            }
                        }
                    }

                    for (int c = 0; c < 3; c++)
                    {
                        result[res_pixel_loc + c] = Median(neighborhood[c]);
                    }
                }
            }

            Marshal.Copy(result, 0, result_data.Scan0, res_bytes);
            image.UnlockBits(result_data);
        }

        byte Median(byte[] byteArray)
        {
            Array.Sort(byteArray); // Сортировка массива байтов

            byte median;
            if (byteArray.Length % 2 == 0) // Если количество элементов четное
            {
                // Находим средние два элемента и находим их среднее значение
                median = (byte)((byteArray[byteArray.Length / 2] + byteArray[byteArray.Length / 2 - 1]) / 2);
            }
            else
            {
                // Если количество элементов нечетное, то медианой будет элемент посередине массива
                median = byteArray[byteArray.Length / 2];
            }
            return median;
        }
    }
}
