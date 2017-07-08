## SendEventToFSM: 
![alt text](http://imgur.com/xe4EkUG)	![alt text](http://imgur.com/CjOyFOl)
Send an OnClick-, OnPressUp- and/or OnPressDown-Event to the specified FSM, optionally set the value of an existing GameObject-Variable in that FSM to the GameObject this script is attached to, beforehand (to make checks where the signal came from, or use that one further, in the Target-FSM).
The second image shows what the Target-FSM needs: The specified global Event(s) with matching name(s) and optionally the pre-existing variable, also with exact matching names.
The GameObject this component is attached to also need a collider of any kind to register the click happening.

Very useful to save a lot of FSM-Components (since they can add ~0.05s to loading times per FSM), 
especially when you have a lot of similar GameObjects or instantiate a lot of prefabs (I cut down aprox. 5000 FSM with this quite light-weight script) 
since with this you don't need an FSM for every button but can have one global FSM for all children.

## SendEventToFSMAdvanced: 
![alt text](http://imgur.com/r6X0nlE)
```
Requires the `SendEventToFSMAdvancedInspector`-Script in the Editor-Folder
```
An extended version of `SendEventToFSM` with the following additions:
1. You can specify the FSM-Name if several FSM's coexist on the Target-FSM;
2. You can choose any variable-type the selected variable in the Target-FSM has (except Quaternions, Arrays and Enums);
3. You can optionally set a PlayerPrefs to retrieve that information in the Target-FSM (Expands the field for the PlayerPrefs-Value once you type in a Key);

Not as light-weight as the first version, so best to use it more scarcely.