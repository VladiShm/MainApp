using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using PluginInterface;
using System.Drawing.Imaging;

namespace Transforms7
{
    [Version(7, 0)]
    public class Razmitie : IPlugin
    {
        public string GetVersion()
        {
            Type type = typeof(Razmitie);
            VersionAttribute version = (VersionAttribute)Attribute.GetCustomAttribute(type, typeof(VersionAttribute));
            return ("Версия: " + version.Major + "." + version.Minor);
        }
        public string Name
        {
            get
            {
                return "Размытие";
            }
        }

        public string Author
        {
            get
            {
                return "Me";
            }
        }
            struct RGB
    {
    public float R;
    public float G;
    public float B;
    }

    public static UInt32[,] matrix_filtration(int W, int H, UInt32[,] pixel, int N, double[,] matryx)
    {
    int i, j, k, m, gap = (int)(N / 2);
    int tmpH = H + 2 * gap, tmpW = W + 2 * gap;
    UInt32[,] tmppixel = new UInt32[tmpH, tmpW];
    UInt32[,] newpixel = new UInt32[H, W];
    //заполнение временного расширенного изображения
    //углы
    for (i = 0; i < gap; i++)
    for (j = 0; j < gap; j++)
    {
    tmppixel[i, j] = pixel[0, 0];
    tmppixel[i, tmpW - 1 - j] = pixel[0, W - 1];
    tmppixel[tmpH - 1 - i, j] = pixel[H - 1, 0];
    tmppixel[tmpH - 1 - i, tmpW - 1 - j] = pixel[H - 1, W - 1];
    }
    //крайние левая и правая стороны
    for (i = gap; i < tmpH - gap; i++)
    for (j = 0; j < gap; j++)
    {
    tmppixel[i, j] = pixel[i - gap, j];
    tmppixel[i, tmpW - 1 - j] = pixel[i - gap, W - 1 - j];
    }
    //крайние верхняя и нижняя стороны
    for (i = 0; i < gap; i++)
    for (j = gap; j < tmpW - gap; j++)
    {
    tmppixel[i, j] = pixel[i, j - gap];
    tmppixel[tmpH - 1 - i, j] = pixel[H - 1 - i, j - gap];
    }
    //центр
    for (i = 0; i < H; i++)
    for (j = 0; j < W; j++)
    tmppixel[i + gap, j + gap] = pixel[i, j];
    //применение ядра свертки
    RGB ColorOfPixel = new RGB();
    RGB ColorOfCell = new RGB();
    for (i = gap; i < tmpH - gap; i++)
    for (j = gap; j < tmpW - gap; j++)
    {
    ColorOfPixel.R = 0;
    ColorOfPixel.G = 0;
    ColorOfPixel.B = 0;
    for (k = 0; k < N; k++)
    for (m = 0; m < N; m++)
    {
    ColorOfCell = calculationOfColor(tmppixel[i - gap + k, j - gap + m], matryx[k, m]);
    ColorOfPixel.R += ColorOfCell.R;
    ColorOfPixel.G += ColorOfCell.G;
    ColorOfPixel.B += ColorOfCell.B;
    }
    //контролируем переполнение переменных
    if (ColorOfPixel.R < 0) ColorOfPixel.R = 0;
    if (ColorOfPixel.R > 255) ColorOfPixel.R = 255;
    if (ColorOfPixel.G < 0) ColorOfPixel.G = 0;
    if (ColorOfPixel.G > 255) ColorOfPixel.G = 255;
    if (ColorOfPixel.B < 0) ColorOfPixel.B = 0;
    if (ColorOfPixel.B > 255) ColorOfPixel.B = 255;

    newpixel[i - gap, j - gap] = build(ColorOfPixel);
    }

    return newpixel;
    }

    //вычисление нового цвета
    private static RGB calculationOfColor(UInt32 pixel, double coefficient)
    {
    RGB Color = new RGB();
    Color.R = (float)(coefficient * ((pixel & 0x00FF0000) >> 16));
    Color.G = (float)(coefficient * ((pixel & 0x0000FF00) >> 8));
    Color.B = (float)(coefficient * (pixel & 0x000000FF));
    return Color;
    }

    //сборка каналов
    private static UInt32 build(RGB ColorOfPixel)
    {
    UInt32 Color;
    Color = 0xFF000000 | ((UInt32)ColorOfPixel.R << 16) | ((UInt32)ColorOfPixel.G << 8) | ((UInt32)ColorOfPixel.B);
    return Color;
    }

    //повышение резкости
    public const int N1 = 3;
    public static double[,] sharpness = new double[N1, N1] {{-1, -1, -1},
    {-1, 9, -1},
    {-1, -1, -1}};

    //размытие
    public const int N2 = 3;
    public static double[,] blur = new double[N1, N1] {{0.111, 0.111, 0.111},
    {0.111, 0.111, 0.111},
    {0.111, 0.111, 0.111}};

    public void Transform(Bitmap bitmap)
    {
    UInt32[,] pixel = new UInt32[bitmap.Height, bitmap.Width];
    for (int y = 0; y < bitmap.Height; y++)
    for (int x = 0; x < bitmap.Width; x++)
    pixel[y, x] = (UInt32)(bitmap.GetPixel(x, y).ToArgb());

    pixel = matrix_filtration(bitmap.Width, bitmap.Height, pixel, N2, blur);
    for (int y = 0; y < bitmap.Height; y++)
    for (int x = 0; x < bitmap.Width; x++)
    bitmap.SetPixel(x, y, Color.FromArgb((int)pixel[y, x]));
    }
    }
}
