using UnityEditor;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof (Vector2IntValue))]
    public class Vector2IntValueDrawer : GenericValuePropertyDrawer<Vector2IntValue, Vector2IntObject> { }
}