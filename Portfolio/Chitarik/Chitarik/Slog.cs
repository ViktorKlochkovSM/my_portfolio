using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chitarik
{
    public enum SlogType {Slog2_Glas_Glas = 0, Slog2_glas_Soglas = 1, Slog2_Soglas_Glas = 2, Slogs_All = 3 };
    public class Slog
    {
        static List<Slog> listSlog_Glas_Glas = GenerateSlogs(true, true);
        public static List<Slog> ListSlog_Glas_Glas
        {
            get { return listSlog_Glas_Glas; }
        }
        static List<Slog> listSlog_Glas_Soglas = GenerateSlogs(true, false);
        public static List<Slog> ListSlog_Glas_Soglas
        {
            get { return listSlog_Glas_Soglas; }
        }
        static List<Slog> listSlog_Soglas_Glas = GenerateSlogs(false, true);
        public static List<Slog> ListSlog_Soglas_Glas
        {
            get { return listSlog_Soglas_Glas; }
        }

        public static List<Slog> ListAllSlogTypes
        {
            get 
            {
                List<Slog> list = new List<Slog>();

                ConcatLists(ListSlog_Glas_Glas, list);
                ConcatLists(ListSlog_Glas_Soglas, list);
                ConcatLists(listSlog_Soglas_Glas, list);

                return list;
            }
        }

        string mySlog;
        public string MySlog
        {
            get { return mySlog; }
        }

        public Slog(string slog)
        {
            mySlog = slog;
        }

        public static List<Slog> GenerateSlogs(bool isGlasFirst, bool IsGlasSecond)
        {
            List<Slog> slogsList = new List<Slog>();

            List<char> glassSymbols = SymbolInfo.GetBaseCollectionSymbols(true);
            List<char> soglassSymbols = SymbolInfo.GetBaseCollectionSymbols(false);

            List<char> firstList = (isGlasFirst ? glassSymbols : soglassSymbols);
            List<char> secondList = (isGlasFirst ? (IsGlasSecond ? glassSymbols : soglassSymbols) : glassSymbols);

            for (int i = 0; i < firstList.Count; i++)
            {
                char ic = firstList[i];
                for (int y = 0; y < secondList.Count; y++)
                {
                    char yc = secondList[y];
                    if (ic != yc)
                    {
                        if (ic == 'й')
                            continue;
                        if (yc == 'й' && !isGlasFirst)
                            continue;
                        Slog slg = new Slog(String.Format("{0}{1}", ic, yc));
                        slogsList.Add(slg);
                    }
                }
            }

            return slogsList;
        }

        public static List<string> ConverSlogListToStringList(List<Slog> slogList)
        {
            List<string> strList = new List<string>();

            for (int i = 0; i < slogList.Count; i++)
            {
                if (Settings.SlogFilter != null)
                {
                    if (slogList[i].MySlog.IndexOf((char)Settings.SlogFilter) != -1)
                        strList.Add(slogList[i].MySlog);
                }
                else
                    strList.Add(slogList[i].MySlog);
            }

            return strList;
        }

        public static List<Slog> ConcatLists(List<Slog> slogListSource, List<Slog> slogListDest)
        {
            List<Slog> slogList = new List<Slog>();

            for (int i = 0; i < slogListSource.Count; i++)
            {
                Slog slg = slogListSource[i];
                slogListDest.Add(slg);
            }

            return slogList;
        }
    }
}
