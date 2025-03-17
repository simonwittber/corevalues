using System;
using System.Collections.Generic;
using Dffrnt.CoreValues;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dffrnt.CoreValues
{
    // [CustomPropertyDrawer(typeof(SetProperties))]
    // public class SetPropertiesDrawer : PropertyDrawer
    // {
    //     public override VisualElement CreatePropertyGUI(SerializedProperty property)
    //     {
    //         var container = new VisualElement();
    //         container.style.flexDirection = FlexDirection.Row;
    //
    //
    //         // Get short name of the type
    //         // var typeNameProperty = property.FindPropertyRelative(nameof(SerializableType.typeName));
    //         var label = new Label();
    //         container.Add(label);
    //         return container;
    //     }
    // }

    [CustomPropertyDrawer(typeof(SerializableType), useForChildren: false)]
    public class SerializableTypeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;


            // Get short name of the type
            var typeNameProperty = property.FindPropertyRelative(nameof(SerializableType.typeName));
            var label = new Label();
            container.Add(label);

            var propertyField = new PropertyField(typeNameProperty) {style = {display = DisplayStyle.None}};
            propertyField.RegisterValueChangeCallback(e => label.text = typeNameProperty.stringValue.Split(',')[0]);
            container.Add(propertyField);

            return container;
        }
    }
}