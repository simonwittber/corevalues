using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Dffrnt.CoreValues
{
    public class DependencyTracker
    {
        public readonly List<SearchItem> hierarchyResults = new List<SearchItem>();
        public readonly List<SearchItem> projectResults = new List<SearchItem>();
        
        public Action OnHierarchyResultsChanged, OnProjectResultsChanged;

        public void Track(Object target)
        {
            var gid = GlobalObjectId.GetGlobalObjectIdSlow(target);
            SearchService.Request($"h:ref={gid}", OnHierarchySearchCompleted);
            SearchService.Request($"p:ref={gid}", OnProjectSearchCompleted);
        }

        private void OnProjectSearchCompleted(SearchContext context, IList<SearchItem> results)
        {
            projectResults.Clear();
            projectResults.AddRange(results);
            OnHierarchyResultsChanged?.Invoke();
        }

        private void OnHierarchySearchCompleted(SearchContext context, IList<SearchItem> results)
        {
            hierarchyResults.Clear();
            hierarchyResults.AddRange(results);
            OnHierarchyResultsChanged?.Invoke();
        }

        public void DrawResults()
        {
            GUILayout.Label("Dependencies", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            GUILayout.Label("Hierarchy");
            if (hierarchyResults.Count == 0)
                GUILayout.Label("No dependencies found");
            foreach (var i in hierarchyResults)
            {
                var o = i.ToObject();
                EditorGUILayout.ObjectField(o, o.GetType(), true);
            }

            GUILayout.Space(16);
            GUILayout.Label("Project");
            if(projectResults.Count == 0)
                GUILayout.Label("No dependencies found");
            foreach (var d in projectResults)
            {
                var o = d.ToObject();
                EditorGUILayout.ObjectField(o, o.GetType(), true);
            }
            EditorGUI.indentLevel--;
        }
    }
}