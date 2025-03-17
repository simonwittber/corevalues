using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [CoreValueType(typeof(Vector3))]
    [Serializable]
    public struct Vector3Value : IGenericValue
    {
        [SerializeField] private Vector3Object _object;
        [SerializeField] private Vector3 value;
        [SerializeField] private bool overrideValue;
        public Vector3 Value => !overrideValue ? value : _object.GetValue();

        public static implicit operator Vector3(Vector3Value reference)
        {
            return reference.Value;
        }
    }
}