using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcessVisualizer
{
    public class Timer
    {
        private float start;

        public Timer()
        {
            this.start = Time.time;
        }

        public float GetTime()
        {
            return Time.time - start;
        }
    }
}
