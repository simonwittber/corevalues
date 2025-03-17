using UnityEditor;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof(Vector3IntValue))]
    public class Vector3IntValueDrawer : GenericValuePropertyDrawer<Vector3IntValue, Vector3IntObject>
    {
    }
}