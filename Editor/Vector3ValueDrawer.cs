using UnityEditor;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof(Vector3Value))]
    public class Vector3ValueDrawer : GenericValuePropertyDrawer<Vector3Value, Vector3Object>
    {
    }
}