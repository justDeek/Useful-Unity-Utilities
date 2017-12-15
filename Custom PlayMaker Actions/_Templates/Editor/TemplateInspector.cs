using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomActionEditor(typeof(Template))]
public class TemplateInspector : CustomActionEditor
{
	//store references to the target script
	private static Template action;
	private PlayMakerFSM thisFSM;

	//set this to true on every change to only update OnGUI when something has differed from the last call
	private bool isDirty = false;

	//gets called once when the target script has been enabled
	public override void OnEnable()
	{

	}

	//gets called when interacting with the action window or if something has changed
	public override bool OnGUI()
	{
		///Get action and FSM reference
		action = target as Template;

		if(action.Fsm == null || action.Fsm.FsmComponent == null)
		{
			return false;
		}
		thisFSM = action.Fsm.FsmComponent;

		//optionally draw the default inspector (action-content) if you only want to add to it
		base.DrawDefaultInspector();

		//-- Your main code goes in here --//

		//needs to be at the end and tells OnGUI if something has changed
		return isDirty || GUI.changed;
	}

}
