using System;
using System.Linq;
using System.Reflection;
using Dffrnt.CoreValues;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof(TypeToStringDropdownAttribute))]
    public class TypeToStringDropdownDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var baseType = (attribute as TypeToStringDropdownAttribute)?.type;
            
            // draw the property
            var typeName = property.stringValue;
            var rect = position;
            rect.width -= 20;
            EditorGUI.PropertyField(rect, property, label);
            
            if (EditorGUI.DropdownButton(rect, new(typeName), FocusType.Keyboard))
            {
                var allTypes = TypeCache.GetTypesDerivedFrom(baseType).ToArray();
                var dropdown = new TypeSelectorDropDown(new AdvancedDropdownState(), allTypes);
                rect.height = 0;
                dropdown.callback = (type) =>
                {
                    property.stringValue = type == null ? "" : type.AssemblyQualifiedName;
                    property.serializedObject.ApplyModifiedProperties();
                };
                dropdown.Show(rect);
            }
        }
        
    }
}