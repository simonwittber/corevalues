using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [CustomEditor(typeof(GenericObject), true)]
    public class GenericValueEditor : Editor
    {
        DependencyTracker tracker = new DependencyTracker();
        
        private void OnEnable()
        {
            tracker.Track(target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(16);
            tracker.DrawResults();
        }
    }
}