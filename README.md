# process-visualizer
![Screenshot](docs/screenshot-example.png)

## Get Started
Want to visualize a process involving nodes/components/services which are dynamically connected by arrows?
- Simply build the graph visually as a Unity UI with the `ProcessNode` and `ProcessArrow` prefabs.
- Describe arbitrary processes in JSON format separated from the logic and visualization in Unity.
- Control your process flow with a simple UI.

## Building your process
TODO

## JSON Description
The `ScenarioController` can read different scenario description files which have to follow a certain format.\
At high level, we have a JSON object containing a list of transitions:
```
{
  "transitions":
  [
      {
        ...
      },
      {
        ...
      },
      ...
  ]
}
```
Each of these transitions is described by a number of parameters:
```
{
  "id": "A -> B",     // unique identifier
  "time": 1,          // when it takes place (in seconds)
  "repeatFreq": 1,    // repeat every 1s
                      // 0 = no repetition
  "priority": 0,      // lowest priority
  "from": "A",        // start of transition
  "to": "B",          // end of transition
  "durationFrom": 0,  // A is not highlighted
  "durationTo": 2,    // B highlighted for 2s
  "durationArrow": 1, // arrow visible for 1s
  "annotation": "1",  // annotation for this transition
  "color": "blue",    // color of arrow
  "answer":
  {
    "id": "B -> A",       // unique identifier
    "delay": 1,           // 1s after original transition
    "priority": 1,        // higher priority
    "durationFrom": 0.5,  // B highlighted for 0.5s
    "durationTo": -1,     // A highlighted "forever"
    "durationArrow": 0.5, // arrow visible for 0.5s
    "annotation": "3",    // annotation for answering transition
    "color": "blue"       // color of arrow
  }
}
```
*Important notes:*
- Comments are not part of JSON format and thus not allowed to be in the file.
- In `answer` the roles of A and B are switched, so B is now `from`.
- `duration` of 0 means no activation/highlighting will be done while -1 means unlimited activation/highlighting.
- Starting states can be defined as a transition from A to A which will not be present in the visualization but can highlight nodes at arbitrary points.
