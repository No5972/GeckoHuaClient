using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JingsuPlatform
{
    public partial class CustomResolution : Form
    {
        public int thisHeight { get; set; }
        public int thisWidth { get; set; }

        public CustomResolution()
        {
            InitializeComponent();
        }

        public CustomResolution(int width, int height)
        {
            InitializeComponent();
            this.textBox1.Value = width;
            this.textBox2.Value = height;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.thisWidth = (int)this.textBox1.Value;
            this.thisHeight = (int)this.textBox2.Value;
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void CustomResolution_Load(object sender, EventArgs e)
        {
            this.textBox1.Select();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.button1_Click(sender, e);
            }
        }
    }
}
