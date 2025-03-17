using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof(IntegerValue))]
    public class IntegerValueDrawer : GenericValuePropertyDrawer<IntegerValue, IntegerObject>
    {
        
    }
}