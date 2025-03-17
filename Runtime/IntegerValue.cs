using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [CoreValueType(typeof(int))]
    [Serializable]
    public struct IntegerValue : IGenericValue
    {
        [SerializeField] private IntegerObject _object;
        [SerializeField] private int value;
        [SerializeField] private bool overrideValue;
        public int Value => !overrideValue ? value : _object.GetValue();

        public static implicit operator int(IntegerValue reference)
        {
            return reference.Value;
        }
    }
}