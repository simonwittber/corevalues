using System;
using System.Linq;
using Dffrnt.CoreValues;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dffrnt.CoreValues
{
 
    [CustomPropertyDrawer(typeof(SelectableType), useForChildren: false)]
    public class SelectableTypeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();

            //find types that implement the base type
            var selectableType = property.managedReferenceValue as SelectableType;
            Debug.Log(property.managedReferenceValue);
            if(selectableType == null) return container;
            if (selectableType.baseType.Type == null)
            {
                container.Add(new Label($"BaseType is missing ({selectableType.baseType.typeName})"));
                return container;
            }

            var typesDerivedFrom = TypeCache.GetTypesDerivedFrom(selectableType.baseType.Type);

            // Create a dropdown to select the implementationType
            var choices = typesDerivedFrom.Select(i => i.AssemblyQualifiedName);
            var dropdown = new DropdownField();
            dropdown.choices = choices.ToList();
            if (selectableType.selectedType != null)
                dropdown.index = choices.ToList().IndexOf(selectableType.selectedType.typeName);
            dropdown.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                selectableType.selectedType.typeName = evt.newValue;
                property.managedReferenceValue = selectableType;
                property.serializedObject.ApplyModifiedProperties();
            });
            //var popupField = new PopupField<string>("", choices.ToList(), 0);
            container.Add(dropdown);
            return container;
        }
    }
}