using UnityEditor;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof(Vector2Value))]
    public class Vector2ValueDrawer : GenericValuePropertyDrawer<Vector2Value, Vector2Object>
    {
    }
}