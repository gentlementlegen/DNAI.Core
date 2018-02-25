using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Plugin.Unity.Editor
{
    [Serializable]
    public class SettingsClassBuilder : ScriptableObject
    {
        [SerializeField]
        public Dictionary<string, object> Properties = new Dictionary<string, object>();

        [SerializeField]
        public List<AUnitySerializable> Objects = new List<AUnitySerializable>();
    }

    [Serializable]
    public class AUnitySerializable
    {
        [SerializeField]
        public string Test = "";
    }

    [Serializable]
    public class UnityObject : AUnitySerializable
    {
        [SerializeField]
        public string TestChildren = "";
    }
}