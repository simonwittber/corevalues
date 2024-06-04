using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [Serializable]
    public struct BoolValue : IGenericValue
    {
        [SerializeField] private BoolObject _object;
        [SerializeField] private bool value;
        [SerializeField] private bool overrideValue;
        public bool Value => !overrideValue ? value : _object.GetValue();
        public static implicit operator bool(BoolValue reference) => reference.Value;
    }
}