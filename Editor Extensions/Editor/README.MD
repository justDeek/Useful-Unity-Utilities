## CurrentSelectionMenu.cs
![Image](http://i.imgur.com/gEAmnXT.jpg)
A custom Menu for all sorts of selection-based actions with shortcuts:

- Clone selected GameObject: Duplicates the currently selected GameObjects without adding the any incrementation like (1), (2), ... and resets position, rotation and scale of the duplicates
- Undo Cloning: Removes the last cloned GOs
- Rename selected GameObjects incrementally: Adds an increment from 01, 02, ... to all selected GOs
- Select Parent: Jumps to the parent of the currently selected GO
- Parent: Select all Children: Selects all children of the currently selected GO
- Parent: Sort Children by Name: Sorts all children of the currently selected GO alphabetically

## CustomAssetImporter.cs
![Image](http://i.imgur.com/gEAmnXT.jpg)

Based on [Saper Soher's Custom Asset Importer](http://www.sarpersoher.com/a-custom-asset-importer-for-unity/) Script with added options and comments.

Customize the Unity Import-Settings to your hearts content. Any option you see in the import settings (selecting a file / asset and see the inspector) can be modified herewith.

## EasySpriteSheetCopy.cs
![Image](http://i.imgur.com/gEAmnXT.jpg)

The free, wonderful and very helpful script from the [Asset Store](https://www.assetstore.unity3d.com/en/#!/content/27435).

Can copy plactically all values and parameters from one SpriteSheet to the next, especially handy if you have several similar ones and don't want to set the pivot of all of them manually for example.

## UnityHook.cs
![Image](http://i.imgur.com/gEAmnXT.jpg)

Fixes the Unity bug of not converting floats correctly sometimes 
(Example: when you duplicate a GameObject with Ctrl+D, the duplicate likely has more decimal places / fractional digits in its transform)