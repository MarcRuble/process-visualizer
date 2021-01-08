using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcessVisualizer
{
    public class Parameters : MonoBehaviour
    {
        public float arrowAnimationDuration = 0.1f;
        public Color standard;
        public Color highlight;
        public Color background;
        public Color blue;
        public Color red;
        public Color green;
        public Color grey;
        public Color magenta;
        public Color yellow;
        public Color orange;
        public Color black;
        public Color white;

        public static Parameters instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Debug.LogError("[INIT] instance already defined by " + instance.gameObject.name);
        }

        public Color StringToColor(string s)
        {
            switch (s)
            {
                case "blue":
                    return blue;
                case "red":
                    return red;
                case "green":
                    return green;
                case "grey":
                    return grey;
                case "magenta":
                    return magenta;
                case "yellow":
                    return yellow;
                case "orange":
                    return orange;
                case "black":
                    return black;
                case "white":
                    return white;
                default:
                    return blue;
            }
        }
    }
}
