using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [CoreValueType(typeof(AnimationCurve))]
    [Serializable]
    public struct AnimationCurveValue : IGenericValue
    {
        [SerializeField] private AnimationCurveObject _object;
        [SerializeField] private AnimationCurve value;
        [SerializeField] private bool overrideValue;
        public AnimationCurve Value => !overrideValue ? value : _object.GetValue();

        public static implicit operator AnimationCurve(AnimationCurveValue reference)
        {
            return reference.Value;
        }
    }
}