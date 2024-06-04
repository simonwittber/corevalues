using UnityEditor;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof (ColorValue))]
    public class ColorValueDrawer : GenericValuePropertyDrawer<ColorValue, ColorObject> { }
}