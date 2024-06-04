using UnityEditor;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof (StringValue))]
    public class StringValueDrawer : GenericValuePropertyDrawer<StringValue, StringObject> { }
}