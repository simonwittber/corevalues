using UnityEditor;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof (IntegerValue))]
    public class IntegerValueDrawer : GenericValuePropertyDrawer<IntegerValue, IntegerObject> { }
}