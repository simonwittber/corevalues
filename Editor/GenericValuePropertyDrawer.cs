using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dffrnt.CoreValues
{
    public class GenericValuePropertyDrawer<T, U> : PropertyDrawer where U : ScriptableObject
    {

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;

            // Create property fields.
            container.Add(new Label(property.name) {style = {width = EditorGUIUtility.labelWidth}});

            var overrideValueProperty = property.FindPropertyRelative("overrideValue");
            var overrideValueField = new PropertyField(overrideValueProperty, "") {tooltip = $"Override this value with a reference.", style = {width = 18}};
            var valueField = new PropertyField(property.FindPropertyRelative("value"), "") {style = {flexGrow = 1}};
            var objectProperty = property.FindPropertyRelative("_object");
            var objectField = new PropertyField(objectProperty, "") {style = {flexGrow = 1}};

            void CreateAsset()
            {
                var path = EditorPrefs.GetString("GVP_CreateAssetPath", "Assets");
                var defaultName = $"{property.name}.asset";
                var extension = "asset";
                path = EditorUtility.SaveFilePanel("Save Asset", path, defaultName, extension);
                if (path != string.Empty)
                {
                    var relativePath = System.IO.Path.GetRelativePath("Assets", path);
                    var savedPath = System.IO.Path.GetDirectoryName(relativePath);
                    EditorPrefs.SetString("GVP_CreateAssetPath", savedPath);
                    var newAsset = ScriptableObject.CreateInstance<U>();
                    AssetDatabase.CreateAsset(newAsset, $"Assets/{relativePath}");
                    objectProperty.objectReferenceValue = newAsset;
                    objectProperty.serializedObject.ApplyModifiedProperties();
                }
            } 

            var createButton = new Button(CreateAsset);
            createButton.text = "Create";
            

            void UpdateVisibleFields()
            {
                if (overrideValueProperty.boolValue)
                {
                    objectField.style.display = DisplayStyle.Flex;
                    valueField.style.display = DisplayStyle.None;
                    if (objectProperty.objectReferenceValue == null)
                    {
                        createButton.style.display = DisplayStyle.Flex;
                    }
                    else
                    {
                        createButton.style.display = DisplayStyle.None;
                    }
                }
                else
                {
                    objectField.style.display = DisplayStyle.None;
                    valueField.style.display = DisplayStyle.Flex;
                    createButton.style.display = DisplayStyle.None;
                }
            }

            overrideValueField.RegisterValueChangeCallback(evt => { UpdateVisibleFields(); });
            objectField.RegisterValueChangeCallback(evt => { UpdateVisibleFields(); });
        
            container.Add(overrideValueField);
            overrideValueField.style.marginRight = 16;
            container.Add(objectField);
            container.Add(valueField);
            container.Add(createButton);
            UpdateVisibleFields();

            return container;
        }
    }
}