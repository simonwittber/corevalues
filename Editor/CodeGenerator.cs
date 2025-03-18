using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreValues.Editor;
using Dffrnt.CoreValues;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [Serializable]
    internal class CodeGeneratedType
    {
        [TypeToStringDropdown(typeof(Component))]
        public string typeName;
    }
    
    [CreateAssetMenu(menuName = "Core/Code Generator", fileName = "CodeGenerator")]
    internal class CodeGenerator : ScriptableObject
    {
        
        public TextAsset assemblyDefinition;
        public List<CodeGeneratedType> componentTypes = new ();
        

        [ContextMenu("Generate")]
        private void Generate()
        {
            var supportedTypes = TypeCache.GetTypesWithAttribute<CoreValueTypeAttribute>();
            var supportedTypeMap = supportedTypes.ToDictionary(t => t.GetCustomAttribute<CoreValueTypeAttribute>().type);
            var assemblyDefinitionPath = AssetDatabase.GetAssetPath(assemblyDefinition);
            var outputFolder = System.IO.Path.GetDirectoryName(assemblyDefinitionPath);
            var assemblies = new HashSet<Assembly>();

            foreach (var componentType in componentTypes)
            {
                var type = Type.GetType(componentType.typeName);
                if (type == null)
                {
                    Debug.LogWarning($"Type {componentType.typeName} not found");
                    continue;
                }
                if(type.IsAbstract)
                    continue;
                if(type.IsGenericType)
                    continue;
                if(type.IsInterface)
                    continue;
                
                GeneratePropertySetterClasses(type, assemblies, supportedTypeMap, outputFolder);
                GenerateMethodCommandClasses(type, supportedTypeMap, assemblies, outputFolder);
            }
            
            WriteAssemblyDefinitionFile(assemblies, assemblyDefinitionPath);

            AssetDatabase.Refresh();
        }

        private static void GeneratePropertySetterClasses(Type type, HashSet<Assembly> assemblies, Dictionary<Type, Type> supportedTypeMap, string outputFolder)
        {
            var model = new SerializedPropertySetterModel();
            var template = new SerializedPropertySetterTemplate();
            var dependencies = new HashSet<string>();
            dependencies.Add("UnityEngine");
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            if (type.Namespace != null)
                dependencies.Add(type.Namespace);
            assemblies.Add(type.Assembly);
            model.parameters = new List<(string, string)>();
            model.targetTypeName = type.Name;
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                if (property.CanWrite == false)
                    continue;
                if (!supportedTypeMap.TryGetValue(property.PropertyType, out var propertyType))
                    propertyType = property.PropertyType;
                if (property.GetCustomAttribute<ObsoleteAttribute>() != null)
                    continue;
                if (propertyType.GetCustomAttribute<HideInInspector>() != null)
                    continue;
                if (property.Name is "tag" or "hideFlags" or "name")
                    continue;
                model.parameters.Add((propertyType.Name, property.Name));

                dependencies.Add(property.PropertyType.Namespace);
                assemblies.Add(property.PropertyType.Assembly);
            }

            foreach (var field in fields)
            {
                if (!supportedTypeMap.TryGetValue(field.FieldType, out var fieldType))
                    fieldType = field.FieldType;
                model.parameters.Add((fieldType.Name, field.Name));

                dependencies.Add(field.FieldType.Namespace);
                assemblies.Add(field.FieldType.Assembly);
            }

            model.usings = new List<string>(dependencies);
            model.ns = "Dffrnt.CoreValues.Generated";
            model.className = $"{type.Name}_PropertySetter_Command";
            model.menuPath = $"{type.Namespace}.{type.Name}";
            model.niceName = $"{type.Name}.PropertySetter";
            template.model = model;
            var code = template.TransformText();
            var path = $"{outputFolder}/{model.className}.cs";
            System.IO.File.WriteAllText(path, code);
        }

        private void WriteAssemblyDefinitionFile(HashSet<Assembly> assemblies, string assemblyDefinitionPath)
        {
            // add assemblies to assembly definition
            var assemblyConfig = (AssemblyConfig) JsonUtility.FromJson(assemblyDefinition.text, typeof(AssemblyConfig));

            var references = new HashSet<string>(assemblyConfig.references);

            foreach (var i in assemblies)
            {
                var assemblyName = i.GetName().Name;
                // skip unity assemblies as they are auto referenced
                if (assemblyName.StartsWith("UnityEngine"))
                    continue;
                // skip mscorlib as it is auto referenced
                if (assemblyName == "mscorlib")
                    continue;
                references.Add(assemblyName);
            }

            references.Add("Dffrnt.CoreValues.Runtime");

            assemblyConfig.references = references.ToList();

            System.IO.File.WriteAllText(assemblyDefinitionPath, JsonUtility.ToJson(assemblyConfig, true));
        }

        private void GenerateMethodCommandClasses(Type type, Dictionary<Type, Type> supportedTypeMap, HashSet<Assembly> assemblies, string outputFolder)
        {
            foreach (var method in type.GetMethods())
            {
                if (!IsSupportedMethod(supportedTypeMap, method))
                    continue;

                var model = new SerializedMethodCallModel();
                var template = new SerializedMethodCallTemplate();

                var dependencies = new HashSet<string>();
                dependencies.Add("UnityEngine");
                var parameters = method.GetParameters();

                if (type.Namespace != null)
                    dependencies.Add(type.Namespace);
                if (method.ReturnType.Namespace != null)
                    dependencies.Add(method.ReturnType.Namespace);
                assemblies.Add(type.Assembly);
                assemblies.Add(method.ReturnType.Assembly);
                model.methodIsStatic = method.IsStatic;
                model.methodName = method.Name;
                model.returnTypeName = method.ReturnType == typeof(void) ? "void" : method.ReturnType.Name;
                if (method.ReturnType == typeof(bool))
                    model.baseClass = nameof(IGameObjectCondition);
                else
                    model.baseClass = nameof(IGameObjectCommand);
                model.parameters = new List<(string, string, string)>();
                model.targetTypeName = type.Name;

                foreach (var parameter in parameters)
                {
                    if (!supportedTypeMap.TryGetValue(parameter.ParameterType, out var parameterType))
                        parameterType = parameter.ParameterType;
                    model.parameters.Add((parameterType.Name, parameter.Name, parameter.DefaultValue?.ToString()));

                    dependencies.Add(parameter.ParameterType.Namespace);
                    assemblies.Add(parameter.ParameterType.Assembly);
                }

                model.usings = new List<string>(dependencies);
                model.ns = "Dffrnt.CoreValues.Generated";
                var parameterTypeNames = string.Join("_", parameters.Select(p => p.ParameterType.Name));
                model.className = $"{type.Name}_{method.Name}_{parameterTypeNames}{(parameterTypeNames.Length > 0 ? "_" : string.Empty)}Command";
                model.menuPath = $"{type.Namespace}.{type.Name}";
                model.niceName = $"{type.Name}.{method.Name} ({string.Join(", ", parameters.Select(p => p.Name))})";
                template.model = model;
                var code = template.TransformText();
                var path = $"{outputFolder}/{model.className}.cs";
                System.IO.File.WriteAllText(path, code);
            }
        }

        private bool IsSupportedMethod(Dictionary<Type, Type> supportedTypeMap, MethodInfo method)
        {
            if(method.Name is "BroadcastMessage" or "SendMessage" or "SendMessageUpwards")
                return false;
            if(method.IsSpecialName)
                return false;
            if(!method.IsPublic)
                return false;
            if(method.IsGenericMethod)
                return false;
            if (method.GetCustomAttribute<ObsoleteAttribute>() != null)
                return false;
            var parameterTypes = method.GetParameters();
            foreach(var p in parameterTypes)
            {
                if (p.ParameterType.IsSubclassOf(typeof(UnityEngine.Object)))
                    continue;
                if (p.ParameterType.IsEnum)
                    continue;
                if(!supportedTypeMap.ContainsKey(p.ParameterType))
                    return false;
            }

            if (!(method.ReturnType == typeof(void) || method.ReturnType == typeof(bool)))
                return false;
            
            // if (!supportedTypeMap.ContainsKey(method.ReturnType))
            //     return false;
            

            return true;
        }
    }
    
    

    [Serializable]
    public class AssemblyConfig
    {
        public string name;
        public List<string> references;
        public List<string> includePlatforms;
        public List<string> excludePlatforms;
        public bool overrideReferences;
        public List<string> precompiledReferences;
        public bool autoReferenced;
        public List<string> defineConstraints;
        public List<VersionDefine> versionDefines;
        public bool noEngineReferences;
    }

    [Serializable]
    public class VersionDefine
    {
        public string name;
        public string expression;
        public string define;
    }

}