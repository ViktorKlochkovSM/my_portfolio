using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chitarik
{
    public class MultiLineObject
    {
        string name;
        string objectName;
        public string ObjectName
        {
            get { return objectName; }
        }

        List<string> contentList;
        public List<string> ContentList
        {
            get { return contentList; }
        }

        Category baseCategory = null;
        public Category BaseCategory
        {
            get { return baseCategory; }
        }

        public MultiLineObject(string _name, List<string> _contentList, Category _baseCategory)
        {
            name = _name;
            baseCategory = _baseCategory;
            contentList = _contentList;
            if (!String.IsNullOrEmpty(name))
            {
                objectName = name.Replace(".txt", "");
                string[] sl = objectName.Split(new char[]{'\\'});
                if (sl.Length > 0)
                    objectName = sl[sl.Length - 1];
            }
        }
    }
}
