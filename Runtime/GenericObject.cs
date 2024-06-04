using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    public class GenericObject : ScriptableObject
    {
        [Multiline]
        [SerializeField]
        #pragma warning disable
        private string description = "";
    }
  
    public class GenericObject<T> : GenericObject, ICore
    {
        [SerializeField]
        private T _value;

        [SerializeField]
        private T _initialValue;

        public T Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        private void OnEnable() => _value = _initialValue;

        public event Action<T> OnChange;

        public T GetValue() => !Application.isPlaying ? _initialValue : _value;

        public void SetValue(T value)
        {
            if (Application.isPlaying)
                _value = value;
            else
                _initialValue = value;
            Action<T> onChange = OnChange;
            if (onChange == null)
                return;
            onChange((T) this);
        }

        public static implicit operator T(GenericObject<T> reference)
        {
            return reference != null ? reference.GetValue() : default (T);
        }
    }
}