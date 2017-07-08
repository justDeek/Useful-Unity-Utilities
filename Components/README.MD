### DontDestroyDeleteDuplicates: 

![alt text](http://i.imgur.com/FqJX5va.jpg)

```diff
- Requires you to add the Tags 'Persistent' and 'RemoveAtRuntime' to Unity. 
```
Add this script to any GameObject that is supposed to stay persistent through all scenes and tag that GO as 'Persistent'.
The advantage of this script is that it removes any duplicate that Unity creates when going back to a previously visited scene.
With the 'RemoveAtRuntime' Tag you can have tagged GameObjects be destroyed when a new scene has been loaded. Useful if you have a similar 
GOs in several scenes for testing purposes but only want one copy to be persistent at the beginning, wich then destroys all those testing-copies.

