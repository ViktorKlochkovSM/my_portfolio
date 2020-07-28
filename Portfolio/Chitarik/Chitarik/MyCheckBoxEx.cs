using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Chitarik
{
    public class MyCheckBoxEx:CheckBox
    {
        public static List<MyCheckBoxEx> checkBoxesList = new List<MyCheckBoxEx>();
        public static int countCheckBtns = 0;
        int index = 0;
        public int Index
        {
            get { return index; }
        }

        Bukva bukva;
        public MyCheckBoxEx(int _index, Bukva _bukva, string text, bool is_checked)
        {
            bukva = _bukva;
            index = _index;
            this.Appearance = Appearance.Button;
            this.Text = text;
            this.Checked = is_checked;
            
            bool is_enabled = false;
            if (bukva != null && bukva.Symbol_Info.IsGlas)
                is_enabled = true;

            this.Enabled = is_enabled;

            if (is_checked)
            {
                this.BackColor = Color.WhiteSmoke;
            }
        }
    }
}
