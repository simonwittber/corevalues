using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [Serializable]
    public struct StringValue : IGenericValue
    {
        [SerializeField] private StringObject _object;
        [SerializeField] private string value;
        [SerializeField] private bool overrideValue;
        public string Value => overrideValue ? _object.GetValue() : value;
        public static implicit operator string(StringValue reference) => reference.Value;
    }
}