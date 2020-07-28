using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Forms;

namespace Chitarik
{
    public class Category
    {
        string name;
        public string Name
        {
            get { return name; }
        }

        List<Slovo> slova;
        public List<Slovo> Slova
        {
            get { return slova; }
        }

        List<Slovo> slovaNonFiltered;
        public List<Slovo> SlovaNonFiltered
        {
            get { return slovaNonFiltered; }
            set { slovaNonFiltered = value; }
        }

        List<Slovo> slovaByRasskazy;
        public List<Slovo> SlovaByRasskazy
        {
            get { return slovaByRasskazy; }
            set { slovaByRasskazy = value; }
        }

        List<Slovo> slovaRandomed;
        public List<Slovo> SlovaRandomed
        {
            get { return slovaRandomed; }
        }

        List<string> predlogenia;
        public List<string> Predlogenia
        {
            get { return predlogenia; }
            set { predlogenia = value; }
        }

        List<string> predlogeniaByRasskazy;
        public List<string> PredlogeniaByRasskazy
        {
            get { return predlogeniaByRasskazy; }
            set { predlogeniaByRasskazy = value; }
        }

        List<string> predlogeniaRandomed;
        public List<string> PredlogeniaRandomed
        {
            get { return predlogeniaRandomed; }
        }

        List<MultiLineObject> stishki;
        public List<MultiLineObject> Stishki
        {
            get { return stishki; }
        }
        List<MultiLineObject> songs;
        public List<MultiLineObject> Songs
        {
            get { return songs; }
        }
        List<MultiLineObject> schitalki;
        public List<MultiLineObject> Schitalki
        {
            get { return schitalki; }
        }
        List<MultiLineObject> skorogovorki;
        public List<MultiLineObject> Skorogovorki
        {
            get { return skorogovorki; }
        }
        List<MultiLineObject> rasskazy;
        public List<MultiLineObject> Rasskazy
        {
            get { return rasskazy; }
        }

        bool isChanged = false;
        public bool IsChanged
        {
            get { return isChanged; }
        }

        public Category(string _name)
        {
            name = _name;
            slova = new List<Slovo>();
            slovaNonFiltered = new List<Slovo>();
            slovaRandomed = new List<Slovo>();
            predlogenia = new List<string>();
            ReadSlova();
            ReadMultiWordsContent(ReadMode.Predlogenie);
            ReadMultiWordsContent(ReadMode.Stishki);
            ReadMultiWordsContent(ReadMode.Skorogovorki);
            ReadMultiWordsContent(ReadMode.Schitalki);
            ReadMultiWordsContent(ReadMode.Rasskazy);
            ReadMultiWordsContent(ReadMode.Songs);

            predlogeniaByRasskazy = Lib.FillPredlogeniyaFromRasskazy(predlogenia, Rasskazy);
            slovaByRasskazy = Lib.FillSlovaFromPredlogeniaByRasskazy(slovaNonFiltered, predlogeniaByRasskazy, this);//Lib.FillSlovaFromRasskazy(slovaNonFiltered, Rasskazy);
        }

        public void SetChangedStatus()
        {
            isChanged = true;
        }

        int currentRenderedIndex = 0;

        public void ReadSlova()
        {
            string catFolder = Lib.LibFolderName + "\\" + name;

            string[] fl = Directory.GetFiles(catFolder, "Слова.txt");

            if (fl != null && fl.Length > 0 && File.Exists(fl[0]))
            {
                FileStream fs = File.Open(fl[0], FileMode.Open);
                StreamReader sr = new StreamReader(fs, Encoding.GetEncoding(1251));

                if (slova != null)
                    slova.Clear();
                if (slovaNonFiltered != null)
                    slovaNonFiltered.Clear();

                Category baseCat = this;

                while (!sr.EndOfStream)
                {
                    string ln = sr.ReadLine();

                    string[] lnv = ln.Split(new char[] { ' ' });
                    int accentIndex = -1;
                    if (lnv.Length > 1)
                    {
                        int.TryParse(lnv[1], out accentIndex);
                    }

                    if (Settings.BukvFilter >= lnv[0].Length)
                    {
                        Slovo sl = new Slovo(lnv[0], accentIndex, baseCat);
                        slova.Add(sl);
                    }

                    Slovo sl2 = new Slovo(lnv[0], accentIndex, baseCat);
                    slovaNonFiltered.Add(sl2);

                }

                sr.Close();
                fs.Close();

                FillRandomedList();
            }
        }

        void ReadMultiWordsContent(string fileName)
        {
            string catFolder = Lib.LibFolderName + "\\" + name;

            if (!File.Exists(catFolder + "\\" + fileName))
                return;

            string[] fl = Directory.GetFiles(catFolder, fileName);

            FileStream fs = File.Open(fl[0], FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding(1251));

            if (predlogenia != null)
                predlogenia.Clear();

            Category baseCat = this;

            while (!sr.EndOfStream)
            {
                string ln = sr.ReadLine();

                //string[] lnv = ln.Split(new char[] { '.', '!', '?', ';' });

                //if (lnv.Length > 0)
                //{
                //    for (int i = 0; i < lnv.Length; i++)
                //    {
                        //string s = lnv[i]/**/;
                        if (!String.IsNullOrEmpty(ln))
                        {
                            predlogenia.Add(ln);
                        }
                    //}
                //}

            }

            sr.Close();
            fs.Close();

            FillRandomedPredlogenia();
        }

