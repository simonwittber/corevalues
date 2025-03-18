using Dffrnt.CoreValues;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof(GameEvent))]
    public class GameEventDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(property.objectReferenceValue == null)
            {
                position.width -= 60;
                EditorGUI.PropertyField(position, property, label);
                position.x += position.width;
                position.width = 60;
                if (GUI.Button(position, "Create", EditorStyles.miniButton))
                {
                    CreateAsset(property.name, property);
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
        
        void CreateAsset(string name, SerializedProperty objectProperty)
        {
            var defaultName = $"{name}.asset";
            var newAsset = ScriptableObjectUtility.CreateInstance<GameEvent>(defaultName);
            if (newAsset != null)
            {
                objectProperty.objectReferenceValue = newAsset;
                objectProperty.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}