using Dffrnt.CoreValues;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Dffrnt.CoreValues
{
    [CustomPropertyDrawer(typeof(GameEvent))]
    public class GameEventDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;

            // Create property fields.
            container.Add(new Label(property.name) {style = {width = EditorGUIUtility.labelWidth}});

            var objectField = new PropertyField(property, "") {style = {flexGrow = 1}};

            void CreateAsset()
            {
                var defaultName = $"{property.name}.asset";
                var newAsset = ScriptableObjectUtility.CreateInstance<GameEvent>(defaultName);
                if (newAsset != null)
                {
                    property.objectReferenceValue = newAsset;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }

            var createButton = new Button(CreateAsset);
            createButton.text = "Create";

            void UpdateVisibleFields()
            {
                if (property.objectReferenceValue == null)
                    createButton.style.display = DisplayStyle.Flex;
                else
                    createButton.style.display = DisplayStyle.None;
            }

            objectField.RegisterValueChangeCallback(evt => { UpdateVisibleFields(); });

            container.Add(objectField);
            container.Add(createButton);
            UpdateVisibleFields();

            return container;
        }
    }
}