        void ReadMultiWordsContent(ReadMode rm)
        {
            string fileName = "";
            switch (rm)
            {
                case ReadMode.Predlogenie:
                    fileName = "Предложения.txt";
                    ReadMultiWordsContent(fileName);
                    break;
                case ReadMode.Schitalki:
                    ReadMultiLineContent(MultiLineContentType.Schitalki);
                    break;
                case ReadMode.Stishki:
                    ReadMultiLineContent(MultiLineContentType.Stishki);
                    break;
                case ReadMode.Skorogovorki:
                    ReadMultiLineContent(MultiLineContentType.Skorogovorki);
                    break;
                case ReadMode.Songs:
                    ReadMultiLineContent(MultiLineContentType.Songs);
                    break;
                case ReadMode.Rasskazy:
                    ReadMultiLineContent(MultiLineContentType.Rasskazy);
                    break;
            }
        }

        void ReadMultiLineContent(MultiLineContentType contentType)
        {
            string dirName = "";
            bool hasSpliter = false;
            string fileName = "*.txt";
            switch (contentType)
            {
                case MultiLineContentType.Stishki:
                    dirName = "Стишки";
                    break;
                case MultiLineContentType.Songs:
                    dirName = "Песенки";
                    break;
                case MultiLineContentType.Rasskazy:
                    dirName = "Рассказы";
                    break;
                case MultiLineContentType.Skorogovorki:
                    hasSpliter = true;
                    fileName = "Скороговорки.txt";
                    break;
                case MultiLineContentType.Schitalki:
                    hasSpliter = true;
                    fileName = "Считалки.txt";
                    break;
            }

            ReadMultiLineTypedContent(fileName, dirName, hasSpliter, contentType);
        }

        void ReadMultiLineTypedContent(string fileName, string dirName, bool isHasSplitter, MultiLineContentType contentType)
        {
            List<MultiLineObject> mobList = new List<MultiLineObject>();
            string catFolder = Lib.LibFolderName + "\\" + name + "\\" + dirName;
            if (!isHasSplitter)
            {
                if (Directory.Exists(catFolder))
                {
                    string[] fl = Directory.GetFiles(catFolder, fileName);
                    if (fl != null && fl.Length > 0)
                    {

                        Hashtable ht = new Hashtable();
                        for (int i = 0; i < fl.Length; i++)
                        {
                            List<string> contentList = new List<string>();
                            string flName = fl[i];
                            if (File.Exists(flName))
                            {
                                ht.Add(flName, null);
                                FileStream fs = File.Open(flName, FileMode.Open);
                                StreamReader sr = new StreamReader(fs, Encoding.GetEncoding(1251));

                                while (!sr.EndOfStream)
                                {
                                    string ln = sr.ReadLine();
                                    contentList.Add(ln);
                                }

                                ht[flName] = contentList;

                                sr.Close();
                                fs.Close();
                            }
                        }

                        foreach (string key in ht.Keys)
                        {
                            List<string> contentList = (List<string>)ht[key];
                            MultiLineObject mob = new MultiLineObject(key, contentList, this);
                            mobList.Add(mob);
                        }
                    }
                }
            }
            else
            {
                if (Directory.Exists(catFolder))
                {
                    string[] fl = Directory.GetFiles(catFolder, fileName);
                    if (fl != null && fl.Length > 0)
                    {
                        Hashtable ht = new Hashtable();
                        for (int i = 0; i < fl.Length; i++)
                        {
                            List<string> contentList = new List<string>();
                            string flName = fl[i];
                            if (File.Exists(flName))
                            {
                                ht.Add(flName, null);
                                FileStream fs = File.Open(flName, FileMode.Open);
                                StreamReader sr = new StreamReader(fs, Encoding.GetEncoding(1251));

                                while (!sr.EndOfStream)
                                {
                                    string ln = sr.ReadLine();
                                    if (!String.IsNullOrEmpty(ln) && ln == "@")
                                    {
                                        MultiLineObject mob = new MultiLineObject(flName, contentList, this);
                                        mobList.Add(mob);
                                        contentList = new List<string>();
                                    }
                                    else
                                        contentList.Add(ln);
                                }

                                ht[flName] = contentList;

                                sr.Close();
                                fs.Close();
                            }
                        }

                        foreach (string key in ht.Keys)
                        {
                            List<string> contentList = (List<string>)ht[key];
                            MultiLineObject mob = new MultiLineObject(key, contentList, this);
                            mobList.Add(mob);
                        }
                    }
                }
            }

            switch (contentType)
            {
                case MultiLineContentType.Schitalki:
                    if (!Settings.MixElementsEnabled)
                        schitalki = mobList;
                    else
                    {
                        //надо перемешать считалки
                        schitalki = Category.FillRandomMobList(mobList);
                    }
                    break;
                case MultiLineContentType.Skorogovorki:
                    if (!Settings.MixElementsEnabled)
                        skorogovorki = mobList;
                    else
                    {
                        //надо перемешать скороговорки
                        skorogovorki = Category.FillRandomMobList(mobList);
                    }
                    break;
                case MultiLineContentType.Stishki:
                    stishki = mobList;
                    break;
                case MultiLineContentType.Songs:
                    songs = mobList;
                    break;
                case MultiLineContentType.Rasskazy:
                    rasskazy = mobList;
                    break;
            }
        }

