using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dffrnt.CoreValues;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dffrnt.CoreValues
{
    
    
    [CustomPropertyDrawer(typeof(RefPickerAttribute))]
    public class RefPickerDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var fieldType = fieldInfo.FieldType;
            if (fieldInfo.FieldType.IsArray || (fieldInfo.FieldType.IsGenericType && fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>)))
            {
                fieldType = fieldInfo.FieldType.GetElementType() ?? fieldInfo.FieldType.GetGenericArguments()[0];
            }
            
            var typeName = property.managedReferenceValue == null?"None":NiceName(property.managedReferenceValue.GetType());
            var rect = position;
            var labelWidth = EditorGUIUtility.labelWidth + 2;
            rect.x += labelWidth;
            rect.width -= labelWidth;
            rect.height = EditorGUIUtility.singleLineHeight;
            var menuRect = rect;
            
            if (EditorGUI.DropdownButton(rect, new(typeName), FocusType.Keyboard))
            {
                
                var allTypes = TypeCache.GetTypesDerivedFrom(fieldType).ToArray();
                var dropdown = new TypeSelectorDropDown(new AdvancedDropdownState(), allTypes);
                dropdown.callback = (type) =>
                {
                    property.managedReferenceValue = type == null?null:System.Activator.CreateInstance(type);
                    property.serializedObject.ApplyModifiedProperties();
                };
                
                rect.height = 0;
                dropdown.Show(rect);
            }
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, position.height), property, label, true);
        }

        private string NiceName(Type type)
        {
            if (type == null) return null;
            var nna = type.GetCustomAttribute<NiceNameAttribute>();
            if (nna != null) return nna.name;
            return type.Name;
        }
    }
    
    [CustomPropertyDrawer(typeof(ProxyPickerAttribute))]
    public class ProxyPickerDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var fieldType = fieldInfo.FieldType;
            if (fieldInfo.FieldType.IsArray || (fieldInfo.FieldType.IsGenericType && fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>)))
            {
                fieldType = fieldInfo.FieldType.GetElementType() ?? fieldInfo.FieldType.GetGenericArguments()[0];
            }
            
            var typeName = property.managedReferenceValue?.GetType().Name ?? "None";
            var rect = position;
            var labelWidth = EditorGUIUtility.labelWidth + 2;
            rect.x += labelWidth;
            rect.width -= labelWidth;
            rect.height = EditorGUIUtility.singleLineHeight;
            var menuRect = rect;
            
            if (EditorGUI.DropdownButton(rect, new(typeName), FocusType.Keyboard))
            {
                var menu = new GenericMenu();
                menu.AddItem(new("None"), typeName == "None", () =>
                {
                    property.managedReferenceValue = default;
                    property.serializedObject.ApplyModifiedProperties();
                });
                foreach (var type in TypeCache.GetTypesDerivedFrom(typeof(Component)))
                {
                    var menuPath = type.Name;
                    menu.AddItem(new(menuPath), typeName == type.Name, () =>
                    {
                        var proxyType = ProxyBuilder.CreateProxyType(type);
                        var instance = System.Activator.CreateInstance(proxyType);
                        property.managedReferenceValue = instance;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
                menu.DropDown(rect);
            }
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, position.height), property, label, true);
        }
      
    }
}