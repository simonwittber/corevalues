using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [Serializable]
    public struct Vector2Value : IGenericValue
    {
        [SerializeField] private Vector2Object _object;
        [SerializeField] private Vector2 value;
        [SerializeField] private bool overrideValue;
        public Vector2 Value => !overrideValue ? value : _object.GetValue();
        public static implicit operator Vector2(Vector2Value reference) => reference.Value;
    }
}