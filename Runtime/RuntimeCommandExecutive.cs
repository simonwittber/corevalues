using UnityEngine;
using UnityEngine.LowLevel;

namespace Dffrnt.CoreValues
{
    public static class RuntimeCommandExecutive
    {
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            Debug.Log("RuntimeCommandScheduler Init");
            var loop = PlayerLoop.GetDefaultPlayerLoop();
            // if we can find this loop, exit.
            for (int i = 0; i < loop.subSystemList.Length; i++)
            {
                if (loop.subSystemList[i].type == typeof(RuntimeCommandScheduler))
                {
                    Debug.Log("RuntimeCommandScheduler already installed. Exiting.");
                    return;
                }
            }
            
            // find the Update loop
            for (int i = 0; i < loop.subSystemList.Length; i++)
            {
                if (loop.subSystemList[i].type == typeof(UnityEngine.PlayerLoop.Update))
                {
                    var subSystem = loop.subSystemList[i];
                    // insert this custom update loop at the beginning
                    var newSubSystemList = new PlayerLoopSystem[subSystem.subSystemList.Length + 1];
                    newSubSystemList[0] = new PlayerLoopSystem
                    {
                        type = typeof(RuntimeCommandScheduler),
                        updateDelegate = Tick
                    };
                    subSystem.subSystemList.CopyTo(newSubSystemList, 1);
                    subSystem.subSystemList = newSubSystemList;
                    loop.subSystemList[i] = subSystem;
                    Debug.Log("Installing RuntimeCommandScheduler");
                    break;
                }
            }
            PlayerLoop.SetPlayerLoop(loop);
            ServiceLocatorBindings.BindAll();
            Debug.Log(Resources.Load("Objects/Global Controller"));
        }

        private static void Tick()
        {
            // Debug.Log("Tick");
        }

    }
}