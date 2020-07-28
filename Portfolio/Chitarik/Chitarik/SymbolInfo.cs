using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Chitarik
{
    public class SymbolInfo
    {
        char symbol;
        public char Symbol
        {
            get { return (isAccent ? symbol : symbol); }
        }

        public string SymbolText
        {
            get { return (isAccent && Settings.ShowAccent ? GetSymbolStrRegistered() + Settings.AccentStr : GetSymbolStrRegistered()); }
        }

        bool isGlas;
        public bool IsGlas
        {
            get { return isGlas; }
        }

        bool isAccent;
        public bool IsAccent
        {
            get { return isAccent; }
        }

        public SymbolInfo(char _symbol, bool _isAccent)
        {
            symbol = _symbol;
            isAccent = _isAccent;
            CheckSymbol();
        }

        public void ResetAccent()
        {
            isAccent = false;
        }

        string GetSymbolStrRegistered()
        {
            return (Settings.IsUpperRegister ? symbol.ToString().ToUpper() : symbol.ToString());
        }

        void CheckSymbol()
        {
            isGlas = false;
            switch (symbol)
            {
                case 'а':
                    isGlas = true;
                    break;
                case 'б':
                    break;
                case 'в':
                    break;
                case 'г':
                    break;
                case 'д':
                    break;
                case 'е':
                    isGlas = true;
                    break;
                case 'ё':
                    isGlas = true;
                    break;
                case 'ж':
                    break;
                case 'з':
                    break;
                case 'и':
                    isGlas = true;
                    break;
                case 'й':
                    isGlas = true;
                    break;
                case 'к':
                    break;
                case 'л':
                    break;
                case 'м':
                    break;
                case 'н':
                    break;
                case 'о':
                    isGlas = true;
                    break;
                case 'п':
                    break;
                case 'р':
                    break;
                case 'с':
                    break;
                case 'т':
                    break;
                case 'у':
                    isGlas = true;
                    break;
                case 'ф':
                    break;
                case 'х':
                    break;
                case 'ц':
                    break;
                case 'ч':
                    break;
                case 'ш':
                    break;
                case 'щ':
                    break;
                case 'ъ':
                    break;
                case 'ы':
                    isGlas = true;
                    break;
                case 'ь':
                    break;
                case 'э':
                    isGlas = true;
                    break;
                case 'ю':
                    isGlas = true;
                    break;
                case 'я':
                    isGlas = true;
                    break;
            }
        }

        public static bool IsGlasSymbol(char symbol)
        {
            bool is_Glas = false;
            switch (symbol)
            {
                case 'а':
                    is_Glas = true;
                    break;
                case 'б':
                    break;
                case 'в':
                    break;
                case 'г':
                    break;
                case 'д':
                    break;
                case 'е':
                    is_Glas = true;
                    break;
                case 'ё':
                    is_Glas = true;
                    break;
                case 'ж':
                    break;
                case 'з':
                    break;
                case 'и':
                    is_Glas = true;
                    break;
                case 'й':
                    is_Glas = true;
                    break;
                case 'к':
                    break;
                case 'л':
                    break;
                case 'м':
                    break;
                case 'н':
                    break;
                case 'о':
                    is_Glas = true;
                    break;
                case 'п':
                    break;
                case 'р':
                    break;
                case 'с':
                    break;
                case 'т':
                    break;
                case 'у':
                    is_Glas = true;
                    break;
                case 'ф':
                    break;
                case 'х':
                    break;
                case 'ц':
                    break;
                case 'ч':
                    break;
                case 'ш':
                    break;
                case 'щ':
                    break;
                case 'ъ':
                    break;
                case 'ы':
                    is_Glas = true;
                    break;
                case 'ь':
                    break;
                case 'э':
                    is_Glas = true;
                    break;
                case 'ю':
                    is_Glas = true;
                    break;
                case 'я':
                    is_Glas = true;
                    break;
            }
            return is_Glas;
        }

        public static List<char> GetBaseCollectionSymbols(bool isGlas)
        {
            List<char> list = new List<char>();

            if (isGlas)
            {
                list.Add('а');
                list.Add('е');
                list.Add('ё');
                list.Add('и');
                list.Add('й');
                list.Add('о');
                list.Add('у');
                list.Add('ы');
                list.Add('э');
                list.Add('ю');
                list.Add('я');
            }
            else
            {
                list.Add('б');
                list.Add('в');
                list.Add('г');
                list.Add('д');
                list.Add('ж');
                list.Add('з');
                list.Add('к');
                list.Add('л');
                list.Add('м');
                list.Add('н');
                list.Add('п');
                list.Add('р');
                list.Add('с');
                list.Add('т');
                list.Add('ф');
                list.Add('х');
                list.Add('ц');
                list.Add('ч');
                list.Add('ш');
                list.Add('щ');
            }

            return list;
        }
    }
}