        static List<MultiLineObject> FillRandomMobList(List<MultiLineObject> mobList)
        {
            List<MultiLineObject> rList = new List<MultiLineObject>();
            foreach (MultiLineObject mob in mobList)
            {
                rList.Add(mob);
            }

            Random r = new Random(7);
            for (int i = 0; i < rList.Count; i++)
            {
                int number = r.Next(0, rList.Count);
                MultiLineObject mob = rList[number];
                rList.RemoveAt(number);
                rList.Insert(i, mob);
            }

            return rList;
        }

        void FillRandomedList()
        {
            slovaRandomed = new List<Slovo>();
            foreach (Slovo sl in slovaNonFiltered)
            {
                slovaRandomed.Add(sl);
            }

            Random r = new Random(7);
            for (int i = 0; i < slovaRandomed.Count; i++)
            {
                int number = r.Next(0, slovaRandomed.Count);
                Slovo sl = slovaRandomed[number];
                slovaRandomed.RemoveAt(number);
                slovaRandomed.Insert(i, sl);
            }
        }

        void FillRandomedPredlogenia()
        {
            predlogeniaRandomed = new List<string>();
            foreach (string s in predlogenia)
            {
                predlogeniaRandomed.Add(s);
            }

            Random r = new Random(7);
            for (int i = 0; i < predlogeniaRandomed.Count; i++)
            {
                int num = r.Next(0, predlogeniaRandomed.Count);
                string s = predlogeniaRandomed[num];
                predlogeniaRandomed.RemoveAt(num);
                predlogeniaRandomed.Insert(i, s);
            }
        }

        public void SaveSlova()
        {
            string catFolder = Lib.LibFolderName + "\\" + name;
            string fileName = catFolder + "\\Слова.txt";
            if (File.Exists(fileName))
                File.Delete(fileName);
            FileStream fs = File.Open(fileName, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding(1251));
            foreach (Slovo sl in slovaNonFiltered)
            {
                string s = String.Format("{0}{1}{2}", sl.OriginalText, (sl.IsHasAccent && sl.AccentIndex >= 0 ? " " : ""), (sl.IsHasAccent && sl.AccentIndex >= 0 ? sl.AccentIndex.ToString() : ""));
                sw.WriteLine(s);
            }

            sw.Close();
            fs.Close();
        }

        bool IsHasSlovoInList(string slovoTxt, List<Slovo> list)
        {
            foreach (Slovo sl in list)
            {
                if (sl.OriginalText.ToLower() == slovoTxt.ToLower())
                    return true;
            }
            return false;
        }

        List<Slovo> GetAllChangedSlova()
        {
            List<Slovo> res = new List<Slovo>();

            foreach (Slovo sl in slova)
            {
                if (sl.IsChanged)
                    res.Add(sl);
            }

            return res;
        }

        public Slovo GetFirsSlovo()
        {
            if (slova != null && slova.Count > 0)
            {
                return slova[0];
            }
            return null;
        }

        public Slovo GetNextSlovo()
        {
            if (currentRenderedIndex + 1 >= slova.Count)
                currentRenderedIndex = 0;
            else
                currentRenderedIndex += 1;

            if (slova != null && slova.Count > 0 && slova.Count >= currentRenderedIndex)
                return slova[currentRenderedIndex];
            return null;
        }

        public Slovo GetPrevSlovo()
        {
            if (currentRenderedIndex - 1 < 0)
                currentRenderedIndex = slova.Count - 1;
            else
                currentRenderedIndex -= 1;

            if (slova != null && slova.Count > 0 && slova.Count >= currentRenderedIndex)
                return slova[currentRenderedIndex];
            return null;
        }

        public Slovo FindSlovoByOriginalText(string origText, bool isFromFilteredList)
        {
            List<Slovo> sl_list = slova;
            if (!isFromFilteredList)
                sl_list = slovaNonFiltered;
            if (slova != null && sl_list.Count > 0)
            {
                foreach (Slovo sl in sl_list)
                {
                    if (sl.OriginalText == origText)
                        return sl;
                }
            }
            return null;
        }
    }
}
