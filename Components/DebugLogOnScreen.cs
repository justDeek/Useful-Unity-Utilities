using UnityEngine;
using System.Collections.Generic;

public class DebugLogOnScreen : MonoBehaviour
{
	public bool clear, test;

	List<string> log = new List<string>();
	string currLine;
	string tmp;

	void Start()
	{
		if(test)
		{
			Debug.Log("Test - Log");
			Debug.LogWarning("Test LogWarning");
			Debug.LogError("Test LogError");
		}
	}

	void OnEnable()
	{
		//subscribe to Unity's debug log
		Application.logMessageReceived += HandleLog;
	}

	void OnDisable()
	{
		//unsubscribe from Unity's debug log
		Application.logMessageReceived -= HandleLog;
	}

	void HandleLog(string logString, string stackTrace, LogType type)
	{
		currLine = logString;

		currLine = "<b>[" + type + "]</b>: " + currLine;

		//add rich-text tags (colorize labels)
		switch(type)
		{
			case LogType.Log:
				currLine = "<color=black>" + currLine + "</color>";
				break;
			case LogType.Warning:
				currLine = "<color=orange>" + currLine + "</color>";
				break;
			case LogType.Error:
			case LogType.Assert:
			case LogType.Exception:
				currLine = "<color=maroon>" + currLine + "</color>";
				break;
		}

		currLine = "\n" + currLine;
		log.Add(currLine);

		//add the whole stacktrace if it's an exception
		if(type == LogType.Exception)
		{
			tmp = "\n <color=maroon>" + stackTrace + "</color>";
			log.Add(tmp);
		}

		//clamp log-length (also adds a continuous scrolling effect)
		if(log.Count > 30) log.RemoveAt(0);

		currLine = string.Empty; //clear current log line
		foreach(string mylog in log) currLine += mylog;
	}

	void OnGUI()
	{
		GUIStyle lblStyle = new GUIStyle();
		lblStyle.richText = true;

		GUILayout.Label(currLine, lblStyle);

		if(clear)
		{
			currLine = string.Empty;
			log.Clear();
			clear = false;
		}
	}
}