using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace My3DMapEditor
{
    public partial class Hollow : Form
    {
        int currentSizeGrid;
        public Hollow(int cSize)
        {
            InitializeComponent();

            currentSizeGrid = cSize;
        }

        private void Hollow_Load(object sender, EventArgs e)
        {
            textBox1.Text = currentSizeGrid.ToString();
            this.CenterToScreen();
        }
    }
}