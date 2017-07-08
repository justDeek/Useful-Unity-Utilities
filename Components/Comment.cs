using System.Collections;
using UnityEngine;

public class Comment : MonoBehaviour {

	//the main comment
	public string text;

	public bool displayOnScreen = false;
	public Color textColor = Color.cyan;
	public Vector2 offset;

	public bool debug = false;
	public bool removeAtRuntime = false;

	void OnGUI() {
		if (displayOnScreen)
		{
			GUI.color = textColor;
			Vector2 ownerPos = Camera.main.WorldToScreenPoint(transform.position);
			ownerPos.y = Screen.height - ownerPos.y;
			var textSize = GUI.skin.label.CalcSize(new GUIContent(text));
			GUI.Label(new Rect(ownerPos.x + offset.x, ownerPos.y - offset.y, textSize.x, textSize.y), text);
		}
	}

	void Start ()
	{
		if (debug) Debug.Log(text);
		if (removeAtRuntime) Destroy(this);
	}

}
