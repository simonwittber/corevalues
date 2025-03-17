using UnityEditor;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof(AnimationCurveValue))]
    public class AnimationCurveValueDrawer : GenericValuePropertyDrawer<AnimationCurveValue, AnimationCurveObject>
    {
    }
}