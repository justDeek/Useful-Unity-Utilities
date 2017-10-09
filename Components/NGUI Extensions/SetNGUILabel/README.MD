## SetNGUILabel.cs
![Image](/Screenshots/Components/SetNGUILabel_Info.png)

Changes the Text of the NGUI UILabel to the specified input.
The main purpose of it is to retrieve any runtime value and set the text of the Label accordingly (mainly for translation purposes or for frequently changing values)
Options are:
- Raw String: Just type in a normal string to display it in the Label
- PlayerPrefs: Put in the value of any PlayerPrefs String (checks if the PlayerPrefs exists and if it contains a string value)
- PlayMaker Globals: Use the value of any PlayMaker Global (checks if the Global exists and if it is of type 'String')
- Game Data Editor: Searches for the given Item- and Field-Name and if it returns a result
  (click on the "Load GDE" button to visualize the result of your input; might take half a second to load, that's why it's optional)
  You can also use a PlayMaker Global for the Field-Name in case you want to use a variable that changes at runtime
 ``` diff
- currently has some default values for my project in the Reset() function (remove all lines directly under ""LMAYAQi" Project-specific defaults")
 ```


ToDo: Don't apply the text if it the input is not a string (or convert it to it)
		  Remove PlayMaker and GDE Options if one or both are not installed
		  Maybe add more options if some come to mind