using System;
using System.Collections.Generic;
using System.Linq;
using Dffrnt.CoreValues;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dffrnt.CoreValues
{
    [CustomEditor(typeof(ServiceLocatorData))]
    public class ServiceLocatorDataEditor : Editor
    {
        private VisualElement root;
        public override VisualElement CreateInspectorGUI()
        {
            root = new VisualElement();
            
            // Create a button to add any missing interfaces
            var addMissingInterfacesButton = new Button(() =>
            {
                AddMissingInterfacesToServiceLocatorData();
                serializedObject.Update();
            });
            addMissingInterfacesButton.text = "Add Missing Interfaces";
            
            root.Add(addMissingInterfacesButton);
            
            var listView = new MultiColumnListView
            {
                fixedItemHeight = 25,
                showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly,
                showBoundCollectionSize = false
            };
            
            listView.columns.Add(new Column
            {
                title = "Interface",
                width = 200,
                bindingPath = nameof(ServiceBindingPair.interfaceType)
            });
            
            listView.columns.Add(new Column
            {
                title = "Implementation",
                width = 200,
                bindingPath = nameof(ServiceBindingPair.implementationType)
            });

            // Bind the list to the bindings property
            listView.BindProperty(serializedObject.FindProperty(nameof(ServiceLocatorData.bindings)));
            root.Add(listView);
            
            var code = GenerateBindingCode();
            var codeField = new TextField()
            {
                value = code
            };
            root.Add(codeField);
            listView.RegisterCallback((ChangeEvent<string> evt) =>
            {
                serializedObject.ApplyModifiedProperties();
                codeField.value = GenerateBindingCode();
            });
            var saveButton = new Button(() =>
            {
                var path = AssetDatabase.GetAssetPath(target);
                var directory = System.IO.Path.GetDirectoryName(path);
                var filename = System.IO.Path.Combine(directory, "ServiceLocatorBindings.cs");
                System.IO.File.WriteAllText(filename, GenerateBindingCode());
                AssetDatabase.Refresh();
            });
            saveButton.text = "Save Code";
            root.Add(saveButton);
            return root;
        }

        string GenerateBindingCode()
        {
            var code = @"
namespace Dffrnt.CoreValues
{
    public static class ServiceLocatorBindings
    {
        public static void BindAll()
        {
";
            var serviceLocatorData = target as ServiceLocatorData;
            if (serviceLocatorData == null) return null;
            foreach (var binding in serviceLocatorData.bindings)
            {
                code = $"{code}             ServiceLocator<{binding.interfaceType.Type.FullName}>.Bind(new {binding.implementationType.selectedType.Type.FullName}());\n";
            }
            code += @"
        }
    }
}
";
            return code;
        }

        private void AddMissingInterfacesToServiceLocatorData()
        {
            var serviceLocatorData = target as ServiceLocatorData;
            if (serviceLocatorData == null) return;
            
            // remove any null types
            for (int i = serviceLocatorData.bindings.Length - 1; i >= 0; i--)
            {
                var binding = serviceLocatorData.bindings[i];
                if (binding.interfaceType.Type == null)
                {
                    ArrayUtility.RemoveAt(ref serviceLocatorData.bindings, i);
                }
            }

            var interfaceTypes = TypeCache.GetTypesDerivedFrom(typeof(IServiceInterface)).Where(i => i.IsInterface);
            // remove any bindings that are not in the interfaceTypes
            for (int i = serviceLocatorData.bindings.Length - 1; i >= 0; i--)
            {
                var binding = serviceLocatorData.bindings[i];
                if (!interfaceTypes.Contains(binding.interfaceType.Type))
                {
                    ArrayUtility.RemoveAt(ref serviceLocatorData.bindings, i);
                }
            }
            
            foreach (var type in interfaceTypes)
            {
                var found = false;
                foreach (var binding in serviceLocatorData.bindings)
                {
                    if (binding.interfaceType.Type == type)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    var interfaceType = new SerializableType(type);
                    var concreteTypes = TypeCache.GetTypesDerivedFrom(type).Where(i => !i.IsInterface && !i.IsAbstract).ToArray();
                    var concreteType = concreteTypes.Length == 1?concreteTypes[0] : null;
                    var implementationType = new SelectableType() { baseType = interfaceType, selectedType = new SerializableType(concreteType) };
                    var binding = new ServiceBindingPair
                    {
                        interfaceType = interfaceType,
                        implementationType = implementationType
                    };
                    ArrayUtility.Add(ref serviceLocatorData.bindings, binding);
                }
            }
            System.Array.Sort(serviceLocatorData.bindings, (a, b) => String.Compare(a.interfaceType.typeName, b.interfaceType.typeName, StringComparison.Ordinal));
        }
    }
    
    
}