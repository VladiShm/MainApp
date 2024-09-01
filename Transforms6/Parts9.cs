using PluginInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transforms6
{
    [Version(6, 0)]
    internal class Parts9 : IPlugin
    {
        public string GetVersion()
        {
            Type type = typeof(Parts9);
            VersionAttribute version = (VersionAttribute)Attribute.GetCustomAttribute(type, typeof(VersionAttribute));
            return ("Версия: " + version.Major + "." + version.Minor);
        }
        public string Name
        {
            get
            {
                return "Разбиение на 9 частей";
            }
        }

        public string Author
        {
            get
            {
                return "Me";
            }
        }

        public void Transform(Bitmap inputImage)
        {
            int chunkWidth = inputImage.Width / 3;
            int chunkHeight = inputImage.Height / 3;
            int index = 0;
            Bitmap[] chunks = new Bitmap[9];

            // Split the image into 9 chunks
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    chunks[index] = new Bitmap(chunkWidth, chunkHeight);
                    using (Graphics g = Graphics.FromImage(chunks[index]))
                    {
                        g.DrawImage(inputImage, new Rectangle(0, 0, chunkWidth, chunkHeight),
                                    new Rectangle(x * chunkWidth, y * chunkHeight, chunkWidth, chunkHeight),
                                    GraphicsUnit.Pixel);
                    }
                    index++;
                }
            }

            // Shuffle the chunks
            Random rng = new Random();
            int n = chunks.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Bitmap temp = chunks[k];
                chunks[k] = chunks[n];
                chunks[n] = temp;
            }

            // Draw the shuffled chunks onto the original image
            index = 0;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    using (Graphics g = Graphics.FromImage(inputImage))
                    {
                        g.DrawImage(chunks[index], new Point(x * chunkWidth, y * chunkHeight));
                    }
                    index++;
                }
            }
        }

    }
}
