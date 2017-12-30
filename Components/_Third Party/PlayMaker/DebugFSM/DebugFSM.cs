
using UnityEngine;
using System.Collections.Generic;
using HutongGames.PlayMaker;

[ExecuteInEditMode]
public class DebugFSM : MonoBehaviour
{
	[Header("Setup")]
	[UnityEngine.Tooltip("The FSM that should get tracked.")]
	public PlayMakerFSM targetFSM;
	[UnityEngine.Tooltip("The size of the displayed text.")]
	public int fontSize = 14;
	[UnityEngine.Tooltip("What color the text should be displayed in. The alpha determines both the color- and shadow-opacity.")]
	public Color32 fontColor = new Color32(124, 72, 72, 255);
	[UnityEngine.Tooltip("Where the text should be displayed at. If you want the text to be on the lower end, apply a negivate Y in Label Position to not let the text get cropped.")]
	public TextAnchor alignment = TextAnchor.UpperLeft;
	[UnityEngine.Tooltip("Apply an offset to the default label position.")]
	public Vector2 labelPosition = Vector2.zero;
	[Header("Options")]
	[UnityEngine.Tooltip("If the time between the previous and current state should be calculated independent of the current TimeScale of the game or not.")]
	public bool timerInRealTime = true;
	[UnityEngine.Tooltip("Determines if the GUI should scale depending on the current device-resolution or not.")]
	public bool resolutionIndependent = false;
	[UnityEngine.Tooltip("Show shadows on each label to highlight them (gets automatically disabled on mobile, since the increase in redraws makes it lag).")]
	public bool showShadows = true;

	private GUIStyle _guiStyle = new GUIStyle();
	private GUIStyle _guiStyle2 = new GUIStyle();
	private string currStateName = "";
	private string prevStateName = "";
	private List<string> previousStateNames = new List<string>();
	private List<float> previousStateTimers = new List<float>();
	private Rect labelRect;
	private NamedVariable[] allVariables;
	private Color32 shadowColor = Color.black;
	private float startTime = 0;
	private float timer = 0;

	//for custom inspector
	[HideInInspector] public bool debugStateNames = true;
	[HideInInspector] public bool debugVariables = false;
	[HideInInspector] public int traceBackAmount = 3;
	[HideInInspector] public int startFrom = 0;

	public void Awake()
	{
		startTime = FsmTime.RealtimeSinceStartup;

		//use the owner FSM component if nothing specified or this got attached
		if(targetFSM == null)
		{
			if(this.gameObject.GetComponent<PlayMakerFSM>())
			{
				targetFSM = gameObject.GetComponent<PlayMakerFSM>();
			}
		}

		if(targetFSM && targetFSM.Fsm != null)
		{
			currStateName = targetFSM.Fsm.ActiveStateName;
			AddToPrevStates(currStateName);
		}
	}

	public void OnGUI()
	{
		//disable shadows if not on pc to prevent lags due to too many redraws
#if !UNITY_STANDALONE
		showShadows = false;
#endif

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
			content.text = "Current State: " + targetFSM.Fsm.ActiveStateName
						 + " (" + timer.ToString("n2") + "s)";

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
				content.text = "Prev. State #" + i + ": "
									 + previousStateNames[previousStateNames.Count - (i + 1)]
									 + " (" + previousStateTimers[previousStateTimers.Count - i].ToString("n2") + "s)";

				GUI.Label(labelRect, content, _guiStyle);
				DrawShadow(labelRect, content, _guiStyle, fontColor, shadowColor, new Vector2(0, 1));
			}
		}
		///Variables
		else if(debugVariables && allVariables != null)
		{
			int skipped = 0;
			//iterate through variables
			for(int i = 1; i < allVariables.Length; i++)
			{
				//skip if startFrom is bigger than current index
				if(i < startFrom)
				{
					skipped++;
					continue;
				}

				//if startFrom is negative, clamp variables from behind
				if(startFrom < 0)
				{
					int endAt = allVariables.Length + startFrom;
					if(i > endAt)
					{
						skipped++;
						return;
					}
				}

				try
				{
					labelRect = new Rect(labelRect.x, labelPosition.y + (fontSize * (i - skipped) + (fontSize / 2)),
									 labelRect.width, labelRect.height);
					content.text = "Variable #" + i + " - \"" + allVariables[i].Name
										 + "\" (" + allVariables[i].VariableType.ToString()
										 + ")" + ": " + allVariables[i].RawValue.ToString();

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
		if(!timerInRealTime)
		{
			UpdateValues();
		}
	}

	public void FixedUpdate()
	{
		if(timerInRealTime)
		{
			UpdateValues();
		}
	}

	void UpdateValues()
	{
		if(!targetFSM || targetFSM.Fsm == null)
		{
			return;
		}

		///Variables
		if(debugVariables)
		{
			allVariables = targetFSM.Fsm.Variables.GetAllNamedVariables();
			return;
		}

		///State Names
		//get timespan between previous and current state
		if(timerInRealTime)
		{
			timer += Time.unscaledDeltaTime;
		} else
		{
			timer += Time.deltaTime;
		}

		currStateName = targetFSM.Fsm.ActiveStateName;
		if(targetFSM.Fsm.PreviousActiveState != null)
		{
			prevStateName = targetFSM.Fsm.PreviousActiveState.Name;
		}

		if(previousStateNames.Count > 1 && !string.IsNullOrEmpty(prevStateName))
		{
			if(previousStateNames[previousStateNames.Count - 2] != prevStateName)
			{
				AddToPrevStates(prevStateName);
			}
		}

		AddToPrevStates(currStateName);
	}

	void AddToPrevStates(string stateName)
	{
		//skip if given statename is empty
		if(string.IsNullOrEmpty(stateName))
		{
			return;
		}

		if(previousStateNames.Count != 0)
		{
			//check if last item already equals given state
			if(previousStateNames[previousStateNames.Count - 1] == stateName)
			{
				return;
			}
		}

		//add given entry to list
		previousStateNames.Add(stateName);
		previousStateTimers.Add(timer);
		timer = 0f;
	}

	//snippet of "ShadowAndOutline" by Bérenger from the UnifyCommunity (http://wiki.unity3d.com/index.php/ShadowAndOutline)
	private void DrawShadow(Rect rect, GUIContent content, GUIStyle style, Color txtColor, Color shadowColor,
									Vector2 direction)
	{
		if(!showShadows)
		{
			return;
		}

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
