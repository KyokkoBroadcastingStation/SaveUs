using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaveMe.Window
{
    public partial class genre : Form
    {
        public string _genre { get; set; }
        public genre(string dst_path)
        {
            InitializeComponent();
            DirectoryInfo di = new DirectoryInfo(dst_path);
            DirectoryInfo[] diall = di.GetDirectories();
            foreach(DirectoryInfo d in diall)
            {
                comboBox1.Items.Add(d.Name);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text != null)
            {
                _genre = comboBox1.Text;
            }
        }
    }
}
