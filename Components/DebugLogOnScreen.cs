
using UnityEngine;
using System.Collections.Generic;

public class DebugLogOnScreen : MonoBehaviour
{
	List<string> log = new List<string>();
	string myLog;
	string tmp;
	int lines = 0;

	void Start()
	{
	}

	void OnEnable()
	{
		Application.logMessageReceived += HandleLog;
	}

	void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;
	}

	void HandleLog(string logString, string stackTrace, LogType type)
	{
		myLog = logString;

		myLog = "<b>[" + type + "]</b>: " + myLog;
		switch(type)
		{
			case LogType.Log:
				myLog = "<color=black>" + myLog + "</color>";
				break;
			case LogType.Warning:
				myLog = "<color=orange>" + myLog + "</color>";
				break;
			case LogType.Error:
			case LogType.Assert:
			case LogType.Exception:
				myLog = "<color=maroon>" + myLog + "</color>";
				break;
		}

		myLog = "\n" + myLog;
		log.Add(myLog);

		if(type == LogType.Exception)
		{
			tmp = "\n <color=maroon>" + stackTrace + "</color>";
			log.Add(tmp);
		}

		lines = log.Count;

		if(log.Count > 30)
			log.RemoveAt(0);

		myLog = string.Empty;
		foreach(string mylog in log)
		{
			myLog += mylog;
		}
	}

	void OnGUI()
	{
		GUIStyle lblStyle = new GUIStyle();
		lblStyle.richText = true;

		GUILayout.Label(myLog, lblStyle);
	}
}

