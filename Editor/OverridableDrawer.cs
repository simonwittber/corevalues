using UnityEditor;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof(Overrideable<>))]
    public class OverridableDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var hasValueProperty = property.FindPropertyRelative(nameof(Overrideable<bool>.hasValue));
            var baseHeight = EditorGUI.GetPropertyHeight(hasValueProperty, label); 
            if (hasValueProperty.boolValue)
            {
                var valueProperty = property.FindPropertyRelative(nameof(Overrideable<bool>.value));
                baseHeight += EditorGUI.GetPropertyHeight(valueProperty, label);
            }

            return baseHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var hasValueProperty = property.FindPropertyRelative(nameof(Overrideable<bool>.hasValue));
            var valueProperty = property.FindPropertyRelative(nameof(Overrideable<bool>.value));
            
            var rect = position;
            EditorGUI.BeginProperty(position, label, property);
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, hasValueProperty, label);
            if (hasValueProperty.boolValue)
            {
                rect = position;
                rect.y += EditorGUIUtility.singleLineHeight;
                rect.height -= EditorGUIUtility.singleLineHeight;
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(rect, valueProperty, GUIContent.none);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();

        }
    }
}