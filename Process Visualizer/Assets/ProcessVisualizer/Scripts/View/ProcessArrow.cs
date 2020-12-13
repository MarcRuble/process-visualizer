using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProcessVisualizer
{
    public class ProcessArrow : MonoBehaviour
    {
        public string from;
        public string to;
        public Image image;

        private Vector3 startPosition;
        private Vector2 startPivot;
        private Context currentContext = Context.EMPTY;
        private bool reset = false;

        // Start is called before the first frame update
        void Start()
        {
            startPosition = image.rectTransform.position;
            startPivot = image.rectTransform.pivot;
            Reset();
        }

        public void Reset()
        {
            image.rectTransform.pivot = startPivot;
            image.rectTransform.position = startPosition;
            image.transform.localScale = Vector3.zero;
            reset = true;
        }

        public void Activate(Context context, string annotation, string color)
        {
            Reset();
            reset = false;
            
            // if there is no current context or the object is deactivated
            // or the new context overrides the old one
            if (currentContext.IsEmpty() || !image.gameObject.activeSelf
                || context.Equals(currentContext) 
                || context.priority > currentContext.priority)
            {
                // apply changes
                image.color = Parameters.instance.StringToColor(color);
                StartCoroutine(ScaleArrow(true));
            }
        }

        public void Deactivate(Context context)
        {
            Reset();
            reset = false;
            
            if (currentContext.IsEmpty() 
                || context.Equals(currentContext)
                || context.priority > currentContext.priority)
            {
                StartCoroutine(ScaleArrow(false));
            }
        }

        private IEnumerator ScaleArrow(bool grow)
        {
            float start = Time.time;
            Vector3 gone = new Vector3(1f, 0f, 1f);
            Vector3 there = Vector3.one;

            // scale to start or end of arrow
            if (!grow)
            {
                RectTransform rt = image.rectTransform;
                rt.position = rt.position + rt.up * rt.rect.height;
                rt.pivot = new Vector2(0.5f, 1f);
            }

            float duration = Parameters.instance.arrowAnimationDuration;

            while (!reset && Time.time - start < duration)
            {
                // interpolate scaling
                image.transform.localScale = grow
                    ? Vector3.Lerp(gone, there, (Time.time - start) / duration)
                    : Vector3.Lerp(there, gone, (Time.time - start) / duration);
                yield return null;
            }
            image.transform.localScale = grow ? there : gone;
        }
    }
}
