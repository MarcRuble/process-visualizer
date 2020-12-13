using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcessVisualizer
{
    [System.Serializable]
    public class Transition
    {
        public string id;
        public float time;
        public float repeatFreq;
        public int priority;
        public string from;
        public string to;
        public float durationFrom;
        public float durationTo;
        public float durationArrow;
        public string annotation;
        public string color;
        public Answer answer;

        // Constructs a transition out of t's answer.
        public Transition(Transition t)
        {
            id = t.answer.id;
            time = t.time + t.answer.delay;
            repeatFreq = t.repeatFreq;
            priority = t.answer.priority;
            from = t.to;
            to = t.from;
            durationFrom = t.answer.durationFrom;
            durationTo = t.answer.durationTo;
            durationArrow = t.answer.durationArrow;
            annotation = t.answer.annotation;
            color = t.answer.color;
        }

        public Transition(Transition t, float time)
        {
            id = t.id;
            this.time = time;
            repeatFreq = t.repeatFreq;
            priority = t.priority;
            from = t.from;
            to = t.to;
            durationFrom = t.durationFrom;
            durationTo = t.durationTo;
            durationArrow = t.durationArrow;
            annotation = t.annotation;
            color = t.color;
            answer = t.answer;
        }

        public Context GetContext()
        {
            return new Context(id, priority);
        }
    }
}
