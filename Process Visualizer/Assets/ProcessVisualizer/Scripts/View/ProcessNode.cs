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
        }

        public void StartHighlight(Context context)
        {
            visual.color = Parameters.instance.highlight;
        }

        public void EndHighlight(Context context)
        {
            visual.color = Parameters.instance.standard;
        }
    }
}
