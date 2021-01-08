using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Subsystems;
using UnityEngine.UI;
using TMPro;

namespace ProcessVisualizer
{
    public class ScenarioController : MonoBehaviour
    {
        #region PUBLIC FIELDS
        public float defaultTimeMultiplier = 1f;
        public float timePadding = 1f;
        public TMP_Dropdown scenarioDropdown;
        public TextMeshProUGUI pauseText;
        public Slider slider;
        public Toggle toggle;

        public static ScenarioController instance;
        #endregion

        #region PRIVATE FIELDS
        private List<Transition> transitions;
        private List<Transition> transitionStarts;
        private ScenarioView scenarioView;
        private Timer timer;
        private float speed = 1f;
        private bool loop = false;
        private float maxTime = 10f;
        #endregion

        #region UNITY INTERFACE
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Debug.LogError("[INIT] instance already defined by " + instance.gameObject.name);
        }

        private void Start()
        {
            scenarioView = ScenarioView.instance;
            Reset();
            SetSpeed();
        }

        private void Update()
        {
            if (timer == null)
                return;

            if (timer.GetTime() > maxTime)
            {
                if (loop)
                    StartScenario();
                else
                    return;
            }

            // half of animation duration
            float duration = Parameters.instance.arrowAnimationDuration / 2f;

            // handle starts of the transition animation
            while (transitionStarts.Count > 0 && transitionStarts[0].time - duration <= timer.GetTime())
            {
                StartTransition(transitionStarts[0]);
                transitionStarts.RemoveAt(0);
            }

            // handle the actual transitions
            while (transitions.Count > 0 && transitions[0].time + duration <= timer.GetTime())
            {
                ApplyTransition(transitions[0]);
                transitions.RemoveAt(0);
            }
        }
        #endregion

        #region PUBLIC METHODS
        public void LoadScenario()
        {
            // reset current state
            Reset();

            // read the new scenario
            Scenario scenario = ScenarioReader.Read(
                scenarioDropdown.options[scenarioDropdown.value].text);

            // create a list out of the scenario
            transitions = new List<Transition>(scenario.transitions);

            // determine highest time value plus padding
            maxTime = GetHighestTime(transitions) + timePadding;

            // unfold the repetitions by copying transitions
            transitions = UnfoldRepetitions(transitions, maxTime);

            // unfold the answers by copying them as separate transitions
            transitions = UnfoldAnswers(transitions);

            // sort the list on time
            transitions.Sort((t1, t2) => t1.time.CompareTo(t2.time));

            // create separate copy for managing of animation starts
            transitionStarts = new List<Transition>(transitions);
        }

        public void StartScenario()
        {
            scenarioView.Reset();
            LoadScenario();
            timer = new Timer();
        }

        public void PauseScenario()
        {
            if (Time.timeScale > 0f)
            {
                Time.timeScale = 0f;
                pauseText.text = "Resume";
            }
            else
            {
                Time.timeScale = speed;
                pauseText.text = "Pause";
            }
        }

        public void SetSpeed()
        {
            float speed = slider.value;
            this.speed = speed;
            Time.timeScale = defaultTimeMultiplier * speed;
        }

        public void SetLoop()
        {
            loop = toggle.isOn;
        }
        #endregion

        #region HELPER METHODS
        private void Reset()
        {
            transitions = new List<Transition>();
            transitionStarts = new List<Transition>();
        }

        // Starts the arrow animation.
        private void StartTransition(Transition t)
        {
            // activate the arrow
            scenarioView.ActivateArrow(t.from, t.to, t.GetContext(), t.annotation, t.color);
        }

        // Actually applies the transition with its consequences.
        private void ApplyTransition(Transition t)
        {
            // first hightlight the receiving node
            scenarioView.StartHighlightNode(t.to, t.GetContext());

            // and start a coroutine to end this effect
            if (t.durationTo >= 0f)
                StartCoroutine(
                    DelayEndHighlightNode(t.durationTo, t.to, t.GetContext()));

            // check if outgoing node is modified
            if (t.durationFrom < 0f)
            {
                scenarioView.StartHighlightNode(t.from, t.GetContext());
            } 
            else if (t.durationFrom > 0f)
            {
                StartCoroutine(
                    DelayEndHighlightNode(t.durationFrom, t.from, t.GetContext()));
            }

            // start coroutine to end arrow
            StartCoroutine(
                DelayDeactivateArrow(t.durationArrow, t.from, t.to, t.GetContext()));
        }
        #endregion

        #region COROUTINES
        private IEnumerator DelayEndHighlightNode
            (float delay, string nodeID, Context context)
        {
            yield return new WaitForSeconds(delay + Parameters.instance.arrowAnimationDuration);
            scenarioView.EndHighlightNode(nodeID, context);
        }

        private IEnumerator DelayDeactivateArrow
            (float delay, string from, string to, Context context)
        {
            yield return new WaitForSeconds(delay);
            scenarioView.DeactivateArrow(from, to, context);
        }
        #endregion

        #region STATIC METHODS
        private static float GetHighestTime(List<Transition> transitions)
        {
            return transitions.Max(t => t.time);
        }

        private static List<Transition> UnfoldRepetitions(List<Transition> transitions, float maxTime)
        {
            List<Transition> unfold = new List<Transition>();

            foreach (Transition t in transitions)
            {
                float nextTime = t.time;

                // repeat until max is reached
                while (nextTime <= maxTime)
                {
                    unfold.Add(new Transition(t, nextTime));
                    nextTime += t.repeatFreq;

                    if (t.repeatFreq == 0f)
                        break;
                }
            }
            return unfold;
        }

        private static List<Transition> UnfoldAnswers(List<Transition> transitions)
        {
            List<Transition> unfold = new List<Transition>();

            foreach (Transition t in transitions)
            {
                unfold.Add(t);

                if (t.answer.id != null && t.answer.id.Length > 0)
                    unfold.Add(new Transition(t));
                    
            }
            return unfold;
        }
        #endregion
    }
}
