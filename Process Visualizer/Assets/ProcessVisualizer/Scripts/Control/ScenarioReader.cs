using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcessVisualizer
{
    public class ScenarioReader
    {
        public static Scenario Read(string name)
        {
            var jsonTextFile = Resources.Load<TextAsset>(name);
            return JsonUtility.FromJson<Scenario>(jsonTextFile.text);
        }
    }
}
