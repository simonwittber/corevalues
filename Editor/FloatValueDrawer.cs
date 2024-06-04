using UnityEditor;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof (FloatValue))]
    public class FloatValueDrawer : GenericValuePropertyDrawer<FloatValue, FloatObject> { }
}