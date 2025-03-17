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
            var typeName = NiceName(property.managedReferenceValue?.GetType()) ?? "None";
            var rect = position;
            var labelWidth = EditorGUIUtility.labelWidth + 2;
            rect.x += labelWidth;
            rect.width -= labelWidth;
            rect.height = EditorGUIUtility.singleLineHeight;
            var menuRect = rect;
            
            if (EditorGUI.DropdownButton(rect, new(typeName), FocusType.Keyboard))
            {
                // var menu = new GenericMenu();
                // menu.AddItem(new("None"), typeName == "None", () =>
                // {
                //     property.managedReferenceValue = default;
                //     property.serializedObject.ApplyModifiedProperties();
                // });
                var allTypes = TypeCache.GetTypesDerivedFrom(fieldType).ToArray();
                var dropdown = new TypeSelectorDropDown(new AdvancedDropdownState(), allTypes);
                dropdown.callback = (type) =>
                {
                    property.managedReferenceValue = type == null?null:System.Activator.CreateInstance(type);
                    property.serializedObject.ApplyModifiedProperties();
                };
                // foreach (var type in TypeCache.GetTypesDerivedFrom(fieldType))
                // {
                //     var constructor = type.GetConstructor(System.Type.EmptyTypes);
                //     if(constructor == null) continue;
                //     var menuPathAttribute = type.GetCustomAttribute<MenuPathAttribute>();
                //     var menuPath = menuPathAttribute?.path ?? type.Name;
                //     menu.AddItem(new(menuPath), typeName == type.Name, () =>
                //     {
                //         property.managedReferenceValue = constructor.Invoke(null);
                //         property.serializedObject.ApplyModifiedProperties();
                //     });
                // }
                rect.height = 0;
                dropdown.Show(rect);
                // menu.DropDown(rect);
            }
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, position.height), property, label, true);
        }

        private string NiceName(Type type)
        {
            if (type == null) return null;
            var nna = type.GetCustomAttribute<NiceNameAttribute>();
            if (nna != null) return nna.name;
            return null;
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

    [CustomPropertyDrawer(typeof(Overrideable<>))]
    public class OverridableDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var hasValueProperty = property.FindPropertyRelative(nameof(Overrideable<bool>.hasValue));
            var valueProperty = property.FindPropertyRelative(nameof(Overrideable<bool>.value));
            
            var rect = position;
            var labelWidth = EditorGUIUtility.labelWidth + 2;
            // rect.x += labelWidth;
            // rect.width -= labelWidth;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, hasValueProperty, label);
            if (hasValueProperty.boolValue)
            {
                rect.x += labelWidth + 22;
                rect.width -= (labelWidth + 22);
                EditorGUI.PropertyField(rect, valueProperty, GUIContent.none);
            }

            property.serializedObject.ApplyModifiedProperties();

        }
    }
}