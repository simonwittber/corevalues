using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
  [Serializable]
  public struct FloatValue : IGenericValue
  {
    [SerializeField] private FloatObject _object;
    [SerializeField] private float value;
    [SerializeField] private bool overrideValue;
    public float Value => !overrideValue ? value : _object.GetValue();
    public static implicit operator float(FloatValue reference) => reference.Value;
  }
}
