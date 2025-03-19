using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dffrnt.CoreValues
{
    [CustomEditor(typeof(GameEvent), true)]
    public class GameEventEditor : Editor
    {
        private DependencyTracker tracker = new DependencyTracker();
        
        private void OnEnable()
        {
            tracker.Track(target);
            tracker.OnHierarchyResultsChanged += Repaint;
            tracker.OnProjectResultsChanged += Repaint;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var e = target as GameEvent;
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Raise"))
            {
                e.Invoke();
            }

            GUI.enabled = true;
            GUILayout.Space(16);
            tracker.DrawResults();
        }
    }
}