## SendEventToFSM.cs
![Image](/Screenshots/Components/SendEventToFSM_Info.png)

Send an OnStart-, OnClick-, OnPressUp- and/or OnPressDown-Event to the specified FSM, optionally set the value of an existing GameObject-Variable in that FSM to the GameObject this script is attached to, beforehand (to make checks where the signal came from, or use that one further, in the Target-FSM).

![Image](/Screenshots/Components/SendEventToFSM_Info2.png)

The second image shows what the Target-FSM needs: The specified global Event(s) with matching name(s) and optionally the pre-existing variable, also with exact matching names.
The GameObject this component is attached to also need a collider of any kind to register the click happening.

Very useful to save a lot of FSM-Components (because they can add ~0.05s to loading times per FSM), 
especially when you have a lot of similar GameObjects or instantiate a lot of prefabs (in one project I cut down aprox. 7000 FSM with this quite light-weight script), 
since with this you don't need an FSM for every button but can have one global FSM for all children.

## SendEventToFSMAdvanced.cs
![Image](/Screenshots/Components/SendEventToFSMAdvanced_Info.png)

```
Requires the SendEventToFSMInspector-Script in the Editor-Folder
```

An extended version of `SendEventToFSM` with the following additions:
1. You can specify the FSM-Name if several FSM's coexist on the Target-FSM;
2. You can choose any variable-type the selected variable in the Target-FSM has (except Quaternions, Arrays and Enums);
3. You can optionally set a PlayerPrefs to retrieve that information in the Target-FSM (expands the field for the PlayerPrefs-Value once you type in a Key);

Not as light-weight as the base version, so best to use it more scarcely.