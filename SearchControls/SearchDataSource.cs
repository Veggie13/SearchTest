using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchControls
{
    public class SearchDataSource : ICollection<SearchItem>
    {
        private FilterSet<string, SearchItem> _set;
        private HashSet<SearchItem> _all = new HashSet<SearchItem>();

        public SearchDataSource()
        {
            _set = new FilterSet<string, SearchItem>((x, y) =>
            {
                if (x.Equals(y))
                    return KeySetRelationship.Equal;
                if (x.Contains(y))
                    return KeySetRelationship.Sub;
                if (y.Contains(x))
                    return KeySetRelationship.Super;
                return KeySetRelationship.None;
            });
        }

        public HashSet<SearchItem> this[string key]
        {
            get
            {
                return _set.GetItemSet(key);
            }
        }

        public void Add(SearchItem item)
        {
            bool result = false;
            foreach (string keyword in item.Keywords)
            {
                result |= _set.Add(keyword, item);
            }

            if (result)
            {
                _all.Add(item);
            }
        }

        public void Clear()
        {
            _set.Clear();
            _all.Clear();
        }

        public bool Contains(SearchItem item)
        {
            return _all.Contains(item);
        }

        public void CopyTo(SearchItem[] array, int arrayIndex)
        {
            _all.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _all.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(SearchItem item)
        {
            if (!_all.Remove(item))
                return false;

            _set.Remove(item);
            return true;
        }

        public IEnumerator<SearchItem> GetEnumerator()
        {
            return _all.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
