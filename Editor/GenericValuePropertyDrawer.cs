using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dffrnt.CoreValues
{
    public class GenericValuePropertyDrawer<T, U> : PropertyDrawer where U : ScriptableObject
    {

        void CreateAsset(string name, SerializedProperty objectProperty)
        {
            var defaultName = $"{name}.asset";
            var newAsset = ScriptableObjectUtility.CreateInstance<U>(defaultName, "ValueObject");
            if (newAsset != null)
            {
                objectProperty.objectReferenceValue = newAsset;
                objectProperty.serializedObject.ApplyModifiedProperties();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var overrideValueProperty = property.FindPropertyRelative("overrideValue");
            if(overrideValueProperty.boolValue)
                return base.GetPropertyHeight(property, label);
            var valueProperty = property.FindPropertyRelative("value");
            return EditorGUI.GetPropertyHeight(valueProperty);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var overrideValueProperty = property.FindPropertyRelative("overrideValue");
            var valueProperty = property.FindPropertyRelative("value");
            var objectProperty = property.FindPropertyRelative("_object");
            
            // label.text = $"[ {property.displayName} ]";
            label = new GUIContent($"[ {property.displayName} ]", label.tooltip);
            
            // The next two lines force the property to be recognized as a IGenericValue to the contextualMenu callback.
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.EndProperty();
            
            if (overrideValueProperty.boolValue)
            {
                if (objectProperty.objectReferenceValue == null)
                {
                    var buttonWidth = 60;
                    position.width -= buttonWidth;
                    EditorGUI.PropertyField(position, objectProperty, label);
                    position.x += position.width;
                    position.width = buttonWidth;
                    if (GUI.Button(position, "Create", EditorStyles.miniButton))
                    {
                        CreateAsset(property.name, objectProperty);
                    }
                }
                else
                {
                    EditorGUI.PropertyField(position, objectProperty, label);
                }
            }
            else
            {
                EditorGUI.PropertyField(position, valueProperty, label, true);
            }
        }
    }
    
    public static class GenericValuePropertyDrawerExtension
    {
        [DidReloadScripts]
        public static void Init()
        {
            EditorApplication.contextualPropertyMenu -= OnPropertyContextMenu;
            EditorApplication.contextualPropertyMenu += OnPropertyContextMenu;
           
        }

        static void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.Generic)
            {
                var overrideProperty = property.FindPropertyRelative("overrideValue");
                if (overrideProperty != null && overrideProperty.propertyType == SerializedPropertyType.Boolean)
                {
                    menu.AddItem(new GUIContent("Enable CoreValue slot?"), overrideProperty.boolValue, () =>
                    {
                        overrideProperty.boolValue = !overrideProperty.boolValue;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
            }
        }
    }
}