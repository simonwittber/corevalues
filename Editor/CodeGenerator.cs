using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreValues.Editor;
using Dffrnt.CoreValues;
using UnityEditor;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [Serializable]
    internal class CodeGeneratedType
    {
        [TypeToStringDropdown(typeof(Component))]
        public string typeName;
        public bool useDerivedTypes = false;
        
    }
    
    [CreateAssetMenu(menuName = "Core/Code Generator", fileName = "CodeGenerator")]
    internal class CodeGenerator : ScriptableObject
    {
        public string outputFolder;
        public List<CodeGeneratedType> scripts = new ();
        

        [ContextMenu("OnEnable")]
        private void OnEnable()
        {
            foreach (var script in scripts)
            {
                
            }
        }
        
        [ContextMenu("Generate")]
        private void Generate()
        {
            var supportedTypes = TypeCache.GetTypesWithAttribute<CoreValueTypeAttribute>();
            var supportedTypeMap = supportedTypes.ToDictionary(t => t.GetCustomAttribute<CoreValueTypeAttribute>().type);
            
            foreach (var script in scripts)
            {
                var type = Type.GetType(script.typeName);
                if (type == null)
                {
                    Debug.LogWarning($"Type {script.typeName} not found");
                    continue;
                }
                if(type.IsAbstract)
                    continue;
                if(type.IsGenericType)
                    continue;
                if(type.IsInterface)
                    continue;
                
                foreach(var method in type.GetMethods())
                {
                    if(!IsSupportedMethod(supportedTypeMap, method))
                        continue;
                    
                    var model = new SerializedMethodCallModel();
                    var smct = new SerializedMethodCallTemplate();
                   
                    var dependencies = new HashSet<string>();
                    dependencies.Add("UnityEngine");
                    var parameters = method.GetParameters();
                    
                    dependencies.Add(type.Namespace);
                    dependencies.Add(method.ReturnType.Namespace);
                    model.methodIsStatic = method.IsStatic;
                    model.methodName = method.Name;
                    model.returnTypeName = method.ReturnType == typeof(void)?"void":method.ReturnType.Name;
                    model.parameters = new List<(string, string)>();
                    model.targetTypeName = type.Name;
                    
                    foreach (var parameter in parameters)
                    {
                        if(!supportedTypeMap.TryGetValue(parameter.ParameterType, out var parameterType))
                            parameterType = parameter.ParameterType;
                        model.parameters.Add((parameterType.Name, parameter.Name));
                        
                        dependencies.Add(parameter.ParameterType.Namespace);
                    }
                    model.usings = new List<string>(dependencies);
                    model.ns = "Dffrnt.CoreValues.Generated";
                    var parameterTypeNames = string.Join("_", parameters.Select(p => p.ParameterType.Name));
                    model.className = $"{type.Name}_{method.Name}_{parameterTypeNames}{(parameterTypeNames.Length>0?"_":string.Empty)}Command"; 
                    model.menuPath = $"{type.Namespace}.{type.Name}";
                    model.niceName = $"{type.Name}.{method.Name} ({string.Join(", ",parameters.Select(p => p.Name))})";
                    smct.Model = model;
                    var code = smct.TransformText();
                    var path = $"{outputFolder}/{model.className}.cs";
                    System.IO.File.WriteAllText(path, code);
                }
                
            }
            AssetDatabase.Refresh();
        }

        private bool IsSupportedMethod(Dictionary<Type, Type> supportedTypeMap, MethodInfo method)
        {
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

            if (method.ReturnType != typeof(void))
                return false;
            
            // if (!supportedTypeMap.ContainsKey(method.ReturnType))
            //     return false;
            

            return true;
        }
    }
}