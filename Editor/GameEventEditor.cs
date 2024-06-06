using System;
using UnityEditor;
using UnityEngine;

namespace Dffrnt.Core
{
  [CustomEditor(typeof (GameEvent), true)]
  public class GameEventEditor : Editor
  {
    public virtual void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      GUI.enabled = Application.isPlaying;
      GameEvent target = this.target as GameEvent;
      if (!GUILayout.Button("Raise", Array.Empty<GUILayoutOption>()))
        return;
      GameEvent.Trigger((IInvokable) target);
    }
  }
}
