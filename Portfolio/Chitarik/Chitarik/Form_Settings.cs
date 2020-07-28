using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chitarik
{
    public partial class Form_Settings : Form
    {
        bool isApplied = false;
        bool isHasChanged = false;

        bool isShowAccent;

        public Form_Settings()
        {
            InitializeComponent();

            isShowAccent = ShowAccent_CB.Checked = Settings.ShowAccent;

            CountSlov_LBL.Text = "0";
            CountCategories_LBL.Text = Form2.lib.Categories.Count.ToString();

            foreach (Category cat in Form2.lib.Categories)
            {
                Categories_LB.Items.Add(cat.Name);
            }
        }

        private void Apply_BTN_Click(object sender, EventArgs e)
        {
            Settings.ShowAccent = ShowAccent_CB.Checked;

            SerializeStatic.Save(typeof(Settings), Settings.FileNameSettings);

            foreach (Category cat in Form2.lib.Categories)
            {
                if (cat.IsChanged)
                    cat.SaveSlova();
            }

            Form2.lib = new Lib();

            isApplied = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel_BTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ShowAccent_CB_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowAccent_CB.Checked)
                accent_UC1.Visible = true;
            else
                accent_UC1.Visible = false;

            if(Settings.ShowAccent != ShowAccent_CB.Checked)
                SetChangedStatus();
            Settings.ShowAccent = ShowAccent_CB.Checked;
        }

        private void Categories_LB_SelectedIndexChanged(object sender, EventArgs e)
        {
            accent_UC1.Reset();
            Slova_LB.Items.Clear();
            CountSlov_LBL.Text = "0";

            //найдем выбранную категорию
            Category cat = Lib.FindCategory(Categories_LB.SelectedItem.ToString(), Form2.lib);
            if (cat != null)
            {
                CountSlov_LBL.Text = cat.SlovaNonFiltered.Count.ToString();
                //заполним Slova_LB словами из выбранной категории
                foreach (Slovo sl in cat.SlovaNonFiltered)
                {
                    if (sl.IsHasAccent && !String.IsNullOrEmpty(sl.TextWithAccent))
                        Slova_LB.Items.Add(sl.TextWithAccent);
                    else
                        Slova_LB.Items.Add(sl.OriginalText);
                }
            }
        }

        private void Slova_LB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //найдем выбранную категорию
            Category cat = Lib.FindCategory(Categories_LB.SelectedItem.ToString(), Form2.lib);
            if (cat != null)
            {
                Slovo sl = cat.FindSlovoByOriginalText(Slova_LB.SelectedItem.ToString().Replace(Settings.AccentStr, ""), false);
                if(sl != null)
                    accent_UC1.SetSlovo(sl);
            }
        }

        private void Form_Settings_Load(object sender, EventArgs e)
        {

        }

        void SetChangedStatus()
        {
            isHasChanged = true;
        }

        private void Form_Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isApplied && isHasChanged)
            {
                if (MessageBox.Show("Все несохраненные изменения будут утеряны.\r\nПродолжить?", "Внимание!!!", MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
                else
                {
                    Settings.ShowAccent = isShowAccent;
                }
            }
        }
    }
}
