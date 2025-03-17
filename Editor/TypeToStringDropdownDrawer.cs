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
        TypeSelectorDropDown dropdown;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var baseType = (attribute as TypeToStringDropdownAttribute)?.type;
            if(dropdown == null) CreateDropDown(baseType);
            var target = property.serializedObject.targetObject;
            // draw the property
            var typeName = property.stringValue;
            var rect = position;
            rect.width -= 20;
            EditorGUI.PropertyField(rect, property, label);
            CreateDropDown(baseType);
            if (GUI.Button(new Rect(position.x + position.width - 20, position.y, 20, position.height), "V"))
            {
                dropdown.Show(new Rect(position.x, position.y, position.width, 0));
                dropdown.callback = (type) =>
                {
                    property.stringValue = type == null ? "" : type.AssemblyQualifiedName;
                    property.serializedObject.ApplyModifiedProperties();
                };
            }


        }

        private void CreateDropDown(Type baseType)
        {
            var allTypes = TypeCache.GetTypesDerivedFrom(baseType).ToArray();
            dropdown = new TypeSelectorDropDown(new AdvancedDropdownState(), allTypes);
        }
    }
}