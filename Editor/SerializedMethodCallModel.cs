using System.Collections;
using System.Collections.Generic;

namespace Dffrnt.CoreValues
{
    public class SerializedMethodCallModel
    {
        public string className = "GeneratedClass";
        public string ns = "Dffrnt.CoreValues";
        public List<(string,string)> parameters = new List<(string, string)>();
        public string targetTypeName = "UnityEngine.Debug";
        public string returnTypeName = "void";
        public string methodName = "Log";
        public List<string> usings = new List<string>();
        public string menuPath = "GameObject/Debug/Log";
        public bool methodIsStatic;
        public string niceName;
    }
}