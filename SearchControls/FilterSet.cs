using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchControls
{
    delegate KeySetRelationship KeySetPrecedenceComparison<T>(T item1, T item2);

    class FilterSet<KeyT, ItemT>
    {
        private class DefaultKeySetOperator : IKeySetOperator<KeyT>
        {
            private KeySetPrecedenceComparison<KeyT> _precedence;

            public DefaultKeySetOperator(KeySetPrecedenceComparison<KeyT> precedence)
            {
                _precedence = precedence;
            }

            public KeySetRelationship ComparePrecedence(KeyT item1, KeyT item2)
            {
                return _precedence(item1, item2);
            }
        }

        private IKeySetOperator<KeyT> _operator;
        private Dictionary<KeyT, HashSet<ItemT>> _items = new Dictionary<KeyT, HashSet<ItemT>>();
        
        public FilterSet(IKeySetOperator<KeyT> op)
        {
            _operator = op;
        }

        public FilterSet(KeySetPrecedenceComparison<KeyT> compare)
        {
            _operator = new DefaultKeySetOperator(compare);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool ContainsKey(KeyT key)
        {
            return _items.ContainsKey(key);
        }

        public bool Contains(ItemT item)
        {
            return _items.Values.Any(s => s.Contains(item));
        }

        public bool Add(KeyT key, ItemT item)
        {
            if (!_items.ContainsKey(key))
                _items[key] = new HashSet<ItemT>();
            return _items[key].Add(item);
        }

        public bool Remove(KeyT key)
        {
            return _items.Remove(key);
        }

        public bool Remove(ItemT item)
        {
            bool result = false;
            foreach (KeyT key in _items.Keys.ToList())
            {
                result |= _items[key].Remove(item);
                if (_items[key].Count == 0)
                    _items.Remove(key);
            }
            return result;
        }

        public HashSet<ItemT> GetItemSet(KeyT superKey)
        {
            HashSet<ItemT> result = new HashSet<ItemT>();
            foreach (KeyT key in _items.Keys.Where(k => _operator.ComparePrecedence(superKey, k).HasFlag(KeySetRelationship.Super)))
            {
                result.UnionWith(_items[key]);
            }
            return result;
        }
    }
}
