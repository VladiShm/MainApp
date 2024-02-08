using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PluginInterface;

namespace MainApp
{
    public partial class Form1 : Form
    {
        Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();
        public Form1()
        {
            InitializeComponent();
            FindPlugins();
            CreatePluginsMenu();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        void FindPlugins()
        {
            // папка с плагинами
            string folder = "C:\\Users\\vlada\\source\\repos\\MainApp\\MainApp\\Plagins";

            // dll-файлы в этой папке
            string[] files = Directory.GetFiles(folder, "*.dll");

            foreach (string file in files)
                try
                {
                    Assembly assembly = Assembly.LoadFile(file);

                    foreach (Type type in assembly.GetTypes())
                    {
                        Type iface = type.GetInterface("PluginInterface.IPlugin");

                        if (iface != null)
                        {
                            IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                            plugins.Add(plugin.Name, plugin);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки плагина\n" + ex.Message);
                }
        }

        private void OnPluginClick(object sender, EventArgs args)
        {
            IPlugin plugin = plugins[((ToolStripMenuItem)sender).Text];
            plugin.Transform((Bitmap)pictureBox.Image);
            pictureBox.Refresh();
            textBox1.Text = $"Автор: {plugin.Author}--- Название плагина: {plugin.Name} {plugin.GetVersion()}";

        }
        void CreatePluginsMenu()
        {
            foreach (IPlugin p in plugins.Values)
            {
                var menuItem = new ToolStripMenuItem(p.Name);
                menuItem.Click += OnPluginClick;

                filtersToolStripMenuItem.DropDownItems.Add(menuItem);
               
            }

        }

        private void filtersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

    }
}
