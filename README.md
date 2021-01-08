# process-visualizer
![Screenshot](docs/screenshot-example.png)

## Get Started
Want to visualize a process involving nodes/components/services which are dynamically connected by arrows?
- Simply build the graph visually as a Unity UI with the `ProcessNode` and `ProcessArrow` prefabs.
- Describe arbitrary processes in JSON format separated from the logic and visualization in Unity.
- Control your process flow with a simple UI.

## Building your process
To create a process on your own, this project provices Unity prefabs in the `ProcessVisualizer` subfolder of the `Assets` folder.\
To get started, simply drag the [`Process` prefab](Process%20Visualizer/Assets/ProcessVisualizer/Prefabs/Process.prefab) into an empty scene (or open the `SampleScene`). This already contains the complete example process which you can view in the editor and also play it.\
Under `Process/ProcessView/Canvas` the whole UI lives and you can modify it to your own process with the following steps.

### 1. Nodes
Here you find a set of abstract nodes which are also instantiated from a prefab and can represent systems, components, services, people or basically anything you can imagine. But for our purposes they are reduced to a simple rectangle (you can modify the visuals as you like) with a name inside it.\
Apart from the visuals, they hold a `ProcessNode` component where 2 variables must be set/connected:
* `Id`: a unique name for this node (later addressed in the scenario description file)
* `Visual`: a link to the visual `Image` component (by default the rectangle) used for highlighting this node
Replace these nodes with your own and arrange them in any way you would like.

### 2. Arrows
Similar to the nodes, these objects are also instances of a prefab and visualize transitions between the nodes.\
They hold a `ProcessArrow` component with following variables:
* `From`: the unique `Id` of the node where this arrow starts
* `To`: the unique `Id` of the node where this arrow ends
* `Image`: a link to the visual `Image` component (by default the arrow sprite) used for highlighting and animating this arrow
* `Annotation Text`: a link to a `TextMeshPro` text element where annotations should be displayed (see below for explanation), leave this empty to use the default annotation label

### 3. Annotations
These are texts being displayed while a transition occurs. To modify the positions of such texts and how many of these labels you need, modify the text elements under `Canvas/Arrows/Annotations` and link the `Annotation Text` variable of each arrow accordingly (as described above).

### 4. Misc
Here you can add any fancy stuff and decoration which has nothing to do with the logic of our process visualization, such as the 2 frames in the example image.

### 5. Controls
These should work without any additional effort with one exception:\
You need to add all scenario files which you would like to select later on in the `Dropdown` component's `Options` under `Canvas/Controls/Scenario/Dropdown`. Name each option like the corresponding file without the `.json` extension.

### 6. Setup description files
For each scenario you want to visualize, provide a description file in the format specified below and save it under `Assets/Resources/`.

## JSON Description
The `ScenarioController` can read different scenario description files which have to follow a certain format.\
For an example which can be used as template, please see [example.json](Process%20Visualizer/Assets/Resources/example.json).\
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
