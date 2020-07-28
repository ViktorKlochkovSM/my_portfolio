using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace My3DMapEditor
{
    

    public partial class ImagesAndNames : UserControl
    {
        int index = -1;
        public ImagesAndNames()
        {
            try
            {
                InitializeComponent();
            }
            catch
            {
                return;
            }
        }
        public ImagesAndNames(Image img,string name,int indx)
        {
            try
            {
                InitializeComponent();

                index = indx;

                pictureBox1.Size = new Size(img.Width + 4, img.Height + 4);

                pictureBox1.Padding = new Padding(2, 2, 0, 0);
                pictureBox1.Image = img;
                pictureBox1.Image.Tag = name;


                int ln = name.Length * 8;


                if (img.Width >= ln)
                {
                    this.Width = img.Width + 10;

                    this.Height = img.Height + 10 + label1.Height + 10;
                }
                else
                {
                    this.Width = ln + 10;

                    this.Height = img.Height + 10 + label1.Height + 10;
                }

                if (pictureBox1.Width >= ln)
                {
                    pictureBox1.Location = new Point(0, 0);

                    label1.Width = pictureBox1.Width;
                    label1.Location = new Point(0, img.Height + 3);
                }
                else
                {
                    pictureBox1.Location = new Point((this.Width / 2) - (pictureBox1.Width / 2), 0);

                    label1.Width = ln;
                    label1.Location = new Point(0, img.Height + 3);
                }
                label1.Text = name;
            }
            catch
            {
                return;
            }
        }
        public int MyWidth
        {
            get 
            {
                if (pictureBox1.Width >= label1.Width)
                {
                    return pictureBox1.Width;
                }
                else
                {
                    return label1.Width;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //((PictureBox)sender).BackColor = Color.Red;
        }

        private void ImagesAndNames_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Its my object!");
        }
    }
}
