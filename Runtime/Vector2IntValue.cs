using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [CoreValueType(typeof(Vector2Int))]
    [Serializable]
    public struct Vector2IntValue : IGenericValue
    {
        [SerializeField] private Vector2IntObject _object;
        [SerializeField] private Vector2Int value;
        [SerializeField] private bool overrideValue;
        public Vector2Int Value => !overrideValue ? value : _object.GetValue();

        public static implicit operator Vector2Int(Vector2IntValue reference)
        {
            return reference.Value;
        }
    }
}