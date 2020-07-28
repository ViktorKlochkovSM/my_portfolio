using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace My3DMapEditor
{
    public delegate void ApplyDelegate(string nameText, int indexSide);
    public delegate void ChangeTexturePositionDelegate(float x, float y, float sx, float sy, int side, RectPrimitive rpr);
    public delegate void HideSelectionDelegate(bool isSelHide);
    public delegate void TextureFormKeyDownDelegate(object sender, KeyEventArgs e);

    public partial class TexturesSettings : Form
    {
        private RectPrimitive selPrimitive = null;
        public RectPrimitive convertedPrimitive = null;
        private Rectangle selTextureSize = new Rectangle();
        private bool showMark = true;

        static public event ApplyDelegate Apply;

        static public event ChangeTexturePositionDelegate ChangeTexturePosition;

        static public event HideSelectionDelegate HideSelection;

        public event TextureFormKeyDownDelegate TextureFormKeyDown;

        public string nameTex = "";

        private string pathToLibrary = "";
        
        public TexturesSettings()
        {
            InitializeComponent();

            ExtendedImageList.SelectTexture += new SelectTextureDelegate(ExtendedImageList_SelectTexture);

            Form1.SelectObject += new SelectObjectDelegate(Form1_SelectObject);
        }

        public void Form1_SelectObject(RectPrimitive rp,RectPrimitive convertedRpr)
        {
            selPrimitive = rp;
            convertedPrimitive = convertedRpr;
        }

        void ExtendedImageList_SelectTexture(string name,Rectangle rect)
        {
            if (name != "")
            {
                nameTex = name;
                selTextureSize = rect;
            }
        }

        private void TexturesSettings_Load(object sender, EventArgs e)
        {
            try
            {
                radioButtonAllSide.Enabled = true;

                radioButtonNone.Checked = true;

                textBoxX.Enabled = false;
                textBoxY.Enabled = false;
                buttonXMinus.Enabled = false;
                buttonXPlus.Enabled = false;
                buttonYMinus.Enabled = false;
                buttonYPlus.Enabled = false;
                comboBoxModeIncrement.Enabled = false;


                buttonRightDownXPlus.Enabled = false;
                buttonRightDownXMinus.Enabled = false;
                buttonLeftDownYPlus.Enabled = false;
                buttonLeftDownYMinus.Enabled = false;
                textBoxScaleX.Enabled = false;
                textBoxScaleY.Enabled = false;

                SearchFilesInFolder();

                comboBoxModeIncrement.SelectedIndex = 2;
                this.SetDesktopLocation(Screen.PrimaryScreen.WorkingArea.Right - this.Width, 25);

                //extendedImageList1.VerticalScroll.Minimum = 50;
                //extendedImageList1.VerticalScroll.Maximum = 50;

                this.MouseWheel += new MouseEventHandler(TexturesSettings_MouseWheel);


                Environment.CurrentDirectory = Form1.startingPath;
                if (File.Exists("Marks.mk"))
                {
                    FileStream f = File.Open("Marks.mk", FileMode.Open);
                    StreamReader r = new StreamReader(f);

                    richTextBox1.Clear();
                    richTextBox1.Text = r.ReadToEnd();

                    r.Close();
                    f.Close();
                }

                showMark = false;
                this.Width = this.Width - richTextBox1.Width;
                richTextBox1.Hide();
                this.SetDesktopLocation(Screen.PrimaryScreen.WorkingArea.Right - this.Width, 25);
            }
            catch
            {
                return;
            }
        }

        void TexturesSettings_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Delta > 0)
                {
                    if (extendedImageList1.panel1.VerticalScroll.Value - 100 > 0)
                        extendedImageList1.panel1.VerticalScroll.Value -= 100;
                    else
                        extendedImageList1.panel1.VerticalScroll.Value = 0;
                }
                else
                {
                    if (extendedImageList1.panel1.VerticalScroll.Value + 100 < extendedImageList1.panel1.VerticalScroll.Maximum)
                        extendedImageList1.panel1.VerticalScroll.Value += 100;
                    else
                        extendedImageList1.panel1.VerticalScroll.Value = extendedImageList1.panel1.VerticalScroll.Maximum;
                }
            }
            catch
            {
                return;
            }
        }
        private void SearchFilesInFolder()
        {
            try
            {
                Environment.CurrentDirectory = Form1.startingPath;
                DirectoryInfo dinfo = new DirectoryInfo(@"Textures\");
                if (dinfo.Exists)
                {
                    pathToLibrary = dinfo.FullName;
                    FileInfo[] finfo = new FileInfo[50];
                    finfo = dinfo.GetFiles();
                }
                else
                {
                    MessageBox.Show("The folder \"Textures\" not exist!");
                    return;
                }
            }
            catch
            {
                return;
            }
        }
        private void radioButtonAllSide_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (radioButtonAllSide.Checked)
                {
                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void radioButtonFrontSide_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (radioButtonFrontSide.Checked)
                {
                    if (radioButtonNoOwn.Checked)
                    {
                        Calc();
                    }
                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void radioButtonRightSide_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (radioButtonRightSide.Checked)
                {
                    if (radioButtonNoOwn.Checked)
                    {
                        Calc();
                    }
                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void radioButtonBackSide_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (radioButtonBackSide.Checked)
                {
                    if (radioButtonNoOwn.Checked)
                    {
                        Calc();
                    }
                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void radioButtonLeftSide_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (radioButtonLeftSide.Checked)
                {
                    if (radioButtonNoOwn.Checked)
                    {
                        Calc();
                    }
                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void radioButtonUpSide_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (radioButtonUpSide.Checked)
                {
                    if (radioButtonNoOwn.Checked)
                    {
                        Calc();
                    }
                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void radioButtonDownSide_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (radioButtonDownSide.Checked)
                {
                    if (radioButtonNoOwn.Checked)
                    {
                        Calc();
                    }
                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void radioButtonScale_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (radioButtonScale.Checked)
                {
                    textBoxX.Text = "0";
                    textBoxY.Text = "0";

                    textBoxScaleY.Text = "1";
                    textBoxScaleX.Text = "1";

                    radioButtonAllSide.Enabled = true;

                    radioButtonNone.Checked = true;

                    textBoxX.Enabled = false;
                    textBoxY.Enabled = false;
                    buttonXMinus.Enabled = false;
                    buttonXPlus.Enabled = false;
                    buttonYMinus.Enabled = false;
                    buttonYPlus.Enabled = false;
                    comboBoxModeIncrement.Enabled = false;


                    buttonRightDownXPlus.Enabled = false;
                    buttonRightDownXMinus.Enabled = false;
                    buttonLeftDownYPlus.Enabled = false;
                    buttonLeftDownYMinus.Enabled = false;
                    textBoxScaleX.Enabled = false;
                    textBoxScaleY.Enabled = false;
                }
            }
            catch
            {
                return;
            }
        }

        private void radioButtonNoOwn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (radioButtonNoOwn.Checked)
                {
                    radioButtonAllSide.Enabled = false;
                    radioButtonNone.Checked = true;

                    textBoxX.Enabled = true;
                    textBoxY.Enabled = true;
                    buttonXMinus.Enabled = true;
                    buttonXPlus.Enabled = true;
                    buttonYMinus.Enabled = true;
                    buttonYPlus.Enabled = true;

                    buttonRightDownXPlus.Enabled = true;
                    buttonRightDownXMinus.Enabled = true;
                    buttonLeftDownYPlus.Enabled = true;
                    buttonLeftDownYMinus.Enabled = true;
                    textBoxScaleX.Enabled = true;
                    textBoxScaleY.Enabled = true;

                    comboBoxModeIncrement.Enabled = true;
                }
            }
            catch
            {
                return;
            }
        }

        private void Calc()
        {
            try
            {
                if (selPrimitive == null || convertedPrimitive == null)
                    return;

                int actionSide = 0;
                if (radioButtonAllSide.Checked)
                    actionSide = 0;
                if (radioButtonFrontSide.Checked)
                    actionSide = 1;
                if (radioButtonRightSide.Checked)
                    actionSide = 2;
                if (radioButtonBackSide.Checked)
                    actionSide = 3;
                if (radioButtonLeftSide.Checked)
                    actionSide = 4;
                if (radioButtonUpSide.Checked)
                    actionSide = 5;
                if (radioButtonDownSide.Checked)
                    actionSide = 6;

                switch (actionSide)
                {
                    case 0: return;
                    case 1:
                        textBoxX.Text = "0";
                        textBoxY.Text = "0";
                        float w = ((float)selPrimitive.MasLinesFront[1].X - (float)selPrimitive.MasLinesFront[0].X) / (float)selTextureSize.Width;
                        textBoxScaleX.Text = w.ToString();
                        float h = ((float)selPrimitive.MasLinesFront[3].Y - (float)selPrimitive.MasLinesFront[0].Y) / (float)selTextureSize.Height;
                        textBoxScaleY.Text = h.ToString();
                        break;
                    case 2:
                        textBoxX.Text = "0";
                        textBoxY.Text = "0";
                        w = ((float)selPrimitive.MasLinesRight[1].X - (float)selPrimitive.MasLinesRight[0].X) / (float)selTextureSize.Width;
                        textBoxScaleX.Text = w.ToString();
                        h = ((float)selPrimitive.MasLinesRight[3].Y - (float)selPrimitive.MasLinesRight[0].Y) / (float)selTextureSize.Height;
                        textBoxScaleY.Text = h.ToString();
                        break;
                    case 3:
                        textBoxX.Text = "0";
                        textBoxY.Text = "0";
                        w = ((float)selPrimitive.MasLinesFront[6].X - (float)selPrimitive.MasLinesFront[5].X) / (float)selTextureSize.Width;
                        textBoxScaleX.Text = w.ToString();
                        h = ((float)selPrimitive.MasLinesFront[8].Y - (float)selPrimitive.MasLinesFront[5].Y) / (float)selTextureSize.Height;
                        textBoxScaleY.Text = h.ToString();
                        break;
                    case 4:
                        textBoxX.Text = "0";
                        textBoxY.Text = "0";
                        w = ((float)selPrimitive.MasLinesRight[6].X - (float)selPrimitive.MasLinesRight[5].X) / (float)selTextureSize.Width;
                        textBoxScaleX.Text = w.ToString();
                        h = ((float)selPrimitive.MasLinesRight[8].Y - (float)selPrimitive.MasLinesRight[5].Y) / (float)selTextureSize.Height;
                        textBoxScaleY.Text = h.ToString();
                        break;
                    case 5:
                        textBoxX.Text = "0";
                        textBoxY.Text = "0";
                        w = ((float)selPrimitive.MasLinesTop[1].X - (float)selPrimitive.MasLinesTop[0].X) / (float)selTextureSize.Width;
                        textBoxScaleX.Text = w.ToString();
                        h = ((float)selPrimitive.MasLinesTop[3].Y - (float)selPrimitive.MasLinesTop[0].Y) / (float)selTextureSize.Height;
                        textBoxScaleY.Text = h.ToString();
                        break;
                    case 6:
                        textBoxX.Text = "0";
                        textBoxY.Text = "0";
                        w = ((float)selPrimitive.MasLinesTop[6].X - (float)selPrimitive.MasLinesTop[5].X) / (float)selTextureSize.Width;
                        textBoxScaleX.Text = w.ToString();
                        h = ((float)selPrimitive.MasLinesTop[8].Y - (float)selPrimitive.MasLinesTop[5].Y) / (float)selTextureSize.Height;
                        textBoxScaleY.Text = h.ToString();
                        break;
                }
            }
            catch
            {
                return;
            }
        }

        private void buttonXMinus_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (comboBoxModeIncrement.SelectedIndex != -1)
                {
                    float t = float.Parse(textBoxX.Text);
                    float c = float.Parse(comboBoxModeIncrement.Items[comboBoxModeIncrement.SelectedIndex].ToString());
                    t -= c;
                    textBoxX.Text = t.ToString();

                    float tx;
                    float.TryParse(textBoxScaleX.Text, out tx);
                    tx -= c;
                    textBoxScaleX.Text = tx.ToString();

                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void buttonXPlus_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (comboBoxModeIncrement.SelectedIndex != -1)
                {
                    float t = float.Parse(textBoxX.Text);
                    float c = float.Parse(comboBoxModeIncrement.Items[comboBoxModeIncrement.SelectedIndex].ToString());
                    t += c;
                    textBoxX.Text = t.ToString();

                    float tx;
                    float.TryParse(textBoxScaleX.Text, out tx);
                    tx += c;
                    textBoxScaleX.Text = tx.ToString();

                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void buttonYMinus_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (comboBoxModeIncrement.SelectedIndex != -1)
                {
                    float t = float.Parse(textBoxY.Text);
                    float c = float.Parse(comboBoxModeIncrement.Items[comboBoxModeIncrement.SelectedIndex].ToString());
                    t -= c;
                    textBoxY.Text = t.ToString();

                    float ty;
                    float.TryParse(textBoxScaleY.Text, out ty);
                    ty -= c;
                    textBoxScaleY.Text = ty.ToString();

                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void buttonYPlus_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (comboBoxModeIncrement.SelectedIndex != -1)
                {
                    float t = float.Parse(textBoxY.Text);
                    float c = float.Parse(comboBoxModeIncrement.Items[comboBoxModeIncrement.SelectedIndex].ToString());
                    t += c;
                    textBoxY.Text = t.ToString();

                    float ty;
                    float.TryParse(textBoxScaleY.Text, out ty);
                    ty += c;
                    textBoxScaleY.Text = ty.ToString();

                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void buttonRightDownXMinus_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (comboBoxModeIncrement.SelectedIndex != -1)
                {
                    float t = float.Parse(textBoxScaleX.Text);
                    float c = float.Parse(comboBoxModeIncrement.Items[comboBoxModeIncrement.SelectedIndex].ToString());
                    t -= c;
                    textBoxScaleX.Text = t.ToString();

                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void buttonRightDownXPlus_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (comboBoxModeIncrement.SelectedIndex != -1)
                {
                    float t = float.Parse(textBoxScaleX.Text);
                    float c = float.Parse(comboBoxModeIncrement.Items[comboBoxModeIncrement.SelectedIndex].ToString());
                    t += c;
                    textBoxScaleX.Text = t.ToString();

                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void buttonLeftDownYMinus_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (comboBoxModeIncrement.SelectedIndex != -1)
                {
                    float t = float.Parse(textBoxScaleY.Text);
                    float c = float.Parse(comboBoxModeIncrement.Items[comboBoxModeIncrement.SelectedIndex].ToString());
                    t -= c;
                    textBoxScaleY.Text = t.ToString();

                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void buttonLeftDownYPlus_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (comboBoxModeIncrement.SelectedIndex != -1)
                {
                    float t = float.Parse(textBoxScaleY.Text);
                    float c = float.Parse(comboBoxModeIncrement.Items[comboBoxModeIncrement.SelectedIndex].ToString());
                    t += c;
                    textBoxScaleY.Text = t.ToString();

                    TranslateSetiingsToManaged3D();
                }
            }
            catch
            {
                return;
            }
        }

        private void extendedImageList1_Load(object sender, EventArgs e)
        {

        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                int actionSide = 0;
                if (radioButtonAllSide.Checked)
                    actionSide = 0;
                if (radioButtonFrontSide.Checked)
                    actionSide = 1;
                if (radioButtonRightSide.Checked)
                    actionSide = 2;
                if (radioButtonBackSide.Checked)
                    actionSide = 3;
                if (radioButtonLeftSide.Checked)
                    actionSide = 4;
                if (radioButtonUpSide.Checked)
                    actionSide = 5;
                if (radioButtonDownSide.Checked)
                    actionSide = 6;

                if (Apply != null)
                {
                    Apply(nameTex, actionSide);
                    Form1.lastTexture = nameTex;
                }
            }
            catch
            {
                return;
            }
        }

        private void TranslateSetiingsToManaged3D()
        {
            try
            {
                int actionSide = 0;
                if (radioButtonAllSide.Checked)
                    actionSide = 0;
                if (radioButtonFrontSide.Checked)
                    actionSide = 1;
                if (radioButtonRightSide.Checked)
                    actionSide = 2;
                if (radioButtonBackSide.Checked)
                    actionSide = 3;
                if (radioButtonLeftSide.Checked)
                    actionSide = 4;
                if (radioButtonUpSide.Checked)
                    actionSide = 5;
                if (radioButtonDownSide.Checked)
                    actionSide = 6;

                if (ChangeTexturePosition != null)
                {
                    float x, y, sx, sy;

                    float.TryParse(textBoxX.Text, out x);
                    float.TryParse(textBoxY.Text, out y);
                    float.TryParse(textBoxScaleX.Text, out sx);
                    float.TryParse(textBoxScaleY.Text, out sy);

                    ChangeTexturePosition(x, y, sx, sy, actionSide, convertedPrimitive);
                }
            }
            catch
            {
                return;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Focus();
                if (checkBox1.Checked)
                {
                    if (HideSelection != null)
                        HideSelection(true);
                }
                else
                {
                    if (HideSelection != null)
                        HideSelection(false);

                }
            }
            catch
            {
                return;
            }
        }
        public void GenerateHideSelection()
        {
            try
            {
                checkBox1.Checked = false;

                if (HideSelection != null)
                    HideSelection(false);
            }
            catch
            {
                return;
            }
        }

        private void TexturesSettings_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Hide();
                }
                if (e.KeyCode == Keys.F5)
                {
                    textBoxX.Focus();
                    if (TextureFormKeyDown != null)
                    {
                        TextureFormKeyDown(sender, new KeyEventArgs(Keys.Q));
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private void radioButtonNone_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonMarks_Click(object sender, EventArgs e)
        {
            try
            {
                if (showMark)
                {
                    showMark = false;
                    this.Width = this.Width - richTextBox1.Width;
                    richTextBox1.Hide();
                    this.SetDesktopLocation(Screen.PrimaryScreen.WorkingArea.Right - this.Width, 25);
                }
                else
                {
                    showMark = true;
                    richTextBox1.Show();
                    this.Width = this.Width + richTextBox1.Width;
                    this.SetDesktopLocation(Screen.PrimaryScreen.WorkingArea.Right - this.Width, 25);
                }
            }
            catch
            {
                return;
            }
        }

        private void TexturesSettings_Deactivate(object sender, EventArgs e)
        {
            try
            {
                Environment.CurrentDirectory = Form1.startingPath;
                if (File.Exists("Marks.mk"))
                {
                    FileStream f = File.Open("Marks.mk", FileMode.Truncate);
                    StreamWriter s = new StreamWriter(f);

                    s.WriteLine(richTextBox1.Text);

                    s.Close();
                    f.Close();
                }
                else
                {
                    FileStream f = File.Open("Marks.mk", FileMode.Create);
                    StreamWriter s = new StreamWriter(f);

                    s.WriteLine(richTextBox1.Text);

                    s.Close();
                    f.Close();
                }
            }
            catch
            {
                return;
            }
        }

		private void comboBoxModeIncrement_TextChanged(object sender, EventArgs e)
		{
            try
            {
                float res;
                if (!float.TryParse(comboBoxModeIncrement.Text.ToString(), out res))
                {
                    if (comboBoxModeIncrement.SelectedIndex != -1)
                    {
                        comboBoxModeIncrement.Text = comboBoxModeIncrement.Items[comboBoxModeIncrement.SelectedIndex].ToString();
                    }
                    else
                    {
                        comboBoxModeIncrement.SelectedIndex = 2;
                        comboBoxModeIncrement.Text = comboBoxModeIncrement.Items[2].ToString();
                    }
                }
            }
            catch
            {
                return;
            }
		}
    }
}