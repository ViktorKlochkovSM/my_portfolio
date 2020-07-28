using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Chitarik
{
    public class Slovo
    {
        Category baseCategory;
        public Category BaseCategory
        {
            get { return baseCategory; }
        }

        int startIndex;
        public int StartIndex
        {
            get { return startIndex; }
        }

        int accentIndex = -1;//ударение
        public int AccentIndex
        {
            get { return accentIndex; }
        }

        List<Bukva> bukvy;
        public List<Bukva> Bukvy
        {
            get { return bukvy; }
        }

        string originalText;
        public string OriginalText
        {
            get { return originalText; }
        }

        string textWithAccent;
        public string TextWithAccent
        {
            get { return textWithAccent; }
        }

        public bool IsHasAccent
        {
            get { return (Settings.ShowAccent && accentIndex >= 0); }
        }

        bool isChanged = false;
        public bool IsChanged
        {
            get { return isChanged; }
        }

        public Slovo(string _originalText, int _accentIndex, Category _baseCategory)
        {
            baseCategory = _baseCategory;
            originalText = _originalText;
            SetAccentIndex(_accentIndex);
            bukvy = new List<Bukva>();
            string txt = originalText;
            for (int i = 0; i < txt.Length; i++)
            {
                bukvy.Add(new Bukva(txt[i], (IsHasAccent? (i == accentIndex) : false)));
            }
        }

        public string GetText()
        {
            if (Settings.ShowAccent && !String.IsNullOrEmpty(textWithAccent))
                return textWithAccent;
            else
                return originalText;
        }

        public void SetAccentIndex(int index)
        {
            isChanged = accentIndex != index;
            if (isChanged)
            {
                if (baseCategory != null)
                    baseCategory.SetChangedStatus();
            }
            accentIndex = index;
            if (index >= 0)
            {
                if (index < originalText.Length)
                    textWithAccent = originalText.Insert(index + 1, Settings.AccentStr);

                if (bukvy != null)
                {
                    foreach (Bukva b in bukvy)
                    {
                        b.ResetAccent();
                    }

                    Bukva bk = new Bukva(bukvy[index].Symbol_Info.Symbol, true);

                    bukvy[index] = bk;
                }
            }
            else
                textWithAccent = null;
        }

        public Bukva FindBukvuByIndex(int ind)
        {
            if (bukvy != null && bukvy.Count > ind)
            {
                return bukvy[ind];
            }
            return null;
        }
    }
}
