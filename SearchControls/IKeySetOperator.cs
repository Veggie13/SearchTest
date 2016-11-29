using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchControls
{
    [Flags]
    enum KeySetRelationship
    {
        None = 0,
        Super = 1,
        Sub = 2,
        Equal = Super | Sub
    }

    interface IKeySetOperator<T>
    {
        KeySetRelationship ComparePrecedence(T item1, T item2);
    }
}
