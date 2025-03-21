﻿using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [CoreValueType(typeof(Color))]
    [Serializable]
    public struct ColorValue : IGenericValue
    {
        [SerializeField] private ColorObject _object;
        [SerializeField] private Color value;
        [SerializeField] private bool overrideValue;
        public Color Value => !overrideValue ? value : _object.GetValue();

        public static implicit operator Color(ColorValue reference)
        {
            return reference.Value;
        }
    }
}