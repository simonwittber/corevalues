using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [CustomEditor(typeof(GenericObject), true)]
    public class GenericValueEditor : Editor
    {
        private List<Object> dependencies = new List<Object>();
        private List<SearchItem> searchResults = new List<SearchItem>();
        private void OnEnable()
        {
            dependencies.Clear();
            var dependents = new Object[] {target};
            dependencies.AddRange(EditorUtility.CollectDependencies(dependents));
            var gid = GlobalObjectId.GetGlobalObjectIdSlow(target);
            SearchService.Request($"h:ref={gid}", OnSearchCompleted);

        }

        private void OnSearchCompleted(SearchContext context, IList<SearchItem> results)
        {
            searchResults.Clear();
            searchResults.AddRange(results);
        }

        private void OnDisable()
        {
            dependencies.Clear();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(16);
            GUILayout.Label("Hierarchy");
            foreach (var i in searchResults)
            {
                var o = i.ToObject();
                EditorGUILayout.ObjectField(o, o.GetType(), true);
            }

            GUILayout.Space(16);
            GUILayout.Label("Project");
            foreach(var d in dependencies)
                EditorGUILayout.ObjectField(d, d.GetType(), true);
        }
    }
}