using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Chitarik
{
    public class Lib
    {
        List<Category> categories;
        public List<Category> Categories
        {
            get { return categories; }
        }

        public static string LibFolderName = "Lib";

        List<Slovo> slova;
        public List<Slovo> Slova
        {
            get { return slova; }
            set { slova = value; }
        }

        List<Slovo> slovaByRasskazy;
        public List<Slovo> SlovaByRasskazy
        {
            get { return slovaByRasskazy; }
            set { slovaByRasskazy = value; }
        }

        List<string> predlogeniaFromAllCategories;
        public List<string> PredlogeniaFromAllCategories
        {
            get { return predlogeniaFromAllCategories; }
            set { predlogeniaFromAllCategories = value; }
        }

        List<string> predlogeniaFromAllCategoriesByRasskazy;
        public List<string> PredlogeniaFromAllCategoriesByRasskazy
        {
            get { return predlogeniaFromAllCategoriesByRasskazy; }
            set { predlogeniaFromAllCategoriesByRasskazy = value; }
        }

        List<MultiLineObject> songsFromAllCategories;
        public List<MultiLineObject> SongsFromAllCategories
        {
            get { return songsFromAllCategories; }
        }

        List<MultiLineObject> stishkiFromAllCategories;
        public List<MultiLineObject> StishkiFromAllCategories
        {
            get { return stishkiFromAllCategories; }
        }

        List<MultiLineObject> skorogovorkiFromAllCategories;
        public List<MultiLineObject> SkorogovorkiFromAllCategories
        {
            get { return skorogovorkiFromAllCategories; }
        }

        List<MultiLineObject> rasskazyFromAllCategories;
        public List<MultiLineObject> RasskazyFromAllCategories
        {
            get { return rasskazyFromAllCategories; }
        }

        List<MultiLineObject> schitalkiFromAllCategories;
        public List<MultiLineObject> SchitalkiFromAllCategories
        {
            get { return schitalkiFromAllCategories; }
        }

        public Lib()
        {
            categories = new List<Category>();

            slovaByRasskazy = new List<Slovo>();

            if (!Directory.Exists(Lib.LibFolderName))
                Directory.CreateDirectory(Lib.LibFolderName);

            ReadCategories();
        }

        void ReadCategories()
        {
            string[] categoryNames = Directory.GetDirectories(Lib.LibFolderName);

            foreach (string categoryName in categoryNames)
            {
                int lastInd = categoryName.LastIndexOf("\\");
                string nm = categoryName.Substring(lastInd + 1, (categoryName.Length - lastInd) - 1);
                Category category = new Category(nm);

                categories.Add(category);
            }

            FillAllSlovaListByAllCategories();
            FillPredlogeniaFromAllCategories();
            FillSchitalkiFromAllCategories();
            FillStishkiFromAllCategories();
            FillSongsFromAllCategories();
            FillSkorogovorkiFromAllCategories();
            FillRasskazyFromAllCategories();

            predlogeniaFromAllCategoriesByRasskazy = Lib.FillPredlogeniyaFromRasskazy(predlogeniaFromAllCategories, rasskazyFromAllCategories);
            slovaByRasskazy = Lib.FillSlovaFromPredlogeniaByRasskazy(slova, predlogeniaFromAllCategoriesByRasskazy, null);//Lib.FillSlovaFromRasskazy(slova, rasskazyFromAllCategories);
        }

        void FillAllSlovaListByAllCategories()
        {
            slova = new List<Slovo>();
            foreach (Category cat in categories)
            {
                foreach (Slovo sl in cat.SlovaNonFiltered)
                {
                    slova.Add(sl);
                }
            }
        }

        void FillPredlogeniaFromAllCategories()
        {
            predlogeniaFromAllCategories = new List<string>();
            foreach (Category cat in categories)
            {
                foreach (string pr in cat.Predlogenia)
                {
                    predlogeniaFromAllCategories.Add(pr);
                }
            }
        }

        void FillSchitalkiFromAllCategories()
        {
            schitalkiFromAllCategories = new List<MultiLineObject>();
            foreach (Category cat in categories)
            {
                foreach (MultiLineObject mob in cat.Schitalki)
                {
                    schitalkiFromAllCategories.Add(mob);
                }
            }
        }

        void FillStishkiFromAllCategories()
        {
            stishkiFromAllCategories = new List<MultiLineObject>();
            foreach (Category cat in categories)
            {
                foreach (MultiLineObject mob in cat.Stishki)
                {
                    stishkiFromAllCategories.Add(mob);
                }
            }
        }

        void FillSongsFromAllCategories()
        {
            songsFromAllCategories = new List<MultiLineObject>();
            foreach (Category cat in categories)
            {
                foreach (MultiLineObject mob in cat.Songs)
                {
                    songsFromAllCategories.Add(mob);
                }
            }
        }

        void FillSkorogovorkiFromAllCategories()
        {
            skorogovorkiFromAllCategories = new List<MultiLineObject>();
            foreach (Category cat in categories)
            {
                foreach (MultiLineObject mob in cat.Skorogovorki)
                {
                    skorogovorkiFromAllCategories.Add(mob);
                }
            }
        }

        void FillRasskazyFromAllCategories()
        {
            rasskazyFromAllCategories = new List<MultiLineObject>();
            foreach (Category cat in categories)
            {
                foreach (MultiLineObject mob in cat.Rasskazy)
                {
                    rasskazyFromAllCategories.Add(mob);
                }
            }
        }

        public static List<string> FillPredlogeniyaFromRasskazy(List<string> predlogeniyaList, List<MultiLineObject> mobListRasskazy)
        {
            List<string> listRes = new List<string>();
            ArrayList al = new ArrayList();

            if (mobListRasskazy != null && predlogeniyaList != null)
            {
                Category curCat = null;
                if (mobListRasskazy.Count > 0)
                    curCat = mobListRasskazy[0].BaseCategory;
                foreach (string p in predlogeniyaList)
                {
                    if (!String.IsNullOrEmpty(p) && (Lib.IsStringCorrect(p)))
                    {
                        TextObjectByCategory tob = new TextObjectByCategory(p, curCat);
                        if (!String.IsNullOrEmpty(p) && !al.Contains(tob) && !predlogeniyaList.Contains(p))
                        {
                            al.Add(tob);
                        }
                    }
                }

                foreach (MultiLineObject mob in mobListRasskazy)
                {
                    Category curCategory = mob.BaseCategory;
                    //тут получаем предложения
                    foreach (string s_orig in mob.ContentList)
                    {
                        string s = Lib.CorrectPredlogenie(s_orig);
                        if (!String.IsNullOrEmpty(s) && (Lib.IsStringCorrect(s)))
                        {
                            TextObjectByCategory tob = new TextObjectByCategory(s, curCategory);
                            if (!String.IsNullOrEmpty(s) && s.Length > 10 && !al.Contains(tob) && !predlogeniyaList.Contains(s))
                            {
                                al.Add(tob);
                            }
                        }
                    }
                }

                if (al != null && al.Count > 0)
                {
                    foreach (TextObjectByCategory tob in al)
                    {
                        string s = tob.Text;
                        listRes.Add(tob.Text);
                    }
                }
            }

            return listRes;
        }

        public static List<Slovo> FillSlovaFromRasskazy(List<Slovo> listSlov, List<MultiLineObject> mobList)
        {
            List<Slovo> slovaByRasskazy = new List<Slovo>();
            ArrayList al = new ArrayList();

            if (mobList != null && listSlov != null)
            {
                Category curCat = null;
                if (mobList.Count > 0)
                    curCat = mobList[0].BaseCategory;
                foreach (Slovo slv in listSlov)
                {
                    TextObjectByCategory tob = new TextObjectByCategory(slv.GetText(), curCat);
                    if (!al.Contains(tob))
                    {
                        al.Add(tob);
                    }
                }

                foreach (MultiLineObject mob in mobList)
                {
                    Category curCategory = mob.BaseCategory;
                    foreach (string s_orig in mob.ContentList)
                    {
                        //тут получаем предложение
                        string[] s = s_orig.Split(new char[] { ',', '.', ';', '!', '?', '"', '(', ')', '\r', '\n', ' ', '-', ':' });
                        if (s != null && s.Length > 0)
                        {
                            for (int is1 = 0; is1 < s.Length; is1++)
                            {
                                string sn = s[is1].Trim();
                                if (Lib.IsStringCorrect(sn))
                                {
                                    TextObjectByCategory tob = new TextObjectByCategory(sn, curCategory);
                                    Slovo nsl = new Slovo(sn, -1, curCategory);
                                    if (!String.IsNullOrEmpty(sn) && sn.Length > 2 && !al.Contains(tob) && !listSlov.Contains(nsl))
                                    {
                                        al.Add(tob);
                                    }
                                }
                            }
                        }
                    }
                }

                if (al != null && al.Count > 0)
                {
                    foreach (TextObjectByCategory tob in al)
                    {
                        Category curCat2 = tob.BaseCategory;
                        string s = tob.Text;
                        slovaByRasskazy.Add(new Slovo(s, -1, curCat2));
                    }
                }
            }
            return slovaByRasskazy;
        }

        public static List<Slovo> FillSlovaFromPredlogeniaByRasskazy(List<Slovo> listSlov, List<string> predlogenia, Category baseCategory)
        {
            List<Slovo> res = new List<Slovo>();

            ArrayList al = new ArrayList();

            if (predlogenia != null && listSlov != null)
            {
                foreach (string predl in predlogenia)
                {
                    if (!String.IsNullOrEmpty(predl) && Lib.IsStringCorrect(predl))
                    {
                        //тут получаем предложение
                        string[] s = predl.Split(new char[] { ',', '.', ';', '!', '?', '"', '(', ')', '\r', '\n', ' ', '-', ':' });
                        if (s != null && s.Length > 0)
                        {
                            for (int is1 = 0; is1 < s.Length; is1++)
                            {
                                string sn = s[is1].Trim();
                                if (Lib.IsStringCorrect(sn))
                                {
                                    TextObjectByCategory tob = new TextObjectByCategory(sn, baseCategory);
                                    Slovo nsl = new Slovo(sn, -1, baseCategory);
                                    if (!String.IsNullOrEmpty(sn) && sn.Length > 2 && !al.Contains(tob) && !listSlov.Contains(nsl))
                                    {
                                        al.Add(tob);
                                    }
                                }
                            }
                        }
                    }
                }

                if (al != null && al.Count > 0)
                {
                    foreach (TextObjectByCategory tob in al)
                    {
                        Category curCat = tob.BaseCategory;
                        string s = tob.Text;
                        res.Add(new Slovo(s, -1, curCat));
                    }
                }
            }

            return res;
        }

        public static List<Slovo> GetListByFilterBukv(int filter,List<Slovo> sourceList)
        {
            List<Slovo> lst = new List<Slovo>();
            for (int i = 0; i < sourceList.Count; i++)
            {
                Slovo sl = sourceList[i];
                if (sl.OriginalText.Length <= filter)
                    lst.Add(sl);
            }
            return lst;
        }

        public static Category FindCategory(string catName, Lib lib)
        {
            foreach (Category cat in lib.Categories)
            {
                if (cat.Name == catName)
                    return cat;
            }
            return null;
        }

        public static string CorrectPredlogenie(string predl)
        {
            string res = "";
            string[] s = predl.Split(new char[] {'\t', '\r', '\n' });
            if (s != null && s.Length > 0)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    s[i] = s[i].Replace("0", "")
                    .Replace("1", "")
                    .Replace("2", "")
                    .Replace("3", "")
                    .Replace("4", "")
                    .Replace("5", "")
                    .Replace("6", "")
                    .Replace("7", "")
                    .Replace("8", "")
                    .Replace("9", "")
                    .Replace(".", "")
                    .Replace(".", "")
                    .Replace("  ", " ")
                    .Replace("  ", " ")
                    .Replace("  ", " ")
                    .Replace("  ", " ")
                    .Replace("  ", " ")
                    .Replace("  ", " ")
                    .Replace("  ", " ")
                    .Replace("  ", " ")
                    .Replace("  ", " ")
                    .Replace("  ", " ")
                    ;

                    if (s[i].Contains("\0171") && !s[i].Contains("\0187") || !s[i].Contains("\0171") && s[i].Contains("\0187"))
                    {
                        s[i] = s[i].Replace("\0171", "")//открывающая русская двойная кавычка
                            .Replace("\0187", "")//закрывающая русская двойная кавычка
                            ;
                    }
                    if (!s[i].Contains("«") && s[i].Contains("»") || s[i].Contains("«") && !s[i].Contains("»"))
                    {
                        s[i] = s[i].Replace("«", "")
                            .Replace("»", "");
                    }

                    int indTire = -1;
                    indTire = s[i].IndexOf("—");
                    if (indTire == 0 || indTire == s[i].Length - 1)
                        s[i] = s[i].Replace("—", "");

                    indTire = -1;
                    indTire = s[i].IndexOf("-");
                    if (indTire == 0 || indTire == s[i].Length - 1)
                        s[i] = s[i].Replace("-", "");
                }

                for (int i = 0; i < s.Length; i++)
                {
                    string ss = s[i].Trim();
                    if (i > 0 && !String.IsNullOrEmpty(s[i - 1].Trim()))
                    {
                        res += " ";
                    }
                    if (!String.IsNullOrEmpty(ss))
                        res += ss;
                }
            }

            return res;
        }

        public static MultiLineObject FindMobInListByNameObj(string objName, List<MultiLineObject> listMob)
        {
            for (int i = 0; i < listMob.Count; i++)
            {
                if (listMob[i].ObjectName == objName)
                    return listMob[i];
            }
            return null;
        }

        public static void Randomize_ListOfMob(ref List<MultiLineObject> mobList)
        {
            List<MultiLineObject> randomedList = new List<MultiLineObject>();
            for (int p = 0; p < mobList.Count; p++)
            {
                randomedList.Add(mobList[p]);
            }

            Random rs = new Random();

            int seed = rs.Next(1, 255);

            Random r = new Random(seed);
            for (int i = 0; i < randomedList.Count; i++)
            {
                int number = r.Next(0, randomedList.Count);
                MultiLineObject sl = randomedList[number];
                randomedList.RemoveAt(number);
                randomedList.Insert(i, sl);
            }

            mobList = randomedList;
        }

        public static List<Slovo> Randomize_ListOfSlovo(List<Slovo> slovList)
        {
            List<Slovo> randomedList = new List<Slovo>();
            for (int p = 0; p < slovList.Count; p++)
            {
                randomedList.Add(slovList[p]);
            }

            Random rs = new Random();

            int seed = rs.Next(1, 255);

            Random r = new Random(seed);
            for (int i = 0; i < randomedList.Count; i++)
            {
                int number = r.Next(0, randomedList.Count);
                Slovo sl = randomedList[number];
                randomedList.RemoveAt(number);
                randomedList.Insert(i, sl);
            }

            return randomedList;
        }

        public static List<string> Randomize_ListOfStrings(List<string> list)
        {
            List<string> randomedList = new List<string>();
            for (int p = 0; p < list.Count; p++)
            {
                randomedList.Add(list[p]);
            }

            Random rs = new Random();

            int seed = rs.Next(1, 255);

            Random r = new Random(seed);
            for (int i = 0; i < randomedList.Count; i++)
            {
                int number = r.Next(0, randomedList.Count);
                string sl = randomedList[number];
                randomedList.RemoveAt(number);
                randomedList.Insert(i, sl);
            }

            list = randomedList;

            return randomedList;
        }

        public static bool IsStringCorrect(string s)
        {
            bool res = true;
            if (!String.IsNullOrEmpty(s))
            {
                for (int i = 0; i < s.Length; i++)
                {
                    char c = s[i];
                    switch (c)
                    {
                        case '!':
                        case '"':
                        case '№':
                        case ';':
                        case '%':
                        case ':':
                        case '?':
                        case ',':
                        case '.':
                        case '*':
                        case '(':
                        case ')':
                        case '-':
                        case '—': 
                        case '_':
                        case '=':
                        case '+':
                        case '/':
                        case '\\':
                        case '»':
                        case '«':
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9': res = false; break;
                    }

                    if (!res)
                        return res;
                }
            }

            return res;
        }
    }

    public class TextObjectByCategory
    {
        string txt;
        public string Text
        {
            get { return txt; }
        }

        Category baseCategory = null;
        public Category BaseCategory
        {
            get { return baseCategory; }
        }

        public TextObjectByCategory(string text, Category cat)
        {
            txt = text;
            baseCategory = cat;
        }
    }
}
