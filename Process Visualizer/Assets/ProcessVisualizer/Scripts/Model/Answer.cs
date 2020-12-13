using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcessVisualizer
{
    [System.Serializable]
    public class Answer
    {
        public string id;
        public float delay;
        public int priority;
        public float durationFrom;
        public float durationTo;
        public float durationArrow;
        public string annotation;
        public string color;
    }
}
