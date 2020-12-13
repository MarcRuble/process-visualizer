using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcessVisualizer
{
    public struct Context
    {
        public string id;
        public int priority;
        public static Context EMPTY = new Context("", 0);

        public Context(string id, int priority)
        {
            this.id = id;
            this.priority = priority;
        }

        public bool IsEmpty()
        {
            return this.Equals(EMPTY);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType()))
                return false;

            Context other = (Context)obj;
            return id.Equals(other.id);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
