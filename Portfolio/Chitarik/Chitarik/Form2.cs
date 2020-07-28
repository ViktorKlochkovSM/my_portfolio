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
    public partial class Form2 : Form
    {
        Render myRender;
        public static Lib lib = new Lib();
        List<MultiLineObject> currentListMobs = null;

        public Form2()
        {
            InitializeComponent();

            SetCountLibObject(0, Color.Blue, SlovsFilteredByBukvCount_LBL);

            SetCountLibObject(0, Color.Green, FullSlogsCount_LBL);
            SetCountLibObject(0, Color.Green, SlogCountByType_LBL);
            SetCountLibObject(0, Color.Green, SlogCountByZvuk_LBL);
            SetCountLibObject(0, Color.Green, SlovsCount_LBL);
            SetCountLibObject(0, Color.Green, CountPredlogen_LBL);
            SetCountLibObject(0, Color.Green, FullCountSkorogovorki_LBL);
            SetCountLibObject(0, Color.Green, FullCountStishki_LBL);
            SetCountLibObject(0, Color.Green, FullSongsCount_LBL);
            SetCountLibObject(0, Color.Green, FullSchitalkiCount_LBL);
            SetCountLibObject(0, Color.Green, FullRasskazyCount_LBL);

            ColorizeSymbols_TSB.Checked = Settings.IsColorizerBukvEnabled;

            if (Settings.IsUpperRegister)
            {
                UpperRegister_TSB.Checked = true;
                LowerRegister_TSB.Checked = false;
            }
            else
            {
                UpperRegister_TSB.Checked = false;
                LowerRegister_TSB.Checked = true;
            }

            switch (Settings.TextHorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    AlignLeft_TSB.Checked = true;
                    AlignCenter_TSB.Checked = false;
                    AlignRight_TSB.Checked = false;
                    break;
                case HorizontalAlignment.Center:
                    AlignCenter_TSB.Checked = true;
                    AlignLeft_TSB.Checked = false;
                    AlignRight_TSB.Checked = false;
                    break;
                case HorizontalAlignment.Right:
                    AlignRight_TSB.Checked = true;
                    AlignLeft_TSB.Checked = false;
                    AlignCenter_TSB.Checked = false;
                    break;
            }

            Categories_CB.Items.Add("Все");
            foreach (Category cat in lib.Categories)
            {
                Categories_CB.Items.Add(cat.Name);
            }

            Settings.SlogFilter = null;
            Mix_CB.Checked = Settings.MixElementsEnabled;
            BukvFilter_NUD.Value = Settings.BukvFilter;

            switch (Settings.Read_Mode)
            {
                case ReadMode.Sloga:
                    tabControl2.SelectedIndex = 0;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = true;
                    break;
                case ReadMode.Slova:
                    tabControl2.SelectedIndex = 1;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = true;
                    break;
                case ReadMode.Predlogenie:
                    tabControl2.SelectedIndex = 2;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = true;
                    break;
                case ReadMode.Skorogovorki:
                    tabControl2.SelectedIndex = 3;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = true;
                    break;
                case ReadMode.Stishki:
                    tabControl2.SelectedIndex = 4;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = false;
                    break;
                case ReadMode.Songs:
                    tabControl2.SelectedIndex = 5;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = false;
                    break;
                case ReadMode.Schitalki:
                    tabControl2.SelectedIndex = 6;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = true;
                    break;
                case ReadMode.Rasskazy:
                    tabControl2.SelectedIndex = 7;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = false;
                    break;
            }

            RenderElementsCount_NUD.Value = Settings.ElementsCount;

            richTextBox1.BackColor = Settings.BackGroundColor;
            BukvFilter_NUD.Value = Settings.BukvFilter;

            SlogType_CB.Items.Add("Все");
            SlogType_CB.Items.Add((Settings.IsUpperRegister ? "АА" : "аа"));
            SlogType_CB.Items.Add((Settings.IsUpperRegister ? "АБ" : "аб"));
            SlogType_CB.Items.Add((Settings.IsUpperRegister ? "БА" : "ба"));

            switch (Settings.SlogCurrentType)
            {
                case SlogType.Slog2_Glas_Glas:
                    SlogType_CB.SelectedIndex = 1;
                    break;
                case SlogType.Slog2_glas_Soglas:
                    SlogType_CB.SelectedIndex = 2;
                    break;
                case SlogType.Slog2_Soglas_Glas:
                    SlogType_CB.SelectedIndex = 3;
                    break;
                case SlogType.Slogs_All:
                    SlogType_CB.SelectedIndex = 0;
                    break;
            }

            RenderElementsCount_NUD.Value = Settings.ElementsCount;
            BukvFilter_NUD.Value = Settings.BukvFilter;
        }

        private void SetingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Settings form_sett = new Form_Settings();
            DialogResult dr = form_sett.ShowDialog();
            if (dr == DialogResult.OK)
            {
                RenderNext();
            }
        }

        void CreateRender()
        {
            List<string> list = new List<string>();
            //тут надо заполнить список нужными элементами
            list = GetListElements();

            myRender = new Render(richTextBox1, list);
            myRender.ListMobs = currentListMobs;
            myRender.RenderCurrentPart(true);
        }

        List<string> GetListElements()
        {
            List<string> list = new List<string>();
            currentListMobs = new List<MultiLineObject>();
            Category cat = GetSelectedCategory();
            switch (Settings.Read_Mode)
            {
                case ReadMode.Slova:
                    //вывод слов
                    //заполнить список с нефильтрованными словами из выбранной категории
                    if (Categories_CB.SelectedIndex != 0)
                    {
                        if (cat != null)
                        {
                            if (!AddSlovaByRasskazy_CB.Checked)
                            {
                                if (Mix_CB.Checked)//если включена перемешка - выполним рандомное перемешивание
                                {
                                    Settings.MixElementsEnabled = Mix_CB.Checked;
                                    cat.SlovaNonFiltered = Lib.Randomize_ListOfSlovo(cat.SlovaNonFiltered);
                                    Mix_CB.Checked = false;
                                }
                                else
                                    Settings.MixElementsEnabled = Mix_CB.Checked;

                                SetCountLibObject(cat.SlovaNonFiltered.Count, Color.Green, SlovsCount_LBL);
                                foreach (Slovo sl in cat.SlovaNonFiltered)
                                {
                                    if (sl.OriginalText.Length <= Settings.BukvFilter)
                                        list.Add(sl.GetText());
                                }
                            }
                            else
                            {
                                if (Mix_CB.Checked)//если включена перемешка - выполним рандомное перемешивание
                                {
                                    Settings.MixElementsEnabled = Mix_CB.Checked;
                                    cat.SlovaByRasskazy = Lib.Randomize_ListOfSlovo(cat.SlovaByRasskazy);
                                    Mix_CB.Checked = false;
                                }
                                else
                                    Settings.MixElementsEnabled = Mix_CB.Checked;

                                SetCountLibObject(cat.SlovaByRasskazy.Count, Color.Green, SlovsCount_LBL);
                                foreach (Slovo sl in cat.SlovaByRasskazy)
                                {
                                    if (sl.OriginalText.Length <= Settings.BukvFilter)
                                        list.Add(sl.GetText());
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!AddSlovaByRasskazy_CB.Checked)
                        {
                            if (Mix_CB.Checked)//если включена перемешка - выполним рандомное перемешивание
                            {
                                Settings.MixElementsEnabled = Mix_CB.Checked;
                                lib.Slova = Lib.Randomize_ListOfSlovo(lib.Slova);
                                Mix_CB.Checked = false;
                            }
                            else
                                Settings.MixElementsEnabled = Mix_CB.Checked;

                            SetCountLibObject(lib.Slova.Count, Color.Green, SlovsCount_LBL);
                            //заполнить список словами из всех категорий
                            foreach (Slovo sl in lib.Slova)
                            {
                                if (sl.OriginalText.Length <= Settings.BukvFilter)
                                    list.Add(sl.GetText());
                            }
                        }
                        else
                        {
                            if (Mix_CB.Checked)//если включена перемешка - выполним рандомное перемешивание
                            {
                                Settings.MixElementsEnabled = Mix_CB.Checked;
                                lib.SlovaByRasskazy = Lib.Randomize_ListOfSlovo(lib.SlovaByRasskazy);
                                Mix_CB.Checked = false;
                            }
                            else
                                Settings.MixElementsEnabled = Mix_CB.Checked;

                            SetCountLibObject(lib.SlovaByRasskazy.Count, Color.Green, SlovsCount_LBL);
                            foreach (Slovo sl in lib.SlovaByRasskazy)
                            {
                                if (sl.OriginalText.Length <= Settings.BukvFilter)
                                    list.Add(sl.GetText());
                            }
                        }
                    }
                    SetCountLibObject(list.Count, Color.Blue, SlovsFilteredByBukvCount_LBL);
                    break;
                case ReadMode.Predlogenie:
                    //вывод предложений
                    if (Categories_CB.SelectedIndex != 0)
                    {
                        //заполнить список предложениями из выбранной категории
                        cat = GetSelectedCategory();
                        if (cat != null)
                        {
                            if (!AddPredlogenyaByRasskazy_CB.Checked)
                            {
                                if (Mix_CB.Checked)//если включена перемешка - выполним рандомное перемешивание
                                {
                                    Settings.MixElementsEnabled = Mix_CB.Checked;
                                    cat.Predlogenia = Lib.Randomize_ListOfStrings(cat.Predlogenia);
                                    Mix_CB.Checked = false;
                                }
                                else
                                    Settings.MixElementsEnabled = Mix_CB.Checked;

                                SetCountLibObject(cat.Predlogenia.Count, Color.Green, CountPredlogen_LBL);
                                foreach (string p in cat.Predlogenia)
                                {
                                    list.Add(p);
                                }
                            }
                            else
                            {
                                if (Mix_CB.Checked)//если включена перемешка - выполним рандомное перемешивание
                                {
                                    Settings.MixElementsEnabled = Mix_CB.Checked;
                                    cat.PredlogeniaByRasskazy = Lib.Randomize_ListOfStrings(cat.PredlogeniaByRasskazy);
                                    Mix_CB.Checked = false;
                                }
                                else
                                    Settings.MixElementsEnabled = Mix_CB.Checked;

                                SetCountLibObject(cat.PredlogeniaByRasskazy.Count, Color.Green, CountPredlogen_LBL);
                                foreach (string p in cat.PredlogeniaByRasskazy)
                                {
                                    list.Add(p);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!AddPredlogenyaByRasskazy_CB.Checked)
                        {
                            if (Mix_CB.Checked)//если включена перемешка - выполним рандомное перемешивание
                            {
                                Settings.MixElementsEnabled = Mix_CB.Checked;
                                lib.PredlogeniaFromAllCategories = Lib.Randomize_ListOfStrings(lib.PredlogeniaFromAllCategories);
                                Mix_CB.Checked = false;
                            }
                            else
                                Settings.MixElementsEnabled = Mix_CB.Checked;

                            SetCountLibObject(lib.PredlogeniaFromAllCategories.Count, Color.Green, CountPredlogen_LBL);
                            foreach (string p in lib.PredlogeniaFromAllCategories)
                            {
                                list.Add(p);
                            }
                        }
                        else
                        {
                            if (Mix_CB.Checked)//если включена перемешка - выполним рандомное перемешивание
                            {
                                Settings.MixElementsEnabled = Mix_CB.Checked;
                                lib.PredlogeniaFromAllCategoriesByRasskazy = Lib.Randomize_ListOfStrings(lib.PredlogeniaFromAllCategoriesByRasskazy);
                                Mix_CB.Checked = false;
                            }
                            else
                                Settings.MixElementsEnabled = Mix_CB.Checked;

                            SetCountLibObject(lib.PredlogeniaFromAllCategoriesByRasskazy.Count, Color.Green, CountPredlogen_LBL);
                            foreach (string p in lib.PredlogeniaFromAllCategoriesByRasskazy)
                            {
                                list.Add(p);
                            }
                        }
                    }
                    break;
                case ReadMode.Sloga:
                    //генерация и вывод слогов
                    int allSlogsCount = Slog.ListSlog_Glas_Glas.Count + Slog.ListSlog_Glas_Soglas.Count + Slog.ListSlog_Soglas_Glas.Count;
                    SetCountLibObject(allSlogsCount, Color.Green, FullSlogsCount_LBL);

                    switch (Settings.SlogCurrentType)
                    {
                        case SlogType.Slogs_All:
                            List<Slog> lstAll = Slog.ListAllSlogTypes;
                            SetCountLibObject(lstAll.Count, Color.Green, SlogCountByType_LBL);
                            list = Slog.ConverSlogListToStringList(lstAll);
                            break;
                        case SlogType.Slog2_Glas_Glas:
                            SetCountLibObject(Slog.ListSlog_Glas_Glas.Count, Color.Green, SlogCountByType_LBL);
                            list = Slog.ConverSlogListToStringList(Slog.ListSlog_Glas_Glas);
                            break;
                        case SlogType.Slog2_glas_Soglas:
                            SetCountLibObject(Slog.ListSlog_Glas_Soglas.Count, Color.Green, SlogCountByType_LBL);
                            list = Slog.ConverSlogListToStringList(Slog.ListSlog_Glas_Soglas);
                            break;
                        case SlogType.Slog2_Soglas_Glas:
                            SetCountLibObject(Slog.ListSlog_Soglas_Glas.Count, Color.Green, SlogCountByType_LBL);
                            list = Slog.ConverSlogListToStringList(Slog.ListSlog_Soglas_Glas);
                            break;
                    }

                    SetCountLibObject(list.Count, Color.Green, SlogCountByZvuk_LBL);
                    break;
                case ReadMode.Stishki:
                    if (StihNames_CB.SelectedIndex >= 0)
                    {
                        if (Categories_CB.SelectedIndex != 0)
                        {
                            if (cat != null)
                            {
                                MultiLineObject mob = Lib.FindMobInListByNameObj(StihNames_CB.SelectedItem.ToString(), cat.Stishki);
                                if (mob != null)
                                    list = mob.ContentList;
                            }
                        }
                        else
                        {
                            MultiLineObject mob = Lib.FindMobInListByNameObj(StihNames_CB.SelectedItem.ToString(), lib.StishkiFromAllCategories);
                            if (mob != null)
                                list = mob.ContentList;
                        }
                    }
                    break;
                case ReadMode.Skorogovorki:
                    if (Categories_CB.SelectedIndex != 0)
                    {
                        if (cat != null)
                        {
                            SetCountLibObject(cat.Skorogovorki.Count, Color.Green, FullCountSkorogovorki_LBL);
                            currentListMobs = cat.Skorogovorki;
                        }
                    }
                    else
                    {
                        SetCountLibObject(lib.SkorogovorkiFromAllCategories.Count, Color.Green, FullCountSkorogovorki_LBL);
                        currentListMobs = lib.SkorogovorkiFromAllCategories;
                    }
                    break;
                case ReadMode.Schitalki:
                    if (Categories_CB.SelectedIndex != 0)
                    {
                        if (cat != null)
                        {
                            SetCountLibObject(cat.Schitalki.Count, Color.Green, FullSchitalkiCount_LBL);
                            currentListMobs = cat.Schitalki;
                        }
                    }
                    else
                    {
                        SetCountLibObject(lib.SchitalkiFromAllCategories.Count, Color.Green, FullSchitalkiCount_LBL);
                        currentListMobs = lib.SchitalkiFromAllCategories;
                    }
                    break;
                case ReadMode.Rasskazy:
                    if (RasskazyNames_CB.SelectedIndex >= 0)
                    {
                        if (Categories_CB.SelectedIndex != 0)
                        {
                            if (cat != null)
                            {
                                MultiLineObject mob = Lib.FindMobInListByNameObj(RasskazyNames_CB.SelectedItem.ToString(), cat.Rasskazy);
                                if (mob != null)
                                    list = mob.ContentList;
                            }
                        }
                        else
                        {
                            MultiLineObject mob = Lib.FindMobInListByNameObj(RasskazyNames_CB.SelectedItem.ToString(), lib.RasskazyFromAllCategories);
                            if (mob != null)
                                list = mob.ContentList;
                        }
                    }
                    break;
                case ReadMode.Songs:
                    if (SongNames_CB.SelectedIndex >= 0)
                    {
                        if (Categories_CB.SelectedIndex != 0)
                        {
                            if (cat != null)
                            {
                                MultiLineObject mob = Lib.FindMobInListByNameObj(SongNames_CB.SelectedItem.ToString(), cat.Songs);
                                if (mob != null)
                                    list = mob.ContentList;
                            }
                        }
                        else
                        {
                            MultiLineObject mob = Lib.FindMobInListByNameObj(SongNames_CB.SelectedItem.ToString(), lib.SongsFromAllCategories);
                            if (mob != null)
                                list = mob.ContentList;
                        }
                    }
                    break;
            }

            if (Settings.Read_Mode != ReadMode.Rasskazy 
                && Settings.Read_Mode != ReadMode.Songs 
                && Settings.Read_Mode != ReadMode.Stishki
                && Settings.Read_Mode != ReadMode.Schitalki
                && Settings.Read_Mode != ReadMode.Skorogovorki)
            {
                //if (Mix_CB.Checked)//если включена перемешка - выполним рандомное перемешивание
                //{
                //    Settings.MixElementsEnabled = Mix_CB.Checked;
                //    Lib.Randomize_ListOfStrings(ref list);
                //    Mix_CB.Checked = false;
                //}
                //else
                //    Settings.MixElementsEnabled = Mix_CB.Checked;
            }
            return list;
        }

        Category GetSelectedCategory()
        {
            if (Categories_CB.SelectedItem != null)
                return Lib.FindCategory(Categories_CB.SelectedItem.ToString(), lib);
            return null;
        }

        void SetCountLibObject(int count, Color defaultColor, Label lbl)
        {
            lbl.Text = count.ToString();
            lbl.ForeColor = (count == 0 ? Color.Red : defaultColor);
        }

        void RenderPrev()
        {
            richTextBox1.Clear();
            if (myRender == null)
            {
                CreateRender();
                myRender.RenderCurrentPart(false);
            }
            else
            {
                myRender.ListElements = GetListElements();
                myRender.RenderPrevPart();
            }

            richTextBox1.Focus();
        }

        void RenderCurrent()
        {
            richTextBox1.Clear();
            if (myRender == null)
                CreateRender();
            else
            {
                myRender.ListElements = GetListElements();
            }
            myRender.RenderCurrentPart(true);

            richTextBox1.Focus();
        }

        void RenderNext()
        {
            richTextBox1.Clear();
            if (myRender == null)
            {
                CreateRender();
                myRender.RenderCurrentPart(true);
            }
            else
            {
                myRender.ListElements = GetListElements();
                myRender.RenderNextPart();
            }

            richTextBox1.Focus();
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl2.SelectedIndex)
            {
                case 0: Settings.Read_Mode = ReadMode.Sloga;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = true;
                    break;
                case 1: Settings.Read_Mode = ReadMode.Slova;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = true;
                    break;
                case 2: Settings.Read_Mode = ReadMode.Predlogenie;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = true;
                    break;
                case 3: Settings.Read_Mode = ReadMode.Skorogovorki;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = true;
                    break;
                case 4: Settings.Read_Mode = ReadMode.Stishki;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = false;
                    break;
                case 5: Settings.Read_Mode = ReadMode.Songs;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = false;
                    break;
                case 6: Settings.Read_Mode = ReadMode.Schitalki;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = true;
                    break;
                case 7: Settings.Read_Mode = ReadMode.Rasskazy;
                    Mix_CB.Enabled = groupBox3.Enabled = Forvard_BTN.Enabled = Back_BTN.Enabled = false;
                    break;
            }
            CreateRender();
            RenderCurrent();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            SerializeStatic.Save(typeof(Settings), Settings.FileNameSettings);
        }

        private void RenderElementsCount_NUD_ValueChanged(object sender, EventArgs e)
        {
            Settings.ElementsCount = (int)RenderElementsCount_NUD.Value;
            RenderCurrent();
        }

        private void SlogZvuk_CB_TextChanged(object sender, EventArgs e)
        {
            string s = SlogZvuk_CB.Text.Trim();
            if (s.Length == 1)
            {
                //найдем совпадения в списке доступных букв слогов
                bool found = false;
                List<char> glasList = SymbolInfo.GetBaseCollectionSymbols(true);
                for (int i = 0; i < glasList.Count; i++)
                {
                    if (s[0] == glasList[i])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    List<char> soglasList = SymbolInfo.GetBaseCollectionSymbols(false);
                    for (int y = 0; y < soglasList.Count; y++)
                    {
                        if (s[0] == soglasList[y])
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (found)
                {
                    Settings.SlogFilter = s[0];
                }
                else
                {
                    Settings.SlogFilter = null;
                    MessageBox.Show("Вводите только 1 символ буквы из русского алфавита");
                    SlogZvuk_CB.Text = "";
                }
            }
            else
            {
                if (s.Length > 1)
                {
                    MessageBox.Show("Вводите только 1 символ буквы из русского алфавита");
                }
                SlogZvuk_CB.Text = "";
                Settings.SlogFilter = null;
            }
            RenderCurrent();
        }

        private void SlogType_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (SlogType_CB.SelectedIndex)
            {
                case 1:
                    Settings.SlogCurrentType = SlogType.Slog2_Glas_Glas;
                    break;
                case 2:
                    Settings.SlogCurrentType = SlogType.Slog2_glas_Soglas;
                    break;
                case 3:
                    Settings.SlogCurrentType = SlogType.Slog2_Soglas_Glas;
                    break;
                case 0:
                    Settings.SlogCurrentType = SlogType.Slogs_All;
                    break;
            }
            SlogZvuk_CB.Text = "";
            RenderCurrent();
        }

        private void Back_BTN_Click(object sender, EventArgs e)
        {
            RenderPrev();
        }

        private void Forvard_BTN_Click(object sender, EventArgs e)
        {
            RenderNext();
        }

        private void Mix_CB_CheckedChanged(object sender, EventArgs e)
        {
            //Settings.MixElementsEnabled = Mix_CB.Checked;
            //RenderCurrent();
        }

        private void BukvFilter_NUD_ValueChanged(object sender, EventArgs e)
        {
            Settings.BukvFilter = (int)BukvFilter_NUD.Value;
            RenderCurrent();
        }

        void ResetNamesMultilineObjectsCB()
        {
            StihNames_CB.SelectedItem = null;
            StihNames_CB.SelectedText = "";
            StihNames_CB.Items.Clear();

            SongNames_CB.SelectedItem = null;
            SongNames_CB.SelectedText = "";
            SongNames_CB.Items.Clear();

            RasskazyNames_CB.SelectedItem = null;
            RasskazyNames_CB.SelectedText = "";
            RasskazyNames_CB.Items.Clear();

            List<MultiLineObject> listMobStishki = null;
            List<MultiLineObject> listMobSongs = null;
            List<MultiLineObject> listMobRasskazy = null;

            Category cat = GetSelectedCategory();
            if (Categories_CB.SelectedIndex != 0)
            {
                if (cat != null)
                {
                    //Стишки
                    listMobStishki = cat.Stishki;

                    //Rasskazy
                    listMobRasskazy = cat.Rasskazy;

                    //Songs
                    listMobSongs = cat.Songs;
                }
            }
            else
            {
                //со всей бибилиотеки
                //Стишки
                listMobStishki = lib.StishkiFromAllCategories;

                //Rasskazy
                listMobRasskazy = lib.RasskazyFromAllCategories;

                //Songs
                listMobSongs = lib.SongsFromAllCategories;
            }

            if (listMobStishki != null)
            {
                SetCountLibObject(listMobStishki.Count, Color.Green, FullCountStishki_LBL);
                foreach (MultiLineObject mob in listMobStishki)
                {
                    StihNames_CB.Items.Add(mob.ObjectName);
                }
                if (listMobStishki.Count > 0)
                    StihNames_CB.SelectedIndex = 0;
            }

            if (listMobSongs != null)
            {
                SetCountLibObject(listMobSongs.Count, Color.Green, FullSongsCount_LBL);
                foreach (MultiLineObject mob in listMobSongs)
                {
                    SongNames_CB.Items.Add(mob.ObjectName);
                }
                if (listMobSongs.Count > 0)
                    SongNames_CB.SelectedIndex = 0;
            }

            if (listMobRasskazy != null)
            {
                SetCountLibObject(listMobRasskazy.Count, Color.Green, FullRasskazyCount_LBL);
                foreach (MultiLineObject mob in listMobRasskazy)
                {
                    RasskazyNames_CB.Items.Add(mob.ObjectName);
                }
                if (listMobRasskazy.Count > 0)
                    RasskazyNames_CB.SelectedIndex = 0;
            }
        }

        private void Categories_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            myRender = null;

            ResetNamesMultilineObjectsCB();

            RenderCurrent();
        }

        private void StihNames_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            RenderCurrent();
        }

        private void SongNames_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            RenderCurrent();
        }

        private void RasskazyNames_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            RenderCurrent();
        }

        private void DecreaseLeftIndent_TSB_Click(object sender, EventArgs e)
        {
            if (Settings.CountLeftIndent > 0)
            {
                Settings.CountLeftIndent--;
                RenderCurrent();
            }
        }

        private void IncreaseLeftIndent_TSB_Click(object sender, EventArgs e)
        {
            Settings.CountLeftIndent++;
            RenderCurrent();
        }

        private void IncreaseUpperMargin_TSB_Click(object sender, EventArgs e)
        {
            Settings.UpperMargin++;
            RenderCurrent();
        }

        private void DecreaseUpperMargin_TSB_Click(object sender, EventArgs e)
        {
            if (Settings.UpperMargin > 0)
            {
                Settings.UpperMargin--;
                RenderCurrent();
            }
        }

        private void IncreaseBetweenLineMargin_TSB_Click(object sender, EventArgs e)
        {
            Settings.BeforeLineMargin++;
            RenderCurrent();
        }

        private void DecreaseBetweenLineMargin_TSB_Click(object sender, EventArgs e)
        {
            if (Settings.BeforeLineMargin > 0)
            {
                Settings.BeforeLineMargin--;
                RenderCurrent();
            }
        }

        private void AlignLeft_TSB_Click(object sender, EventArgs e)
        {
            CheckHorizontalAlign(0);
            RenderCurrent();
        }

        private void AlignCenter_TSB_Click(object sender, EventArgs e)
        {
            CheckHorizontalAlign(1);
            RenderCurrent();
        }

        private void AlignRight_TSB_Click(object sender, EventArgs e)
        {
            CheckHorizontalAlign(2);
            RenderCurrent();
        }

        void CheckHorizontalAlign(int curChecked)
        {
            switch (curChecked)
            {
                case 0: Settings.TextHorizontalAlignment = HorizontalAlignment.Left;
                    AlignLeft_TSB.Checked = true;
                    AlignCenter_TSB.Checked = false;
                    AlignRight_TSB.Checked = false;
                    break;
                case 1: Settings.TextHorizontalAlignment = HorizontalAlignment.Center;
                    AlignCenter_TSB.Checked = true;
                    AlignLeft_TSB.Checked = false;
                    AlignRight_TSB.Checked = false;
                    break;
                case 2: Settings.TextHorizontalAlignment = HorizontalAlignment.Right;
                    AlignRight_TSB.Checked = true;
                    AlignLeft_TSB.Checked = false;
                    AlignCenter_TSB.Checked = false;
                    break;
            }
        }

        private void UpperRegister_TSB_Click(object sender, EventArgs e)
        {
            CheckRegister(true);
            RenderCurrent();
        }

        private void LowerRegister_TSB_Click(object sender, EventArgs e)
        {
            CheckRegister(false);
            RenderCurrent();
        }

        void CheckRegister(bool isUpper)
        {
            if (isUpper)
            {
                Settings.IsUpperRegister = UpperRegister_TSB.Checked = true;
                LowerRegister_TSB.Checked = false;
            }
            else
            {
                Settings.IsUpperRegister = UpperRegister_TSB.Checked = false;
                LowerRegister_TSB.Checked = true;
            }
        }

        private void ColorGlass_TSB_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            colorDialog1.Color = Settings.GlasColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Settings.GlasColor = colorDialog1.Color;
                RenderCurrent();
            }
        }

        private void ColorSoglass_TSB_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            colorDialog1.Color = Settings.SoglasColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Settings.SoglasColor = colorDialog1.Color;
                RenderCurrent();
            }
        }

        private void Font_TSB_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog1 = new FontDialog();
            fontDialog1.ShowColor = true;
            fontDialog1.Font = Settings.FontGlobal;
            fontDialog1.Color = Settings.DefaultForeColor;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                Settings.FontGlobal = fontDialog1.Font;
                Settings.DefaultForeColor = fontDialog1.Color;
                RenderCurrent();
            }
        }

        private void BackGroundColor_TSB_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            colorDialog1.Color = Settings.BackGroundColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Settings.BackGroundColor = colorDialog1.Color;
                RenderCurrent();
            }
        }

        private void ColorizeSymbols_TSB_Click(object sender, EventArgs e)
        {
            Settings.IsColorizerBukvEnabled = ColorizeSymbols_TSB.Checked;
            RenderCurrent();
        }

        private void AddPredlogenyaByRasskazy_CB_CheckedChanged(object sender, EventArgs e)
        {
            RenderCurrent();
        }

        private void AddSlovaByRasskazy_CB_CheckedChanged(object sender, EventArgs e)
        {
            RenderCurrent();
        }
    }
}
