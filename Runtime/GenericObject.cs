using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    public class GenericObject : ScriptableObject
    {
        [Multiline] [SerializeField]
#pragma warning disable
        private string description = "";
    }

    public class GenericObject<T> : GenericObject
    {
        [SerializeField] private T _value;

        [SerializeField] private T _initialValue;
        
        public event Action<T> OnChange;

        public T Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        private void OnEnable()
        {
            _value = _initialValue;
        }

        public T GetValue()
        {
            return Application.isPlaying ? _value : _initialValue;
        }

        public void SetValue(T value)
        {
            if (Application.isPlaying)
                _value = value;
            else
                _initialValue = value;
            OnChange?.Invoke(this);
        }

        public static implicit operator T(GenericObject<T> reference)
        {
            return reference != null ? reference.GetValue() : default;
        }
    }
}