using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dffrnt.CoreValues
{
    [CustomEditor(typeof(GameEvent), true)]
    public class GameEventEditor : Editor
    {
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            // Add default inspector
            var defaultInspector = new IMGUIContainer(() => base.OnInspectorGUI());
            root.Add(defaultInspector);

            // Add Fire button
            var fireButton = new Button(() =>
            {
                var gameEvent = target as GameEvent;
                GameEvent.Trigger(gameEvent);
            })
            {
                text = "Fire"
            };
            fireButton.SetEnabled(Application.isPlaying);
            root.Add(fireButton);

            return root;
        }
    }
}