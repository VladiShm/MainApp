using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginInterface;

namespace Transforms
{
    [Version(5, 0)]
    internal class Date : IPlugin
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
                    return "Дата";
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
            int xPosition = bitmap.Width - 400;
            int yPosition = bitmap.Height - 200;

            string fontName = "";
            float fontSize = 40;
            string colorName1 = "Red";
            string colorName2 = "Red";
            string fontStyle = "";
            string text = DateTime.Today.ToShortDateString();

            Graphics gr = Graphics.FromImage(bitmap);

            if (string.IsNullOrEmpty(fontName))
                fontName = "Times New Roman";
            if (fontSize == null)
                fontSize = 10.0F;

            Font font = new Font(fontName, fontSize);

            if (!string.IsNullOrEmpty(fontStyle))
            {
                FontStyle fStyle = FontStyle.Regular;
                switch (fontStyle.ToLower())
                {
                    case "bold":
                        fStyle = FontStyle.Bold;
                        break;
                    case "italic":
                        fStyle = FontStyle.Italic;
                        break;
                    case "underline":
                        fStyle = FontStyle.Underline;
                        break;
                    case "strikeout":
                        fStyle = FontStyle.Strikeout;
                        break;
                }
                font = new Font(fontName, fontSize, fStyle);
            }

            if (string.IsNullOrEmpty(colorName1))
                colorName1 = "Black";
            if (string.IsNullOrEmpty(colorName2))
                colorName2 = colorName1;

            Color color1 = Color.FromName(colorName1);
            Color color2 = Color.FromName(colorName2);

            int gW = (int)(text.Length * fontSize);
            gW = gW == 0 ? 10 : gW;

            LinearGradientBrush LGBrush = new LinearGradientBrush(new Rectangle(0, 0, gW, (int)fontSize), color1, color2, LinearGradientMode.Vertical);

            gr.DrawString(text, font, LGBrush, xPosition, yPosition);
        }
    }
}
