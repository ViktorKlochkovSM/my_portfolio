using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Chitarik
{
    public class Render
    {
        int currentIndex = 0;
        public int CurrentIndex
        {
            get { return currentIndex; }
        }

        List<string> listElements;
        public List<string> ListElements
        {
            get { return listElements; }
            set { listElements = value; }
        }

        List<MultiLineObject> listMobs;
        public List<MultiLineObject> ListMobs
        {
            get { return listMobs; }
            set { listMobs = value; }
        }

        RichTextBox rich_TB;

        public Render(RichTextBox _rich_TB, List<string> _listElements)
        {
            rich_TB = _rich_TB;
            listElements = _listElements;
        }

        void RenderSingleLinePart(bool isDirectionForvard)
        {
            if (Settings.Read_Mode == ReadMode.Stishki || Settings.Read_Mode == ReadMode.Songs || Settings.Read_Mode == ReadMode.Rasskazy || Settings.Read_Mode == ReadMode.Schitalki || Settings.Read_Mode == ReadMode.Skorogovorki)
                currentIndex = 0;

            if (currentIndex >= listElements.Count)
                currentIndex = listElements.Count - 1;

            if (currentIndex >= 0 && currentIndex < listElements.Count)
            {
                //отступ
                int countUpperOtstup = 0;
                if (Settings.UpperMargin > 0)
                {
                    for (int i = 0; i < Settings.UpperMargin; i++)
                    {
                        rich_TB.AppendText("\r\n");
                        countUpperOtstup++;
                    }
                }

                string leftIndentStr = "";
                if (Settings.CountLeftIndent > 0)
                {
                    for (int li = 0; li < Settings.CountLeftIndent; li++)
                    {
                        leftIndentStr += "\t";
                    }
                }

                //выводим нужное кол-во элементов
                int countPartsRedered = 0;
                int directionIndex = 1;
                if (!isDirectionForvard)
                    directionIndex = -1;
                for (int i = currentIndex; i <= listElements.Count && i >= -1; i += directionIndex)
                {
                    if (i == -1)
                    {
                        currentIndex = i = listElements.Count - 1;
                    }
                    if (i == listElements.Count)
                    {
                        currentIndex = i = 0;
                    }

                    if (countPartsRedered > 0)
                    {
                        //после отрисовки элемента сделать переход на новую строку
                        rich_TB.AppendText("\r\n");
                        if (Settings.Read_Mode != ReadMode.Stishki && Settings.Read_Mode != ReadMode.Songs && Settings.Read_Mode != ReadMode.Rasskazy)
                        {
                            if (Settings.BeforeLineMargin > 0)
                            {
                                //междустрочный интервал
                                for (int l = 0; l < Settings.BeforeLineMargin; l++)
                                {
                                    rich_TB.AppendText("\r\n");
                                }
                            }
                        }
                    }

                    //тут отрисуем элемент
                    string s = listElements[i];
                    if (Settings.IsUpperRegister)
                        s = s.ToUpper();
                    rich_TB.AppendText(String.Format("{0}{1}", leftIndentStr, s));
                    countPartsRedered++;

                    currentIndex = i;
                    if (Settings.Read_Mode != ReadMode.Stishki && Settings.Read_Mode != ReadMode.Songs && Settings.Read_Mode != ReadMode.Rasskazy && Settings.Read_Mode != ReadMode.Skorogovorki && Settings.Read_Mode != ReadMode.Schitalki)
                    {
                        if (countPartsRedered == Settings.ElementsCount)
                            break;
                    }
                    else
                    {
                        if (countPartsRedered == listElements.Count)
                            break;
                    }
                }

                //вделим все элементы и выровним 
                rich_TB.SelectAll();
                rich_TB.SelectionAlignment = Settings.TextHorizontalAlignment;
                rich_TB.Select(0, 0);

                //подсвечиваем текст нужными цветами
                if (Settings.Read_Mode == ReadMode.Sloga || Settings.Read_Mode == ReadMode.Slova || Settings.Read_Mode == ReadMode.Predlogenie)
                    ColorizeRederedText();

                rich_TB.Select(0, 0);
            }
        }

        void RenderMultiLinePart(bool isDirectionForvard)
        {
            if (listMobs == null)
                return;

            if (currentIndex >= listMobs.Count)
                currentIndex = listMobs.Count - 1;

            if (currentIndex >= 0 && currentIndex < listMobs.Count)
            {
               
                //отступ
                int countUpperOtstup = 0;
                if (Settings.UpperMargin > 0)
                {
                    for (int i = 0; i < Settings.UpperMargin; i++)
                    {
                        rich_TB.AppendText("\r\n");
                        countUpperOtstup++;
                    }
                }

                string leftIndentStr = "";
                if (Settings.CountLeftIndent > 0)
                {
                    for (int li = 0; li < Settings.CountLeftIndent; li++)
                    {
                        leftIndentStr += "\t";
                    }
                }

                //выводим нужное кол-во элементов
                int countPartsRedered = 0;
                int directionIndex = 1;
                if (!isDirectionForvard)
                    directionIndex = -1;
                for (int i = currentIndex; i <= listMobs.Count && i >= -1; i += directionIndex)
                {
                    if (i == -1)
                    {
                        currentIndex = i = listMobs.Count - 1;
                    }
                    if (i == listMobs.Count)
                    {
                        currentIndex = i = 0;
                    }

                    if (countPartsRedered > 0)
                    {
                        //после отрисовки элемента сделать переход на новую строку
                        rich_TB.AppendText("\r\n");
                        if (Settings.Read_Mode != ReadMode.Stishki && Settings.Read_Mode != ReadMode.Songs && Settings.Read_Mode != ReadMode.Rasskazy)
                        {
                            if (Settings.BeforeLineMargin > 0)
                            {
                                //междустрочный интервал
                                for (int l = 0; l < Settings.BeforeLineMargin; l++)
                                {
                                    rich_TB.AppendText("\r\n");
                                }
                            }
                        }
                    }

                    //тут отрисуем элемент
                    MultiLineObject mob = listMobs[i];
                    for (int m = 0; m < mob.ContentList.Count; m++)
                    {
                        if(m > 0)
                            rich_TB.AppendText("\r\n");
                        string s = mob.ContentList[m];
                        if (Settings.IsUpperRegister)
                            s = s.ToUpper();
                        rich_TB.AppendText(String.Format("{0}{1}", leftIndentStr, s));
                    }

                    countPartsRedered++;
                    

                    //currentIndex = i;
                    if (countPartsRedered == Settings.ElementsCount)
                        break;
                }

                //вделим все элементы и выровним 
                rich_TB.SelectAll();
                rich_TB.SelectionAlignment = Settings.TextHorizontalAlignment;
                rich_TB.Select(0, 0);

                //подсвечиваем текст нужными цветами
                if (Settings.Read_Mode == ReadMode.Sloga || Settings.Read_Mode == ReadMode.Slova || Settings.Read_Mode == ReadMode.Predlogenie)
                    ColorizeRederedText();

                rich_TB.Select(0, 0);
            }
        }

        public void RenderCurrentPart(bool isDirectionForvard)
        {
            rich_TB.Clear();
            //настройки шрифта
            rich_TB.Font = Settings.FontGlobal;
            rich_TB.ForeColor = Settings.DefaultForeColor;
            rich_TB.BackColor = Settings.BackGroundColor;

            if (currentIndex < 0)
                currentIndex = 0;

            if (Settings.Read_Mode == ReadMode.Schitalki || Settings.Read_Mode == ReadMode.Skorogovorki)
                RenderMultiLinePart(isDirectionForvard);
            else
                RenderSingleLinePart(isDirectionForvard);
        }

        Color GetColorBySymbol(char c, Color prevColor)
        {
            Color color = Settings.DefaultForeColor;

            switch (c)
            {
                case '!': break;
                case '"': break;
                case '№': break;
                case ';': break;
                case '%': break;
                case ':': break;
                case '?': break;
                case ',': break;
                case '.': break;
                case '*': break;
                case '(': break;
                case ')': break;
                case '-': break;
                case '—': break;
                case '_': break;
                case '=': break;
                case '+': break;
                case '/': break;
                case '\\': break;
                case '0': break;
                case '1': break;
                case '2': break;
                case '3': break;
                case '4': break;
                case '5': break;
                case '6': break;
                case '7': break;
                case '8': break;
                case '9': break;
                case '»': break;
                case '«': break;
                case '\u0301': color = prevColor; break;
                default:
                    if(Settings.IsColorizerBukvEnabled)
                        color = (SymbolInfo.IsGlasSymbol(c) ? Settings.GlasColor : Settings.SoglasColor);
                    break;
            }

            return color;
        }

        void ColorizeRederedText()
        {
            Color prevColor = Color.White;
            for (int i = 0; i < rich_TB.Text.Length; i++)
            {
                char c = rich_TB.Text.ToLower()[i];
                if (c != '\r' && c != '\n')
                {
                    rich_TB.Select(i, 1);

                    prevColor = rich_TB.SelectionColor = GetColorBySymbol(c, prevColor);
                }
            }
            rich_TB.Select(0, 0);
        }

        public void RenderNextPart()
        {
            currentIndex++;
            if (currentIndex == listElements.Count)
                currentIndex = 0;
            RenderCurrentPart(true);
        }

        public void RenderPrevPart()
        {
            currentIndex--;
            if (currentIndex == -1)
                currentIndex = listElements.Count -1;
            RenderCurrentPart(false);
        }
    }
}
