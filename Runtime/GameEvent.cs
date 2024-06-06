// Decompiled with JetBrains decompiler
// Type: Dffrnt.Core.GameEvent
// Assembly: com.dffrnt.core.runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 47BBACE6-DB48-4708-89A5-9EBC6DAF03B9
// Assembly location: C:\Users\simon\Desktop\dc\com.dffrnt.core.runtime.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Dffrnt.Core
{
  [CreateAssetMenu]
  public class GameEvent : ScriptableObject, IInvokable
  {
    private readonly HashSet<IInvokable> _listeners = new HashSet<IInvokable>();

    public void Invoke()
    {
      foreach (var listener in _listeners)
        listener.Invoke();
    }

    public void Register(IInvokable listener) => _listeners.Add(listener);

    public void Unregister(IInvokable listener) => _listeners.Remove(listener);

    public static void Trigger(IInvokable ev) => ev?.Invoke();

  }
}
