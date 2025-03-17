using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    public class GeneratedClass
    {
        public string name;
        public string _namespace;
        public List<string> usings = new List<string>();
        public List<FieldInfo> fields = new List<FieldInfo>();
        public List<PropertyInfo> properties = new List<PropertyInfo>();
        
        public void Generate()
        {
            var code = $"namespace {_namespace}\n{{\n";
            foreach (var u in usings)
            {
                code += $"using {u};\n";
            }
            code += $"public class {name}\n{{\n";
            foreach (var f in fields)
            {
                foreach (var a in f.GetCustomAttributes(true))
                {
                    var attributeType = a.GetType();
                    var attributeArgs = attributeType.GetConstructors()
                        .FirstOrDefault()?
                        .GetParameters()
                        .Select(p => p.ParameterType)
                        .ToArray();

                    var args = attributeArgs != null && attributeArgs.Length > 0
                        ? string.Join(", ", attributeArgs.Select(arg => 
                        {
                            var property = a.GetType().GetProperty(arg.Name);
                            var value = property?.GetValue(a);
                            return value != null ? value.ToString() : $"default({arg.Name})";
                        }))
                        : string.Empty;

                    code += $"[{attributeType.Name}({args})]\n";
                }
                code += $"public {f.FieldType.Name} {f.Name};\n";
            }
            foreach (var f in properties)
            {
                foreach (var a in f.GetCustomAttributes(true))
                {
                    var attributeType = a.GetType();
                    var attributeArgs = attributeType.GetConstructors()
                        .FirstOrDefault()?
                        .GetParameters()
                        .Select(p => p.ParameterType)
                        .ToArray();

                    var args = attributeArgs != null && attributeArgs.Length > 0
                        ? string.Join(", ", attributeArgs.Select(arg => 
                        {
                            var property = a.GetType().GetProperty(arg.Name);
                            var value = property?.GetValue(a);
                            return value != null ? value.ToString() : $"default({arg.Name})";
                        }))
                        : string.Empty;

                    code += $"[{attributeType.Name}({args})]\n";
                }
                code += $"public {f.PropertyType.Name} {f.Name};\n";
            }
            code += "}\n";
            code += "}\n";
            Debug.Log(code);
        }
    }
}