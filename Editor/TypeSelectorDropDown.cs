using System;
using System.Collections.Generic;
using System.Linq;
using Dffrnt.CoreValues;
using OdinSerializer.Utilities;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Dffrnt.CoreValues
{
    public class TypeSelectorDropDown : AdvancedDropdown
    {
        public List<System.Type> allTypes;

        public TypeSelectorDropDown(AdvancedDropdownState state, System.Type[] allTypes) : base(state)
        {
            this.allTypes = allTypes.ToList();
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("Types");
            root.AddChild(new AdvancedDropdownItem("None") {id = -1});
            
            var namespaces = allTypes.Select(t =>
            {
                var path = t.GetAttribute<MenuPathAttribute>()?.path ?? t.Namespace;
                var niceName = t.GetAttribute<NiceNameAttribute>()?.name ?? t.Name;
                return (path, niceName, t);
            }).ToList();
            
            namespaces.Sort((a, b) =>
            {
                var ns = String.Compare(a.path, b.path, StringComparison.Ordinal);
                if (ns != 0) return ns;
                return String.Compare(a.niceName, b.niceName, StringComparison.Ordinal);
            });

            foreach(var (path, niceName, type) in namespaces)
            {
                var ns = path ?? "GLOBAL";
                var parts = ns.Split(".");
                var current = root;
                foreach (var part in parts)
                {
                    var child = current.children.FirstOrDefault(c => c.name == part);
                    if (child == null)
                    {
                        child = new AdvancedDropdownItem(part);
                        current.AddChild(child);
                    }

                    current = child;
                }
                current.AddChild(new AdvancedDropdownItem(niceName) {id = allTypes.IndexOf(type)});
            }
            
            
            // var allNamespaces = new Dictionary<string, AdvancedDropdownItem>();
            // foreach (var type in allTypes)
            // {
            //     var ns = type.Namespace??"GLOBAL";
            //     if (!allNamespaces.ContainsKey(ns))
            //     {
            //         var nsItem = new AdvancedDropdownItem(ns) {id = -1};
            //         allNamespaces.Add(ns,nsItem);
            //         root.AddChild(nsItem);
            //     }
            // }
            //
            //
            // for (var index = 0; index < allTypes.Count; index++)
            // {
            //     var type = allTypes[index];
            //     var ns = type.Namespace??"GLOBAL";
            //     var name = type.GetAttribute<NiceNameAttribute>()?.name ?? type.Name;
            //     allNamespaces[ns].AddChild(new AdvancedDropdownItem(name) { id = index });
            // }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            callback?.Invoke(item.id==-1?null:allTypes[item.id]);
        }
        
        public Action<System.Type> callback;
    }
}