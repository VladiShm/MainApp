using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using PluginInterface;
using System.Xml.Linq;
using System.Linq;

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
            string configFile = "C:\\Users\\vlada\\source\\repos\\MainApp\\MainApp\\plugins.config"; // путь к конфигурационному файлу
            List<string> pluginsToLoad = new List<string>();

            // Проверяем, существует ли конфигурационный файл
            if (File.Exists(configFile))
            {
                // Загружаем конфигурационный файл
                XDocument configDocument = XDocument.Load(configFile);
                var autoLoad = configDocument.Element("configuration").Element("plugins").Attribute("autoLoad").Value;

                // Если в конфигурации указано autoLoad="false", загружаем только перечисленные плагины
                if (autoLoad.Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var pluginElement in configDocument.Element("configuration").Element("plugins").Elements("add"))
                    {
                        string pluginName = pluginElement.Attribute("name").Value;
                        if (!string.IsNullOrEmpty(pluginName))
                        {
                            pluginsToLoad.Add(Path.Combine(folder, pluginName));
                        }
                    }
                }
            }

            // dll-файлы в этой папке (если autoLoad=true или файла конфигурации нет, загружаем все dll)
            string[] files = (pluginsToLoad.Count == 0) ? Directory.GetFiles(folder, "*.dll") : pluginsToLoad.ToArray();

            foreach (string file in files)
            {
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

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.gif, *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox.Image = new Bitmap(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке изображения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
