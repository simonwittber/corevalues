using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [Serializable]
    public struct Vector4Value : IGenericValue
    {
        [SerializeField] private Vector4Object _object;
        [SerializeField] private Vector4 value;
        [SerializeField] private bool overrideValue;
        public Vector4 Value => !overrideValue ? value : _object.GetValue();
        public static implicit operator Vector4(Vector4Value reference) => reference.Value;
    }
}