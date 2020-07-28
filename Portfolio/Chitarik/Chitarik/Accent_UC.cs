using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chitarik
{
    public delegate void OnAccentIndexChanged(int newAccentIndex);
    public partial class Accent_UC : UserControl
    {
        public event OnAccentIndexChanged onAccentIndexChanged;

        Slovo curSlovo;
        public Accent_UC()
        {
            InitializeComponent();
            onAccentIndexChanged += new OnAccentIndexChanged(Accent_UC_onAccentIndexChanged);
        }

        public void Reset()
        {
            panel1.Controls.Clear();
            MyCheckBoxEx.checkBoxesList.Clear();
            curSlovo = null;
            SlovoView_LBL.Text = "";
        }

        void Accent_UC_onAccentIndexChanged(int newAccentIndex)
        {
            if (curSlovo != null)
            {
                curSlovo.SetAccentIndex(newAccentIndex);
                SetSlovo(curSlovo);

                foreach (Category cat in Form2.lib.Categories)
                {
                    //ищем категорию в библиоетеке
                    if (cat.Name == curSlovo.BaseCategory.Name)
                    {
                        //ищем слово в категории
                        foreach (Slovo sl in cat.SlovaNonFiltered)
                        {
                            if (sl.OriginalText == curSlovo.OriginalText)
                            {
                                sl.SetAccentIndex(newAccentIndex);
                            }
                        }
                    }
                }
            }
        }

        public void SetSlovo(Slovo slovo)
        {
            panel1.Controls.Clear();
            MyCheckBoxEx.checkBoxesList.Clear();

            curSlovo = slovo;

            if (slovo.IsHasAccent && !String.IsNullOrEmpty(slovo.TextWithAccent))
                SlovoView_LBL.Text = slovo.TextWithAccent;
            else
                SlovoView_LBL.Text = slovo.OriginalText;

            SlovoView_LBL.ForeColor = Color.Blue;

            int slovoLength = curSlovo.OriginalText.Length;

            int x_loc_start = 10;
            int y_loc_start = 10;

            int btnWidth = 40;
            int btnHeight = 50;

            for (int i = 0; i < slovoLength; i++)
            {
                if (i > 0)
                    x_loc_start += btnWidth + 5;
                bool is_checked = false;
                if (slovo.IsHasAccent && slovo.AccentIndex >= 0 && slovo.AccentIndex == i)
                    is_checked = true;
                MyCheckBoxEx.countCheckBtns++;

                Bukva bk = curSlovo.FindBukvuByIndex(i);
                MyCheckBoxEx mcb = new MyCheckBoxEx(i, bk, curSlovo.OriginalText[i].ToString(), is_checked);
                mcb.Appearance = Appearance.Button;
                mcb.Font = new Font("Arial", 24);
                mcb.CheckedChanged += new EventHandler(mcb_CheckedChanged);
                mcb.Size = new Size(btnWidth, btnHeight);
                mcb.Location = new Point(x_loc_start, y_loc_start);

                MyCheckBoxEx.checkBoxesList.Add(mcb);
            }

            for (int i = 0; i < MyCheckBoxEx.checkBoxesList.Count; i++)
            {
                panel1.Controls.Add(MyCheckBoxEx.checkBoxesList[i]);
            }
        }

        void mcb_CheckedChanged(object sender, EventArgs e)
        {
            int index = ((MyCheckBoxEx)sender).Index;
            if (!((MyCheckBoxEx)sender).Checked)
                index = -1;
            if (onAccentIndexChanged != null)
                onAccentIndexChanged(index);
        }

        private void Accent_UC_Load(object sender, EventArgs e)
        {

        }
    }
}
