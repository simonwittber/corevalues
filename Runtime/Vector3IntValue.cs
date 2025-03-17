using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [CoreValueType(typeof(Vector3Int))]
    [Serializable]
    public struct Vector3IntValue : IGenericValue
    {
        [SerializeField] private Vector3IntObject _object;
        [SerializeField] private Vector3Int value;
        [SerializeField] private bool overrideValue;
        public Vector3Int Value => !overrideValue ? value : _object.GetValue();

        public static implicit operator Vector3Int(Vector3IntValue reference)
        {
            return reference.Value;
        }
    }
}