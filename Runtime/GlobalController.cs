using System;
using UnityEngine;

namespace Dffrnt.CoreValues
{
    [CreateAssetMenu(menuName = "CoreValues/GlobalController")]
    public class GlobalController : ScriptableObject
    {
        private void OnEnable()
        {
            Debug.Log("GlobalController enabled");
        }
        
        private void OnDisable()
        {
            Debug.Log("GlobalController disabled");
        }
    }
    
}