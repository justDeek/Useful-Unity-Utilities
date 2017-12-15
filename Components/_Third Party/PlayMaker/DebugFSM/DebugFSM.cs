﻿
using UnityEngine;
using System.Collections.Generic;
using HutongGames.PlayMaker;

[ExecuteInEditMode]
public class DebugFSM : MonoBehaviour
{
	[UnityEngine.Tooltip("The FSM that should get tracked.")]
	public PlayMakerFSM targetFSM;
	[UnityEngine.Tooltip("The size of the displayed text.")]
	public int fontSize = 15;
	[UnityEngine.Tooltip("Determines if the GUI should scale depending on the current device-resolution or not.")]
	public bool resolutionIndependent = false;
	[UnityEngine.Tooltip("What color the text should be displayed in. The alpha determines both the color- and shadow-opacity.")]
	public Color32 fontColor = new Color32(124, 72, 72, 255);
	[UnityEngine.Tooltip("Where the text should be displayed at. If you want the text to be on the lower end, apply a negivate Y in Label Position to not let the text get cropped.")]
	public TextAnchor alignment = TextAnchor.UpperLeft;
	[UnityEngine.Tooltip("Apply an offset to the default label position.")]
	public Vector2 labelPosition = Vector2.zero;

	private GUIStyle _guiStyle = new GUIStyle();
	private GUIStyle _guiStyle2 = new GUIStyle();
	private string currentStateName = "";
	private List<string> previousStateNames = new List<string>();
	private Rect labelRect;
	private NamedVariable[] allVariables;
	private Color32 shadowColor = Color.black;

	//for custom inspector
	[HideInInspector] public bool debugStateNames = true;
	[HideInInspector] public bool debugVariables = false;
	[HideInInspector] public int traceBackAmount = 3;
	[HideInInspector] public int startFrom = 0;

	public void Start()
	{
		//use the owner FSM component if nothing specified or this got attached
		if(targetFSM == null)
		{
			if(this.gameObject.GetComponent<PlayMakerFSM>())
			{
				targetFSM = gameObject.GetComponent<PlayMakerFSM>();
			}
		}
	}

	public void OnGUI()
	{
		if(resolutionIndependent)
		{
			//scale UI by resolution
			float rx = 1 + (float)Screen.width / (float)Screen.currentResolution.width;
			GUI.matrix = Matrix4x4.TRS(new Vector3(rx, rx, 0), Quaternion.identity, new Vector3(rx, rx, 1));
		}

		_guiStyle.fontSize = fontSize;
		_guiStyle.normal.textColor = fontColor;
		_guiStyle.alignment = alignment;

		_guiStyle2.fontSize = fontSize;
		_guiStyle2.normal.textColor = fontColor;
		_guiStyle2.alignment = alignment;
		_guiStyle2.fontStyle = FontStyle.Bold;

		shadowColor.a = fontColor.a;

		//base rect for label position and size
		labelRect = new Rect(labelPosition.x, labelPosition.y, Screen.width, Screen.height);

		GUIContent content = new GUIContent();

		content.text = "";

		//return if no FSM component has been selected
		if(targetFSM == null || targetFSM.gameObject == null)
		{
			GUI.Label(labelRect, "No FSM component specified on" + ": "
								 + this.gameObject.GetGameObjectPath(true), _guiStyle);
			DrawShadow(labelRect, content, _guiStyle, fontColor, shadowColor, new Vector2(0, 1));
			return;
		}

		content.text = targetFSM.gameObject.name + ": " + targetFSM.FsmName;

		//display name of the GameObject the FSM component is attached to and the FSM name
		GUI.Label(labelRect, content, _guiStyle);
		DrawShadow(labelRect, content, _guiStyle, fontColor, shadowColor, new Vector2(0, 1));

		///State names
		if(debugStateNames)
		{
			labelRect = new Rect(labelRect.x, labelPosition.y + (fontSize + fontSize / 2),
								 labelRect.width, labelRect.height);
			content.text = "Current State: " + currentStateName;

			//display current state name
			GUI.Label(labelRect, content, _guiStyle2);
			DrawShadow(labelRect, content, _guiStyle2, fontColor, shadowColor, new Vector2(0, 1));

			//revert list
			previousStateNames.Reverse();

			//iterate through previous state names
			for(int i = 1; i < previousStateNames.Count; i++)
			{
				if(i > traceBackAmount)
				{
					continue;
				}

				labelRect = new Rect(labelRect.x, labelPosition.y + (fontSize * (i + 1.5f) + (fontSize / 2)),
									 labelRect.width, labelRect.height);
				content.text = "Previous State " + i + ": "
									 + previousStateNames[previousStateNames.Count - (i + 1)];

				GUI.Label(labelRect, content, _guiStyle);
				DrawShadow(labelRect, content, _guiStyle, fontColor, shadowColor, new Vector2(0, 1));
			}
		}
		///Variables
		else if(debugVariables && allVariables != null)
		{
			var i = 0;
			var skipped = 0;
			//iterate through variables
			foreach(var currentVariable in allVariables)
			{
				i++;
				if(i < startFrom)
				{
					skipped++;
					continue;
				}
				try
				{
					labelRect = new Rect(labelRect.x, labelPosition.y + (fontSize * (i - skipped) + (fontSize / 2)),
									 labelRect.width, labelRect.height);
					content.text = "Variable " + i + " - \"" + currentVariable.Name
										 + "\" (" + currentVariable.VariableType.ToString()
										 + ")" + ": " + currentVariable.RawValue.ToString();

					GUI.Label(labelRect, content, _guiStyle);
					DrawShadow(labelRect, content, _guiStyle, fontColor, shadowColor, new Vector2(0, 1));
				} catch(System.Exception)
				{

				}

			}
		}
	}

	public void Update()
	{
		if(targetFSM == null)
		{
			return;
		}

		///State names
		currentStateName = targetFSM.Fsm.ActiveStateName;

		//add first state to the list if the list is empty
		if(previousStateNames.IsEmpty() && !currentStateName.IsNullOrEmpty())
		{
			previousStateNames.Add(currentStateName);
		}

		//only add active state to list if the state changed
		if(previousStateNames.GetLastItem() != currentStateName)
		{
			previousStateNames.Add(currentStateName);
		}

		///Variables
		allVariables = targetFSM.Fsm.Variables.GetAllNamedVariables();
	}

	//out of "ShadowAndOutline" by Bérenger from the UnifyCommunity (http://wiki.unity3d.com/index.php/ShadowAndOutline)
	public void DrawShadow(Rect rect, GUIContent content, GUIStyle style, Color txtColor, Color shadowColor,
									Vector2 direction)
	{
		GUIStyle backupStyle = style;

		style.normal.textColor = shadowColor;
		rect.x += direction.x;
		rect.y += direction.y;
		GUI.Label(rect, content, style);

		style.normal.textColor = txtColor;
		rect.x -= direction.x;
		rect.y -= direction.y;
		GUI.Label(rect, content, style);

		style = backupStyle;
	}
}
