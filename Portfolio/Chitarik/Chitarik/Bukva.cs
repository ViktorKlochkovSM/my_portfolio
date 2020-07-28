using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chitarik
{
    public class Bukva
    {
        SymbolInfo symbol_info;
        public SymbolInfo Symbol_Info
        {
            get { return symbol_info; }
        }

        public Bukva(char _symbol, bool _isAccent)
        {
            symbol_info = new SymbolInfo(_symbol, _isAccent);
        }

        public void ResetAccent()
        {
            symbol_info.ResetAccent();
        }
    }
}
