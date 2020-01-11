using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Reflection;


namespace ZAD1_CSzarp
{
    public partial class Form1 : Form
    {
        void LoadPlugins()
        {
            foreach (string myFilename in Directory.GetFiles(@"C:\Root", "*.dll", SearchOption.AllDirectories))
            {
                Assembly plugin = Assembly.LoadFrom(myFilename);
                foreach (Type item in plugin.GetTypes()) //lista klas
                    foreach (MethodInfo method in item.GetMethods())
                    {
                        
                        wtyczkiToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(method.Name, null,
MenuHandler, myFilename + "|" + item.Name + "|" + method.Name));

                    }

            }
        }

        private void MenuHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            string[] names = item.Name.Split(new char[] { '|' });
            string NazwaAssembly = names[0];
            string NazwaKlasy = names[1];
            string NazwaMetody = names[2];
            Assembly plugin = Assembly.LoadFile(NazwaAssembly);
            MethodInfo method;
            foreach (Type item1 in plugin.GetTypes())
            {
                method = item1.GetMethod(NazwaMetody);
                object result;
                try
                {
                    result = method.Invoke(null, new object[] { });
                    method = item1.GetMethod(result.ToString().Substring(1));
                }
                catch
                {
                    continue;
                }
                if (result.ToString().Substring(0, 1) == "r")
                {
                    method.Invoke(null, new object[] { richTextBox1 });
                }
                else if(result.ToString().Substring(0, 1).Equals("s")){
                        richTextBox1.Text = method.Invoke(null, new object[] { result.ToString().Substring(1) }).ToString();
                }
                else if (result.ToString().Substring(0, 1).Equals("e"))
                {
                    method.Invoke(null, new object[] { result.ToString().Substring(1) });
                }
                break;


            }
            


        }

        public Form1()
        {
            InitializeComponent();
            LoadPlugins();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.DefaultExt = "*.rtf";
            saveFile1.Filter = "RTF Files|*.rtf";
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
            saveFile1.FileName.Length > 0)
            {
                richTextBox1.SaveFile(saveFile1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile1 = new OpenFileDialog();
            openFile1.DefaultExt = "*.rtf";
            openFile1.Filter = "RTF Files|*.rtf";
            if (openFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
            openFile1.FileName.Length > 0)
            {
                richTextBox1.LoadFile(openFile1.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!wtyczkiToolStripMenuItem.DropDownItems.ContainsKey(textBox1.Text))
            {
                wtyczkiToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(textBox1.Text,
 null, MenuHandler, textBox1.Text));
            }

        }
    }
}
