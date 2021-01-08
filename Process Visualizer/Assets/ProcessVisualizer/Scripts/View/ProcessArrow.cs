using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProcessVisualizer
{
    public class ProcessArrow : MonoBehaviour
    {
        public string from;
        public string to;
        public Image image;
        public TextMeshProUGUI annotationText;

        private Vector3 startPosition;
        private Vector2 startPivot;
        private float startHeight;
        private float scaleFactor;
        private Context currentContext = Context.EMPTY;
        private bool reset = false;

        // Start is called before the first frame update
        void Start()
        {
            CreateStaticClone();
            startPosition = image.rectTransform.position;
            startPivot = image.rectTransform.pivot;
            startHeight = image.rectTransform.rect.height;
            scaleFactor = image.rectTransform.lossyScale.y;
            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);

            if (annotationText == null)
                annotationText = transform.parent.Find(
                    "Annotations/MainAnnotation/Annotation").GetComponent<TextMeshProUGUI>();

            Reset();
        }

        public void Reset()
        {
            image.rectTransform.pivot = startPivot;
            image.rectTransform.position = startPosition;
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
                currentContext = context;
                annotationText.text = annotation;
                annotationText.color = image.color;
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

            // scale to start or end of arrow
            if (!grow)
            {
                RectTransform rt = image.rectTransform;
                rt.position = rt.position + rt.up * scaleFactor * rt.rect.height;
                rt.pivot = new Vector2(0.5f, 1f);
            }

            float duration = Parameters.instance.arrowAnimationDuration;

            while (!reset && Time.time - start < duration)
            {
                float progress = (Time.time - start) / duration;

                if (grow)
                    image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 
                        startHeight * progress);
                else
                    image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                        startHeight * (1f - progress));
                
                yield return null;
            }
            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                grow ? startHeight : 0f);

            if (!grow)
                currentContext = Context.EMPTY;
        }

        private void CreateStaticClone()
        {
            GameObject clone = Instantiate(gameObject, transform.parent);
            ProcessArrow clonedArrow = clone.GetComponent<ProcessArrow>();
            clonedArrow.image.color = Parameters.instance.background;
            DestroyImmediate(clonedArrow);
        }
    }
}
