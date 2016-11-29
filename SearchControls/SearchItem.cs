using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SearchControls
{
    public class SearchItem
    {
        public SearchItem(string title, Image icon, params string[] keywords)
        {
            Icon = icon;
            Title = title;
            Keywords = keywords.ToList();
        }

        public Image Icon { get; private set; }

        public string Title { get; private set; }

        public IEnumerable<string> Keywords { get; private set; }

        public override string ToString()
        {
            return Title;
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
    }
}
