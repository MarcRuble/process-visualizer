using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProcessVisualizer
{
    public class ProcessNode : MonoBehaviour
    {
        public string id;
        public Image visual;

        private Context currentContext = Context.EMPTY;

        // Start is called before the first frame update
        void Start()
        {
            if (visual == null)
                visual = GetComponent<Image>();

            Reset();
        }

        public void Reset()
        {
            visual.color = Parameters.instance.standard;
            currentContext = Context.EMPTY;
        }

        public void StartHighlight(Context context)
        {
            visual.color = Parameters.instance.highlight;
            currentContext = context;
        }

        public void EndHighlight(Context context)
        {
            // avoid overriding of active components
            if (currentContext.IsEmpty() || currentContext.Equals(context)
                || currentContext.priority < context.priority)
            {
                visual.color = Parameters.instance.standard;
                currentContext = Context.EMPTY;
            }
        }
    }
}
