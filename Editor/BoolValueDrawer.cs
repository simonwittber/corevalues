using UnityEditor;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof(BoolValue))]
    public class BoolValueDrawer : GenericValuePropertyDrawer<BoolValue, BoolObject>
    {
    }
}