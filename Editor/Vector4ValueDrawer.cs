using UnityEditor;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof (Vector4Value))]
    public class Vector4ValueDrawer : GenericValuePropertyDrawer<Vector4Value, Vector4Object> { }
}