using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcessVisualizer
{
    public class ScenarioView : MonoBehaviour
    {
        #region PUBLIC FIELDS
        public static ScenarioView instance;
        #endregion

        #region PRIVATE FIELDS
        private Dictionary<string, ProcessNode> nodes;
        private Dictionary<(string, string), ProcessArrow> arrows;
        #endregion

        #region UNITY INTERFACE
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Debug.LogError("[INIT] instance already defined by " + instance.gameObject.name);

            nodes = Utils.CreateDictFromChildren<string, ProcessNode>(this, n => n.id);
            arrows = Utils.CreateDictFromChildren<string, string, ProcessArrow>(this, a => (a.from, a.to));
        }

        private void Start()
        {
            
        }

        private void Update()
        {

        }
        #endregion

        #region PUBLIC METHODS
        public void Reset()
        {
            foreach (ProcessNode n in nodes.Values)
                n.Reset();

            foreach (ProcessArrow a in arrows.Values)
                a.Reset();
        }

        public void StartHighlightNode(string node, Context context)
        {
            Debug.Log("<b>" + node + "</b>");
            nodes[node].StartHighlight(context);
        }

        public void EndHighlightNode(string node, Context context)
        {
            Debug.Log(node);
            nodes[node].EndHighlight(context);
        }

        public void ActivateArrow(string from, string to, Context context, 
            string annotation, string color)
        {
            Debug.Log(from + " --" + annotation + "|" + color + "--> " + to);
            if (!from.Equals(to))
                arrows[(from, to)].Activate(context, annotation, color);
        }

        public void DeactivateArrow(string from, string to, Context context)
        {
            Debug.Log(from + "  " + to);
            if (!from.Equals(to))
                arrows[(from, to)].Deactivate(context);
        }
        #endregion

        #region HELPER METHODS

        #endregion
    }
}
