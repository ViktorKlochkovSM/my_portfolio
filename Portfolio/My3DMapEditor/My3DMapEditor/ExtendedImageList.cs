using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace My3DMapEditor
{
    public delegate void SelectTextureDelegate(string name,Rectangle rect);
    public partial class ExtendedImageList : UserControl
    {
        public static event SelectTextureDelegate SelectTexture;

        private Hashtable texturesNames = new Hashtable();
        private ArrayList arrListNames = new ArrayList();
        private PictureBox lastSelectedPicture = null;
        //private ImagesAndNames lastSelectedControl = null;

        private int tmpX = 0;
        //private int lastY = 0;
        private int maxY = 0;

        private int curPos = 0;

        public ExtendedImageList()
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

        private void FirstRead()
        {
            try
            {
                Label lbl = new Label();

                int w = 0;
                maxY = 0;
                int lastX = 0;
                int y = 0;
                for (int i = curPos; i < curPos + 100; i++, w += lastX + 10)
                {
                    Image im = Image.FromFile(@"Textures\" + arrListNames[i].ToString());

                    ImagesAndNames imgNm = new ImagesAndNames(im, arrListNames[i].ToString(), i);

                    if (w + 10 + imgNm.Width > panel1.Width - 10)
                    {
                        w = 0;
                        y += maxY;
                        tmpX = 0;
                        maxY = 0;
                    }

                    imgNm.Location = new Point(w + 10, y + 10);

                    imgNm.pictureBox1.Click += new EventHandler(pictureBox1_Click);

                    if (i == 0)
                    {
                        pictureBox1_Click(imgNm.pictureBox1, null);
                    }

                    lastX = imgNm.MyWidth + 10;

                    tmpX += imgNm.MyWidth + 10;

                    if (maxY < imgNm.Height)
                        maxY = imgNm.Height;

                    panel1.Controls.Add(imgNm);

                }
                curPos += 100;
                panel1.AutoScrollMinSize = new Size(w + 50, y + 10);
                tmpX = 0;
                textBox1.Text = curPos.ToString();
            }
            catch
            {
                return;
            }
        }

        void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                if (lastSelectedPicture == null)
                {
                    ((PictureBox)sender).BackColor = Color.Red;
                    lastSelectedPicture = ((PictureBox)sender);
                    //MessageBox.Show(lastSelectedPicture.Image.Tag.ToString());
                    if (SelectTexture != null)
                        SelectTexture(lastSelectedPicture.Image.Tag.ToString(), ((PictureBox)sender).ClientRectangle);
                }
                else
                {
                    lastSelectedPicture.BackColor = Color.Black;
                    ((PictureBox)sender).BackColor = Color.Red;
                    lastSelectedPicture = ((PictureBox)sender);
                    //MessageBox.Show(lastSelectedPicture.Image.Tag.ToString());
                    if (SelectTexture != null)
                        SelectTexture(lastSelectedPicture.Image.Tag.ToString(), ((PictureBox)sender).ClientRectangle);
                }
            }
            catch
            {
                return;
            }
        }

        private void ExtendedImageList_Load(object sender, EventArgs e)
        {
            try
            {
                panel1.AutoScrollMinSize = new Size(1000, 1000);

                this.VerticalScroll.LargeChange = 50;
                this.VerticalScroll.SmallChange = 50;

                DirectoryInfo dinfo = new DirectoryInfo(@"Textures\");
                if (dinfo.Exists)
                {
                    FileInfo[] finfo = new FileInfo[50];
                    finfo = dinfo.GetFiles();

                    int i1 = 1;
                    foreach (FileInfo fn in finfo)
                    {
                        if (fn.Extension == ".jpg" || fn.Extension == ".bmp")
                        {
                            arrListNames.Add(fn.Name);
                        }
                        i1++;
                    }
                    arrListNames.Sort();

                    label1.Text = arrListNames.Count.ToString();

                    FirstRead();
                }
                textBox1.Text = curPos.ToString();
            }
            catch
            {
                return;
            }
        }


        private void NextRead()
        {
            try
            {
                bool dd = true;

                int w = 0;
                maxY = 0;
                int lastX = 0;
                int y = 0;
                if (curPos + 100 < arrListNames.Count)
                {
                    for (int i = curPos; i < curPos + 100; i++, w += lastX + 10)
                    {
                        Image im = Image.FromFile(@"Textures\" + arrListNames[i].ToString());

                        ImagesAndNames imgNm = new ImagesAndNames(im, arrListNames[i].ToString(), i);

                        if (w + 10 + imgNm.Width > panel1.Width - 10)
                        {
                            w = 0;
                            y += maxY;
                            tmpX = 0;
                            maxY = 0;
                        }

                        if (dd)
                        {
                            pictureBox1_Click(imgNm.pictureBox1, null);
                            dd = false;
                        }

                        imgNm.Location = new Point(w + 10, y + 10);

                        imgNm.pictureBox1.Click += new EventHandler(pictureBox1_Click);

                        if (i == 0)
                        {
                            pictureBox1_Click(imgNm.pictureBox1, null);
                        }

                        lastX = imgNm.MyWidth + 10;

                        tmpX += imgNm.MyWidth + 10;

                        if (maxY < imgNm.Height)
                            maxY = imgNm.Height;

                        panel1.Controls.Add(imgNm);

                    }
                    curPos += 100;
                    panel1.AutoScrollMinSize = new Size(w + 50, y + 10);
                    tmpX = 0;
                }
                else
                {
                    for (int i = curPos; i < arrListNames.Count; i++, w += lastX + 10)
                    {
                        curPos++;
                        Image im = Image.FromFile(@"Textures\" + arrListNames[i].ToString());

                        ImagesAndNames imgNm = new ImagesAndNames(im, arrListNames[i].ToString(), i);

                        if (w + 10 + imgNm.Width > panel1.Width - 10)
                        {
                            w = 0;
                            y += maxY;
                            tmpX = 0;
                        }

                        if (dd)
                        {
                            pictureBox1_Click(imgNm.pictureBox1, null);
                            dd = false;
                        }

                        imgNm.Location = new Point(w + 10, y + 10);

                        imgNm.pictureBox1.Click += new EventHandler(pictureBox1_Click);

                        if (i == 0)
                        {
                            pictureBox1_Click(imgNm.pictureBox1, null);
                        }

                        lastX = imgNm.MyWidth + 10;

                        tmpX += imgNm.MyWidth + 10;

                        if (maxY < imgNm.Height)
                            maxY = imgNm.Height;

                        panel1.Controls.Add(imgNm);


                    }
                    //curPos += 100;
                    panel1.AutoScrollMinSize = new Size(w + 50, y + 10);
                    tmpX = 0;
                }
                textBox1.Text = curPos.ToString();
            }
            catch
            {
                return;
            }
        }

        private void PrevRead()
        {
            try
            {
                bool dd = true;
                Label l = new Label();

                int w = 0;
                maxY = 0;
                int lastX = 0;
                int y = 0;
                if (curPos == arrListNames.Count)
                    curPos--;
                if (curPos - 100 > 0)
                {
                    for (int i = curPos; i > curPos - 100; i--, w += lastX + 10)
                    {
                        Image im = Image.FromFile(@"Textures\" + arrListNames[i].ToString());

                        ImagesAndNames imgNm = new ImagesAndNames(im, arrListNames[i].ToString(), i);

                        if (w + 10 + imgNm.Width > panel1.Width - 10)
                        {
                            w = 0;
                            y += maxY;
                            tmpX = 0;
                            maxY = 0;
                        }

                        if (dd)
                        {
                            pictureBox1_Click(imgNm.pictureBox1, null);
                            dd = false;
                        }

                        imgNm.Location = new Point(w + 10, y + 10);

                        imgNm.pictureBox1.Click += new EventHandler(pictureBox1_Click);

                        if (i == 0)
                        {
                            pictureBox1_Click(imgNm.pictureBox1, null);
                        }

                        lastX = imgNm.MyWidth + 10;

                        tmpX += imgNm.MyWidth + 10;

                        if (maxY < imgNm.Height)
                            maxY = imgNm.Height;

                        panel1.Controls.Add(imgNm);

                    }
                    curPos -= 100;
                    panel1.AutoScrollMinSize = new Size(w + 50, y + 10);
                    tmpX = 0;
                }
                else
                {
                    for (int i = curPos; i > 0; i--, w += lastX + 10)
                    {
                        Image im = Image.FromFile(@"Textures\" + arrListNames[i].ToString());

                        ImagesAndNames imgNm = new ImagesAndNames(im, arrListNames[i].ToString(), i);

                        if (w + 10 + imgNm.Width > panel1.Width - 10)
                        {
                            w = 0;
                            y += maxY;
                            tmpX = 0;
                            maxY = 0;
                        }

                        if (dd)
                        {
                            pictureBox1_Click(imgNm.pictureBox1, null);
                            dd = false;
                        }

                        imgNm.Location = new Point(w + 10, y + 10);

                        imgNm.pictureBox1.Click += new EventHandler(pictureBox1_Click);

                        if (i == 0)
                        {
                            pictureBox1_Click(imgNm.pictureBox1, null);
                        }

                        lastX = imgNm.MyWidth + 10;

                        tmpX += imgNm.MyWidth + 10;

                        if (maxY < imgNm.Height)
                            maxY = imgNm.Height;

                        panel1.Controls.Add(imgNm);

                    }
                    curPos = 0;
                    panel1.AutoScrollMinSize = new Size(w + 50, y + 10);
                    tmpX = 0;
                }


                textBox1.Text = curPos.ToString();
            }
            catch
            {
                return;
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (curPos < arrListNames.Count)
                {
                    DeleteAllPictures();

                    NextRead();

                }
            }
            catch
            {
                return;
            }
        }

        //void p_Click(object sender, EventArgs e)
        //{
        //    if (lastSelectedPicture == null)
        //    {
        //        ((PictureBox)sender).BackColor = Color.Red;
        //        lastSelectedPicture = ((PictureBox)sender);
        //    }
        //    else
        //    {
        //        lastSelectedPicture.BackColor = Color.Black;
        //        ((PictureBox)sender).BackColor = Color.Red;
        //        lastSelectedPicture = ((PictureBox)sender);
        //    }
        //}
        private void DeleteAllPictures()
        {
            try
            {

                lastSelectedPicture = null;
                panel1.AutoScrollMinSize = new Size();
                panel1.Controls.Clear();
                tmpX = 0;
                maxY = 0;
            }
            catch
            {
                return;
            }
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (curPos > 0)
            {
                try
                {
                    DeleteAllPictures();

                    PrevRead();
                }
                catch
                {
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)//Jamp to index
        {
            try
            {
                int t = 0;
                if (!int.TryParse(textBox1.Text, out t))
                {
                    textBox1.Text = curPos.ToString();
                    //MessageBox.Show("Must be integer characters!!!");
                }
                else
                {
                    if (t >= arrListNames.Count || t < 0)
                    {
                        //MessageBox.Show("Out of range!!!");
                        textBox1.Text = curPos.ToString();
                    }
                    else
                    {
                        curPos = t;
                        buttonNext_Click(null, null);
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)//Search
        {
            try
            {
                bool dd = true;
                DeleteAllPictures();
                curPos = 0;
                if (textBox2.Text != "")
                {
                    int y = 0;
                    int w = 0;
                    int lastX = 0;
                    //int k = 0;
                    for (int i = curPos, j = 1; i < arrListNames.Count; i++)
                    {
                        string name = arrListNames[i].ToString();

                        for (int r = 0, s = textBox2.Text.Length; s < name.Length; s++, r++)
                        {
                            string nm = name.Substring(r, textBox2.Text.Length);
                            if (nm.ToLower() == textBox2.Text.ToLower())
                            {
                                Image im = Image.FromFile(@"Textures\" + arrListNames[i].ToString());

                                ImagesAndNames imgNm = new ImagesAndNames(im, arrListNames[i].ToString(), i);


                                if (w + 10 + imgNm.Width > panel1.Width - 10)
                                {
                                    w = 0;
                                    y += maxY;
                                    tmpX = 0;
                                    maxY = 0;
                                }


                                if (dd)
                                {
                                    pictureBox1_Click(imgNm.pictureBox1, null);
                                    dd = false;
                                }

                                imgNm.Location = new Point(w + 10, y + 10);



                                imgNm.pictureBox1.Click += new EventHandler(pictureBox1_Click);

                                if (i == 0)
                                {
                                    pictureBox1_Click(imgNm.pictureBox1, null);
                                }

                                lastX = imgNm.MyWidth + 10;

                                tmpX += imgNm.MyWidth + 10;

                                if (maxY < imgNm.Height)
                                    maxY = imgNm.Height;

                                panel1.Controls.Add(imgNm);

                                w += lastX + 10;

                                j++;
                                break;
                            }
                        }
                    }

                    panel1.AutoScrollMinSize = new Size(w + 50, y + 10);
                    tmpX = 0;
                }
            }
            catch
            {
                return;
            }
        }
    }
}
